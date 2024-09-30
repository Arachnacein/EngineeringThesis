using Domain.Entites.Person;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IDutyRepository
    {
        IEnumerable<Duty> GetAll();
        Duty GetById(int id);
        Duty GetByName(string name);
        Duty Add(Duty duty);
        void Update(Duty duty);
        void Delete(Duty duty);
        Duty GetDutyByTime(int duty_time);
    }
}