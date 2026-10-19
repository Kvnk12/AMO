using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Report
{
    [Table("ReportViewField")]
    public class ReportViewField
    {
        public int Id {get;set;}
        public int ReportViewId {get;set;}
        public string FieldName  {get;set;}
        public string FieldCaption { get; set; }
        public int FieldType{get;set;}
        public int Sequence {get;set;} 	
        public int? SortOrder { get; set; }
        public int? Aggregatetype { get; set; }
    }
}
