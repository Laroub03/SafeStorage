using System;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyInDotNet
{
    // Class for calculating for generating keys
    public class PBKDF2
    {
        internal static byte[] GenerateSalt()
        {
            using var rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[16]; 
            rng.GetBytes(salt);
            return salt;
        }

        internal static byte[] GeneratePBKDF2Key(string password, byte[] salt, int iterations)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            return pbkdf2.GetBytes(32);
        }
        internal static bool ValidatePassword(string originalPassword, string storedSaltBase64, string storedKeyBase64, int iterations)
        {
            byte[] storedSalt = ConvertBase64ToBytes(storedSaltBase64);
            byte[] storedKey = ConvertBase64ToBytes(storedKeyBase64);

            byte[] generatedKey = GeneratePBKDF2Key(originalPassword, storedSalt, iterations);

            return CompareByteArrays(storedKey, generatedKey);
        }


        private static byte[] ConvertBase64ToBytes(string base64)
        {
            return Convert.FromBase64String(base64);
        }


        private static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
