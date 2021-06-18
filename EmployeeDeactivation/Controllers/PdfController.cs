using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeDeactivation.Interface;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf;
using System.Drawing;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System.Net.Mail;
using EmployeeDeactivation.Models;
using System.Net;

namespace EmployeeDeactivation.Controllers
{

    public class PdfController : Controller
    {
        private readonly IPdfDataOperation _pdfDataOperation;

        public PdfController(IPdfDataOperation pdfDataOperation)
        {
            _pdfDataOperation = pdfDataOperation;
        }
        [HttpGet]
        [Route("Pdf/Index")]
        public IActionResult Index(string GId)
        {
           var bytes = _pdfDataOperation.FillPdfForm(GId);
           return Json("data:application/pdf;base64," + Convert.ToBase64String(bytes));
        }

        [HttpGet]
        [Route("Pdf/ActivationForm")]
        public IActionResult ActivationForm(string GId)
        {
            var bytes = _pdfDataOperation.FillActivationPdfForm(GId);
            return Json("data:application/pdf;base64," + Convert.ToBase64String(bytes));
        }


        [HttpPost]
        [Route("Pdf/PdfAttachment")]
        public void PdfAttachment(string memoryStream,string employeeName, string teamName)
        {
            _pdfDataOperation.SendPdfAsEmailAttachmentDeactivation( memoryStream,  employeeName, teamName);
        }

        [HttpPost]
        [Route("Pdf/PdfAttachmentActivation")]
        public void PdfAttachmentActivation(string memoryStream, string employeeName, string teamName, string sponsorGID, string siemensGid)
        {
            _pdfDataOperation.SendPdfAsEmailAttachmentActivation(memoryStream, employeeName, teamName, sponsorGID, siemensGid);
        }

        [HttpGet]
        [Route("Pdf/SendReminder")]
        public void SendReminder()
        {
            _pdfDataOperation.SendReminderEmail();
        }

        [HttpPost]
        [Route("Pdf/DeclineEmail")]
        public void DeclineEmail( string gid, string employeeName)
        {
            _pdfDataOperation.SendEmailDeclined(gid, employeeName);
        }


    }
}
