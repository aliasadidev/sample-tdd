using MongoDB.Driver;
using SampleTDD.Core.Collections;
using SampleTDD.Core.Collections.StateMachine;

namespace SampleTDD.Core.Contracts.Repositories
{
	public interface IMongoSampleTDDContext
	{
		IMongoCollection<TCollection> Set<TCollection>() where TCollection : BaseCollection;
		IMongoCollection<TCollection> Set<TCollection>(string name) where TCollection : BaseCollection;

		IMongoCollection<BP> BPs { get; }
		IMongoCollection<BPState> BPStates { get; }
		IMongoCollection<ChangeStateRule> ChangeStateRules { get; }
		IMongoCollection<State> States { get; }

		IMongoDatabase Database { get; }
		IMongoClient Client { get; }
	}
}
