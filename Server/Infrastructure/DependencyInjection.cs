using Domain.Interfaces;
using Domain.Interfaces.Requests;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDutyRepository, DutyRepository>();
            services.AddScoped<IPersonalRequestsRepository, PersonalRequestsRepository>();
            services.AddScoped<ISwapRepository, SwapRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IVacationRepository, VacationRepository>();

            return services;
        }
    }
}