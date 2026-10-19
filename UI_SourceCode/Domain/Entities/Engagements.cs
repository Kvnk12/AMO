using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("Engagements")]
    public class Engagements : DelEntity
    {
        
        public DateTime EngagementStartDate { get; set; }
        public DateTime EngagementEndDate { get; set; }
        //public int DurationOfRelationship { get; set; }
        public decimal DurationOfRelationship { get; set; } //Modified as per the requirement to show DurationOfRelationship in decimal
    }
}
