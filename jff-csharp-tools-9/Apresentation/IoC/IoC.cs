using JffCsharpTools.Domain.Interfaces.Repositories;
using JffCsharpTools9.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apresentation.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddJFF(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionsString = configuration.GetConnectionString("WorkerConnection");

            // services.AddScoped(typeof(IDefaultRepository), typeof(DefaultRepository<DbContext>));
            return services;
        }
    }
}