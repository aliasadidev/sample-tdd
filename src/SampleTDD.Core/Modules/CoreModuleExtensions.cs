using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace SampleTDD.Core.Modules
{
	public static class CoreModuleExtensions
	{
		public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
		{

			var automapper = registerAutoMapperService();
			services.AddSingleton(automapper);
			return services;
		}

		private static IMapper registerAutoMapperService()
		{
			// Auto Mapper Configurations
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			IMapper mapper = mappingConfig.CreateMapper();

			return mapper;
		}
	}
}
