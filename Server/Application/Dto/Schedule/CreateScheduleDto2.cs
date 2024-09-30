using System;

namespace Application.Dto
{
    public class CreateScheduleDto2
    {
        public DateTime Date { get; set; }//kiedy 
        public string Login { get; set; }
        public int Id_Duty { get; set; } // ranek, nic
    }
}
