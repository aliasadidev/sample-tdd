using SampleTDD.Core.Contracts.Services;
using SampleTDD.Core.DTOs.Security;
using SampleTDD.UnitTest.Modules;
using SampleTDD.UnitTest.Seeds;
using Xunit;

namespace SampleTDD.UnitTest.TestInfrastructure
{
	public class WorkflowEngineServiceTests : BaseUnitTest
	{
		[Fact]
		public void GetPremissions_is_correct_for_InsuredCustomer()
		{
			// Arrange
			IWorkflowEngineService srv = GetInstance<IWorkflowEngineService>();

			// Act
			PermissionDTO permission = srv.GetPremissions(DBSeed.BPID, Core.Constants.RoleTypes.InsuredCustomer, DBSeed.UserID);

			// Assert
			Assert.True(permission.CanApprove);
		}
	}
}