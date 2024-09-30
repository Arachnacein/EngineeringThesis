using Domain.Entites;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IVacationRepository
    {
        IEnumerable<Vacation> GetAll();
        Vacation GetById(int id);
        Vacation Add(Vacation vacation);
        void Update(Vacation vacation);
        void Delete(Vacation vacation);
        DateTime GetEasterDay(int year);
        
    }
}
