using System.Net.Http;
using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Linq;
using SampleTDD.IntegrationTest.Modules;
using SampleTDD.Core.Contracts.Repositories;
using SampleTDD.IntegrationTest.Seeds;

namespace SampleTDD.IntegrationTest
{
	public class TestFixture<TStartup> : IDisposable
	{
		private TestServer Server;

		public TestFixture()
			: this(Path.Combine(""))
		{
		}

		public HttpClient Client { get; }

		public void Dispose()
		{
			Client.Dispose();
			Server.Dispose();
		}

		protected TestFixture(string relativeTargetProjectParentDir)
		{
			var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly.ManifestModule.Name.Replace(".dll", "");
			string contentRoot = System.IO.Path.GetFullPath($@"../../../../../src/{startupAssembly }/");

			var configurationBuilder = new ConfigurationBuilder()
				.SetBasePath(contentRoot)
				.AddJsonFile("appsettings.Development.json");
			var configuration = configurationBuilder.Build();
			configuration["MongoDB:Database"] = configuration["MongoDB:Database"];

			var webHostBuilder = new WebHostBuilder()
				.UseContentRoot(contentRoot)
				.UseEnvironment("Development")
				.UseStartup(typeof(TStartup))
				.UseConfiguration(configuration)
				.ConfigureTestServices((services) =>
				{
					var descriptors = services.Where(d =>
														   d.ServiceType == typeof(IMongoSampleTDDContext)).ToList();


					foreach (var descriptor in descriptors)
					{
						services.Remove(descriptor);
					}

					services.AddSingleton<IMongoSampleTDDContext, MongoSampleTDDContextTest>();
					services.AddTransient<DBSeed>();
					services.AddRazorPages();
				});


			// Create instance of test server
			Server = new TestServer(webHostBuilder);
			// seed data to mongodb
			var seed = Server.Services.GetService<DBSeed>();
			seed.Init();
			// Add configuration for client
			Client = Server.CreateClient();

			Client.BaseAddress = new Uri("http://localhost:5001");
			Client.DefaultRequestHeaders.Accept.Clear();
			Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}
	}
}
