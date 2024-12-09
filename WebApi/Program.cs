using NLog.Web;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Business.Mappings;
using Shared.Extensions;
using Shared.Helpers;
using Data.Settings;
using Shared.Constants;
using NLog;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using Data.DataStore.LLM_Platform;

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
            ConfigureAppSettings(builder, environment);

            // 設定 NLog 為 Logging Provider
            ConfigureLogging(builder, environment);

            // 註冊 Web API 控制器及相關服務
            ConfigureServices(builder);

            // 取App Settings 
            var appSettings = builder.Configuration.GetSection(AppConstants.AppSettings).Get<AppSettings>();

            // MSSQL 設定
            ConfigureDatabase(builder);

            // timeZone 設定 
            var timezone = appSettings?.TimeZone ?? string.Empty;
            ConfigureTimeHelper(timezone);

            var app = builder.Build();

            // 取得 ILogger
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Application starting...");

            // 確認日誌輸出：資料庫連線字串和 AppSettings
            var LLM_PlateformConnectionString = appSettings?.DataStore.LLM_Platform.GetConnectionString();
            logger.LogInformation("LLM Plateform MSSQL connection string: {LLM_PlateformConnectionString}", LLM_PlateformConnectionString);
            logger.LogInformation("AppSettings is： {@AppSettings}", appSettings);
            logger.LogInformation("TimeZone is: {timeZone}", timezone);

            // 配置 HTTP 請求管道
            ConfigureHttpPipeline(app);

            app.Run();
        }

        private static void ConfigureAppSettings(WebApplicationBuilder builder, string environment)
        {
            builder.Configuration
                .AddYamlFile("appsettings.yaml", optional: true, reloadOnChange: true)
                .AddYamlFile($"appsettings.{environment}.yaml", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        private static void ConfigureLogging(WebApplicationBuilder builder, string environment)
        {
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(AppHelper.IsProduction(environment) ? LogLevel.Warning : LogLevel.Trace);
            builder.Host.UseNLog();

            if (!LogManager.IsLoggingEnabled())
            {
                Console.WriteLine("NLog configuration failed to load.");
            }
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // 註冊服務
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(AppConstants.AppSettings));

            // 註冊 Web API 控制器
            builder.Services.AddControllers();

            // 註冊 Swagger 設定（僅開發環境）
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 自動掃描並註冊服務
            builder.Services.AddServicesFromAttributes(Assembly.GetExecutingAssembly());

            // autoMapper 設定
            builder.Services.AddAutoMapper(typeof(MappingProfile));
        }

        private static void ConfigureDatabase(WebApplicationBuilder builder)
        {
            // Get App Settings
            var appSettings = builder.Configuration.GetSection(AppConstants.AppSettings).Get<AppSettings>();
            var LLM_PlateformConnectionString = appSettings?.DataStore.LLM_Platform.GetConnectionString();

            // 註冊 MSSQL
            builder.Services.AddDbContext<LLM_PlatformDbContext>(options =>
                options.UseSqlServer(LLM_PlateformConnectionString)
            );
        }

        private static void ConfigureHttpPipeline(WebApplication app)
        {
            // 配置 HTTP 請求管道
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }

        private static void ConfigureTimeHelper(string timeZone)
        {
            TimeHelper.Initialize(timeZone);
        }
    }
}
