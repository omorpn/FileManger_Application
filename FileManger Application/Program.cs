

using FileManger_Application.Config;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationConfiguration(builder.Configuration);
var app = builder.Build();


app.UseRouting();
app.UseAuthentication();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();


app.Run();
