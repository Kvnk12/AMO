using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace NTTData.AMOPortal.Domain.Entities
{
    public class Client : DelEntity
    {

        public string ClientName { get; set; }
        public string BusinessDesc { get; set; }
        public int VerticalId { get; set; }
        public int SubVerticalId { get; set; }
        public int Currency { get; set; }
        public string TotalContractValue { get; set; }
        public string TotalContractValueToDate { get; set; }
        public string AvgAnnualRevenue { get; set; }
        public int PrimaryAddressId { get; set; }
        [ForeignKey("PrimaryAddressId")]
        public Address PrimaryAddress { get; set; }
        //public int EngagementId { get; set; }
        //[ForeignKey("EngagementId")]
        //public Engagements Engagement { get; set; }
        public string GlobalClientManager { get; set; }
        public string GlobalDeliveryManager { get; set; }
        public string ClientTitle { get; set; }
        public int ClientStatus { get; set; }
        public int IsReferenceable { get; set; }
        public int HasReferred { get; set; }
        public string ReferredClientName { get; set; }
        public DateTime CustomerSurveyDate { get; set; }
        public int CustomerRating { get; set; }

        public string AvgAnnualFTEs { get; set; }
        public string AvgAnnualFTEsOnShore { get; set; }
        public string AvgAnnualFTEsNearShore { get; set; }
        public string AvgAnnualFTEsOffShore { get; set; }

        public string Notes { get; set; }
        private DateTime _DateCreated = DateTime.Now;
        public DateTime DateCreated { get { return _DateCreated; } set { _DateCreated = value; } }
        public DateTime _DateUpdated = DateTime.Now;
        public DateTime DateUpdated { get { return _DateCreated; } set { _DateCreated = value; } }
        public bool AutoCalculatedFTE { get; set; } // added by Neeraj
       // public bool IsDeleted { get; set; } // added by Neeraj
        public bool? SupportEcoSystem { get; set; }
        public int? EnableDispositionAutomation { get; set; }
        public string SurveyTypeName { get; set; }
        public int? IncludeAlternativeDashboard { get; set; }
       // public string CPMFilename { get; set; }
        public string Filename { get; set; }
      //  public HttpPostedFileBase httpPostedTemplateFile { get; set; }
        public byte[] CPMFile { get; set; }
        public bool EnableMFA { get; set; }

        public bool MultipleApprovals { get; set; }
        public bool BusinessCalender { get; set; }

        public string EmailTemplateImageName { get; set; }  
        public byte[] EmailTemplateImageCPMFile { get; set; }
        public bool IMAutoApprovalSelection { get; set; }
        



    }
}
