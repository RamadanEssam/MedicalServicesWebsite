using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.ViewModels
{
    public class ReservationFormViewModel
    {

        public int Hos_Id { get; set; }
        [Display(Name = "عدد الحضانات")]
        [Required]
        [Range(1,100)]
        public int Hos_Incubators { get; set; }

        [Display(Name = "رقم البطاقة")]
        [Required,MaxLength(100)]
        public string SSN { get; set; }
    }
}
