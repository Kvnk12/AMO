using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class RATaskModel:IGanttTask
    {
        public int ApplicationId { get; set; }
        public int TaskId { get; set; }
        public int OrderId { get; set; }
        [Required]
        public string Title { get; set; }
        public bool Summary { get; set; }
        public int? ParentId { get; set; }
        public int? ProgressStatus { get; set; }
        public decimal PercentComplete { get; set; }
        public bool Expanded { get; set; }
        public bool? Milestone { get; set; }
        public string Status { get; set; }
        private DateTime start;
        public DateTime Start
        {
            get { return start; }
            set { start = value.ToUniversalTime(); }
        }

        private DateTime end;
        public DateTime End
        {
            get { return end; }
            set { end = value.ToUniversalTime(); }
        }
        public DateTime? EstimatedStart { get; set; }

        public DateTime? EstimatedEnd { get; set; }
        public int? Duration
        {
            get
            {
                return (int)(this.End - this.Start).TotalHours;
            }
        }

        public int? ResourceId { get; set; }
        public string Resource { get; set; }
        public decimal? EstimatedEfforts { get; set; }
        public decimal? ActualEfforts { get; set; }
        public int? WorkProdId { get; set; }
        [Url]
        public string WorkProduct { get; set; }
        public int? RoleId { get; set; }
        public int? ResourceGroupId { get; set; }


    }
}