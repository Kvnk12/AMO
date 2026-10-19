using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.A3MM
{
    [Table("A3MM_QuestRespAttachments")]
    public class QuestRespAttachments
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int QuestionRespId { get; set; }
        public string AttachFile { get; set; }
        public string FileName { get; set; }
        public string Fileextension { get; set; }
        public byte[] FileCnt { get; set; }
    }
}
