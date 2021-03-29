using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleTDD.Core.Contracts.Repositories;
using SampleTDD.Core.DTOs.Settings;
using SampleTDD.Core.Modules;
using SampleTDD.Infrastructure.Data.Mongo;
using SampleTDD.Infrastructure.Services;
using SampleTDD.UnitTest.Seeds;

namespace SampleTDD.UnitTest.Modules
{
	public class SharedFixture : IDisposable
	{
		private IServiceProvider _serviceProvider;

		public SharedFixture()
		{
			IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.AddJsonFile("appsettings.Development.json");

			IConfiguration configuration = configurationBuilder.Build();
			ServiceCollection srvCollection = new ServiceCollection();

			srvCollection.AddTransient<IMongoSampleTDDContext, MongoSampleTDDContextTest>();
			srvCollection.RegisterCoreServices();
			srvCollection.RegisterMongoRepositoryServices();
			srvCollection.RegisterInfrastructureServices();

			srvCollection.Configure<AppSettings>(configuration);

			_serviceProvider = srvCollection.BuildServiceProvider();
			var db = GetInstance<IMongoSampleTDDContext>();
			// seed data
			new DBSeed(db).SeedData();
		}

		public void Dispose()
		{
			_serviceProvider = null;
		}


		public T GetInstance<T>()
		{
			return _serviceProvider.GetService<T>();
		}

		public IServiceScope GetNewScope()
		{
			var serviceScopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();
			return serviceScopeFactory.CreateScope();
		}

	}
}
