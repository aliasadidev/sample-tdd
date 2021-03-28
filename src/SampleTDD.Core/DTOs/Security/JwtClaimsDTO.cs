using SampleTDD.Core.Constants;
using System.Collections.Generic;
using System.Linq;

namespace SampleTDD.Core.DTOs.Security
{

	public class JwtClaimsDTO
	{
		public string UserName { get; set; }
		public long UserID { get; set; }
		public RoleTypes RoleID { get; set; }
		public IEnumerable<JwtUserRoleDTO> UserRoles { get; set; }
		public bool HasAccessInCurrentSubSystem { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsValid { get; set; }
	}
}

