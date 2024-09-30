using System;

namespace Application.Dto.PersonalRequests
{
    public class PersonalRequestsDto
    {
        public int Id { get; set; }
        public int Id_User { get; set; }
        public int Id_Duty { get; set; }
        public bool YesOrNo { get; set; } // want or not want work that day
        public DateTime Date { get; set; }
    }
}