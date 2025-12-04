using FileManger_Application.Data;
using FileManger_Application.Model;
using FileManger_Application.Repositories;
using FileManger_Application.UnitOfWorks;
using Microsoft.AspNetCore.Identity;

namespace FileManger_Application.Config
{
    public static class ApplicationConfigurations
    {
        public static IServiceCollection ApplicationConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 1;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            return serviceCollection;
        }
    }
}
