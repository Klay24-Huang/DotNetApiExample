//using Xunit;
//using Moq;
//using Xunit.Abstractions;
//using Shared.Helpers;
//using Domain.Models.Others;
//using Microsoft.Extensions.Logging;

//namespace SharedTests.Helpers
//{
//    public class ConfigurationHelperTests
//    {
//        private readonly Mock<ILogger<ConfigurationHelper>> _mockLogger;
//        private readonly ConfigurationHelper _configurationHelper;
//        private readonly ITestOutputHelper _output;

//        public ConfigurationHelperTests(ITestOutputHelper outputHelper)
//        {
//            _mockLogger = new Mock<ILogger<ConfigurationHelper>>();
//            _configurationHelper = new ConfigurationHelper(_mockLogger.Object);
//            _output = outputHelper;
//        }

//        [Fact]
//        public void LoadYamlConfiguration_ShouldLoadDefaultAndEnvironmentConfigs()
//        {
//            // Arrange: 創建臨時的 YAML 配置文件
//            var environment = "Development";
//            var appSettingsYamlContent = @"
//                Logging:
//                  LogLevel:
//                    Default: 'Information'
//                    MicrosoftAspNetCore: 'Warning'
//                AllowedHosts: '*'";

//            var environmentYamlContent = @"
//                Logging:
//                  LogLevel:
//                    Default: 'Error'
//                AllowedHosts: 'localhost'";

//            var defaultYamlPath = "appsettings.yaml";
//            var environmentYamlPath = $"appsettings.{environment}.yaml";

//            // 使用 File.WriteAllText 創建臨時 YAML 文件
//            File.WriteAllText(defaultYamlPath, appSettingsYamlContent);
//            File.WriteAllText(environmentYamlPath, environmentYamlContent);

//            // Act: 加載配置
//            var appSettings = _configurationHelper.LoadYamlConfiguration<AppSettings>(environment);

//            // Assert: 驗證配置是否正確加載
//            Assert.NotNull(appSettings);
//            Assert.Equal("localhost", appSettings.AllowedHosts); // 驗證 AllowedHosts
//            Assert.Equal("Error", appSettings.Logging.LogLevel.Default); // 驗證 Logging 的 Default 等級
//            Assert.Equal("Warning", appSettings.Logging.LogLevel.MicrosoftAspNetCore); // 驗證 Logging 的 MicrosoftAspNetCore 等級 (沒被覆蓋)

//            // 清理測試文件
//            File.Delete(defaultYamlPath);
//            File.Delete(environmentYamlPath);
//        }

//        [Fact]
//        public void LoadYamlConfiguration_ShouldLogErrorWhenFileMissing()
//        {
//            // Arrange
//            var environment = "NonExistingEnvironment";
//            var mockLogger = new Mock<ILogger<ConfigurationHelper>>();
//            mockLogger.Setup(logger => logger.Log(
//                    LogLevel.Error,
//                    It.IsAny<EventId>(),
//                    It.IsAny<It.IsAnyType>(),
//                    It.IsAny<Exception>(),
//                    It.IsAny<Func<It.IsAnyType, Exception, string>>()))
//                .Verifiable();

//            var configurationHelper = new ConfigurationHelper(mockLogger.Object);

//            // Act
//            configurationHelper.LoadYamlConfiguration<AppSettings>(environment);

//            // Assert: 確保日誌中記錄了錯誤
//            mockLogger.Verify();
//        }

//        [Fact]
//        public void LoadYamlConfiguration_ShouldLogWarningWhenSettingsAreMissing()
//        {
//            // Arrange: 通過公共方法，這裡模擬 `CheckForMissingSettings` 進行測試
//            var lostPropertyYamlContent = "AllowedHosts: 'localhost'";
//            var environment = "lostPropertyTest";
//            var lostPropertyYamlPath = $"appsettings.{environment}.yaml";

//            var appSettingYamlContent = "AllowedHosts: 'localhost'";
//            var defaultYamlPath = "appsettings.yaml";

//            // 創建測試yaml
//            File.WriteAllText(lostPropertyYamlPath, lostPropertyYamlContent);
//            File.WriteAllText(defaultYamlPath, appSettingYamlContent);

//            var mockLogger = new Mock<ILogger<ConfigurationHelper>>();
//            mockLogger.Setup(logger => logger.Log(
//                    LogLevel.Warning,
//                    It.IsAny<EventId>(),
//                    It.IsAny<It.IsAnyType>(),
//                    It.IsAny<Exception>(),
//                    It.IsAny<Func<It.IsAnyType, Exception, string>>()))
//                .Verifiable();

//            var configurationHelper = new ConfigurationHelper(mockLogger.Object);

//            // Act: 假設 `LoadYamlConfiguration` 中有處理缺少設置的邏輯
//            configurationHelper.LoadYamlConfiguration<AppSettings>(environment); // 這會調用 `CheckForMissingSettings`

//            // Assert: 確保日誌中記錄了警告
//            mockLogger.Verify();

//            // 清除測試文件
//            File.Delete(lostPropertyYamlPath);
//            File.Delete(defaultYamlPath);
//        }
//    }
//}
