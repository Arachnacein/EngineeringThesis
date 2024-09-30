using Application.Dto.Vacation;
using Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IVacationService
    {
        IEnumerable<VacationViewModel> GetAllVacations();
        VacationViewModel GetVacationById(int id);
        VacationDto AddNewVacation(CreateVacationDto dt);
        void UpdateVacation(UpdateVacationDto dt);
        void DeleteVacation(int id);
        void DeleteAllVacationOfOneUser(int id);
        IEnumerable<VacationDto> GetAllVacations(int id_user);
        int CountTotalVacationDaysInYear(DateTime date, int id_user);
        int GetRemainingVacationDays(int id_user, int year);
        bool GetVacation(DateTime vacationDate, int id_user);
        int GetVacationTimeBetweenDates(DateTime startDate, DateTime endDate, int user_id);
        List<string> GetVacationTypes();

    }
}
