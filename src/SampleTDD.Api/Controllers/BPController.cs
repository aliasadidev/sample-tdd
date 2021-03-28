using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;
using SampleTDD.Core.Contracts.Services;
using SampleTDD.Core.Contracts.Services.Security;
using SampleTDD.Core.DTOs;
using SampleTDD.Core.Wrappers;

namespace SampleTDD.Api.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class BPController : ControllerBase
	{
		private readonly IBPService _bpService;
		private readonly IJwtService _jwtService;
		public BPController(IJwtService jwtService, IBPService bpService)
		{
			_jwtService = jwtService;
			_bpService = bpService;
		}

		[HttpPost]
		[Route("[action]")]
		public JSONResultBase Approve([FromBody] BPDTO dto)
		{
			_bpService.Approve(dto, _jwtService.Identity.RoleID, _jwtService.Identity.UserID);
			return OK(message: "The business process successfully approved");
		}

		[HttpPost]
		[Route("[action]")]
		public JSONResultBase Post([FromBody] BPDTO dto)
		{
			_bpService.Start(dto, _jwtService.Identity.RoleID, _jwtService.Identity.UserID);
			return OK(message: "The business process successfully created");
		}

		[HttpGet]
		[Route("[action]/{bpID}")]
		public JSONResult<BPDTO> Get(string bpID)
		{
			ObjectId id = ObjectId.Parse(bpID);
			var result = _bpService.GetDTO(id, _jwtService.Identity.RoleID, _jwtService.Identity.UserID);
			return new JSONResult<BPDTO>(result);
		}

		protected JSONResultBase OK(string message = "successful", int statusCode = 200)
		{
			return new JSONResult<string>(null, message: message, statusCode: statusCode);
		}

	}
}