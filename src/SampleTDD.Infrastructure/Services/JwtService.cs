
using System;
using System.Text;
using System.Linq;
using System.Net;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using SampleTDD.Core.Modules;
using SampleTDD.Core.Constants;
using SampleTDD.Core.DTOs.Security;
using SampleTDD.Core.Contracts.Services.Security;
using SampleTDD.Core.Collections;

namespace SampleTDD.Infrastructure.Services
{
	public class JwtService : IJwtService
	{

		public JwtClaimsDTO Identity { get; private set; }


		public const string SECRET_KEY = "1qaz2wsx3edc4rfv5tgb6yhn7ujm";



		public JwtClaimsDTO LoadUserIdentity(string token, bool throwException = true, bool checkIsVerified = true)
		{
			var result = decryptToken(token);
			if (!result.IsValid && throwException)
				throw new CustomException("loging again", CustomStatus.MoveToLogin.ToInt(), HttpStatusCode.Unauthorized, autoRedirectURL: "Authenticate/Login");
			Identity = result;

			return result;
		}

		private JwtClaimsDTO decryptToken(string token)
		{
			var dto = new JwtClaimsDTO();
			string secret = SECRET_KEY;
			var key = Encoding.ASCII.GetBytes(secret);
			IdentityModelEventSource.ShowPII = true;
			var handler = new JwtSecurityTokenHandler();

			var validations = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false,
			};

			var jwthandler = new JwtSecurityTokenHandler();
			var jwttoken = jwthandler.ReadJwtToken(token);


			if (jwttoken == null)
				throw new CustomException("-", CustomStatus.MoveToLogin.ToInt(), HttpStatusCode.Unauthorized, autoRedirectURL: "Authenticate/Login");

			var expDate = jwttoken.ValidTo;
			if (expDate < DateTime.UtcNow.AddMinutes(-1))
				throw new CustomException("the token expired", CustomStatus.TokenExpired.ToInt(), HttpStatusCode.Unauthorized, autoRedirectURL: "Authenticate/RefreshToken");

			ClaimsPrincipal principal;
			SecurityToken tokenSecure;
			try
			{
				principal = handler.ValidateToken(token, validations, out tokenSecure);
			}
			catch
			{
				throw new CustomException("-", CustomStatus.MoveToLogin.ToInt(), HttpStatusCode.Unauthorized, autoRedirectURL: "Authenticate/Login");
			}

			var validJwt = tokenSecure as JwtSecurityToken;
			if (validJwt == null)
			{
				dto.IsValid = false;
			}
			else

			{

				dto.UserID = Convert.ToInt64(principal.Identity.Name);

				var roleUser = principal.Claims.FirstOrDefault(x => x.Type == "UserRoles")?.Value ?? string.Empty;
				var userRoles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JwtUserRoleDTO>>(roleUser);
				dto.UserRoles = userRoles;
				dto.HasAccessInCurrentSubSystem = userRoles.Any(x => x.SubSystemID == AppSetting.SUB_SYSTEM_ID);
				dto.RoleID = userRoles.FirstOrDefault(x => x.SubSystemID == AppSetting.SUB_SYSTEM_ID).RoleID;
				dto.UserName = principal.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value ?? string.Empty;
				dto.FirstName = principal.Claims.FirstOrDefault(x => x.Type == "FirstName")?.Value ?? string.Empty;
				dto.LastName = principal.Claims.FirstOrDefault(x => x.Type == "LastName")?.Value ?? string.Empty;
				dto.IsValid = true;
			}

			return dto;
		}
	}
}