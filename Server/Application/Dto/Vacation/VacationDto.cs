using System;

namespace Application.Dto.Vacation
{
    public class VacationDto
    {
        public int Id { get; set; }
        public int Id_User { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Id_VacationType { get; set; }
        public int TotalDays { get; set; }
    }
}
