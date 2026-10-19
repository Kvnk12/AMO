using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
    {


    [Table("vwRegDataSet")]
    public class vwRegDataSet   : Entity
        {


        //public virtual Int64 Id { get; set; }
           public int  ClientId {get;set;}
           public string DatabaseObjectName {get;set;}
           public string DataSetName { get; set; }
           public int RegisterDatasetId {get;set;}
           public string VariableType {get;set;}
           public int datasetcolmapId { get; set; }
           public string DatasetColumnName { get; set; }


        }
    }
