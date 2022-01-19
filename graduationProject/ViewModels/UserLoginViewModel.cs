using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace graduationProject.ViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        //[EmailAddress]
        [Display(Name = "اسم المستخدم / البريد الالكتروني")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر")]
        public string Password { get; set; }

        [Display(Name = "تذكرني؟")]
        public bool RememberMe { get; set; }

        
        


    }
}
