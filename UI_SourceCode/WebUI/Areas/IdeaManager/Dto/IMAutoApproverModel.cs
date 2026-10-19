using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.IdeaManager.Dto
{
    public class IMAutoApproverModel 
    {
        public int Id { get; set; }
        public int ApproverId { get; set; }
        public int? IdeaApproverId { get; set; }
        public string ApproverName { get; set; }
        public int? ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
