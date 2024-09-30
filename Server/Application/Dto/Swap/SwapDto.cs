using System;

namespace Application.Dto.Swap
{
    public class SwapDto
    {
        public int Id { get; set; }
        public int Id_User1 { get; set; }
        public DateTime Date1 { get; set; }
        public int Id_Duty1 { get; set; }
        public int Id_User2 { get; set; }
        public DateTime Date2 { get; set; }
        public int Id_Duty2 { get; set; }
        public bool IsConfirmedByUser { get; set; }
        public bool IsCheckedByUser { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsCheckedByAdmin { get; set; }
        public bool IsNotificationSent { get; set; }

    }
}
