using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class LogModel
    {
        public string UserName { get; set; }
        public string Auth { get; set; }
        public bool IsVerified { get; set; }
        public DateTime Date { get; set; }

    }
}