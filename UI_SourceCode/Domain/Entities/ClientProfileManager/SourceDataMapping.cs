using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ClientProfileManager
{
    [Table("SourceDataMapping")]
    public class SourceDataMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public int MappingId { get; set; }

       // [Required]
        [MaxLength(100)]
        public string SField { get; set; }

       // [Required]
        [MaxLength(100)]
        public string MappingTable { get; set; }

      //  [Required]
        [MaxLength(100)]
        public string MappingCol { get; set; }

        [MaxLength(400)]
        public string MapColDesc { get; set; }

        public string DefVal { get; set; }

       // [Required]
        [MaxLength(100)]
        public string DataType { get; set; }

        public int? DLength { get; set; }

        public bool IsMandatory { get; set; }

        [MaxLength(100)]
        public string DisplayName { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool DataMasking { get; set; }
    }
}
