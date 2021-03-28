using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using SampleTDD.Core.Wrappers;
using SampleTDD.Core.Constants;
using Xunit;

namespace SampleTDD.IntegrationTest
{
	public static class Extensions
	{

		private static TResult toDTOResult<TResult>(HttpResponseMessage response)
		where TResult : class
		{
			string responeData = response.Content.ReadAsStringAsync().Result;
			TResult obj = JsonConvert.DeserializeObject<TResult>(responeData);
			return obj;
		}

		public static JSONResult<TResult> ToDTOResult<TResult>(this HttpResponseMessage response)
		where TResult : class
		{
			return toDTOResult<JSONResult<TResult>>(response);
		}

		public static PaginationResult<TResult> ToPaginationResult<TResult>(this HttpResponseMessage response)
				where TResult : class
		{
			return toDTOResult<PaginationResult<TResult>>(response);
		}

		public static JSONResultList<TResult> ToDTOListResult<TResult>(this HttpResponseMessage response)
			   where TResult : class
		{
			return toDTOResult<JSONResultList<TResult>>(response);
		}

		public static void IsEqula(this object expectedObj, object actualObj)
		{
			var obj1 = JsonConvert.SerializeObject(expectedObj);
			var obj2 = JsonConvert.SerializeObject(actualObj);
			Assert.Equal(obj1, obj2);
		}

		public static void AddAuthToken(this HttpClient client, RoleTypes role)
		{
			string token;
			switch (role)
			{
				case RoleTypes.BranchAdmin:
					{
						token = MockJwtTokens.BranchAdminUser();
						break;
					}
				case RoleTypes.Assessor:
					{
						token = MockJwtTokens.AssessorUser();
						break;
					}
				case RoleTypes.InsuredCustomer:
					{
						token = MockJwtTokens.InsuredCustomerUser();
						break;
					}
				case RoleTypes.Doctor:
					{
						token = MockJwtTokens.DoctorUser();
						break;
					}
				case RoleTypes.SampleTDDAdmin:
					{
						token = MockJwtTokens.AdminUser();
						break;
					}
				default:
					{
						throw new System.ArgumentException($"##the role { role } mock token not found!##");
					}
			}

			if (client.DefaultRequestHeaders.Contains("AuthToken"))
				client.DefaultRequestHeaders.Remove("AuthToken");

			client.DefaultRequestHeaders.Add("AuthToken", token);
		}
	}
}