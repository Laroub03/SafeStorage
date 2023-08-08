using System;
using System.Security.Cryptography;

namespace SafeStorage
{
    // Entry point of the program
    class Program
    {
        static void Main()
        {
            // Initialize necessary components
            var saltGenerator = new PBKDF2SaltGenerator();
            var keyGenerator = new PBKDF2KeyGenerator();
            var passwordValidator = new PBKDF2PasswordValidator(keyGenerator);

            // Create and run the console user interface
            var userInterface = new ConsoleUserInterface(saltGenerator, keyGenerator, passwordValidator);
            userInterface.Run();
        }
    }
}
