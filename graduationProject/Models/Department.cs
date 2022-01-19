using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Models
{
    public class Department
    {
        [Key]
        public int Dept_Id { get; set; }

        [Display(Name = "القسم / المركز")]
        public string Dept_Name { get; set; }
        [ForeignKey("Governorate")]
        public int Gov_Id { get; set; }
        public Governorate Governorate { get; set; }
    }
}
