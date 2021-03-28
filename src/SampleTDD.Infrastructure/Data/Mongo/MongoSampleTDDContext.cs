using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SampleTDD.Core.Collections;
using SampleTDD.Core.Collections.StateMachine;
using SampleTDD.Core.Contracts.Repositories;
using SampleTDD.Core.DTOs.Settings;

namespace SampleTDD.Infrastructure.Data.Mongo
{
	public class MongoSampleTDDContext : IMongoSampleTDDContext
	{
		public virtual IMongoClient Client { get; }
		public virtual IMongoDatabase Database { get; }
		public MongoSampleTDDContext(IOptions<AppSettings> options)
		{
			Client = new MongoClient(options.Value.MongoDB.ConnectionString);
			Database = Client.GetDatabase(options.Value.MongoDB.Database);
		}
		public IMongoCollection<BP> BPs => Set<BP>(nameof(BP));
		public IMongoCollection<BPState> BPStates => Set<BPState>(nameof(BPState));
		public IMongoCollection<ChangeStateRule> ChangeStateRules => Set<ChangeStateRule>(nameof(ChangeStateRule));
		public IMongoCollection<State> States => Set<State>(nameof(State));
		public IMongoCollection<TCollection> Set<TCollection>() where TCollection : BaseCollection
		{
			return Database.GetCollection<TCollection>(typeof(TCollection).Name).WithWriteConcern(WriteConcern.WMajority);
		}

		public IMongoCollection<TCollection> Set<TCollection>(string name) where TCollection : BaseCollection
		{
			return Database.GetCollection<TCollection>(name).WithWriteConcern(WriteConcern.WMajority);
		}
	}
}
