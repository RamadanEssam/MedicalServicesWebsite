using graduationProject.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalApiController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public HospitalApiController(ApplicationDbContext _db)
        {
            db = _db;
        }
        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var postTitle = db.Hospitals.Where(p => p.Hos_Name.Contains(term))
                                            .Select(p => p.Hos_Name).ToList();
                return Ok(postTitle);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
