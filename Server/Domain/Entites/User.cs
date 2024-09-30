using Domain.Common;
using Domain.Const;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites
{
    [Table("User")]
    public class User : Auditable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Surname { get; set; }

        public string Login { get; set; }

        [Required]
        [MaxLength(64)]
        public string Password { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public RankEnum Id_Rank { get; set; }

        [Required]
        public ContractTypeEnum Id_ContractType { get; set; }

        [Required]
        public bool IsOnVacation { get; set; }
        [Required]
        public int VacationDays { get; set; }
        [Required]
        public int VacationDaysLimit { get; set; }
        public bool Want_24 { get; set; }
        public int MinimumHours { get; set; }
        //
        public ICollection<PersonalRequests> PersonalRequestss { get; set; }
        public ICollection<Schedule> Schedules { get; set; }

    }
}
