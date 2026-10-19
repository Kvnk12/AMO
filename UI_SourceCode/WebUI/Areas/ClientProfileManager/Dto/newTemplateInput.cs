using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using WebUI.Areas.ClientProfileManager.ViewModel;
namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class newTemplateInput
    {
        ////public int AppId { get; set; }
        ////public string FileFormat { get; set; }
        ////public string Delimiter { get; set; }
        ////public string Description { get; set; }
        ////public string isstandard { get; set; }
        ////public int? BaseMappingId { get; set; }
        ////public int ClientId { get; set; }
        ////public int createdBy { get; set; }
        ////public int updatedBy { get; set; }
        public MappingTemplateViewModel TemplateViewModel { get; set; }
        //public HttpPostedFileBase FileToUpload { get; set; }

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

        public IEnumerable<SelectListItem> SourceTemplateFormats
        {
            get
            {
                return GetDropDownListItems.SourceTemplateFormats; ;
            }
        }
        public IEnumerable<SelectListItem> StandardSourceTemplates
        {
            get;set;
            //{
            //    return GetDropDownListItems.StandardSourceTemplates;
            //}
        }

    }
}