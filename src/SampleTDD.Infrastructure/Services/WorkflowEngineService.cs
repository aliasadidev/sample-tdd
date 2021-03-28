using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using SampleTDD.Core.Collections.StateMachine;
using SampleTDD.Core.Constants;
using SampleTDD.Core.Contracts.Repositories;
using SampleTDD.Core.Contracts.Services;
using SampleTDD.Core.DTOs.Security;
using SampleTDD.Core.Modules;

namespace SampleTDD.Infrastructure.Services
{
	public class WorkflowEngineService : IWorkflowEngineService
	{
		private readonly IMongoBPStateRepository _bpStateRepo;
		private readonly IMongoChangeStateRuleRepository _bpChangeStateRuleRepo;
		private readonly IMongoBPRepository _mongoBPRepository;

		public WorkflowEngineService(
				IMongoBPStateRepository mongoBPStateRepository,
				IMongoChangeStateRuleRepository mongoChangeStateRuleRepository,
				IMongoBPRepository mongoBPRepository
				)
		{
			_bpStateRepo = mongoBPStateRepository;
			_bpChangeStateRuleRepo = mongoChangeStateRuleRepository;
			_mongoBPRepository = mongoBPRepository;
		}

		private IEnumerable<ChangeStateRule> getNextBpStates(IClientSessionHandle session, ObjectId bpID, StateTypes currentState, RoleTypes role, OperationTypes operationType)
		{
			ChangeStateRule[] currentRoleNextStates = _bpChangeStateRuleRepo.GetNextStep(session, bpID, currentState, role, operationType).ToArray();



			if (!currentRoleNextStates.Any() || currentRoleNextStates.Any(x => x.NextState == 0))
				throw new CustomException("the next state not found", StatusCodeTypes.UndefineStateMachineTransmission.ToInt());

			if (operationType == OperationTypes.Reject && currentRoleNextStates.Any(x => x.NextState == StateTypes.Start))
			{
				var en = _bpStateRepo.GetFirstState(session, bpID);
				if (en != null)
					for (int i = 0; i < currentRoleNextStates.Length; i++)
					{
						if (currentRoleNextStates[i].NextState == StateTypes.Start)
							currentRoleNextStates[i].RoleID = en.RoleID;
					}
			}
			return currentRoleNextStates.ToList();
		}

		public virtual void Approve(IClientSessionHandle session, ObjectId bpID, RoleTypes role, long userID)
		{
			var currentState = getCurrentBpState(session, bpID, role);
			var nextStates = getNextBpStates(session, bpID, currentState, role, OperationTypes.Approve);

			// one to many
			if (nextStates.Count() > 1)
			{
				foreach (var nextState in nextStates)
				{
					RoleTypes nextRoleID;
					bool isComplete = false;
					if ((nextState.NextState == StateTypes.FinishedSuccessfully || nextState.NextState == StateTypes.FinishedUnsuccessfully))
					{
						nextRoleID = role;
						isComplete = true;
					}
					else
					{
						nextRoleID = _bpChangeStateRuleRepo.GetRoleByCurrentState(session, nextState.NextState);
					}
					bool isCompleted = _bpStateRepo.CurrentStateIsCompleted(session, nextState.NextState, bpID, nextRoleID);
					bool inProccess = _bpStateRepo.CurrentStateInProccess(session, nextState.NextState, bpID, nextRoleID);
					if (isCompleted == false && inProccess == false)
						changeBPState(session, bpID, nextState.NextState, nextRoleID, isComplete, userID);
				}
			}
			else // many to one / one to one
			{
				var nextState = nextStates.First();
				bool hasPreState;

				if (_bpChangeStateRuleRepo.IsOneToOneState(nextState.NextState))
				{
					hasPreState = true;
				}
				else
				{
					var preStates = _bpStateRepo.GetPreviousStates(session, nextState.NextState, bpID, currentState);
					hasPreState = preStates.Count() < 1;
				}
				RoleTypes nextRoleID;
				bool isComplete = false;
				if ((nextState.NextState == StateTypes.FinishedSuccessfully || nextState.NextState == StateTypes.FinishedUnsuccessfully))
				{
					nextRoleID = role;
					isComplete = true;
				}
				else
				{
					nextRoleID = _bpChangeStateRuleRepo.GetRoleByCurrentState(session, nextState.NextState);
				}

				bool inProccess = _bpStateRepo.CurrentStateInProccess(session, nextState.NextState, bpID, nextRoleID);
				bool isCompleted = _bpStateRepo.CurrentStateIsCompleted(session, nextState.NextState, bpID, nextRoleID);

				if (hasPreState && isCompleted == false && inProccess == false)
					changeBPState(session, bpID, nextState.NextState, nextRoleID, isComplete, userID);

			}

			_bpStateRepo.CompleteBPState(session, bpID, currentState, userID);

		}

