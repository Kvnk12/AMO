using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WebUI.Areas.DemandManager.Dto
{
    public class DMCompletedAndNextStepsModel
    {
        public int Id { get; set; }
        public int DemandId { get; set; }
        //[StringLength(255)]
        //[Required]
        public string CompletedSteps { get; set; }
        //[StringLength(255)]
        public string NextSteps { get; set; }
        //[StringLength(255)]
        public string Comments { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }
       
        public int? ImpManagers { get; set; }

        public List<string> ImpName
        {
            get
            {
                if (ImpManagers > 0)
                    return DMHelper.GetImpName(ImpManagers.Value);
                else
                    return null;
            }
        }
    }
}