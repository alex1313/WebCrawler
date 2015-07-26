namespace WebCrawler.Services
{
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
            var result = new UsersViewModel();

            foreach (var user in users)
            {
                if (result.AgeFriendsDictionary.Keys.Contains(user.Age)) continue;

                var usersWithSameAge = users
                    .Where(x => user.Age == x.Age)
                    .ToArray();
                result.AgeFriendsDictionary.Add(user.Age, usersWithSameAge.Sum(x => x.Age)/usersWithSameAge.Count());
            }

            return result;
        }
    }
}