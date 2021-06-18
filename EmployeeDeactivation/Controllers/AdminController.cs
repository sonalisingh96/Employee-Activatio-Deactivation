using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeDeactivation.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDeactivation.Controllers
{
    [Authorize("Admin")]
    public class AdminController : Controller 
    {
        private readonly IAdminDataOperation _adminDataOperation;
        public AdminController(IAdminDataOperation adminDataOperation)
        {
            _adminDataOperation = adminDataOperation;
        }

        public IActionResult AdminPage()
        {
            return View();
        }

        public IActionResult AccountDeactivationDatePage()
        {
            return View(_adminDataOperation.DeactivationEmployeeData());
        }
        

        public IActionResult AccountActivationDetailsPage()
        {
            return View(_adminDataOperation.ActivationEmployeeData());
        }


        [HttpGet]
        [Route("Admin/SponsorDetails")]
        public JsonResult SponsorDetails()
        {
            return Json(_adminDataOperation.RetrieveSponsorDetails());
        }

        [HttpPost]
        [Route("Admin/AddSponsorDetails")]
        public JsonResult AddSponsorDetails(string teamName, string sponsorFirstName, string sponsorLastName, string sponsorGid, string sponsorEmail, string sponsorDepartment, string reportingManagerEmail)
        {
            var updateStatus = _adminDataOperation.AddSponsorData(teamName, sponsorFirstName, sponsorLastName, sponsorGid, sponsorEmail, sponsorDepartment, reportingManagerEmail);

            return Json(true);
        }

        [HttpPost]
        [Route("Admin/DeleteSponsorDetail")]
        public JsonResult DeleteSponsorDetails(string gId)
        {
            

            return Json(_adminDataOperation.DeleteSponsorData(gId));
        }

        [HttpGet]
        [Route("Admin/EmployeeDetails")]
        public JsonResult EmployeeDetails()
        {
            return Json(_adminDataOperation.RetrieveEmployeeDetails());
        }

    }
}