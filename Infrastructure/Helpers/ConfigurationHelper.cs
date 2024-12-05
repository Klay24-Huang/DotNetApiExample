using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace Infrastructure.Helpers
{
    /// <summary>
    /// 幫助載入 YAML 配置的輔助類別，支援多層次合併與屬性檢查。
    /// </summary>
    public class ConfigurationHelper
    {
        private readonly ILogger<ConfigurationHelper> _logger;

        public ConfigurationHelper(ILogger<ConfigurationHelper> logger)
        {
            _logger = logger;
        }

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
                var deserializer = new DeserializerBuilder().Build();
                var yamlContent = File.ReadAllText(filePath);

                _logger.LogInformation("讀取的 YAML 文件內容為: {YamlContent}", yamlContent);

                // 使用 YAML 解析器提取文件中的所有屬性名稱
                var yamlObject = new Deserializer().Deserialize<Dictionary<string, object>>(yamlContent);
                var yamlProperties = yamlObject?.Keys ?? Enumerable.Empty<string>();

                // 提取類型 T 中的所有屬性名稱
                var typeProperties = typeof(T).GetProperties()
                    .Where(p => p.CanWrite)
                    .Select(p => p.Name);

                // 找出缺少的屬性
                var missingProperties = typeProperties.Except(yamlProperties, StringComparer.OrdinalIgnoreCase).ToList();

                if (missingProperties.Count > 0)
                {
                    _logger.LogWarning(
                        "YAML 文件缺少以下配置類型 {ConfigType} 的屬性: {MissingProperties}",
                        typeof(T).Name,
                        string.Join(", ", missingProperties)
                    );
                }

                // 反序列化為目標對象
                return deserializer.Deserialize<T>(yamlContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析 YAML 文件時發生錯誤: {FilePath}", filePath);
                throw;
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

                if (newValue != null)
                {
                    property.SetValue(baseSettings, newValue);
                }
            }

            return baseSettings;
        }
    }
}
