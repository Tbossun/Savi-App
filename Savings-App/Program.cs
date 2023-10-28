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
using Microsoft.Extensions.Options;
using System.Reflection;
using Savings_App.Swagger;
using Microsoft.AspNetCore.Mvc;

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

        builder.Services.AddControllers(options =>
        {
            options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
                (x) => $"The value '{x}' is invalid.");
            options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
                (x) => $"The field {x} must be a number.");
            options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
                (x, y) => $"The value '{x}' is not valid for {y}.");
            options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(
                () => $"A value is required.");

            options.CacheProfiles.Add("NoCache",
                new CacheProfile() { NoStore = true });
            options.CacheProfiles.Add("Any-60",
                new CacheProfile() { Location = ResponseCacheLocation.Any, Duration = 60 });
        });
        //Register DbContext 
        builder.Services.AddDbContext<SaviContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));

        //Register Cloudinary Service
        builder.Services.AddCloudinaryExtension(builder.Configuration);
        builder.Services.AddScoped<IDocumentUploadService, DocumentUploadService>();

        //Register unitOfWork
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<IPaymentService, PaymentService>();

        //Register Identity Service
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 5;
        })
            .AddEntityFrameworkStores<SaviContext>();
        //.AddDefaultTokenProviders();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(cfg =>
            {
                cfg.WithOrigins(builder.Configuration["AllowedOrigins"]);
                cfg.AllowAnyHeader();
                cfg.AllowAnyMethod();
            });
            options.AddPolicy(name: "AnyOrigin",
                cfg =>
                {
                    cfg.AllowAnyOrigin();
                    cfg.AllowAnyHeader();
                    cfg.AllowAnyMethod();
                });
        });

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

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        //Configure swagger to user bearer authentication
        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFilename =
            $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(System.IO.Path.Combine(
                AppContext.BaseDirectory, xmlFilename));

            c.EnableAnnotations();

            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Savi.Api", Version = "v1.0" });
            

            // To Enable authorization using Swagger (JWT) 
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your valid token in the text input below \r\n\r\nExample: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
            });

            c.OperationFilter<AuthRequirementFilter>();
            c.DocumentFilter<CustomDocumentFilter>();
           // c.RequestBodyFilter<PasswordRequestFilter>();
            /*      c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                  });*/

        });

        //Configure Authententication using jwtBearer
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
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
                ClockSkew = TimeSpan.FromMinutes(30)
            };
        });

        builder.Services.AddAuthorization();

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
        else
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            // HTTP Security Headers
            app.UseHsts();
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options",
                    "sameorigin");
                context.Response.Headers.Add("X-XSS-Protection",
                    "1; mode=block");
                context.Response.Headers.Add("X-Content-Type-Options",
                    "nosniff");
                context.Response.Headers.Add("Content-Security-Policy",
                    "default-src 'self'; script-src 'self' 'nonce-23a98b38c'");
                context.Response.Headers.Add("Referrer-Policy",
                    "strict-origin");
                await next();
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

}
