using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int VersionNumber { get; set; }
        public int ServiceTypeId { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ServiceCriticalityId { get; set; }
        public int ServiceOwnerId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int ScheduleId { get; set; }
        public int WorkflowTemplateId { get; set; }
        public int WorkflowStatusId { get; set; }
        public string Problems { get; set; }
        public int CompanyId { get; set; }
        public int SLAId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        //public DateTime RowVersion { get; set; }
    }
}