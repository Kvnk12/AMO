using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUI.Areas.IdeaManager.Models
{
    public class IMIdeaSourceMasterModel
    {
        public int Id { get; set; }
        public string IdeaSource { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Isdeleted { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ClientId { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
