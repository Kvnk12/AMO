using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FDBKManager
{
    [Table("IMFDBKPost")]
    public class IMFDBKPost
    {
        public int Id { get; set; }
        public int Typeid { get; set; }
        public int? Productid { get; set; }
        public int? Digitalid { get; set; }
        public string ShortDescription { get; set; }
        public string DetailDescription { get; set; }
        public int ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public bool? isDeleted { get; set; }
        public string IMFDBKNumber { get; set; }
        public string OriginatedFrom { get; set; }
        public int? FdbkSentiment { get; set; }

    }
}
