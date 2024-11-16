using Microsoft.Extensions.DependencyInjection;
using DAL.DapperContext;
using BAL.Interface.ProductRepo;
using DAL.Repositry;
namespace Utility
{

    public static class DependecyRegister
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddSingleton<DapperContext>();
            services.AddScoped<IProductRepo,ProductRepo>();
        }
    }
}
