namespace Shared.Helpers
{
    public class EncryptHelper
    {
        /// <summary>
        /// BCrypt加密字串
        /// </summary>
        /// <param name="str">要加密的文字</param>
        /// <returns>已加密的文字</returns>
        public static string BCryptHash(string str) => BCrypt.Net.BCrypt.HashPassword(str);
    }
}
