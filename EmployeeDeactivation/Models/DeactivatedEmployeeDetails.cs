using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EmployeeDeactivation.Models
{
    public class DeactivatedEmployeeDetails
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        [Key]
        public string GId { get; set; }
        public DateTime Date { get; set; }
        public string TeamName { get; set; }
        public string SponsorName { get; set; }
        public string SponsorEmailID { get; set; }
        public string Department { get; set; }
        public string SponsorGId { get; set; }
        
    }
}
