using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.ViewModels
{
    public class ReservationTubeFormViewModel
    {
        public int OxgnId { get; set; }
        [Display(Name = "عدد الانابيب")]
        [Required]
        public int tube_num { get; set; }

        [Display(Name = "رقم البطاقة")]
        [Required, MaxLength(100)]
        public string SSN { get; set; }
    }
}
