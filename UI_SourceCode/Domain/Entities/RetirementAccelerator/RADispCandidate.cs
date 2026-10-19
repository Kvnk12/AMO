using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.RetirementAccelerator
{   [Table("RADispCandidate")]
    public class RADispCandidate
    {
        public int Id { get; set; }
        public int RAApplicationId { get; set; }
        public int? ReplacementScopeId { get; set; }
        public bool? ArchiveData { get; set; }
        public bool? IsDeCommissioned { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}
