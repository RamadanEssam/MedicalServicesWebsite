using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [Remote("CheckFirstName", "Employees", ErrorMessage = "FirstName already exists.")]
        public string FirstName { get; set; }

        //code here
    }
}
