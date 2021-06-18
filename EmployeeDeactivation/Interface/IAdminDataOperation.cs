using EmployeeDeactivation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Interface
{
   public interface IAdminDataOperation
    {
        List<Teams> RetrieveSponsorDetails();
        Task<bool> AddSponsorData(string teamName, string sponsorFirstName, string sponsorLastName, string sponsorGid, string sponsorEmail, string sponsorDepartment, string reportingManagerEmail);
        Task<bool> DeleteSponsorData(string gId);
        List<DeactivatedEmployeeDetails> RetrieveEmployeeDetails();
        List<DeactivatedEmployeeDetails> DeactivationEmployeeData();
        List<ActivationWorkflowModel> ActivationEmployeeData();
    }
}
