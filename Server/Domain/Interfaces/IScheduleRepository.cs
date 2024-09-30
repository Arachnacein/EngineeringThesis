using Domain.Entites;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IScheduleRepository
    {
        IEnumerable<Schedule> GetAll();
        Schedule GetById(int id);
        Schedule GetByDate(DateTime date);
        Schedule Add(Schedule s);
        void Update(Schedule s);
        void Delete(Schedule s);
        bool CheckLastMonthLastDayNightDuty(DateTime date, int id);
        Schedule GetByUserIdAndByDateAndByDityId(DateTime date, int user_id, int duty_id);
    }
}
