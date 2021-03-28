using SampleTDD.Core.Constants;
using SampleTDD.Core.Contracts.Services;
using SampleTDD.Core.DTOs;
using SampleTDD.Core.DTOs.States;
using SampleTDD.Core.DTOs.States.Approve;
using SampleTDD.Core.Modules;
using System;
using System.Collections.Generic;

namespace SampleTDD.Infrastructure.Services
{
	public class DTOMapperService : IDTOMapperService
	{
		static readonly Dictionary<StateTypes, Type> _getDtoMapper;
		static readonly Dictionary<StateTypes, Type> _approveDtoMapper;

		static DTOMapperService()
		{
			_getDtoMapper = new Dictionary<StateTypes, Type>
			{
				{ StateTypes.Start, typeof(SabteDarkhastStateDTO) },

				{ StateTypes.FinishedSuccessfully, typeof(BPDTO) },
				{ StateTypes.Closed, typeof(BPDTO) },
				{ StateTypes.FinishedUnsuccessfully, typeof(BPDTO) },
			};

			_approveDtoMapper = new Dictionary<StateTypes, Type>
			{
				{ StateTypes.Start, typeof(SabteDarkhastStateApproveDTO) },
			};

		}

		public Type Get(StateTypes state)
		{
			if (!_getDtoMapper.ContainsKey(state))
			{
				throw new CustomException("the dto map not found", 500);
			}
			return _getDtoMapper[state];
		}

		public Type GetApproveType(StateTypes state)
		{
			if (!_approveDtoMapper.ContainsKey(state))
			{
				throw new CustomException("the dto map not found", 500);
			}
			return _approveDtoMapper[state];
		}

		public bool CheckApproveTypeIsMapped(StateTypes state) => _approveDtoMapper.ContainsKey(state);

	}
}
