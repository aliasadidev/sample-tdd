using SampleTDD.Core.Contracts.Services;
using SampleTDD.UnitTest.Modules;
using Xunit;

namespace SampleTDD.UnitTest.TestInfrastructure
{
	public class WorkflowEngineServiceTests : BaseUnitTest
	{
		[Fact]
		public void GetPremissions_is_correct_for_InsuredCustomer()
		{
			// Arrange
			var srv = GetInstance<IWorkflowEngineService>();

			// Act
			var permission = srv.GetPremissions(MongoSampleTDDContextTest.BPID,
					Core.Constants.RoleTypes.InsuredCustomer, MongoSampleTDDContextTest.UserID);

			// Assert
			Assert.True(permission.CanApprove);
		}
	}
}