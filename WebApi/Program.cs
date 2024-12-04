
using Domain.Models.Others;
using Infrastrructure.Helpers;
using Microsoft.Extensions.Options;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 讀取環境變數
            var environment = builder.Environment.EnvironmentName;  // 這是 .NET 內建的環境變數

            builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddYamlFile("appsettings.yaml", optional: true, reloadOnChange: true) // 加載默認的配置文件
            .AddYamlFile($"appsettings.{environment}.yaml", optional: true, reloadOnChange: true); // 加載環境特定的配置文件


            // 注冊配置並監聽變更
            builder.Services.Configure<AppSettings>(builder.Configuration);

            // 注冊後台服務，監聽配置變更並記錄日志
            builder.Services.AddSingleton<IHostedService, AppSettingsMonitorService>();

            // Register logging and other services
            builder.Services.AddLogging();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
}
