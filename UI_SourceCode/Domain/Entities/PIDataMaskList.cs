using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("PIDataMaskList")]
    public class PIDataMaskList
    {
        [Key]
        public int Id { get; set; }
        public string DataFieldName { get; set; }
        public string DataMaskPattern { get; set; }
        public string DataFieldDescription { get; set; }
        public int? ClientId { get; set; }
        public string DataMaskExample { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ReplacementValue { get; set; }


    }
}
