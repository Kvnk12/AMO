using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class ExportViewModel
    {
        [AllowHtml]
        public string Csv { get; set; }
        public DataTable RejectedRecords { get; set; }
    }

}
