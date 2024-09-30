using Application.Dto.Vacation;
using Application.Exceptions;
using Application.Exceptions.Vacation;
using Application.Interfaces;
using Application.Interfaces.Mappers;
using Application.ViewModels;
using AutoMapper;
using Domain.Const;
using Domain.Entites;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Application.Services
{
    public class VacationService : IVacationService
    {
        private readonly IVacationRepository _vacationRepository;
        private readonly IMapper _mapper;
        private readonly IVacationMapper _vacationMapper;
        private readonly IUserRepository _userRepository;

        public VacationService(IVacationRepository vacationRepository, IMapper mapper, IVacationMapper vacationMapper, IUserRepository userRepository)
        {
            _vacationRepository = vacationRepository;
            _mapper = mapper;
            _vacationMapper = vacationMapper;
            _userRepository = userRepository;
        }
        public IEnumerable<VacationViewModel> GetAllVacations()
        {
            var x = _vacationRepository.GetAll();
            if (x == null)
                throw new VacationNotFOundException("Vacations not found");
            return _vacationMapper.MapElements(x.ToList());
        }
        public VacationViewModel GetVacationById(int id)
        {
            var x = _vacationRepository.GetById(id);
            if (x == null)
                throw new VacationNotFOundException($"Vacation not found. ID:{id}");
            return _vacationMapper.Map(x);
        }
        public VacationDto AddNewVacation(CreateVacationDto dt)
        {

            DateTime startDate = DateTime.Parse($"{dt.StartDateYear}-{dt.StartDateMonth}-{dt.StartDateDay}");
            DateTime endDate = DateTime.Parse($"{dt.EndDateYear}-{dt.EndDateMonth}-{dt.EndDateDay}");

            if (_userRepository.GetById(dt.Id_User).IsOnVacation == true )
                throw new UserDisabledException("This user is disabled");

            var user = _userRepository.GetById(dt.Id_User);
            if (user == null)
                throw new UserNotFoundException($"User not found. Id:{dt.Id_User}");

            if (user.Id_ContractType.ToString() != ContractTypeEnum.State.ToString())
                throw new VacationErrorException($"The user is not employed on a full-time basis");

            if (startDate.Date > endDate.Date)
                throw new BadValueException($"Start date [{startDate}] must be before end date [{endDate}].");
     
            if (!Enum.IsDefined(typeof(VacationTypeEnum), dt.VcationType.GetEnumValueByDescription<VacationTypeEnum>()))
                throw new BadValueException($"Vacation type does not exist. ID:{dt.VcationType }");

            int actuallyDaysRequest = (endDate - startDate).Days + 1;

            switch (dt.VcationType)
            {
                //case VacationTypeEnum.LeaveAtRequest.GetEnumValueByDescription<VacationTypeEnum>()://.GetEnumDescriptionByValue():
                case "Urlop na rządanie":

                    var leaveAtRequestRepos = _vacationRepository.GetAll().Where(x => x.Id_VacationType == VacationTypeEnum.LeaveAtRequest && x.StartDate.Year == DateTime.Today.Year && x.Id_User == user.Id);
                    int leaveAtRequestReposDays = 0;

                    foreach (var item in leaveAtRequestRepos)
                        leaveAtRequestReposDays += item.TotalDays;

                    if (leaveAtRequestReposDays == 4)
                        throw new VacationLimitExceededException("Vcation limit achieved. Cannot add more vacation.");

                    if (leaveAtRequestReposDays + actuallyDaysRequest > 4)
                        throw new VacationLimitExceededException($"Vacation limit will be exceeded. You have [{leaveAtRequestReposDays}] days of leave at request. You want take [{actuallyDaysRequest}] days of leave at request. It is more than 4 days.");
                    break;

                case "Urlop Wypoczynkowy":

                    int userLimit = user.VacationDaysLimit;
                    var lastYearUsedVacationRepo = _vacationRepository.GetAll().Where(x => x.Id_User == user.Id && x.StartDate.Year == DateTime.Today.AddYears(-1).Year);
                    int lastYearUsedVacationDays = 0;
                    foreach (var item in lastYearUsedVacationRepo)
                        lastYearUsedVacationDays += item.TotalDays; //last year used vacation days

                    int substractLastYear = userLimit - lastYearUsedVacationDays; // last year remaining days

                    var thisYearUsedVacationRepo = _vacationRepository.GetAll().Where(x => x.Id_User == user.Id && x.StartDate.Year == DateTime.Today.Year);
                    int thisYearUsedVacationDays = 0;
                    foreach (var item in thisYearUsedVacationRepo)
                        thisYearUsedVacationDays += item.TotalDays; //this year used days

                    int substractThisYear = userLimit - thisYearUsedVacationDays; //this day remaining days

                    int totalDaysRemainig = substractLastYear + substractThisYear;
                    if (totalDaysRemainig - actuallyDaysRequest < 0)
                        throw new VacationLimitExceededException($"Vacation limit will be exceeded. You have [{totalDaysRemainig}] days of vacation left. You want take [{actuallyDaysRequest}] days. Result [{(totalDaysRemainig - actuallyDaysRequest)}].");
                    break;

                //case (int)VacationTypeEnum.Arrearage:

                //break;

                default:
                    break;
            }

            user.VacationDays -= actuallyDaysRequest;
            
            var vacation = _mapper.Map<Vacation>(dt);
            vacation.Id_VacationType = dt.VcationType.GetEnumValueByDescription<VacationTypeEnum>();
            vacation.StartDate = startDate;
            vacation.EndDate = endDate;
            var result = _vacationRepository.Add(vacation);
            return _mapper.Map<VacationDto>(result);
        }
        public void UpdateVacation(UpdateVacationDto vacation)
        {
            if (_userRepository.GetById(vacation.Id_User).IsOnVacation == true)
                throw new UserDisabledException("This user is disabled");

            var existingVacation = _vacationRepository.GetById(vacation.Id);
            if (existingVacation == null)
                throw new VacationNotFOundException($"Vacation not found while updating. Id:{vacation.Id}");

            var user = _userRepository.GetById(vacation.Id_User);
            if (user == null)
                throw new UserNotFoundException($"User not found. ID: {vacation.Id_User}");

            if (user.Id_ContractType.ToString() != ContractTypeEnum.State.ToString())
                throw new VacationErrorException($"The user is not employed on a full-time basis");

            if (vacation.StartDate.Date > vacation.EndDate.Date)
                throw new BadValueException($"Start date [{vacation.StartDate}] must be before end date [{vacation.EndDate}].");

            if (vacation.Id_VacationType != 1 || vacation.Id_VacationType != 2)
                throw new BadValueException($"Vacation type does not exist. ID:{vacation.Id_VacationType }");

            int actuallyDaysRequest = (vacation.EndDate - vacation.StartDate).Days + 1;

            switch (vacation.Id_VacationType)
            {
                case (int)VacationTypeEnum.LeaveAtRequest:

                    var leaveAtRequestRepos = _vacationRepository.GetAll().Where(x => x.Id_VacationType == VacationTypeEnum.LeaveAtRequest && x.StartDate.Year == DateTime.Today.Year && x.Id_User == user.Id);
                    int leaveAtRequestReposDays = 0;

                    foreach (var item in leaveAtRequestRepos)
                        leaveAtRequestReposDays += item.TotalDays;

                    if (leaveAtRequestReposDays == 4)
                        throw new VacationLimitExceededException("Vcation limit achieved. Cannot add more vacation.");

                    if (leaveAtRequestReposDays + actuallyDaysRequest > 4)
                        throw new VacationLimitExceededException($"Vacation limit will be exceeded. You have [{leaveAtRequestReposDays}] days of leave at request. You want take [{actuallyDaysRequest}] days of leave at request. It is more than 4 days.");
                    break;

                case (int)VacationTypeEnum.VacationLeave:

                    int userLimit = user.VacationDaysLimit;
                    var lastYearUsedVacationRepo = _vacationRepository.GetAll().Where(x => x.Id_User == user.Id && x.StartDate.Year == DateTime.Today.AddYears(-1).Year);
                    int lastYearUsedVacationDays = 0;
                    foreach (var item in lastYearUsedVacationRepo)
                        lastYearUsedVacationDays += item.TotalDays; //last year used vacation days

                    int substractLastYear = userLimit - lastYearUsedVacationDays; // last year remaining days

                    var thisYearUsedVacationRepo = _vacationRepository.GetAll().Where(x => x.Id_User == user.Id && x.StartDate.Year == DateTime.Today.Year);
                    int thisYearUsedVacationDays = 0;
                    foreach (var item in thisYearUsedVacationRepo)
                        thisYearUsedVacationDays += item.TotalDays; //this year used days

                    int substractThisYear = userLimit - thisYearUsedVacationDays; //this day remaining days

                    int totalDaysRemainig = substractLastYear + substractThisYear;
                    if (totalDaysRemainig - actuallyDaysRequest < 0)
                        throw new VacationLimitExceededException($"Vacation limit will be exceeded. You have [{totalDaysRemainig}] days of vacation left. You want take [{actuallyDaysRequest}] days. Result [{(totalDaysRemainig - actuallyDaysRequest)}].");
                    break;

                //case (int)VacationTypeEnum.Arrearage:

                //break;

                default:
                    break;
            }

            user.VacationDays -= actuallyDaysRequest;

            var result = _mapper.Map<Vacation>(vacation);
            _vacationRepository.Update(result);
        }
        public void DeleteVacation(int id)
        {
            var vacation = _vacationRepository.GetById(id);
            if (vacation == null)
                throw new VacationNotFOundException($"Vacation not found whiledeleting. ID:{id}");
            _vacationRepository.Delete(vacation);
        }
        public void DeleteAllVacationOfOneUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                throw new UserNotFoundException($"User not found whiledeleting. ID:{id}");
            var vacationList = _vacationRepository.GetAll().Where(x => x.Id_User == id);
            foreach (var item in vacationList)
                _vacationRepository.Delete(item);
        }
        public IEnumerable<VacationDto> GetAllVacations(int id_user)
        {
            var checkUser = _userRepository.GetById(id_user);
            if (checkUser == null)
                throw new UserNotFoundException($"User not found. ID:{id_user}");

            var result = _vacationRepository.GetAll().Where(x => x.Id_User == id_user);
            if (result.Count() == 0)
                throw new VacationNotFOundException($"Vacation not found for user Id:{id_user}");
            return _mapper.Map<IEnumerable<Vacation>, IEnumerable<VacationDto>>(result);
        }
        public int CountTotalVacationDaysInYear(DateTime date, int id_user)
        {
            var checkUser = _userRepository.GetById(id_user);
            if (checkUser == null)
                throw new UserNotFoundException($"User not found. ID:{id_user}");

            DateTime startYear = new DateTime(date.Year, 1, 1);
            DateTime endYear = new DateTime(date.Year, 12, 31);
            int totalDays = 0;
            var vacationListOfUser = _vacationRepository.GetAll().Where(x => x.Id_User == id_user && x.StartDate.Date >= startYear && x.EndDate.Date <= endYear);
            if (vacationListOfUser.Count() == 0)
                throw new VacationNotFOundException($"Not found any vacations for user Id:{id_user} in {date.Year} year");
            foreach (var item in vacationListOfUser)
                totalDays += item.TotalDays;

            return totalDays;
        }
        public int GetRemainingVacationDays(int id_user, int year)
        {
            var user = _userRepository.GetById(id_user);
            if (user == null)
                throw new UserNotFoundException($"User not found. ID:{id_user}");

            DateTime startYear = new DateTime(year, 1, 1);
            DateTime endYear = new DateTime(year, 12, 31);
            var reamining = _vacationRepository.GetAll().Where(x => x.Id_User == id_user && x.EndDate.Date <= endYear && x.StartDate.Date >= startYear);

            int days = 0;
            foreach (var item in reamining)
                days += item.TotalDays;

            int remaining = user.VacationDaysLimit - days;

            return remaining;
        }
        public bool GetVacation(DateTime vacationDate, int id_user) // jesli jest urlop w tym dniu, zwroc true
        {
            var result = _vacationRepository.GetAll().Where(x => x.Id_User == id_user)
                                                     .Where(x => (x.StartDate.Year == vacationDate.Year || x.EndDate.Year == vacationDate.Year));
            if (result == null) return false;

            foreach (var item in result)
                if (item.StartDate.Date <= vacationDate && vacationDate <= item.EndDate.Date)
                    return true;

            return false;
        }
        public int GetVacationTimeBetweenDates(DateTime startDate, DateTime endDate, int user_id)
        {
            int vacationTimer = 0;
            for (var x = startDate; x <= endDate; x = x.AddDays(1))
                if (GetVacation(x, user_id) && x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday)
                    vacationTimer += 1;

            var total = vacationTimer * (7 * 60 + 35);
            return total;
        }

        public List<string> GetVacationTypes()
        {
            List<string> list = new List<string>();

            foreach (VacationTypeEnum val in Enum.GetValues(typeof(VacationTypeEnum)))
            {
                string description = val.ToString();
                var x = val.GetType().GetField(description);

                if (x != null)
                {
                    var y = x.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (y != null && y.Length > 0)
                        list.Add(((DescriptionAttribute)y[0]).Description);
                }
            }
            return list;
        }
    }
}