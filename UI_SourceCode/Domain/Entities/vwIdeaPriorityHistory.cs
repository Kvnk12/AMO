using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("vwIdeaPriorityHistory")]
    public class vwIdeaPriorityHistory : Entity
    {

    public int IdeaId { get; set; }
    public int OldPriorityId { get; set; }
    public int NewPriorityId { get; set; }
    public string Comments { get; set; }
    public string CreatedByUser { get; set; }
    public DateTime? CreatedDate { get; set; }

    public string CreateDateString
        {
        get
            {
            return (string.Format("{0:dd-MMM-yyyy hh:mm}", CreatedDate));
            }
        }


    }
}