using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using SampleTDD.Core.Collections;
using SampleTDD.Core.Constants;
using SampleTDD.Core.Contracts.Repositories;
using SampleTDD.Core.Contracts.Services;
using SampleTDD.Core.DTOs;
using SampleTDD.Core.Modules;
using System;
using SampleTDD.Core.Collections.BPIs.BPISabteDarkhast;

namespace SampleTDD.Infrastructure.Services
{
	public class BPService : IBPService
	{
		private readonly IMongoBPRepository _bpRepo;
		private readonly IMapper _mapper;
		private readonly IWorkflowEngineService _wfeSrv;
		private readonly IMongoChangeStateRuleRepository _csrRepo;
		private readonly IDTOMapperService _dtoMapper;
		private readonly IMongoBPStateRepository _bpStateRepo;

		public BPService(
			IMongoBPRepository bpRepo,
			IMapper mapper,
			IWorkflowEngineService wfeSrv,
			IMongoChangeStateRuleRepository csrRepo,
			IDTOMapperService dtoMapper,
			IMongoBPStateRepository bpStateRepo
			)
		{
			_bpRepo = bpRepo;
			_mapper = mapper;
			_wfeSrv = wfeSrv;
			_csrRepo = csrRepo;
			_dtoMapper = dtoMapper;
			_bpStateRepo = bpStateRepo;
		}

		public void Start(BPDTO bpDTO, RoleTypes roleID, long userID)
		{

			var bpCollec = new BP();

			var sd = _mapper.Map<BPISabteDarkhast>(bpDTO.SabteDarkhast);

			bpCollec.SabteDarkhast = sd;
			bpCollec.UserID = userID;
			bpCollec.CreationTime = DateTime.Now;

			_bpRepo.StartTransaction((session) =>
			{
				_bpRepo.Add(session, bpCollec);
				_wfeSrv.Start(session, bpCollec._id, roleID, userID);

			});
			bpDTO.ID = bpCollec._id.ToString();

		}

		public void Approve(BPDTO dto, RoleTypes roleID, long userID)
		{
			_bpRepo.StartTransaction((session) =>
			{
				ObjectId bpID = ObjectId.Parse(dto.ID);
				var currentState = _wfeSrv.GetCurrentBpState(bpID, roleID);
				bool hasApproveDTO = _dtoMapper.CheckApproveTypeIsMapped(currentState);

				if (hasApproveDTO)
					saveApproveData(session, dto, bpID, currentState);

				_wfeSrv.Approve(session, bpID, roleID, userID);
			});
		}


		private void saveApproveData(IClientSessionHandle session, BPDTO dto, ObjectId bpID, StateTypes currentState)
		{
			Type targetType = _dtoMapper.GetApproveType(currentState);

			BP bp = _bpRepo.Get(session, bpID);
			BPDTO dtox = _mapper.Map<BPDTO>(bp);
			var resultx = _mapper.Map(dto, typeof(BPDTO), targetType);

			var final = CoreExtensions.MergeObject(resultx, dtox);

			var finalBP = _mapper.Map<BP>(final);

			_bpRepo.UpdateOrInsertBPI(session, finalBP);
		}


		public BPDTO GetDTO(ObjectId bpID, RoleTypes roleID, long userID)
		{
			StateTypes currentState = _wfeSrv.GetCurrentBpState(bpID, roleID);

			StateTypes targetState = _bpStateRepo.GetRoleStateView(bpID, roleID, currentState);

			Type targetType = _dtoMapper.Get(targetState);

			BP bp = _bpRepo.Get(bpID);
			BPDTO dto = _mapper.Map<BPDTO>(bp);
			var result = _mapper.Map(dto, typeof(BPDTO), targetType);
			BPDTO resultfinal = _mapper.Map(result, targetType, typeof(BPDTO)) as BPDTO;
			resultfinal.Permission = _wfeSrv.GetPremissions(bpID, roleID, userID);
			resultfinal.StateName = targetState.ToString();
			resultfinal.StateType = targetState;
			return resultfinal;
		}
	}
}
