using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.App_Start;

namespace WebUI.Models
{
    public class LeadershipDetails
    {
        [Key]
        public int Id { get; set; }
       // public string UserId { get; set; }
        public int? ClientId { get; set; }
        [StringLength(50)]
        public string EmailAddress { get; set; }
        
        [StringLength(50)]
        public string FirstName { get; set; }
    
        [StringLength(50)]
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Shore { get; set; }
        public int? CountryID { get; set; }
       [Required]
        public int? ShoreID { get; set; }
        
        public string Organization { get; set; }
        public bool NttResource { get;set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string CreatedByName { get; set; }
        public String UpdatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [StringLength(50)]
        public string ReportingManager { get; set; }
        public string LeadRole { get; set; }
        [Required]
        public int? LeadRoleID { get; set; }
      

    }
}