using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.DemandManager.Dto
{
    public class DemandApproverModel
    {
        public int Id { get; set; }//UserProfileId
        public int DemandId { get; set; }
        public bool? isSelected { get; set; }
        public int? DemandApproverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Name { get { return FirstName + " " + LastName; } }
        public string ApprovedOrNot{ get; set; }
    }
}