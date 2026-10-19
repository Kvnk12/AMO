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
    public class MappingTemplateViewModel : IValidatableObject
    {
        //public int AppId { get; set; }
        public int MappingId { get; set; }

        public string AppName { get; set; }
        public string AppStagingTable { get; set; }
        [Required]
        public int StoreApplicationId { get; set; }

        public string GlobalMappingIdentifier { get; set; }
        public string Description { get; set; }

        public int? ClientId { get; set; }

        public string ClientName { get; set; }

        public HttpPostedFileBase httpPostedTemplateFile { get; set; }

        //byte array for binary file
        public byte[] TemplateFile
        {
            get; set;

        }

        public string FileFormat { get; set; }

        public bool IsStandard { get; set; }

        ////[Required]
        [MaxLength(50)]
        public string TemplateName { get; set; }

        public string FileDelimiter { get; set; }

        public bool IsDeleted { get; set; }

        public int? BaseMappingId { get; set; }

        public MappingTemplateViewModel BaseMappingTemplateViewModel { get; set; }

        public bool MappingDone { get; set; }

        public int? AcknowledgeBy { get; set; }
        public string AcknowledgeByUserName { get; set; }

        public DateTime? AcknowledgeOn { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int UpdatedBy { get; set; }
        public string UpdatedByUserName { get; set; }

        public DateTime UpdatedOn { get; set; }

        // public bool ArchiveRequired { get; set; }

        public IEnumerable<SelectListItem> ClientList
        { get; set; }
        //public IEnumerable<SelectListItem> ClientList
        //{
        //    get
        //    {
        //        return GetDropDownListItems.ClientList;
        //    }
        //}
       public IEnumerable<SelectListItem> UserApplications
        {
            get;
            set;
        }
        //public IEnumerable<SelectListItem> UserApplications(int userid, int clientid)
        //{
        //    //get
        //    //{
        //    return GetDropDownListItems.UserApplications(userid, clientid);
        //    //}
        //}

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
            ////{
            ////    return GetDropDownListItems.StandardSourceTemplates;
            ////}
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (this.IsStandard && this.BaseMappingId==null)
            {
                yield return new ValidationResult(string.Format("Please select standard template.", this.BaseMappingId), new[] { "BaseMappingId" });
            }

            if (!this.IsStandard && this.httpPostedTemplateFile == null && this.StoreApplicationId>0)
            {

                yield return new ValidationResult(string.Format("Please select custom template.", this.httpPostedTemplateFile), new[] { "httpPostedTemplateFile" });

            }

            if (this.FileFormat=="TXT" && string.IsNullOrEmpty(this.FileDelimiter))
            {
                yield return new ValidationResult(string.Format("Please provide delimiter value.", this.BaseMappingId), new[] { "BaseMappingId" });

            }

        }

        
    }
}