using System;

namespace Application.ViewModels
{
    public class VacationViewModel
    {
        public int Id { get; set; }
        public int Id_User { get; set; }
        public string  UserName { get; set; }
        public string  UserSurname { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string VacationType { get; set; }
        public int TotalDays { get; set; }

    }
}
