using System.Net.Http;
using System.Threading.Tasks;
using SampleTDD.Api;
using SampleTDD.Core.Collections;
using Xunit;

namespace SampleTDD.IntegrationTest.TestControllers
{
	public class TestCommonController : IClassFixture<TestFixture<Startup>>
	{
		private readonly HttpClient _client;
		private readonly string bpCtrlName = nameof(Api.Controllers.CommonController).Replace("Controller", "");
		public TestCommonController(TestFixture<Startup> fixture)
		{
			_client = fixture.Client;
		}


		[Fact]
		public async Task GetStateTypes_is_correct()
		{
			// Arrange
			var request = $"/api/{ bpCtrlName }/{nameof(Api.Controllers.CommonController.GetStateTypes)}";
			_client.AddAuthToken(Core.Constants.RoleTypes.BranchAdmin);
			// Act
			var response = await _client.GetAsync(request);
			var dto = response.ToDTOListResult<EnumTypeItem>();

			// Assert
			response.EnsureSuccessStatusCode();
			Assert.NotEmpty(dto.Items);
		}

	}
}