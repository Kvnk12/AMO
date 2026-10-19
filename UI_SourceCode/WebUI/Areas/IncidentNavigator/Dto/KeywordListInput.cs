using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.IncidentNavigator.Models;

namespace WebUI.Areas.IncidentNavigator.Dto
{
    public class KeywordListInput
    {
       // public IList<KeywordSearchModel> wordvalueslist { get; set; }
        public int Id { get; set; }
        public string keywords { get; set; }
        public int ClientId { get; set; }
        public string WordLanguage { get; set; }
        public string WordType { get; set; }
        public string WordTotInc { get; set; }
     //   public string WordDtInc { get; set; }
        public string TotalInc { get; set; }

    }
}