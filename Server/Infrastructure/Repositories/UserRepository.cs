using Application.Security;
using Domain.Entites;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;
        private readonly ISecurityHashClass _securityHashClass;

        public UserRepository(Context context, ISecurityHashClass securityHashClass)
        {
            _context = context;
            _securityHashClass = securityHashClass;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return  _context.Users.FirstOrDefault(x => x.Id == id); 
        }
        public User GetByLogin(string login)
        {
            return _context.Users.FirstOrDefault(x => x.Login == login);
        }
        public User GetUser(string login, string passwd)
        {
            string hashedPasswd = _securityHashClass.hashPassword(passwd);
            return _context.Users.FirstOrDefault(x => x.Login == login && x.Password == hashedPasswd);
        }

        public int GetUser(string name, string surname, bool nul)
        {
            return _context.Users.Where(x => x.Name == name && x.Surname == surname).Count();
        }

        public User Add(User user)
        {
            user.Created = DateTime.UtcNow;
            user.IsAdmin = false;
            user.IsOnVacation = false;
            user.Want_24 = false;
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public void Update(User user)
        {
            user.LastModified = DateTime.UtcNow;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void UpdatePassword(User user, string password)
        {
            user.Password = password;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public void DisableUser(User user)
        {
            user.IsOnVacation = true;
            _context.Users.Update(user);
            _context.SaveChanges();
        }
               
        public void EnableUser(User user)
        {
            user.IsOnVacation = false;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void ActivateUser(User user)
        {
            user.IsOnVacation = false;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void UpdateMinimumHours(User user, int minHours)
        {
            user.MinimumHours = minHours;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void UpdateWant24(User user, bool flag)
        {
            user.Want_24 = flag;
            _context.Users.Update(user);
            _context.SaveChanges();
        }


    }
}
