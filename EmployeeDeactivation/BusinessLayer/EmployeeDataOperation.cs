using EmployeeDeactivation.Data;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.BusinessLayer
{
    public class EmployeeDataOperation : IEmployeeDataOperation
    {
        private readonly EmployeeDeactivationContext _context;
        public EmployeeDataOperation(EmployeeDeactivationContext context)
        {
            _context = context;
        }

    public string GetReportingManagerEmailId(string teamName)
        {
            var teamDetails = RetrieveAllSponsorDetails();
            foreach (var item in teamDetails)
            {
                if(item.TeamName == teamName)
                {
                    return item.ReportingManagerEmail;
                }
            }
            return "";      
        }

        public string GetSponsorEmailId(string SponsorGid)
        {
            var teamDetails = RetrieveAllSponsorDetails();
            foreach (var item in teamDetails)
            {
                if (item.SponsorGID == SponsorGid)
                {
                    return item.SponsorEmailID;
                }
            }
            return "";
        }

        public  List<DeactivatedEmployeeDetails> SavedEmployeeDetails()
        {
            List<DeactivatedEmployeeDetails> userDetails = new List<DeactivatedEmployeeDetails>();
            var info =  _context.DeactivationWorkflow.ToList();
            foreach (var item in info)
            {
                userDetails.Add(new DeactivatedEmployeeDetails
                {
                    Firstname = item.Firstname,
                    Lastname = item.Lastname,
                    Email = item.Email,
                    GId = item.GId,
                    Date = item.Date,
                    TeamName = item.TeamName,
                    SponsorName =item.SponsorName,
                    SponsorEmailID =item.SponsorEmailID,
                    Department = item.Department,
                    SponsorGId = item.SponsorGId
                });
            }
            return userDetails;
        }

        public List<Teams> RetrieveAllSponsorDetails()
        {
            List<Teams> teamDetails = new List<Teams>();
            var details = _context.Teams.ToList();
            foreach (var item in details)
            {
                teamDetails.Add(new Teams
                {
                    SponsorGID = item.SponsorGID,
                    TeamName = item.TeamName,
                    SponsorFirstName = item.SponsorFirstName,
                    SponsorLastName = item.SponsorLastName,
                    SponsorEmailID = item.SponsorEmailID,
                    Department = item.Department,
                    ReportingManagerEmail = item.ReportingManagerEmail
                });
            }
            return teamDetails;
        }

        public List<ActivationWorkflowModel> RetrieveAllActivationGid()
        {
            List<ActivationWorkflowModel> activationDetails = new List<ActivationWorkflowModel>();
            var details = _context.ActivationWorkflow.ToList();
            foreach (var item in details)
            {
                activationDetails.Add(new ActivationWorkflowModel
                {
                    SiemensGID = item.SiemensGID,
                    
                });
            }
            return activationDetails;
        }

        public async Task<bool> AddEmployeeData(string firstName, string lastName, string gId, string email, DateTime lastWorkingDate, string teamsName, string sponsorName, string sponsorEmailId, string sponsorDepartment ,string sponsorGID)
        //review change make parameters as class
        {
            DeactivatedEmployeeDetails employee = new DeactivatedEmployeeDetails()
            {
                Firstname = firstName,
                Lastname = lastName,
                GId = gId,
                Email = email,
                Date = lastWorkingDate,
                TeamName = teamsName,
                SponsorName = sponsorName,
                SponsorEmailID = sponsorEmailId,
                Department = sponsorDepartment,
                SponsorGId = sponsorGID
            };
            var check = _context.DeactivationWorkflow.ToList();
            foreach (var i in check)
            {
                if (i.GId == gId)
                {
                    _context.Remove(_context.DeactivationWorkflow.Single(a => a.GId == gId));
                    _context.SaveChanges();
                }
            }

            _context.Add(employee);
            var databaseUpdateStatus = await _context.SaveChangesAsync() == 1 ? true : false;
            return databaseUpdateStatus;
        }

        public async Task<bool> AddActivationEmployeeData(string firstName, string lastName, string siemensEmailId, string siemensgId, string team, string sponsorName, string sponsorEmailId, string sponsordepartment, string sponsorGID, string reportingManagerEmailId, string employeeRole, string gender, DateTime dob, string pob, string address, string phoneNo, string nationality)
        //review change make parameters as class
        {
            ActivationWorkflowModel employeeActivate = new ActivationWorkflowModel()
            {
                FirstName = firstName,
                LastName = lastName,
                SiemensEmailId = siemensEmailId,
                SiemensGID = siemensgId,
                Team = team,
                SponsorName = sponsorName,
                SponsorEmail = sponsorEmailId,
                SponsorGid= sponsorGID,
                SponsorDepartment = sponsordepartment,
                ReportingManagerEmail = reportingManagerEmailId,
                Role= employeeRole,
                Gender = gender,
                DateOfBirth = dob,
                PlaceofBirth = pob,
                Address = address,
                PhoneNo = phoneNo,
                Nationality = nationality
            };
            var check = _context.ActivationWorkflow.ToList();
            foreach (var i in check)
            {
                if (i.SiemensGID == siemensgId)
                {
                    _context.Remove(_context.ActivationWorkflow.Single(a => a.SiemensGID == siemensgId));
                    _context.SaveChanges();
                }
            }

            _context.Add(employeeActivate);
            var databaseUpdateStatus = await _context.SaveChangesAsync() == 1 ? true : false;
            return databaseUpdateStatus;
        }

        public DeactivatedEmployeeDetails RetrieveEmployeeDataBasedOnGid(string gId)
        {
            var details = _context.DeactivationWorkflow.ToList();
            foreach (var item in details)
            {
                if (item.GId == gId)
                {
                    return item;
                }
            }
            return new DeactivatedEmployeeDetails();

        }

        public ActivationWorkflowModel RetrieveActivationDataBasedOnGid(string gId)
        {
            var details = _context.ActivationWorkflow.ToList();
            foreach (var item in details)
            {
                if (item.SiemensGID == gId)
                {
                    return item;
                }
            }
            return new ActivationWorkflowModel();

        }

        public bool savepdf(byte[] pdf, string gid)
        
        {
            var check = _context.ActivationWorkflow.ToList();
            foreach (var i in check)
            {
                if (i.SiemensGID == gid)
                {
                    i.PdfData = pdf;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }

}
