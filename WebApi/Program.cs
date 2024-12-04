using Domain.Models.Others;
using Infrastrructure.Helpers;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 讀取環境變數
            var environment = builder.Environment.EnvironmentName;  // 這是 .NET 內建的環境變數

            // 註冊 ConfigurationHelper
            builder.Services.AddSingleton<ConfigurationHelper>();

            // 使用 ConfigurationHelper 加載 YAML 配置並直接返回 AppSettings
            builder.Services.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ConfigurationHelper>>();
                var configurationHelper = new ConfigurationHelper(logger);

                // 使用泛型載入指定配置（例如 AppSettings）
                return configurationHelper.LoadYamlConfiguration<AppSettings>(environment);
            });

            // 註冊 AppSettings 配置，基於加載的 YAML 配置
            builder.Services.Configure<AppSettings>(builder.Configuration);

            // 註冊日誌和其他服務
            builder.Services.AddLogging();

            // 註冊 Web API 控制器及相關服務
            builder.Services.AddControllers();

            // 註冊 Swagger 設定（僅開發環境）
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
