using System;

namespace Application.ViewModels
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; } //kto
        public string Duty { get; set; } // ranek, nic
    }
}