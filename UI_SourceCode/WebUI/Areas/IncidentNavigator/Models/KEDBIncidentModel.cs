using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KEDBIncidentModel
    {
        public int Id { get; set; }
        public int KEDBID { get; set; }
        public int IncidentID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedBy { get; set; }
    }
}