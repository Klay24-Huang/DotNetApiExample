using Domain.Models.Others;
using Microsoft.Extensions.Options;

namespace WebApi.Services
{
    /// <summary>
    /// 監聽應用程式設定變更的服務，並記錄任何缺少或未設置的屬性。
    /// </summary>
    public class AppSettingsMonitorService : IHostedService
    {
        private readonly IOptionsMonitor<AppSettings> _optionsMonitor; // 用來監控 AppSettings 配置的變更
        private readonly ILogger<AppSettingsMonitorService> _logger; // 用來記錄變更和警告的日誌

        /// <summary>
        /// 建構子，注入所需的 IOptionsMonitor 和 ILogger 服務。
        /// </summary>
        /// <param name="optionsMonitor">用來監控 AppSettings 配置的變更</param>
        /// <param name="logger">用來記錄變更和警告的日誌</param>
        public AppSettingsMonitorService(IOptionsMonitor<AppSettings> optionsMonitor, ILogger<AppSettingsMonitorService> logger)
        {
            _optionsMonitor = optionsMonitor;
            _logger = logger;
        }

        /// <summary>
        /// 啟動服務時，開始監控 AppSettings 的變更，並對缺少的屬性進行警告。
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // 監聽 AppSettings 配置變更
            _optionsMonitor.OnChange(updatedSettings =>
            {
                // 檢查有沒有未設置或為 null 的屬性
                var propertiesWithMissingValues = updatedSettings.GetType().GetProperties()
                    .Where(p => p.GetValue(updatedSettings) == null)
                    .Select(p => p.Name)
                    .ToList();

                // 如果有缺少的屬性，記錄警告
                if (propertiesWithMissingValues.Count != 0)
                {
                    _logger.LogWarning("配置中缺少以下屬性: {MissingProperties}", string.Join(", ", propertiesWithMissingValues));
                }

                // 記錄更新的設定
                _logger.LogInformation("AppSettings 已更新，時間: {Time}", DateTime.Now);
            });

            return Task.CompletedTask;
        }

        /// <summary>
        /// 停止服務時的操作，目前沒有額外的停止邏輯。
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
