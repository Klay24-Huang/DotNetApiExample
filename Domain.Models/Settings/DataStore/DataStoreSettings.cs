namespace Data.Settings.DataStore
{
    /// <summary>
    /// 數據存儲設定。
    /// </summary>
    public class DataStoreSettings
    {
        /// <summary>
        /// LLM_Platform 資料庫設定。 MSSQL
        /// </summary>
        public MSSQL_Settings LLM_Platform { get; set; } = new MSSQL_Settings();
    }
}
