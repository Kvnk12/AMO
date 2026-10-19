using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMaskingLib
{
    public class RegExModel
    {
        public string DataFieldName { get; set; }
        public string DataFieldDesc { get; set; }
        public string RegEx { get; set; }
        public string TextValue { get; set; }
        public string ReplacementText { get; set; }

    }
}
