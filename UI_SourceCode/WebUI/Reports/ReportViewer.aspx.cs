using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Net;
using System.Collections;
using System.Security.Principal;

namespace WebUI.Reports
{
    public partial class ReportViewer : AspReportpagesBaseClass
    {
        private string ReportName
        {
            get
            {
                return Convert.ToString(Request.QueryString["ReportName"]);
            }
        }
        private string ReportID
        {
            get
            {
                return Convert.ToString(Request.QueryString["ID"]);
            }
        }

        private string ReportType

        {

        get
            {
            return Convert.ToString(Request.QueryString["ReportType"]);
            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    ReportViewerControl.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                    ReportViewerControl.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServer"]);
                    ReportViewerControl.ShowCredentialPrompts = false;



                    string ReportsPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString();
                    ReportViewerControl.ServerReport.ReportPath = ReportsPath + ReportName;
                    ReportViewerControl.ShowParameterPrompts = false;
                    ReportViewerControl.ShowExportControls = true;
                    ReportViewerControl.ShowZoomControl = false;
                    CustomReportCredentials cred = new CustomReportCredentials();
                    ReportViewerControl.ServerReport.ReportServerCredentials = new CustomReportCredentials();

                    if (ReportName.Contains("rptIncidentAssignedGroupByPriority")    ||
                        ReportName.Contains("rptTicketAgeingAnalysis") || ReportName.Contains("rptInterface")
                         || ReportName.Contains("SLADashboard")
                        )
                    {
                        ReportViewerControl.ShowParameterPrompts = true;
                      
                    }

                    if (ReportName.Contains("rptCloudMigrator") ||
                            ReportName.Contains("rptCloudMigrator_Type2"))
                    {
                        List<ReportParameter> parameters = new List<ReportParameter>();
                        parameters.Add(new ReportParameter("ClientId", Request.QueryString["ClientId"].ToString()));
                        ReportViewerControl.ServerReport.SetParameters(parameters);
                        ReportViewerControl.ShowParameterPrompts = true;
                    }

                    if (ReportName.Contains("rptPrdSuptHM") 
                        || ReportName.Contains("rptTicketVolumeByStatus") ||
                        ReportName.Contains("rptTicketBreakdownByUnAssignedtoPerson")
                        || ReportName.Contains("rptTicketBreakdownByAssignedGroup") || ReportName.Contains("rptIncidentMetricByDataSource")
                        || ReportName.Contains("rptTicketBreakdownByAssignedToPerson")
                        || ReportName.Contains("rpt_se2_IncidentsMissedSLA")
                        || ReportName.Contains("rpt_se2_SLA_Alert") || ReportName.Contains("rptFirstTimeRight") ||
                        ReportName.Contains("rptTicketBreakdownBacklog") ||
                        ReportName.Contains("rptDeliveryRevision") ||
                        ReportName.Contains("rptScheduleVariance") ||
                        ReportName.Contains("rptEffortVariance") 
                        )
                    {
                        List<ReportParameter> parameters = new List<ReportParameter>();
                        parameters.Add(new ReportParameter("clientid", Request.QueryString["clientid"].ToString()));
                        ReportViewerControl.ServerReport.SetParameters(parameters);
                        ReportViewerControl.ShowParameterPrompts = true;
                    }

                    if (ReportName.Contains("rptAssignedGroupDrillDown"))
                    {

                        List<ReportParameter> parameters = new List<ReportParameter>();
                        //ReportViewerControl.ShowParameterPrompts = true;
                        //parameters.Add(new ReportParameter("PAReportName", Request.QueryString["PAReportName"].ToString()));
                        parameters.Add(new ReportParameter("SupportGroupType", Request.QueryString["SupportGroupType"].ToString()));
                        var values = Request.QueryString.GetValues("Priority");
                        string priority="";
                        foreach (var item in values)
                        {
                            priority += "'"+item+"',";
                        }
                        priority = priority.Remove(priority.Length - 1);

                        var Typevalues = Request.QueryString.GetValues("Type");
                        string type = "";
                        foreach (var item in Typevalues)
                        {
                            type += "'" + item + "',";
                        }
                        type = type.Remove(type.Length - 1);

                        string s = "[Priority] IN (" + priority + ") and [Type] IN (" + type + ") ";
                       parameters.Add(new ReportParameter("WhereClause",s));

                       parameters.Add(new ReportParameter("StartDate", Request.QueryString["StartDate"].ToString()));
                       parameters.Add(new ReportParameter("EndDate", Request.QueryString["EndDate"].ToString()));
                       if (!string.IsNullOrEmpty(Request.QueryString["AgeingNumber"]))
                       {
                           parameters.Add(new ReportParameter("AgeingNumber", Request.QueryString["AgeingNumber"].ToString()));
                       }
                      
                     
                        ReportViewerControl.ServerReport.SetParameters(parameters);
                        //ReportViewerControl.ShowParameterPrompts = true;

                    }

                    if (ReportName.Contains("rptUnAssignedtoPersonDrillDown"))
                    {

                        List<ReportParameter> parameters = new List<ReportParameter>();
                        //ReportViewerControl.ShowParameterPrompts = true;
                        //parameters.Add(new ReportParameter("PAReportName", Request.QueryString["PAReportName"].ToString()));
                        parameters.Add(new ReportParameter("SupportGroupType", Request.QueryString["SupportGroupType"].ToString()));
                        ReportParameter param = new ReportParameter("Priority");
                        var values = Request.QueryString.GetValues("Priority");
                       //ArrayList a = new ArrayList();
                       //foreach (var item in values)
                       //{
                       //    a.Add(item);
                       //}
                        //priority = priority.Remove(priority.Length - 1);

                        //string s = "[Priority] IN (" + priority + ")";
                        //string[] values = new string[] { "a", "b", "c" };

                       param.Values.AddRange(values);

                       parameters.Add(param);

                       ReportParameter paramType = new ReportParameter("Type");
                       var Typevalues = Request.QueryString.GetValues("Type");
                       //ArrayList a = new ArrayList();
                       //foreach (var item in values)
                       //{
                       //    a.Add(item);
                       //}
                       //priority = priority.Remove(priority.Length - 1);

                       //string s = "[Priority] IN (" + priority + ")";
                       //string[] values = new string[] { "a", "b", "c" };

                       paramType.Values.AddRange(Typevalues);

                       parameters.Add(paramType);


                        parameters.Add(new ReportParameter("StartDate", Request.QueryString["StartDate"].ToString()));
                        parameters.Add(new ReportParameter("EndDate", Request.QueryString["EndDate"].ToString()));
                     


                        ReportViewerControl.ServerReport.SetParameters(parameters);
                        //ReportViewerControl.ShowParameterPrompts = true;

                    }
                    if (ReportName.Contains("ReportConfigurationData"))
                        {

                        List<ReportParameter> parameters = new List<ReportParameter>();
                        //ReportViewerControl.ShowParameterPrompts = true;
                        //parameters.Add(new ReportParameter("PAReportName", Request.QueryString["PAReportName"].ToString()));
                        parameters.Add(new ReportParameter("clientId", Request.QueryString["ClientID"].ToString()));
                        ReportViewerControl.ServerReport.SetParameters(parameters);
                        ReportViewerControl.ShowParameterPrompts = true;

                        }

                    if (ReportName.Contains("rptAgingBacklog"))
                    {

                        List<ReportParameter> parameters = new List<ReportParameter>();
                        //ReportViewerControl.ShowParameterPrompts = true;
                        //parameters.Add(new ReportParameter("PAReportName", Request.QueryString["PAReportName"].ToString()));
                        parameters.Add(new ReportParameter("ClientId", Request.QueryString["ClientId"].ToString()));
                        ReportViewerControl.ServerReport.SetParameters(parameters);
                        ReportViewerControl.ShowParameterPrompts = true;

                    }



                    if (ReportName.Equals("PAReportTemplateTrendLineOneColumn"))
                        {

                        List<ReportParameter> parameters = new List<ReportParameter>();
                        //ReportViewerControl.ShowParameterPrompts = true;
                        parameters.Add(new ReportParameter("PAReportName", Request.QueryString["PAReportName"].ToString()));
                        parameters.Add(new ReportParameter("ClientId", Request.QueryString["ClientID"].ToString()));
                        parameters.Add(new ReportParameter("CalculationType", Request.QueryString["CalculationType"].ToString()));
                        ReportViewerControl.ServerReport.SetParameters(parameters);
                        //ReportViewerControl.ShowParameterPrompts = true;

                        }




                    if (ReportName.Equals("PAReportTemplateTrendLine"))
                        {

                        List<ReportParameter> parameters = new List<ReportParameter>();
                        //ReportViewerControl.ShowParameterPrompts = true;
                        parameters.Add(new ReportParameter("PAReportName", Request.QueryString["PAReportName"].ToString()));
                        parameters.Add(new ReportParameter("ClientId", Request.QueryString["ClientID"].ToString()));
                        ReportViewerControl.ServerReport.SetParameters(parameters);

                        ReportViewerControl.ShowParameterPrompts = false;

                        }


                    


                    //if (ReportName.Contains("NLGDemoGraph"))
                    //    {

                    //    List<ReportParameter> parameters = new List<ReportParameter>();
                    //    //ReportViewerControl.ShowParameterPrompts = true;
                    //    parameters.Add(new ReportParameter("PAReportName", Request.QueryString["PAReportName"].ToString()));
                    //    parameters.Add(new ReportParameter("ClientId", Request.QueryString["ClientID"].ToString()));
                    //    ReportViewerControl.ServerReport.SetParameters(parameters);

                    //    ReportViewerControl.ShowParameterPrompts = false;

                    //    }




                    if (ReportName.Contains("clientEngagements"))
                        {

                        ReportViewerControl.ShowParameterPrompts = true;

                        }

                    if (ReportName.Contains("clientsByRegionAndVertical"))
                        {

                        ReportViewerControl.ShowParameterPrompts = true;

                        }


                    if (ReportName.Contains("HeatMapReport"))
                    {

                        List<ReportParameter> parameters = new List<ReportParameter>();
                        parameters.Add(new ReportParameter("ServiceArea", Request.QueryString["variableName"].ToString()));
                        parameters.Add(new ReportParameter("reportname", Request.QueryString["selectedReportName"].ToString()));
                        parameters.Add(new ReportParameter("ClientID", Request.QueryString["ClientId"].ToString()));
                        ReportViewerControl.ServerReport.SetParameters(parameters);


                    }


                    if (ReportName.Contains("HeatMapTrend"))
                    {

                        List<ReportParameter> parameters = new List<ReportParameter>();
                        parameters.Add(new ReportParameter("reportname", Request.QueryString["selectedReportName"].ToString()));
                        parameters.Add(new ReportParameter("ClientID", Request.QueryString["ClientId"].ToString()));
                        parameters.Add(new ReportParameter("ServiceArea", Request.QueryString["variableName"].ToString()));
                        ReportViewerControl.ServerReport.SetParameters(parameters);
                        //ReportViewerControl.ShowToolBar = false;

                    }


                    if (ReportName.Contains("rptAvgRsptimeQ") || ReportName.Contains("rptAvgreslntimeM") || ReportName.Contains("rptTicketTrend"))
                    {

                        List<ReportParameter> parameters = new List<ReportParameter>();
                        parameters.Add(new ReportParameter("clientid", Request.QueryString["clientid"].ToString()));
                        ReportViewerControl.ServerReport.SetParameters(parameters);
                        ReportViewerControl.ShowParameterPrompts = true;

                    }


                    if (ReportName.Contains("rptIncPrdSupt") || ReportName.Contains("rptMRPrdSupt") || ReportName.Contains("rptPblmPrdSupt"))
                    {
                        List<ReportParameter> parameters = new List<ReportParameter>();
                        parameters.Add(new ReportParameter("clientid", Request.QueryString["ClientId"].ToString()));
                        if (Request.QueryString["Waivedindicator"].ToString() != "")
                        {
                            parameters.Add(new ReportParameter("Waivedindicator", Request.QueryString["Waivedindicator"].ToString()));
                        }
                        if (Request.QueryString["ResolutionYear"].ToString() != "")
                        {
                            parameters.Add(new ReportParameter("ResolutionYear", Request.QueryString["ResolutionYear"].ToString()));
                        }
                        if (Request.QueryString["ResolutionMonth"].ToString() != "")
                        {
                            parameters.Add(new ReportParameter("ResolutionMonth", Request.QueryString["ResolutionMonth"].ToString()));
                        }
                        if (Request.QueryString["ResolvedByGroup"].ToString() != "")
                        {
                            parameters.Add(new ReportParameter("Resolvedbygroup", Request.QueryString["ResolvedByGroup"].ToString()));
                        }
                        ReportViewerControl.ServerReport.SetParameters(parameters);
                        //ReportViewerControl.ShowToolBar = false;

                    }

                    if (ReportName.Contains("rptIncResp") || ReportName.Contains("rptMaintResp") || ReportName.Contains("rptPblmResp") || 
                        ReportName.Contains("rptPblmEstResp") || ReportName.Contains("rptMaintEstResp") || ReportName.Contains("rptIncEstResp") 
                        || ReportName.Contains("rptIncResln") || ReportName.Contains("rptMaintResln") || ReportName.Contains("rptPblmResln"))
                    {

                        List<ReportParameter> parameters = new List<ReportParameter>();
                        parameters.Add(new ReportParameter("clientid", Request.QueryString["clientid"].ToString()));
                        ReportViewerControl.ServerReport.SetParameters(parameters);
                        
                        //List<ReportParameter> parameters = new List<ReportParameter>();
                        //parameters.Add(new ReportParameter("clientid", Request.QueryString["ClientId"].ToString()));
                        //if (Request.QueryString["RecordIncluded"].ToString() != "")
                        //{
                        //    parameters.Add(new ReportParameter("RecordIncluded", Request.QueryString["RecordIncluded"].ToString()));
                        //}
                        //if (Request.QueryString["ResVendor"].ToString() != "")
                        //{
                        //    parameters.Add(new ReportParameter("resolutionbyVendor", Request.QueryString["ResVendor"].ToString()));
                        //}
                        //if (Request.QueryString["Resgroup"].ToString() != "")
                        //{
                        //    parameters.Add(new ReportParameter("resolutionbygroup", Request.QueryString["Resgroup"].ToString()));
                        //}
                        //if (Request.QueryString["waived"].ToString() != "")
                        //{
                        //    parameters.Add(new ReportParameter("waived", Request.QueryString["waived"].ToString()));
                        //}
                        //ReportViewerControl.ServerReport.SetParameters(parameters);
                        ReportViewerControl.ShowParameterPrompts = true;
                        //ReportViewerControl.ShowToolBar = false;

                    }

                    if (ReportName.Contains("ServiceBASummary"))
                    {
                        //ReportViewerControl.ShowToolBar = false;
                        List<ReportParameter> parameters = new List<ReportParameter>();
                        parameters.Add(new ReportParameter("clientid", Request.QueryString["ClientId"].ToString()));
                        if (Request.QueryString["ecdyear"].ToString() != "")
                        {
                            parameters.Add(new ReportParameter("ecdyear", Request.QueryString["ecdyear"].ToString()));
                        }
                        if (Request.QueryString["ecdmonth"].ToString() != "")
                        {
                            parameters.Add(new ReportParameter("ecdmonth", Request.QueryString["ecdmonth"].ToString()));
                        }
                        ReportViewerControl.ServerReport.SetParameters(parameters);
                    }
                    if (ReportName.Contains("rptSLADashboard"))
                    {
                        //ReportViewerControl.ShowToolBar = false;
                        List<ReportParameter> parameters = new List<ReportParameter>();
                        parameters.Add(new ReportParameter("clientid", Request.QueryString["ClientId"].ToString()));
                        if (Request.QueryString["IncludeRecord"].ToString() != "")
                        {
                            parameters.Add(new ReportParameter("incRec", Request.QueryString["IncludeRecord"].ToString()));
                        }
                        if (Request.QueryString["ECD_ACDYear"].ToString() != "")
                        {
                            parameters.Add(new ReportParameter("ecdyear", Request.QueryString["ECD_ACDYear"].ToString()));
                        }
                        if (Request.QueryString["ECD_ACDMonth"].ToString() != "")
                        {
                            parameters.Add(new ReportParameter("ecdmonth", Request.QueryString["ECD_ACDMonth"].ToString()));
                        }
                        if (Request.QueryString["ServiceReqCategory"].ToString() != "")
                        {
                            parameters.Add(new ReportParameter("SRCat", Request.QueryString["ServiceReqCategory"].ToString()));
                        }
                        ReportViewerControl.ServerReport.SetParameters(parameters);
                    }
                    //if (ReportName.Contains("rptIdeaBenefitbyLOB"))
                    //{
                    //    //ReportViewerControl.ShowToolBar = false;
                    //    List<ReportParameter> parameters = new List<ReportParameter>();
                    //    parameters.Add(new ReportParameter("clientid", Request.QueryString["ClientId"].ToString()));
                    //    if (Request.QueryString["ideaSub"].ToString() != "")
                    //    {
                    //        parameters.Add(new ReportParameter("ideaSub", Request.QueryString["ideaSub"].ToString()));
                    //    }
                    //    if (Request.QueryString["ideastatus"].ToString() != "")
                    //    {
                    //        parameters.Add(new ReportParameter("ideastatus", Request.QueryString["ideastatus"].ToString()));
                    //    }
                    //    if (Request.QueryString["ideaactyear"].ToString() != "")
                    //    {
                    //        parameters.Add(new ReportParameter("ideaactyear", Request.QueryString["ideaactyear"].ToString()));
                    //    }
                    //    if (Request.QueryString["ideaactmonth"].ToString() != "")
                    //    {
                    //        parameters.Add(new ReportParameter("ideaactmonth", Request.QueryString["ideaactmonth"].ToString()));
                    //    }
                    //    ReportViewerControl.ServerReport.SetParameters(parameters);
                    //}
                    //if (ReportName.Contains("rptIdeaLOBTrend"))
                    //{
                    //    //ReportViewerControl.ShowToolBar = false;
                    //    List<ReportParameter> parameters = new List<ReportParameter>();
                    //    parameters.Add(new ReportParameter("clientid", Request.QueryString["ClientId"].ToString()));
                    //    if (Request.QueryString["ideaSub"].ToString() != "")
                    //    {
                    //        parameters.Add(new ReportParameter("ideaSub", Request.QueryString["ideaSub"].ToString()));
                    //    }
                    //    if (Request.QueryString["ideastatus"].ToString() != "")
                    //    {
                    //        parameters.Add(new ReportParameter("ideastatus", Request.QueryString["ideastatus"].ToString()));
                    //    }
                    //    ReportViewerControl.ServerReport.SetParameters(parameters);
                    //}
                    //if (ReportName.Contains("rptIdeaInnoTrend"))
                    //{
                    //    //ReportViewerControl.ShowToolBar = false;
                    //    List<ReportParameter> parameters = new List<ReportParameter>();
                    //    parameters.Add(new ReportParameter("clientid", Request.QueryString["ClientId"].ToString()));
                    //    if (Request.QueryString["ideaSub"].ToString() != "")
                    //    {
                    //        parameters.Add(new ReportParameter("ideaSub", Request.QueryString["ideaSub"].ToString()));
                    //    }
                    //    ReportViewerControl.ServerReport.SetParameters(parameters);
                    //}
                    ReportViewerControl.ServerReport.Refresh();
                }
            }
            catch
            {
                if (Session["UserId"] == null)
                {
                    Response.Redirect("~/Home/Appstore");
                }
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Oops, some error occured while processing report')", true);
            }
        }

