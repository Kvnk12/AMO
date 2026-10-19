using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Domain.Entities
{
	[Table("ServiceImprovementPlan_Q")]
	public class ServiceImprovementPlan_Q
    {
		public int Id { get; set; }
		public int? ClientId { get; set; }
		public int? ServiceImprovementGoalId { get; set; }
		public int? Year { get; set; }
		public string Quarter { get; set; }
		public decimal GoalAchievement { get; set; }
		public string Comment { get; set; }
		public int? CreatedBy { get; set; }
		public int? PlanModifiedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? PlanModifiedDate { get; set; }

		public decimal Goal { get; set; }

		public int? AchivementModifiedBy { get; set; }

		public DateTime? AchievementModifiedDate { get; set; }
	}
}







