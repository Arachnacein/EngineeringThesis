using Application.Dto;
using Application.Dto.PersonalRequests;
using Application.Dto.Schedule;
using Application.Interfaces;
using AutoMapper;
using System;
using System.Linq;

namespace Application.Logic.ScheduleGenerator
{
    public class GeneratorService : IGeneratorService
    {
        private readonly IScheduleService _scheduleService;
        private readonly IDutyService _dutyService;
        private readonly IUserService _userService;
        private readonly IPersonalRequestsService _personalRequestsService;
        private readonly IVacationService _vacationService;

        public GeneratorService(
            IScheduleService scheduleService,
            IDutyService dutyService,
            IUserService userService,
            IPersonalRequestsService personalRequestsService,
            IVacationService vacationService,
            IMapper imapper,
            IDateTimeHelper dateTimeHelper)
        {
            _scheduleService = scheduleService;
            _dutyService = dutyService;
            _userService = userService;
            _personalRequestsService = personalRequestsService;
            _vacationService = vacationService;
        }

        public void Add24h(DateTime randomDay, int randomDuty, int worker_id)
        {
            var request1 = _personalRequestsService.GetOnePersonalRequest(randomDay, randomDuty, worker_id);//prosby workera w danym dniu
            var request2 = _personalRequestsService.GetOnePersonalRequest(randomDay, 3, worker_id);//prosby workera w danej nocy
            var request3 = _personalRequestsService.GetOnePersonalRequest(randomDay.AddDays(1), randomDuty, worker_id);//prosby workera w nastepnym dniu

            if (CheckBeforeAndAfterDuty(randomDay, randomDuty, worker_id) == 0 && //dzień obecny
                CheckBeforeAndAfterDuty(randomDay, 3, worker_id) == 0 && //noc obecna
                CheckBeforeAndAfterDuty(randomDay.AddDays(1), randomDuty, worker_id) == 0) //dzień nazajutrz
                if ((request1 == null || request1.YesOrNo == true) || (request2 == null || request2.YesOrNo == true) || (request3 == null || request3.YesOrNo == true))//czy nie waliduje z prośbami
                {
                    // wstaw dobe
                    // wstaw dzień \
                    CreateScheduleDto newSchedule = new CreateScheduleDto
                    {
                        Id_User = worker_id,
                        Id_Duty = 2,
                        Date = randomDay.Date.Date
                    };
                    _scheduleService.AddSchedule(newSchedule);

                    // wstaw noc
                    CreateScheduleDto newSchedule2 = new CreateScheduleDto
                    {
                        Id_User = worker_id,
                        Id_Duty = 3,
                        Date = randomDay.Date.Date
                    };
                    _scheduleService.AddSchedule(newSchedule2);
                }
        }

        public void Add(DateTime randomDay, int randomDuty, int worker_id)
        {
            var actuallyRequest = _personalRequestsService.GetOnePersonalRequest(randomDay, randomDuty, worker_id);//prosby workera w danym dniu
            var adjacentDuty = 0;


            ScheduleDto actuallyDuty = null;
            ScheduleDto nextDuty = null;
            ScheduleDto beforeDuty = null;


            if (randomDuty == 2)
            {
                actuallyDuty = _scheduleService.GetOneSchedule(randomDay, worker_id, 2);
                nextDuty = _scheduleService.GetOneSchedule(randomDay, worker_id, 3);
                beforeDuty = _scheduleService.GetOneSchedule(randomDay.AddDays(-1), worker_id, 3);
            }
            else
            {
                actuallyDuty = _scheduleService.GetOneSchedule(randomDay, worker_id, 3);
                nextDuty = _scheduleService.GetOneSchedule(randomDay.AddDays(1), worker_id, 2);
                beforeDuty = _scheduleService.GetOneSchedule(randomDay, worker_id, 2);
            }

            if (actuallyDuty == null && nextDuty == null && beforeDuty == null)
                if (actuallyRequest == null || actuallyRequest.YesOrNo != false) // jeśli nie ma w danym dniu prośby lub prośba == true
                {
                    if (randomDuty == 3)
                    {
                        if (!_vacationService.GetVacation(randomDay.Date, worker_id)
                            && !_vacationService.GetVacation(randomDay.AddDays(1).Date, worker_id))
                        {
                            CreateScheduleDto newSchedule = new CreateScheduleDto
                            {
                                Id_User = worker_id,
                                Id_Duty = randomDuty,
                                Date = randomDay.Date.Date
                            };
                            _scheduleService.AddSchedule(newSchedule);
                        }
                    }
                    else
                    {
                        if (!_vacationService.GetVacation(randomDay.Date, worker_id))
                        {
                            CreateScheduleDto newSchedule = new CreateScheduleDto
                            {
                                Id_User = worker_id,
                                Id_Duty = randomDuty,
                                Date = randomDay.Date.Date
                            };
                            _scheduleService.AddSchedule(newSchedule);
                        }
                    }
                }
        }

