using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastrructure.Helpers
{
    /// <summary>
    /// 提供配置設置的幫助方法，包括從 YAML 檔案加載並合併設定。
    /// </summary>
    public static class AppSettingHelper
    {
        /// <summary>
        /// 根據指定的環境名稱從 YAML 檔案加載設定並合併，然後返回合併後的設定。
        /// </summary>
        /// <typeparam name="T">設定的類型，必須是具有無參構造函數的類型。</typeparam>
        /// <param name="environment">當前運行環境的名稱，例如 "development" 或 "production"。</param>
        /// <param name="logger">用於記錄日誌的 <see cref="ILogger"/> 實例。</param>
        /// <returns>返回合併後的設定。</returns>
        /// <exception cref="ArgumentNullException">當參數為 null 時，拋出此異常。</exception>
        public static T ConfigureAppSettings<T>(string environment, ILogger logger) where T : class, new()
        {
            // 加載預設檔案，如果檔案不存在則不拋出異常
            var exampleSettings = ConfigurationHelper.LoadYaml<T>("appsetting.yaml", logger, optional: true);
            // 加載環境特定的設定
            var environmentSettings = ConfigurationHelper.LoadYaml<T>($"appsetting.{environment}.yaml", logger, optional: true);

            // 合併設定，這裡會自動處理 null 情況
            var finalSettings = ConfigurationHelper.MergeSettings(exampleSettings, environmentSettings);

            return finalSettings;  // 返回合併後的設定
        }
    }

}
