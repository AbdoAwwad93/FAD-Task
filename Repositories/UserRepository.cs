using System.Collections.Generic;
using System.Linq;
using FadTask.Models;

namespace FadTask.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();
        private int _nextId = 1;

        public User? GetByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }

        public void Add(User user)
        {
            user.Id = _nextId++;
            _users.Add(user);
        }

        public bool ValidateCredentials(string email, string password)
        {
            var u = GetByEmail(email);
            if (u == null) return false;
            return u.Password == password;
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }
    }
}
