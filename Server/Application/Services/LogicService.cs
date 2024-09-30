using Application.Dto;
using Application.Dto.Schedule;
using Application.Interfaces;
using Application.Logic.ScheduleGenerator;
using Domain.Const;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class LogicService : ILogicService
    {
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IUserService _userService;
        private readonly IPersonalRequestsService _personalRequestsService;
        private readonly IScheduleService _scheduleService;
        private readonly IVacationService _vacationService;
        private readonly IGeneratorService _generatorService;
        private readonly IDutyService _dutyService;

        public LogicService(IDateTimeHelper dateTimeHelper,
                            IUserService userService,
                            IPersonalRequestsService personalRequestsService,
                            IScheduleService scheduleService,
                            IVacationService vacationService,
                            IGeneratorService generatorService,
                            IDutyService dutyService)
        {
            _dateTimeHelper = dateTimeHelper;
            _userService = userService;
            _personalRequestsService = personalRequestsService;
            _scheduleService = scheduleService;
            _vacationService = vacationService;
            _generatorService = generatorService;
            _dutyService = dutyService;
        }

        public void MainLogic()
        {
            var startDay = _dateTimeHelper.GetFirstDayOfMonth(DateTime.UtcNow.AddMonths(1));
            var endDay = _dateTimeHelper.GetLastDayOfMonth(DateTime.UtcNow.AddMonths(1));

            StateRequests(startDay, endDay);
            NonStateRequests(startDay, endDay);
            FillStateWorkers(startDay, endDay);
            FillNonStateWorkers(startDay, endDay);
            Castlings(startDay, endDay);
            //CheckSundays(startDay, endDay);

        }

        public void StateRequests(DateTime startDay, DateTime endDay)
        {
            //lista  etatowych
            var stateWorkersList = _userService.GetAllStateUsers();//+
            foreach (var stateWorker in stateWorkersList)
            {
                if (stateWorker.IsOnVacation == true)
                    continue;
                //wstaw prosby etatowych
                var request = _personalRequestsService.GetPersonalRequests(startDay, endDay, stateWorker.Id).Where(x => x.YesOrNo == true);
                foreach (var item2 in request)
                {
                    if (_scheduleService.GetOneSchedule(item2.Date.Date, item2.Id_User, item2.Id_Duty) == null)
                    {
                        CreateScheduleDto schedule = new CreateScheduleDto
                        {
                            Id_User = item2.Id_User,
                            Id_Duty = item2.Id_Duty,
                            Date = item2.Date.Date
                        };
                        _scheduleService.AddSchedule(schedule);
                    }
                }
                //sprawdź czy worker ma jedną niedzielę dzień
                var sundayList = _scheduleService.GetSchedule(startDay, endDay, stateWorker.Id);
                List<ScheduleDto> sundayDuties = new List<ScheduleDto>();
                foreach (var item in sundayList)
                    if (item.Date.DayOfWeek == DayOfWeek.Sunday && item.Id_Duty == 2)
                        sundayDuties.Add(item);

                if (sundayDuties == null)//jeśli nie ma żadnej niedzieli
                {
                    //wylosuj niedziele
                    var randomSundayList = _dateTimeHelper.RandomSundayDay(startDay, endDay, stateWorker.Id);
                    int randomNumber = _generatorService.RandomNumber(0, randomSundayList.Count() - 1);
                    DateTime sunday = randomSundayList.ElementAt(randomNumber);
                    //sprawdź czy ilosc pracowników < 14
                    var workersCount = _scheduleService.GetSchedule(sunday, 2).Count();
                    if(workersCount < 14)
                    {
                        //wstaw dyżur => niedziele dzień
                        if(_generatorService.ValidateMove(sunday, stateWorker.Id, 2))
                            _generatorService.Add(sunday, 2, stateWorker.Id);
                    }
                }
                else continue;
            }
        }

        public void NonStateRequests(DateTime startDay, DateTime endDay)
        {
            //lista kontraktowych
            var nonStateWorkers = _userService.GetAllNonStateUsers();
            //dodaj prośby kontraktowych
            foreach (var nonStateWorker in nonStateWorkers)
            {
                if (nonStateWorker.IsOnVacation == true)
                    continue;
                //wstaw prosby kontraktowych
                var request = _personalRequestsService.GetPersonalRequests(startDay, endDay, nonStateWorker.Id);
                foreach (var item2 in request)
                {
                    if (_scheduleService.GetOneSchedule(item2.Date.Date, item2.Id_User, item2.Id_Duty) == null)
                    {
                        CreateScheduleDto schedule = new CreateScheduleDto
                        {
                            Id_User = item2.Id_User,
                            Id_Duty = item2.Id_Duty,
                            Date = item2.Date.Date
                        };
                        _scheduleService.AddSchedule(schedule);
                    }
                }
            }
        }

        public void FillStateWorkers(DateTime startDay, DateTime endDay)
        {
            var dutyFlag = 2;

            var stateWorkersList = _userService.GetAllStateUsers();//+
            foreach (var worker in stateWorkersList)
            {
                if (worker.IsOnVacation == true)
                    continue;

                int monthlyWorkTime = _dateTimeHelper.CountWorkingHours(_dateTimeHelper.CountWorkingDays(startDay, endDay));
                monthlyWorkTime -= _vacationService.GetVacationTimeBetweenDates(startDay, endDay, worker.Id); // uwzględnienie urlopów
                int increment = 1;
                for (var schedule = startDay; schedule <= endDay; schedule = schedule.AddDays(increment))
                {
                    while (_dateTimeHelper.CountWorkHours(worker.Id, startDay, endDay) < monthlyWorkTime - 720)//jeśli jeszcze nie osiągnięto limitu godzin pracy w miesiącu
                    {
                    Random:
                        var randomStartDay = _generatorService.RandomNumber(0, 3);
                        if (schedule.AddDays(randomStartDay) <= endDay)
                            schedule = schedule.AddDays(randomStartDay);
                        else goto Random;

                        if (_dateTimeHelper.CountWorkNights(startDay, endDay, worker.Id) < 12)
                        {
                            if (_dateTimeHelper.CheckDaysAndNights(schedule.Date, worker.Id))
                            {
                                if (dutyFlag == 2) dutyFlag = 3;
                                else dutyFlag = 2;
                            }
                            else dutyFlag = 2;
                        }
                        else dutyFlag = 2;

                        //                                                                 //sprawdz sąsiednie dyżury
                        if (_generatorService.CheckBeforeAndAfterDuty(schedule.Date, dutyFlag, worker.Id) == 0) // 0 - oba dni wolne
                        {
                            if(_generatorService.ValidateMove(schedule.Date, worker.Id, dutyFlag))
                                _generatorService.Add(schedule.Date, dutyFlag, worker.Id);
                            increment = 1;
                            break;
                        }
                        else if (_generatorService.CheckBeforeAndAfterDuty(schedule.Date, dutyFlag, worker.Id) == -1)
                        {
                            increment = 2;
                            break;
                        }
                        else
                        {
                            increment = 1;
                            break;
                        }
                    }
                }

            //Dopełenienie reszty luk
            CheckSpaces:
                var randomNumber = _generatorService.RandomNumber(1, DateTime.DaysInMonth(startDay.Year, startDay.Month));
                var randomDay = DateTime.Parse($"{randomNumber}.{startDay.Month}.{startDay.Year}");
                var randomDuty = _generatorService.RandomNumber(2, 3);

                if (_dateTimeHelper.CountWorkHours(worker.Id, startDay, endDay) < monthlyWorkTime - 720)
                {
                    if (_generatorService.CheckBeforeAndAfterDuty(randomDay, randomDuty, worker.Id) == 0)
                        if (_generatorService.ValidateMove(randomDay, worker.Id, dutyFlag))
                            _generatorService.Add(randomDay, randomDuty, worker.Id);

                    goto CheckSpaces;
                }

                #region DodajRanki

                int monthWorkingTime = _dateTimeHelper.CountWorkingHours(_dateTimeHelper.CountWorkingDays(startDay, endDay)) - _vacationService.GetVacationTimeBetweenDates(startDay, endDay, worker.Id); ;//policz sumę godzin w miesiacu,
                int monthWorkedTime = _scheduleService.GetSchedule(startDay, endDay, worker.Id).Count();
                monthWorkedTime *= 720;
                int substract = monthWorkingTime - monthWorkedTime;//oblicz róznice

            //wstaw ranek o ilości godzin = róznicy
            AddDuty:
              randomNumber = _generatorService.RandomNumber(1, DateTime.DaysInMonth(startDay.Year, startDay.Month));
                randomDay = DateTime.Parse($"{randomNumber}.{startDay.Month}.{startDay.Year}");
                randomDuty = 2;

                if (_dateTimeHelper.CountWorkHours(worker.Id, startDay, endDay) < monthlyWorkTime)
                {
                    if (_generatorService.CheckBeforeAndAfterDuty(randomDay, randomDuty, worker.Id) == 0)
                       if(_generatorService.AddMorning(randomDay, randomDuty, worker.Id, substract));
                            else  goto AddDuty; 
                    else  goto AddDuty; 

                }
                

                #endregion DodajRanki
            }
        }

        public void FillNonStateWorkers(DateTime startDay, DateTime endDay)
        {
            //dodaj prośby kontraktowych
            var nonStateWorkers = _userService.GetAllNonStateUsers();
            foreach (var nonStateWorker in nonStateWorkers)
            {
                if (nonStateWorker.IsOnVacation == true)
                    continue;

                var list = _scheduleService.GetSchedule(startDay, endDay, nonStateWorker.Id);
                int minimumWorkTime = nonStateWorker.MinimumHours * 60;
                int actuallyWorkTime = 0;

                foreach (var item in list)
                    actuallyWorkTime += _dutyService.GetDutyTime(item.Id_Duty);

                if (nonStateWorker.Want_24 == true)
                {
                    while (actuallyWorkTime < minimumWorkTime)
                    {
                        //losuj dyżur,
                        var randomNumber = _generatorService.RandomNumber(1, DateTime.DaysInMonth(startDay.Year, startDay.Month));
                        var randomDay = DateTime.Parse($"{randomNumber}.{startDay.Month}.{startDay.Year}").Date;
                        var randomDuty = 2;

                        _generatorService.Add24h(randomDay, randomDuty, nonStateWorker.Id);

                        list = _scheduleService.GetSchedule(startDay, endDay, nonStateWorker.Id);
                        actuallyWorkTime = 0;
                        foreach (var item in list)
                            actuallyWorkTime += _dutyService.GetDutyTime(item.Id_Duty);
                    }
                }
                else//not want 24h duties
                {
                    while (actuallyWorkTime < minimumWorkTime)
                    {
                        //losuj dyżur,
                        var randomNumber = _generatorService.RandomNumber(1, DateTime.DaysInMonth(startDay.Year, startDay.Month));
                        var randomDay = DateTime.Parse($"{randomNumber}.{startDay.Month}.{startDay.Year}").Date;
                        var randomDuty = _generatorService.RandomNumber(2, 3);

                        _generatorService.Add(randomDay, randomDuty, nonStateWorker.Id);

                        list = _scheduleService.GetSchedule(startDay, endDay, nonStateWorker.Id);
                        actuallyWorkTime = 0;
                        foreach (var item in list)
                            actuallyWorkTime += _dutyService.GetDutyTime(item.Id_Duty);
                    }
                }
            }
        }

        public void Castlings(DateTime startDay, DateTime endDay)
        {
            //zrob tablice o rozmiarze ilości dni w miesiacu
            int[] tabD = new int[DateTime.DaysInMonth(startDay.Year, startDay.Month)];
            int[] tabN = new int[DateTime.DaysInMonth(startDay.Year, startDay.Month)];
            int[] tabM = new int[DateTime.DaysInMonth(startDay.Year, startDay.Month)];
            int ii;
            int maxD, indexMaxD, minD, indexMinD;
            int maxN, indexMaxN, minN, indexMinN;
            int maxM, indexMaxM, minM, indexMinM;
        MinMaxLoop:
            //wstaw w poszczególne komórki tablicy(dni) ilość dyżurantów
            ii = 0;
            for (var day = startDay; day <= endDay; day = day.AddDays(1))
            {
                tabD[ii] = _scheduleService.GetSchedule(day, 2).Count();//Tablica Dni
                tabM[ii] = _scheduleService.GetScheduleMornings(day).Count();//Tablica Dni
                tabN[ii++] = _scheduleService.GetSchedule(day, 3).Count();//Tablica Nocy
            }

            //znajdź największą liczbę dyżurantów i najmniejszą i ich indeksy
            (maxD, indexMaxD, minD, indexMinD) = _generatorService.CastlingsHelper(tabD.Length, tabD);
            (maxN, indexMaxN, minN, indexMinN) = _generatorService.CastlingsHelper(tabN.Length, tabN);


            while (maxD - minD > 1)//dzien
            {
                //losuj usera z tego dnia, gdzie jest MAX
                int randomUserId = _generatorService.RandomUserOfDuty(DateTime.Parse($"{indexMaxD + 1}/{startDay.Month}/{startDay.Year}"), 2);
                UserDto randomUserDto = _userService.GetUserById(randomUserId);

                if (randomUserDto.Id_ContractType != (int)ContractTypeEnum.State && randomUserDto.Want_24 == true)//jesli user chce 24h
                {
                    //sprawdź czy wybrany dyżur należy do doby
                    ScheduleDto var24h_D = _scheduleService.GetOneSchedule(DateTime.Parse($"{indexMaxD + 1}/{startDay.Month}/{startDay.Year}"), randomUserId, 2);
                    ScheduleDto var24h_N = _scheduleService.GetOneSchedule(DateTime.Parse($"{indexMaxD + 1}/{startDay.Month}/{startDay.Year}"), randomUserId, 3);
                    
                    if(var24h_D != null && var24h_N != null)//mamy dobę
                    {
                        int iterator = 0;
                        //znajdź indeksy w tabelach Dni i Nocy,  w których jest niedobór dyżurów
                        do
                        {
                            if(tabD[iterator] < 15 && tabN[iterator] < 13)//if jest niedobór
                            {
                                //przenieś dobę na ten dzień
                                if(_generatorService.Validate24h(var24h_D.Date.Date, randomUserId))
                                {
                                    _generatorService.Update(var24h_D, DateTime.Parse($"{iterator + 1}/{startDay.Month}/{startDay.Year}"));
                                    _generatorService.Update(var24h_N, DateTime.Parse($"{iterator + 1}/{startDay.Month}/{startDay.Year}"));
                                    break;  
                                }
                            }
                            iterator++;
                        }while(iterator < DateTime.DaysInMonth(startDay.Year, startDay.Month));
                    }
                }
                else
                {
                    //zmień datę grafiku
                    if (_generatorService.CheckBeforeAndAfterDuty(DateTime.Parse($"{indexMinD + 1}/{startDay.Month}/{startDay.Year}"), 2, randomUserId) == 0)
                        if (_generatorService.ValidateMove(DateTime.Parse($"{indexMinD + 1}/{startDay.Month}/{startDay.Year}"), randomUserId, 2))
                        {
                            _generatorService.Update(_scheduleService.GetOneSchedule(DateTime.Parse($"{indexMaxD + 1}/{startDay.Month}/{startDay.Year}"), randomUserId, 2),
                                    DateTime.Parse($"{indexMinD + 1}/{startDay.Month}/{startDay.Year}"));
                        }
                    goto MinMaxLoop;

                }
            }

            while (maxN - minN > 1)//noc
            {
                //losuj usera z tego noc, gdzie jest MAX
                int randomUserId = _generatorService.RandomUserOfDuty(DateTime.Parse($"{indexMaxN + 1}/{startDay.Month}/{startDay.Year}"), 3);

                //zmień datę grafiku
                if (_generatorService.CheckBeforeAndAfterDuty(DateTime.Parse($"{indexMinN + 1}/{startDay.Month}/{startDay.Year}"), 3, randomUserId) == 0)
                    if (_generatorService.ValidateMove(DateTime.Parse($"{indexMinN + 1}/{startDay.Month}/{startDay.Year}"), randomUserId, 3))
                    {
                        _generatorService.Update(_scheduleService.GetOneSchedule(DateTime.Parse($"{indexMaxN + 1}/{startDay.Month}/{startDay.Year}"), randomUserId, 3),
                                DateTime.Parse($"{indexMinN + 1}/{startDay.Month}/{startDay.Year}"));
                    }
                goto MinMaxLoop;
            }

            #region CastlingMornings

            CastlingMornings:
            ii = 0;
            for (var day = startDay; day <= endDay; day = day.AddDays(1))
                tabM[ii++] = _scheduleService.GetScheduleMornings(day).Count();//Tablica Dni

            (maxM, indexMaxM, minM, indexMinM) = _generatorService.CastlingsHelper(tabM.Length, tabM);

            while(maxM > 4)
            {
                var maxMorning = _scheduleService.GetScheduleMornings(DateTime.Parse($"{indexMaxM + 1 }-{startDay.Month}-{startDay.Year}"));//lista ranków w danym dniu gdzie jest MAX
                DateTime minMorning = DateTime.Parse($"{indexMinM + 1}-{startDay.Month}-{startDay.Year}");//dzień kiedy jest najmniej ranków (MIN)
                var morningToMove = maxMorning.ElementAt(_generatorService.RandomNumber(0, maxMorning.Count() - 1));//losuj jeden z nadmiarowych do zamiany
                if(_generatorService.ValidateMove(minMorning, morningToMove.Id_User, morningToMove.Id_Duty))             
                    _generatorService.Update(morningToMove, minMorning);
                goto CastlingMornings;
            }
            #endregion
        }

        public void CheckSundays(DateTime startDay, DateTime endDay)
        {
            //każdy etatowy ma mieć conajmniej jeden dzień w niedzielę
            //pętla po pracownikach etatowych
            var stateList = _userService.GetAllStateUsers();
            foreach (var worker in stateList)
            {
                var scheduleList = _scheduleService.GetSchedule(startDay, endDay, worker.Id);
                bool sundayFlag = false;
                foreach (var schedule in scheduleList)
                    if (schedule.Date.DayOfWeek == DayOfWeek.Sunday && schedule.Id_Duty == 2)
                        sundayFlag = true;

                if (!sundayFlag)//jesli pracownik w całym miesiacu nie ma ani jednego dnia w niedziele
                {
                    List<DateTime> sundayList = _dateTimeHelper.GetSundaysInMonth(scheduleList.ElementAt(0).Date);
                    //Sprawdź czy można wstawić dyżur w niedziele
                    foreach (var sunday in sundayList)
                    {
                        if (_generatorService.CheckBeforeAndAfterDuty(sunday, 2, worker.Id) == 0)//jeśli nie koliduje, sprawdz urlopy i porsby
                        {
                            if (_generatorService.ValidateMove(sunday, worker.Id, 2))// jeśli nie koliduje przsesuń losowy dyżur na tą niedziele
                            {
                                ScheduleDto randomSchedule;
                                do
                                {
                                    //losuj dyżur do przestawienia na niedziele, najlepiej dniowy wyzur
                                    randomSchedule = _scheduleService.GetOneSchedule(DateTime.Parse($"{sunday.Year}-{sunday.Month}-{_generatorService.RandomNumber(1, DateTime.DaysInMonth(sunday.Year, sunday.Month))}"),
                                                                                    worker.Id, 2);
                                } while (randomSchedule == null);
                                _generatorService.Update(randomSchedule, sunday);
                            }
                            break;
                        }
                    }
                //jeśli nie da się wstawić w niedziele
                //losuj niedziele
                Again:
                    DateTime randomSunday = sundayList.ElementAt(_generatorService.RandomNumber(0, sundayList.Count() - 1));
                    //znajdź dyżur do zabrania i przestaw go na niedziele
                    if (_generatorService.CheckBeforeAndAfterDuty(randomSunday, 2, worker.Id) == 1)// sobota noc jest zajęta
                    {
                        //sprawdź urlopy
                        if (_vacationService.GetVacation(randomSunday.Date, worker.Id) == false)
                        {
                            //sprawdź prośby
                            var requestDay = _personalRequestsService.GetOnePersonalRequest(randomSunday, 2, worker.Id);//prosby workera niedziele d
                            //sprawdz niedziele noc grafik
                            var sche = _scheduleService.GetOneSchedule(randomSunday, worker.Id, 3);

                            //zamien sobote noc na niedziele dzien
                            if ((requestDay == null || requestDay.YesOrNo != false) && sche == null)
                            {
                                _generatorService.Update(_scheduleService.GetOneSchedule(randomSunday.AddDays(-1), worker.Id, 3), randomSunday);  // zamiana soboty na niedziele
                                _generatorService.Update(_scheduleService.GetOneSchedule(randomSunday, worker.Id, 3), 2);             //zamiana nocy na dzień
                            }
                            else goto Again;
                        }
                        else goto Again;
                    }
                    if (_generatorService.CheckBeforeAndAfterDuty(randomSunday, 2, worker.Id) == -1)// niedziela noc jest zajęta
                    {
                        //sprawdź urlopy
                        if (_vacationService.GetVacation(randomSunday.Date, worker.Id) == false)
                        {
                            //sprawdź prośby
                            var requestDay = _personalRequestsService.GetOnePersonalRequest(randomSunday, 2, worker.Id);//prosby workera niedziele d
                            //sprawdz niedziele dzien grafik
                            var sche = _scheduleService.GetOneSchedule(randomSunday.AddDays(-1), worker.Id, 3);

                            //zamien niedizele noc na niedziele dzien
                            if ((requestDay == null || requestDay.YesOrNo != false) && sche == null)
                            {
                                _generatorService.Update(_scheduleService.GetOneSchedule(randomSunday, worker.Id, 3), 2);
                            }
                            else goto Again;
                        }
                        else goto Again;
                    }
                }
            }
        }
    }
}