using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public class ManagerApprovalStatus
    {
        public string EmployeeName { get; set; }
        public string LastworkingDate { get; set; }
        [Key]
        public string GId { get; set; }
        public string TeamName { get; set; }
        public string SponsorName { get; set; }
        public byte[] PdfAttachment { get; set; }
        public string ReportingManagerEmail { get; set; }
        public string Status { get; set; }
    }
}
