using Data;
using Domain.Entities.Report;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using WebUI.App_Start;
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.IncidentNavigator.Dto;
using WebUI.Dto;

namespace WebUI.Areas.IncidentNavigator.AspxReport
{
    public partial class OLAPAnalysis : System.Web.UI.Page
    {
        HashSet<string> columnNames = new HashSet<string>();
        Dictionary<string, LineSeries> LineSeriesByRowName = new Dictionary<string, LineSeries>();
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {

            }
           // RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.Equals,
           //                                     RadPivotGrid1.Fields["Country"], "Canada");
            // RadPivotGrid1.FilterByValue(PivotGridFilterFunction.Contains, RadPivotGrid1.Fields["Country"],RadPivotGrid1.Fields["SalesAmount"],)
        }



        public string cubeName
        {
            get
            {
               // Client client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];

                if (1 == 1)
                    return "Adventure Works";
                else
                    return "Adventure Works1";
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ////  RadPivotGrid1.
                ////RadPivotGrid1.OlapSettings.
                RadPivotGrid1.ClearFilter(RadPivotGrid1.Fields["Country"]);
                var cleanList = new List<string>();
                //cleanList.Add("United States");
                cleanList.Add("Canada");
                RadPivotGrid1.SetFilterIncludes("Country", cleanList);
                RadPivotGrid1.Fields["Country"].IsHidden = true;
                // RadPivotGrid1.Rebind();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            RadPivotGrid1.OlapSettings.ProviderType = PivotGridOlapProviderType.Xmla;

            RadPivotGrid1.OlapSettings.AdomdConnectionSettings.Cube = "Adventure Works";
            RadPivotGrid1.OlapSettings.AdomdConnectionSettings.DataBase = "Adventure Works DW 2008R2";
            RadPivotGrid1.OlapSettings.AdomdConnectionSettings.ConnectionString =
                "Data Source=https://demos.telerik.com/olap/msmdpump.dll;Catalog=Adventure Works DW 2008R2";

            RadPivotGrid1.OlapSettings.XmlaConnectionSettings.ServerAddress = "https://demos.telerik.com/olap/msmdpump.dll";
            RadPivotGrid1.OlapSettings.XmlaConnectionSettings.Cube = "Adventure Works";
            RadPivotGrid1.OlapSettings.XmlaConnectionSettings.DataBase = "Adventure Works DW 2008R2";
        }

        ////protected override void OnPreInit(EventArgs e)
        ////{
        ////    base.OnPreInit(e);
        ////    RadPivotGrid1.OlapSettings.ProviderType = Telerik.Web.UI.PivotGridOlapProviderType.Xmla;
        ////    RadPivotGrid1.OlapSettings.XmlaConnectionSettings.Cube = "Adventure Works";
        ////    RadPivotGrid1.OlapSettings.XmlaConnectionSettings.DataBase = "Adventure Works DW 2008R2";
        ////    RadPivotGrid1.OlapSettings.XmlaConnectionSettings.ServerAddress = "Data Source=https://demos.telerik.com/olap/msmdpump.dll";
        ////    //RadPivotGrid1.OlapSettings.AdomdConnectionSettings.Cube = "Adventure Works";
        ////    //RadPivotGrid1.OlapSettings.AdomdConnectionSettings.DataBase = "Adventure Works DW 2008R2";
        ////    //RadPivotGrid1.OlapSettings.AdomdConnectionSettings.ConnectionString = "Data Source=https://demos.telerik.com/olap/msmdpump.dll;Catalog=Adventure Works DW 2008R2";
        ////}

        protected void RadPivotGrid1_Init(object sender, EventArgs e)
        {
            ////RadPivotGrid1.OlapSettings.ProviderType = Telerik.Web.UI.PivotGridOlapProviderType.Xmla;
            ////RadPivotGrid1.OlapSettings.XmlaConnectionSettings.Cube = "Adventure Works";
            ////RadPivotGrid1.OlapSettings.XmlaConnectionSettings.DataBase = "Adventure Works DW 2008R2";
            ////RadPivotGrid1.OlapSettings.XmlaConnectionSettings.ServerAddress = "Data Source=https://demos.telerik.com/olap/msmdpump.dll";
            //////RadPivotGrid1.OlapSettings.AdomdConnectionSettings.Cube = "Adventure Works";
            //RadPivotGrid1.OlapSettings.AdomdConnectionSettings.DataBase = "Adventure Works DW 2008R2";
            //RadPivotGrid1.OlapSettings.AdomdConnectionSettings.ConnectionString = "Data Source=https://demos.telerik.com/olap/msmdpump.dll;Catalog=Adventure Works DW 2008R2";

        }
    }
}