using MongoDB.Bson;
using SampleTDD.Core.Collections.StateMachine;
using SampleTDD.Core.Constants;
using SampleTDD.Core.DTOs.Security;
using System.Collections.Generic;
using MongoDB.Driver;

namespace SampleTDD.Core.Contracts.Repositories
{
	public interface IMongoChangeStateRuleRepository : IMongoRepositoryBase<ChangeStateRule>
	{
		PermissionDTO GetCurrentOperations(RoleTypes roleID, StateTypes currentBPState);
		IEnumerable<ChangeStateRule> GetNextStep(StateTypes currentState, OperationTypes operationType);
		IEnumerable<ChangeStateRule> GetNextStep(IClientSessionHandle session, ObjectId bpID, StateTypes currentState, RoleTypes role, OperationTypes operationType);
		bool CanStart(RoleTypes roleID);
		RoleTypes GetRoleByCurrentState(IClientSessionHandle session, StateTypes nextState);
		bool IsOneToOneState(StateTypes nextState);
	}
}
