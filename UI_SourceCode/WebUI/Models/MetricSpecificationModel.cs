using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebUI.App_Start;

namespace WebUI.Models
{
    public class MetricSpecificationModel
    {
        public int Id { get; set; }
        public int MetricId { get; set; }
        public int ApplicationId { get; set; }
        public int ClientId { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string ContractName { get; set; }

        public string ClientPriority { get; set; }

        
        public int ContractId { get; set; }
        public double MetricBaseline { get; set; }
        public double TargetUpperLimit { get; set; }
        public double TargetLowerLimit { get; set; }
        public double MetricTarget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public Boolean HasSLA { get; set; }
        public int? MetricOwnerId { get; set; }
        [Required]
        public int MetricStatusId { get; set; }

        public string MetricStatus { get; set; }
        public string MetricName { get; set; }
        [Required]
        public string MetricOwner { get; set; }
        public string MetricSLAName { get; set; }
        public string CalculationData { get; set; }
        public string ComplianceUnits { get; set; }
        public string ComplianceBaseline { get; set; }
        public string ComplianceTarget { get; set; }
        public string ExceedsExpectation { get; set; }
        public string MetExpectationlowerlimit { get; set; }
        public string MetExpectationhigherlimit { get; set; }
        public string BelowExpectation { get; set; }
        private DateTime _DateCreated = DateTime.Now;
        public DateTime CreatedDate { get { return _DateCreated; } set { _DateCreated = value; } }
        public DateTime _DateUpdated = DateTime.Now;
        public DateTime UdatedDate { get { return _DateCreated; } set { _DateCreated = value; } }
        public int Priority { get; set; }
        public int Clocksetup { get; set; }

        public string ClockType { get; set; }
     
        public IEnumerable<SelectListItem> AccountNamesList { get; set; }
        public IEnumerable<SelectListItem> AccContractList { get; set; }
        public IEnumerable<SelectListItem> MetricStatusList { get; set; }
        public IEnumerable<SelectListItem> MetricUnitList { get; set; }
        public string MetricUnit { get; set; }

        public int? Weightage { get; set; }

        public string MetSLAExpectationsBTW { get; set; }

        public string Service { get; set; }

        public string ServiceCategory { get; set; }

       
    }
    public enum SLAClock
    {
        BusinessHours = 1,
        ClockHours = 2
    }

  


}
