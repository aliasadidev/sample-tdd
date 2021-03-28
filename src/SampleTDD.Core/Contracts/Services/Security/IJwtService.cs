using SampleTDD.Core.DTOs.Security;

namespace SampleTDD.Core.Contracts.Services.Security
{
	public interface IJwtService
	{

		JwtClaimsDTO LoadUserIdentity(string token, bool throwException = true, bool checkIsVerified = true);
		JwtClaimsDTO Identity { get; }
	}
}
