using System.Collections.ObjectModel;
using System.Data;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Polly;
using ProductManagement.Core.Consts;
using ProductManagement.Infrastructure;
using ProductManagement.Persistence;
using ProductManagement.Persistence.Contexts;
using ProductManagement.WebAPI;
using ProductManagement.WebAPI.Filters;
using ProductManagement.WebAPI.Middlewares;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddControllers(c =>
    {
        c.Filters.Add<TransactionFilter>();
        c.Filters.Add<ValidationFilter>();
    });


var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});


builder.Services.AddPersistenceLayerServices(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddAPIServices(builder.Configuration, builder.Host, builder);


builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddTransient<RolePermissionMiddleware>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

await DataSeeder.InitializeAsync(builder.Services.BuildServiceProvider());


app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseMiddleware<RolePermissionMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();