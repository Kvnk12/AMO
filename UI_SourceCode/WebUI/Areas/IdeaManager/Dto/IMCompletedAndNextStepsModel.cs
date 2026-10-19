using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WebUI.Areas.IdeaManager.Dto
{
    public class IMCompletedAndNextStepsModel
    {
        public int Id { get; set; }
        public int IdeaId { get; set; }
        //[StringLength(255)]
        //[Required]
        public string CompletedSteps { get; set; }
        //[StringLength(255)]
        public string NextSteps { get; set; }
        //[StringLength(255)]
        public string Comments { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }
       
        public string ImpManagers { get; set; }

        public List<string> ImpName
        {
            get
            {
                if (Convert.ToInt32(ImpManagers) > 0)
                    return IMHelper.GetImpName(Convert.ToInt32(ImpManagers));
                else
                    return null;
            }
        }
    }
}