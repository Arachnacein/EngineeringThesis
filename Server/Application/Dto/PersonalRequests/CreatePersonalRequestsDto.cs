using System;

namespace Application.Dto.PersonalRequests
{
    public class CreatePersonalRequestsDto
    {
        public int Id_User { get; set; }
        public string Duty { get; set; }
        public bool YesOrNo { get; set; } // want or not want work that day
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
