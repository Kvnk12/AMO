using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Dto
{
    public class BCApplicationJunctionModel
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public int BCId { get; set; }
        public string BCName { get; set; }
        public string DoSId { get; set; }
       // public string PreDoSId { get; set; }
        public string PostDoSId { get; set; }
        public string PostDosName { get; set; }

        public string DOSName { get; set; }
        public string Hierarchy { get; set; }

    }
}