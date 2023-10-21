using System.Text;
using Dat_api.Data;
using Dat_api.Extentions;
using Dat_api.Interfaces;
using Dat_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

//After line on the top
app.UseAuthentication(); //Asks do you have an authentication? Valid id?
app.UseAuthorization(); //Checks if the user is authorized to do this request, valid token?
//Until here

app.MapControllers();

app.Run();

