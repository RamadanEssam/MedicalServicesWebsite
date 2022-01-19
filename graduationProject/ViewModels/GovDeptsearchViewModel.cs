using graduationProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.ViewModels
{
    public class GovDeptsearchViewModel
    {
        [ForeignKey("Governorates")]
        public int Gov_Id { get; set; }

        [Display(Name = "المحافظة")]
        public List<Governorate> Governorates { get; set; }

        [ForeignKey("Departments")]
        public int Dept_Id { get; set; }

        [Display(Name = "المدينة")]
        public List<Department> Departments { get; set; }
    }
}
