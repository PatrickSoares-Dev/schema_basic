using System.Security.Cryptography;
using System.Text;

namespace swagger_basic.Utilities
{
    public static class Criptografia
    {
        public static string GerarHash(this string value)
        {
            var hash = SHA1.Create();
            var encode = new ASCIIEncoding();
            var array = encode.GetBytes(value);

            array = hash.ComputeHash(array);

            var result = new StringBuilder();

            foreach ( var item in array )
            {
                result.Append(item.ToString("x2"));
            }

            return result.ToString();
        }
    }
}
