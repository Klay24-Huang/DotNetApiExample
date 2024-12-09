using Data.Settings.Cors;
using Data.Settings.DataStore;

namespace Data.Settings
{
    /// <summary>
    /// 儲存應用程式設定。
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 數據存儲設定（如 MSSQL、Redis、ELK、MongoDB 等）。
        /// </summary>
        public DataStoreSettings DataStore { get; set; } = new DataStoreSettings();

        /// <summary>
        /// CORS 設定。
        /// </summary>
        public CorsSettings CORS { get; set; } = new CorsSettings();

        /// <summary>
        /// 時區
        /// </summary>
        public string TimeZone { get; set; } = "Taipei Standard Time";
    }
}
