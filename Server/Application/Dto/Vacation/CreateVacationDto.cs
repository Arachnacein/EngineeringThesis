using Domain.Const;
using System;

namespace Application.Dto.Vacation
{
    public class CreateVacationDto
    {
        public int Id_User { get; set; }
        public int StartDateYear { get; set; }
        public int StartDateMonth { get; set; }
        public int StartDateDay { get; set; }
        public int EndDateYear { get; set; }
        public int EndDateMonth { get; set; }
        public int EndDateDay { get; set; }
        public string VcationType { get; set; }
        private int TotalDays { get; set; }
    }
}
