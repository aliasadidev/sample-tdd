using SampleTDD.Core.Constants;
using System;

namespace SampleTDD.Core.Contracts.Services
{
	public interface IDTOMapperService
	{
		bool CheckApproveTypeIsMapped(StateTypes state);
		Type Get(StateTypes state);
		Type GetApproveType(StateTypes state);
	}
}
