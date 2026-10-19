using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.FDBKManager.Models
{
    public class FeedbackTypeModel
    {
        public int Id { get; set; }
        public string FeedbackType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
    public enum IMFeedbackStatus
    {
        Draft = 1,
        //SubmitForApproval = 2,
        Approved = 3,
        Rejected = 4,
        Pending = 5,
        PartiallyApproved = 6
    }
    public enum IMFeedbackRole
    {
        ProductManager = 1,
        OfferingLead = 2
    }
}