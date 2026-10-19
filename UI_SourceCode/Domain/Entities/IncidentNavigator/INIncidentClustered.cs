using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.IncidentNavigator
{
    [Table("INIncidentClustered")]
    public class INIncidentClustered
    {
        [Key]
        public int INCIDENT_CLUSTERED_ID { get; set; }

        public int CLUSTERID { get; set; }

        public int INCIDENT_ID { get; set; }
      
        public int Clientid { get; set; }

        public string CLUSTER_LABEL { get; set; }

        public string CLUSTER_NAME { get; set; }

    }
}
