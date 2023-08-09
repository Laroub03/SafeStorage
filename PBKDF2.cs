using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SafeStorage
{
    // Define interfaces for salt generation, key generation, and password validation
    public interface ISaltGenerator
    {
        byte[] GenerateSalt();
    }

    public interface IKeyGenerator
    {
        byte[] GenerateKey(string password, byte[] salt, int iterations);
    }

    public interface IPasswordValidator
    {
        bool ValidatePassword(string originalPassword, string storedSaltBase64, string storedKeyBase64, int iterations);
    }

    // Implement the PBKDF2SaltGenerator using RNGCryptoServiceProvider
    public class PBKDF2SaltGenerator : ISaltGenerator
    {
        public byte[] GenerateSalt()
        {
            using var rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[16];
            rng.GetBytes(salt);
            return salt;
        }
    }

    // Implement the PBKDF2KeyGenerator using Rfc2898DeriveBytes
    public class PBKDF2KeyGenerator : IKeyGenerator
    {
        public byte[] GenerateKey(string password, byte[] salt, int iterations)
        {
            try
            {
                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
                return pbkdf2.GetBytes(32);
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating key.", ex);
            }
        }
    }

    // Implement the PBKDF2PasswordValidator using key generator
    public class PBKDF2PasswordValidator : IPasswordValidator
    {
        private readonly IKeyGenerator _keyGenerator;

        public PBKDF2PasswordValidator(IKeyGenerator keyGenerator)
        {
            _keyGenerator = keyGenerator;
        }

        public bool ValidatePassword(string originalPassword, string storedSaltBase64, string storedKeyBase64, int iterations)
        {
            try
            {
                byte[] storedSalt = Convert.FromBase64String(storedSaltBase64);
                byte[] storedKey = Convert.FromBase64String(storedKeyBase64);

                byte[] generatedKey = _keyGenerator.GenerateKey(originalPassword, storedSalt, iterations);

                return CompareByteArrays(storedKey, generatedKey);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
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
