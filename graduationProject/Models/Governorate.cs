using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Models
{
    public class Governorate
    {
        [Key]
        public int Gov_Id { get; set; }
        [Required]
        [Display(Name = "المحافظة")]
        [Remote(action: "GovernorateName", controller: "Governorate")]
        public string Gov_Name { get; set; }
    }
}
