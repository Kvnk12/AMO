using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.ComponentModel;

namespace WebUI.Areas.DemandManager.Dto
{
    public class DMTaskAndActivitiesModel 
    {
        public int Id { get; set; }
        public int DemandId { get; set; }
        [Required(ErrorMessage = "Please select Task")]
        public string TaskId { get; set; }
        public string Task { get; set; }
        [Display(Name = "Start Date:")]
        [Required(ErrorMessage = "Please select StartDate")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date:")]
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        //[RegularExpression(@"^\w+([-+.']\w+)*@nttdata.com$", ErrorMessage = "Invalid Email.")]
        //[StringLength(50, ErrorMessage = "Less than 50 characters")]
        [Required(ErrorMessage ="Please enter user EmailId")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string AssignedTo { get; set; }
        [Required(ErrorMessage = "Please select Stage")]
        public string StageId { get; set; }
        public string Stage { get; set; }
        public string Comments { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserTaskToken { get; set; }
    }
}