using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("mdmdatauploadtable")]
    public  class mdmDataUploadTable : DelEntity
    {
      public string DataTablename { get; set; }
      public int ClientId { get; set; }
      public DateTime DataTableCreatedatetime { get; set; }

      public DateTime DataTableLastUploaddatetime { get; set; }

    }
}
