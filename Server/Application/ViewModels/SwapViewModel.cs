using System;

namespace Application.ViewModels
{
    public class SwapViewModel
    {
        public int Id { get; set; }
        public string Login1 { get; set; }
        public DateTime Date1 { get; set; }
        public string Duty1 { get; set; }
        public string Login2 { get; set; }
        public DateTime Date2 { get; set; }
        public string Duty2 { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsCheckedByAdmin { get; set; }
    }
}