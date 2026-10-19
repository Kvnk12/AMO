using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.A3MM
{
    [Table("A3MM_EngagementQuestionSet")]
    public class EngagementQuestionSet
    {
        public int Id { get; set; }
        public string VersionID { get; set; }
        public int? Assessment_ID { get; set; }
        public int? Question_ID { get; set; }
        public string Status { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int? isdeleted { get; set; }



    }
}
