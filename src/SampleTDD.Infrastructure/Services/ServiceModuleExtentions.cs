using SampleTDD.Core.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using SampleTDD.Core.Contracts.Services.Security;

namespace SampleTDD.Infrastructure.Services
{
	public static class ServiceModuleExtensions
	{
		public static void RegisterInfrastructureServices(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IJwtService, JwtService>();
			serviceCollection.AddSingleton<IDTOMapperService, DTOMapperService>();
			serviceCollection.AddTransient<IBPService, BPService>();
			serviceCollection.AddTransient<IWorkflowEngineService, WorkflowEngineService>();
		}
	}
}
