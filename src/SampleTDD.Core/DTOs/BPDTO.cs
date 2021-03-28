using SampleTDD.Core.DTOs.Security;
using SampleTDD.Core.Constants;
using SampleTDD.Core.DTOs.BPIs.BPISabteDarkhast;

namespace SampleTDD.Core.DTOs
{
	public sealed class BPDTO : BaseCollectionDTO
	{
		public PermissionDTO Permission { get; set; }
		public string StateName { get; set; }
		public StateTypes StateType { get; set; }
		public BPISabteDarkhastDTO SabteDarkhast { get; set; }
	}
}

