using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KEDBAssIncModel
    {
        public int IncId { get; set; }
        public string TicketId { get; set; }
        public string TicketDescription { get; set; }
        public bool isAutoIdentified { get; set; }
        public string isAutoIdentifiedVal {
            get {
                if (isAutoIdentified)
                    return "../../Content/images/marked_to_kedb.svg";
                else
                    return "../../Content/images/added_automation.svg";
                 }
        }
        public string TckTooltip {
            get
            {
                if (isAutoIdentified)
                    return "Item Added through Automation";
                else
                    return "Item Mapped to KEDB";
            }
        }

    }
}