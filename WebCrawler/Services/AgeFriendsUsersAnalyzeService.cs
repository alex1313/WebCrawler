namespace WebCrawler.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess;
    using Models;

    internal class AgeFriendsUsersAnalyzeService : IUsersAnalyzeService
    {
        private readonly IRepository<User> _repository;

        public AgeFriendsUsersAnalyzeService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public UsersViewModel GetUsersDetails()
        {
            var users = _repository.GetAll();
            var result = new Dictionary<int, int>();

            foreach (var user in users)
            {
                if (result.Keys.Contains(user.Age)) continue;

                var usersWithSameAge = users
                    .Where(x => user.Age == x.Age)
                    .ToArray();
                result.Add(user.Age, usersWithSameAge.Sum(x => x.FriendsCount)/usersWithSameAge.Count());
            }
            var sortedResult = result.OrderBy(x => x.Key);
            result = sortedResult.ToDictionary(x => x.Key, x => x.Value);

            return new UsersViewModel { AgeFriendsDictionary = result };
        }
    }
}