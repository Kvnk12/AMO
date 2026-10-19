using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("IMEmailtemplates")]
    public class IMEmailtemplates
    {
        public int Id { get; set; }
        public string Statusnote { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
    }
}
