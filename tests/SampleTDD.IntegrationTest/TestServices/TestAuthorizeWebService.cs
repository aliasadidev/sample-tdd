using System.Collections.Generic;
using SampleTDD.Core.Contracts.Services.Security;
using SampleTDD.Core.DTOs.Security;

namespace SampleTDD.IntegrationTest.TestServices
{
	public class TestAuthorizeWebService : IAuthorizeWebService
	{
		public bool CheckUser(long userId, string controllerName, string actionName, int subSystemId)
		{
			return true;
		}

		public IEnumerable<ActionsDTO> GetActions(long userId, string controllerName, int subSystemId)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<ControllersDTO> GetControllers(long userId, int subSystemId)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<long> GetUsersWithRoles(IEnumerable<long> roles)
		{
			throw new System.NotImplementedException();
		}
	}
}