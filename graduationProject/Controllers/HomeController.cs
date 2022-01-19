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
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using graduationProject.ViewModels;
using System.Diagnostics;

namespace graduationProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
       
        private readonly InterfaceRepository<Governorate> governorateRepository;
        private readonly InterfaceRepository<Department> departmentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly InterfaceRepository<OxygenTube> oxygenTubeRepository;
        private readonly InterfaceRepository<UserReservationOxegin> userReservationOxeginRepository;
        private readonly InterfaceRepository<Hospital> hospitalRepository;
        private readonly InterfaceRepository<UserReservation> userReservationRepository;

        public HomeController(ILogger<HomeController> logger,
                              InterfaceRepository<Hospital> hospitalRepository,
                              InterfaceRepository<UserReservation> userReservationRepository,
                                InterfaceRepository<Governorate> governorateRepository,
                              InterfaceRepository<Department> departmentRepository,
                              UserManager<ApplicationUser> userManager,
                              InterfaceRepository<OxygenTube> oxygenTubeRepository,
                              InterfaceRepository<UserReservationOxegin> userReservationOxeginRepository)
        {
            _logger = logger;
            _userManager = userManager;
            this.oxygenTubeRepository = oxygenTubeRepository;
            this.userReservationOxeginRepository = userReservationOxeginRepository;
            this.governorateRepository = governorateRepository;
            this.departmentRepository = departmentRepository;
            this.hospitalRepository = hospitalRepository;
            this.userReservationRepository = userReservationRepository;
        }
        GovDeptsearchViewModel GetAllGovernorate()
        {
            var governorates = governorateRepository.List().ToList();
            governorates.Insert(0, new Governorate { Gov_Id = -1, Gov_Name = "------من فضلك حدد محافظة------" });
            var viewModel = new GovDeptsearchViewModel
            {
                Governorates = governorates
            };
            return viewModel;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Hadanat()
        {
            var governorates = governorateRepository.List().ToList();
            governorates.Insert(0, new Governorate { Gov_Id = -1, Gov_Name = "من فضلك حدد محافظة" });
            SelectList s = new SelectList(governorates, "Gov_Id", "Gov_Name");
            return View(s);
        }
        [AllowAnonymous]
        public IActionResult getDepartment(int id)
        {
            var departments = departmentRepository.List().Where(g => g.Gov_Id == id).ToList();
            departments.Insert(0, new Department { Dept_Id = -1, Dept_Name = "من فضلك اختار مدينة" });
            SelectList departmentList = new SelectList(departments, "Dept_Id", "Dept_Name");
            return PartialView("_GetDepartmentsPartial", departmentList);
        }
        [AllowAnonymous]
        public IActionResult GetHospitalbyDept(int id)
        {
            var reservationsForHospital = userReservationRepository.List().ToList();
            var hospitals = hospitalRepository.List().Where(g => g.Dept_Id == id);
            foreach (var item in hospitals)
            {
                var myReservations = reservationsForHospital.Where(a => a.Hos_Id == item.Hos_Id).ToList();
                item.Hos_Incubators = item.Hos_Incubators - (myReservations.Select(b => b.Num_Incubators).Sum());
            }

            return PartialView("_GetHospitalsByDeptPartial", hospitals);
        }
        [AllowAnonymous]
        public IActionResult GetHospitalbyName(string name)
        {
            var hospital = hospitalRepository.List().Where(a => a.Hos_Name == name);

            return PartialView("_GetHospitalsByNamePartial", hospital);
        }
        [AllowAnonymous]
        public IActionResult Oxygen()
        {
            var governorates = governorateRepository.List().ToList();
            governorates.Insert(0, new Governorate { Gov_Id = -1, Gov_Name = "من فضلك حدد محافظة" });
            SelectList s = new SelectList(governorates, "Gov_Id", "Gov_Name");
            return View(s);
        }
        [AllowAnonymous]
        public IActionResult GettubesbyDept(int id)
        {
            var reservationsForTube = userReservationOxeginRepository.List().ToList();
            var tubes = oxygenTubeRepository.List().Where(g => g.Dept_Id == id);
            foreach (var item in tubes)
            {
                var myReservations = reservationsForTube.Where(a => a.Oxygen_Id == item.OxgnId).ToList();
                item.OxgnAmount = item.OxgnAmount - (myReservations.Select(b => b.Num_tubes).Sum());
            }

            return PartialView("_GetTubesByDeptPartial", tubes);
        }
        
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        /************get all hospital list**************/
        
        public ActionResult GetAllHospitals()
        {
            return View(hospitalRepository.List());
        }

        [Authorize]
        public async Task<IActionResult> UserSubmitReservation(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var hospital = hospitalRepository.Find(id);
            var user =await  _userManager.FindByIdAsync(userId);
            
            var ViewModel = new ReservationFormViewModel
            {
                Hos_Id = hospital.Hos_Id,
                SSN = user.SSN
            };
             return View(ViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserSubmitReservation(int id, ReservationFormViewModel model)
        {
            
            try
            {
                /*************to get all number without effect hospital table*************/
                var reservationsForHospital = userReservationRepository.List().ToList();
                var hospitalnum = hospitalRepository.Find(model.Hos_Id);
                   var myReservations = reservationsForHospital.Where(a => a.Hos_Id == model.Hos_Id).ToList();
                hospitalnum.Hos_Incubators = hospitalnum.Hos_Incubators - (myReservations.Select(b => b.Num_Incubators).Sum());
                
                /**************************/
                //var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var hospitalIncubators = hospitalRepository.Find(model.Hos_Id);
                if (hospitalIncubators.Hos_Incubators !=0 && hospitalnum.Hos_Incubators >= model.Hos_Incubators)
                {
                    var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await _userManager.FindByIdAsync(userId);
                    user.SSN = model.SSN;
                    await _userManager.UpdateAsync(user);
                    var reservation = new UserReservation
                    {
                        Hos_Id = model.Hos_Id,
                        User_Id = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                        Num_Incubators = model.Hos_Incubators,
                        Res_Date = DateTime.Now
                    };
                    userReservationRepository.Add(reservation);
                    //to updata number of incubators
                    //hospitalIncubators.Hos_Incubators = hospitalIncubators.Hos_Incubators - model.Hos_Incubators;
                    //hospitalRepository.Update(model.Hos_Id, hospitalIncubators);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Oxygen") });
                }
                else
                {
                    ModelState.AddModelError("Hos_Incubators", "لا يوجد عدد كامل");
                    return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "UserSubmitReservation", model) });

                }
            }
           catch
            {
                ////to handle error message in validation span
                ModelState.AddModelError("", "يجب عليك ادخال عدد الحضانات");
                //return View(GetAllGovernorate());

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "UserSubmitReservation", model) });

            }
            
        }
        [Authorize]
        public async Task<IActionResult> UserSubmitTubes(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tube = oxygenTubeRepository.Find(id);
            var user = await _userManager.FindByIdAsync(userId);

            var ViewModel = new ReservationTubeFormViewModel
            {
                OxgnId = tube.OxgnId,
                SSN = user.SSN
            };
            return View(ViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserSubmitTubes(int id, ReservationTubeFormViewModel model)
        {

            try
            {
                ///*************to get all number without effect hospital table*************/
                //var reservationsForHospital = userReservationRepository.List().ToList();
                //var hospitalnum = hospitalRepository.Find(model.Hos_Id);
                //var myReservations = reservationsForHospital.Where(a => a.Hos_Id == model.Hos_Id).ToList();
                //hospitalnum.Hos_Incubators = hospitalnum.Hos_Incubators - (myReservations.Select(b => b.Num_Incubators).Sum());

                /*************to get all number without effect hospital table*************/
                var reservationsFortubes = userReservationOxeginRepository.List().ToList();
                var tubenum = oxygenTubeRepository.Find(model.OxgnId);
                var myReservations = reservationsFortubes.Where(a => a.Oxygen_Id == model.OxgnId).ToList();
                tubenum.OxgnAmount = tubenum.OxgnAmount - (myReservations.Select(b => b.Num_tubes).Sum());

                /**************************/
                //var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var tubeIncubators = oxygenTubeRepository.Find(model.OxgnId);
                if (tubeIncubators.OxgnAmount != 0 && tubeIncubators.OxgnAmount >= model.tube_num)
                {
                    var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await _userManager.FindByIdAsync(userId);
                    user.SSN = model.SSN;
                    await _userManager.UpdateAsync(user);
                    var reservation = new UserReservationOxegin
                    {
                        Oxygen_Id = model.OxgnId,
                        User_Id = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                        Num_tubes = model.tube_num,
                        Res_Date = DateTime.Now
                    };
                    userReservationOxeginRepository.Add(reservation);
                    //to updata number of incubators
                    //hospitalIncubators.Hos_Incubators = hospitalIncubators.Hos_Incubators - model.Hos_Incubators;
                    //hospitalRepository.Update(model.Hos_Id, hospitalIncubators);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "GetAllHospitals") });
                }
                else
                {
                    ModelState.AddModelError("tube_num", "لا يوجد عدد كافى");
                    return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "UserSubmitTubes", model) });

                }
            }
            catch
            {
                ////to handle error message in validation span
                ModelState.AddModelError("", "يجب عليك ادخال عدد الحضانات");
                //return View(GetAllGovernorate());

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "UserSubmitTubes", model) });

            }

        }
    }
}
