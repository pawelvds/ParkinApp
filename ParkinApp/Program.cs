using FluentValidation;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Extensions;
using ParkinApp.Persistence.Repositories;
using ParkinApp.Services;
using FluentValidation.AspNetCore;
using ParkinApp.Domain.DTOs;
using ParkinApp.Domain.Entities;
using ParkinApp.DTOs;
using ParkinApp.Validators;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IHostedService, CleanupService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IParkingSpotRepository, ParkingSpotRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IParkingSpotService, ParkingSpotService>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
builder.Services.AddScoped<IValidator<ParkingSpot>, ParkingSpotValidator>(); //do zmiany
builder.Services.AddScoped<IValidator<CreateReservationDto>, CreateReservationDtoValidator>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();


builder.Services.AddMemoryCache();

builder.Services.AddIdentityServices(builder.Configuration);

// Use ApplicationServiceExtensions to register DbContext and other services.
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddControllers();

    
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();