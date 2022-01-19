using graduationProject.Models;
using graduationProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static graduationProject.Helper;

namespace graduationProject.Controllers
{
    [Authorize(Roles ="Admin")]
    public class GovernorateController : Controller
    {
        private readonly InterfaceRepository<Governorate> governorateRepository;

        public GovernorateController(InterfaceRepository<Governorate> governorateRepository)
        {
            this.governorateRepository = governorateRepository;
        }
        // GET: GovernorateController
        public ActionResult Index()
        {

            return View(governorateRepository.List());
        }

        // GET: GovernorateController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: GovernorateController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: GovernorateController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Governorate governorate)
        //{
        //    if (ModelState.IsValid)
        //    {
                
        //        try
        //        {
        //            governorateRepository.Add(governorate);
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "You have to fill all the required fields");
        //        return View();
        //    }
        //}

        //// GET: GovernorateController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    var governorate = governorateRepository.Find(id);
        //    return View(governorate);
        //}

        //// POST: GovernorateController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, Governorate governorate)
        //{
        //    if (ModelState.IsValid) { 
        //        try
        //        {
        //            governorateRepository.Update(id, governorate);
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("Gov_Name", "You have to fill all the required fields");
        //        return View();
        //    }
        //}
        /////////////to make pop up add edit
        ///add or delete
        ///عشان مفيش حد مباشر ينادى اللينك بتاعها 
        [NoDirectAccess]
        public ActionResult AddOrEdit(int id = 0)
        {
            //هنا هو بيشوف لو الاى دى صفر يبقي هو بينشاء مستخدم لو مش بصفر يبقي بيعدل عليه
            if (id == 0)
            {
                return View(new Governorate());
            }
            else
            {
                
                return View(governorateRepository.Find(id));
            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, Governorate model)
        {
            

            if (ModelState.IsValid)
            {
                var governotate = governorateRepository.List().Select(a => a.Gov_Name);
                if (governotate.Contains(model.Gov_Name))
                {
                    ModelState.AddModelError("Gov_Name", $"Email {model.Gov_Name} is already in use.");

                    return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", model) });

                }
                else
                {
                    if (id == 0)
                    {
                        governorateRepository.Add(model);
                    }
                    else
                    {
                        governorateRepository.Update(id, model);
                    }
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllGov", governorateRepository.List()) });

                }
            }
            else
            {
                ////to handle error message in validation span
                ModelState.AddModelError("", "You have to fill all the required fields");
                //return View(GetAllGovernorate());

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", model) });
            }

        }
        // POST: GovernorateController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {

                governorateRepository.Delete(id);
                return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAllGov", governorateRepository.List()) });


            }
            catch
            {
                return NotFound();
            }
        }
        /*******Role Name Validation**********/
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> GovernorateName(Governorate model)
        {
            var governotate = governorateRepository.List().Select(a => a.Gov_Name);
            
            if (governotate.Contains(model.Gov_Name))
            {
                //ModelState.AddModelError("Gov_Name", $"Email {model.Gov_Name} is already in use.");

                return Json($"governorate {model.Gov_Name} is already in use.");


            }
            else
            {
                return Json(true);
            }

            
        }
        /************end role validation**************/
    }

}
