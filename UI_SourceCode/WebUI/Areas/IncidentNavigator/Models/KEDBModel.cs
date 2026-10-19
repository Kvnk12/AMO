using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KEDBModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string KEDID { get; set; }
        public string Cause { get; set; }
        public string ResolutionSteps { get; set; }
        public string Escalations { get; set; }
        public bool SolutionApproved { get; set; }
        public string Author { get; set; }
        public string CIName { get; set; }
        public string BusinessFunction { get; set; }
        public string TicketidClusters { get; set; }
        public byte[] KEDBFile { get; set; }
        public string FileName { get; set; }
        public string IssueDescription { get; set; }
        public int IssueTypeId { get; set; }
        public int SolutionCategoryId { get; set; }
        public int SolutionStatusId { get; set; }
        public string KEDBID { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int BusinessFunctionId { get; set; }
        public string ConfigItemName { get; set; }
        public string Tower { get; set; }
        public string KedbIssueType { get; set; }
        public string SolutionCategory { get; set; }
        public string SolStatus { get; set; }
       
        public string TicketId { get; set; }

        //public string Tower { get; set; }
    }
}