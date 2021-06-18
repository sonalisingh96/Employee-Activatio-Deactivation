using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Interface
{
    public interface IPdfDataOperation
    {
        void SendPdfAsEmailAttachmentDeactivation(string memoryStream, string employeeName, string teamName);
        void SendPdfAsEmailAttachmentActivation(string memoryStream, string employeeName, string teamName, string sponsorGID, string siemensGid);
        byte[] FillPdfForm(string GId);
        byte[] FillActivationPdfForm(string GId);
        void SendReminderEmail();
        void SendEmailDeclined(string gid, string employeeName);


    }
}
