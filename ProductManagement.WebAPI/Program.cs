using ProductManagement.Infrastructure;
using ProductManagement.Persistence;
using ProductManagement.WebAPI;
using ProductManagement.WebAPI.Filters;
using ProductManagement.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddControllers(c =>
    {
        c.Filters.Add<TransactionFilter>();
        c.Filters.Add<ValidationFilter>();
    });

builder.Services.AddPersistenceLayerServices(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddAPIServices(builder.Configuration,builder.Host, builder);


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