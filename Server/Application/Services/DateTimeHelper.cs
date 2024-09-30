using Application.Exceptions;
using Application.Interfaces;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public class DateTimeHelper : IDateTimeHelper
    {
        private readonly IScheduleService _scheduleService;
        private readonly IDutyService _dutyService;

        public DateTimeHelper(IScheduleService scheduleService, IDutyService dutyService)
        {
            _scheduleService = scheduleService;
            _dutyService = dutyService;
        }

        public int CountWorkingHours(int days) => days * (7 * 60 + 35);

        public int CountWorkingDays(DateTime startDate, DateTime endDate)
        {
            try
            {
                int holidays = 0;

                //Święta stałe
                if (startDate.Month == 1)
                {
                    if (DateTime.Parse($"01.01.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"01.01.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        holidays += 1;//Nowy rok  (01.01)

                    if (DateTime.Parse($"06.01.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"06.01.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        holidays += 1;//Święto 3 Króli  (06.01)
                }
                else if (startDate.Month == 5)
                {
                    if (DateTime.Parse($"01.05.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"01.05.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        holidays += 1;//Święto Pracy (01.05)

                    if (DateTime.Parse($"03.05.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"03.05.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        holidays += 1;//Święto Konstytucji 3 maja (03.05)
                }
                else if (startDate.Month == 8)
                {
                    if (DateTime.Parse($"15.08.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"15.08.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        holidays += 1; // Wniebowzięcie NMP(15.08)
                }
                else if (startDate.Month == 11)
                {
                    if (DateTime.Parse($"01.11.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"01.11.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        holidays += 1; //Święto Wszystkich Świętych(01.11)

                    if (DateTime.Parse($"11.11.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"11.11.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        holidays += 1; //Święto Niepodległosci(11.11)
                }
                else if (startDate.Month == 12)
                {
                    if (DateTime.Parse($"25.12.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"25.12.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        holidays += 1; //Boże Narodzenie Dzień 1(25.12)

                    if (DateTime.Parse($"26.12.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"26.12.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        holidays += 1; //Boże Narodzenie Dzień 2(26.12)
                }

                //Święta ruchome
                //algorytm Gaussa
                if (startDate.Month == 3 || startDate.Month == 4)
                {
                    var easterDay1 = GetEasterDay(startDate.Year);
                    var easterDay2 = GetEasterDay(startDate.Year).AddDays(1);

                    if (easterDay1 < endDate && easterDay1 > startDate)
                        if (easterDay1.DayOfWeek != DayOfWeek.Saturday && easterDay1.DayOfWeek != DayOfWeek.Sunday)
                            holidays += 1;//wielkanoc dzień 1

                    if (easterDay2 < endDate && easterDay2 > startDate)
                        if (easterDay2.DayOfWeek != DayOfWeek.Saturday && easterDay2.DayOfWeek != DayOfWeek.Sunday)
                            holidays += 1;//wielkanoc dzień 2
                }
                else if (startDate.Month == 5 || startDate.Month == 6)
                {
                    var bożeCiało = GetEasterDay(startDate.Year).AddDays(60);
                    if (bożeCiało < endDate && bożeCiało > startDate)
                        if (bożeCiało.DayOfWeek != DayOfWeek.Saturday && bożeCiało.DayOfWeek != DayOfWeek.Sunday)
                            holidays += 1;//boże ciało
                }

                int workingDAys = 0;
                for (var day = startDate; day <= endDate; day = day.AddDays(1))
                    if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                        workingDAys += 1;

                return workingDAys - holidays;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int CountWorkHours(int user_id, DateTime startDate, DateTime endDate)//
        {
            try
            {
                var schedule = _scheduleService.GetSchedule(startDate, endDate, user_id);
                int totalTime = 0;
                foreach (var item in schedule)
                    totalTime += _dutyService.GetDutyTime(item.Id_Duty);

                return totalTime;
            }
            catch (ScheduleNotFoundException exc)
            {
                throw new ScheduleNotFoundException(exc.Message);
            }
            catch (DutyNotFoundException exc)
            {
                throw new DutyNotFoundException(exc.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool CheckDaysAndNights(DateTime date, int id_user)//+
        {
            try
            {
                DateTime startDate = GetFirstDayOfMonth(date);
                DateTime endDate = GetLastDayOfMonth(date);
                int daysCounter = 0, nightCounter = 0;

                int day_id = _dutyService.GetDutyIdByName("Dzień");
                int night_id = _dutyService.GetDutyIdByName("Noc");

                var x = _scheduleService.GetSchedule(startDate, endDate);
                foreach (var item in x)
                {
                    if (item.Id_User == id_user)
                    {
                        if (item.Id_Duty == day_id)
                            daysCounter++;
                        if (item.Id_Duty == night_id)
                            nightCounter++;
                    }
                }

                if (nightCounter + 2 < daysCounter)
                    return true;
                else return false;
            }
            catch (UserNotFoundException exc)
            {
                throw new UserNotFoundException(exc.Message);
            }
            catch (DutyNotFoundException exc)
            {
                throw new DutyNotFoundException(exc.Message);
            }
            catch (ScheduleNotFoundException exc)
            {
                throw new ScheduleNotFoundException(exc.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int CountWorkNights(DateTime startDate, DateTime endDate, int id_user)
        {
            try
            {
                int nightCounter = 0;
                int night_id = _dutyService.GetDutyIdByName("Noc");
                var x = _scheduleService.GetSchedule(startDate, endDate, id_user);

                foreach (var item in x)
                    if (item.Id_Duty == night_id)
                        nightCounter++;

                return nightCounter;
            }
            catch (UserNotFoundException exc)
            {
                throw new UserNotFoundException(exc.Message);
            }
            catch (DutyNotFoundException exc)
            {
                throw new DutyNotFoundException(exc.Message);
            }
            catch (ScheduleNotFoundException exc)
            {
                throw new ScheduleNotFoundException(exc.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }//+

        public DateTime GetFirstDayOfMonth(DateTime date)
        {
            //first day of  month
            var x = date.Date;
            return date.Date.AddDays(1 - x.Day);
        }//+

        public DateTime GetLastDayOfMonth(DateTime date)
        {
            //last day of month
            var y = GetFirstDayOfMonth(date).AddMonths(1).AddDays(-1);
            var xy = y;
            return xy;
            //return date.Date.AddDays(1 - y.Day).AddMonths(1).AddTicks(-1);
        }//+

        public DateTime GetEasterDay(int year)
        {
            //http://kaj.uniwersytetradom.pl/csh7.html
            // Gauss EasterDay alghoritm

            int G = year % 19;
            int C = year / 100;
            int H = (C - C / 4 - (8 * C + 13) / 25 + 19 * G + 15) % 30;
            int I = H - (H / 28) * (1 - (H / 28) * 29 / (H + 1) * (21 - G) / 11);
            int J = (year + year / 4 + I + 2 - C + C / 4) % 7;
            int L = I - J;
            int M = 3 + (L + 40) / 44;       // Miesiąc Wielkanocy
            int D = L + 28 - 31 * (M / 4);   // Dzień Wielkanocy

            return new DateTime(year, M, D);
        }

        public List<DateTime> GetSundaysInMonth(DateTime date)
        {
            DateTime startDate = DateTime.Parse($"{date.Year}-{date.Month}-{1}");
            DateTime endDate = DateTime.Parse($"{date.Year}-{date.Month}-{DateTime.DaysInMonth(date.Year, date.Month)}");
            List<DateTime> sundaysList = new List<DateTime>();

            for (DateTime i = startDate; i < endDate; i = i.AddDays(1))
                if (i.DayOfWeek == DayOfWeek.Sunday)
                    sundaysList.Add(i);

            return sundaysList;
        }

        public List<DateTime> RandomSundayDay(DateTime startDate, DateTime endDate, int id_user)
        {
            List<DateTime> sundayList = new List<DateTime>();

            for (DateTime i = startDate; i < endDate; i = i.AddDays(1))
                if (i.DayOfWeek == DayOfWeek.Sunday)
                    sundayList.Add(i);
            
            return sundayList;
        }
    }
}