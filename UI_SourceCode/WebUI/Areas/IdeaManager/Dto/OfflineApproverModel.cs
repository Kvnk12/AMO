using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
//using WebUI.Areas.AlignmentManager.Objects;


namespace NTTData.AMOPortal.WebUI.Dto
    {

    public class OfflineApproverModel /*: Input*/
    {
    public int Id { get; set; }       
    public int IdeaId { get; set; }
    public string ApproverName { get; set; }  
    public string Comments { get; set; }
    public DateTime? CreatedDate{ get; set; }
    public DateTime? ApprovedOn { get; set; }
    public DateTime? LastUpdatedDate { get; set; } 
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string IdeaNumber { get; set; }
        public string ItemName { get; set; }
        public string ImpManagers { get; set; }
        public string Status { get; set; }
        public string CreatedByName { get; set; }
        public string ProposedBy { get; set; }
        public int ClientId { get; set; }
        public int AssignedByUser { get; set; }
        public DateTime? AssignedDateTime { get; set; }
        public int Owner { get; set; }
        public int NewOwner { get; set; }
        public List<ApproverAttachment> OffApprovalAttachment { get; set; }
    }
    public class ApprovalAttachment
    {
        public int Id { get; set; }
        public string filename { get; set; }
        public string fileCont { get; set; }
    }
    public class ApproverAttachment
    {
        public int Id { get; set; }
        public string filename { get; set; }
        public string fileCont { get; set; }
    }
}