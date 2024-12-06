using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Settings.Cors
{
    /// <summary>
    /// CORS 設定。
    /// </summary>
    public class CorsSettings
    {
        /// <summary>
        /// 允許的來源列表。
        /// 預設值為 "*"，表示允許所有來源。
        /// </summary>
        public List<string> AllowedOrigins { get; set; } = ["*"];
    }
}
