using MongoDB.Bson;
using SampleTDD.Core.Collections;
using SampleTDD.Core.Constants;
using MongoDB.Driver;

namespace SampleTDD.Core.Contracts.Repositories
{
	public interface IMongoBPRepository : IMongoRepositoryBase<BP>
	{
		bool HasAccess(RoleTypes roleID, ObjectId bpID);
		bool UpdateOrInsertBPI(IClientSessionHandle session, BP collection);
		ushort GetBranchID(ObjectId bpID);
	}
}