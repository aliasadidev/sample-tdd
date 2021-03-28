using AutoMapper;
using MongoDB.Driver;
using SampleTDD.Core.Collections;
using SampleTDD.Core.Contracts.Repositories;

namespace SampleTDD.Infrastructure.Data.Mongo.Repositories
{
	public class MongoBPRepository : MongoRepositoryBase<BP>, IMongoBPRepository
	{
		private readonly IMapper _mapper;
		public MongoBPRepository(IMongoSampleTDDContext mongoSampleTDDContext, IMapper mapper) : base(mongoSampleTDDContext)
		{
			_mapper = mapper;
		}

		public bool UpdateOrInsertBPI(IClientSessionHandle session, BP collection)
		{
			return base.Update(session, collection, true);
		}

	}
}


