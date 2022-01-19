using graduationProject.Models;
using graduationProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Controllers.Api
{
    [Route("api/[controller]")] // write api/users
    [ApiController] //اللى هو هيستخدمه ا بي اي كنترولر
    
    public class Userreservation : ControllerBase
    {
        private readonly InterfaceRepository<UserReservation> _userReserveRepository;

        public Userreservation(InterfaceRepository<UserReservation> UserReserveRepository)
        {
            _userReserveRepository = UserReserveRepository;
        }
        [HttpDelete]
        public ActionResult Delete(int Id)
        {
            var user = _userReserveRepository.List().Where(a => a.Id == Id).SingleOrDefault();
            
            if (user == null)
                return NotFound();
            try {

                _userReserveRepository.DeleteById(Id);
                return Ok();
            }
            catch {
                throw new Exception();
            }

        }
    }
}
