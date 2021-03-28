using SampleTDD.Core.Constants;

namespace SampleTDD.Core.DTOs.Security
{

	public class JwtUserRoleDTO
	{
		public RoleTypes RoleID { get; set; }
		public int SubSystemID { get; set; }
		public string HCode { get; set; }
	}
}
