using System;

namespace Application.ViewModels
{
    public class PersonalRequestsViewModel
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Duty { get; set; }
        public bool YesOrNo { get; set; } // want or not want work that day
        public DateTime Date { get; set; }
    }
}