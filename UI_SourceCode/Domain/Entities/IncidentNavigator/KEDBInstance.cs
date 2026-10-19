using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.IncidentNavigator
{
    [Table("KEDBInstance")]
    public class KEDBInstance
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string PointOfContact { get; set; }
        public string KnowledgeArticle { get; set; }
        public string Description { get; set; }
        public int? IssueTypeId { get; set; }
        public int? SolutionCategoryId { get; set; }
        public int? SolutionStatusId { get; set; }
        public string KEDBNumber { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? BusinessFunctionId { get; set; }
        public bool IsDeleted { get; set; }
        //public string httpPostedKAFile { get; set; }
        public byte[] KAFile{ get; set;}
        public string FileName { get; set; }
        public string ResolutionSteps { get; set; }

    }
}
