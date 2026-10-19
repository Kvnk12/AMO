using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.A3MM
{
    [Table("A3MM_MultipleFollowUpQuestions")]
    public class A3MM_MultipleFollowUpQuestions

    {
        public int Id { get; set; }
        public int ResponseID { get; set; }
        public string FollowUpQNumber { get; set; }
        public string SubFollowUpQuestion { get; set; }
        public int ClientID { get; set; }
        public int SubFollowUpQOrder { get; set; }
        public int AssessmentId { get; set; }
        public int SubResponseId {  get; set; }
       // public bool isFCompleted {  get; set; }


    }
}