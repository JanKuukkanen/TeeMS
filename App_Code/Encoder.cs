using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Summary description for Encoder
/// </summary>
namespace TeeMs.Encoder
{
    public class Encoder
    {
        public Encoder()
        {
            
        }

        public string GetSalt()
        {
            var random = new RNGCryptoServiceProvider();

            //Maximum length of salt
            int max_length = 32;

            //Empty salt array
            byte[] salt = new byte[max_length];

            //build the random bytes
            random.GetNonZeroBytes(salt);

            // Return the string encoded salt
            return Convert.ToBase64String(salt);
        }

        public string GenerateSaltedHash(string password, string salt)
        {
            // Append the salt to the end of the password
            string passwordsalt = password + salt;

            // Calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(passwordsalt);

            byte[] hash = md5.ComputeHash(inputBytes);

            // Convert byte array to hex string
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    } 
}