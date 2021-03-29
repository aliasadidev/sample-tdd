
using System;
using Microsoft.Extensions.Configuration;

using SampleTDD.Core.Modules;
using SampleTDD.Infrastructure.Services;
using SampleTDD.Infrastructure.Data.Mongo;
using Microsoft.Extensions.DependencyInjection;
using SampleTDD.UnitTest.Seeds;

namespace SampleTDD.UnitTest.Modules
{

	public abstract class BaseUnitTest
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public BaseUnitTest()
		{
			if (_serviceProvider == null)
			{
				IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
				configurationBuilder.AddJsonFile("appsettings.Development.json");

				IConfiguration configuration = configurationBuilder.Build();
				ServiceCollection srvCollection = new ServiceCollection();


				srvCollection.RegisterCoreServices();
				srvCollection.RegisterMongoRepositoryServices();
				srvCollection.RegisterInfrastructureServices();

				srvCollection.Configure<SampleTDD.Core.DTOs.Settings.AppSettings>(configuration);

				srvCollection.AddTransient<IMongoSampleTDDContextTest, MongoSampleTDDContextTest>();
				srvCollection.AddTransient<MongoSampleTDDContext, MongoSampleTDDContextTest>();
				srvCollection.AddSingleton<DBSeed>();

				_serviceProvider = srvCollection.BuildServiceProvider();
				_serviceScopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();

				var db = GetInstance<DBSeed>();
				db.SeedData();
			}
		}


		protected T GetInstance<T>()
		{
			return _serviceProvider.GetService<T>();
		}

		protected T GetInstance<T>(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetService<T>();
		}

		private IServiceScope getNewScope => _serviceScopeFactory.CreateScope();

	}

}