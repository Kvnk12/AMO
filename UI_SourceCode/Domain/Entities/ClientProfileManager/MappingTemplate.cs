using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ClientProfileManager
{
    [Table("SourceTemplate")]
    public class MappingTemplate
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public int SourceTemplateId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string GlobalMappingIdentifier { get; set; }

        public int? ClientId { get; set; }

        public byte[] TemplateFile { get; set; }

        public string FileFormat { get; set; }

        public bool IsStandard { get; set; }

        public string TemplateName { get; set; }

        public string FileDelimiter { get; set; }

        public bool IsDeleted { get; set; }

        public int? BaseMappingId { get; set; }

        public bool MappingDone { get; set; }

        public int? AcknowledgeBy { get; set; }

        public DateTime? AcknowledgeOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

      //  public bool ArchiveRequired { get; set; }


    }
}
