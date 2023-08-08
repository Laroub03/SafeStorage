using System;

namespace CryptographyInDotNet
{
    public class Program
    {
        static void DisplayMenu()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("                  Login Page");
            Console.WriteLine("==================================================");
            Console.WriteLine("\n1. Login");
            Console.WriteLine("2. Change Password");
            Console.WriteLine("3. Exit");
            Console.WriteLine("Insert choice");
        }

        static int GetUserChoice()
        {
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3)
            {
                Console.WriteLine("Invalid choice. Please select a valid option.");
            }
            return choice;
        }

        static void Login()
        {
            Console.WriteLine("Enter username: ");
            string username = Console.ReadLine();

            Console.WriteLine("Enter password: ");
            string password = GetMaskedPassword();

            // Hent brugerens gemte salt og nøgle fra en sikker lagermekanisme
            string storedSaltBase64 = "saved_salt_base64_here"; // Erstatt med faktiske gemte værdier
            string storedKeyBase64 = "saved_key_base64_here";

            // Valider adgangskoden
            bool isValidPassword = PBKDF2.ValidatePassword(password, storedSaltBase64, storedKeyBase64, iterations);

            if (isValidPassword)
            {
                Console.WriteLine("Login successful.");
            }
            else
            {
                Console.WriteLine("Login failed.");
            }
        }

        static void ChangePassword()
        {
            Console.WriteLine("Enter username: ");
            string username = Console.ReadLine();

            Console.WriteLine("Enter old password: ");
            string oldPassword = GetMaskedPassword();

            Console.WriteLine("Enter new password: ");
            string newPassword = GetMaskedPassword();

            // Hent brugerens gemte salt fra en sikker lagermekanisme
            string storedSaltBase64 = "saved_salt_base64_here"; // Erstatt med faktiske gemte værdier

            // Generer ny nøgle og salt for den nye adgangskode
            byte[] newSalt = PBKDF2.GenerateSalt();
            string newSaltBase64 = Convert.ToBase64String(newSalt);
            byte[] newKey = PBKDF2.GeneratePBKDF2Key(newPassword, newSalt, iterations);
            string newKeyBase64 = Convert.ToBase64String(newKey);

            // Opdater brugerens gemte salt og nøgle med de nye værdier i lageret

            Console.WriteLine("Password changed successfully.");
        }


        static void Main()
        {
            int iterations = 10000;
            byte[] salt = PBKDF2.GenerateSalt();
            string saltBase64 = Convert.ToBase64String(salt);

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
