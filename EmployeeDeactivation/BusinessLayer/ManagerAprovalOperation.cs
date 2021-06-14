using EmployeeDeactivation.Data;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeDeactivation.BusinessLayer
{
    public class ManagerApprovalOperation: IManagerApprovalOperation
    {
        private readonly EmployeeDeactivationContext _context;
        public ManagerApprovalOperation(EmployeeDeactivationContext context)
        {
            _context = context;
        }

        public void PdfAttachment(string employeeName, string lastWorkingDatee, string gId, string teamName, string sponsorName, string memoryStream, string reportingManagerEmail)
        {
            byte[] bytes = System.Convert.FromBase64String(memoryStream);
            ManagerApprovalStatus ManagerApprovalStatus = new ManagerApprovalStatus()
            {
                EmployeeName = employeeName,
                GId = gId,
                LastworkingDate = lastWorkingDatee,
                TeamName = teamName,
                SponsorName = sponsorName,
                PdfAttachment = bytes,
                ReportingManagerEmail= reportingManagerEmail,
                Status="pending"
            };
            _context.Add(ManagerApprovalStatus);
            _context.SaveChanges();

        }

        public List<ManagerApprovalStatus> RetrieveDeactivationDetailss()
        {
            List<ManagerApprovalStatus> deactivationDetails = new List<ManagerApprovalStatus>();
            var infoo = _context.ManagerApprovalStatus.ToList();
            foreach (var item in infoo)
            {
                    deactivationDetails.Add(new ManagerApprovalStatus
                {
                    EmployeeName = item.EmployeeName,
                    LastworkingDate = item.LastworkingDate,
                    GId = item.GId,
                    TeamName = item.TeamName,
                    SponsorName = item.SponsorName,
                    PdfAttachment = item.PdfAttachment,
                    ReportingManagerEmail = item.ReportingManagerEmail,
                    Status = item.Status

                });
            }
            return deactivationDetails;
        }

        public byte[] Getpdf(string GId)
        {
            var DDetails = RetrieveDeactivationDetailss();
            foreach (var item in DDetails)
            {
                if (item.GId == GId)
                {
                    byte[] be = item.PdfAttachment;
                    return be;
                }
            }
            byte[] bb = null;
            return bb;
        }

        public List<ManagerApprovalStatus> GetPendingDeactivationWorkflowForParticularManager(string userEmail)
        {
            
            List<ManagerApprovalStatus> pendingDeactivationWorkflows = new List<ManagerApprovalStatus>();
            var allDeactivationWorkfolw = (RetrieveDeactivationDetailss());
            foreach (var item in allDeactivationWorkfolw)
            {
                if (item.Status.ToLower() == "pending" /*&& item.ReportingManagerEmail == userEmail*/)
                {
                    pendingDeactivationWorkflows.Add(item);
                }

            }
            return pendingDeactivationWorkflows;
        }

        public List<ManagerApprovalStatus> GetAllPendingDeactivationWorkflows()
        {

            List<ManagerApprovalStatus> pendingDeactivationWorkflows = new List<ManagerApprovalStatus>();
            var allDeactivationWorkfolw = (RetrieveDeactivationDetailss());
            foreach (var item in allDeactivationWorkfolw)
            {
                if (item.Status.ToLower() == "pending")
                {
                    pendingDeactivationWorkflows.Add(item);
                }

            }
            return pendingDeactivationWorkflows;
        }

        public List<ManagerApprovalStatus> GetAllApprovedDeactivationWorkflows()
        {

            List<ManagerApprovalStatus> pendingDeactivationWorkflows = new List<ManagerApprovalStatus>();
            var allDeactivationWorkfolw = (RetrieveDeactivationDetailss());
            foreach (var item in allDeactivationWorkfolw)
            {
                if (item.Status.ToLower() == "approve")
                {
                    pendingDeactivationWorkflows.Add(item);
                }

            }
            return pendingDeactivationWorkflows;
        }

        public bool ApproveRequest(string gId)
        {
            var check = _context.ManagerApprovalStatus.ToList();
            foreach (var i in check)
            {
                if (i.GId == gId && i.Status.ToLower() == "pending")
                {
                    i.Status = "approve";
                    _context.SaveChanges();
                    return true;          
                }
            }
            return false;
        }

        public bool DeclineRequest(string gId)
        {
            var check = _context.ManagerApprovalStatus.ToList();
            foreach (var i in check)
            {
                if (i.GId == gId && i.Status.ToLower() == "pending")
                {
                    i.Status = "denied";
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

    }
    
}
