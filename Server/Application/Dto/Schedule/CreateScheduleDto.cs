using System;

namespace Application.Dto
{
    public class CreateScheduleDto
    {
        public DateTime Date { get; set; }//kiedy 
        public int Id_User { get; set; } //kto
        public int Id_Duty { get; set; } // ranek, nic
    }
}
