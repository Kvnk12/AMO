using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IdeaManager.Dto
{
    public class IMStageModel
    {
        public int Id { get; set; }
        [UIHint("Phase")]
        public int Phase { get; set; }
        [Required]
        public string Stage { get; set; }
        public int? ClientId { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}