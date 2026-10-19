using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Report
{
    [Table("ReportView")]
    public class ReportView
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReportId { get; set; }
        public int ClientId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsPublic { get; set; }
        public string ViewName { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DateType { get; set; }
    }
}
