using Domain.Const;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites
{
    [Table("Vacation")]
    public class Vacation // tabela zawierająca urlopy pracowników
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Id_User { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }

        [Required]
        public VacationTypeEnum Id_VacationType { get; set; }

        public int TotalDays { get; set; }
    }
}