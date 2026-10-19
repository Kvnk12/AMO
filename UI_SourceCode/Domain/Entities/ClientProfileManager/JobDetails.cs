using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ClientProfileManager
{
    [Table("JobDetail")]
    public class JobDetail
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public int SourceTemplateId { get; set; }

        [Key]
        public int Id { get; set; }

       public int? MappingId { get; set; }

        public int? ClientId { get; set; }

        public int? ApplicationId { get; set; }

        public string UserId { get; set; }

        public DateTime JobStartTime { get; set; }
        
        public DateTime? JobEndTime { get; set; } 

        public string FileName { get; set; }

        public string RunStatus { get; set; }

        public int TotalRecordRecieved { get; set; }

        public int AddRowCount { get; set; }

        public int UpdateRowCount { get; set; }

        public int DuplicateRowCount { get; set; }

        public int RejectedRecordsCount { get; set; }

        public int IgnoredRecords { get; set; }
        public bool? IsChecked { get; set; }
    }
}