        public bool AddMorning(DateTime randomDay, int randomDuty, int worker_id, int workingTime)
        {
            var actuallyRequest = _personalRequestsService.GetOnePersonalRequest(randomDay, randomDuty, worker_id);//prosby workera w danym dniu

            if (_scheduleService.GetOneSchedule(randomDay.Date, worker_id, randomDuty) == null)
                if (actuallyRequest == null || actuallyRequest.YesOrNo != false) // jeśli nie ma w danym dniu prośby lub prośba == true
                {
                    if (!_vacationService.GetVacation(randomDay.Date, worker_id))
                    {
                        if (workingTime < (6 * 60))//jeśli zostało mniej niż 6h do podziału
                        {
                            var substract = (7 * 60) - workingTime;//odejmij od niego tyle czasu, by ranek miał 7h
                            var dutyTime7hours = _dutyService.GetDutyByTime(7 * 60);

                            CreateScheduleDto newSchedule = new CreateScheduleDto // wstaw ranek 7h
                            {
                                Id_User = worker_id,
                                Id_Duty = dutyTime7hours,
                                Date = randomDay.Date.Date
                            };
                            _scheduleService.AddSchedule(newSchedule);

                        //wylosuj drugi dyżur dzienny
                        Losuj:
                            var randomNumber1 = RandomNumber(1, DateTime.DaysInMonth(randomDay.Year, randomDay.Month));
                            var randomDay1 = DateTime.Parse($"{randomNumber1}.{randomDay.Month}.{randomDay.Year}");

                            ScheduleDto scheduleToUpdate = _scheduleService.GetOneSchedule(randomDay1, worker_id, 2);//dyzur do zmiany czasu pracy
                            if (scheduleToUpdate == null)
                                goto Losuj;

                            var newDutyTime = _dutyService.GetDutyByTime(720 - substract); // pobranie Id duty o podanym czsie pracy
                            scheduleToUpdate.Id_Duty = newDutyTime; // zamiana Id duty
                            _scheduleService.UpdateSchedule(scheduleToUpdate);
                            return true;
                        }
                        else//jeśli więcej lub równe 6h, to wstaw ranek 6h lub więksdzy
                        {
                            int dutyTimeId = _dutyService.GetDutyByTime(workingTime);

                            CreateScheduleDto newSchedule = new CreateScheduleDto
                            {
                                Id_User = worker_id,
                                Id_Duty = dutyTimeId,
                                Date = randomDay.Date.Date
                            };
                            _scheduleService.AddSchedule(newSchedule);
                            return true;
                        }
                    }
                }
            return false;
        }

        public void Update(ScheduleDto obj, DateTime date)
        {
            obj.Date = date.Date;
            _scheduleService.UpdateSchedule(obj);
        }

        public void Update(ScheduleDto obj, int duty_id)
        {
            obj.Id_Duty = duty_id;
            _scheduleService.UpdateSchedule(obj);
        }

        public bool ValidateMove(DateTime dateTo, int id_user, int id_duty)
        {
            //sprawdź czy tego dnia nie ma już user dyżuru
            var x = _scheduleService.GetSchedule(dateTo, id_duty);
            foreach (var item in x)
                if (item.Id == id_user)
                    return false;

            //sprawdź czy nie koliduje z urlopem jeśli etatowy
            if (_userService.GetUserById(id_user).Id_ContractType == 1)
            {
                if (_vacationService.GetVacation(dateTo.Date, id_user))
                    return false;
                if (id_duty == 3 && _vacationService.GetVacation(dateTo.AddDays(1).Date, id_user))
                    return false;
            }

            //sprawdź czy nie koliduje z prośbami
            var request = _personalRequestsService.GetOnePersonalRequest(dateTo, id_duty, id_user);//prosby workera w danym dniu
            PersonalRequestsDto requestBefore = null;
            PersonalRequestsDto requestAfter = null;
            if (id_duty == 2)
            {
                requestBefore = _personalRequestsService.GetOnePersonalRequest(dateTo.AddDays(-1), 3, id_user);//prosby workera poprzednim dyż€r
                requestAfter = _personalRequestsService.GetOnePersonalRequest(dateTo, 3, id_user);//prosby workera w następnym dyżur
            }
            else
            {
                requestBefore = _personalRequestsService.GetOnePersonalRequest(dateTo, 2, id_user);//prosby workera poprzedni dyżur
                requestAfter = _personalRequestsService.GetOnePersonalRequest(dateTo.AddDays(1), 2, id_user);//prosby workera w następnym dyżur
            }

            if ((request == null || request.YesOrNo != false) && (requestAfter == null || requestAfter.YesOrNo != false) && (requestBefore == null || requestBefore.YesOrNo != false))
                return true;
            else return false;
        }

