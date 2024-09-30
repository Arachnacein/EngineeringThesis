using Domain.Entites;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly Context _context;

        public ScheduleRepository(Context context)
        {
            _context = context;
        }
        public IEnumerable<Schedule> GetAll()
        {
            return _context.Schedule.ToList();
        }        
        public Schedule GetById(int id)
        {
            return _context.Schedule.SingleOrDefault(x => x.Id == id);
        }   
        public Schedule GetByDate(DateTime date)
        {
            return _context.Schedule.SingleOrDefault(x => x.Date == date);
        }      
        public Schedule GetByUserIdAndByDateAndByDityId(DateTime date, int user_id, int duty_id)
        {
            return _context.Schedule.SingleOrDefault(x => x.Date.Date == date.Date && x.Id_User == user_id && x.Id_Duty == duty_id);
        }
        public Schedule Add(Schedule s)
        {
            var created = _context.Add(s);
            _context.SaveChanges();
            return created.Entity;
        }
        public void Update(Schedule s)
        {
            _context.Schedule.Update(s);
            _context.SaveChanges();
        }
        public void Delete(Schedule s)
        {
            _context.Schedule.Remove(s);
            _context.SaveChanges();
        }
        public bool CheckLastMonthLastDayNightDuty(DateTime date,int id)
        {
            if (_context.Schedule.SingleOrDefault(x => x.Id_User == id && x.Date.Date == date.Date && x.Id_Duty == 3) != null)
                return true;
            else return false;
        }
    }
}