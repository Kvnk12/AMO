using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FDBKManager
{
    [Table("IMFDBKComment")]
    public class IMFDBKComment
    {
        public int Id { get; set; }
        public int ImfdbkId { get; set; }
        public string Comments { get; set; }
        public int CommentedBy { get; set; }
        public string CommentedByName { get; set; }
        public DateTime? CommentedOn { get; set; }
        public int ClientId { get; set; }
    }
}