using ECommerce.API.Middleware;
using ECommerce.Application.DependencyInjection;
using ECommerce.Application.Orders.Commands.CheckoutOrder;
using ECommerce.Infrastructure.DependencyInjection;
using ECommerce.Infrastructure.Persistence;
using MediatR;
using StackExchange.Redis;
using ECommerce.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("Redis");

    return ConnectionMultiplexer.Connect(connectionString!);
});

builder.Services.AddScoped<ICacheService, RedisCacheService>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CheckoutOrderCommand>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(); 

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
app.MapHub<ChatHub>("/hubs/chat");

public partial class Program
{
}
