using Domain.Entites;
using Domain.Interfaces.Requests;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class PersonalRequestsRepository : IPersonalRequestsRepository
    {
        private readonly Context _context;

        public PersonalRequestsRepository(Context context)
        {
            _context = context;
        }

        public IEnumerable<PersonalRequests> GetAll()
        {
            return _context.PersonalRequests.ToList();
        }

        public IEnumerable<PersonalRequests> GetAllByDate(DateTime date)
        {
            return _context.PersonalRequests.Where(x => x.Date.Date == date.Date);
        }        
        
        public IEnumerable<PersonalRequests> GetAllBetweenDates(DateTime date1, DateTime date2)
        {
            return _context.PersonalRequests.Where(x => x.Date.Date >= date1.Date && x.Date.Date <= date2.Date);
        }

        public IEnumerable<PersonalRequests> GetAllByUserId(int userId)
        {
            return _context.PersonalRequests.Where(x => x.Id_User == userId);
        }

        public PersonalRequests GetById(int id)
        {
            return _context.PersonalRequests.SingleOrDefault(x => x.Id == id);
        }

        public PersonalRequests Add(PersonalRequests pr)
        {
            var created = _context.PersonalRequests.Add(pr);
            _context.SaveChanges();
            return created.Entity;
        }

        public void Update(PersonalRequests pr)
        {
            _context.PersonalRequests.Update(pr);
            _context.SaveChanges();
        }

        public void Delete(PersonalRequests pr)
        {
            _context.PersonalRequests.Remove(pr);
            _context.SaveChanges();
        }

        public PersonalRequests GetPersonalRequestByDateAndByUserIdAndByDuty(DateTime date, int duty_id, int user_id)
        {
            return _context.PersonalRequests.SingleOrDefault(x => x.Id_Duty == duty_id && x.Id_User == user_id && x.Date.Date == date.Date);
        }

        //public IEnumerable<PersonalRequests> FindId(int user_id, DateTime date)
        //{
        //   return  _context.PersonalRequests.Where(x => x.Date.Date == date && x.Id_User == user_id);
        //}
    }
}