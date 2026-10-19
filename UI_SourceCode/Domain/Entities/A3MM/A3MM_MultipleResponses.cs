using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.A3MM
{
    [Table("A3MM_MultipleResponses")]
    public class A3MM_MultipleResponses

    {
        public int Id { get; set; }
        public int ResponseID { get; set; }
        public string ResponseNumber { get; set; }
        public string SubResponse { get; set; }
        public int ClientID { get; set; }
        public int AssessmentId { get; set; }
        public int SubResponseOrder { get; set; }
        public string SQNo {  get; set; }
        //public bool isRCompleted{ get; set; }

    }
}