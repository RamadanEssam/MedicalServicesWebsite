using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Models
{
    public class UserReservationOxegin
    {
        [Key]
        public int Id { get; set; }
        //[Key]
        //[Column(Order = 1)]
        [Required]
        [ForeignKey("User")]
        public string User_Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        //[Key]
        //[Column(Order = 2)]
        [ForeignKey("OxygenTube")]
        public int Oxygen_Id { get; set; }
        public OxygenTube OxygenTube { get; set; }
        [Display(Name = "وقت التسجيل")]
        public DateTime Res_Date { get; set; }
        [Required]
        [Display(Name = "عدد الانابيب")]
        public int Num_tubes { get; set; }
    }
}
