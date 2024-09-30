using Domain.Entites.Person;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites
{
    [Table("Schedule")]
    public class Schedule                    //table areprezentuje grafik pracy, jeden rekord zawiera jeden dyżur
    {                                        // czyli jeden rekord to jeden pracownik na jednym stanowisku pracy na jednym dyżuże, => 7:00 - 19:00
        [Key]                                // jesli ktoś ma dobę, to będą to dwa rekordy, pierwszy z duty = dzien a drugi z duty = noc
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime Date { get; set; }//kiedy 

        [Required]
        public int Id_User { get; set; } //kto

        [Required]
        public int Id_Duty { get; set; } // ranek, noc, dzień, test


        //navigation proprties
        public User Users { get; set; }
        public Duty Duties{ get; set; }


    }
}
