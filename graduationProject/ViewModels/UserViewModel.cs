using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        //[Display(Name = "الاسم")]
        public string FirstName { get; set; }

        //[Display(Name = "الاسم")]
        public string LastName { get; set; }

        //[Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }

        //[Display(Name = "البريد الالكترونى")]
        public string Email { get; set; }

        //[Display(Name = "")]
        public IEnumerable<string> Roles { get; set; }
    }
}
