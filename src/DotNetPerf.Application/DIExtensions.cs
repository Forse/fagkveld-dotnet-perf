
using Microsoft.Extensions.DependencyInjection;

namespace DotNetPerf.Application
{
    public static class DIExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediator();
            return services;
        }
    }
}
