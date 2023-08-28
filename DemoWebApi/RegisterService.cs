using AutoMapper;
using DemoWebApi.Localizers;
using DemoWebApi.Middlewares;
using DemoWebApi.Repositories;
using DemoWebApi.Services;
using Microsoft.Extensions.Localization;
using NetCore.AutoRegisterDi;
using System.Reflection;

namespace DemoWebApi
{
    public static class RegisterService
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

            services.AddSingleton<LocalizationMiddleware>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            var assembliesToScans = new[]
            {
                Assembly.GetAssembly(typeof(ApplicationServiceBase)),
            };

            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScans)
            .Where(x => x.Name.EndsWith("Repository") || x.Name.EndsWith("Service"))
              .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            var mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddHttpContextAccessor();
        }
    }
}
