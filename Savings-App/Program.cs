using Savings_App.Extensions;
using Serilog;
using Serilog.Extensions.Logging;

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



        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var loggerFactory = new SerilogLoggerFactory(Log.Logger);
        builder.Services.AddSingleton<ILoggerFactory>(loggerFactory);
        var app = builder.Build();

        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        app.ConfigureExceptionHandler(logger);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

}
