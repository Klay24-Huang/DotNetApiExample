namespace Data.Settings.DataStore
{
    public class MSSQL_Settings
    {
        /// <summary>
        /// 資料庫伺服器 URL。
        /// </summary>
        public string Server { get; set; } = "localhost";

        /// <summary>
        /// 資料庫名稱。
        /// 預設值為 "your_database_name"。
        /// </summary>
        public string DatabaseName { get; set; } = "LLM_Plateform";

        /// <summary>
        /// 使用者名稱。
        /// </summary>
        public string UserName { get; set; } = "sa";

        /// <summary>
        /// 密碼。
        /// </summary>
        public string Password { get; set; } = "your_password";

        /// <summary>
        /// 資料庫連接埠。
        /// 預設值為 1433 (MSSQL 預設埠)。
        /// </summary>
        public int Port { get; set; } = 1433;

        /// <summary>
        /// 取得 MSSQL 連接字串。
        /// 根據設定中的資料庫伺服器、資料庫名稱、使用者名稱、密碼和連接埠來建立連接字串。
        /// </summary>
        /// <returns>返回格式化的 MSSQL 連接字串。</returns>
        public string GetConnectionString()
        {
            // 格式化並組裝 MSSQL 連接字串
            return $"Server={Server},{Port};Database={DatabaseName};User Id={UserName};Password={Password};";
        }
    }
}
