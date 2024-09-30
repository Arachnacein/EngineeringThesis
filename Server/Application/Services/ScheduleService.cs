using Application.Dto;
using Application.Dto.Schedule;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Mappers;
using Application.ViewModels;
using AutoMapper;
using Domain.Entites;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _iScheduleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDutyRepository _dutyRepository;
        private readonly IMapper _mapper;
        private readonly IScheduleMapper _scheduleMapper;
        private readonly IUserService _userService;

        public ScheduleService(IScheduleRepository iScheduleRepository,
                                IMapper mapper,
                                IScheduleMapper scheduleMapper,
                                IUserRepository userRepository,
                                IDutyRepository dutyRepository,
                                IUserService userService)
        {
            _iScheduleRepository = iScheduleRepository;
            _mapper = mapper;
            _scheduleMapper = scheduleMapper;
            _userRepository = userRepository;
            _dutyRepository = dutyRepository;
            _userService = userService;
        }

        public IEnumerable<ScheduleViewModel> GetSchedule()
        {
            var dtos =  _iScheduleRepository.GetAll();
            if (dtos == null)
                throw new ScheduleNotFoundException("Schedule not found");
            return _scheduleMapper.MapElements(dtos.ToList());
        }
        public IEnumerable<ScheduleViewModel> GetSchedule(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                throw new UserNotFoundException($"User not found. ID {id}");
            var schedule = _iScheduleRepository.GetAll().Where(x => x.Id_User == id);
            if (schedule == null)
                throw new ScheduleNotFoundException($"Schedule not found. Id:{id}");
            return _scheduleMapper.MapElements(schedule.ToList());
        }
        public IEnumerable<ScheduleDto> GetSchedule(DateTime date)
        {
            var dtos = _iScheduleRepository.GetAll().Where(x => x.Date.Date == date.Date);
            if (dtos == null)
                throw new ScheduleNotFoundException($"Schedule not found. Date:{date}");
            return _mapper.Map<IEnumerable<Schedule>, IEnumerable<ScheduleDto>>(dtos);
        }
        public IEnumerable<ScheduleDto> GetScheduleMornings(DateTime date)
        {
            var dtos = _iScheduleRepository.GetAll().Where(x => x.Date.Date == date.Date && x.Id_Duty != 2 && x.Id_Duty != 3);
            if (dtos == null)
                throw new Exception($"Zero results for date: {date}");
            return _mapper.Map<IEnumerable<Schedule>, IEnumerable<ScheduleDto>>(dtos);
        }
        public bool GetScheduleMornings(DateTime date, int id_user)
        {
            var result = GetScheduleMornings(date).Where(x => x.Id_User == id_user);
            if (result.Count() == 0)
                return false;
            else return true;
        }
        public List<UserDto> GetDutyCrew(int id_duty, int year, int month, int day)
        {
            DateTime date = DateTime.Parse($"{year}-{month}-{day}");

            var dtos = _iScheduleRepository.GetAll().Where(x => x.Id_Duty == id_duty && x.Date.Date == date.Date).Select(x => x.Id_User).ToList();
            if (dtos == null)
                throw new Exception($"Zero results for date: {date}");

            List<UserDto> list = new List<UserDto>();
            foreach (var item in dtos)
            {
                list.Add(_userService.GetUserById(item));
            }
            return list;
        }
        public IEnumerable<ScheduleViewModel> GetMonthSchedule(int year, int month, int user_id)
        {
            var datex = DateTime.Parse($"{year}-{month}-1");
            var temp = datex.Date;
            var startDay = datex.Date.AddDays(1 - temp.Day);
            var endDay = startDay.AddMonths(1).AddDays(-1);

            var dtos = _iScheduleRepository.GetAll().Where(x => x.Date.Date >= startDay && x.Date.Date <= endDay && x.Id_User == user_id).OrderBy(x => x.Date);
            if (dtos == null)
                throw new ScheduleNotFoundException($"Schedule not found. Date:{datex}, user id:{user_id}");
            return _scheduleMapper.MapElements(dtos.ToList());
        }
        public bool CheckLastMonthLastDayNightDuty(DateTime date, int id_user)
        {
            var result =  _iScheduleRepository.CheckLastMonthLastDayNightDuty(date,id_user);
            return result;
        }
        public ScheduleViewModel GetOneSchedule(int id)
        {
            var x =  _iScheduleRepository.GetById(id);
            if (x == null)
                throw new ScheduleNotFoundException($"Schedule not found. Id:{id}");
            return _scheduleMapper.Map(x);
        }       
        public ScheduleDto GetOneSchedule(DateTime date)
        {
            var x =  _iScheduleRepository.GetByDate(date);
            if (x == null)
                throw new ScheduleNotFoundException($"Schedule not found. Date:{date}");
            return _mapper.Map<ScheduleDto>(x);
        }
        public IEnumerable<ScheduleDto> GetSchedule(DateTime date1, DateTime date2)
        {
            var x = _iScheduleRepository.GetAll().Where(x => x.Date.Date <= date2.Date.AddDays(1) && x.Date.Date >= date1.Date);
            if (x == null)
                throw new ScheduleNotFoundException($"Schedule between 2 dates not found. Date1 :{date1}, Date2 :{date2}");
           return  _mapper.Map<IEnumerable<Schedule>, IEnumerable<ScheduleDto>>(x);
        }       
        public IEnumerable<ScheduleDto> GetSchedule(DateTime date, int id_duty)
        {
            var x = _iScheduleRepository.GetAll().Where(x => x.Date.Date == date.Date && x.Id_Duty == id_duty);
            if (x == null)
                throw new ScheduleNotFoundException($"Schedule not found. Date :{date}, User Id :{id_duty}");
           return  _mapper.Map<IEnumerable<Schedule>, IEnumerable<ScheduleDto>>(x);
        }
        public ScheduleDto AddSchedule(CreateScheduleDto sch)
        {
            var user = _userRepository.GetById(sch.Id_User);
            if (user == null)
                throw new UserNotFoundException($"User id not found. ID:{sch.Id_User}");

            var duty = _dutyRepository.GetById(sch.Id_Duty);
            if (duty == null)
                throw new DutyNotFoundException($"Duty not found. ID:{sch.Id_Duty}");

            var schedule = _mapper.Map<Schedule>(sch);
            var result =  _iScheduleRepository.Add(schedule);
            return _mapper.Map<ScheduleDto>(result);
        }
        public ScheduleDto AddSchedule2(CreateScheduleDto2 obj)
        {
            var user = _userRepository.GetByLogin(obj.Login);
            if (user == null)
                throw new UserNotFoundException($"User id not found. ID:{obj.Login}");

            var duty = _dutyRepository.GetById(obj.Id_Duty);
            if (duty == null)
                throw new DutyNotFoundException($"Duty not found. ID:{obj.Id_Duty}");

            var schedule = _mapper.Map<Schedule>(obj);
            var result = _iScheduleRepository.Add(schedule);
            return _mapper.Map<ScheduleDto>(result);

        }
        public void UpdateSchedule(ScheduleDto sch)
        {
            var user = _userRepository.GetById(sch.Id_User);
            if (user == null)
                throw new UserNotFoundException($"User id not found. ID:{sch.Id_User}");

            var duty = _dutyRepository.GetById(sch.Id_Duty);
            if (duty == null)
                throw new DutyNotFoundException($"Duty not found. ID:{sch.Id_Duty}");

            var existingSchedule = _iScheduleRepository.GetById(sch.Id);
            if (existingSchedule == null)
                throw new ScheduleNotFoundException($"Schedule not found while updating. Id:{sch.Id}");
            var result = _mapper.Map(sch, existingSchedule);
            _iScheduleRepository.Update(result);
        }
        public void DeleteSchedule(int id)
        {
            var x =  _iScheduleRepository.GetById(id);
            if (x == null)
                throw new ScheduleNotFoundException($"Schedule not found while deleting. Id:{id}");
             _iScheduleRepository.Delete(x);
        }        
        public void DeleteForeighSchedule(string login, int year, int month, int day, int id_duty)
        {
            DateTime date = DateTime.Parse($"{year}-{month}-{day}");
            var dutyObj = _dutyRepository.GetById(id_duty);
            if (dutyObj == null)
                throw new DutyNotFoundException($"Duty nod found. id:{id_duty}");

            var user = _userRepository.GetByLogin(login);
            if (user == null)
                throw new UserNotFoundException($"User not found, login:{login}");

            var x =  _iScheduleRepository.GetByUserIdAndByDateAndByDityId(date, user.Id, id_duty);
            if (x == null)
                throw new ScheduleNotFoundException($"Schedule not found while deleting. {date}, {user}, {id_duty}");
             _iScheduleRepository.Delete(x);
        }
        public ScheduleDto GetOneSchedule(DateTime date, int user_id, int duty_id)
        {
            var result = _iScheduleRepository.GetByUserIdAndByDateAndByDityId(date, user_id, duty_id);

            if (result == null)
                return null;
            else return _mapper.Map<ScheduleDto>(result);
        }      
        public IEnumerable<ScheduleDto> GetSchedule(DateTime startDate, DateTime endDate, int id_user)
        {
            var result = _iScheduleRepository.GetAll().Where(x => x.Date <= endDate && x.Date >= startDate && x.Id_User == id_user);
            if (result == null)
                throw new ScheduleNotFoundException($"Schedule not found. Dates [{startDate} : {endDate}] id: {id_user}");
            return _mapper.Map<IEnumerable<Schedule>, IEnumerable<ScheduleDto>>(result);
        }
        public IEnumerable<ScheduleViewModel> GetSchedule(int id, int duty_id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                throw new UserNotFoundException($"User not found. ID {id}");

            var duty = _dutyRepository.GetById(duty_id);
            if (duty == null)
                throw new DutyNotFoundException($"Dity not found. ID:{duty_id}");

            var schedule = _iScheduleRepository.GetAll().Where(x => x.Id_User == id).Where(x => x.Id_Duty == duty_id);
            if (schedule == null)
                throw new ScheduleNotFoundException($"Schedule not found. Id:{id}");

            return _scheduleMapper.MapElements(schedule.ToList());
        }
        public List<UserDto> GetNonSundayWorkers()
        {
            var startDay = DateTime.Parse($"1-02-2023");
            var endDay = DateTime.Parse($"28-02-2023");

            var stateWorksersList = _userService.GetAllStateUsers();
            List<UserDto> resultList = new List<UserDto>();

            foreach (var item in stateWorksersList)
            {
                var monthlySchedule = GetSchedule(startDay, endDay, item.Id);
                bool flag = false;
                foreach (var item2 in monthlySchedule)
                {
                    if (item2.Date.DayOfWeek == DayOfWeek.Sunday) 
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag != true)
                {
                    resultList.Add(item);
                }
                else flag = false;
            }
            return resultList;
        }
        public (int[], int[], int[]) CheckArrays()
        {
            var startDay = DateTime.Parse($"1-02-2023");
            var endDay = DateTime.Parse($"28-02-2023");
            //zrob tablice o rozmiarze ilości dni w miesiacu
            int[] tabD = new int[DateTime.DaysInMonth(startDay.Year, startDay.Month)];
            int[] tabN = new int[DateTime.DaysInMonth(startDay.Year, startDay.Month)];
            int[] tabM = new int[DateTime.DaysInMonth(startDay.Year, startDay.Month)];

            //wstaw w poszczególne komórki tablicy(dni) ilość dyżurantów
            int ii = 0;

            for (var day = startDay; day <= endDay; day = day.AddDays(1))
            {
                tabD[ii] = GetSchedule(day, 2).Count();//Tablica Dni
                tabM[ii] = GetScheduleMornings(day).Count(); // dodawnaie ranków
                tabN[ii++] = GetSchedule(day, 3).Count();//Tablica Nocy
            }
            return(tabD, tabN, tabM);
        }

        public (string, int, int, int) ReturnNextDuty(int id_user)
        {
            var user = _userRepository.GetById(id_user);
            if (user == null)
                throw new UserNotFoundException($"User not found. Id{id_user}");
            var now = DateTime.UtcNow;
            var nextDuty = _iScheduleRepository.GetAll().Where(x => x.Date >= now.Date && x.Id_User == id_user).OrderBy(x => x.Date).ToList();

            
            for (int i = 0; i < nextDuty.Count; i++)
            {
                if (nextDuty[i].Id_Duty == 2 && nextDuty[i + 1].Id_Duty == 3 && (nextDuty[i].Date == nextDuty[i+1].Date))
                    return ("Doba", nextDuty[i].Date.Year, nextDuty[i].Date.Month, nextDuty[i].Date.Day);

                else if (nextDuty[i].Id_Duty == 2)
                    return ("Dzień", nextDuty[i].Date.Year, nextDuty[i].Date.Month, nextDuty[i].Date.Day);

                else if(i - 1 >= 0) 
                        if (nextDuty[i].Id_Duty == 3 && nextDuty[i-1].Id_Duty != 2 && (nextDuty[i].Date != nextDuty[i+1].Date))
                    return ("Noc", nextDuty[i].Date.Year, nextDuty[i].Date.Month, nextDuty[i].Date.Day);

                else return ("Ranek", nextDuty[i].Date.Year, nextDuty[i].Date.Month, nextDuty[i].Date.Day);
            }
            return ("Error", 1,1,now.Date.Year);
        }


    }
}