using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.A3MM
{
    [Table("A3MM_ActionItemTracker")]
    public class ActionItemTracker

    {
        public int Id { get; set; }
        public int AssessmentID { get; set; }
        public int Discipline_ID { get; set; }
        public int Component_ID { get; set; }
        public int Aspect_ID { get; set; }
        public string ActionItem { get; set; }
        public int? OwnerID { get; set; }

        public DateTime? PlannedCompletionDate { get; set; }

        public string Status { get; set; }
        public string Comments { get; set; }

        public int? ClientID { get; set; }

        public int? QResID { get; set; }
        public int? IdeaId { get; set; }

        public int? ImplementationPlan_ID { get; set; }

        public int? BusinessDriver_ID { get; set; }

        public int? Catalog_ID { get; set; }
        public int? Priority { get; set; }

        public string Benefits { get; set; }
        public int? BenefitId { get; set; }
    }
}
