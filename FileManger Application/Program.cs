
using FileManger_Application.Config;
using FileManger_Application.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddApplicationConfiguration();

var app = builder.Build();



app.Run();
