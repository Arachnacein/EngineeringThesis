using System;

namespace Application.Dto.Swap
{
    public class CreateSwapDto
    {
        public int Id_User1 { get; set; }
        public int Year1 { get; set; }
        public int Month1 { get; set; }
        public int Day1 { get; set; }
        public string Duty1 { get; set; }

        public int Id_User2 { get; set; }
        public int Year2 { get; set; }
        public int Month2 { get; set; }
        public int Day2 { get; set; }
        public string Duty2 { get; set; }

    }
}
