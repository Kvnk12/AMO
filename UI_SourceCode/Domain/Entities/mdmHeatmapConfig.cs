using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("mdmHeatmapConfig")]
    public class mdmHeatmapConfig : DelEntity
    {
        
     

        public int HeatmapConfigId { get; set; }
        public int? ClientId { get; set; }
        public int? AlgorithmId { get; set; }
        public string HeatmapReportName { get; set; }
        public int? HeatVariableID { get; set; }
        public int? HeatVariableRelationshipId { get; set; }
    	public int? HeatUseMetadataId { get; set; }
        public int? HeatUseReportId { get; set; }
        public int? CreatedByUser { get; set; }
        public int? LastUpdatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
	

	
 
    }
}