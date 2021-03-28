using MongoDB.Bson;
using SampleTDD.Core.Constants;
using SampleTDD.Core.DTOs.Security;
using MongoDB.Driver;

namespace SampleTDD.Core.Contracts.Services
{
	public interface IWorkflowEngineService
	{
		void Start(IClientSessionHandle session, ObjectId bpID, RoleTypes RoleID, long userID);
		void Approve(IClientSessionHandle session, ObjectId bpID, RoleTypes RoleID, long userID);
		PermissionDTO GetPremissions(ObjectId bpID, RoleTypes roleID, long userID);
		StateTypes GetCurrentBpState(ObjectId bpID, RoleTypes roleID);
	}
}
