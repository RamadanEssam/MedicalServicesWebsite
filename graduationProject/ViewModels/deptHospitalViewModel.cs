using graduationProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.ViewModels
{
    public class deptHospitalViewModel
    {
        
        public int Hos_Id { get; set; }

        [Required]
        [Display(Name = "اسم المستشفي")]
        [Remote(action: "HospitalName", controller: "Hospital")]
        public string Hos_Name { get; set; }

        [Required]
        [Display(Name = "عدد الحضانات")]
        public int Hos_Incubators { get; set; }

        [Required]
        [Display(Name = "سعر اليوم")]
        public float DayPrice { get; set; }

        [Required]
        [Display(Name = "العنوان")]
        public string Hos_Location { get; set; }

        [Required]
        [Display(Name = "رقم التليفون")]
        public string Hos_Phone { get; set; }
        public IFormFile File { get; set; }

        [Display(Name = "صورة المستشفي")]
        public string Hos_Image { get; set; }

        [Display(Name = "صورة المستشفي")]
        public byte[] hos_pic { get; set; }

        [Display(Name = "تاريخ التسجيل")]
        public DateTime dateCreate { get; set; }

        [ForeignKey("Department")]
        public int Dept_Id { get; set; }
        public List<Department> Departments { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
