using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SampleTDD.Core.Constants;
using SampleTDD.Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;

namespace SampleTDD.IntegrationTest
{
	public static class MockJwtTokens
	{
		public static string GenerateToken(ClaimsIdentity claimsIdentity)
		{
			byte[] key = Encoding.ASCII.GetBytes(JwtService.SECRET_KEY);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = claimsIdentity,
				Expires = DateTime.UtcNow.AddDays(30),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
			string tokenString = tokenHandler.WriteToken(token);
			return tokenString;
		}


		public static string InsuredCustomerUser()
		{
			var userRoles = new[]{
								new {
									RoleID = RoleTypes.InsuredCustomer,
									SubSystemID = AppSetting.SUB_SYSTEM_ID,
									HCode = "1.13.1.4.1"
								}
			};

			var userRoleDto = Newtonsoft.Json.JsonConvert.SerializeObject(userRoles);
			var claims = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, "-2"),
					new Claim("UserRoles", userRoleDto),
					new Claim("FirstName","Insured Customer"),
					new Claim("LastName","Test"),
					new Claim("UserName","InsuredCustomer"),
					new Claim("IsVerified",true.ToString()),
				});
			return GenerateToken(claims);
		}

		public static string AdminUser()
		{
			var userRoles = new[]{
								new {
									RoleID = RoleTypes.Admin,
									SubSystemID = AppSetting.SUB_SYSTEM_ID,
									HCode = "1.13.1.1"
								}
			};

			var userRoleDto = Newtonsoft.Json.JsonConvert.SerializeObject(userRoles);
			var claims = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, "-5"),
					new Claim("UserRoles", userRoleDto),
					new Claim("FirstName","Admin"),
					new Claim("LastName","Test"),
					new Claim("UserName","SampleTDDAdmin"),
					new Claim("IsVerified",true.ToString()),
				});
			return GenerateToken(claims);
		}
	}
}