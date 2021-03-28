using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SampleTDD.Core.Contracts.Services.Security;
using SampleTDD.Core.Modules;

namespace SampleTDD.Api.Modules
{

	public class AuthApiMiddleware
	{
		private readonly RequestDelegate _next;


		public AuthApiMiddleware(RequestDelegate next)
		{
			_next = next;
		}


		public Task Invoke(HttpContext httpContext, IJwtService _jwtService)
		{

			string token = httpContext.Request.Headers["AuthToken"].ToString();

			if (string.IsNullOrWhiteSpace(token))
				throw new CustomException("The token is empty", (int)HttpStatusCode.Unauthorized);

			_jwtService.LoadUserIdentity(token);

			return _next(httpContext);
		}
	}
}