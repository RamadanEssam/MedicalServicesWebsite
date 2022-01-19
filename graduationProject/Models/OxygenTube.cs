using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace graduationProject.Models
{
    public partial class OxygenTube
    {
        [Key]
        public int OxgnId { get; set; }
        [Display(Name = "النوع")]
        public string OxgnType { get; set; }
        [Display(Name = "العدد")]
        public int? OxgnAmount { get; set; }
        [Display(Name = "السعر")]
        public int? OxgnCost { get; set; }
        [Display(Name = "المحمول")]
        public string OxgnPhone { get; set; }
        [Display(Name = "الموقع")]
        public string OxgnLocation { get; set; }
        [Display(Name = "الوصف")]
        public string OxgnDescription { get; set; }

        public DateTime dateCreate { get; set; }

        [ForeignKey("Department")]
        public int Dept_Id { get; set; }

        public Department Department { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
