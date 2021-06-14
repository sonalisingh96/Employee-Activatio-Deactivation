using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public class ActivationWorkflowModel
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SiemensEmailId { get; set; }
        [Key]
        public string SiemensGID { get; set; }
        public string Team { get; set; }
        public string SponsorName { get; set; }
        public string SponsorEmail { get; set; }
        public string SponsorGid { get; set; }
        public string SponsorDepartment { get; set; }
        public string ReportingManagerEmail { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceofBirth { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Nationality { get; set; }
        public byte[] PdfData { get; set; }

    }
}

