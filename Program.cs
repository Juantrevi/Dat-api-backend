
using Dat_api.Data;
using Dat_api.Entities;
using Dat_api.Extentions;
using Dat_api.Middleware;
using Dat_api.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
.WithOrigins("http://localhost:4200"));

//After line on the top
app.UseAuthentication(); //Asks do you have an authentication? Valid id?
app.UseAuthorization(); //Checks if the user is authorized to do this request, valid token?
//Until here

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try {     
    var context = services.GetRequiredService<DataContext>();
	var userManager = services.GetRequiredService<UserManager<AppUser>>();
	var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
	await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();

