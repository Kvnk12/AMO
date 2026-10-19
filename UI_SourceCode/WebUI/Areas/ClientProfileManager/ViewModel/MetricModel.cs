using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebUI.App_Start;

namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class MetricModel
    {
        public int Id { get; set; }
        public string MetricName { get; set; }
        public int MetricTypeId { get; set; }
        public string MetricType { get; set; }
        public int ServiceId { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ServiceComponentId { get; set; }
        public int MetricPerspectiveId { get; set; }
        public string MetricPerspective { get; set; }
        public string Objective { get; set; }
     
        public int ObjectiveTypeId { get; set; }
        public string ObjectiveType { get; set; }
        public int DefaultCalculationId { get; set; }
        public string AvaliableForAllApps { get; set; }
        public string AvailableForUse { get; set; }
        public bool inUse { get; set; }
        public int MetricUnitId { get; set; }
        public string MetricUnit { get; set; }
        public int MetricStatusId { get; set; }
        public string HasSLA { get; set; }
        //public string TargetUpperLimit { get; set; }
        //public string TargetLowerLimit { get; set; }
        //public string MetricTarget { get; set; }
        //public string MetricBaseline { get; set; }
        public int ChartIndicatorId { get; set; }
        public string ChartIndicator{ get; set; }
        public int CalculationId { get; set; }

        public string Calculation { get; set; }
        public int CalculationDataId { get; set; }
        public string CalculationDataElement { get; set; }

        public int ContextId { get; set; }
        public string ContextualDataElement { get; set; }
        public int RulesId { get; set; }
        public string Rules { get; set; }
        public int PrimaryAudienceId { get; set; }
        public string PrimaryAudience { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        //public DateTime RowVersion { get; set; }

        public string Service { get; set; }

        public string ServiceCategory { get; set; }
        public string ServiceComponent { get; set; }
        public int Flag { get; set; }

        public IEnumerable<SelectListItem> ServiceCategoryList { get; set; }
        public IEnumerable<SelectListItem> ServiceList { get; set; }
        public IEnumerable<SelectListItem> ServiceComponentList { get; set; }
        public IEnumerable<SelectListItem> MetricPerspectiveList { get; set; }
        public IEnumerable<SelectListItem> MetricTypeList { get; set; }
    }
}