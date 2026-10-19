using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Report
{
    [Table("ReportFieldType")]
    public class ReportFieldType
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
