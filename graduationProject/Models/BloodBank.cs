using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace graduationProject.Models
{
    public partial class BloodBank
    {
        [Key]
        public int BloodId { get; set; }
        public string BloodType { get; set; }
        public int? BloodCost { get; set; }
        public string BloodPhone { get; set; }
        public string BloodLocation { get; set; }
        public string BloodDescription { get; set; }
        [ForeignKey("Department")]
        public int Dept_Id { get; set; }

        public Department Department { get; set; }
        
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
