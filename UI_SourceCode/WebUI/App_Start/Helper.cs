using Data;
using Domain.Entities.Report;
using NTTData.AMOPortal.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.UI;
using Telerik.Web.UI.PivotGrid.Core;
using Telerik.Web.UI.PivotGrid.Core.Filtering;
using WebUI.Areas.ClientProfileManager.ViewModel;
using WebUI.Areas.IdeaManager.Dto;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.Configuration;
using System.Net;
using Newtonsoft.Json;
using WebUI.Dto;
using WebUI.App_Start;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.Domain.Entities;
using System.Net.Mail;
using log4net;

namespace WebUI.App_Start
{
    public class Helper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Helper));

        public enum PivotGridGroupFilter
        {
            PivotGridLabelGroupFilter = 1,
            PivotGridValueGroupFilter = 2,
            PivotGridReportFilter =3

        }
        public enum PivotGridFilterConditionType
        {
            PivotGridSetCondition =11,
            PivotGridTextComparisonCondition=12,
            PivotGridIntervalCondition=13,
            PivotGridComparisonCondition=14
        }
        public static void DeleteFields(int ViewId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var getallViews = unitOfWork.ReportViewField.Get(g => g.ReportViewId == ViewId);
            foreach (var views in getallViews)
            {
                unitOfWork.ReportViewField.Delete(views.Id);
            }
            unitOfWork.Save();
        }

        public static void DeleteFields(int ViewId, UnitOfWorks unitOfWork)
        {
            //var contextFactory = new DbContextFactory();
            //var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var getallViews = unitOfWork.ReportViewField.Get(g => g.ReportViewId == ViewId);
            foreach (var views in getallViews)
            {
                unitOfWork.ReportViewField.Delete(views.Id);
            }
            unitOfWork.Save();
        }

        public static void InsertFields(ReportViewField Rvf)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            unitOfWork.ReportViewField.Insert(Rvf);
            unitOfWork.Save();

        }

        //Getting Report Views for DropDown
        public static IEnumerable<SelectListItem> GetViewsList(int uid, int clientid, int ReportFor)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var ViewsList = unitOfWork.ReportView.Get().Where(g => (g.UserId == uid || g.IsPublic ==true) && g.ClientId== clientid && g.ReportId == ReportFor);
            var selectlist = ViewsList.Select(g => new SelectListItem { Text = g.ViewName, Value = g.Id.ToString() }).ToList();
            var sl = new SelectListItem();
            sl.Text = "System Default View";
            sl.Value = "0";
            selectlist.Add(sl);
            var selectList1 = selectlist.OrderBy(g => g.Value).AsEnumerable();
            return selectList1;
        }

        public static IEnumerable<SelectListItem> GetViewsList(int uid, int clientid, int ReportFor, string fromDate, string toDate, string dateType)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var ViewsList = unitOfWork.ReportView.Get().Where(g => (g.UserId == uid || g.IsPublic == true) && g.ClientId == clientid && g.ReportId == ReportFor && g.StartDate==fromDate && g.EndDate==toDate && g.DateType==dateType );
            var selectlist = ViewsList.Select(g => new SelectListItem { Text = g.ViewName, Value = g.Id.ToString() }).ToList();
            var sl = new SelectListItem();
            sl.Text = "System Default View";
            sl.Value = "0";
            selectlist.Add(sl);
            var selectList1 = selectlist.OrderBy(g => g.Value).AsEnumerable();
            return selectList1;
        }



        public static IEnumerable<ReportViewField> GetFieldsList(int ViewId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var returnObj= unitOfWork.ReportViewField.Get().Where(g => g.ReportViewId == ViewId);
            return returnObj;

        }
        //To Check CheckBox
        public static bool IsCheckedForReport(int ViewId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var returnObj = unitOfWork.ReportView.Get().Where(g => g.Id == ViewId && g.IsDefault==true).Any();
            return returnObj;
        }

        //To Check PUBLIC CheckBox
        public static bool IsPublicForReport(int ViewId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var returnObj = unitOfWork.ReportView.Get().Where(g => g.Id == ViewId && g.IsPublic == true).Any();
            return returnObj;
        }

        
        public static ReportViewModel GetReportView(int ViewId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var returnObj = unitOfWork.ReportView.Get().Where(g => g.Id == ViewId).FirstOrDefault();
            ReportViewModel retmodel = new ReportViewModel();
            retmodel.Id = returnObj.Id;
            retmodel.EndDate = returnObj.EndDate;
            retmodel.StartDate = returnObj.StartDate;
            retmodel.UserId = returnObj.UserId;
            retmodel.ViewName = returnObj.ViewName;
            retmodel.ReportId = returnObj.ReportId;
            retmodel.IsPublic = returnObj.IsPublic;
            retmodel.IsDefault = returnObj.IsDefault;
            retmodel.DateType = returnObj.DateType;
            return retmodel;
        }

        //to delete all the IsDefault
        public static void resetIsDefault(int cid, int uid, int reportFor)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var list = unitOfWork.ReportView.Get().Where(g => g.ClientId == cid && g.UserId == uid && g.ReportId == reportFor);
            foreach(var ie in list)
            {
                ie.IsDefault = false;
                unitOfWork.ReportView.Update(ie);
            }
            unitOfWork.Save();
        }
        //to find the Default View
        public static ReportView Find_IsDefault_View(int cid, int uid, int reportFor)
        {
            //var retunid = 0;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var list = unitOfWork.ReportView.Get().Where(g => g.ClientId == cid && g.UserId == uid && g.ReportId == reportFor && g.IsDefault==true).FirstOrDefault();
            if (list != null)
            {
                return list;
            }
            else
            {
                ReportView cv = new ReportView();
                return cv;
            }
            
        }

        public static void DeleteViewFilterDetails(int ViewID)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var getList = unitOfWork.ReportViewFilter.Get().Where(g => g.ViewId == ViewID);
            foreach (var gl in getList)
            {
                unitOfWork.ReportViewFilter.Delete(gl.Id);
            }
            unitOfWork.Save();
        }

        public static int SaveReportViewDetails(int cid,int uid, bool defaultChekbox, string ViewNameText, PivotGridFieldsCollection GridFields,int reportforrrr, bool MakePublic)
        //public static void SaveReportViewDetails(int cid, int uid, bool defaultChekbox, string ViewNameText, PivotGridFieldsCollection GridFields, int reportforrrr)
        {
            
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            // ReportFilterDetailModel cv = new ReportFilterDetailModel();
            ReportView reportInsert = new ReportView();
            reportInsert.UserId = uid;
            reportInsert.ClientId = cid;
            reportInsert.IsPublic = MakePublic;
            if (defaultChekbox == true)
            {
                resetIsDefault(cid, uid, reportforrrr);
                reportInsert.IsDefault = true;
            }
            reportInsert.ReportId = reportforrrr;
            reportInsert.ViewName = ViewNameText == null ? "" : ViewNameText.Trim();

            var Isthere = unitOfWork.ReportView.Get(g => g.ViewName == reportInsert.ViewName && g.ReportId == reportforrrr && g.UserId == uid && g.ClientId == cid).FirstOrDefault();

            if (defaultChekbox == true)
            {
                resetIsDefault(cid, uid, reportforrrr);
            }
            if (Isthere == null)
            {
                unitOfWork.ReportView.Insert(reportInsert);
                unitOfWork.Save();
            }
            else
            {
                reportInsert.Id = Isthere.Id;
                if (defaultChekbox == true)
                {
                    Isthere.IsDefault = true;
                }
                Isthere.IsPublic = MakePublic;
                unitOfWork.ReportView.Update(Isthere);
                unitOfWork.Save();
                //Delete all the Existing Fields 
                DeleteFields(Isthere.Id, unitOfWork);

            }
            foreach (var Ex in GridFields)
            {
                ReportViewField fieldInsert = new ReportViewField();
                fieldInsert.ReportViewId = reportInsert.Id;
                if (Ex.IsHidden == false)
                {
                    switch (Ex.ZoneType)
                    {
                        case PivotGridFieldZoneType.Row:
                            fieldInsert.FieldName = Ex.DataField;
                            fieldInsert.FieldCaption = Ex.Caption;
                            fieldInsert.SortOrder = (int)Ex.SortOrder;
                            // Enum PivotGridFieldZoneType
                            fieldInsert.FieldType = (int)PivotGridFieldZoneType.Row;
                            fieldInsert.Sequence = Ex.ZoneIndex;
                            unitOfWork.ReportViewField.Insert(fieldInsert);
                            break;

                        case PivotGridFieldZoneType.Column:
                            fieldInsert.FieldName = Ex.DataField;
                            fieldInsert.FieldCaption = Ex.Caption;
                            fieldInsert.SortOrder = (int)Ex.SortOrder;
                            // Enum PivotGridFieldZoneType
                            fieldInsert.FieldType = (int)PivotGridFieldZoneType.Column;
                            fieldInsert.Sequence = Ex.ZoneIndex;
                            unitOfWork.ReportViewField.Insert(fieldInsert);
                            break;

                        case PivotGridFieldZoneType.Aggregate:
                            PivotGridAggregateField AG = new PivotGridAggregateField();
                            AG = (PivotGridAggregateField)Ex;
                            fieldInsert.FieldName = Ex.DataField;
                            fieldInsert.FieldCaption = Ex.Caption;
                            fieldInsert.SortOrder = (int)Ex.SortOrder;
                            // Enum PivotGridFieldZoneType
                            fieldInsert.Aggregatetype = (int)AG.Aggregate;
                            fieldInsert.FieldType = (int)PivotGridFieldZoneType.Aggregate;
                            fieldInsert.Sequence = Ex.ZoneIndex;
                            unitOfWork.ReportViewField.Insert(fieldInsert);
                            break;

                            //case PivotGridFieldZoneType.Filter:
                            //    fieldInsert.FieldName = Ex.DataField;
                            //    fieldInsert.FieldCaption = Ex.Caption;
                            //    // Enum PivotGridFieldZoneType
                            //    fieldInsert.FieldType = (int)PivotGridFieldZoneType.Filter;
                            //    fieldInsert.Sequence = Ex.ZoneIndex;

                            //    Helper.InsertFields(fieldInsert);
                            //    break;
                    }
                }
            }
            unitOfWork.Save();
            return reportInsert.Id;
        }

        //same as the above one with different Argument Signature - Only for Incident Navigator - Public CheckBox
        public static int SaveReportViewDetails(int cid, int uid, bool defaultChekbox, string ViewNameText, PivotGridFieldsCollection GridFields, int reportforrrr)
        //public static void SaveReportViewDetails(int cid, int uid, bool defaultChekbox, string ViewNameText, PivotGridFieldsCollection GridFields, int reportforrrr)
        {

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            // ReportFilterDetailModel cv = new ReportFilterDetailModel();
            ReportView reportInsert = new ReportView();
            reportInsert.UserId = uid;
            reportInsert.ClientId = cid;
            if (defaultChekbox == true)
            {
                resetIsDefault(cid, uid, reportforrrr);
                reportInsert.IsDefault = true;
            }
            reportInsert.ReportId = reportforrrr;
            reportInsert.ViewName = ViewNameText == null ? "" : ViewNameText.Trim();

            var Isthere = unitOfWork.ReportView.Get(g => g.ViewName == reportInsert.ViewName && g.ReportId == reportforrrr && g.UserId == uid && g.ClientId == cid).FirstOrDefault();

            if (defaultChekbox == true)
            {
                resetIsDefault(cid, uid, reportforrrr);
            }
            if (Isthere == null)
            {
                unitOfWork.ReportView.Insert(reportInsert);
                unitOfWork.Save();
            }
            else
            {
                reportInsert.Id = Isthere.Id;
                if (defaultChekbox == true)
                {
                    Isthere.IsDefault = true;
                }
                unitOfWork.ReportView.Update(Isthere);
                unitOfWork.Save();
                //Delete all the Existing Fields 
                DeleteFields(Isthere.Id, unitOfWork);

            }
            foreach (var Ex in GridFields)
            {
                ReportViewField fieldInsert = new ReportViewField();
                fieldInsert.ReportViewId = reportInsert.Id;
                if (Ex.IsHidden == false)
                {
                    switch (Ex.ZoneType)
                    {
                        case PivotGridFieldZoneType.Row:
                            fieldInsert.FieldName = Ex.DataField;
                            fieldInsert.FieldCaption = Ex.Caption;
                            fieldInsert.SortOrder = (int)Ex.SortOrder;
                            // Enum PivotGridFieldZoneType
                            fieldInsert.FieldType = (int)PivotGridFieldZoneType.Row;
                            fieldInsert.Sequence = Ex.ZoneIndex;
                            unitOfWork.ReportViewField.Insert(fieldInsert);
                            break;

                        case PivotGridFieldZoneType.Column:
                            fieldInsert.FieldName = Ex.DataField;
                            fieldInsert.FieldCaption = Ex.Caption;
                            fieldInsert.SortOrder = (int)Ex.SortOrder;
                            // Enum PivotGridFieldZoneType
                            fieldInsert.FieldType = (int)PivotGridFieldZoneType.Column;
                            fieldInsert.Sequence = Ex.ZoneIndex;
                            unitOfWork.ReportViewField.Insert(fieldInsert);
                            break;

                        case PivotGridFieldZoneType.Aggregate:
                            PivotGridAggregateField AG = new PivotGridAggregateField();
                            AG = (PivotGridAggregateField)Ex;
                            fieldInsert.FieldName = Ex.DataField;
                            fieldInsert.FieldCaption = Ex.Caption;
                            fieldInsert.SortOrder = (int)Ex.SortOrder;
                            // Enum PivotGridFieldZoneType
                            fieldInsert.Aggregatetype = (int)AG.Aggregate;
                            fieldInsert.FieldType = (int)PivotGridFieldZoneType.Aggregate;
                            fieldInsert.Sequence = Ex.ZoneIndex;
                            unitOfWork.ReportViewField.Insert(fieldInsert);
                            break;

                            //case PivotGridFieldZoneType.Filter:
                            //    fieldInsert.FieldName = Ex.DataField;
                            //    fieldInsert.FieldCaption = Ex.Caption;
                            //    // Enum PivotGridFieldZoneType
                            //    fieldInsert.FieldType = (int)PivotGridFieldZoneType.Filter;
                            //    fieldInsert.Sequence = Ex.ZoneIndex;

                            //    Helper.InsertFields(fieldInsert);
                            //    break;
                    }
                }
            }
            unitOfWork.Save();
            return reportInsert.Id;
        } 
        public static void setRadPivot(int ViewId, ref RadPivotGrid RadPivotGrid1)
        {
            if (ViewId != 0)
            {
                var Fieldslist = Helper.GetFieldsList(ViewId);
                var RowList = Fieldslist.Where(g => g.FieldType == (int)PivotGridFieldZoneType.Row).OrderBy(g => g.Sequence);
                var ColumnList = Fieldslist.Where(g => g.FieldType == (int)PivotGridFieldZoneType.Column).OrderBy(g => g.Sequence);
                var AggList = Fieldslist.Where(g => g.FieldType == (int)PivotGridFieldZoneType.Aggregate).OrderBy(g => g.Sequence);
                var FiltersList = Fieldslist.Where(g => g.FieldType == (int)PivotGridFieldZoneType.Filter).OrderBy(g => g.Sequence);
                //Columns
                //RadPivotGrid1.Fields.Clear();


                List<string> collection = new List<string>();//= RadPivotGrid1.Fields;
                foreach (var t in RadPivotGrid1.Fields)
                {
                    if (t.ZoneType == PivotGridFieldZoneType.Row || t.ZoneType == PivotGridFieldZoneType.Column || t.ZoneType == PivotGridFieldZoneType.Aggregate)
                    {
                        collection.Add(t.UniqueName);
                    }
                }
                foreach (var t in collection)
                {
                    var tuu = RadPivotGrid1.Fields[t];
                    RadPivotGrid1.Fields.Remove(tuu);
                }
                
                //foreach (var fil in FiltersList)
                //{
                //    PivotGridReportFilterField rff = new PivotGridReportFilterField();
                //    rff.DataField = fil.FieldName;
                //    rff.ZoneIndex = fil.Sequence;
                //    if (fil.FieldCaption != null)
                //    {
                //        rff.Caption = fil.FieldCaption;
                //    }
                //    RadPivotGrid1.Fields.Add(rff);
                //}
                foreach (var agg in AggList)
                {
                    PivotGridAggregateField rffr = new PivotGridAggregateField();
                    rffr.DataField = agg.FieldName;
                    if (agg.FieldCaption != null)
                    {
                        rffr.Caption = agg.FieldCaption;
                    }
                    if (agg.SortOrder != null)
                    {
                        rffr.SortOrder = (PivotGridSortOrder)agg.SortOrder;
                    }
                    if(agg.Aggregatetype != null)
                    {
                        rffr.Aggregate = (PivotGridAggregate)agg.Aggregatetype;
                    }
                    else
                    {
                        rffr.Aggregate = PivotGridAggregate.Count;
                    }
                    //rffr.ZoneIndex = agg.Sequence;
                    RadPivotGrid1.Fields.Add(rffr);
                }
                foreach (var col in ColumnList)
                {
                    PivotGridColumnField rff = new PivotGridColumnField();
                    if (col.FieldCaption != null)
                    {
                        rff.Caption = col.FieldCaption;
                    }
                    if (col.SortOrder != null)
                    {
                        rff.SortOrder = (PivotGridSortOrder)col.SortOrder;
                    }
                    //rff.ZoneIndex = col.Sequence;
                    rff.DataField = col.FieldName;
                    RadPivotGrid1.Fields.Add(rff);
                }
                foreach (var row in RowList)
                {
                    PivotGridRowField rff = new PivotGridRowField();
                    if (row.FieldCaption != null)
                    {
                        rff.Caption = row.FieldCaption;
                    }
                    if (row.SortOrder != null)
                    {
                        rff.SortOrder = (PivotGridSortOrder)row.SortOrder;
                    }
                    //rff.ZoneIndex = row.Sequence;
                    rff.DataField = row.FieldName;
                    RadPivotGrid1.Fields.Add(rff);
                }
                RadPivotGrid1.ColumnGroupsDefaultExpanded = true;
                RadPivotGrid1.RowGroupsDefaultExpanded = true;
                RadPivotGrid1.ClientSettings.EnableFieldsDragDrop = false;
                RadPivotGrid1.ShowFilterHeaderZone = false;
                RadPivotGrid1.AllowFiltering = false;
                
                
                
            }

        }
        //params = (int ViewId, ref RadPivotGrid RadPivotGrid1)
        public static void saveFilterDetails(int Viewid, ref RadPivotGrid RadPivotGrid1)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var checkExistingfilters = unitOfWork.ReportViewFilter.Get().Where(g => g.ViewId == Viewid).Any();
            if (checkExistingfilters)
            {
                DeleteViewFilterDetails(Viewid);
            }
            List<PivotGridFilter> FilterList = (List<PivotGridFilter>)RadPivotGrid1.Filters.SaveViewState();
            foreach (var Fl in FilterList)
            {
                var Fieldname = Fl.FieldName;
                switch (Fl.GetType().ToString())
                {
                    case "Telerik.Web.UI.PivotGridLabelGroupFilter":
                        PivotGridLabelGroupFilter gp = (PivotGridLabelGroupFilter)Fl;
                        var GpFilter = gp.Condition;
                        switch (GpFilter.GetType().ToString())
                        {
                            case "Telerik.Web.UI.PivotGridSetCondition":
                                PivotGridSetCondition sc = (PivotGridSetCondition)GpFilter;
                                //Enum - SetComparision
                                //var comparision = (int)sc.Comparison;
                                //var list = sc.Items.ToString();
                                ReportViewFilter rvf = new ReportViewFilter();
                                rvf.ViewId = Viewid;
                                rvf.FieldName = Fieldname;
                                rvf.FilterType = (int)PivotGridGroupFilter.PivotGridLabelGroupFilter;
                                rvf.FilterCondition = (int)PivotGridFilterConditionType.PivotGridSetCondition;
                                rvf.FilterConditionValue=(int)sc.Comparison;
                                rvf.Param1 = "#";
                                foreach(var s in sc.Items)
                                {
                                    rvf.Param1 = rvf.Param1 + "," + s.ToString();
                                }
                                unitOfWork.ReportViewFilter.Insert(rvf);

                                break;
                            case "Telerik.Web.UI.PivotGridTextComparisonCondition":
                                PivotGridTextComparisonCondition tcc = (PivotGridTextComparisonCondition)GpFilter;
                                //Enum - TextComparision
                                //var textComparision = (int)tcc.Comparison;
                                //var textPattern = tcc.Pattern;
                                ReportViewFilter rvf1 = new ReportViewFilter();
                                rvf1.ViewId = Viewid;
                                rvf1.FieldName = Fieldname;
                                rvf1.FilterType = (int)PivotGridGroupFilter.PivotGridLabelGroupFilter;
                                rvf1.FilterCondition = (int)PivotGridFilterConditionType.PivotGridTextComparisonCondition;
                                rvf1.FilterConditionValue = (int)tcc.Comparison;
                                rvf1.Param1 = tcc.Pattern;
                                unitOfWork.ReportViewFilter.Insert(rvf1);
                                break;

                            case "Telerik.Web.UI.PivotGridIntervalCondition":
                                PivotGridIntervalCondition ic = (PivotGridIntervalCondition)GpFilter;
                                //Enum - IntervalComparision
                                //var intervalCond = (int)ic.Condition;
                                //var inFrom = ic.From.ToString();
                                //var inTo = ic.To.ToString();
                                ReportViewFilter rvf2 = new ReportViewFilter();
                                rvf2.ViewId = Viewid;
                                rvf2.FieldName = Fieldname;
                                rvf2.FilterType = (int)PivotGridGroupFilter.PivotGridLabelGroupFilter;
                                rvf2.FilterCondition = (int)PivotGridFilterConditionType.PivotGridIntervalCondition;
                                rvf2.FilterConditionValue = (int)ic.Condition;
                                rvf2.Param1 = ic.From.ToString();
                                rvf2.Param2 = ic.To.ToString();
                                unitOfWork.ReportViewFilter.Insert(rvf2);
                                break;

                            case "Telerik.Web.UI.PivotGridComparisonCondition":
                                PivotGridComparisonCondition cc = (PivotGridComparisonCondition)GpFilter;
                                //Enum - Comparision
                                //var compareCondition = (int)cc.Condition;
                                //var ccThan = cc.Than.ToString();
                                ReportViewFilter rvf3 = new ReportViewFilter();
                                rvf3.ViewId = Viewid;
                                rvf3.FieldName = Fieldname;
                                rvf3.FilterType = (int)PivotGridGroupFilter.PivotGridLabelGroupFilter;
                                rvf3.FilterCondition = (int)PivotGridFilterConditionType.PivotGridComparisonCondition;
                                rvf3.FilterConditionValue = (int)cc.Condition;
                                rvf3.Param1 = cc.Than.ToString();
                                unitOfWork.ReportViewFilter.Insert(rvf3);
                                break;
                                
                        }
                        
                        break;
                    //Filter Zone Fields[Filters]
                    case "Telerik.Web.UI.PivotGridReportFilter":
                        PivotGridReportFilter gr = (PivotGridReportFilter)Fl;
                        var GrFilter = gr.Condition;
                        switch (GrFilter.GetType().ToString())
                        {
                            case "Telerik.Web.UI.PivotGridSetCondition":
                                PivotGridSetCondition sc = (PivotGridSetCondition)GrFilter;
                                //Enum - SetComparision
                                //var comparision = (int)sc.Comparison;
                                //var list = sc.Items.ToString();
                                ReportViewFilter rvf = new ReportViewFilter();
                                rvf.ViewId = Viewid;
                                rvf.FieldName = Fieldname;
                                rvf.FilterType = (int)PivotGridGroupFilter.PivotGridLabelGroupFilter;
                                rvf.FilterCondition = (int)PivotGridFilterConditionType.PivotGridSetCondition;
                                rvf.FilterConditionValue = (int)sc.Comparison;
                                rvf.Param1 = "#";
                                foreach (var s in sc.Items)
                                {
                                    rvf.Param1 = rvf.Param1 + "," + s.ToString();
                                }
                                unitOfWork.ReportViewFilter.Insert(rvf);

                                break;
                        }
                        break;

                    case "Telerik.Web.UI.PivotGridValueGroupFilter":
                        PivotGridValueGroupFilter gv = (PivotGridValueGroupFilter)Fl;
                        var GvFilter = gv.Condition;
                        switch (GvFilter.GetType().ToString())
                        {
                            case "Telerik.Web.UI.PivotGridSetCondition":
                                PivotGridSetCondition sc = (PivotGridSetCondition)GvFilter;
                                //Enum - SetComparision
                                //var comparision = (int)sc.Comparison;
                                //var list = sc.Items.ToString();
                                ReportViewFilter rvf4 = new ReportViewFilter();
                                rvf4.ViewId = Viewid;
                                rvf4.FieldName = Fieldname;
                                rvf4.FilterType = (int)PivotGridGroupFilter.PivotGridValueGroupFilter;
                                rvf4.FilterCondition = (int)PivotGridFilterConditionType.PivotGridSetCondition;
                                rvf4.FilterConditionValue = (int)sc.Comparison;
                                rvf4.Param1 = "#";
                                foreach (var s in sc.Items)
                                {
                                    rvf4.Param1 = rvf4.Param1 + "," + s.ToString();
                                }
                                unitOfWork.ReportViewFilter.Insert(rvf4);
                                break;

                            case "Telerik.Web.UI.PivotGridTextComparisonCondition":
                                PivotGridTextComparisonCondition tcc = (PivotGridTextComparisonCondition)GvFilter;
                                //Enum - TextComparision
                                //var textComparision = (int)tcc.Comparison;
                                //var textPattern = tcc.Pattern;
                                ReportViewFilter rvf5 = new ReportViewFilter();
                                rvf5.ViewId = Viewid;
                                rvf5.FieldName = Fieldname;
                                rvf5.FilterType = (int)PivotGridGroupFilter.PivotGridValueGroupFilter;
                                rvf5.FilterCondition = (int)PivotGridFilterConditionType.PivotGridTextComparisonCondition;
                                rvf5.FilterConditionValue = (int)tcc.Comparison;
                                rvf5.Param1 = tcc.Pattern;
                                unitOfWork.ReportViewFilter.Insert(rvf5);
                                break;

                            case "Telerik.Web.UI.PivotGridIntervalCondition":
                                PivotGridIntervalCondition ic = (PivotGridIntervalCondition)GvFilter;
                                //Enum - IntervalComparision
                                //var intervalCond = (int)ic.Condition;
                                //var inFrom = ic.From.ToString();
                                //var inTo = ic.To.ToString();
                                ReportViewFilter rvf6 = new ReportViewFilter();
                                rvf6.ViewId = Viewid;
                                rvf6.FieldName = Fieldname;
                                rvf6.FilterType = (int)PivotGridGroupFilter.PivotGridValueGroupFilter;
                                rvf6.FilterCondition = (int)PivotGridFilterConditionType.PivotGridIntervalCondition;
                                rvf6.FilterConditionValue = (int)ic.Condition;
                                rvf6.Param1 = ic.From.ToString();
                                rvf6.Param2 = ic.To.ToString();
                                unitOfWork.ReportViewFilter.Insert(rvf6);
                                break;

                            case "Telerik.Web.UI.PivotGridComparisonCondition":
                                PivotGridComparisonCondition cc = (PivotGridComparisonCondition)GvFilter;
                                //Enum - Comparision
                                //var compareCondition = (int)cc.Condition;
                                //var ccThan = cc.Than.ToString();
                                ReportViewFilter rvf7 = new ReportViewFilter();
                                rvf7.ViewId = Viewid;
                                rvf7.FieldName = Fieldname;
                                rvf7.FilterType = (int)PivotGridGroupFilter.PivotGridValueGroupFilter;
                                rvf7.FilterCondition = (int)PivotGridFilterConditionType.PivotGridTextComparisonCondition;
                                rvf7.FilterConditionValue = (int)cc.Condition;
                                rvf7.Param1 = cc.Than.ToString();
                                unitOfWork.ReportViewFilter.Insert(rvf7);
                                break;
                        }
                        break;

                }
            }
            unitOfWork.Save();
        }
        //helper to setFiltersTo
        public static void settingFiltersToPivot(int viewid , ref RadPivotGrid RadPivotGrid1)
        {
            List<PivotGridFilter> FilterList = new List<PivotGridFilter>();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            RadPivotGrid1.ClearAllFilters();

            var getFiltersList = unitOfWork.ReportViewFilter.Get().Where(g => g.ViewId == viewid);
            foreach(var filt in getFiltersList)
            {
                switch ((PivotGridGroupFilter)filt.FilterType)
                {
                    case PivotGridGroupFilter.PivotGridLabelGroupFilter:
                        PivotGridLabelGroupFilter gp = new PivotGridLabelGroupFilter();
                        gp.FieldName = filt.FieldName;
                        switch ((PivotGridFilterConditionType)filt.FilterCondition)
                        {
                            case PivotGridFilterConditionType.PivotGridComparisonCondition:
                                switch ((Comparison)filt.FilterConditionValue)
                                {
                                    case Comparison.DoesNotEqual:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.DoesNotEqual,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case Comparison.Equals:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.Equals,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case Comparison.IsGreaterThan:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.IsGreaterThan,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case Comparison.IsGreaterThanOrEqualTo:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.IsGreaterThanOrEqualTo,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case Comparison.IsLessThan:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.IsLessThan,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case Comparison.IsLessThanOrEqualTo:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.IsLessThanOrEqualTo,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    
                                }
                                 
                                break;

                            case PivotGridFilterConditionType.PivotGridIntervalCondition:
                                switch ((IntervalComparison)filt.FilterConditionValue)
                                {
                                    case IntervalComparison.IsBetween:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.IsBetween,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1, filt.Param2);
                                        }
                                        catch { }
                                        break;
                                    case IntervalComparison.IsNotBetween:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.IsNotBetween,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1, filt.Param2);
                                        }
                                        catch { }
                                        break;
                                }
                                break;

                            case PivotGridFilterConditionType.PivotGridSetCondition:
                                var list = filt.Param1.Split(',').ToList();
                                var cleanList = new List<string>();
                                foreach(var strings  in list)
                                {
                                    if (strings != "#")// && strings != "0")
                                    {
                                        cleanList.Add(strings);
                                    }

                                }
                                    switch ((SetComparison)filt.FilterConditionValue)
                                {
                                    case SetComparison.Includes:
                                        try
                                        {
                                            RadPivotGrid1.SetFilterIncludes(filt.FieldName, cleanList);
                                        }
                                        catch { }
                                        break;
                                    case SetComparison.DoesNotInclude:
                                        try
                                        {
                                            RadPivotGrid1.SetFilterExcludes(filt.FieldName, cleanList);
                                        }
                                        catch { }
                                        break;
                                }
                                
                                break;

                            case PivotGridFilterConditionType.PivotGridTextComparisonCondition:
                                switch ((TextComparison)filt.FilterConditionValue)
                                {
                                    case TextComparison.BeginsWith:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.BeginsWith,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case TextComparison.Contains:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.Contains,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case TextComparison.DoesNotBeginWith:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.DoesNotBeginWith,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case TextComparison.DoesNotContain:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.DoesNotContain,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case TextComparison.DoesNotEndWith:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.DoesNotEndWith,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case TextComparison.EndsWith:
                                        try
                                        {
                                            RadPivotGrid1.FilterByLabel(PivotGridFilterFunction.EndsWith,
                                                RadPivotGrid1.Fields[filt.FieldName], filt.Param1);
                                        }
                                        catch { }
                                        break;
                                }
                                break;
                        }
                        break;
                    
                    case PivotGridGroupFilter.PivotGridReportFilter:
                        PivotGridReportFilter gr = new PivotGridReportFilter();
                        gr.FieldName = filt.FieldName;
                        switch ((PivotGridFilterConditionType)filt.FilterCondition)
                        {
                            case PivotGridFilterConditionType.PivotGridSetCondition:
                                var list = filt.Param1.Split(',').ToList();
                                var cleanList = new List<string>();
                                foreach (var strings in list)
                                {
                                    if (strings != "#" )//&& strings != "0")
                                    {
                                        cleanList.Add(strings);
                                    }
                                }
                                switch ((SetComparison)filt.FilterConditionValue)
                                {
                                    case SetComparison.Includes:
                                        try
                                        {
                                            RadPivotGrid1.SetFilterIncludes(filt.FieldName, cleanList);
                                        }
                                        catch { }
                                        break;
                                    case SetComparison.DoesNotInclude:
                                        try
                                        {
                                            RadPivotGrid1.SetFilterExcludes(filt.FieldName, cleanList);
                                        }
                                        catch { }
                                        break;
                                }

                                break;
                        }
                        break;
                    //-------------//
                    case PivotGridGroupFilter.PivotGridValueGroupFilter:
                        PivotGridValueGroupFilter gv = new PivotGridValueGroupFilter();

                        switch ((PivotGridFilterConditionType)filt.FilterCondition)
                        {
                            case PivotGridFilterConditionType.PivotGridComparisonCondition:
                                switch ((Comparison)filt.FilterConditionValue)
                                {
                                    case Comparison.DoesNotEqual:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.DoesNotEqual,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case Comparison.Equals:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.Equals,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case Comparison.IsGreaterThan:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.IsGreaterThan,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case Comparison.IsGreaterThanOrEqualTo:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.IsGreaterThanOrEqualTo,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case Comparison.IsLessThan:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.IsLessThan,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case Comparison.IsLessThanOrEqualTo:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.IsLessThanOrEqualTo,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1);
                                        }
                                        catch { }
                                        break;

                                }
                                break;

                            case PivotGridFilterConditionType.PivotGridIntervalCondition:
                                switch ((IntervalComparison)filt.FilterConditionValue)
                                {
                                    case IntervalComparison.IsBetween:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.IsBetween,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1, filt.Param2);
                                        }
                                        catch { }
                                        break;
                                    case IntervalComparison.IsNotBetween:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.IsNotBetween,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1, filt.Param2);
                                        }
                                        catch { }
                                        break;
                                }
                                break;

                            case PivotGridFilterConditionType.PivotGridSetCondition:
                                var list = filt.Param1.Split(',').ToList();
                                var cleanList = new List<string>();
                                foreach (var strings in list)
                                {
                                    if (strings != "#" )//&& strings != "0")
                                    {
                                        cleanList.Add(strings);
                                    }
                                }
                                switch ((SetComparison)filt.FilterConditionValue)
                                {
                                    case SetComparison.Includes:
                                        try
                                        {
                                            RadPivotGrid1.SetFilterIncludes(filt.FieldName, cleanList);
                                        }
                                        catch { }
                                        break;
                                    case SetComparison.DoesNotInclude:
                                        try
                                        {
                                            RadPivotGrid1.SetFilterExcludes(filt.FieldName, cleanList);
                                        }
                                        catch { }
                                        break;
                                }
                                break;

                            case PivotGridFilterConditionType.PivotGridTextComparisonCondition:
                                switch ((TextComparison)filt.FilterConditionValue)
                                {
                                    case TextComparison.BeginsWith:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.BeginsWith,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case TextComparison.Contains:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.Contains,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case TextComparison.DoesNotBeginWith:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.DoesNotBeginWith,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case TextComparison.DoesNotContain:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.DoesNotContain,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case TextComparison.DoesNotEndWith:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.DoesNotEndWith,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1);
                                        }
                                        catch { }
                                        break;
                                    case TextComparison.EndsWith:
                                        try
                                        {
                                            RadPivotGrid1.FilterByValue(PivotGridFilterFunction.EndsWith,
                                                RadPivotGrid1.Fields[filt.FieldName],
                                                RadPivotGrid1.Fields["Id"] as PivotGridAggregateField, filt.Param1);
                                        }
                                        catch { }
                                        break;
                                }
                                break;
                        }
                        

                        break;
                }
            }
            
        }


        //New Landing Page, ClientDetails
        public static ServiceDeliveryStatsModel ClientDeliveryStats(int cid)
        {
            ServiceDeliveryStatsModel coldet = new ServiceDeliveryStatsModel();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { cid };
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("Landing_DeliveryStats", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    coldet.TotalPortfolios =dr["TotalPortfolios"].ToString();
                    coldet.TotalApps = dr["TotalApplications"].ToString();
                    coldet.HighestTickForApp =dr["Configitemname"].ToString();
                    //coldet.OverallTCO = dr["OverallTCO"].ToString();
                    
                    //-- SLA --//
                    //coldet.Mon_TotalSLA = dr[""].ToString();
                    //coldet.Quar_TotalSLA = dr[""].ToString();
                    //coldet.Year_TotalSLA = dr[""].ToString();

                    //coldet.Mon_MeetSLA = dr[""].ToString();
                    //coldet.Quar_MeetSLA = dr[""].ToString();
                    //coldet.Year_MeetSLA = dr[""].ToString();

                    //coldet.Mon_ExSLA = dr[""].ToString();
                    //coldet.Quar_ExSLA = dr[""].ToString();
                    //coldet.Year_ExSLA= dr[""].ToString();

                    //coldet.Mon_BrSLA = dr[""].ToString();
                    //coldet.Quar_BrSLA = dr[""].ToString();
                    //coldet.Year_BrSLA = dr[""].ToString();

                    //-- Incidents--//
                    coldet.IN_L1Tickets= dr["Incident_L1_Tickets"].ToString();
                    coldet.IN_L1L4Tickets= dr["Incident_L2L4_Tickets"].ToString();
                    //coldet.IN_AvgTicketsPerMonth = dr[""].ToString();
                    coldet.IN_AvgCostPerTicket = dr["EffortPer_Incidents"].ToString();
                    coldet.IN_AvgResTime= dr["AvgResolutionTime_Incidents"].ToString();
                    coldet.IN_Clusters = dr["TotalClusters"].ToString();

                    //-- ServiceRequest --//
                    coldet.SR_L1Tickets = dr["ServiceReq_L1_Tickets"].ToString();
                    coldet.SR_L1L4Tickets = dr["ServiceReq_L2L4_Tickets"].ToString();
                    //coldet.SR_AvgTicketsPerMonth = dr[""].ToString();
                    coldet.SR_AvgCostPerTicket = dr["EffortPer_UserSupportSR"].ToString();
                    coldet.SR_AvgResTime = dr["AvgResolutionTime_SR"].ToString();
                    //coldet.SR_Clusters = dr[""].ToString();

                    //-- Problems --//
                    coldet.Prob_L1Tickets = dr["Problems_L1_Tickets"].ToString();
                    coldet.Prob_L1L4Tickets = dr["Problems_L2L4_Tickets"].ToString();
                    //coldet.Prob_AvgTicketsPerMonth = dr[""].ToString();
                    coldet.Prob_AvgCostPerTicket = dr["EffortPer_Problems"].ToString();
                    coldet.Prob_AvgResTime = dr["AvgResolutionTime_Problem"].ToString();
                    //coldet.Prob_Clusters = dr[""].ToString();

                    //-- ChangeManagement : Enhancements --//
                    coldet.CM_L1Tickets = dr["Enhance_L1_Tickets"].ToString();
                    coldet.CM_L1L4Tickets = dr["Enhance_L2L4_Tickets"].ToString();
                    //coldet.CM_AvgTicketsPerMonth = dr[""].ToString();
                    coldet.CM_AvgCostPerTicket = dr["EffortPer_Enhancements"].ToString();
                    coldet.CM_AvgResTime = dr["AvgResolutionTime_CR"].ToString();
                    //coldet.CM_Clusters = dr[""].ToString();

                }


                //- Idea Details
                var AllIdeas = unitOfWork.IMPostIdea.Get().Where(g => g.ClientId == cid).Count();
                var ApproIdeas = unitOfWork.IMPostIdea.Get().Where(g => g.StatusId == (int)IMStatus.Approved && g.ClientId == cid).Count();
                var CSIIdeas = unitOfWork.IMPostIdea.Get().Where(g => g.StatusId == (int)IMStatus.Implemented && g.ClientId == cid).Count();

                var totalIdeas = unitOfWork.IMPostIdea.Get().Where(g => g.ClientId == cid).Count();

                var thisMonth = unitOfWork.IMPostIdea.Get().Where(g => g.ClientId == cid && g.CreatedDate.Value.Month == DateTime.Now.Month).Count();
                var prevMonth = unitOfWork.IMPostIdea.Get().Where(g => g.ClientId == cid && g.CreatedDate.Value.Month == (DateTime.Now.Month - 1)).Count();

                if (thisMonth > prevMonth)
                {
                    coldet.IdeaTrend = "UP";
                }
                else if(thisMonth<prevMonth)
                {
                    coldet.IdeaTrend = "DOWN";
                }
                else
                {
                    coldet.IdeaTrend = "NONE";
                }
                
                coldet.TotalIdeas = AllIdeas.ToString();
                coldet.IdeaApproved = ApproIdeas.ToString();
                if (ApproIdeas == 0 || CSIIdeas == 0)
                {
                    coldet.CSI_index = "0.00";
                }
                else
                {
                    coldet.CSI_index = (CSIIdeas / ApproIdeas).ToString();
                }
            }
            return coldet;
        }


        /// <summary>
        /// //send email
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>

        public static void SendEMail(string emailid, string subject, string body, bool? isNucleus = false)
        {



            MailMessage mail = new MailMessage();
            String EmailFrom = WebConfigurationManager.AppSettings["EmailFrom"];
            String EmailFromDisplayName = WebConfigurationManager.AppSettings["EmailFromDisplayName"];



            if ((bool)isNucleus)
            {
                Log.Info("Email attempt starting:");
                EmailFrom = WebConfigurationManager.AppSettings["EmailNucleusFrom"];
                EmailFromDisplayName = WebConfigurationManager.AppSettings["EmailNucleusFromDisplayName"];
                Log.Info(EmailFrom);
                Log.Info(EmailFromDisplayName);
            }
            mail.From = new MailAddress(EmailFrom, EmailFromDisplayName);
            // mail.To
            mail.To.Add(emailid); // Replace with a "To" address. If your account is still in the



            String Bcc = WebConfigurationManager.AppSettings["EmailBcc"];
            if (!string.IsNullOrEmpty(Bcc))
            {
                mail.Bcc.Add(Bcc);
            }




            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;



            String HOST = WebConfigurationManager.AppSettings["smtpHost"];



            int PORT = Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);




            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {



                // Send the email.
                try
                {



                    String pwd = WebConfigurationManager.AppSettings["smtpUserPwd"];
                    String userid = WebConfigurationManager.AppSettings["smtpUserId"];
                    String TLS = WebConfigurationManager.AppSettings["smtpTLS"];
                    if (!string.IsNullOrEmpty(TLS) && TLS == "Yes")
                    {
                        client.EnableSsl = true;
                    }
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        NetworkCredential Credentials = new NetworkCredential(userid, pwd);
                        client.Credentials = Credentials;
                    }
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    Log.Error("error sending mail from web server", ex);
                    throw new Exception("error sending mail from web server", ex);
                }
            }



        }
        public static void SendEMail_A3MM(string emailid, string subject, string body, bool? isNucleus = false)
        {



            MailMessage mail = new MailMessage();
            String EmailFrom = WebConfigurationManager.AppSettings["EmailFrom"];
            String EmailFromDisplayName = WebConfigurationManager.AppSettings["A3MMEmailFromDisplayName"];



            if ((bool)isNucleus)
            {
                Log.Info("Email attempt starting:");
                EmailFrom = WebConfigurationManager.AppSettings["EmailNucleusFrom"];
                EmailFromDisplayName = WebConfigurationManager.AppSettings["EmailNucleusFromDisplayName"];
                Log.Info(EmailFrom);
                Log.Info(EmailFromDisplayName);
            }
            mail.From = new MailAddress(EmailFrom, EmailFromDisplayName);
            // mail.To
            mail.To.Add(emailid); // Replace with a "To" address. If your account is still in the



            String Bcc = WebConfigurationManager.AppSettings["EmailBcc"];
            if (!string.IsNullOrEmpty(Bcc))
            {
                mail.Bcc.Add(Bcc);
            }




            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;



            String HOST = WebConfigurationManager.AppSettings["smtpHost"];



            int PORT = Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);




            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {



                // Send the email.
                try
                {



                    String pwd = WebConfigurationManager.AppSettings["smtpUserPwd"];
                    String userid = WebConfigurationManager.AppSettings["smtpUserId"];
                    String TLS = WebConfigurationManager.AppSettings["smtpTLS"];
                    if (!string.IsNullOrEmpty(TLS) && TLS == "Yes")
                    {
                        client.EnableSsl = true;
                    }
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        NetworkCredential Credentials = new NetworkCredential(userid, pwd);
                        client.Credentials = Credentials;
                    }
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    Log.Error("error sending mail from web server", ex);
                    throw new Exception("error sending mail from web server", ex);
                }
            }



        }
        public static void SendEMailwithCC(string emailid, string ccEmailid,string subject, string body, bool? isNucleus = false)
        {
            MailMessage mail = new MailMessage();
            String EmailFrom = WebConfigurationManager.AppSettings["EmailFrom"];
            String EmailFromDisplayName = WebConfigurationManager.AppSettings["EmailFromDisplayName"];

            if ((bool)isNucleus)
            {
                Log.Info("Email attempt starting:");
                EmailFrom = WebConfigurationManager.AppSettings["EmailNucleusFrom"];
                EmailFromDisplayName = WebConfigurationManager.AppSettings["EmailNucleusFromDisplayName"];
                Log.Info(EmailFrom);
                Log.Info(EmailFromDisplayName);
            }
            mail.From = new MailAddress(EmailFrom, EmailFromDisplayName);
            mail.To.Add(emailid); // Replace with a "To" address. If your account is still in the
            mail.CC.Add(ccEmailid);

            String Bcc = WebConfigurationManager.AppSettings["EmailBcc"];
            if (!string.IsNullOrEmpty(Bcc))
            {
                mail.Bcc.Add(Bcc);
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            String HOST = WebConfigurationManager.AppSettings["smtpHost"];

            int PORT = Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);

            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Send the email.
                try
                {
                    String pwd = WebConfigurationManager.AppSettings["smtpUserPwd"];
                    String userid = WebConfigurationManager.AppSettings["smtpUserId"];
                    String TLS = WebConfigurationManager.AppSettings["smtpTLS"];
                    if (!string.IsNullOrEmpty(TLS) && TLS == "Yes")
                    {
                        client.EnableSsl = true;
                    }
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        NetworkCredential Credentials = new NetworkCredential(userid, pwd);
                        client.Credentials = Credentials;
                    }
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    Log.Error("error sending mail from web server", ex);
                    throw new Exception("error sending mail from web server", ex);
                }
            }

        }

        public static void SendEMailwithMultiCC(string emailid, string[] ccEmailid, string subject, string body, bool? isNucleus = false)
        {
            MailMessage mail = new MailMessage();
            String EmailFrom = WebConfigurationManager.AppSettings["EmailFrom"];
            String EmailFromDisplayName = WebConfigurationManager.AppSettings["EmailFromDisplayName"];

            if ((bool)isNucleus)
            {
                Log.Info("Email attempt starting:");
                EmailFrom = WebConfigurationManager.AppSettings["EmailNucleusFrom"];
                EmailFromDisplayName = WebConfigurationManager.AppSettings["EmailNucleusFromDisplayName"];
                Log.Info(EmailFrom);
                Log.Info(EmailFromDisplayName);
            }
            mail.From = new MailAddress(EmailFrom, EmailFromDisplayName);
            mail.To.Add(emailid); // Replace with a "To" address. If your account is still in the
            foreach(var cc in ccEmailid) {
                mail.CC.Add(cc);
            }
            

            String Bcc = WebConfigurationManager.AppSettings["EmailBcc"];
            if (!string.IsNullOrEmpty(Bcc))
            {
                mail.Bcc.Add(Bcc);
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            String HOST = WebConfigurationManager.AppSettings["smtpHost"];

            int PORT = Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);

            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Send the email.
                try
                {
                    String pwd = WebConfigurationManager.AppSettings["smtpUserPwd"];
                    String userid = WebConfigurationManager.AppSettings["smtpUserId"];
                    String TLS = WebConfigurationManager.AppSettings["smtpTLS"];
                    if (!string.IsNullOrEmpty(TLS) && TLS == "Yes")
                    {
                        client.EnableSsl = true;
                    }
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        NetworkCredential Credentials = new NetworkCredential(userid, pwd);
                        client.Credentials = Credentials;
                    }
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    Log.Error("error sending mail from web server", ex);
                    throw new Exception("error sending mail from web server", ex);
                }
            }

        }

        public static void SendEMailwithToandCC(string[] emailid, string[] ccEmailid, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            String EmailFrom = WebConfigurationManager.AppSettings["EmailFrom"];
            String EmailFromDisplayName = WebConfigurationManager.AppSettings["EmailFromDisplayName"];

            Log.Info("Subject : "+subject);
            mail.From = new MailAddress(EmailFrom, EmailFromDisplayName);
            foreach (var cc in emailid)
            {
                mail.To.Add(cc);
            }
            foreach (var cc in ccEmailid)
            {
                mail.CC.Add(cc);
            }

            String Bcc = WebConfigurationManager.AppSettings["EmailBcc"];
            if (!string.IsNullOrEmpty(Bcc))
            {
                mail.Bcc.Add(Bcc);
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            String HOST = WebConfigurationManager.AppSettings["smtpHost"];

            int PORT = Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);

            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Send the email.
                try
                {
                    String pwd = WebConfigurationManager.AppSettings["smtpUserPwd"];
                    String userid = WebConfigurationManager.AppSettings["smtpUserId"];
                    String TLS = WebConfigurationManager.AppSettings["smtpTLS"];
                    if (!string.IsNullOrEmpty(TLS) && TLS == "Yes")
                    {
                        client.EnableSsl = true;
                    }
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        NetworkCredential Credentials = new NetworkCredential(userid, pwd);
                        client.Credentials = Credentials;
                    }
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    Log.Error("error sending mail from web server", ex);
                    throw new Exception("error sending mail from web server", ex);
                }
            }

        }


        public static void SendNucleusEMail(string emailid, string subject, string body)
        {

            MailMessage mail = new MailMessage();
            String EmailFrom = WebConfigurationManager.AppSettings["EmailNucleusFrom"];
            String EmailFromDisplayName = WebConfigurationManager.AppSettings["EmailNucleusFromDisplayName"];
            mail.From = new MailAddress(EmailFrom, EmailFromDisplayName);
            // mail.To
            mail.To.Add(emailid); // Replace with a "To" address. If your account is still in the

            String Bcc = WebConfigurationManager.AppSettings["EmailBcc"];
            if (!string.IsNullOrEmpty(Bcc))
            {
                mail.Bcc.Add(Bcc);
            }


            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            String HOST = WebConfigurationManager.AppSettings["smtpHost"];

            int PORT = Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);


            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {

                // Send the email. 
                try
                {

                    String pwd = WebConfigurationManager.AppSettings["smtpUserPwd"];
                    String userid = WebConfigurationManager.AppSettings["smtpUserId"];
                    String TLS = WebConfigurationManager.AppSettings["smtpTLS"];
                    if (!string.IsNullOrEmpty(TLS) && TLS == "Yes")
                    {
                        client.EnableSsl = true;
                    }
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        NetworkCredential Credentials = new NetworkCredential(userid, pwd);
                        client.Credentials = Credentials;
                    }
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    throw new Exception("error sending mail from web server", ex);
                }
            }

        }

        ////////////XL


        public static System.IO.Stream GetXLFileStream(DataSet ds)
        {

            System.IO.Stream stream = new MemoryStream();
            SpreadsheetDocument spreadsheet = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
            //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
            //  to a file, or writing to a MemoryStream.
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
            spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart stylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();
            Stylesheet stylesheet = new Stylesheet();
            stylesPart.Stylesheet = stylesheet;


            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            uint worksheetNumber = 1;
            Sheets sheets = spreadsheet.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
            foreach (DataTable dt in ds.Tables)
            {
                //  For each worksheet you want to create
                string worksheetName = dt.TableName;

                //  Create worksheet part, and add it to the sheets collection in workbook
                WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                Sheet sheet = new Sheet() { Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart), SheetId = worksheetNumber, Name = worksheetName };

                // If you want to define the Column Widths for a Worksheet, you need to do this *before* appending the SheetData  
                // http://social.msdn.microsoft.com/Forums/en-US/oxmlsdk/thread/1d93eca8-2949-4d12-8dd9-15cc24128b10/
                
                DocumentFormat.OpenXml.Spreadsheet.Columns cs = new DocumentFormat.OpenXml.Spreadsheet.Columns();
                Column c = new Column()
                {
                    Min = 1U,
                    Max = 2U,
                    Width = 44.33203125D,
                    CustomWidth = true
                };
                cs.Append(c);
                sheets.Append(cs);
                //Style-end


                sheets.Append(sheet);

                //  Append this worksheet's data to our Workbook, using OpenXmlWriter, to prevent memory problems
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart);

                worksheetNumber++;
            }

            spreadsheet.WorkbookPart.Workbook.Save();

            spreadsheet.Close();


            return stream;
        }

        public static System.IO.Stream GetXLFileStream(DataTable dt)
        {

            System.IO.Stream stream = new MemoryStream();
            SpreadsheetDocument spreadsheet = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
            //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
            //  to a file, or writing to a MemoryStream.
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
            spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart stylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();
            Stylesheet stylesheet = new Stylesheet();
            stylesPart.Stylesheet = stylesheet;


            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            uint worksheetNumber = 1;
            Sheets sheets = spreadsheet.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            string worksheetName = "Sheet1";

                //  Create worksheet part, and add it to the sheets collection in workbook
                WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                Sheet sheet = new Sheet() { Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart), SheetId = worksheetNumber, Name = worksheetName };

                // If you want to define the Column Widths for a Worksheet, you need to do this *before* appending the SheetData  
                // http://social.msdn.microsoft.com/Forums/en-US/oxmlsdk/thread/1d93eca8-2949-4d12-8dd9-15cc24128b10/

                DocumentFormat.OpenXml.Spreadsheet.Columns cs = new DocumentFormat.OpenXml.Spreadsheet.Columns();
                Column c = new Column()
                {
                    Min = 1U,
                    Max = 2U,
                    Width = 44.33203125D,
                    CustomWidth = true
                };
                cs.Append(c);
                sheets.Append(cs);

                sheets.Append(sheet);

                //  Append this worksheet's data to our Workbook, using OpenXmlWriter, to prevent memory problems
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart);


            spreadsheet.WorkbookPart.Workbook.Save();

            spreadsheet.Close();
            
            return stream;
        }

        private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart)
        {
            OpenXmlWriter writer = OpenXmlWriter.Create(worksheetPart, Encoding.UTF8);//.ASCII);
            writer.WriteStartElement(new Worksheet());
            writer.WriteStartElement(new SheetData());

            string cellValue = "";

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.
            int numberOfColumns = dt.Columns.Count;
            bool[] IsNumericColumn = new bool[numberOfColumns];
            bool[] IsDateColumn = new bool[numberOfColumns];

            string[] excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            //
            //  Create the Header row in our Excel Worksheet
            //
            uint rowIndex = 1;

            writer.WriteStartElement(new Row { RowIndex = rowIndex });
            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                DataColumn col = dt.Columns[colInx];
                AppendTextCell(excelColumnNames[colInx] + "1", col.ColumnName, ref writer);
                IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32") || (col.DataType.FullName == "System.Double") || (col.DataType.FullName == "System.Single");
                IsDateColumn[colInx] = (col.DataType.FullName == "System.DateTime");
            }
            writer.WriteEndElement();   //  End of header "Row"

            //
            //  Now, step through each row of data in our DataTable...
            //
            double cellNumericValue = 0;
            foreach (DataRow dr in dt.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;

                writer.WriteStartElement(new Row { RowIndex = rowIndex });

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = dr.ItemArray[colInx].ToString();
                    cellValue = ReplaceHexadecimalSymbols(cellValue);

                    // Create cell with data
                    if (IsNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        cellNumericValue = 0;
                        if (double.TryParse(cellValue, out cellNumericValue))
                        {
                            cellValue = cellNumericValue.ToString();
                            AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, ref writer);
                        }
                    }
                    else if (IsDateColumn[colInx])
                    {
                        //  This is a date value.
                        DateTime dtValue;
                        string strValue = "";
                        if (DateTime.TryParse(cellValue, out dtValue))
                            strValue = dtValue.ToShortDateString();
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), strValue, ref writer);
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, ref writer);
                    }
                }
                writer.WriteEndElement(); //  End of Row
            }
            writer.WriteEndElement(); //  End of SheetData
            writer.WriteEndElement(); //  End of worksheet

            writer.Close();
        }

        private static void AppendTextCell(string cellReference, string cellStringValue, ref OpenXmlWriter writer)
        {
            //  Add a new Excel Cell to our Row 
            writer.WriteElement(new Cell
            {
                CellValue = new CellValue(cellStringValue),
                CellReference = cellReference,
                DataType = CellValues.String
            });
        }

        private static void AppendNumericCell(string cellReference, string cellStringValue, ref OpenXmlWriter writer)
        {
            //  Add a new Excel Cell to our Row 
            writer.WriteElement(new Cell
            {
                CellValue = new CellValue(cellStringValue),
                CellReference = cellReference,
                DataType = CellValues.Number
            });
        }

        private static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }

        //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
        public static string GetExcelColumnName(int columnIndex)
        {
            //  eg  (0) should return "A"
            //      (1) should return "B"
            //      (25) should return "Z"
            //      (26) should return "AA"
            //      (27) should return "AB"
            //      ..etc..
            char firstChar;
            char secondChar;
            char thirdChar;

            if (columnIndex < 26)
            {
                return ((char)('A' + columnIndex)).ToString();
            }

            if (columnIndex < 702)
            {
                firstChar = (char)('A' + (columnIndex / 26) - 1);
                secondChar = (char)('A' + (columnIndex % 26));

                return string.Format("{0}{1}", firstChar, secondChar);
            }

            int firstInt = columnIndex / 26 / 26;
            int secondInt = (columnIndex - firstInt * 26 * 26) / 26;
            if (secondInt == 0)
            {
                secondInt = 26;
                firstInt = firstInt - 1;
            }
            int thirdInt = (columnIndex - firstInt * 26 * 26 - secondInt * 26);

            firstChar = (char)('A' + firstInt - 1);
            secondChar = (char)('A' + secondInt - 1);
            thirdChar = (char)('A' + thirdInt);

            return string.Format("{0}{1}{2}", firstChar, secondChar, thirdChar);
        }

        // -- User Edit profile Page Code begins
        public static UserProfileModel getUserDetails(int UserId)
        {
            DbContextFactory db = new DbContextFactory();
            Repo<UserProfile> UserProfiles = new Repo<UserProfile>(db);

            UserProfile selectedUser = UserProfiles.Get(UserId);

            UserProfileModel upm = new UserProfileModel();
            if (selectedUser.Id != 0)
            {
                upm.Id = selectedUser.Id;
                upm.FirstName = selectedUser.FirstName;
                upm.LastName = selectedUser.LastName;
                upm.UserName = selectedUser.UserName;
                //email is user name
                upm.Email = selectedUser.UserName;
                upm.JobTitle = selectedUser.JobTitle;
                upm.Phone = selectedUser.Phone;
                upm.CountryCode = selectedUser.CountryCode.Value;
                upm.Active = selectedUser.Active;
                upm.ReportingManager = selectedUser.ReportingManager;
                upm.IsPhoneVerified = selectedUser.IsPhoneVerified;
            }
            return upm;
        }

        public static UserProfileModel getUserDetails(string UserName)
        {
            DbContextFactory db = new DbContextFactory();
            Repo<UserProfile> UserProfiles = new Repo<UserProfile>(db);
            UserProfile selectedUser = UserProfiles.Where(w=>w.UserName==UserName).FirstOrDefault();

            UserProfileModel upm = new UserProfileModel();
            if (selectedUser.Id != 0)
            {
                upm.Id = selectedUser.Id;
                upm.FirstName = selectedUser.FirstName;
                upm.LastName = selectedUser.LastName;
                upm.UserName = selectedUser.UserName;
                //email is user name
                upm.Email = selectedUser.UserName;
                upm.JobTitle = selectedUser.JobTitle;
                upm.Phone = selectedUser.Phone;
                if (upm.CountryCode != null)
                {
                    upm.CountryCode = selectedUser.CountryCode.Value;
                }
                
                upm.Active = selectedUser.Active;
                upm.ReportingManager = selectedUser.ReportingManager;
                upm.IsPhoneVerified = selectedUser.IsPhoneVerified;
            }
            return upm;
        }

        //Updating except phone number!
        public static void updateUserDetails(UserProfileModel model)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var entity = unitOfWork.UserProfile.GetByID(model.Id);
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.ReportingManager = model.ReportingManager;
            entity.JobTitle = model.JobTitle;
            entity.Phone = model.Phone;
            entity.CountryCode = model.CountryCode;
            unitOfWork.UserProfile.Update(entity);
            unitOfWork.Save();
        }
        //Updating except phone number!
        public static void updateUserPhoneNumber(int UserID,int CountryCode,string Phone)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var entity = unitOfWork.UserProfile.GetByID(UserID);
            entity.CountryCode= CountryCode;
            entity.Phone= Phone;
            unitOfWork.UserProfile.Update(entity);
            unitOfWork.Save();
        }


        public static List<LoggedDevices> getLoggedDevicesList(ClientDetail details)
        {

            try
            {
                //POST Request
                string requestUrl = WebConfigurationManager.AppSettings["APISavedDevices"].ToString();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ClientDetail));
                MemoryStream mem = new MemoryStream();
                ser.WriteObject(mem, details);
                string dataString = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
                WebClient webClient = new WebClient();
                webClient.Headers["Content-type"] = "application/json";
                webClient.Encoding = Encoding.UTF8;
                string ronse = webClient.UploadString(requestUrl, "POST", dataString);
                var Response = JsonConvert.DeserializeObject<List<LoggedDevices>>(ronse);
                return Response;
            }
            catch (Exception)
            {
                return new List<LoggedDevices>();
            }

        }

        public static bool deleteSavedDevice(LoggedDevices device)
        {
            try
            {
                //POST Request
                string requestUrl = WebConfigurationManager.AppSettings["APIRemoveDevice"].ToString();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(LoggedDevices));
                MemoryStream mem = new MemoryStream();
                ser.WriteObject(mem, device);
                string dataString = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
                WebClient webClient = new WebClient();
                webClient.Headers["Content-type"] = "application/json";
                webClient.Encoding = Encoding.UTF8;
                string ronse = webClient.UploadString(requestUrl, "POST", dataString);
                var Response = JsonConvert.DeserializeObject<bool>(ronse);
                if (Response)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }


        }


        [AllowAnonymous]
        public static IEnumerable<SelectListItem> GetCountriesList()
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var tempObj = unitOfWork.Countries.Get();
            var returnObj = tempObj.Select(f => new SelectListItem
            {
                Text = f.CountryName + " (" + "+" + f.CountryCode + ")",
                Value = f.CountryCode.ToString()
            }).Distinct();
            return returnObj;

        }


        [AllowAnonymous]
        public static IEnumerable<SelectListItem> GetShoreList()
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var tempObj = unitOfWork.EngagementLocation.Get();
            var returnObj = tempObj.Select(f => new SelectListItem
            {
                Text = f.Location,
                Value = f.Location
            }).Distinct();
            return returnObj;

        }

        [AllowAnonymous]
        public static IEnumerable<SelectListItem> GetLeadRoleList()
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var tempObj = unitOfWork.LeadershipRoles.Get();
            var returnObj = tempObj.Select(f => new SelectListItem
            {
                Text = f.RoleName ,
                Value = f.RoleName
            }).Distinct();
            return returnObj;

        }

        public enum AMOApplicationName
        {
            PortfolioAnalyzer = 1,
            BusinessAlignment = 2,
            SLAManager = 3,
            ServiceCatalog = 4,
            IdeaManager = 5,
            MigrationPlanner = 6,
            RetirementAccelerator = 7,
            KnowledgeManager = 8,
            StaffingModelOptimizer = 9,
            CloudMigrator = 10,
            CodeAnalyzer = 11,
            PerformancePredictor = 12,
            IncidentNavigator = 13,
            SourcingeffectivenessEvaluator = 14,
            ClientProfileManager = 15,
            Default = 16,
            TransformationAccelerator = 17,
            DemandManager = 18,
            FeedbackManager = 19,
            AssessmentManager = 20

        }
        public enum AMOUserRoles
        {
            User = 1,
            Admin = 2,
            Sysadmin = 5,
            CustomerUser = 6,
            ImplementationManager = 7,
            KEDBManager = 8
        }


        public  static bool HasRole(int UserId, int Clientid, int Appid, int RoleId, HomeInput input)
        {
            if (UserId != 0)
            {
                
                if (input.UserRole.ToLower() == "superuser")
                {
                    return true;
                }
            }
           
           var Ulist = (from r in input.UserApplications
                         where (r.ClientId == Clientid && r.ApplicationId == Appid && r.UserId == UserId &&  r.RoleId == RoleId)
                         select r).ToList();
            if (Ulist.Count() > 0)
            {
                return true;

            }
            return false;
        }


        public static bool HasRole( int Appid, int RoleId)
        {
            ClientDetail client = (ClientDetail)HttpContext.Current.Session["SessionClient"];
            var input = (HomeInput)HttpContext.Current.Session["MenuItemsInput"];
            var Clientid = client.ClientId;
            var UserId = input.UserId;
            if (UserId != 0)
            {
                if (input.UserRole.ToLower() == "superuser")
                {
                    return true;
                }
            }

            var Ulist = (from r in input.UserApplications
                         where (r.ClientId == Clientid && r.ApplicationId == Appid && r.UserId == UserId && r.RoleId == RoleId)
                         select r).ToList();
            if (Ulist.Count() > 0)
            {
                return true;

            }
            return false;
        }


        public static bool HasRole(string[] RoleNames)
        {
            ClientDetail client = (ClientDetail)HttpContext.Current.Session["SessionClient"];
            var input = (HomeInput)HttpContext.Current.Session["MenuItemsInput"];
            var Clientid = client.ClientId;
            var UserId = input.UserId;
            if (UserId != 0)
            {
                if (input.UserRole.ToLower() == "superuser")
                {
                    return true;
                }
            }
            var Ulist = (from r in input.UserApplications
                         where (r.ClientId == Clientid  && r.UserId == UserId && RoleNames.Contains(r.RoleName.ToLower()))
                         select r).ToList();
            if (Ulist.Count() > 0)
            {
                return true;

            }
            return false;
        }

        public static bool HasRole(int UserId, int Clientid, string[] AppNames, string[] RoleNames)
        {
           // Client client=(Client) HttpContext.Current.Session["SessionClient"];
            var input = (HomeInput)HttpContext.Current.Session["MenuItemsInput"];
            if (UserId != 0)
            {

                if (input.UserRole.ToLower() == "superuser")
                {
                    return true;
                }
            }
           
            var Ulist = (from r in input.UserApplications
                         where (r.ClientId == Clientid && AppNames.Contains( r.ApplicationName.Trim()) && r.UserId == UserId && RoleNames.Contains(r.RoleName.ToLower()))
                         select r).ToList();
            if (Ulist.Count() > 0)
            {
                return true;

            }
            return false;
        }

    }
}