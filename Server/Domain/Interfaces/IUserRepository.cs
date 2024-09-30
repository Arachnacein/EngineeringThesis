using Domain.Entites;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        User GetByLogin(string login);
        User GetUser(string login, string passwd);
        int GetUser(string name, string surname, bool nul);
        User Add(User user);
        void Update(User user);
        void UpdatePassword(User user, string password);
        void UpdateMinimumHours(User user, int minHours);
        void UpdateWant24(User user, bool flag);
        void DisableUser(User user);
        void EnableUser(User user);
        void ActivateUser(User user);
        void Delete(User user);

    }
}
