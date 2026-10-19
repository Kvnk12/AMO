using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.FDBKManager.Models
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }
        public int Typeid { get; set; }
        public int? Productid { get; set; }
        public int? Digitalid { get; set; }
        //public int? Clientid { get; set; }
        public string ShortDescription { get; set; }
        public string DetailDescription { get; set; }
        public int ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public bool? isDeleted { get; set; }
        public IEnumerable<SelectListItem> IntClient { get; set; }
        public IEnumerable<SelectListItem> ProductList { get; set; }
        public IEnumerable<SelectListItem> offeringList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> TagsList { get; set; }
        public IEnumerable<ProductFtr> ProductFtr { get; set; }
        public string CreatorName { get; set; }
        public List<searchKeyFeedback> Searchfeedback { get; set; }
        public List<ViewFeedbackList> Viewfeedbacklist { get; set; }
        public List<CommentList> Comments { get; set; }
        public List<string> FdbkImages { get; set; }
    }
    public class searchKeyFeedback
    {
        public int FdbkId { get; set; }
        public bool Votestatus { get; set; }
        public int Typeid { get; set; }
        public int? Productid { get; set; }
        public int? DigitalId { get; set; }
        public int UniqueId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateStr { get; set; }
        public string shortDesc { get; set; }
        public string breif { get; set; }
        public string creatorName { get; set; }
        public string LastActivity { get; set; }
        public string FeedbackType { get; set; }
        public int voteCount { get; set; }
        public int Cmtcount { get; set; }
        public int product_digitalid { get; set; }
        public int Statusid { get; set; }
        public string Status { get; set; }
        public bool isMyFeekback { get; set; }
        public string FdbkTagList { get; set; }
        public string prodDigiName { get; set; }
        public int notifyType { get; set; }
    }
    public class ViewFeedbackList
    {
        public string Prod_DigitalName { get; set; }
        public string Prod_DigiId { get; set; }
        public string selMode { get; set; }
        //  public bool isMyFeekback { get; set; }
        public List<searchKeyFeedback> Searchfeedback { get; set; }

    }
    public class ProductFtr
    {
        public string ProductGroupName { get; set; }
        public int grpId { get; set; }
        public List<prdList> prdListitem { get; set; }

    }
    public class CommentList
    {
        public int fdbkId { get; set; }
        public string CommentedBy { get; set; }
        public string CommentedOn { get; set; }
        public string Comments { get; set; }

    }
    public class prdList
    {
        public int prdid { get; set; }
        public string product { get; set; }
    }
    public class FdbkTagList
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
    }
}