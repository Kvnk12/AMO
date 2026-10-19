using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
//using WebUI.Areas.AlignmentManager.Objects;

namespace NTTData.AMOPortal.WebUI.Dto
{
  public class DMItemCommentsInput /*: Input*/
  {
    public int Id { get; set; }
    public int ItemId { get; set; }
    //[StringLength(255)]
    public string Comments { get; set; }
    //public string CreatedByUser { get; set; }
    public DateTime? CreatedDate { get; set; }
    //public string LastUpdatedUser { get; set; }
    public DateTime? LastUpdatedDate { get; set; }

    public string CreateDateString
        {
        get
            {
            return (string.Format("{0:dd-MMM-yyyy hh:mm}", CreatedDate));
            }
        }

    //public bool checkNameExists(int ItemId, string Name)
    //{

    //return false;
        
    //}
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string CreatedByName { get; set; }
    }
}