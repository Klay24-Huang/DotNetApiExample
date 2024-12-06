using NLog.Web;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Business.Mappings;
using Data.Db;
using Shared.Extensions;
using Shared.Helpers;
using Data.Settings;
using Shared.Constants;
using NLog;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
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

            // 加入 YAML 配置支持
            builder.Configuration
                .AddYamlFile("appsettings.yaml", optional: true, reloadOnChange: true) // Load default yaml
                .AddYamlFile($"appsettings.{environment}.yaml", optional: true, reloadOnChange: true) // Load environment yaml
                .AddEnvironmentVariables(); // 環境變數支持

            // 設定 NLog 為 Logging Provider
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(AppHelper.IsProduction(environment) ? LogLevel.Warning : LogLevel.Trace);
            builder.Host.UseNLog();

            if (!LogManager.IsLoggingEnabled())
            {
                Console.WriteLine("NLog configuration failed to load.");
            }

            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(AppConstants.AppSettings));

            // 註冊 Web API 控制器及相關服務
            builder.Services.AddControllers();

            // 註冊 Swagger 設定（僅開發環境）
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Get App Settings
            var appSettings = builder.Configuration.GetSection(AppConstants.AppSettings).Get<AppSettings>();


            // 自動掃描並註冊服務
            builder.Services.AddServicesFromAttributes(Assembly.GetExecutingAssembly());

            // MSSQL Setting
            var LLM_PlateformConnectionString = appSettings?.DataStore.LLM_Platform.GetConnectionString();
            builder.Services.AddDbContext<TCoeusDbContext>(options =>
                options.UseSqlServer(LLM_PlateformConnectionString)
                );

            // autoMapper profile settings
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            // 取得 ILogger
            var logger = app.Services.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Application starting...");
            logger.LogInformation("LLM Plateform MSSQL contnetion string: {LLM_PlateformConnectionString}", LLM_PlateformConnectionString);
            logger.LogInformation("AppSettings is： {@AppSettings}", appSettings);

            // 配置 HTTP 請求管道
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
