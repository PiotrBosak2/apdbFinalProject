using System;
using System.Security.Cryptography;
using APDB_Project.Exceptions;

namespace APDB_Project.Services.Auxilary
{
    public class SecurityUtility

    {
        private const int NUMBER_OF_ITERATIONS = 5000;

        public static string SecurePassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password,salt,NUMBER_OF_ITERATIONS);
            var hash = pbkdf2.GetBytes(20);
            var hashBytes = new byte[36];
            Array.Copy(salt,0,hashBytes,0,16);
            Array.Copy(hash,0,hashBytes,16,20);
            return Convert.ToBase64String(hashBytes);
        }

        public static bool IsPasswordCorrect(string enteredPassword,string passwordFromDb)
        {
            var salt = new byte[16];
            var hashBytes = Convert.FromBase64String(passwordFromDb);
            Array.Copy(hashBytes,0,salt,0,16);
            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword,salt,NUMBER_OF_ITERATIONS);
            var hash = pbkdf2.GetBytes(20);
            for(var i = 0; i<20; ++i)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
}