using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.AlignmentManager.Objects;

namespace NTTData.AMOPortal.WebUI.Dto
    {
    public class DataSetColumnsForHeatMapReport : Input
        {

        public string ColumnName { get; set; }
        
        public bool isSelected { get; set; }


        //public IEnumerable<SelectListItem> GetAllVarTypes
        //    {
        //    get
        //        {
        //        return AlignmentHelper.GetVariableTypes();
        //        }
        //    }

        }
    }