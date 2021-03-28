using MongoDB.Bson;
using MongoDB.Driver;
using SampleTDD.Core.Collections.StateMachine;
using SampleTDD.Core.Constants;
using SampleTDD.Core.Contracts.Repositories;
using SampleTDD.Core.DTOs.Security;
using System.Collections.Generic;
using System.Linq;

namespace SampleTDD.Infrastructure.Data.Mongo.Repositories
{
	public class MongoChangeStateRuleRepository : MongoRepositoryBase<ChangeStateRule>, IMongoChangeStateRuleRepository
	{
		public MongoChangeStateRuleRepository(IMongoSampleTDDContext mongoSampleTDDContext) : base(mongoSampleTDDContext) { }

		public IEnumerable<ChangeStateRule> GetNextStep(StateTypes currentState, OperationTypes operationType)
		{
			var nextStates = _db.ChangeStateRules.Find(x => x.CurrentState == currentState && x.OperatinID == operationType).ToList();
			return nextStates;
		}

		public PermissionDTO GetCurrentOperations(RoleTypes roleID, StateTypes currentBPState)
		{

			var list = _currentCollec.Find(x => x.RoleID == roleID && x.CurrentState == currentBPState).Project(x => x.OperatinID).ToList();
			var per = new PermissionDTO
			{
				CanApprove = list.Any(x => x == OperationTypes.Approve),
				CanReject = list.Any(x => x == OperationTypes.Reject),
				CanClose = list.Any(x => x == OperationTypes.Close),
				CanFinalize = list.Any(x => x == OperationTypes.Finalize)
			};
			return per;
		}

		public IEnumerable<ChangeStateRule> GetNextStep(IClientSessionHandle session, ObjectId bpID, StateTypes currentState, RoleTypes role, OperationTypes operationType)
		{
			var q = _db.BPStates.Find(session, e => e.BPID == bpID && e.IsCompleted == false && e.IsDeleted == false).Project(x => x.StateID).ToList();
			var query = from csr in _db.ChangeStateRules.AsQueryable()
						where csr.OperatinID == operationType && csr.IsDeleted == false
						where csr.CurrentState == currentState && q.Contains(csr.CurrentState)

						select new { csr.OperatinID, csr.NextState, csr.CurrentState, csr.RoleID };

			var currentRoleNextStates = (from roleID in new RoleTypes[] { role }
										 join state in query on roleID equals state.RoleID
										 select new ChangeStateRule { NextState = state.NextState, RoleID = roleID, CurrentState = state.CurrentState });


			var list = currentRoleNextStates.ToList()
											.Select(x => new ChangeStateRule
											{
												RoleID = x.RoleID,
												NextState = x.NextState,
												CurrentState = x.CurrentState,
												OperatinID = x.OperatinID
											});

			return list;

		}

		public RoleTypes GetRoleByCurrentState(IClientSessionHandle session, StateTypes nextState)
		{
			var one = _currentCollec.Find(session, x => x.CurrentState == nextState).First();
			return one.RoleID;
		}

		public bool IsOneToOneState(StateTypes nextState)
		{
			return _currentCollec.AsQueryable().Where(d => d.NextState == nextState && d.IsDeleted == false).Count() == 1;
		}
	}
}