        public void AllDdl()
        {
            //ddlPortfolio.DataSource = VDispositionSummaryChartContext.GetPortfolioList();
            //ddlPortfolio.DataTextField = "PortfolioName";
            //ddlPortfolio.DataValueField = "PortfolioID";
            //ddlPortfolio.DataBind();
            //ddlPortfolio.Items.Insert(0, new ListItem("All", ""));

            //ddlApplication.DataSource = ApplicationNameContext.GetAllApplicationNameList();
            //ddlApplication.DataTextField = "AppName";
            //ddlApplication.DataValueField = "AppID";
            //ddlApplication.DataBind();
            //ddlApplication.Items.Insert(0, new ListItem("All", ""));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReportView_Click(object sender, EventArgs e)
        {
            //    string param1 = Convert.ToString(ddlPortfolio.SelectedValue);
            //    if (param1 == "All")
            //        param1 = "";

            //    string param2 = Convert.ToString(ddlApplication.SelectedValue);
            //    if (param2 == "All")
            //        param2 = "";

            //    ReportViewerControl.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            //    ReportViewerControl.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServer"]);
            //    ////for TxDOT test site
            //    //ReportViewerControl.ServerReport.ReportPath = "/APRToolReports/" + ReportName;
            //    //for NTT test site
            //    ReportViewerControl.ServerReport.ReportPath = "/APRToolReports_NTT_Test/" + ReportName;
            //    List<ReportParameter> parameters = new List<ReportParameter>();
            //    parameters.Add(new ReportParameter("Section", ReportID));
            //    parameters.Add(new ReportParameter("param1", param1));
            //    parameters.Add(new ReportParameter("param2", param2));
            //    ReportViewerControl.ServerReport.SetParameters(parameters);
            //    ReportViewerControl.ServerReport.Refresh();
        }


        [Serializable]
        public class CustomReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
        {


            private string _UserName;
            private string _PassWord;
            private string _DomainName;

            //#region IReportServerCredentials Members
            public CustomReportCredentials()
            {
                _UserName = "";
                _PassWord = "";
                _DomainName = "";
            }


            public bool GetFormsCredentials(out Cookie authCookie,
                          out string userName, out string password,
                          out string authority)
            {
                authCookie = null;
                userName = null;
                password = null;
                authority = null;

                // Not using form credentials
                return false;
            }



            public WindowsIdentity ImpersonationUser
            {
                get
                {
                    return null;
                }
            }
            //public WindowsIdentity ImpersonationUser
            //    {
            //    get
            //        {

            //        System.Security.Principal.WindowsImpersonationContext impersonationContext = null;
            //        System.Security.Principal.WindowsIdentity currentWindowsIdentity = null;
            //        currentWindowsIdentity = (System.Security.Principal.WindowsIdentity)System.Web.HttpContext.Current.User.Identity;
            //        impersonationContext = currentWindowsIdentity.Impersonate();

            //        //Insert your code that runs under the security context of the authenticating user here.

            //        impersonationContext.Undo();

            //        return currentWindowsIdentity; // not use ImpersonationUser
            //        }
            //    }

            public System.Net.ICredentials NetworkCredentials
            {
                get
                {


                    string userName = System.Configuration.ConfigurationManager.AppSettings["ReportUser"].ToString();
                    string password = System.Configuration.ConfigurationManager.AppSettings["ReportUserpwd"].ToString();
                    string domainName = System.Configuration.ConfigurationManager.AppSettings["ReportUserDomain"].ToString();
                    return new System.Net.NetworkCredential(userName, password, domainName);
                }
            }


        }
        //    }
        protected void ddlPortfolio_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //    if (ddlPortfolio.SelectedIndex == 0)
            //    {
            //        ddlApplication.DataSource = ApplicationNameContext.GetAllApplicationNameList();
            //    }
            //    else
            //    {
            //        ddlApplication.DataSource = ApplicationNameContext.GetAppNameListForProfileSetup(Convert.ToInt32(ddlPortfolio.SelectedValue));
            //    }
            //        ddlApplication.DataTextField = "AppName";
            //        ddlApplication.DataValueField = "AppID";
            //        ddlApplication.DataBind();
            //        ddlApplication.Items.Insert(0, new ListItem("All", ""));
        }
    }
}