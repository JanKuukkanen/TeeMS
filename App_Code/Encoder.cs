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

        private TeeMsEntities ctx;

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

            // Calculate sha512 hash from input
            SHA512 sha512 = System.Security.Cryptography.SHA512.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(passwordsalt);

            byte[] hash = sha512.ComputeHash(inputBytes);

            // Convert byte array to hex string
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public Boolean AuthenticateUser(string uname, string password)
        {
            try
            {
                ctx = new TeeMsEntities();

                // Query for the person with a username similar to the users input
                var userperson = ctx.person.Where(p => p.username == uname);
                var logininfo = ctx.login.Where(p => p.login_name == uname);

                person authenticator = userperson.FirstOrDefault();
                login loginauth = logininfo.FirstOrDefault();

                if (authenticator.username == loginauth.login_name)
                {
                    string saltedhash = GenerateSaltedHash(password, loginauth.salt);

                    if (saltedhash == loginauth.password)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    } 
}