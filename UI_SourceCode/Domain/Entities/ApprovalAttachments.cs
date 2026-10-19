using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("IM_ApprovalAttachments")]
    public class ApprovalAttachments
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int OffapprovalId { get; set; }
        public string AttachFile { get; set; }
        public string FileName { get; set; }
        public string Fileextension { get; set; }
        public byte[] FileCnt { get; set; }
    }
}
