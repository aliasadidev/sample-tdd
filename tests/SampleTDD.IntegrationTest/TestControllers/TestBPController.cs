using Xunit;
using System.Net.Http;
using SampleTDD.Api;
using SampleTDD.Core.Constants;
using System.Text;
using Newtonsoft.Json;
using System.Net.Mime;
using SampleTDD.Core.DTOs;
using SampleTDD.Core.DTOs.BPIs.BPISabteDarkhast;
using SampleTDD.Core.Wrappers;
using Xunit.Priority;
using System.Linq;

namespace SampleTDD.IntegrationTest.TestControllers
{
	[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
	public class TestBPController : IClassFixture<TestFixture<Startup>>
	{
		private readonly HttpClient _client;
		private readonly string bpCtrlName = nameof(Api.Controllers.BPController).Replace("Controller", "");
		public TestBPController(TestFixture<Startup> fixture)
		{
			_client = fixture.Client;
		}


		[Fact, Priority(1)]
		public void CreateNewBP()
		{
			// Arrange
			var request = $"/api/{ bpCtrlName }/{nameof(Api.Controllers.BPController.NewBP)}";
			_client.AddAuthToken(RoleTypes.InsuredCustomer);

			BPDTO dto = new BPDTO
			{

				SabteDarkhast = new BPISabteDarkhastDTO
				{
					BranchID = 1,
					BranchName = "Berlin",
					CityID = 150,
					FullName = "Ali Asadi",
					PhoneNumber = "+989126449201",
					ProvinceID = 158
				}
			};
			var jsonDTO = JsonConvert.SerializeObject(dto);
			var stringContent = new StringContent(jsonDTO, UnicodeEncoding.UTF8, MediaTypeNames.Application.Json);

			// Act
			var response = _client.PostAsync(request, stringContent).Result;
			var resultDTO = response.ToDTOResult<string>();

			// Assert
			response.EnsureSuccessStatusCode();
			Assert.True(resultDTO.StatusCode == 200);
		}


		[Fact, Priority(2)]
		public void GetAll()
		{
			// Arrange
			var request = $"/api/{ bpCtrlName }/{nameof(Api.Controllers.BPController.GetAll)}";
			_client.AddAuthToken(RoleTypes.InsuredCustomer);

			// Act
			var response = _client.GetAsync(request).Result;
			var resultDTO = response.ToDTOListResult<BPDTO>();

			// Assert
			response.EnsureSuccessStatusCode();
			Assert.True(resultDTO.StatusCode == 200);
			Assert.True(resultDTO.Items.Count() == 1);
		}


		[Fact, Priority(3)]
		public void Get()
		{
			// Arrange
			var requestGetAll = $"/api/{ bpCtrlName }/{nameof(Api.Controllers.BPController.GetAll)}";
			_client.AddAuthToken(RoleTypes.InsuredCustomer);

			// Act
			var responseGetAll = _client.GetAsync(requestGetAll).Result;
			var resultDTO = responseGetAll.ToDTOListResult<BPDTO>().Items.First();

			var request = $"/api/{ bpCtrlName }/{nameof(Api.Controllers.BPController.Get)}/{ resultDTO.ID }";
			var response = _client.GetAsync(request).Result;
			var resultDTOGet = response.ToDTOResult<BPDTO>();

			// Assert
			response.EnsureSuccessStatusCode();
			Assert.True(resultDTOGet.StatusCode == 200);
			Assert.True(resultDTOGet.Item != null);
		}

	}
}