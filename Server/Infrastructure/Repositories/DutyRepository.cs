using Domain.Entites.Person;
using Domain.Interfaces;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class DutyRepository : IDutyRepository
    {
        private readonly Context _context;

        public DutyRepository(Context context)
        {
            _context = context;
        }

        public IEnumerable<Duty> GetAll()
        {
            return _context.Duties.ToList();
        }      
        
        public Duty GetByName(string name)
        {
            return _context.Duties.SingleOrDefault(x => x.Name == name);
        }

        public Duty GetById(int id)
        {
            return _context.Duties.SingleOrDefault(x => x.Id == id);
        }

        public Duty Add(Duty duty)
        {
            var created = _context.Duties.Add(duty);
            _context.SaveChanges();
            return created.Entity;
        }

        public void Update(Duty duty)
        {
            _context.Duties.Update(duty);
            _context.SaveChanges();
        }

        public void Delete(Duty duty)
        {
            _context.Duties.Remove(duty);
            _context.SaveChanges();
        }

        public Duty GetDutyByTime(int duty_time)
        {
            return _context.Duties.SingleOrDefault(x => x.WorkTime == duty_time);
        }
    }
}