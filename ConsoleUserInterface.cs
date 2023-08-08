using SafeStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeStorage
{
    // Implement the console user interface
    public class ConsoleUserInterface
    {
        private readonly ISaltGenerator _saltGenerator;
        private readonly IKeyGenerator _keyGenerator;
        private readonly IPasswordValidator _passwordValidator;

        public ConsoleUserInterface(ISaltGenerator saltGenerator, IKeyGenerator keyGenerator, IPasswordValidator passwordValidator)
        {
            _saltGenerator = saltGenerator;
            _keyGenerator = keyGenerator;
            _passwordValidator = passwordValidator;
        }

        // Helper method to convert base64 string to byte array
        private byte[] ConvertBase64ToBytes(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        // Display the main menu
        private void DisplayMenu()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("                  Login Page");
            Console.WriteLine("==================================================");
            Console.WriteLine("\n1. Login");
            Console.WriteLine("2. Change Password");
            Console.WriteLine("3. Exit");
            Console.WriteLine("Insert choice");
        }

        // Get user's choice from the menu
        private int GetUserChoice()
        {
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3)
            {
                Console.WriteLine("Invalid choice. Please select a valid option.");
            }
            return choice;
        }

        // Get masked password input from the user
        private string GetMaskedPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true); // Read a key without showing it
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*"); // Display asterisk
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Remove(password.Length - 1);
                        Console.Write("\b \b"); // Clear the last character
                    }
                }
            } while (key.Key != ConsoleKey.Enter);

            return password;
        }

        // Handle user login
        private void Login()
        {
            Console.WriteLine("Enter username: ");
            string username = Console.ReadLine();

            Console.WriteLine("Enter password: ");
            string password = GetMaskedPassword();

            byte[] salt = _saltGenerator.GenerateSalt();
            string saltBase64 = Convert.ToBase64String(salt);

            byte[] key = _keyGenerator.GenerateKey(password, salt, 10000);
            string keyBase64 = Convert.ToBase64String(key);

            bool isValidPassword = _passwordValidator.ValidatePassword(password, saltBase64, keyBase64, 10000);

            if (isValidPassword)
            {
                Console.WriteLine();
                Console.WriteLine("Login successful.");
                Console.WriteLine();
                Console.WriteLine("Username: " + username);
                Console.WriteLine("Password: " + password);
                Console.WriteLine("Salt: " + saltBase64);
                Console.WriteLine("Key: " + keyBase64);
            }
            else
            {
                Console.WriteLine("Login failed.");
            }
        }

        // Handle password change
        private void ChangePassword()
        {
            Console.WriteLine("Enter username: ");
            string username = Console.ReadLine();

            Console.WriteLine("Enter old password: ");
            string oldPassword = GetMaskedPassword();

            Console.WriteLine("Enter new password: ");
            string newPassword = GetMaskedPassword();

            byte[] newSalt = _saltGenerator.GenerateSalt();
            string newSaltBase64 = Convert.ToBase64String(newSalt);

            byte[] newKey = _keyGenerator.GenerateKey(newPassword, newSalt, 10000);
            string newKeyBase64 = Convert.ToBase64String(newKey);

            Console.WriteLine("Password changed successfully.");
        }

        // Main program loop
        public void Run()
        {
            while (true)
            {
                DisplayMenu();
                int menuChoice = GetUserChoice();

                switch (menuChoice)
                {
                    case 1:
                        Login();
                        break;
                    case 2:
                        ChangePassword();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Thanks for using this menu!");
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
