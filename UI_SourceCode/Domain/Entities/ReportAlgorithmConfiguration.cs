using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
    {

    [Table("ReportAlgorithmConfiguration")]
    public class ReportAlgorithmConfiguration : DelEntity
        {

        public string AlgorithmName { get; set; }
        public string AlgorithmSolution { get; set; }
        public int NoOfColumnsToSelect { get; set; }
        public int NoOfColumnsForPartitionBy { get; set; }
        public int NoOfColumnsForPartitionDate { get; set; }

        }
    }