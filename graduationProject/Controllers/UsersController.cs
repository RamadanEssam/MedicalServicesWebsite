using graduationProject.Models;
using graduationProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static graduationProject.Helper;

namespace graduationProject.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            //to get all user with his role
            var users = await _userManager.Users.Where(u => u.status == 1).Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result //to get all roles to that user
            }).ToListAsync();
            return View(users);
        }
        //to manage roles for users
        [NoDirectAccess]
        public async Task<IActionResult> ManageRoles(string id)
        {
            //to check if user exit or not
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            else
            {
                var roles = await _roleManager.Roles.ToListAsync();
                var viewModel = new UserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = roles.Select(role => new RoleViewModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                    }).ToList()
                };
                return View(viewModel);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                //هشوف هل فى مستخدم ولا لا 
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound();
                }
                //هيجيب كل ال الرول اللى تخص المستخدم دا
                var userRoles = await _userManager.GetRolesAsync(user);
                //همشي على كل واحده من الورل اللى جوا المودل اللى جايه من الصفحه واقارنها بالموجود
                foreach (var role in model.Roles)
                {
                    //هشوف هيا موجوده ومتعلم عليها ولا لا وكذلك موجوده ومش متعلم ولا لا
                    if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                    }
                    if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                    {
                        await _userManager.AddToRoleAsync(user, role.RoleName);
                    }
                }
                ///get all users
                var users = await _userManager.Users.Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = _userManager.GetRolesAsync(user).Result //to get all roles to that user
                }).ToListAsync();
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllUser", users) });

            }
            else
            {
                ModelState.AddModelError("", "You have to fill all the required fields");
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "EditProfile", model) });

            }


        }

        //to create new user
        //public async Task<IActionResult> Add()
        //{
        //    //هجيب كل الرول من جدول الرول بس تيجي ايدي واسم وبس
        //    var roles = await _roleManager.Roles.
        //        Select(r => new RoleViewModel {
        //            RoleId =r.Id,
        //            RoleName = r.Name
        //            }).ToListAsync();
        //    var viewModel = new AddUserViewModel
        //    {
        //        Roles = roles
        //    };
        //    return View(viewModel);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Add(AddUserViewModel model)
        //{
        //    //هيفحص هل المودل اللى جاى فيه داتا ولا لا
        //    if (ModelState.IsValid)
        //    {
        //        //هيشوف هو على الاقل اختار رول واحده ولا لا
        //        if(!model.Roles.Any(r => r.IsSelected))
        //        {
        //            ModelState.AddModelError("Roles", "Please Select At Least one Role");
        //            return View(model);
        //        }
        //        //هيشوف هل الايميل واليوزرنيم موجودين قبل كدا ولا لا
        //        if (await _userManager.FindByEmailAsync(model.Email)!=null)
        //        {
        //            ModelState.AddModelError("Email", "Email is Aleardy Exit");
        //            return View(model);
        //        }
        //        if (await _userManager.FindByNameAsync(model.UserName) != null)
        //        {
        //            ModelState.AddModelError("UserName", "UserName is Aleardy Exit");
        //            return View(model);
        //        }
        //        //هنا بقي ابدا اخزنها فى الداتا بيز بتاعت المستخدم هعمل اوبجكت
        //        var user = new ApplicationUser
        //        {
        //            UserName = model.UserName,
        //            Email = model.Email,
        //            FirstName = model.FirstName,
        //            LastName = model.LastName,
        //            FullName = model.FirstName + model.LastName
        //        };
        //        var result = await _userManager.CreateAsync(user, model.Password);
        //        //هشوف هل تم تخزينه فعلا ولا لا 
        //        if (!result.Succeeded)
        //        {
        //            //هدور على الايرور فين 
        //            foreach (var error in result.Errors)
        //            {
        //                //هحطها فى الرول عشان اخر حاجه تحت وتبان 
        //                ModelState.AddModelError("Roles", error.Description);
        //            }
        //            return View(model);
        //        }
        //        //هنا بقي طالما نجح التسجيل يبقي اضيفه فى جدول الرول واستخدم 
        //        //واراعي حتت السليكت عشان ياخد اللى اتحددت بس مش كلهم
        //        await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r=>r.RoleName));
        //        return RedirectToAction(nameof(Index));

        //    }
        //    else
        //    {
        //        return View(model);
        //    }
        //}
        // جزء تعديل الصفحه الشخصية للمستخدم
        [NoDirectAccess]
        public async Task<IActionResult> EditProfile(string id)
        {
            //هجيب المستخدم بي الاى دى بتاعه
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var viewModel = new ProfileViewModel
                {
                    Id = id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName
                };
                return View(viewModel);
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                //هو هيجيب المستخدم ويشوفه موجود ولا لا
                var user = await _userManager.FindByIdAsync(model.Id);
                if(user == null)
                {
                    return NotFound();
                }
                //هيشوف بي الاي دى هيجيب الايميل موجود ولا لا
                var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
                if(userWithSameEmail != null && userWithSameEmail.Id != model.Id)
                {
                    ModelState.AddModelError("Email", "email is aleardy exit");
                    //return View(model);
                    return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "EditProfile", model) });

                }
                //هيشوف اليوزر نيم موجود قبل كدا ولا لا
                var userWithSameUsername = await _userManager.FindByNameAsync(model.UserName);
                if(userWithSameUsername!=null && userWithSameUsername.Id != model.Id)
                {
                    ModelState.AddModelError("UserName", "UserName is aleardy exit");
                    //return View(model);
                    return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "EditProfile", model) });

                }
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                await _userManager.UpdateAsync(user);

                ///get all users
                var users = await _userManager.Users.Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = _userManager.GetRolesAsync(user).Result //to get all roles to that user
                }).ToListAsync();
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllUser", users) });

            }
            else
            {
                ModelState.AddModelError("", "You have to fill all the required fields");
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "EditProfile", model) });

            }
        }
        /////// هنا جزء البوب اب/////////////////////
        
        [NoDirectAccess]
        public async Task<IActionResult> Add(int id)
        {
            //هجيب كل الرول من جدول الرول بس تيجي ايدي واسم وبس
            var roles = await _roleManager.Roles.
                Select(r => new RoleViewModel
                {
                    RoleId = r.Id,
                    RoleName = r.Name
                }).ToListAsync();
            var viewModel = new AddUserViewModel
            {
                Roles = roles
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int id, AddUserViewModel model)
        {
            //هيفحص هل المودل اللى جاى فيه داتا ولا لا
            if (ModelState.IsValid)
            {
                //هيشوف هو على الاقل اختار رول واحده ولا لا
                if (!model.Roles.Any(r => r.IsSelected))
                {
                    ModelState.AddModelError("Roles", "Please Select At Least one Role");
                    //return View(model);
                    return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Add", model) });

                }
                //هيشوف هل الايميل واليوزرنيم موجودين قبل كدا ولا لا
                if (await _userManager.FindByEmailAsync(model.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email is Aleardy Exit");
                    //return View(model);
                    return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Add", model) });


                }
                if (await _userManager.FindByNameAsync(model.UserName) != null)
                {
                    ModelState.AddModelError("UserName", "UserName is Aleardy Exit");
                    //return View(model);
                    return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Add", model) });

                }
                //هنا بقي ابدا اخزنها فى الداتا بيز بتاعت المستخدم هعمل اوبجكت
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    status = 1
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                //هشوف هل تم تخزينه فعلا ولا لا 
                if (!result.Succeeded)
                {
                    //هدور على الايرور فين 
                    foreach (var error in result.Errors)
                    {
                        //هحطها فى الرول عشان اخر حاجه تحت وتبان 
                        ModelState.AddModelError("Roles", error.Description);
                    }
                    return View(model);
                }
                //هنا بقي طالما نجح التسجيل يبقي اضيفه فى جدول الرول واستخدم 
                //واراعي حتت السليكت عشان ياخد اللى اتحددت بس مش كلهم
                await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName));
                var users = await _userManager.Users.Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = _userManager.GetRolesAsync(user).Result //to get all roles to that user
                }).ToListAsync();
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllUser", users) });

            }
            else
            {
                ModelState.AddModelError("", "You have to fill all the required fields");
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Add", model) });

            }
        }


    }
}
