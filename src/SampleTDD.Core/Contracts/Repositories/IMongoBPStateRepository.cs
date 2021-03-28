using MongoDB.Bson;
using SampleTDD.Core.Collections.StateMachine;
using SampleTDD.Core.Constants;
using System.Collections.Generic;
using MongoDB.Driver;

namespace SampleTDD.Core.Contracts.Repositories
{
	public interface IMongoBPStateRepository : IMongoRepositoryBase<BPState>
	{
		IEnumerable<BPState> GetCurrentBPState(ObjectId bpID);
		BPState GetFirstState(IClientSessionHandle session, ObjectId bpID);
		void CompleteBPState(IClientSessionHandle session, ObjectId bpID, StateTypes currentState, long? userID);
		IEnumerable<string> GetPreviousStates(IClientSessionHandle session, StateTypes nextState, ObjectId bpID, StateTypes currentState);
		bool CurrentStateIsCompleted(IClientSessionHandle session, StateTypes state, ObjectId bpID, RoleTypes role);
		bool IsBPOwner(ObjectId bpID, long userID);
		bool CurrentStateInProccess(IClientSessionHandle session, StateTypes nextState, ObjectId bpID, RoleTypes nextRoleID);
		IEnumerable<BPState> GetCurrentBPState(IClientSessionHandle session, ObjectId bpID);
		StateTypes GetRoleStateView(ObjectId bpID, RoleTypes role, StateTypes stateType);
	}
}
