using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Models
{
    public class Hospital
    {
        [Key]
        public int Hos_Id { get; set; }
        [Display(Name = "اسم المستشفي")]
        public string Hos_Name { get; set; }
        [Display(Name = "عدد الحضانات")]
        public int Hos_Incubators { get; set; }
        [Display(Name = "سعر اليوم")]
        public float DayPrice { get; set; }
        [Display(Name = "العنوان")]
        public string Hos_Location { get; set; }
        [Display(Name = "رقم التليفون")]
        public string Hos_Phone { get; set; }
        [Display(Name = "صورة المستشفي")]
        public string Hos_Image { get; set; }

        [Display(Name = "تاريخ التسجيل")]
        public DateTime dateCreate { get; set; }
        [Display(Name = "صورة المستشفي")]
        public byte[] hos_pic { get; set; }

        [ForeignKey("Department")]
        public int Dept_Id { get; set; }
        public Department Department { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
