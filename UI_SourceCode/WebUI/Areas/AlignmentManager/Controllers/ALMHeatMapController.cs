using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.AlignmentManager.Dto;
using WebUI.Areas.AlignmentManager.Objects;

namespace WebUI.Areas.AlignmentManager.Controllers
{
   
    public class ALMHeatMapController : Controller
    {
        //
        // GET: /AlignmentManager/ALMHeatMap/
  
        public ActionResult Index(int cid=0, int? clientId = null, int? ReportId = null, string detailid = null, string heatmapvariableName = null)
        {

            clientId = cid;
            Session["varHeatMap"] = null;
            heatMapInput varHeatMapInput = new heatMapInput();
            varHeatMapInput.ClientId = cid;
            //if (Session["varHeatMap"] == null)
            //    {
            //    varHeatMapInput = new heatMapInput();
            //    }
            //else
            //    {
            //    varHeatMapInput = (heatMapInput)Session["varHeatMap"];
            //    if (clientId != null)
            //        {
            //        if (clientId != varHeatMapInput.ClientId || ReportId != varHeatMapInput.HeatMapReportId)
            //            {
            //            varHeatMapInput = new heatMapInput();
            //            }
            //        }

            //    }

            //if (ReportDisplay != null)
            //    {
            //    return RedirectToAction("DisplayReport", "ALMHeatMap", new
            //    {
            //        clientId = clientId,
            //        reportId = ReportId,
            //        heatmapvariableName = heatmapvariableName
            //    });

            //    }

            varHeatMapInput.detailId = (detailid == null ? "" : detailid);
            varHeatMapInput.heatMapVariableName = (heatmapvariableName == null ? "" : heatmapvariableName);
            varHeatMapInput.HeatMapReportId = (ReportId == null ? 0 : ReportId);
            if (varHeatMapInput.HeatMapReportId > 0)
            {
                System.Web.Helpers.AntiForgery.Validate();

                SelectListItem lstItem = GetDropDownListItems.HeatMapReportNames(Convert.ToInt32(clientId)).Where(c => Convert.ToInt32(c.Value) == varHeatMapInput.HeatMapReportId).FirstOrDefault();
                if (lstItem != null)
                {
                    varHeatMapInput.HeatMapReportName = lstItem.Text;
                }
            }

            if (clientId == null)
            {
                varHeatMapInput.HeatMapReportNames.ToList().Clear();
            }
            //Session.Add("varHeatMap", varHeatMapInput);
            return View("AlmMainPageForHeatMap", varHeatMapInput);
        }


        public ActionResult DisplayReport(string ssrsReport, string reportId, string ClientId, string heatMapVariableName)
        {
            //return Redirect("/Reports/ReportViewer.aspx?ReportName="+ReportName+"&ClientId="+ClientId+"&variableName="+heatMapVariableName);



            IEnumerable<SelectListItem> list = GetDropDownListItems.HeatMapReportNames(Convert.ToInt32(ClientId));
            string SelectedReportName = "";
            foreach (var item in list)
            {
                if (item.Value == reportId.ToString())
                {
                    SelectedReportName = item.Text;
                    break;
                }
            }

            //return Redirect("http:/localhost/nttamomar10/Reports/WebForm1.aspxasdfasfdad");
            var appPath = GetBaseUrl();


            return Redirect(appPath + "/Reports/ReportViewer.aspx?ReportName=" + ssrsReport + "&SelectedReportName=" + SelectedReportName + "&ClientId=" + ClientId + "&variableName=" + Server.UrlEncode(heatMapVariableName));





        }


        public string GetBaseUrl()
        {

            var baseUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~");
            //this.HttpContext
            //var request = this.HttpContext.Current.Request;
            //var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            //if (!string.IsNullOrWhiteSpace(appUrl)) appUrl += "/";

            //var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            //return baseUrl;
            return baseUrl;
        }

        public ActionResult ReportDetails(int ClientId = 0, int ReportId = 0)
        {
            heatMapInput varHeatMapInput = new heatMapInput();
            varHeatMapInput.ClientId = ClientId;
            varHeatMapInput.HeatMapReportId = ReportId;
            IEnumerable<SelectListItem> list = GetDropDownListItems.HeatMapReportNames(ClientId).Where(c => c.Value == ReportId.ToString());
            //string ReportName = "";
            if (list != null)
            {
                varHeatMapInput.HeatMapReportName = list.FirstOrDefault().Text;
            }


            return View("heatMapReportDetails", varHeatMapInput);
        }

