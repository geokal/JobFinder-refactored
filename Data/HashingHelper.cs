using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace QuizManager.Data
{
    public static class HashingHelper
    {
        public static string HashLong(long number)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(number.ToString());
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                return ConvertToAscii(hashBytes);
            }
        }

        public static string HashString(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return ConvertToAscii(hashBytes);
            }
        }

        private static string ConvertToAscii(byte[] hashBytes)
        {
            byte[] truncated = hashBytes.Take(8).ToArray();

            string base64 = Convert.ToBase64String(truncated)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

            return base64;
        }

        public static string HashLong(long? number)
        {
            if (!number.HasValue) return string.Empty;
            return HashLong(number.Value);
        }
    }
}