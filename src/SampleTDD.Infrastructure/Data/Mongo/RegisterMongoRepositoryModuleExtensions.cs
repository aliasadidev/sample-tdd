using Microsoft.Extensions.DependencyInjection;
using SampleTDD.Core.Contracts.Repositories;
using SampleTDD.Infrastructure.Data.Mongo.Repositories;

namespace SampleTDD.Infrastructure.Data.Mongo
{
	public static class RegisterMongoRepositoryModuleExtensions
	{
		public static void RegisterMongoRepositoryServices(this IServiceCollection services)
		{
			services.AddTransient<IMongoSampleTDDContext, MongoSampleTDDContext>();
			services.AddTransient<IMongoBPRepository, MongoBPRepository>();
			services.AddTransient<IMongoBPStateRepository, MongoBPStateRepository>();
			services.AddTransient<IMongoChangeStateRuleRepository, MongoChangeStateRuleRepository>();
		}
	}
}
