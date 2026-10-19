using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("APRAppClassification")]
    public class ApplicationClassification
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ApplicationClassificationName { get; set; }
    }
}
