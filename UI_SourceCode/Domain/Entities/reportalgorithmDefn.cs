using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
    {

    [Table("reportalgorithmdefn")]
    public class ReportalgorithmDefn : DelEntity
        {

        public string AlgorithmName { get; set; }
        public string AlgorithmSolution { get; set; }
        public string DisplayName { get; set; }
        public int    Order { get; set; }
        public string Type { get; set; }
        public string KeyName { get; set; }
        public string PredefinedValues { get; set; }
        public string FilteredColumn { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get; set; }


        }
    }