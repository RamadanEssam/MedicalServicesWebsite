using graduationProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.ViewModels
{
    public class deptTubeViewModel
    {
        public int OxgnId { get; set; }

        [Display(Name = "النوع")]
        [Required]
        [Remote(action: "TubeType", controller: "OxygenTube")]
        public string OxgnType { get; set; }

        [Display(Name = "العدد")]
        [Required]
        public int? OxgnAmount { get; set; }
        [Display(Name = "السعر")]
        [Required]
        public int? OxgnCost { get; set; }
        [Display(Name = "المحمول")]
        [Required]
        public string OxgnPhone { get; set; }
        [Display(Name = "الموقع")]
        [Required]
        public string OxgnLocation { get; set; }
        [Display(Name = "الوصف")]
        [Required]
        public string OxgnDescription { get; set; }
        [Display(Name = "تاريخ التسجيل")]
        public DateTime dateCreate { get; set; }

        [ForeignKey("Departments")]
        public int Dept_Id { get; set; }

        public List<Department> Departments { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
