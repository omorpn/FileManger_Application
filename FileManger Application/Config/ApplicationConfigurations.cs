using FileManger_Application.Data;
using FileManger_Application.Model;
using FileManger_Application.Repositories;
using FileManger_Application.ServiceContract;
using FileManger_Application.Services;
using FileManger_Application.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

namespace FileManger_Application.Config
{
    public static class ApplicationConfigurations
    {

        public static IServiceCollection ApplicationConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)

        {
            serviceCollection.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("Default")));
            serviceCollection.AddControllersWithViews(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            });
            serviceCollection.AddAuthorization(opt =>
            {
                opt.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });


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
            serviceCollection.AddScoped<UserContract, UserService>();
            serviceCollection.AddScoped<FileContract, FileService>();
            serviceCollection.AddScoped<FolderContract, FolderService>();
            serviceCollection.AddScoped<SharedContract, ShareService>();
            serviceCollection.AddScoped<IStorageContract, StorageService>();
            serviceCollection.AddScoped<IApplicationSettingContract, ApplicationSettingService>();
            return serviceCollection;
        }
    }
}
