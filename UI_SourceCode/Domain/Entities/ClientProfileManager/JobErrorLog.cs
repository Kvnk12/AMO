using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ClientProfileManager
{
    [Table("JobErrorLog")]
    public class JobErrorLog
    {

        [Key]
        public int Id { get; set; }

       public int? JobId { get; set; }

        public int? ClientId { get; set; }

        public int? MappingId { get; set; }

        public int?  RejectId { get; set; }

        public string ELocation { get; set; }

        public string FileName { get; set; }

        public string ErrorDesc { get; set; }

        public string FieldName { get; set; }

        public string FieldValue { get; set; }

        public DateTime? CreatedOn { get; set; }

    }
}
