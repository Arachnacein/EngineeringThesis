using Application.Dto.PersonalRequests;
using Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IPersonalRequestsService
    {
        IEnumerable<PersonalRequestsViewModel> GetPersonalRequests();
        IEnumerable<PersonalRequestsViewModel> GetPersonalRequests(DateTime date);
        IEnumerable<PersonalRequestsViewModel> GetPersonalRequests(int userId);
        PersonalRequestsViewModel GetOnePersonalRequest(int id);
        PersonalRequestsDto GetOnePersonalRequest(DateTime date, int duty_id, int user_id);
        PersonalRequestsDto AddPersonalRequest(CreatePersonalRequestsDto pr);
        void UpdatePersonalRequest(PersonalRequestsDto pr);
        void DeletePersonalRequest(int id_user, int year, int month, int day);
        void DeletePersonalRequests(int id);
        IEnumerable<PersonalRequestsDto> GetPersonalStateRequests(DateTime date1, DateTime date2);
        IEnumerable<PersonalRequestsDto> GetPersonalNonStateRequests(DateTime date1, DateTime date2);
        IEnumerable<PersonalRequestsDto> GetPersonalRequests(DateTime date1, DateTime date2, int user_id);
        IEnumerable<PersonalRequestsViewModel> GetPersonalRequests(int year, int month, int user_id);
    }
}
