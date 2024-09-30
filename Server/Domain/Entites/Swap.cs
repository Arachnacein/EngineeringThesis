using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites
{
    [Table("Swap")]
    public class Swap //tabela zawierająca prośby zamian pracowników
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Id_User1 { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime Date1 { get; set; }

        [Required]
        public int Id_Duty1 { get; set; }


        [Required]
        public int Id_User2 { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime Date2 { get; set; }

        [Required]
        public int Id_Duty2 { get; set; }

        [Required]
        public bool IsConfirmedByUser { get; set; }     
        
        [Required]
        public bool IsCheckedByUser { get; set; }

        [Required]
        public bool IsConfirmed { get; set; }
        
        [Required]
        public bool IsNotificationSent { get; set; }

        [Required]
        public bool IsCheckedByAdmin { get; set; }





    }
}
