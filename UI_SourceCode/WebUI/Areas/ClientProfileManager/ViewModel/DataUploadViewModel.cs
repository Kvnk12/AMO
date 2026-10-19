using Domain.Entities.ClientProfileManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.WebUI.Dto;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class DataUploadViewModel
    {
        public string AppName { get; set; }
        public string ClientName { get; set; }
    }

   
}