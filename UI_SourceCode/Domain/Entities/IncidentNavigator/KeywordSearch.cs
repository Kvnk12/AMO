using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.IncidentNavigator
{
    [Table("WordList")]
   public class KeywordSearch
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public string WordType { get; set; }
        public string WordLanguage { get; set; }
        public string WordValue { get; set; }
        public string WordDescription { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool isdeleted { get; set; }
      //  public bool Selected { get; set; }
    }
}
