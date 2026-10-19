using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using NTTData.AMOPortal.Resources;

namespace NTTData.AMOPortal.WebUI.Dto
{
    public class StrLenAttribute : StringLengthAttribute
    {
        public StrLenAttribute(int maximumLength)
            : base(maximumLength)
        {
            ErrorMessageResourceName = "strlen";
            //ErrorMessageResourceType = typeof(Mui);
        }
    }
}