using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebUI.Dto
    {
    public static class Extensions
        {
        public static string ToInteger(this decimal number)
            {

            return String.Format("{0:###,###,###,###,###,##0}", number);

            }
        

            public static string ToInteger(this int? number)
            {

            return String.Format("{0:###,###,###,###,###,##0}", number);

            }
        }

    }





