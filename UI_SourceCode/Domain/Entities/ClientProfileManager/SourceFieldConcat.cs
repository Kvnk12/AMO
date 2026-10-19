using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ClientProfileManager
{
    [Table("SourceFieldConcat")]
    public class SourceFieldConcat
    {
        [Key]
        public int Id { get; set; }

        public int SourceDataMappingId { get; set; }

        public int MappingId { get; set; }

        public string SourceField { get; set; }

        public string SeparatedBy { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
