using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.A3MM
{
   
    [Table("A3SurveyType")]
    public class A3SurveyType
    {
        public int ID { get; set; }
        public int ClientId { get; set; }
        public string SurveyName { get; set; }
    }
}
