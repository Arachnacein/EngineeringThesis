using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Interfaces.Requests
{
    public interface IPersonalRequestsRepository
    {
        IEnumerable<PersonalRequests> GetAll();
        IEnumerable<PersonalRequests> GetAllByDate(DateTime date);
        IEnumerable<PersonalRequests> GetAllByUserId(int userId);
        PersonalRequests GetById(int id);
        PersonalRequests Add(PersonalRequests pr);
        void Update(PersonalRequests pr);
        void Delete(PersonalRequests pr);
        //IEnumerable<PersonalRequests> FindId(int user_id, DateTime date);
        IEnumerable<PersonalRequests> GetAllBetweenDates(DateTime date1, DateTime date2);
        PersonalRequests GetPersonalRequestByDateAndByUserIdAndByDuty(DateTime date, int duty_id, int user_id);
    }
}
