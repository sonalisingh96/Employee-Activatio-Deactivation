using EmployeeDeactivation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Interface
{
    public interface IManagerApprovalOperation
    {
        void PdfAttachment(string employeeName, string lastWorkingDatee, string gId, string teamName, string sponsorName, string memoryStream, string reportingManagerEmail);
        List<ManagerApprovalStatus> RetrieveDeactivationDetailss();
        byte[] Getpdf(string GId);
        List<ManagerApprovalStatus> GetPendingDeactivationWorkflowForParticularManager(string userEmail);
        List<ManagerApprovalStatus> GetAllPendingDeactivationWorkflows();
        List<ManagerApprovalStatus> GetAllApprovedDeactivationWorkflows();
        bool ApproveRequest(string gId);
        bool DeclineRequest(string gId);
    }
}
