using FileManger_Application.Data;
using FileManger_Application.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(p =>
{
    p.Password.RequiredLength = 5;
    p.Password.RequireNonAlphanumeric = false;
    p.Password.RequireDigit = false;
    p.Password.RequireLowercase = false;
    p.Password.RequireUppercase = false;
    p.Password.RequiredUniqueChars = 1;

}
)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();



app.Run();
