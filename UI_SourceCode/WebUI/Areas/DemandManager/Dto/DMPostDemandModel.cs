using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace NTTData.AMOPortal.WebUI.Dto
{
    public class DMPostDemandModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public string ItemName { get; set; }

        public int? CreatedBy { get; set; }

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

    }


}