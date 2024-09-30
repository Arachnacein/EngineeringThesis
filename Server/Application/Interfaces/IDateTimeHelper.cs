using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IDateTimeHelper
    {
        int CountWorkingHours(int days); // counts working hours in a  month . 1 day is 7h and 35 minutes. Refer only workers on state(Etat).
        int CountWorkHours(int user_id, DateTime startDate, DateTime endDate); //counts worked hours in a mont
        bool CheckDaysAndNights(DateTime date, int id_user); // checks if there is less night duties than day duties in month
        DateTime GetFirstDayOfMonth(DateTime date); // returns 1st day of month of the chosen date
        DateTime GetLastDayOfMonth(DateTime date); // returns last day of month of the chosen date
        DateTime GetEasterDay(int year); // returns a easter date in chosen year using Gauss algorithm
        int CountWorkingDays(DateTime date1, DateTime date2);
        List<DateTime> GetSundaysInMonth(DateTime date);//returns list of sundays in month
        int CountWorkNights(DateTime startDate, DateTime endDate, int id_user);
        List<DateTime> RandomSundayDay(DateTime startDate, DateTime endDate, int id_user);
    }
}
