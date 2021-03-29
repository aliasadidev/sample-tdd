using MongoDB.Driver;
using System;
using SampleTDD.Core.Collections.StateMachine;
using SampleTDD.Core.Collections;
using SampleTDD.Core.Contracts.Repositories;
using Mongo2Go;

namespace SampleTDD.UnitTest.Modules
{
	public class MongoSampleTDDContextTest : IDisposable, IMongoSampleTDDContext
	{
		private MongoDbRunner _runner;
		public IMongoClient Client { get; private set; }
		public IMongoDatabase Database { get; private set; }
		public IMongoCollection<BP> BPs { get; private set; }

		public IMongoCollection<BPState> BPStates { get; private set; }

		public IMongoCollection<ChangeStateRule> ChangeStateRules { get; private set; }

		public IMongoCollection<Core.Collections.StateMachine.State> States { get; private set; }



		public MongoSampleTDDContextTest()
		{
			_runner = MongoDbRunner.Start(singleNodeReplSet: true);
			Client = new MongoClient(_runner.ConnectionString);
			Database = Client.GetDatabase("db");
		}

		public void Dispose()
		{
			_runner?.Dispose();
			_runner = null;
		}

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