        public ActionResult FilterReportData(int ClientId = 0, int ReportId = 0, string heatmapvariableName = "")
        {
            heatMapInput varHeatMapInput = new heatMapInput();
            varHeatMapInput.ClientId = ClientId;
            varHeatMapInput.HeatMapReportId = ReportId;
            varHeatMapInput.heatMapVariableName = heatmapvariableName;
            IEnumerable<SelectListItem> list = GetDropDownListItems.HeatMapReportNames(ClientId).Where(c => c.Value == ReportId.ToString());
            //string ReportName = "";
            if (list != null)
            {
                varHeatMapInput.HeatMapReportName = list.FirstOrDefault().Text;
            }


            return View("heatMapReportFilterDetails", varHeatMapInput);
        }




        [HttpGet]
        public ActionResult DisplayClientHeatMaps(int cid=0, int ClientId = 0, int? ReportId = null, string detailid = null, string heatmapvariableName = null, string ReportToDisplay = null, string ReportDisplay = null, string ssrsReport = null)
        {

            if (cid != 0)
            {
                ClientId = cid;
            }
            //if ReportToDisplay is not Null and variableName = is Null to display whole data from GetReport
            if (ReportToDisplay != null && ReportToDisplay == "ReportData")
            {
                return RedirectToAction("ReportDetails", new { ClientId = ClientId, ReportId = (ReportId == null ? 0 : Convert.ToInt32(ReportId)) });
            }

            if (ReportToDisplay != null && ReportToDisplay == "FilterData")
            {
                return RedirectToAction("FilterReportData", new { ClientId = ClientId, ReportId = (ReportId == null ? 0 : Convert.ToInt32(ReportId)), heatmapvariableName = heatmapvariableName });
            }



            heatMapInput varHeatMapInput;  //= new heatMapInput();
            if (Session["varHeatMap"] == null)
            {
                varHeatMapInput = new heatMapInput();
            }
            else
            {
                varHeatMapInput = (heatMapInput)Session["varHeatMap"];
                if (varHeatMapInput.heatMapData != null)
                {
                    heatMapItem htMapItem = varHeatMapInput.heatMapData.heatMapItems.Where(m => m.id == detailid).FirstOrDefault();
                    if (htMapItem != null)
                        varHeatMapInput.heatMapVariableName = htMapItem.heatMapItemName;
                }
            }

            varHeatMapInput.heatMapVariableName = heatmapvariableName;
            if (ReportDisplay != null)
            {
                return RedirectToAction("DisplayReport", "ALMHeatMap", new
                {
                    clientId = ClientId,
                    ReportId = ReportId,
                    heatmapvariableName = varHeatMapInput.heatMapVariableName,
                    ssrsReport = ssrsReport
                });

            }





            int reportId = (ReportId == null ? 0 : Convert.ToInt32(ReportId));
            if (reportId != varHeatMapInput.HeatMapReportId || varHeatMapInput.ClientId != ClientId || reportId == 0)
            {
                Session["varHeatMap"] = null;
                //varHeatMapInput.heatMapData = new heatMapData(0, 0);
                //varHeatMapInput.heatMapVariableName = "";
                if (reportId > 0 && ClientId > 0)
                {
                    varHeatMapInput.heatMapData = new heatMapData(ClientId, reportId);
                    varHeatMapInput.JsonHeatMapItems = Json(varHeatMapInput.heatMapData.heatMapItems);   //new JsonHelper().ConvertObjectToJSon(varHeatMapInput.heatMapData.heatMapItems);
                }
                else
                {
                    varHeatMapInput.heatMapData = null;
                }

            }




            varHeatMapInput.ClientId = ClientId;
            varHeatMapInput.HeatMapReportId = reportId;
            varHeatMapInput.detailId = detailid;

            if (varHeatMapInput.HeatMapReportId > 0)
            {
                SelectListItem lstItem = GetDropDownListItems.HeatMapReportNames(Convert.ToInt32(ClientId)).Where(c => Convert.ToInt32(c.Value) == varHeatMapInput.HeatMapReportId).FirstOrDefault();
                if (lstItem != null)
                {
                    varHeatMapInput.HeatMapReportName = lstItem.Text;
                }
            }


            //varHeatMapInput.GetHeatMapData(ClientId, (ReportId == null ? 0 : ReportId));
            Session.Add("varHeatMap", varHeatMapInput);
            return View("AlmMainPageForHeatMap", varHeatMapInput);

        }




