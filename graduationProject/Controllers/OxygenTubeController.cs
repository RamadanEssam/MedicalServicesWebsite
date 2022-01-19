using graduationProject.Models;
using graduationProject.Repositories;
using graduationProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static graduationProject.Helper;

namespace graduationProject.Controllers
{
    public class OxygenTubeController : Controller
    {
        private readonly InterfaceRepository<OxygenTube> oxygenTubeRepository;
        private readonly InterfaceRepository<Department> departmentRepository;
        private readonly InterfaceRepository<UserReservationOxegin> userReserveRepository;
       

        public OxygenTubeController(InterfaceRepository<OxygenTube> oxygenTubeRepository,
                                  InterfaceRepository<Department> departmentRepository,
                                  InterfaceRepository<UserReservationOxegin> UserReserveRepository
                                 )
        {
            this.oxygenTubeRepository = oxygenTubeRepository;
            this.departmentRepository = departmentRepository;
            this.userReserveRepository = UserReserveRepository;
           
        }
        // GET: HospitalController
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(oxygenTubeRepository.List());
        }
        /**********department**********/
        deptTubeViewModel getAllDepartment()
        {
            var viewModel = new deptTubeViewModel
            {
                Departments = departmentRepository.List().ToList()
            };
            return viewModel;
        }
        /*************Get All User Reserve in hospital*****************/
        //get all user reserve in hospital
        [Authorize(Roles = "Admin")]
        public ActionResult GetAllUserReserve()
        {
            return View(userReserveRepository.List());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetUserGroupByTube()
        {
            
            var Tubes = userReserveRepository.List().GroupBy(g => g.OxygenTube.OxgnType).Select(s => new TubesViewModel { OxgnType = s.Key, UserReservationOxegin = s });
            return View(Tubes);
        }
        /*****************add or delete and list by publiher********************/
        // get all user reserve in specafic hospital
        [Authorize(Roles = "OxginAdmin")]
        public ActionResult GetUserByTube()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tube = oxygenTubeRepository.List().Where(u => u.UserId == userId).Select(a => a.OxgnId).SingleOrDefault();
            var users = userReserveRepository.List().Where(a => a.Oxygen_Id == tube);
            return View(users);
        }
        // get hospital that publisher create
        [Authorize(Roles = "OxginAdmin")]
        public ActionResult GetTubeByPublisher()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tube = oxygenTubeRepository.List().Where(a => a.UserId == userId);
            return View(tube);
        }
        [Authorize(Roles = "OxginAdmin")]
        //[NoDirectAccess]
        public ActionResult AddOrEditbyPublisher(int id = 0)
        {
            //هنا هو بيشوف لو الاى دى صفر يبقي هو بينشاء مستخدم لو مش بصفر يبقي بيعدل عليه
            if (id == 0)
            {
                return View(getAllDepartment());
            }
            else
            {
                var tube = oxygenTubeRepository.Find(id);
                var deptId = tube.Department == null ? 0 : tube.Department.Dept_Id;
                var viewModel = new deptTubeViewModel
                {
                    OxgnId= tube.OxgnId,
                    OxgnType = tube.OxgnType,
                    OxgnAmount = tube.OxgnAmount,
                    OxgnCost = tube.OxgnCost,
                    OxgnDescription = tube.OxgnDescription,
                    OxgnLocation = tube.OxgnLocation,
                    OxgnPhone = tube.OxgnPhone,
                    dateCreate = tube.dateCreate,
                    Dept_Id = deptId,
                    UserId = tube.UserId,
                    Departments = departmentRepository.List().ToList()
                };
                return View(viewModel);
            }


        }
        //////////////
        // POST: BookController/Add or Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEditbyPublisher(int id, deptTubeViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (id == 0)
                {
                    var hospitalname = oxygenTubeRepository.List().Select(a => a.OxgnType);
                    if (hospitalname.Contains(model.OxgnType))
                    {
                        ModelState.AddModelError("OxgnType", $"OxgnType {model.OxgnType} is already in use.");
                        return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditbyPublisher", getAllDepartment()) });
                    }
                    else
                    {
                        //اجيب المدينه من الجدول بتاعها
                        var department = departmentRepository.Find(model.Dept_Id);
                        var oxygenTube = new OxygenTube
                        {
                            OxgnType = model.OxgnType,
                            OxgnPhone = model.OxgnPhone,
                            OxgnLocation= model.OxgnLocation,
                            OxgnDescription = model.OxgnDescription,
                            OxgnAmount = model.OxgnAmount,
                            OxgnCost = model.OxgnCost,
                            dateCreate = DateTime.Now,
                            UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                            Department = department

                        };
                        oxygenTubeRepository.Add(oxygenTube);

                    }

                }
                else
                {
                    //to upload photo
                    
                    ////////////////////////////////
                    var department = departmentRepository.Find(model.Dept_Id);
                    var oxygenTube = new OxygenTube
                    {
                        OxgnType = model.OxgnType,
                        OxgnPhone = model.OxgnPhone,
                        OxgnLocation = model.OxgnLocation,
                        OxgnDescription = model.OxgnDescription,
                        OxgnAmount = model.OxgnAmount,
                        OxgnCost = model.OxgnCost,
                        dateCreate = DateTime.Now,
                        UserId = model.UserId,
                        Department = department
                    };
                    oxygenTubeRepository.Update(model.OxgnId, oxygenTube);
                }
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var tubes = oxygenTubeRepository.List().Where(a => a.UserId == userId);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewTubesbyPublisher", tubes) });
            }
            else
            {
                ////to handle error message in validation span
                ModelState.AddModelError("", "You have to fill all the required fields");
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditbyPublisher", getAllDepartment()) });
            }

        }

        // جزء الحذف
        [Authorize(Roles = "OxginAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletebyPublisher(int id)
        {
            try
            {
                oxygenTubeRepository.Delete(id);
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var hospitals = oxygenTubeRepository.List().Where(a => a.UserId == userId);
                return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewTubesbyPublisher", hospitals) });
            }
            catch
            {
                return NotFound();
            }


        }
        /****************end publisher section*******************/
        /****************admin[ add edit delete]*******************/
        ///عشان مفيش حد مباشر ينادى اللينك بتاعها 
        [Authorize(Roles = "Admin")]
        [NoDirectAccess]
        public ActionResult AddOrEdit(int id = 0)
        {
            //هنا هو بيشوف لو الاى دى صفر يبقي هو بينشاء مستخدم لو مش بصفر يبقي بيعدل عليه
            if (id == 0)
            {
                return View(getAllDepartment());
            }
            else
            {
                var tube = oxygenTubeRepository.Find(id);
                var deptId = tube.Department == null ? 0 : tube.Department.Dept_Id;
                var viewModel = new deptTubeViewModel
                {
                    OxgnId = tube.OxgnId,
                    OxgnType = tube.OxgnType,
                    OxgnAmount = tube.OxgnAmount,
                    OxgnCost = tube.OxgnCost,
                    OxgnDescription = tube.OxgnDescription,
                    OxgnLocation = tube.OxgnLocation,
                    OxgnPhone = tube.OxgnPhone,
                    dateCreate = tube.dateCreate,
                    Dept_Id = deptId,
                    UserId = tube.UserId,
                    Departments = departmentRepository.List().ToList()
                };
                return View(viewModel);
            }


        }
        //////////////
        // POST: BookController/Add or Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, deptTubeViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var oxgntube = oxygenTubeRepository.List().Select(a => a.OxgnType);
                    if (oxgntube.Contains(model.OxgnType))
                    {
                        ModelState.AddModelError("OxgnType", $"oxgntube {model.OxgnType} is already in use.");
                        return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", getAllDepartment()) });
                    }
                    else
                    {
                        //to upload photo
                        
                        //اجيب المدينه من الجدول بتاعها
                        var department = departmentRepository.Find(model.Dept_Id);
                        var OxygenTube = new OxygenTube
                        {
                            OxgnType = model.OxgnType,
                            OxgnPhone = model.OxgnPhone,
                            OxgnLocation = model.OxgnLocation,
                            OxgnDescription = model.OxgnDescription,
                            OxgnAmount = model.OxgnAmount,
                            OxgnCost = model.OxgnCost,
                            dateCreate = DateTime.Now,
                            UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                            Department = department

                        };
                        oxygenTubeRepository.Add(OxygenTube);
                    }
                }
                else
                {
                   
                    ////////////////////////////////
                    var department = departmentRepository.Find(model.Dept_Id);
                    var OxygenTube = new OxygenTube
                    {
                        OxgnId = model.OxgnId,
                        OxgnType = model.OxgnType,
                        OxgnPhone = model.OxgnPhone,
                        OxgnLocation = model.OxgnLocation,
                        OxgnDescription = model.OxgnDescription,
                        OxgnAmount = model.OxgnAmount,
                        OxgnCost = model.OxgnCost,
                        dateCreate = DateTime.Now,
                        UserId = model.UserId,
                        Department = department
                    };
                    oxygenTubeRepository.Update(model.OxgnId, OxygenTube);
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllTubes", oxygenTubeRepository.List()) });


            }
            else
            {
                ////to handle error message in validation span
                ModelState.AddModelError("", "You have to fill all the required fields");
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", getAllDepartment()) });
            }

        }
        // جزء الحذف
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                oxygenTubeRepository.Delete(id);
                return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAllTubes", oxygenTubeRepository.List()) });
            }
            catch
            {
                return NotFound();
            }


        }
        /*************end admin section***************/

        ///*******Department Name Validation**********/
        [AcceptVerbs("GET", "POST")]
        public IActionResult TubeType(deptTubeViewModel model)
        {
            var department = oxygenTubeRepository.List().Select(a => a.OxgnType);

            if (department.Contains(model.OxgnType))
            {
                //ModelState.AddModelError("Gov_Name", $"Email {model.Gov_Name} is already in use.");

                return Json($"OxgnType {model.OxgnType} is already in use.");
            }
            else
            {
                return Json(true);
            }


        }
    }
}
