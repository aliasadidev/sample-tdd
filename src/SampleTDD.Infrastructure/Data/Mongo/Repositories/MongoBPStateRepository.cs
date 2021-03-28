using MongoDB.Bson;
using MongoDB.Driver;
using SampleTDD.Core.Collections.StateMachine;
using SampleTDD.Core.Constants;
using SampleTDD.Core.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Linq;

namespace SampleTDD.Infrastructure.Data.Mongo.Repositories
{
	public class MongoBPStateRepository : MongoRepositoryBase<BPState>, IMongoBPStateRepository
	{
		public MongoBPStateRepository(IMongoSampleTDDContext mongoSampleTDDContext) : base(mongoSampleTDDContext) { }


		public StateTypes GetRoleStateView(ObjectId bpID, RoleTypes role, StateTypes stateType)
		{
			var last = _db.BPStates.Aggregate().Match(x => x.BPID == bpID && x.RoleID == role && x.IsDeleted == false).SortByDescending(z => z.CreationDate).First();
			return last.StateID;
		}

		public IEnumerable<BPState> GetCurrentBPState(ObjectId bpID)
		{
			IEnumerable<BPState> currentState = _currentCollec.Find(x => x.BPID == bpID && (x.IsCompleted == false) && x.IsDeleted == false)
				.SortByDescending(x => x.CreationDate)
				.ToList();
			return currentState;
		}

		public IEnumerable<BPState> GetCurrentBPState(IClientSessionHandle session, ObjectId bpID)
		{
			IEnumerable<BPState> currentState = _currentCollec.Find(session, x => x.BPID == bpID && (x.IsCompleted == false) && x.IsDeleted == false)
				.SortByDescending(x => x.CreationDate)
				.ToList();
			return currentState;
		}

		public bool IsBPOwner(ObjectId bpID, long userID)
		{
			var q = _db.BPs.Find(x => x._id == bpID && x.UserID == userID).Any();
			return q;

		}

		public BPState GetFirstState(IClientSessionHandle session, ObjectId bpID)
		{
			var firstState = _currentCollec.Find(session, x => x.BPID == bpID && x.IsDeleted == false).SortBy(x => x.CreationDate).FirstOrDefault();
			return firstState;
		}

		public void CompleteBPState(IClientSessionHandle session, ObjectId bpID, StateTypes currentState, long? userID)
		{
			var current = _currentCollec.Find(session, x => x.BPID == bpID &&
													   x.IsCompleted == false &&
													   x.IsDeleted == false &&
													   x.StateID == currentState).FirstOrDefault();
			if (current != null)
			{
				var fields = Builders<BPState>.Update.Set(x => x.IsCompleted, true)
													.Set(x => x.UserID, userID)
													.Set(x => x.CompletedDateTime, DateTime.Now);

				_currentCollec.UpdateOne(session, x => x.BPID == bpID && x.IsCompleted == false && x.IsDeleted == false && x.StateID == currentState, fields);
			}

		}

		public IEnumerable<string> GetPreviousStates(IClientSessionHandle session, StateTypes nextState, ObjectId bpID, StateTypes currentState)
		{
			var next = _db.States.Find(x => x.ID == nextState).First();
			var bpStates = _currentCollec.Find(session, e => e.BPID == bpID && e.IsCompleted == false && e.IsDeleted == false && e.StateID != currentState).Project(x => x.StateID).ToList();
			int count = next.HCode.Split('.').Count();
			var baseHCode = next.HCode.Split('.');

			var query =
					 from csr in _db.ChangeStateRules.AsQueryable()
					 where bpStates.Contains(csr.CurrentState)
					 join state in _db.States.AsQueryable() on csr.NextState equals state.ID
					 select state.HCode;




			var lastHCode = next.HCode.LastIndexOf('.');
			var fullHCode = (next.HCode.Substring(0, lastHCode + 1));


			var states = query.ToList();
			if (states.Any(z => z.Split('.').Count() == count))
			{
				states.RemoveAll(z => (z.Split('.').Count() == count && !z.StartsWith(fullHCode)));
			}

			return states;
		}

		public bool CurrentStateIsCompleted(IClientSessionHandle session, StateTypes state, ObjectId bpID, RoleTypes role)
		{
			var isCompleted = _currentCollec.Find(session, x => x.BPID == bpID && x.StateID == state && x.IsCompleted == true && x.RoleID == role && x.IsDeleted == false).Any();
			return isCompleted;
		}

		public bool CurrentStateInProccess(IClientSessionHandle session, StateTypes state, ObjectId bpID, RoleTypes role)
		{
			var isCompleted = _currentCollec.Find(session, x =>
										x.BPID == bpID &&
										x.StateID == state &&
										x.IsCompleted == false &&
										x.RoleID == role &&
										x.IsDeleted == false).Any();
			return isCompleted;
		}
	}
}
