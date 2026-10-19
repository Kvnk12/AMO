using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebUI.Areas.IdeaManager.Dto
{
    public class IMIdeaReassignModel
    {
        public int Id { get; set; }
        public int IdeaId { get; set; }
        public string IdeaNumber { get; set; }
        public string ItemName { get; set; }
        public string ImpManagers { get; set; }
        public string Status { get; set; }
        public string CreatedByName { get; set; }
        public string ProposedBy { get; set; }
        public int CreatedBy { get; set; }
        public int ClientId { get; set; }
        public int AssignedByUser { get; set; }
        public string AssignedToUserName { get; set; }
        public DateTime? AssignedDateTime { get; set; }
        public int Owner { get; set; }
        public int NewOwner { get; set; }
    }

  

}