using Application.Dto;
using Application.Dto.Schedule;
using Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IScheduleService
    {
        IEnumerable<ScheduleViewModel> GetSchedule();
        IEnumerable<ScheduleViewModel> GetSchedule(int id);
        IEnumerable<ScheduleViewModel> GetSchedule(int id, int duty_id);
        IEnumerable<ScheduleDto> GetSchedule(DateTime date, int id_duty);
        IEnumerable<ScheduleDto> GetSchedule(DateTime startDate, DateTime endDate, int id_user);
        IEnumerable<ScheduleDto> GetSchedule(DateTime date1, DateTime date2);
        IEnumerable<ScheduleDto> GetSchedule(DateTime date);
        IEnumerable<ScheduleDto> GetScheduleMornings(DateTime date);
        bool GetScheduleMornings(DateTime date, int id_user);
        IEnumerable<ScheduleViewModel> GetMonthSchedule(int year, int month, int user_id);
        ScheduleViewModel GetOneSchedule(int id);
        ScheduleDto GetOneSchedule(DateTime date, int user_id, int duty_id);
        ScheduleDto GetOneSchedule(DateTime date);
        ScheduleDto AddSchedule(CreateScheduleDto sch);        
        ScheduleDto AddSchedule2(CreateScheduleDto2 obj);
        void UpdateSchedule(ScheduleDto sch);
        void DeleteSchedule(int id);
        void DeleteForeighSchedule(string login, int year, int month, int day, int id_duty);
        // returns one user who has a night duty in chosen date
        bool CheckLastMonthLastDayNightDuty(DateTime date, int id_user);
        // zwraca listę pracowników, któzy nie mają żadnego dyżuru w niedzielę dzień
        List<UserDto> GetNonSundayWorkers();
        // zwraca listę pracowników, któzy nie mają żadnego dyżuru w niedzielę dzień
        List<UserDto> GetDutyCrew(int id_duty, int year, int month, int day);
        (int[], int[], int[])CheckArrays();
        // zwraca nazwe dyżuru, rok, miesiąc, dzień
        (string, int, int, int) ReturnNextDuty(int id_user); 
    }
}