using graduationProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Controllers.Api
{
    [Route("api/[controller]")] // write api/users
    [ApiController] //اللى هو هيستخدمه ا بي اي كنترولر
    [Authorize(Roles ="Admin")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpDelete]
        //public async Task<IActionResult> Delete(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //        return NotFound();
        //    var result = await _userManager.DeleteAsync(user);
        //    if (!result.Succeeded)
        //        throw new Exception();

        //    return Ok();
        //}
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            user.status = 0;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception();

            return Ok();
        }

    }
}
