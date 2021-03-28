using MongoDB.Bson;
using SampleTDD.Core.Collections.StateMachine;
using SampleTDD.Core.Constants;
using System.Collections.Generic;
using MongoDB.Driver;

namespace SampleTDD.Core.Contracts.Services
{
	public interface IConditionStateService
	{
		IEnumerable<ChangeStateRule> ApplyCondition(IClientSessionHandle session, RoleTypes RoleID, ObjectId bpID, OperationTypes operationTypes, StateTypes stateTypes, IEnumerable<ChangeStateRule> changeStateRules);
		bool IsConditionState(StateTypes stateType, OperationTypes operation);
	}
}