        [HttpGet]
        public ActionResult ReportViewer(int cid=0, int ClientId = 0, int? ReportId = null, string detailid = null, string heatmapvariableName = null, string ReportToDisplay = null, string ReportDisplay = null, string ssrsReport = null)
        {
            if (cid != 0)
            {
                ClientId = cid;
            }
            //if ReportToDisplay is not Null and variableName = is Null to display whole data from GetReport
            if (ReportToDisplay != null && ReportToDisplay == "ReportData")
            {
                return RedirectToAction("ReportDetails", new { ClientId = ClientId, ReportId = (ReportId == null ? 0 : Convert.ToInt32(ReportId)) });
            }

            if (ReportToDisplay != null && ReportToDisplay == "FilterData")
            {
                return RedirectToAction("FilterReportData", new { ClientId = ClientId, ReportId = (ReportId == null ? 0 : Convert.ToInt32(ReportId)), heatmapvariableName = heatmapvariableName });
            }



            heatMapInput varHeatMapInput;  //= new heatMapInput();
            if (Session["varHeatMap"] == null)
            {
                varHeatMapInput = new heatMapInput();
            }
            else
            {
                varHeatMapInput = (heatMapInput)Session["varHeatMap"];
                if (varHeatMapInput.heatMapData != null)
                {
                    heatMapItem htMapItem = varHeatMapInput.heatMapData.heatMapItems.Where(m => m.id == detailid).FirstOrDefault();
                    if (htMapItem != null)
                        varHeatMapInput.heatMapVariableName = htMapItem.heatMapItemName;
                }
            }

            varHeatMapInput.heatMapVariableName = heatmapvariableName;
            if (ReportDisplay != null)
            {
                return RedirectToAction("DisplayReport", "ALMHeatMap", new
                {
                    clientId = ClientId,
                    ReportId = ReportId,
                    heatmapvariableName = varHeatMapInput.heatMapVariableName,
                    ssrsReport = ssrsReport
                });

            }





            int reportId = (ReportId == null ? 0 : Convert.ToInt32(ReportId));
            if (reportId != varHeatMapInput.HeatMapReportId || varHeatMapInput.ClientId != ClientId || reportId == 0)
            {
                Session["varHeatMap"] = null;
                //varHeatMapInput.heatMapData = new heatMapData(0, 0);
                //varHeatMapInput.heatMapVariableName = "";
                if (reportId > 0 && ClientId > 0)
                {
                    varHeatMapInput.heatMapData = new heatMapData(ClientId, reportId);
                    varHeatMapInput.JsonHeatMapItems = Json(varHeatMapInput.heatMapData.heatMapItems);   //new JsonHelper().ConvertObjectToJSon(varHeatMapInput.heatMapData.heatMapItems);
                }
                else
                {
                    varHeatMapInput.heatMapData = null;
                }

            }




            varHeatMapInput.ClientId = ClientId;
            varHeatMapInput.HeatMapReportId = reportId;
            varHeatMapInput.detailId = detailid;

            if (varHeatMapInput.HeatMapReportId > 0)
            {
                SelectListItem lstItem = GetDropDownListItems.HeatMapReportNames(Convert.ToInt32(ClientId)).Where(c => Convert.ToInt32(c.Value) == varHeatMapInput.HeatMapReportId).FirstOrDefault();
                if (lstItem != null)
                {
                    varHeatMapInput.HeatMapReportName = lstItem.Text;
                }
            }


            //varHeatMapInput.GetHeatMapData(ClientId, (ReportId == null ? 0 : ReportId));
            Session.Add("varHeatMap", varHeatMapInput);
            return View("ReportViewer", varHeatMapInput);

        }





        public ActionResult DisplayTrendReport(int cid = 0, int ClientId = 0, string PAReportName = "", int ReportId = 0)
        {

            if (cid != 0)
            {
                ClientId = cid;
            }

            PAReportsInput paReportInput = new PAReportsInput();

            if (ClientId > 0)
            {
                paReportInput.ClientId = ClientId;
            }
            if (PAReportName != "")
            {

                paReportInput.PAReportName = PAReportName;
            }
            if (ReportId > 0)
            {
                System.Web.Helpers.AntiForgery.Validate();

                paReportInput.SSRSTemplateFileId = paReportInput.GetSSRSReportTemplateId((ReportId));
                //ReportName="+@Model.ReportName+ "&ClientID="+Model.ClientId+"&PAReportName="+@Model.PAReportName + ""
                ReportChartTemplates chartTemplateInfo = AlignmentHelper.GetChartInfo(paReportInput.SSRSTemplateFileId);
                string SSRSUrl = ""; //"ReportName=" + PAReportName + "&ClientID=" + ClientId + "&PAReportName=" + "";

                if (chartTemplateInfo != null)
                {
                    SSRSUrl += "ReportName=" + chartTemplateInfo.SSRSReportTemplateFile;
                    SSRSUrl += "&ClientID=" + ClientId;
                    SSRSUrl += "&PAReportName=" + PAReportName;
                    SSRSUrl += "&" + chartTemplateInfo.ParameterToPass + "=" + chartTemplateInfo.ParameterValueToPass;
                }
                paReportInput.SSRSReportURLString = SSRSUrl;

            }

            return View("SinglePAReportPage", paReportInput);

        }
    }









}
