
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

var connString = "";
if (builder.Environment.IsDevelopment())
	connString = builder.Configuration.GetConnectionString("DefaultConnection");
else
{
	// Use connection string provided at runtime by FlyIO.
	var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

	// Parse connection URL to connection string for Npgsql
	connUrl = connUrl.Replace("postgres://", string.Empty);
	var pgUserPass = connUrl.Split("@")[0];
	var pgHostPortDb = connUrl.Split("@")[1];
	var pgHostPort = pgHostPortDb.Split("/")[0];
	var pgDb = pgHostPortDb.Split("/")[1];
	var pgUser = pgUserPass.Split(":")[0];
	var pgPass = pgUserPass.Split(":")[1];
	var pgHost = pgHostPort.Split(":")[0];
	var pgPort = pgHostPort.Split(":")[1];
	var updatedHost = pgHost.Replace("flycast", "internal");

	connString = $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";
}
builder.Services.AddDbContext<DataContext>(opt =>
{
	opt.UseNpgsql(connString);
});


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

//Before preparing for deployment
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
//Preparing for deployment
app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try {     
    var context = services.GetRequiredService<DataContext>();
	var userManager = services.GetRequiredService<UserManager<AppUser>>();
	var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
	await context.Database.MigrateAsync();
	//await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Connections]");
	await Seed.ClearConnections(context);
	await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();

