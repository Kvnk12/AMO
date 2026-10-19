using Data;
using Domain.Entities.PortfolioAnalyzer;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.PortfolioAnalyzer.Models;

namespace WebUI.Areas.PortfolioAnalyzer.Controllers
{
    public class MultiTenantReportingController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/MultiTenantReporting/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ClientHeatmap()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult AppClientHeatmap(int ApplicationID, int surId = 0)
        //{
        //    List<_ratingSummary> ratingModel = new List<_ratingSummary>();
        //    var paramName = new string[] { "AppId", "SurveyId" };
        //    var paramValue = new object[] { 1, 2 };
        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
        //        var ds = unitOfWork.GetDataFromStoredProcedure("APRAppDashboardRating", paramName, paramValue);

        //        foreach (DataRow dr in ds.Rows)
        //        {
        //            var ratingSummary = new _ratingSummary();
        //            ratingSummary.PerspectiveName = (dr["PerspectiveName"]).ToString();
        //            try
        //            {
        //                ratingSummary.Rating = Convert.ToDecimal(dr["Rating"]);
        //            }
        //            catch { }
        //            try
        //            {
        //                ratingSummary.PerspectiveId = Convert.ToInt32(dr["PerspectiveId"]);
        //            }
        //            catch { }

        //            ratingModel.Add(ratingSummary);
        //        }
        //    }
        //    return Json(new { ratingModel }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult AppClientHeatmap([DataSourceRequest]DataSourceRequest request,string AppName,int AppId)
        {
            List<ClientHeatmapModel> ratingModel = new List<ClientHeatmapModel>();
            var paramName = new string[] { "AppId", "SurveyId" };
            var paramValue = new object[] { AppId, 2 };

            var paramName1 = new string[] { "ClientId", "ApplicationName" };
            var paramValue1 = new object[] { 7, AppName };
            var dt1 = new DataTable();
            var dt2 = new DataTable();
          
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                dt1 = unitOfWork.GetDataFromStoredProcedure("APRAppDashboardRating", paramName, paramValue);

                //foreach (DataRow dr in ds.Rows)
                //{
                //    var ratingSummary = new ClientHeatmapModel();
                //    ratingSummary.PerspectiveName = (dr["PerspectiveName"]).ToString();
                //    try
                //    {
                //        ratingSummary.Rating = Convert.ToDecimal(dr["Rating"]);
                //    }
                //    catch { }
                //    try
                //    {
                //        ratingSummary.PerspectiveId = Convert.ToInt32(dr["PerspectiveId"]);
                //    }
                //    catch { }

                //    ratingModel.Add(ratingSummary);
                //}
            }

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                dt2 = unitOfWork.GetDataFromStoredProcedure("APRAppDashboardChildRating", paramName1, paramValue1);

                //foreach (DataRow dr in ds.Rows)
                //{
                //    var ratingSummary = new ClientHeatmapModel();
                //    ratingSummary.PerspectiveName = (dr["PerspectiveName"]).ToString();
                //    try
                //    {
                //        ratingSummary.Rating = Convert.ToDecimal(dr["Rating"]);
                //    }
                //    catch { }
                //    try
                //    {
                //        ratingSummary.PerspectiveId = Convert.ToInt32(dr["PerspectiveId"]);
                //    }
                //    catch { }

                //    ratingModel.Add(ratingSummary);
                //}
            }
            var dtMerged = MergeTablesByIndex(dt1, dt2);

            foreach (DataRow dr in dtMerged.Rows)
            {
                var ratingSummary = new ClientHeatmapModel();
                ratingSummary.PerspectiveName = (dr["PerspectiveName"]).ToString();
                try
                {
                    ratingSummary.BBTRating = Convert.ToDecimal(dr["Rating"]);
                    ratingSummary.STRating = Convert.ToDecimal(dr["Rating_2"]);
                }
                catch { }
                try
                {
                    ratingSummary.PerspectiveId = Convert.ToInt32(dr["PerspectiveId"]);
                }
                catch { }

                ratingModel.Add(ratingSummary);
            }
            // return Json(new { ratingModel }, JsonRequestBehavior.AllowGet);
            return Json(ratingModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public static DataTable MergeTablesByIndex(DataTable t1, DataTable t2)
        {
            if (t1 == null || t2 == null) throw new ArgumentNullException("t1 or t2", "Both tables must not be null");

            DataTable t3 = t1.Clone();  // first add columns from table1
            foreach (DataColumn col in t2.Columns)
            {
                string newColumnName = col.ColumnName;
                int colNum = 1;
                while (t3.Columns.Contains(newColumnName))
                {
                    newColumnName = string.Format("{0}_{1}", col.ColumnName, ++colNum);
                }
                t3.Columns.Add(newColumnName, col.DataType);
            }
            var mergedRows = t1.AsEnumerable().Zip(t2.AsEnumerable(),
                (r1, r2) => r1.ItemArray.Concat(r2.ItemArray).ToArray());
            foreach (object[] rowFields in mergedRows)
                t3.Rows.Add(rowFields);

            return t3;
        }

    }
}
