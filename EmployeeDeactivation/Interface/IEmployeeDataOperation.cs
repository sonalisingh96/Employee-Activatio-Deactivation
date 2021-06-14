using EmployeeDeactivation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeDeactivation.Interface
{
    public interface IEmployeeDataOperation
    {
        List<Teams> RetrieveAllSponsorDetails();
        Task<bool> AddEmployeeData(string firstName, string lastName, string gId, string email, DateTime lastWorkingDate, string teamName, string sponsorName, string sponsorEmailId, string sponsorDepartment, string sponsorGID);
        DeactivatedEmployeeDetails RetrieveEmployeeDataBasedOnGid(string gId);
        ActivationWorkflowModel RetrieveActivationDataBasedOnGid(string gId);
        string GetReportingManagerEmailId(string teamName);
        string GetSponsorEmailId(string SponsorGid);
        List<DeactivatedEmployeeDetails> SavedEmployeeDetails();
        Task<bool> AddActivationEmployeeData(string firstName, string lastName, string siemensEmailId, string siemensgId, string team, string sponsorName, string sponsorEmailId, string sponsordepartment, string sponsorGID, string reportingManagerEmailId, string employeeRole, string gender, DateTime dob, string pob, string address, string phoneNo, string nationality);

    }
}
