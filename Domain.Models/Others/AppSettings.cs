using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Others
{
    /// <summary>
    /// 設定應用程序的配置，包括日誌設置和允許的主機。
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 獲取或設置日誌設置。
        /// </summary>
        public LoggingSettings Logging { get; set; } = new LoggingSettings();  // 設定預設值

        /// <summary>
        /// 獲取或設置允許的主機名。
        /// </summary>
        public string AllowedHosts { get; set; } = "*";  // 設定預設值
    }

    /// <summary>
    /// 設定日誌的詳細設置，包括日誌等級。
    /// </summary>
    public class LoggingSettings
    {
        /// <summary>
        /// 獲取或設置日誌等級設置。
        /// </summary>
        public LogLevelSettings LogLevel { get; set; } = new LogLevelSettings();  // 設定預設值
    }

    /// <summary>
    /// 設定日誌等級，定義了不同組件的日誌級別。
    /// </summary>
    public class LogLevelSettings
    {
        /// <summary>
        /// 獲取或設置默認的日誌等級。
        /// </summary>
        public string Default { get; set; } = "Information";  // 設定預設值

        /// <summary>
        /// 獲取或設置 Microsoft.AspNetCore 的日誌等級。
        /// </summary>
        public string MicrosoftAspNetCore { get; set; } = "Warning";  // 設定預設值
    }

}
