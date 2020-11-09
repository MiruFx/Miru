using System.Security.Cryptography;
using System.Text;

namespace Miru
{
    public class Hash
    {
        public static string Create(string content)
        {
            var data = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(content));
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}