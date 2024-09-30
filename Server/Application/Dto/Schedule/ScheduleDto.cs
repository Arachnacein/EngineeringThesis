using System;

namespace Application.Dto.Schedule
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Id_User { get; set; } //kto
        public int Id_Duty { get; set; } // ranek, nic
    }
}