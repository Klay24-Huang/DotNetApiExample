using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Infrastrructure.Helpers
{
    /// <summary>
    /// 幫助載入 YAML 配置的輔助類別，支援多層次合併與屬性檢查。
    /// </summary>
    /// <remarks>
    /// 初始化 ConfigurationHelper 類別。
    /// </remarks>
    /// <param name="logger">用於記錄訊息的日誌記錄器。</param>
    public class ConfigurationHelper(ILogger<ConfigurationHelper> logger)
    {
        private readonly ILogger<ConfigurationHelper> _logger = logger;

        /// <summary>
        /// 載入 YAML 配置檔案，根據環境名稱合併多層配置。
        /// 支援任何類型的配置類別。
        /// </summary>
        /// <typeparam name="T">目標配置類型。</typeparam>
        /// <param name="environment">當前執行的環境名稱（例如 Development, Production）。</param>
        /// <returns>合併後的配置物件，類型為 T。</returns>
        public T LoadYamlConfiguration<T>(string environment)
        {
            var defaultYamlPath = "appsettings.yaml";
            var environmentYamlPath = $"appsettings.{environment}.yaml";

            var finalSettings = Activator.CreateInstance<T>();

            // 加載默認 YAML 檔案
            if (File.Exists(defaultYamlPath))
            {
                var defaultSettings = ParseYamlFile<T>(defaultYamlPath);
                finalSettings = MergeSettings(finalSettings, defaultSettings);
            }
            else
            {
                _logger.LogError("配置文件缺失: {FilePath}", defaultYamlPath);
            }

            // 加載環境特定的 YAML 檔案
            if (File.Exists(environmentYamlPath))
            {
                var environmentSettings = ParseYamlFile<T>(environmentYamlPath);
                finalSettings = MergeSettings(finalSettings, environmentSettings);
            }
            else
            {
                _logger.LogWarning("環境特定的配置文件不存在: {FilePath}", environmentYamlPath);
            }

            // 檢查是否有缺失的配置屬性
            CheckForMissingSettings(finalSettings);

            return finalSettings;
        }

        /// <summary>
        /// 解析 YAML 檔案內容並反序列化為指定的配置類型。
        /// </summary>
        /// <typeparam name="T">目標配置類型。</typeparam>
        /// <param name="filePath">YAML 檔案的完整路徑。</param>
        /// <returns>解析後的配置物件，類型為 T。</returns>
        private T ParseYamlFile<T>(string filePath)
        {
            try
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var yamlContent = File.ReadAllText(filePath);
                return deserializer.Deserialize<T>(yamlContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析 YAML 文件時發生錯誤: {FilePath}", filePath);
                throw;
            }
        }

        /// <summary>
        /// 檢查配置類型中是否有未設置的屬性，並記錄警告。
        /// </summary>
        /// <typeparam name="T">配置類型。</typeparam>
        /// <param name="settings">需要檢查的配置物件。</param>
        private void CheckForMissingSettings<T>(T settings)
        {
            if (settings == null)
            {
                _logger.LogWarning("配置物件為 null，無法檢查屬性。");
                return;
            }

            var missingProperties = settings.GetType()
                .GetProperties()
                .Where(p => p.GetValue(settings) == null)
                .Select(p => p.Name)
                .ToList();

            if (missingProperties.Count != 0)
            {
                _logger.LogWarning("以下配置屬性缺失: {MissingProperties}", string.Join(", ", missingProperties));
            }
        }


        /// <summary>
        /// 合併兩個配置物件的屬性值。
        /// </summary>
        /// <typeparam name="T">配置物件類型。</typeparam>
        /// <param name="baseSettings">基礎配置物件。</param>
        /// <param name="newSettings">需要合併的配置物件。</param>
        /// <returns>合併後的配置物件。</returns>
        private static T MergeSettings<T>(T baseSettings, T newSettings)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                var baseValue = property.GetValue(baseSettings);
                var newValue = property.GetValue(newSettings);

                if (baseValue == null && newValue != null)
                {
                    property.SetValue(baseSettings, newValue);
                }
            }

            return baseSettings;
        }
    }
}
