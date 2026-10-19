using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.DemandManager.Models;
using WebUI.Dto;

namespace WebUI.Areas.DemandManager.Controllers
{
    public class DetailDemandController : Controller
    {
        //
        // GET: /DemandManager/DetailDemand/

        public ActionResult Index(int DemandId)
        {

            var UserId = (int)Session["UserId"];
           // var Cl = (Client)Session["Client"];
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            var DemandAuth = DMHelper.DemandAuthorize(Cl.ClientId, DemandId);
            if (DemandAuth)
            {
                DemandViewModel DemandViewModelList = new DemandViewModel();

                DemandViewModelList = DMHelper.getDemandDetails(DemandId);//
                DemandViewModelList.BusinessBenefitsList = DMHelper.GetBusinessBenefitsDetails(DemandId);
                DemandViewModelList.ITSavingsList = DMHelper.GetITSavingsDetails(DemandId);
                DemandViewModelList.PriorityHistory = DMHelper.GetPriorityHistoryDet(DemandId);
                DemandViewModelList.ImpPriorityHistory = DMHelper.GetImpPriorityHistoryDetails(DemandId);
                DemandViewModelList.StatusHistory = DMHelper.GetStatusHistoryDetails(DemandId);
                DemandViewModelList.Approvers = DMHelper.getApproversForView(DemandId);
                DemandViewModelList.Comments = DMHelper.GetCommentsDetails(DemandId);
               // DemandViewModelList.RiskMitigationList = DMHelper.GetRiskAndMitigationDetails(DemandId);
              //  DemandViewModelList.MetricTargetList = DMHelper.GetMetricTargetDetails(DemandId);
                DemandViewModelList.DetailClassification = DMHelper.GetDetailClassification(DemandId);
                DemandViewModelList.DetailConfigItem = DMHelper.GetDetailConfigItem(DemandId);
                DemandViewModelList.ImpPriorityHistoryList = DMHelper.GetImpPriorityHistoryDetails(DemandId);
                DemandViewModelList.TaskAndActivitiesList = DMHelper.GetTaskAndActivitiesDetails(Cl.ClientId,DemandId);
                DemandViewModelList.TaskActivitiesHistory = DMHelper.GetTaskActivitiesHistoryDetails(DemandId);

                return View(DemandViewModelList);
            }
            else
            {
                return View("DemandNotFound");
            }
        }
    }

}

