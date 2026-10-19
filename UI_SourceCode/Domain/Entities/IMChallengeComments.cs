using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IMChallengeComments")]
    public class IMChallengeComments : DelEntity
    {

    public int ItemId { get; set; }
    public string Comments { get; set; }
    public string CreatedByUser { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string LastUpdatedUser { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public string CreateDateString {
            get
            {
            return( string.Format("{0:dd-MMM-yyyy hh:mm}", CreatedDate));
        }
        }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}