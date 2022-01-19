using graduationProject.Data;
using graduationProject.Models;
using graduationProject.Repositories;
using graduationProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static graduationProject.Helper;

namespace graduationProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {


        private readonly InterfaceRepository<Governorate> governorateRepository;
        private readonly InterfaceRepository<Department> departmentRepository;

        public DepartmentController(InterfaceRepository<Governorate> governorateRepository,
                                    InterfaceRepository<Department> departmentRepository)
        {
            this.governorateRepository = governorateRepository;
            this.departmentRepository = departmentRepository;
        }
        // GET: get all department
        public IActionResult Index()
        {

            return View(departmentRepository.List());
        }
        //داله عشان اجيب منها كل المحافظات واستخدمها جاهزة
        GovDepartmentViewModel GetAllGovernorate()
        {
            var governorates = governorateRepository.List().ToList();
            governorates.Insert(0, new Governorate { Gov_Id = -1, Gov_Name = "------من فضلك اختار محافظة------" });
            var viewModel = new GovDepartmentViewModel
            {
                Governorates = governorates
            };
            return viewModel;
        }
        public IActionResult GetPartial()
        {
           return PartialView("_viewPartial", GetAllGovernorate());
        }
        /////////////to make pop up add edit
        ///add or delete
        ///عشان مفيش حد مباشر ينادى اللينك بتاعها 
        [NoDirectAccess]
        public ActionResult AddOrEdit(int id =0)
        {
            //هنا هو بيشوف لو الاى دى صفر يبقي هو بينشاء مستخدم لو مش بصفر يبقي بيعدل عليه
            if(id == 0)
            {
                return View(GetAllGovernorate());
            }
            else
            {
                var department = departmentRepository.Find(id);
                var governorateId = department.Governorate == null ? 0 : department.Governorate.Gov_Id;
                var viewModel = new GovDepartmentViewModel
                {
                    Dept_Id = department.Dept_Id,
                    Dept_Name = department.Dept_Name,
                    Gov_Id = governorateId,
                    Governorates = governorateRepository.List().ToList()
                };
                return View(viewModel);
            }
           

        }
        //////////////
        // POST: BookController/Add or Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, GovDepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var departmentname = departmentRepository.List().Select(a => a.Dept_Name);
                if (departmentname.Contains(model.Dept_Name))
                {
                    ModelState.AddModelError("Dept_Name", $"Department {model.Dept_Name} is already in use.");
                    return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", GetAllGovernorate()) });
                }
                else
                { 
                    if (id == 0)
                    {
                    
                        //هنا هشوف هو اختار من الليست ولا لا
                        if (model.Gov_Id == -1)
                        {
                            ModelState.AddModelError("Gov_Id", "يجب ان تختار من قائمه المحافظات");
                            //return View(GetAllGovernorate());
                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", GetAllGovernorate()) });

                        }
                        var governorate = governorateRepository.Find(model.Gov_Id);
                        var department = new Department
                        {
                            Dept_Id = model.Dept_Id,
                            Dept_Name = model.Dept_Name,
                            Governorate = governorate
                        };
                        departmentRepository.Add(department);
                        //return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Department department = new Department
                        {
                            Dept_Id = model.Dept_Id,
                            Dept_Name = model.Dept_Name,
                            Governorate = governorateRepository.Find(model.Gov_Id)
                        };
                        departmentRepository.Update(model.Dept_Id, department);
                        //return RedirectToAction(nameof(Index));
                    }
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", departmentRepository.List()) });

                }
            }
            else
            {
                ////to handle error message in validation span
                ModelState.AddModelError("", "You have to fill all the required fields");
                //return View(GetAllGovernorate());
                
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", GetAllGovernorate()) });
            }

        }
        // جزء الحذف
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            try
            {
                
                departmentRepository.Delete(id);
                return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", departmentRepository.List()) });


            }
            catch
            {
                return NotFound();
            }
            

        }
        ///*******Department Name Validation**********/
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> DepartmentName(GovDepartmentViewModel model)
        {
            var department = departmentRepository.List().Select(a => a.Dept_Name);

            if (department.Contains(model.Dept_Name))
            {
                //ModelState.AddModelError("Gov_Name", $"Email {model.Gov_Name} is already in use.");

                return Json($"department {model.Dept_Name} is already in use.");
            }
            else
            {
                return Json(true);
            }


        }


        //end of create or edit
        //public ActionResult Edit(int id)
        //{
        //    var department = departmentRepository.Find(id);
        //    //هشوف هل المحافظه فارغه ولا لا 
        //    var governorateId = department.Governorate == null ? 0 : department.Governorate.Gov_Id;
        //    var viewModel = new GovDepartmentViewModel
        //    {
        //        Dept_Id = department.Dept_Id,
        //        Dept_Name = department.Dept_Name,
        //        Gov_Id = governorateId,
        //        Governorates = governorateRepository.List().ToList()
        //    };
        //    return View(viewModel);

        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, GovDepartmentViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Department department = new Department
        //        {
        //            Dept_Name = model.Dept_Name,
        //            Governorate = governorateRepository.Find(model.Gov_Id)
        //        };
        //        departmentRepository.Update(model.Dept_Id, department);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "You have to fill all the required fields");
        //        return View();
        //    }
        //}

    }


    //public ActionResult check(string email)
    //{
    //   var us = db.Userdatas.Where(n => n.email == email).FirstOrDefault();
    //    if (us == null)
    //    {
    //        return Json(true, JsonRequestBehavior.AllowGet);
    //    }
    //    else
    //    {
    //        return Json(false, JsonRequestBehavior.AllowGet);
    //    }
    //}



}

