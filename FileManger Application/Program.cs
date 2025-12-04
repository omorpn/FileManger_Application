using FileManger_Application.Data;
using FileManger_Application.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


var app = builder.Build();

app.Services.Applicatio

app.Run();
