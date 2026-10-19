using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("MetricSpecification")]
    public class MetricSpecification : DelEntity
    {
        public int MetricId { get; set; }
        public int ApplicationId { get; set; }
        public int ClientId { get; set; }
        public int AccountId { get; set; }
        public int ContractId { get; set; }
        public double MetricBaseline { get; set; }
        public double TargetUpperLimit { get; set; }
        public double TargetLowerLimit { get; set; }
        public double MetricTarget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Boolean HasSLA { get; set; }
        public int? MetricOwnerId { get; set; }
        public int MetricStatusId { get; set; }
        public string MetricName { get; set; }
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
        public string ClientPriority { get; set; }
        public int Clocksetup { get; set; }
        public int? Weight { get; set; }
    }
}
