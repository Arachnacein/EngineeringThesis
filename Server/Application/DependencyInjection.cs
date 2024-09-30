using Application.Interfaces;
using Application.Interfaces.Mappers;
using Application.Logic.ScheduleGenerator;
using Application.Mappings;
using Application.Security;
using Application.Services;
using Application.Services.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDutyService, DutyService>();
            services.AddScoped<IPersonalRequestsService, PersonalRequestsService>();
            services.AddScoped<ISwapService, SwapService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IGeneratorService, GeneratorService>();
            services.AddScoped<IVacationService, VacationService>();
            services.AddScoped<IDateTimeHelper, DateTimeHelper>();
            services.AddScoped<ISecurityHashClass, SecurityHashClass>();
            services.AddScoped<ILogicService, LogicService>();


            services.AddSingleton(MapperConfig.Initialize());

            return services;
        }
        public static IServiceCollection RegisterMappers(this IServiceCollection services)
        {
            services.AddScoped<IScheduleMapper, ScheduleMapper>();
            services.AddScoped<IPersonalRequestsMapper, PersonalRequestsMapper>();
            services.AddScoped<ISwapMapper, SwapMapper>();
            services.AddScoped<IDutyMapper, DutyMapper>();
            services.AddScoped<IUserMapper, UserMapper>();
            services.AddScoped<IVacationMapper, VacationMapper>();

            return services;
        }
    }
}
