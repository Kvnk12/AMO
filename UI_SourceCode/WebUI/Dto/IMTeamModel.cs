using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Dto
{
    public class IMTeamModel
    {
        public int Id { get; set; }
        public string Team { get; set; }
        public string IdeaSourceVal { get; set; }
        public int? ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}