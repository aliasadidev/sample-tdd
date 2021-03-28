using SampleTDD.Core.Contracts.Repositories;
using SampleTDD.Core.DTOs.Settings;
using SampleTDD.Infrastructure.Data.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SampleTDD.IntegrationTest.TestServices
{
	public class TestMongoSampleTDDContext : MongoSampleTDDContext
	{
		public TestMongoSampleTDDContext(IOptions<AppSettings> options) : base(options)
		{
		}
	}
}