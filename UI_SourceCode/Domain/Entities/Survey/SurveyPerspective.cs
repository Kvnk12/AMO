using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Survey
{
    [Table("SurveyPerspective")]
    public class SurveyPerspective
    {
     [Key]
        public int Id { get; set; }
        public  int ClientId { get; set; }
        public string PerspectiveName { get; set; }
        public string PerspectiveType { get; set; }
        public int PerspectiveCategoryId { get; set; }
        public decimal? Weight { get; set; }
        public DateTime? Createddt { get; set; }
	    public DateTime? updateddt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? ToolId { get; set; }
    }
}
