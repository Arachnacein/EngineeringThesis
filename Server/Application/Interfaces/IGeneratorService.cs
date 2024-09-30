using Application.Dto.Schedule;
using System;

namespace Application.Logic.ScheduleGenerator
{
    public interface IGeneratorService
    {
        void Add24h(DateTime randomDay, int randomDuty, int worker_id);//Dodaje dobę danego dnia

        void Add(DateTime randomDay, int randomDuty, int worker_id);//Dodaje dyżur

        bool AddMorning(DateTime randomDay, int randomDuty, int worker_id, int workingTime);//Dodaje ranek

        void Update(ScheduleDto obj, DateTime date);//Przenosi dyżur (updateuje date)

        void Update(ScheduleDto obj, int duty_id);//Przenosi dyżur (updateuje duty)

        bool ValidateMove(DateTime dateTo, int id_user, int id_duty);//waliduje dyżur z prosbami i urlopami

        bool Validate24h(DateTime date, int id_user); //waliduje prośby dla doby

        void DeleteAllSchedule(); //Usuwa cały grafik

        int RandomNumber(int start, int end); //Losuje losową liczbę z zadanego przedziału

        int RandomUserOfDuty(DateTime date, int id_duty);// Losuje użytkownika, który ma dyżur danego dnia i danego duty

        /// <param name="date">Data dyzuru</param>
        /// <param name="id_duty">Id rodzaju duty (noc, dzień, itp.)</param>
        /// <param name="id_user">Id usera, który ma dyżur</param>
        /// <returns>Zwraca jedną z 4 wartości</returns>
        /// <returns>  0  gdy dyżur przed i dyżur po są wolne.</returns>
        /// <returns>  1  gdy dyżur przed jest zajęty.</returns>
        /// <returns> -1  gdy dyżur po jest zajęty.</returns>
        /// <returns>  2  gdy dyżury przed i po są zajęte.</returns>
        int CheckBeforeAndAfterDuty(DateTime date, int id_duty, int id_user); //Sprawdza czy dyżury poprzedzający i następujący są wolne

        /// <param name="tabLength">Długoś tablicy intów</param>
        /// <param name="arr">Tablica intów</param>
        /// <returns>Zwraca 4 wartości</returns>
        /// <returns>Wartość MAX z tablicy</returns>
        /// <returns>Index wartości MAX</returns>
        /// <returns>Wartość MIN z tablicy</returns>
        /// <returns>Index wartości MIN</returns>
        (int, int, int, int) CastlingsHelper(int tabLength, int[] arr);
    }
}