using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ClientProfileManager
{
    [Table("ClientTicketInfo")]
    public class ClientTicketInfo
    {
       
        public int Id { get; set; }
        public int ClientId { get; set; }

       
        public int Year { get; set; }
        public int Quarter { get; set; }

        public decimal IncidentNumber { get; set; }
        public decimal IncidentEffortHrs { get; set; }
        public decimal IncidentEffortPerUnit { get; set; }


        public decimal UssrNumber { get; set; }
        public decimal UssrEffortHrs { get; set; }
        public decimal UssrEffortPerUnit { get; set; }

        public decimal EnhanceNumber { get; set; }
        public decimal EnhaceEffortHrs { get; set; }
        public decimal EnhanceEffortPerUnit { get; set; }

        public decimal ProblemsNumber { get; set; }
        public decimal ProblemsEffortHrs { get; set; }
        public decimal ProblemsEffortPerUnit { get; set; }

        public decimal SRBacklog { get; set; }

        public decimal InMTTRSev1 { get; set; }
        public decimal InMTTRSev2 { get; set; }
        public decimal InMTTRSev3 { get; set; }
        public decimal InMTTRSev4 { get; set; }

        public decimal SrMTTRSev1 { get; set; }
        public decimal SrMTTRSev2 { get; set; }
        public decimal SrMTTRSev3 { get; set; }
        public decimal SrMTTRSev4 { get; set; }

        public decimal NttFTEOnsite { get; set; }
        public decimal NttFTEOffshore { get; set; }
        public decimal NttFTENearShore { get; set; }

        public int NoOfApplications { get; set; }
        public string Comment { get; set; }
    }
}
