using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Report
{
    [Table("ReportViewFilter")]
    public class ReportViewFilter
    {
        public int Id { get; set; }
        public int ViewId { get; set; }
        public string FieldName { get; set; }
        public int FilterType { get; set; }
        public int FilterCondition { get; set; }
        public int FilterConditionValue { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
    }
}
