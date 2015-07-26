namespace DataCollector
{
    using System;
    using System.Configuration;
    using System.Linq;
    using DataAccess;
    using VkNet;
    using VkNet.Enums.Filters;
    using VkNet.Exception;

    internal class Collector
    {
        private const string AuthorizationApplicationIdSettingsField = "ApplicationId";
        private const string AuthorizationEmailSettingsField = "AuthorizationEmail";
        private const string AuthorizationPasswordSettingsField = "AuthorizationPassword";
        private const string AuthorizationConsoleAuthorizationSettingsField = "ConsoleAuthorization";

        private static readonly VkApi Api = new VkApi();
        private static readonly Random Random = new Random();
        private static readonly IRepository<User> Repository = new MongoRepository<User>();

        private static void Main()
        {
            Authorize();

            const long startId = 54069025;

            var friends = Api.Friends.Get(startId);

            while (true)
            {
                var randomFriendId = friends[Random.Next(friends.Count)].Id;
                var person = Api.Users.Get(randomFriendId, ProfileFields.BirthDate);

                var birthday = person.BirthDate;
                if (birthday == null || birthday.Split('.').Length < 3)
                    continue;

                DateTime dateTimeBirthday;
                DateTime.TryParse(birthday, out dateTimeBirthday);

                var age = DateTime.Now.Year - dateTimeBirthday.Year;

                var friendsCount = 0;
                try
                {
                    var personFriends = Api.Friends.Get(person.Id);
                    if (personFriends.Any())
                    {
                        friendsCount = friends.Count;
                        friends = personFriends;
                    }
                }
                catch (AccessDeniedException)
                {
                }

                Repository.Insert(new User
                {
                    FriendsCount = friendsCount,
                    VkId = person.Id,
                    Age = age
                });
            }
        }

        private static void Authorize()
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

            Api.Authorize(applicationId, email, password, settings);
            Console.WriteLine("Authorized");
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