using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace NTTData.AMOPortal.WebUI.Dto
{
    public class IMPostIdeaModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public string ItemName { get; set; }

        public int? CreatedBy { get; set; }

        //public decimal QtyFication { get; set; }

        //public int IdeaTypeId { get; set; }
        //public int IdeaSourceId { get; set; }

        //public int LOBId { get; set; }

        //public string Description { get; set; }

        //public int ItemTypeId { get; set; }
        //public int CategoryId { get; set; }
        //public int PriorityId { get; set; }
        public int StatusId { get; set; }
      //  public string PriorityComments { get; set; }
        public string StatusComments { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

       // public int? ConfigItem { get; set; }

        public string CreateDateString
        {
            get
            {
                return (string.Format("{0:dd-MMM-yyyy hh:mm}", CreatedDate));
            }
        }
        public int? UpdatedBy { get; set; }

        //public string Complexity { get; set; }
        //public string Risk { get; set; }
        //public string RiskMitigation { get; set; }
        //public bool InternalInitiative { get; set; }

        //public DateTime? PlannedStartDate { get; set; }
        //public DateTime? PlannedEndDate { get; set; }
        //public DateTime? ActualStartDate { get; set; }
        //public DateTime? ActualEndDate { get; set; }
        //public int? ProgressStatus { get; set; }
    }


}