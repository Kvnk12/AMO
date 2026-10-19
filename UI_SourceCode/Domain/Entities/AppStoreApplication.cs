using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/////////////////////////////////////////////////////////////////////////////////////////////////////
// Modified By: Neeraj
// Modified Date: 20-03-2014
// Comment: Added for Access Management
/////////////////////////////////////////////////////////////////////////////////////////////////////

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("AppStoreApplication")]
    public class AppStoreApplications : DelEntity
    {
        public string ApplicationName { get; set; }
        public string StageTable { get; set; }
        public string DataUploadJob { get; set; }
        public string DataUploadSharedFolder { get; set; }
        public string SSISPackageFolder { get; set; }
    }
}
