using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("SurveyINCollection")]
    public class SurveyINCollection : Entity
    {
       //[Key]
        
       //public int Id { get; set; }
        public int SurveyModelId { get; set; }
        public int? OriginalRating { get; set; }
        public int? FinalRating { get; set; }
        public string Comments { get; set; }
        public DateTime? Createddt { get; set; }
        public int? SurveyINUserId { get; set; }
        public DateTime? updateddt { get; set; }
    }
}

