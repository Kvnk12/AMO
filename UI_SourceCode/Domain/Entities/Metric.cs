using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("Metric")]
    public class Metric : DelEntity
    {
        public string MetricName { get; set; }
        public int MetricTypeId { get; set; }
        public int ServiceId { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ServiceComponentId { get; set; }
        public int MetricPerspectiveId { get; set; }
        public string Objective { get; set; }
        public int ObjectiveTypeId { get; set; }
        public int DefaultCalculationId { get; set; }
        public string AvaliableForAllApps { get; set; }
        public string AvailableForUse { get; set; }
        public bool inUse { get; set; }
        public int MetricUnitId { get; set; }
        public int MetricStatusId { get; set; }
        public string HasSLA { get; set; }
        //public string TargetUpperLimit { get; set; }
        //public string TargetLowerLimit { get; set; }
        //public string MetricTarget { get; set; }
        //public string MetricBaseline { get; set; }
        public int ChartIndicatorId { get; set; }
        public int CalculationId { get; set; }
        public int CalculationDataId { get; set; }
        public int ContextId { get; set; }
        public int RulesId { get; set; }
        public int PrimaryAudienceId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        //public long RowVersion { get; set; }


        public string ObjectiveType { get; set; }
        public string Calculation { get; set; }

        public string CalculationDataElement { get; set; }
        public string ContextualDataElement { get; set; }
        public string ChartIndicator { get; set; }
        public string Rules { get; set; }
        public string PrimaryAudience { get; set; }

        public string MetricUnit { get; set; }
        

    }
}