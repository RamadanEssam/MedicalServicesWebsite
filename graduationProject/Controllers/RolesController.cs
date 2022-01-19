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
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        //when i use await youshoud use async
        public async Task<IActionResult> Index()
        {
            //to list all role
            return View(await _roleManager.Roles.ToListAsync());
        }
        //to add new role and use  role view model
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                //check if model exit in database or not
                if (await _roleManager.RoleExistsAsync(model.Name))
                {
                    ModelState.AddModelError("Name", "Role is exits!");
                    return View("Index", await _roleManager.Roles.ToListAsync());
                }
                else
                {
                    //save new role in role table
                    await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                //return view with the same data if model not valid
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
        }
        [NoDirectAccess]
        public async Task<IActionResult> Edit(string id)
        {
            //هجيب المستخدم بي الاى دى بتاعه
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var rolelist = await _roleManager.FindByNameAsync(model.Name);
                if (rolelist != null)
                {
                    //return Json($"Email {model.Name} is already in use.");
                    ModelState.AddModelError("Name", $"Email {model.Name} is already in use.");
                    return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", model) });

                }
                //هو هيجيب المستخدم ويشوفه موجود ولا لا
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role == null)
                {
                    return NotFound();
                }
                role.Name = model.Name;
                await _roleManager.UpdateAsync(role);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllRole", await _roleManager.Roles.ToListAsync())});

            }
            else
            {
                ModelState.AddModelError("", "You have to fill all the required fields");
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", model) });

            }
        }
        /*******Role Name Validation**********/
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> RoleName(IdentityRole model)
        {
            var role = await _roleManager.FindByNameAsync(model.Name);
            if (role != null)
            {
                return Json($"Roles {model.Name} is already in use.");
            }

            return Json(true);
        }



    }
}
