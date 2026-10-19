using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("mdmhstDataFileupload")]
    public  class mdmhstDataFileupload : DelEntity
    {
      public int Datatableid { get; set; }
      public int ClientId { get; set; }

        public string datafilename { get; set; }

        public string datatablename { get; set; }

        public DateTime dateuploaded { get; set; }
      

      public string sheetname { get; set; }

     public string hdrstatus { get; set; }

        public bool chktruncate { get; set; }

    }
}
