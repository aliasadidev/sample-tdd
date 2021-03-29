using AutoMapper;
using MongoDB.Driver;
using SampleTDD.Core.Collections;
using SampleTDD.Core.Contracts.Repositories;

namespace SampleTDD.Infrastructure.Data.Mongo.Repositories
{
	public class MongoBPRepository : MongoRepositoryBase<BP>, IMongoBPRepository
	{
		public MongoBPRepository(IMongoSampleTDDContext mongoSampleTDDContext) : base(mongoSampleTDDContext)
		{
		}

		public bool UpdateOrInsertBPI(IClientSessionHandle session, BP collection)
		{
			return base.Update(session, collection, true);
		}

	}
}


