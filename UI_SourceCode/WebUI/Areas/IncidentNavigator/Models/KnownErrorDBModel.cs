using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList.Mvc;
using System.Web.Mvc;


namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KnownErrorDBModel
    {
        public string Search { get; set; }
        public int Id { get; set; }
        public int ClientId { get; set; }
        [Required(ErrorMessage = "*KEDID is Required")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "*KEDID must be Numeric")]
        [Remote("IsKEDBIdExist", "KnownErrorDB", HttpMethod = "POST", ErrorMessage = "KEDB ID already exist in database.")]
        public string KEDID { get; set; }
        [Required(ErrorMessage = "*Cause is Required")]
        public string Cause { get; set; }
        [Required(ErrorMessage = "*Resolution Steps is Required")]
        [DataType(DataType.MultilineText)]
        public string ResolutionSteps { get; set; }
        [Required(ErrorMessage = "*Escalations is Required")]
        public string Escalations { get; set; }
        //[Required(ErrorMessage = "*SolutionApproved is Required")]
        //public string SolutionApproved { get; set; }

        public bool SolutionApproved { get; set; }

        [Required(ErrorMessage = "*Author is Required")]
        public string Author { get; set; }
        [Required(ErrorMessage = "*CI Name is Required")]
        public string CIName { get; set; }
        [Required(ErrorMessage = "*Business Function is Required")]
        public string BusinessFunction { get; set; }
        [Required(ErrorMessage = "*Ticket Id Clusters is Required")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "*Ticket Id Clusters must be AlphaNumeric")]
        public string TicketidClusters { get; set; }
        public HttpPostedFileBase httpPostedTemplateFile { get; set; }
        public string KEDBFilename { get; set; }
        public string Filename { get; set; }

        public byte[] KEDBFile { get; set; }

        //public IList<KnownErrorDBModel> kedblist = new List<KnownErrorDBModel>();
        public IPagedList<KnownErrorDBModel> kedblist { get; set; }
       
        //public IList<IncidentListModel> incidentlist = new List<IncidentListModel>();
        public IPagedList<IncidentListModel> incidentlist { get; set; }
        //public KnownErrorDBModel emptykedbt { get; set; }

        //public IList<IncidentPopUpList> incidentpopuplist = new List<IncidentPopUpList>();
        public IPagedList<IncidentPopUpList> incidentpopuplist { get; set; }



    }
}