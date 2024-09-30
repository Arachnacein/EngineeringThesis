using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites.Person
{
    [Table("Duty")]
    public class Duty   //tabela zawierająca formy dyżuru : dzień, noc, ranek
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public int WorkTime { get; set; }

        //navigation proprties
        public ICollection<PersonalRequests> PersonalRequestss { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}
