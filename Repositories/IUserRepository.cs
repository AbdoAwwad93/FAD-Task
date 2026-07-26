using FadTask.Models;
using System.Collections.Generic;

namespace FadTask.Repositories
{
    public interface IUserRepository
    {
        User? GetByEmail(string email);
        void Add(User user);
        bool ValidateCredentials(string email, string password);
        IEnumerable<User> GetAll();
    }
}
