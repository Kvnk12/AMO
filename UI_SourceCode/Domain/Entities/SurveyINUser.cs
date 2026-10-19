using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("SurveyINUser")]
    public class SurveyINUser : Entity
    {
      //  [Key]
        
      //  public int Id { get; set; }
        public int ClientId { get; set; }
        public int SurveyId { get; set; }
        public int? IncidentId { get; set; }
        public string TicketId { get; set; }
        public DateTime? INCreatedDate { get; set; }
        public DateTime? INClosedDate { get; set; }
        public DateTime? Createddt { get; set; }
        public DateTime? Completedt { get; set; }
        public string SurveyUserEmail { get; set; }
        public string SurveyUserName { get; set; }
        public DateTime? updateddt { get; set; }
        public int? UpdatedBy { get; set; }
        public int? CreatedBy { get; set; }
        public string SurveyStatus { get; set; }
        public DateTime? validtill { get; set; }
        public string UserSurveyToken { get; set; }
        public string SurveyComments { get; set; }
        public string INTitle { get; set; }
        public DateTime? SurveySentDate { get; set; }
    }
}

