using System.Collections.Generic;
using MongoDB.Bson;
using SampleTDD.Core.Constants;
using SampleTDD.Core.DTOs;

namespace SampleTDD.Core.Contracts.Services
{
	public interface IBPService
	{
		BPDTO GetDTO(ObjectId bpID, RoleTypes roleID, long userID);
		void Approve(BPDTO dto, RoleTypes roleID, long userID);
		void Start(BPDTO bpDTO, RoleTypes roleID, long userID);
		IEnumerable<BPDTO> GetAll(RoleTypes roleID, long userID);
	}
}
