using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IMPostChallenge")]
    public class IMPostChallenge : DelEntity
    {

    public int ClientId { get; set; }

    public string ItemName { get; set; }

    public int? CreatedBy { get; set; }

    public decimal QtyFication { get; set; }

    public int LOBId { get; set; }

    public string Description { get; set; }
    public int ItemTypeId { get; set; }
    public int CategoryId { get; set; }
    public int PriorityId { get; set; }
    public int StatusId { get; set; }
    public string PriorityComments { get; set; }
    public string StatusComments { get; set; }


        //public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        //public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        public int? ConfigItem { get; set; }

        public string CreateDateString
        {
        get
            {
            return (string.Format("{0:dd-MMM-yyyy hh:mm}", CreatedDate));
            }
        }
        public int? UpdatedBy { get; set; }
        public string RootCause { get; set; }
    }
}