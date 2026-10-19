using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FDBKManager
{
    [Table("IMFDBKAttachment")]
    public class IMFDBKAttachment
    {
        public int Id { get; set; }
        public int ImfdbkId { get; set; }
        public string Fdbkimg { get; set; }
    }
}