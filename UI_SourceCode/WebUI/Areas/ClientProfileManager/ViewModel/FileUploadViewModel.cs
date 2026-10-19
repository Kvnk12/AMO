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
    public class FileUploadViewModel 
    {
        //public int AppId { get; set; }
        public int MappingId { get; set; }
        public string AppName { get; set; }
        
        [Required]
        public int StoreApplicationId { get; set; }

       // public string GlobalMappingIdentifier { get; set; }
        public string Description { get; set; }

        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public HttpPostedFileBase httpPostedFileData { get; set; }

        public string DataFileName { get; set; }

        //byte array for binary file
        public byte[] FileData
        {
            get; set;
        }
        public bool ScheduleJob { get; set; }
        ////public int CreatedBy { get; set; }
        ////public string CreatedByUserName { get; set; }

        ////public DateTime CreatedOn { get; set; }

        ////public int UpdatedBy { get; set; }
        ////public string UpdatedByUserName { get; set; }

        ////public DateTime UpdatedOn { get; set; }


        public IEnumerable<SelectListItem> ClientList
        {
            get
            {
                return GetDropDownListItems.ClientList;
            }
        }

        public IEnumerable<SelectListItem> UserApplications(int userid, int clientid)
        {
            //get
            //{
            return GetDropDownListItems.UserApplications(userid, clientid);
            //}
        }

        ////public IEnumerable<SelectListItem> SourceTemplateFormats
        ////{
        ////    get
        ////    {
        ////        return GetDropDownListItems.SourceTemplateFormats; ;
        ////    }
        ////}
        ////public IEnumerable<SelectListItem> StandardSourceTemplates
        ////{
        ////    get
        ////    {
        ////        return GetDropDownListItems.StandardSourceTemplates;
        ////    }
        ////}

    }
}