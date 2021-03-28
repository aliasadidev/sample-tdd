using AutoMapper;
using SampleTDD.Core.DTOs;
using SampleTDD.Core.Collections;
using SampleTDD.Core.DTOs.States;
using SampleTDD.Core.DTOs.States.Approve;
using SampleTDD.Core.Collections.BPIs.BPISabteDarkhast;
using SampleTDD.Core.DTOs.BPIs.BPISabteDarkhast;


namespace SampleTDD.Core.Modules
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<BP, BPDTO>().ChangeNaming();
			CreateMap<BPISabteDarkhastDTO, BPISabteDarkhast>().ReverseMap();

			#region Get DTO
			CreateMap<BPDTO, SabteDarkhastStateDTO>().ReverseMap();
			#endregion

			#region Approve & Edit DTO
			CreateMap<BPDTO, SabteDarkhastStateApproveDTO>().ReverseMap();
			#endregion

		}
	}
}
