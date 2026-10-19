using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
    {

    [Table("ReportThreshold")]
    public class ReportThreshold : DelEntity
        {

        public int ReportConfigId { get; set; }
        public int AlgorithmId { get; set; }
        public string MinimumThreshold { get; set; }
        public string MaximumThreshold { get; set; }
        public string DiscreetThreshold { get; set; }
        public string ThresholdColor { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        }
    }