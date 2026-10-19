using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IdeaManager.Dto
{
    public class ApproverModel
    {
        public int Id { get; set; }//UserProfileId
        public int IdeaId { get; set; }
        public bool? isSelected { get; set; }
        public int? IdeaApproverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Name { get { return FirstName + " " + LastName; } }
        public string ApprovedOrNot{ get; set; }
    }
}