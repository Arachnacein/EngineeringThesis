using Application.Dto.Duty;
using Application.ViewModels;
using Domain.Entites.Person;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IDutyService
    {
        IEnumerable<DutyViewModel> GetAllDuties();
        DutyViewModel GetDutyById(int id);
        int GetDutyTime(int id);
        string GetDutyName(int id);
        int GetDutyIdByName(string name);
        DutyDto AddNewDuty(CreateDutyDto dt);
        void UpdateDuty(DutyDto dt);
        void DeleteDuty(int id);
        int GetDutyByTime(int duty_time);
    }
}
