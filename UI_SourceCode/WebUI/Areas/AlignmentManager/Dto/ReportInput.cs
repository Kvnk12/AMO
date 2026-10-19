using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;
using System.Data;
using System.Data.SqlClient;
using NTTData.AMOPortal.WebUI.Dto;
namespace WebUI.Areas.AlignmentManager.Dto
    {

    public class ReportInput : Input
    {

    public int ClientId { get; set; }
    public string ReportName { get; set; }

    

    public string PageHeading { get; set; }
    public string ReportType { get { return "PAREPORT"; } }

    public IEnumerable<SelectListItem> ClientList
        {
        get
            {
            return GetDropDownListItems.ClientList;
            }
        }
        
    }

    }

