using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Microsoft.Extensions.Logging;

namespace Infrastrructure.Helpers
{
    /// <summary>
    /// 提供處理 YAML 配置文件的幫助方法，包括加載、合併和驗證設定。
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// 從指定的 YAML 文件加載配置，並反序列化為指定類型。
        /// </summary>
        /// <typeparam name="T">配置的類型，必須具有無參構造函數。</typeparam>
        /// <param name="filePath">YAML 文件的路徑。</param>
        /// <param name="logger">用於記錄日誌的 <see cref="ILogger"/> 實例。</param>
        /// <param name="optional">如果為 true，則忽略文件缺失的情況並返回預設值；如果為 false，則在文件缺失時拋出異常。</param>
        /// <returns>反序列化後的設定實例。如果文件缺失且 optional 為 true，則返回一個新的實例。</returns>
        /// <exception cref="FileNotFoundException">當文件未找到且 optional 為 false 時，拋出此異常。</exception>
        public static T LoadYaml<T>(string filePath, ILogger logger, bool optional = false) where T : new()
        {
            // 檢查檔案是否存在
            if (!File.Exists(filePath))
            {
                if (optional)
                {
                    // 如果檔案不存在，並且標註為 optional，則返回一個預設值
                    logger.LogWarning($"檔案 {filePath} 不存在，將使用預設設定");
                    return new T();  // 返回一個新的空物件
                }
                else
                {
                    throw new FileNotFoundException($"配置檔案 {filePath} 未找到");
                }
            }

            try
            {
                var yaml = File.ReadAllText(filePath);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance) // 使用駝峰命名
                    .IgnoreUnmatchedProperties() // 忽略不匹配的屬性
                    .Build();

                // 反序列化 YAML
                var result = deserializer.Deserialize<T>(yaml);

                // 檢查是否有必要的屬性
                ValidateProperties(result, filePath, logger);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "無法加載 YAML 文件: {FilePath}", filePath);
                return new T();
            }
        }

        /// <summary>
        /// 驗證設定對象中是否有缺少的必要屬性。
        /// </summary>
        /// <typeparam name="T">配置的類型。</typeparam>
        /// <param name="settings">反序列化後的配置實例。</param>
        /// <param name="filePath">YAML 文件的路徑。</param>
        /// <param name="logger">用於記錄日誌的 <see cref="ILogger"/> 實例。</param>
        private static void ValidateProperties<T>(T settings, string filePath, ILogger logger)
        {
            var missingProperties = new System.Collections.Generic.List<string>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(settings);
                if (value == null)
                {
                    missingProperties.Add(property.Name);
                }
            }

            if (missingProperties.Count > 0)
            {
                logger.LogWarning("文件 {FilePath} 中缺少以下必要屬性: {MissingProperties}", filePath, string.Join(", ", missingProperties));
            }
        }

        /// <summary>
        /// 合併兩個設定對象，將第二個設定的非空屬性值覆蓋到第一個設定中。
        /// </summary>
        /// <typeparam name="T">設定的類型。</typeparam>
        /// <param name="baseSettings">基礎設定對象。</param>
        /// <param name="overrideSettings">覆蓋設定對象。</param>
        /// <returns>合併後的設定對象。</returns>
        public static T MergeSettings<T>(T baseSettings, T overrideSettings)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                var overrideValue = property.GetValue(overrideSettings);
                if (overrideValue != null)
                {
                    property.SetValue(baseSettings, overrideValue);
                }
            }
            return baseSettings;
        }
    }
}
