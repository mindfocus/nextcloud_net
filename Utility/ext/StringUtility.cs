using System;
using System.Text;

namespace ext
{
    public static class StringUtility
    {
        public static bool IsEmpty(this string input)
        {
            return input == String.Empty;
        }
        public static bool IsNotEmpty(this string input)
        {
            return !input.IsEmpty();
        }

        public static string MD5ext(this string input)
        {
            return MD5(input);
        }
        public static string MD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}