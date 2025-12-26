using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.API.Extensions;
using MotorbikeRental.Common.Options;
using MotorbikeRental.Infrastructure.Data.Contexts;
using MotorbikeRental.Web.Extensions;
using MotorbikeRental.Web.Middlewares;
using MotorbikeRental.Infrastructure.BackgroundJobs;
using MotorbikeRental.Application.Interface.IDataSeedingServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<MotorbikeRentalDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PrimaryDbConnection"));
});
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<AdminAccount>(builder.Configuration.GetSection("AdminAccount"));

builder.Services.AddMemoryCache();
builder.Services.AddHostedService<DiscountCleanupService>();
builder.Services.Services();
builder.Services.RegisterSecurityService(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Enter the token in the format: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header,
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();


#region Database Initialization
using var scope = app.Services.CreateScope();

var db = scope.ServiceProvider.GetRequiredService<MotorbikeRentalDbContext>();
await db.Database.MigrateAsync();

var seedingService = scope.ServiceProvider.GetRequiredService<IDataSeedingService>();
await seedingService.SeedDataAsync();
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowFrontend");

#region Custom Middlewares
app.UseExceptionHandling();
app.UseRequestResponseLogging();
#endregion

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
