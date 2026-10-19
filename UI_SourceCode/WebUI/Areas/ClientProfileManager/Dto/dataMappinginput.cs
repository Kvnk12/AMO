using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.ClientProfileManager.ViewModel;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class dataMappinginput
    {
        public MappingTemplateViewModel template { get; set; }
        public IList<SourceDataMappingViewModel>  templateMapping { get; set; }
        //()  //(int MappingID)
        public IList<SelectListItem> SourceFields { get; set; }
        public IList<SelectListItem> OperatorFields
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "+",
                        Value = "+"
                    },
                    new SelectListItem
                    {
                        Text = "-",
                        Value = "-"
                    },
                    new SelectListItem
                    {
                        Text = "*",
                        Value = "*"
                    },
                    new SelectListItem
                    {
                        Text = "/",
                        Value = "/"
                    }
                };
            }
        }





    }
}