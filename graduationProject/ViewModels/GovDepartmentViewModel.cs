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
    public class GovDepartmentViewModel
    {
        public int Dept_Id { get; set; }

        [Required(ErrorMessage = "مطلوب ادخال الاسم")]
        [Display(Name = "القسم / المركز")]
        [Remote(action: "DepartmentName", controller: "Department")]
        public string Dept_Name { get; set; }
        
        [ForeignKey("Governorates")]
        public int Gov_Id { get; set; }
        
        [Display(Name = "المحافظة")]
        public List<Governorate> Governorates { get; set; }
        public object File { get; internal set; }
    }
}
