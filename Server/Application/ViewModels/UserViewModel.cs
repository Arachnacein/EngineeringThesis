namespace Application.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public bool IsAdmin { get; set; }
        public string Rank { get; set; }
        public string ContractType { get; set; }
        public bool IsOnVacation { get; set; }
        public int VacationDays { get; set; }
        public int VacationDaysLimit { get; set; }
        public bool Want_24 { get; set; }
        public int MinimumHours { get; set; }
    }
}