using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("ClientEngagement")]
    public class ClientEngagement   
	{
		[Key]
		public int Id { get; set; }
		public int? EngagementPhaseId { get; set; }
		public int? ClientId { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }

		public DateTime? CreatedDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
		
	}
    
}

