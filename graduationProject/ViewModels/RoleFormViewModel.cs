using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace graduationProject.ViewModels
{
    public class RoleFormViewModel
    {
        [Required, StringLength(256)]
        [Remote(action: "RoleName", controller: "Roles")]
        public string Name { get; set; }
    }
}
