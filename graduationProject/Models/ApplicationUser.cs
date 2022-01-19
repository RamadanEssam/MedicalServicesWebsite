using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Display(Name = "الاسم الاول")]
        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "العائلة")]
        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Display(Name = "رقم البطاقة")]
        [MaxLength(100)]
        public string SSN { get; set; }

        [Display(Name = "الصورة الشخصية")]
        [MaxLength(150)]
        public byte[] ProfileImg { get; set; }
        public int status { get; set; }

        public virtual ICollection<Hospital> hospitals { get; set; }
        public virtual ICollection<OxygenTube> oxygenTubes { get; set; }

    }
}
