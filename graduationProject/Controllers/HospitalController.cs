using graduationProject.Models;
using graduationProject.Repositories;
using graduationProject.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

using static graduationProject.Helper;
using Microsoft.AspNetCore.Authorization;

namespace graduationProject.Controllers
{
    
    public class HospitalController : Controller
    {
        private readonly InterfaceRepository<Hospital> hospitalRepository;
        private readonly InterfaceRepository<Department> departmentRepository;
        private readonly InterfaceRepository<UserReservation> userReserveRepository;
        private readonly IHostingEnvironment hosting;

        public HospitalController(InterfaceRepository<Hospital> hospitalRepository,
                                  InterfaceRepository<Department> departmentRepository,
                                  InterfaceRepository<UserReservation> UserReserveRepository,
                                  IHostingEnvironment hosting)
        {
            this.hospitalRepository = hospitalRepository;
            this.departmentRepository = departmentRepository;
            this.userReserveRepository = UserReserveRepository;
            this.hosting = hosting;
        }
        // GET: HospitalController
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(hospitalRepository.List());
        }
        // GET: HospitalController/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            return View(hospitalRepository.Find(id));
        }
        [Authorize(Roles = "Admin")]
        deptHospitalViewModel getAllDepartment()
        {
            var viewModel = new deptHospitalViewModel
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
        public ActionResult GetUserGroupByHospital()
        {
            
            var hospital = userReserveRepository.List().GroupBy(g => g.Hospital.Hos_Name).Select(s => new HospitalViewModel { Hos_Name = s.Key, userResverations = s });
            return View(hospital);
        }
        /*****************add or delete and list by publiher********************/
        // get all user reserve in specafic hospital
        [Authorize(Roles = "HospitalAdmin")]
        public ActionResult GetUserByHospital()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var hospital = hospitalRepository.List().Where(u => u.UserId == userId).Select(a => a.Hos_Id).SingleOrDefault();
            var users = userReserveRepository.List().Where(a => a.Hos_Id == hospital);
            return View(users);
        }
        // get hospital that publisher create
        [Authorize(Roles = "HospitalAdmin")]
        public ActionResult GetHospitalByPublisher()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var hospital = hospitalRepository.List().Where(a => a.UserId == userId);
            return View(hospital);
        }
        [Authorize(Roles = "HospitalAdmin")]
        [NoDirectAccess]
        public ActionResult AddOrEditbyPublisher(int id = 0)
        {
            //هنا هو بيشوف لو الاى دى صفر يبقي هو بينشاء مستخدم لو مش بصفر يبقي بيعدل عليه
            if (id == 0)
            {
                return View(getAllDepartment());
            }
            else
            {
                var hospital = hospitalRepository.Find(id);
                var deptId = hospital.Department == null ? 0 : hospital.Department.Dept_Id;
                var viewModel = new deptHospitalViewModel
                {
                    Hos_Id = hospital.Hos_Id,
                    Hos_Name = hospital.Hos_Name,
                    Hos_Location = hospital.Hos_Location,
                    Hos_Incubators = hospital.Hos_Incubators,
                    Hos_Phone = hospital.Hos_Phone,
                    Hos_Image = hospital.Hos_Image,
                    hos_pic = hospital.hos_pic,
                    DayPrice = hospital.DayPrice,
                    dateCreate = hospital.dateCreate,
                    Dept_Id = deptId,
                    UserId = hospital.UserId,
                    Departments = departmentRepository.List().ToList()
                };
                return View(viewModel);
            }


        }
        //////////////
        // POST: BookController/Add or Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEditbyPublisher(int id, deptHospitalViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                if (id == 0)
                {
                    var hospitalname = hospitalRepository.List().Select(a => a.Hos_Name);
                    if (hospitalname.Contains(model.Hos_Name))
                    {
                        ModelState.AddModelError("Hos_Name", $"Hospital {model.Hos_Name} is already in use.");
                        return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditbyPublisher", getAllDepartment()) });
                    }
                    else
                    {
                        //to upload photo
                        //string fileName = string.Empty;
                        //if (model.File != null)
                        //{
                        //    string uploads = Path.Combine(hosting.WebRootPath, "images/Hospital");
                        //    fileName = model.File.FileName;
                        //    string fullPath = Path.Combine(uploads, fileName);
                        //    model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                            
                        //}
                        //اجيب المدينه من الجدول بتاعها
                        var department = departmentRepository.Find(model.Dept_Id);
                        var hospital = new Hospital
                        {
                            Hos_Name = model.Hos_Name,
                            Hos_Incubators = model.Hos_Incubators,
                            Hos_Phone = model.Hos_Phone,
                            //Hos_Image = fileName,
                            Hos_Location = model.Hos_Location,
                            dateCreate = DateTime.Now,
                            DayPrice = model.DayPrice,
                            UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                            Department = department

                        };
                        //take profile img and insert to DB
                        if (Request.Form.Files.Count > 0)
                        {
                            var file = Request.Form.Files.FirstOrDefault();
                            //check file size and extention
                            using (var dataStream = new MemoryStream())
                            {
                                file.CopyTo(dataStream);
                                hospital.hos_pic = dataStream.ToArray();
                            }
                        }
                        hospitalRepository.Add(hospital);

                    }
                      
                }
                else
                {
                    ////to upload photo
                    //string fileName = string.Empty;
                    //if (model.File != null)
                    //{
                    //    string uploads = Path.Combine(hosting.WebRootPath, "images/Hospital");
                    //    fileName = model.File.FileName;
                    //    string fullPath = Path.Combine(uploads, fileName);
                    //    //delete old path
                    //    string oldFileName = hospitalRepository.Find(model.Hos_Id).Hos_Image;
                    //    string fullOldPath = Path.Combine(uploads, oldFileName);
                    //    if (fullOldPath != fullPath)
                    //    {
                    //        System.IO.File.Delete(fullOldPath);
                    //        //save new file
                    //        model.File.CopyTo(new FileStream(fullPath, FileMode.Create));

                    //    }

                    //}
                    ////////////////////////////////
                    var department = departmentRepository.Find(model.Dept_Id);
                    var hospital = new Hospital
                    {
                        Hos_Id = model.Hos_Id,
                        Hos_Name = model.Hos_Name,
                        Hos_Location = model.Hos_Location,
                        Hos_Incubators = model.Hos_Incubators,
                        Hos_Phone = model.Hos_Phone,
                        //Hos_Image = fileName,
                        DayPrice = model.DayPrice,
                        dateCreate = DateTime.Now,
                        hos_pic = model.hos_pic,
                        UserId = model.UserId,
                        Department = department
                    };
                    //take profile img and insert to DB
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();
                        //check file size and extention
                        using (var dataStream = new MemoryStream())
                        {
                            file.CopyTo(dataStream);
                            hospital.hos_pic = dataStream.ToArray();
                        }
                    }
                    hospitalRepository.Update(model.Hos_Id, hospital);
                }
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var hospitals = hospitalRepository.List().Where(a => a.UserId == userId);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewHospitalsbyPublisher", hospitals) });
            }
            else
            {
                ////to handle error message in validation span
                ModelState.AddModelError("", "You have to fill all the required fields");
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditbyPublisher", getAllDepartment()) });
            }

        }

        // جزء الحذف
        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletebyPublisher(int id)
        {
            try
            {
                hospitalRepository.Delete(id);
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var hospitals = hospitalRepository.List().Where(a => a.UserId == userId);
                return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewHospitalsbyPublisher", hospitals) });
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
                var hospital = hospitalRepository.Find(id);
                var deptId = hospital.Department == null ? 0 : hospital.Department.Dept_Id;
                var viewModel = new deptHospitalViewModel
                {
                    Hos_Id = hospital.Hos_Id,
                    Hos_Name = hospital.Hos_Name,
                    Hos_Location = hospital.Hos_Location,
                    Hos_Incubators = hospital.Hos_Incubators,
                    Hos_Phone = hospital.Hos_Phone,
                    Hos_Image = hospital.Hos_Image,
                    hos_pic = hospital.hos_pic,
                    DayPrice = hospital.DayPrice,
                    dateCreate = hospital.dateCreate,
                    Dept_Id = deptId,
                    UserId=hospital.UserId,
                    Departments = departmentRepository.List().ToList()
                };
                return View(viewModel);
            }


        }
        //////////////
        // POST: BookController/Add or Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, deptHospitalViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var hospitalname = hospitalRepository.List().Select(a => a.Hos_Name);
                    if (hospitalname.Contains(model.Hos_Name))
                    {
                        ModelState.AddModelError("Hos_Name", $"Hospital {model.Hos_Name} is already in use.");
                        return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", getAllDepartment()) });
                    }
                    else {
                        ////to upload photo
                        //string fileName = string.Empty;
                        //if (model.File != null)
                        //{
                        //    string uploads = Path.Combine(hosting.WebRootPath, "images/Hospital");
                        //    fileName = model.File.FileName;
                        //    string fullPath = Path.Combine(uploads, fileName);
                        //    model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                        //}
                        
                        //اجيب المدينه من الجدول بتاعها
                        var department = departmentRepository.Find(model.Dept_Id);
                        var hospital = new Hospital
                        {
                            Hos_Name = model.Hos_Name,
                            Hos_Incubators = model.Hos_Incubators,
                            Hos_Phone = model.Hos_Phone,
                            //Hos_Image = fileName,
                            Hos_Location = model.Hos_Location,
                            dateCreate = DateTime.Now,
                            DayPrice = model.DayPrice,
                            UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                            Department = department

                        };
                        //take profile img and insert to DB
                        if (Request.Form.Files.Count > 0)
                        {
                            var file = Request.Form.Files.FirstOrDefault();
                            //check file size and extention
                            using (var dataStream = new MemoryStream())
                            {
                                file.CopyTo(dataStream);
                                hospital.hos_pic = dataStream.ToArray();
                            }

                        }
                        hospitalRepository.Add(hospital);
                    }
                }
                else
                {
                    ////to upload photo
                    //string fileName = string.Empty;
                    //if (model.File != null)
                    //{
                    //    string uploads = Path.Combine(hosting.WebRootPath, "images/Hospital");
                    //    fileName = model.File.FileName;
                    //    string fullPath = Path.Combine(uploads, fileName);
                    //    //delete old path
                    //    string oldFileName = hospitalRepository.Find(model.Hos_Id).Hos_Image;
                    //    string fullOldPath = Path.Combine(uploads, oldFileName);
                    //    if (fullOldPath != fullPath)
                    //    {
                    //        System.IO.File.Delete(fullOldPath);
                    //        //save new file
                    //        model.File.CopyTo(new FileStream(fullPath, FileMode.Create));

                    //    }

                    //}
                    ////////////////////////////////
                    var department = departmentRepository.Find(model.Dept_Id);
                    var hospital = new Hospital
                    {
                        Hos_Id = model.Hos_Id,
                        Hos_Name = model.Hos_Name,
                        Hos_Location = model.Hos_Location,
                        Hos_Incubators = model.Hos_Incubators,
                        Hos_Phone = model.Hos_Phone,
                        //Hos_Image = fileName,
                        DayPrice = model.DayPrice,
                        dateCreate = DateTime.Now,
                        hos_pic = model.hos_pic,
                        UserId = model.UserId,
                        Department = department
                    };
                    //take profile img and insert to DB
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();
                        //check file size and extention
                        using (var dataStream = new MemoryStream())
                        {
                            file.CopyTo(dataStream);
                            hospital.hos_pic = dataStream.ToArray();
                        }
                    }
                        hospitalRepository.Update(model.Hos_Id, hospital);
                }
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllHospital", hospitalRepository.List()) });

                
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
                hospitalRepository.Delete(id);
                return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAllHospital", hospitalRepository.List()) });
            }
            catch
            {
                return NotFound();
            }


        }
        /*************end admin section***************/
       
        ///*******Department Name Validation**********/
        [AcceptVerbs("GET", "POST")]
        public IActionResult HospitalName(deptHospitalViewModel model)
        {
            var department = hospitalRepository.List().Select(a => a.Hos_Name);

            if (department.Contains(model.Hos_Name))
            {
                //ModelState.AddModelError("Gov_Name", $"Email {model.Gov_Name} is already in use.");

                return Json($"hospital {model.Hos_Name} is already in use.");
            }
            else
            {
                return Json(true);
            }


        }

    }
}
