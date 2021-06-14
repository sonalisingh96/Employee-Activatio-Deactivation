using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmployeeDeactivation.BusinessLayer
{

    public class PdfDataOperation : IPdfDataOperation
    {
        private readonly IEmployeeDataOperation _employeeDataOperation;
        private readonly IManagerApprovalOperation _managerApprovalOperation;
        private readonly IAdminDataOperation _adminDataOperation;

        public PdfDataOperation(IEmployeeDataOperation employeeDataOperation, IManagerApprovalOperation managerApprovalOperation , IAdminDataOperation adminDataOperation)
        {
            _employeeDataOperation = employeeDataOperation;
            _managerApprovalOperation = managerApprovalOperation;
            _adminDataOperation = adminDataOperation;
        }

        public byte[] FillPdfForm(string GId)
        {
            var employeeData = _employeeDataOperation.RetrieveEmployeeDataBasedOnGid(GId);
            FileStream docStream = new FileStream("DeactivationFormPDF.pdf", FileMode.Open, FileAccess.Read);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);
            PdfLoadedForm form = loadedDocument.Form;
            (form.Fields[11] as PdfLoadedTextBoxField).Text = employeeData.Firstname;
            (form.Fields[10] as PdfLoadedTextBoxField).Text = employeeData.Lastname;
            (form.Fields[13] as PdfLoadedTextBoxField).Text = employeeData.Email;
            (form.Fields[12] as PdfLoadedTextBoxField).Text = employeeData.GId;
            (form.Fields[4] as PdfLoadedTextBoxField).Text = employeeData.Date.ToString();
            string sponsorFullName = employeeData.SponsorName;
            string[] splitsponsorFullName = sponsorFullName.Split(' ');
            (form.Fields[16] as PdfLoadedTextBoxField).Text = splitsponsorFullName[0];
            (form.Fields[17] as PdfLoadedTextBoxField).Text = splitsponsorFullName[1];
            (form.Fields[18] as PdfLoadedTextBoxField).Text = employeeData.SponsorGId;
            (form.Fields[19] as PdfLoadedTextBoxField).Text = employeeData.Department;
            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);
            stream.Position = 0;
            loadedDocument.Close(true);
            byte[] bytes = stream.ToArray();
            return bytes;
        }

        public byte[] FillActivationPdfForm(string GId)
        {
            var activationEmployeeData = _employeeDataOperation.RetrieveActivationDataBasedOnGid(GId);
            FileStream docStream = new FileStream("Activationpdf.pdf", FileMode.Open, FileAccess.Read);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);
            PdfLoadedForm form = loadedDocument.Form;
            PdfLoadedComboBoxField loadedListBox = form.Fields[8] as PdfLoadedComboBoxField ;
            for (int i = 0; i < loadedListBox.Values.Count; i++)
            {
                if(loadedListBox.Values[i].Value == activationEmployeeData.Gender)
                {
                    loadedListBox.SelectedValue = activationEmployeeData.Gender;
                }
            }
            (form.Fields[11] as PdfLoadedTextBoxField).Text = activationEmployeeData.FirstName;
            (form.Fields[10] as PdfLoadedTextBoxField).Text = activationEmployeeData.LastName;
            (form.Fields[12] as PdfLoadedTextBoxField).Text = activationEmployeeData.SiemensGID;
            (form.Fields[13] as PdfLoadedTextBoxField).Text = activationEmployeeData.PlaceofBirth;
            (form.Fields[9] as PdfLoadedTextBoxField).Text = activationEmployeeData.DateOfBirth.ToString();
            string sponsorFullName = activationEmployeeData.SponsorName;
            string[] splitsponsorFullName = sponsorFullName.Split(' ');
            (form.Fields[16] as PdfLoadedTextBoxField).Text = splitsponsorFullName[0];
            (form.Fields[17] as PdfLoadedTextBoxField).Text = splitsponsorFullName[1];
            (form.Fields[18] as PdfLoadedTextBoxField).Text = activationEmployeeData.SponsorGid;
            (form.Fields[19] as PdfLoadedTextBoxField).Text = activationEmployeeData.SponsorDepartment;
            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);
            stream.Position = 0;
            loadedDocument.Close(true);
            byte[] bytes = stream.ToArray();
            return bytes;
        }

        public void SendPdfAsEmailAttachment(string memoryStream, string employeeName, string teamName)
        {
            var reportingManagerEmailId = _employeeDataOperation.GetReportingManagerEmailId(teamName);
            byte[] bytes = System.Convert.FromBase64String(memoryStream);
            var c = bytes;
            MemoryStream stream = new MemoryStream(bytes);
            Attachment file = new Attachment(stream, "Deactivation workflow_" + employeeName + ".pdf", "application/pdf");
            SendEmail(reportingManagerEmailId,"","",employeeName,false,file);
        }

        public void SendPdfAsEmailAttachmentActivation(string memoryStream, string employeeName, string teamName, string sponsorGID,string siemensGid)
        {
            var reportingManagerEmailId = _employeeDataOperation.GetReportingManagerEmailId(teamName);
            var sponsorEmailId = _employeeDataOperation.GetSponsorEmailId(sponsorGID);
            byte[] bytes = System.Convert.FromBase64String(memoryStream);
            _employeeDataOperation.savepdf(bytes, siemensGid);
            var c = bytes;
            MemoryStream stream = new MemoryStream(bytes);
            Attachment file = new Attachment(stream, "Activation workflow_" + employeeName + ".pdf", "application/pdf");
            SendEmail(reportingManagerEmailId,"techfox69@gmail.com","amnshuman1998@gmail.com", employeeName, false, file);

        }


        public void SendReminderEmail()
        {       
            var employeeDetails = _managerApprovalOperation.GetAllPendingDeactivationWorkflows();
            foreach (var item in employeeDetails)
            {
                string iDate = item.LastworkingDate;
                DateTime date = Convert.ToDateTime(iDate);
                if (DateTime.Today == date || DateTime.Today > date)
                {
                    MemoryStream stream = new MemoryStream(item.PdfAttachment);
                    Attachment file = new Attachment(stream, "Deactivation workflow_" + item.EmployeeName + ".pdf", "application/pdf");
                    SendEmail(item.ReportingManagerEmail,"","", item.EmployeeName, true,file);
                }  
            }
            var approvedEmployeeDetails = _managerApprovalOperation.GetAllApprovedDeactivationWorkflows();
            var employeeData = _employeeDataOperation.SavedEmployeeDetails();            
            foreach (var item in approvedEmployeeDetails)
            {
                if (DateTime.Today.ToString() == item.LastworkingDate)
                {
                    foreach (var employee in employeeData)
                    {
                        if(employee.GId == item.GId)
                        {
                            MemoryStream stream = new MemoryStream(item.PdfAttachment);
                            Attachment file = new Attachment(stream, "Deactivation workflow_" + item.EmployeeName + ".pdf", "application/pdf");
                            SendEmail("",employee.SponsorEmailID,"", item.EmployeeName, true, file);
                        }
                    }
                }
            }

        }
        private void SendEmail(string reportingManagerEmail , string  sponsorEmailId, string cmEmailId, string employeeName, bool isReminderEmail, Attachment file )
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("dontreplydeactivationworkflow@gmail.com");
            message.Sender = new MailAddress("dontreplydeactivationworkflow@gmail.com");
            message.To.Add(reportingManagerEmail);
            message.To.Add(sponsorEmailId);
            message.To.Add(cmEmailId);
            message.Subject = "Deactivation workflow initiated";
            if (isReminderEmail)
            { 
                message.Body = "Today is " + employeeName+"'s last working day please check if you have approved the deactivation workflow";
            }
            else {
                message.Attachments.Add(file);
            }
            
            message.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("dontreplydeactivationworkflow@gmail.com", "Siemens@Banglore98");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);

        }
    }
}
