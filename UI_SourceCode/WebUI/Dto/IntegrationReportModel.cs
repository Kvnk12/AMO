using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.WebUI.Dto
{
    public class IntegrationReportModel
    {
        public int Id { get; set;}
        public string TicketID { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }

        private string _Description;

        public string Description {
            get 
            {
                //return Description.Length > 1000 ? Description.Substring(0, 999) : Description;
                return _Description;
            }

            set
            {
                if (value != null && value.Length >= 1000)
                    _Description = value.Substring(0, 999);
                else
                    _Description = value;
            } 
        }


        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreatedDateTime { get; set; }
     
        public DateTime? AssignedDateTime { get; set; }
     
        public DateTime? ResolutionDateTime { get; set; }
     
        public DateTime? ClosedDateTime { get; set; }
     
        public DateTime? LastUpdatedDateTime { get; set; }
        public string AssignedToSupportGroup { get; set; }
        public string ResponseTimeMetSLA { get; set; }
        public string ResponseTarget { get; set; }
        public int? ResponseTime { get; set; }
        public bool ResponseExceptionIndicator { get; set; }
      
        public string ResponseExceptionReason { get; set; }
        public string ResolutionTimeMetSLA { get; set; }
        public string ResolutionTarget { get; set; }
        public int? ResolutionTime { get; set; }
        public bool ResolutionExceptionIndicator { get; set; }
      
        public string ResolutionExceptionReason { get; set; }
        public string Waivedindicator { get; set; }
        public bool WaiveSLAReport { get; set; }
        public string WaiveReason { get; set; }

        public string Track { get; set; }

        public string ShortDescription { get; set; }
        public string Resolutiontype { get; set; }

        public string ClusterLabel { get; set; }

        public string ClusterName { get; set; }

        public string Resolutiontime1 { get; set; }
        public string ResolutionDescription { get; set; }
        public string Category { get; set; }
        public string Lever { get; set; }
        public string Token { get; set; }

        
    }
}