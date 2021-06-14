using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeDeactivation.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Dynamic;
using EmployeeDeactivation.BusinessLayer;
using EmployeeDeactivation.Interface;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Web;
using System.Net;
using System.Security.Claims;

namespace EmployeeDeactivation.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeDataOperation _employeeDataOperation;

        public EmployeesController(IEmployeeDataOperation employeeDataOperation)
        {
            _employeeDataOperation = employeeDataOperation;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ActivationPage()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
               var userName = GetUserName(User);
            }
            return View();

        }
        public static string GetUserName(ClaimsPrincipal User)
        {
            return (User.Identities.FirstOrDefault().Claims.Where(c => c.Type.Equals("name")).FirstOrDefault().Value);
        }



        [HttpGet]
        [Route("Employees/GetSponsorDetails")]
        public JsonResult GetSponsorDetails()
        {
            return Json(_employeeDataOperation.RetrieveAllSponsorDetails());    
        }


        [HttpPost]
        [Route("Employees/AddDetails")]
        public JsonResult AddDetails(string firstName, string lastName, string gId, string email, DateTime lastWorkingDate, string teamsName, string sponsorName, string sponsorEmailId, string sponsorDepartment, string sponsorGID)
        {
            var updateStatus =  _employeeDataOperation.AddEmployeeData(firstName, lastName, gId, email, lastWorkingDate, teamsName, sponsorName, sponsorEmailId, sponsorDepartment, sponsorGID);

               return Json(true); 
        }

        [HttpPost]
        [Route("Employees/AddActivationDetails")]
        public JsonResult AddActivationDetails(string firstName, string lastName, string siemensEmailId, string siemensgId, string team, string sponsorName, string sponsorEmailId, string sponsordepartment, string sponsorGID, string reportingManagerEmailId, string employeeRole, string gender, DateTime dob, string pob, string address, string phoneNo, string nationality)
        {
            var updateStatus = _employeeDataOperation.AddActivationEmployeeData(firstName, lastName, siemensEmailId, siemensgId, team, sponsorName, sponsorEmailId, sponsordepartment, sponsorGID, reportingManagerEmailId, employeeRole, gender, dob, pob, address, phoneNo, nationality);

            return Json(true);
        }


    }
}