		private ChangeStateRule getSingleNextBPState(StateTypes currentState, RoleTypes roleID, OperationTypes operationType)
		{
			IEnumerable<ChangeStateRule> nextStates = _bpChangeStateRuleRepo.GetNextStep(currentState, operationType);

			var currentRoleNextStates = (from role in new RoleTypes[] { roleID }
										 join state in nextStates on role equals state.RoleID
										 select new ChangeStateRule { NextState = state.NextState, RoleID = role }).ToList();

			var _nextState = currentRoleNextStates.FirstOrDefault();
			if (_nextState == null || _nextState.NextState == 0)
				throw new CustomException("the next state not found", StatusCodeTypes.UndefineStateMachineTransmission.ToInt());

			return new ChangeStateRule()
			{
				CurrentState = currentState,
				NextState = _nextState.NextState,
				OperatinID = operationType,
				RoleID = _nextState.RoleID
			};
		}



		public virtual void Start(IClientSessionHandle session, ObjectId bpID, RoleTypes roleID, long userID)
		{
			var state = getSingleNextBPState(StateTypes.Start, roleID, OperationTypes.Start);
			changeBPState(session, bpID, state.CurrentState, state.RoleID, false, userID, false);
		}

		private ObjectId changeBPState(IClientSessionHandle session, ObjectId bpID, StateTypes state, RoleTypes role, bool isCompleted, long? userID, bool setDetailField = false)
		{
			var bps = new BPState()
			{
				BPID = bpID,
				StateID = state,
				CreationDate = DateTime.Now,
				IsDeleted = false,
				RoleID = role,
				IsCompleted = isCompleted,
			};
			if (isCompleted || setDetailField)
			{
				bps.UserID = userID;
				if (isCompleted)
					bps.CompletedDateTime = DateTime.Now;
			}

			_bpStateRepo.Add(session, bps);

			return bps._id;
		}


		public PermissionDTO GetPremissions(ObjectId bpID, RoleTypes roleID, long userID)
		{
			var currentState = GetCurrentBpState(bpID, roleID);

			var operation = _bpChangeStateRuleRepo.GetCurrentOperations(roleID, currentState);


			if (currentState == StateTypes.Start)
			{
				var isOwner = _bpStateRepo.IsBPOwner(bpID, userID);
				operation.CanApprove = operation.CanApprove && isOwner;
			}

			return operation;
		}
		public StateTypes GetCurrentBpState(ObjectId bpID, RoleTypes roleID)
		{
			var activeStates = _bpStateRepo.GetCurrentBPState(bpID);
			StateTypes activeState;

			if (activeStates.Any(x => x.RoleID == roleID))
				activeState = activeStates.Where(x => x.RoleID == roleID).First().StateID;
			else
				activeState = activeStates.OrderByDescending(x => x.CreationDate).First().StateID;

			return activeState;
		}
		private StateTypes getCurrentBpState(IClientSessionHandle session, ObjectId bpID, RoleTypes roleID)
		{
			var activeStates = _bpStateRepo.GetCurrentBPState(session, bpID);
			StateTypes activeState;

			if (activeStates.Any(x => x.RoleID == roleID))
				activeState = activeStates.Where(x => x.RoleID == roleID).First().StateID;
			else
				activeState = activeStates.OrderByDescending(x => x.CreationDate).First().StateID;

			return activeState;
		}
	}
}
