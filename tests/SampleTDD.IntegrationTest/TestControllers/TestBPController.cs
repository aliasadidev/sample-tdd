using Xunit;
using System.Net.Http;
using SampleTDD.IntegrationTest;
using SampleTDD.Api;
using SampleTDD.Core.DTOs.Dashboard;
using SampleTDD.Core.Constants;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Mime;
using SampleTDD.Core.DTOs;
using SampleTDD.Core.DTOs.BPIs.BPIRequestSampleTDD;
using System.Collections.Generic;
using SampleTDD.Core.DTOs.BPIs;

namespace SampleTDD.IntegrationTest.TestControllers
{
	public class TestBPController : IClassFixture<TestFixture<Startup>>
	{
		private readonly HttpClient _client;
		private readonly string bpCtrlName = nameof(Api.Controllers.BPController).Replace("Controller", "");
		public TestBPController(TestFixture<Startup> fixture)
		{
			_client = fixture.Client;
		}

		[Theory]
		[InlineData(true, true, false, RoleTypes.InsuredCustomer)]
		[InlineData(false, false, true, RoleTypes.BranchAdmin)]
		[InlineData(false, false, false, RoleTypes.Assessor)]
		[InlineData(false, false, false, RoleTypes.Doctor)]
		[InlineData(false, false, false, RoleTypes.SampleTDDAdmin)]
		public void GetUserInfo_is_correct(bool canStart, bool isLimited, bool isBranch, RoleTypes role)
		{
			// Arrange
			var request = $"/api/{ bpCtrlName }/{nameof(Api.Controllers.BPController.GetUserInfo)}";
			_client.AddAuthToken(role);
			// Act
			var response = _client.GetAsync(request).Result;
			var dto = response.ToDTOResult<UserInfoDTO>();

			// Assert
			response.EnsureSuccessStatusCode();
			var expectedObj = new UserInfoDTO
			{
				CanStart = canStart,
				IsBranch = isBranch,
				IsLimited = isLimited
			};
			expectedObj.IsEqula(dto.Item);
		}

		[Fact]
		public async Task Update_an_BP()
		{
			// Arrange
			var request = $"/api/{ bpCtrlName }/{nameof(Api.Controllers.BPController.Edit)}";
			_client.AddAuthToken(RoleTypes.InsuredCustomer);

			//  var da = "{'RequestSampleTDD':{'DocumentDetails':[{'NoeKhesaratFileTypeID':4,'SubNoeKhesaratTypeID':2,'Files':[{'FileID':'5fd5da31427ff0655a4011fe','FileName':'Screenshot from 2020-11-15 21-44-42.png','Desc':null,'FullNameUserCreator':null,'UserID':0,'IsRemoved':false,'ID':'000000000000000000000000'}]},{'NoeKhesaratFileTypeID':5,'SubNoeKhesaratTypeID':2,'Files':[{'FileID':'5fd5da4c427ff0655a4011ff','FileName':'Screenshot from 2020-11-17 11-17-17.png','Desc':null,'FullNameUserCreator':null,'UserID':0,'IsRemoved':false,'ID':'000000000000000000000000'}]},{'NoeKhesaratFileTypeID':3,'SubNoeKhesaratTypeID':1,'Files':[{'FileID':'5fd5f19c34275d3e389908ec','FileName':'Screenshot from 2020-11-17 12-38-04.png','IsUpload':true}]},{'NoeKhesaratFileTypeID':8,'SubNoeKhesaratTypeID':3,'Files':[{'FileID':'5fd5f18534275d3e389908e9','FileName':'Screenshot from 2020-11-17 12-38-04.png','IsUpload':true}]}]},'ID':'5fd3cd7da8596a05515d0573'}";
			// var data = new StringContent(da
			// , Encoding.UTF8,
			// "application/json");
			// var x = JsonConvert.DeserializeObject(da);

			var json = JsonConvert.SerializeObject(new BPDTO
			{
				RequestSampleTDD = new BPIRequestSampleTDDDTO
				{
					RequestSampleTDDItems = new List<RequestSampleTDDItemDTO>()
					{
						new RequestSampleTDDItemDTO{
							InvoiceDocuments = new List<InvoiceDocumentDTO>(){
								new InvoiceDocumentDTO {
									NoeKhesaratFileTypeID  = 4,
									NoeKhesaratFileTypeTitle = "",
									Files = new List<BPIFileDTO>(){
										new BPIFileDTO{
											FileID = "5fd5da31427ff0655a4011fe",
											FileName ="Screenshot from 2020-11-15 21-44-42.png",
										}
									}
								}
							}
						}
					}
				},
				ID = "5fd3cd7da8596a05515d0573"
			}); // or JsonSerializer.Serialize if using System.Text.Json


			var stringContent = new StringContent(json, UnicodeEncoding.UTF8, MediaTypeNames.Application.Json); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
																												// _client.DefaultRequestHeaders.Add("Accept", "application/json");
																												//_client.DefaultRequestHeaders.Add("Content-Type", "application/json");
																												// Act
																												// for (int i = 0; i < 10; i++)
																												// {
																												//     var response = await _client.PostAsync(request, stringContent);
																												//     // var xx = response.Content.ReadAsStringAsync();
																												//     response.EnsureSuccessStatusCode();
																												// }

			Parallel.For(0, 3, (ttt, tt) =>
			{
				var response = _client.PostAsync(request, stringContent).Result;
				// var xx = response.Content.ReadAsStringAsync();
				response.EnsureSuccessStatusCode();
			});


		}
	}
}