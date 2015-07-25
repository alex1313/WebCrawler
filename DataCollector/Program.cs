namespace DataCollector
{
    using System;
    using System.Configuration;
    using VkNet;
    using VkNet.Enums.Filters;

    internal class Program
    {
        private const string AuthorizationApplicationIdSettingsField = "ApplicationId";
        private const string AuthorizationEmailSettingsField = "AuthorizationEmail";
        private const string AuthorizationPasswordSettingsField = "AuthorizationPassword";
        private const string AuthorizationConsoleAuthorizationSettingsField = "ConsoleAuthorization";

        private static void Main()
        {
            var appSettings = ConfigurationManager.AppSettings;
            string email;
            string password;

            var isConsoleAuthorization = bool.Parse(appSettings[AuthorizationConsoleAuthorizationSettingsField]);
            if (isConsoleAuthorization)
            {
                ConsoleAuthorization(out email, out password);
            }
            else
            {
                Console.WriteLine("Authorization data used from App.config file.");
                email = appSettings[AuthorizationEmailSettingsField];
                password = appSettings[AuthorizationPasswordSettingsField];
            }

            var applicationId = int.Parse(appSettings[AuthorizationApplicationIdSettingsField]);
            var settings = Settings.All;

            var api = new VkApi();
            api.Authorize(applicationId, email, password, settings);
            Console.WriteLine("\nAuthorized");
        }

        private static void ConsoleAuthorization(out string email, out string password)
        {
            Console.Write("Enter your email: ");
            email = Console.ReadLine();

            Console.Write("Enter your password: ");
            password = string.Empty;
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key != ConsoleKey.Backspace || password.Length <= 0) continue;
                    password = password.Substring(0, (password.Length - 1));
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
        }
    }
}