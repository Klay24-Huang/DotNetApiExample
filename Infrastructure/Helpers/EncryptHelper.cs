namespace Shared.Helpers
{
    public class EncryptHelper
    {
        /// <summary>
        /// BCrypt加密字串，無法解密
        /// </summary>
        /// <param name="str">要加密的文字</param>
        /// <returns>已加密的文字</returns>
        public static string HasEncrypt(string str) => BCrypt.Net.BCrypt.HashPassword(str);
        
        /// <summary>
        /// 字串加密，可解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //public static string Encrypt(string str){}

        /// <summary>
        /// 字串解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //public static string Decrypt(string str) { }
    }
}
