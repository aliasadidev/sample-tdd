using MongoDB.Bson;
using SampleTDD.Core.Collections;
using SampleTDD.Core.Constants;
using MongoDB.Driver;

namespace SampleTDD.Core.Contracts.Repositories
{
	public interface IMongoBPRepository : IMongoRepositoryBase<BP>
	{
		bool UpdateOrInsertBPI(IClientSessionHandle session, BP collection);
	}
}