        public bool Validate24h(DateTime date, int id_user)
        {
            //sprawdź prośby
            PersonalRequestsDto requestNightBefore24h = _personalRequestsService.GetOnePersonalRequest(date.AddDays(-1), 3, id_user);//prosby workera noc przed dobą
            PersonalRequestsDto requestDayAfter24h = _personalRequestsService.GetOnePersonalRequest(date.AddDays(1), 2, id_user);//prosby workera dzień po dobie
            PersonalRequestsDto requestDay = _personalRequestsService.GetOnePersonalRequest(date, 2, id_user);//prosby workera dzień
            PersonalRequestsDto requestNight = _personalRequestsService.GetOnePersonalRequest(date, 3, id_user);//prosby workera noc
            ScheduleDto schedule24hD = _scheduleService.GetOneSchedule(date, id_user, 2);//doba dzień
            ScheduleDto schedule24hN = _scheduleService.GetOneSchedule(date, id_user, 3);//doba noc
            ScheduleDto scheduleAdter24h = _scheduleService.GetOneSchedule(date.AddDays(1), id_user, 2);//dzień po dobie
            ScheduleDto scheduleBefore24h = _scheduleService.GetOneSchedule(date.AddDays(-1), id_user, 3);//noc przed dobą

            if ((requestDay == null || requestDay.YesOrNo != false) && (requestNight == null || requestNight.YesOrNo != false))//jeśli w tym dniu nie ma prośb lub nie są, że nie chce dyżuru
                if ((requestDayAfter24h == null || requestDayAfter24h.YesOrNo != false) && (requestNightBefore24h == null || requestNightBefore24h.YesOrNo != false))//prosby c.d.
                    if (scheduleAdter24h == null && scheduleBefore24h == null)//jeśli przed i po dobie nie ma dyżurów
                        if (schedule24hD == null && schedule24hN == null)//jesli podczas soby nie ma dyzurow
                            return true;
            return false;
        }

        public void DeleteAllSchedule()
        {
            var schedules = _scheduleService.GetSchedule();
            foreach (var item in schedules)
                _scheduleService.DeleteSchedule(item.Id);
        }

        public int CheckBeforeAndAfterDuty(DateTime date, int id_duty, int id_user) // dzien - 2, //noc - 3, //ranki - 4-12
        {
            var now = _scheduleService.GetOneSchedule(date, id_user, id_duty);
            var mornings = _scheduleService.GetScheduleMornings(date, id_user);
            if (now != null || mornings == true)
                return 2;

            bool before = false;
            bool after = false;
            var morningBefore = false;
            var morningAfter = false;
            ScheduleDto dutyBefore = null;
            ScheduleDto dutyAfter = null;

            switch (id_duty)
            {
                case 2:
                    dutyBefore = _scheduleService.GetOneSchedule(date.AddDays(-1).Date, id_user, 3);
                    dutyAfter = _scheduleService.GetOneSchedule(date, id_user, 3);

                    morningBefore = _scheduleService.GetScheduleMornings(date, id_user);
                    morningAfter = _scheduleService.GetScheduleMornings(date.AddDays(1), id_user);

                    before = dutyBefore == null ? false : true;
                    after = dutyAfter == null ? false : true;

                    if (morningAfter == true)
                        after = false;
                    if (morningBefore == true)
                        before = false;

                    break;

                case 3:
                    dutyBefore = _scheduleService.GetOneSchedule(date, id_user, 2);
                    dutyAfter = _scheduleService.GetOneSchedule(date.AddDays(1), id_user, 2);

                    morningBefore = _scheduleService.GetScheduleMornings(date, id_user);
                    morningAfter = _scheduleService.GetScheduleMornings(date.AddDays(1), id_user);

                    before = dutyBefore == null ? false : true;
                    after = dutyAfter == null ? false : true;

                    if (morningAfter == true)
                        after = false;
                    if (morningBefore == true)
                        before = false;
                    break;
            }

            if (before == false && after == false)
                return 0;
            if (before == false && after == true)
                return -1;
            if (before == true && after == false)
                return 1;
            else //if (before == true && after == true)
                return 2;
        }

        public int RandomNumber(int start, int end)
        {
            Random rnd = new Random();
            return rnd.Next(start, end + 1);
        }

        public int RandomUserOfDuty(DateTime date, int id_duty)
        {
            var list = _scheduleService.GetSchedule(date, id_duty);
            int random = RandomNumber(0, list.Count() - 1);
            return list.ElementAt(random).Id_User;
        }

        public (int, int, int, int) CastlingsHelper(int tabLength, int[] arr)
        {
            int max = -1;
            int indexMax = -1;
            int min = 100;
            int indexMin = 100;

            for (int i = 0; i < tabLength; i++)
            {
                if (arr[i] < min)
                {
                    min = arr[i];
                    indexMin = i;
                }
                if (arr[i] > max)
                {
                    max = arr[i];
                    indexMax = i;
                }
            }
            return (max, indexMax, min, indexMin);
        }
    }
}