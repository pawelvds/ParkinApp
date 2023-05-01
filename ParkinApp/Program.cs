using FluentValidation;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Extensions;
using ParkinApp.Persistence.Repositories;
using ParkinApp.Services;
using FluentValidation.AspNetCore;
using ParkinApp.Application.Services;
using ParkinApp.Controllers;
using ParkinApp.Domain.DTOs;
using ParkinApp.Domain.Entities;
using ParkinApp.Middlewares;
using ParkinApp.Validators;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IHostedService, CleanupExpiredReservations>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IParkingSpotRepository, ParkingSpotRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IParkingSpotService, ParkingSpotService>();
builder.Services.AddScoped<IParkingSpotCacheService, ParkingSpotCacheService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<WebSocketController>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateReservationDtoValidator>();


builder.Services.AddSingleton<IConnectionMultiplexer>(x =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));
builder.Services.AddScoped<IRedisService, RedisService>();

builder.Services.AddMemoryCache();

builder.Services.AddIdentityServices(builder.Configuration);

// Use ApplicationServiceExtensions to register DbContext and other services.
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddFluentValidationAutoValidation();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        corsPolicyBuilder => corsPolicyBuilder
            .WithOrigins("http://localhost:3000") // React app address
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Redis configuration
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseWebSockets();

app.UseRouting();
app.UseWebSocketMiddleware(async context =>
{
    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
    var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<WebSocketController>();
    var reservationService = context.RequestServices.GetService<IReservationService>();
    var webSocketController = new WebSocketController(logger, reservationService);
    await webSocketController.HandleWebSocketAsync(context, webSocket);
});

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowReactApp");

app.MapControllers();

app.Run();