using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IdeaManager.Dto
{
    public class IdeaTaskModel : IGanttTask
    {
        public int IdeaPostId { get; set; }
        public int TaskId { get; set; }
        public int OrderId { get; set; }
        [Required]
        public string Title { get; set; }
        public bool Summary { get; set; }
        public int? ParentId { get; set; }
        public int? status { get; set; }
        public int? progressstatus { get; set; }
        //public string StatusValue { get; set; }
        public string progressstatusColor { get; set; }
        public string Comments { get; set; }
        public decimal PercentComplete { get; set; }
        //public int? PriorityId { get; set; }
        //public string Priority { get; set; }
        public bool Expanded { get; set; }
        public bool? Milestone { get; set; }

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
        //  public object resources { get; set; } 
        public DateTime? EstimatedStart { get; set; }

        public DateTime? EstimatedEnd { get; set; }
        public int? Duration
        {
            get
            {
                return (int)(this.End - this.Start).TotalHours;
            }
        }
    }
}