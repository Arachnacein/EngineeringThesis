using Domain.Entites.Person;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites
{
    [Table("PersonalRequests")]
    public class PersonalRequests // tabela zawierająca prośby pracowników cotyczące grafiku; chce, nie chce
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Id_User { get; set; }

        [Required]
        public int Id_Duty { get; set; }

        [Required]
        public bool YesOrNo { get; set; } // want or not want work that day

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime Date { get; set; }


        //navigation proprties
        public User User { get; set; }
        public Duty Duties { get; set; }



    }
}
