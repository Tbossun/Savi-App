using Savings_App.Extensions;
using SavingsApp.Core.Services;
using SavingsApp.Core.Services.Implementations;
using SavingsApp.Core.Services.Interfaces;
using Serilog;
using Serilog.Extensions.Logging;
using SavingsApp.Core.Services.Implementations;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using SavingsApp.Data.Context;
using SavingsApp.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;
using SavingsApp.Data.Repositories.IRepositories;
using SavingsApp.Data.UnitOfWork;
using SavingsApp.Data.Seeding;
using AutoMapper;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;


        // Add services to the container.
        //Register Logger services
        Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/SaviLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();
        builder.Host.UseSerilog();

        //Register DbContext 
        builder.Services.AddDbContext<SaviContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));

        //Register Cloudinary Service
        builder.Services.AddCloudinaryExtension(builder.Configuration);
        builder.Services.AddScoped<IDocumentUploadService, DocumentUploadService>();

        //Register unitOfWork
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<IPaymentService, PaymentService>();



        //Register Email  Service
        var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        builder.Services.AddSingleton(emailConfig);
        builder.Services.AddScoped<IEmailService, EmailService>();

        //Register PaystackService
        //builder.Services.AddSingleton<PaystackService>(provider => new PaystackService("sk_test_c1e3948a98584d332e3cebf256c84d13e915e2fe"));
        // Register the PaystackService with the required dependencies.
        builder.Services.AddScoped<PaystackService>(provider =>
        {
            var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            var apiKey = "sk_test_c1e3948a98584d332e3cebf256c84d13e915e2fe"; // Replace this with your actual Paystack API key.
           // var mapper = provider.GetRequiredService<IMapper>();
            return new PaystackService(apiKey, unitOfWork);
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        //Configure swagger to user bearer authentication
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Savi.Api", Version = "v1" });

            // To Enable authorization using Swagger (JWT) 
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token in the text input below \r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
            });

        });

        //Configure Authententication using jwtBearer
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
                ClockSkew = TimeSpan.FromSeconds(30)
            };
        });

        //Register Identity Service
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<SaviContext>()
                .AddDefaultTokenProviders();

        //Register Automapper service
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        var loggerFactory = new SerilogLoggerFactory(Log.Logger);
        builder.Services.AddSingleton<ILoggerFactory>(loggerFactory);



        var app = builder.Build();

        /*var logger = app.Services.GetRequiredService<ILogger<Program>>();
        app.ConfigureExceptionHandler(logger);*/

        //configure seeder
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SaviContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            // Seed the data
            Seeder.SeedDataBase(roleManager, userManager, dbContext);
        }

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
    }

}
