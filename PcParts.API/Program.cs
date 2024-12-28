using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web.Resource;
using PcParts.API.DAL;
using PcParts.API.Data;
using SolarFlare.Data;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddDbContext<BotContext>(options => options.UseSqlServer(connectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddAzureWebAppDiagnostics();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var botContext = scope.ServiceProvider.GetRequiredService<BotContext>();
    DBInitializer.Initialize(botContext);
    
    botContext.Database.EnsureCreated();
    botContext.Database.Migrate();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();