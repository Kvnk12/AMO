using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace WebUI.Areas.IncidentNavigator.Models
{
    public class INColumnValueModel
    {
        public int Id { get; set;}
        public string ColumnName { get; set;}
        public string Value { get; set;}
        public int Created_By { get; set; }
        public DateTime? Created_Date { get; set; }

        public int Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public int ClientId { get; set; }
    }
}
