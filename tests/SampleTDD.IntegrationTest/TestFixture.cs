using System.Net.Http;
using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Linq;
using SampleTDD.Core.Contracts.Services;
using SampleTDD.Infrastructure.Services;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SampleTDD.Core.Constants;
using SampleTDD.Core.Contracts.Services.Security;
using SampleTDD.IntegrationTest.TestServices;
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

		protected virtual void InitializeServices(IServiceCollection services)
		{
			//  var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;


			// var descriptors = services.Where(d => d.ServiceType == typeof(IAuthorizeWebService) ||
			//                                                d.ServiceType == typeof(IMongoSampleTDDContext)).ToList();//DbContextOptions<ApplicationDbContext>)


			// foreach (var descriptor in descriptors)
			// {
			//     services.Remove(descriptor);
			// }

			// services.AddTransient<IMongoSampleTDDContext, TestMongoSampleTDDContext>();
			// services.AddTransient<IAuthorizeWebService, TestAuthorizeWebService>();

			// services.AddTransient<DBSeed>();



			// var manager = new ApplicationPartManager
			// {
			//     ApplicationParts =
			//     {
			//         new AssemblyPart(startupAssembly)
			//     },
			//     FeatureProviders =
			//     {
			//         new ControllerFeatureProvider()
			//     }
			// };

			// services.AddSingleton(manager);
			// services.AddMvc();
		}



		protected TestFixture(string relativeTargetProjectParentDir)
		{
			var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly.ManifestModule.Name.Replace(".dll", "");
			string contentRoot = System.IO.Path.GetFullPath($@"../../../../../src/{startupAssembly }/");

			var configurationBuilder = new ConfigurationBuilder()
				.SetBasePath(contentRoot)
				.AddJsonFile("appsettings.Test.json");
			var configuration = configurationBuilder.Build();
			configuration["MongoDB:Database"] = configuration["MongoDB:Database"] + Guid.NewGuid();

			var webHostBuilder = new WebHostBuilder()
				.UseContentRoot(contentRoot)
				.UseEnvironment("Test")
				.UseStartup(typeof(TStartup))
				//   .ConfigureServices(InitializeServices)
				.UseConfiguration(configuration)
				.ConfigureTestServices((services) =>
				{
					var descriptors = services.Where(d => d.ServiceType == typeof(IAuthorizeWebService) ||
														   d.ServiceType == typeof(IMongoSampleTDDContext)).ToList();//DbContextOptions<ApplicationDbContext>)


					foreach (var descriptor in descriptors)
					{
						services.Remove(descriptor);
					}

					services.AddTransient<IMongoSampleTDDContext, TestMongoSampleTDDContext>();
					services.AddTransient<IAuthorizeWebService, TestAuthorizeWebService>();
					services.AddTransient<DBSeed>();
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
