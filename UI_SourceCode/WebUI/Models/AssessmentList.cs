using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;

namespace WebUI.Models
{
    public class AssessmentList
    {
        public string AccountCreate { get; set; }
        public int AssessmentId { get; set; }
        public int AccountId { get; set; }
        [Required(ErrorMessage = "Enter Assessment Name")]
        [StringLength(200)]
        public string AssessmentName { get; set; }
        public string AccountName { get; set; }
        [Required(ErrorMessage = "Enter Discipline Name")]
        [StringLength(200)]
        public string DisciplineName { get; set; }
        [Required(ErrorMessage = "Enter Component Name")]
        [StringLength(200)]
        public string ComponentName { get; set; }
        [Required(ErrorMessage = "Enter Component Definition")]
        [StringLength(200)]
        public string ComponentDefinition { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage = "Select Assessment Status")]
        public string AssessmentStatus { get; set; }
        public string AccountStatus { get; set; }
        [Required(ErrorMessage = "Select  Open Date")]
        [DataType(DataType.Date)]
        public DateTime OpenDate { get; set; }
        [Required(ErrorMessage = "Select  Close Date")]
        [DataType(DataType.Date)]
        public DateTime CloseDate { get; set; }
        public IEnumerable<SelectListItem> AccountStatusList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ClientStatus> AccountStatus = new Repo<ClientStatus>(Context);
                List<NTTData.AMOPortal.Domain.Entities.ClientStatus> AccountStatusList = AccountStatus.GetAll().ToList();
                var Items = AccountStatusList.Select(CList => new SelectListItem
                {
                    Value = CList.Status.ToString(),
                    Text = CList.Status.ToString()
                });
                return Items;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.CloseDate <= this.OpenDate)
            {
                yield return new ValidationResult(string.Format("Close Date Should be greater than {0:MM/dd/yyyy}", this.OpenDate));
            }
            if (this.AccountStatus == "Active")
            {
                if (!(DateTime.Now >= this.OpenDate && this.CloseDate >= DateTime.Now))
                {
                    yield return new ValidationResult(string.Format("Account Status can not be set {0} since Current Date does not fall between  Open & Close Dates", this.AccountStatus));
                }
            }
            if (this.AccountStatus == "Inactive")
            {
                if ((DateTime.Now >= this.OpenDate && this.CloseDate >= DateTime.Now))
                {
                    yield return new ValidationResult(string.Format("Account Status can not be set {0} since Current Date  falls between  Open & Close Dates", this.AccountStatus));
                }
            }
        }
    }
}