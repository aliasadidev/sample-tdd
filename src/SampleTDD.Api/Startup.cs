using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleTDD.Core.Modules;
using SampleTDD.Infrastructure.Services;
using SampleTDD.Infrastructure.Data.Mongo;
using SampleTDD.Core.DTOs.Settings;

namespace SampleTDD.Api
{

	public partial class Startup
	{
		public Startup(IWebHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
								.SetBasePath(env.ContentRootPath)
								.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
								.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore().AddApiExplorer().AddJsonOptions(o =>
			{
				o.JsonSerializerOptions.PropertyNamingPolicy = null;
			});

			services.RegisterCoreServices();
			services.RegisterMongoRepositoryServices();
			services.RegisterInfrastructureServices();
			services.AddOptions();
			services.Configure<AppSettings>(Configuration);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

			if (env.IsDevelopment() || env.IsStaging())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseResponseCaching();
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute("rest-api", "api/{controller}/{action}/{id?}");
				endpoints.MapRazorPages();
			});

		}
	}
}
