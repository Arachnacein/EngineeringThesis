using Domain.Entites;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class VacationRepository : IVacationRepository
    {
        private readonly Context _context;

        public VacationRepository(Context context)
        {
            _context = context;
        }

        public IEnumerable<Vacation> GetAll()
        {
            return _context.Vacation.ToList();
        }

        public Vacation GetById(int id)
        {
            return _context.Vacation.SingleOrDefault(x => x.Id == id);
        }

        public Vacation Add(Vacation vacation)
        {
            var startDate = vacation.StartDate.Date;
            var endDate = vacation.EndDate.Date;
            vacation.TotalDays = (endDate - startDate).Days + 1;


            for (var x = startDate; x <= endDate; x = x.AddDays(1))
            {
                //weekendy
                if (x.DayOfWeek == DayOfWeek.Saturday || x.DayOfWeek == DayOfWeek.Sunday)
                    vacation.TotalDays -= 1;
            }
                //Święta stałe
                if (startDate.Month == 1)
                {
                    if (DateTime.Parse($"01.01.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"01.01.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        vacation.TotalDays -= 1;//Nowy rok  (01.01)

                    if (DateTime.Parse($"06.01.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"06.01.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        vacation.TotalDays -= 1;//Święto 3 Króli  (06.01)
                }
                else if (startDate.Month == 5)
                {
                    if (DateTime.Parse($"01.05.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"01.05.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        vacation.TotalDays -= 1;//Święto Pracy (01.05)

                    if (DateTime.Parse($"03.05.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"03.05.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        vacation.TotalDays -= 1;//Święto Konstytucji 3 maja (03.05)
                }
                else if (startDate.Month == 8)
                {
                    if (DateTime.Parse($"15.08.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"15.08.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        vacation.TotalDays -= 1; // Wniebowzięcie NMP(15.08)
                }
                else if (startDate.Month == 11)
                {
                    if (DateTime.Parse($"01.11.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"01.11.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        vacation.TotalDays -= 1; //Święto Wszystkich Świętych(01.11)

                    if (DateTime.Parse($"11.11.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"11.11.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        vacation.TotalDays -= 1;//Święto Niepodległosci(11.11)
                }
                else if (startDate.Month == 12)
                {
                    if (DateTime.Parse($"25.12.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"25.12.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        vacation.TotalDays -= 1; //Boże Narodzenie Dzień 1(25.12)

                    if (DateTime.Parse($"26.12.{startDate.Year}").DayOfWeek != DayOfWeek.Saturday && DateTime.Parse($"26.12.{startDate.Year}").DayOfWeek != DayOfWeek.Sunday)
                        vacation.TotalDays -= 1;//Boże Narodzenie Dzień 2(26.12)
                }

                //Święta ruchome
                //algorytm Gaussa
                if (startDate.Month == 3 || startDate.Month == 4)
                {
                    var easterDay1 = GetEasterDay(startDate.Year);
                    var easterDay2 = GetEasterDay(startDate.Year).AddDays(1);

                    if (easterDay1 < endDate && easterDay1 > startDate)
                        if (easterDay1.DayOfWeek != DayOfWeek.Saturday && easterDay1.DayOfWeek != DayOfWeek.Sunday)
                            vacation.TotalDays -= 1; //wielkanoc dzień 1

                    if (easterDay2 < endDate && easterDay2 > startDate)
                        if (easterDay2.DayOfWeek != DayOfWeek.Saturday && easterDay2.DayOfWeek != DayOfWeek.Sunday)
                            vacation.TotalDays -= 1; //wielkanoc dzień 2
                }
                else if (startDate.Month == 5 || startDate.Month == 6)
                {
                    var bożeCiało = GetEasterDay(startDate.Year).AddDays(60);
                    if (bożeCiało < endDate && bożeCiało > startDate)
                        if (bożeCiało.DayOfWeek != DayOfWeek.Saturday && bożeCiało.DayOfWeek != DayOfWeek.Sunday)
                            vacation.TotalDays -= 1; //boże ciało
                }
            

            var created = _context.Vacation.Add(vacation);
            _context.SaveChanges();
            return created.Entity;
        }

        public void Update(Vacation vacation)
        {
            var startDate = vacation.StartDate;
            var endDate = vacation.EndDate;
            vacation.TotalDays = (endDate - startDate).Days + 1;

            //_context.Vacation.Update(vacation);
            var xd = _context.Vacation.FirstOrDefault(x => x.Id == vacation.Id);
            xd.TotalDays = vacation.TotalDays;
            _context.SaveChanges();
        }

        public void Delete(Vacation vacation)
        {
            _context.Vacation.Remove(vacation);
            _context.SaveChanges();
        }

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
    }
}
