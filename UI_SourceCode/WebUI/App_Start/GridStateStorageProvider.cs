using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI;
using Telerik.Web.UI.PersistenceFramework;
using System.IO;
using System.IO.Compression;
using System.Text;
using Domain.Entities.Report;
using NTTData.AMOPortal.Data;
/// <summary>
/// Summary description for CookieStorageProvider
/// </summary>
public class GridStateStorageProvider : IStateStorageProvider
{
    

    public void SaveStateToStorage(string key, string serializedState)
    {
        string[] listofKeys = key.Split('|');
        //int cid, int uid, bool defaultChekbox, string ViewNameText,  int reportforrrr
        int clientid=Convert.ToInt32(listofKeys[0]);
        int uid = Convert.ToInt32(listofKeys[1]);
        string reportViewname = listofKeys[2];
        int reportType = Convert.ToInt32(listofKeys[3]);
        bool isDefault = Convert.ToBoolean(listofKeys[4]);
        bool isPublic = Convert.ToBoolean(listofKeys[5]);
        string dateType = "";
        if (listofKeys.Length==7)
        {
            dateType = listofKeys[6];
        }
        string gridState = serializedState;
        ////using (StreamWriter outfile = File.CreateText(@"C:\Users\ilieva\Desktop\XMLTest\TestFiles.xml"))
        ////{
        ////    outfile.Write(serializedState);
        ////}
        SaveReportViewDetails( clientid,  uid,  reportViewname, reportType,  isDefault,  isPublic,dateType, gridState);
    }


    private int SaveReportViewDetails(int cid, int uid,  string ViewNameText, int reportType, bool isDefault,bool isPublic,string DataType,string gridState )
    //public static void SaveReportViewDetails(int cid, int uid, bool defaultChekbox, string ViewNameText, PivotGridFieldsCollection GridFields, int reportforrrr)
    {

        var contextFactory = new DbContextFactory();
        var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
        // ReportFilterDetailModel cv = new ReportFilterDetailModel();
        ReportView reportInsert = new ReportView();
        reportInsert.UserId = uid;
        reportInsert.ClientId = cid;
        if (isDefault == true)
        {
            var list = unitOfWork.ReportView.Get().Where(g => g.ClientId == cid && g.UserId == uid && g.ReportId == reportType);
            foreach (var ie in list)
            {
                ie.IsDefault = false;
                unitOfWork.ReportView.Update(ie);
            }
           reportInsert.IsDefault = true;
        }
        reportInsert.ReportId = reportType;
        reportInsert.ViewName = ViewNameText == null ? "" : ViewNameText.Trim();
       // reportInsert.StartDate = startDate;
       // reportInsert.EndDate = endDate;
        reportInsert.DateType = DataType;
        reportInsert.Description = gridState;
        reportInsert.IsPublic = isPublic;
        var Isthere = unitOfWork.ReportView.Get(g => g.ViewName == reportInsert.ViewName && g.ReportId == reportType && g.UserId == uid && g.ClientId == cid).FirstOrDefault();
        if (Isthere == null)
        {
            unitOfWork.ReportView.Insert(reportInsert);
           // unitOfWork.Save();
        }
        else
        {
            reportInsert.Id = Isthere.Id;
            if (isDefault == true)
            {
                Isthere.IsDefault = true;
            }
            Isthere.Description = gridState;
            unitOfWork.ReportView.Update(Isthere);
            //unitOfWork.Save();
        }
        unitOfWork.Save();
        return reportInsert.Id;
    }


    public string LoadStateFromStorage(string key)
    {
        ////StringBuilder sb = new StringBuilder();
        ////using (StreamReader sr = new StreamReader(@"C:\Users\ilieva\Desktop\XMLTest\TestFiles.xml"))
        ////{
        ////    sb.Append(sr.ReadToEnd());
        ////}
        var contextFactory = new DbContextFactory();
        var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
        string[] listofKeys = key.Split('|');
        //int cid, int uid, bool defaultChekbox, string ViewNameText,  int reportforrrr
        int clientid = Convert.ToInt32(listofKeys[0]);
        int uid = Convert.ToInt32(listofKeys[1]);
        string reportViewname = listofKeys[2];
        int reportType = Convert.ToInt32(listofKeys[3]);
        bool isDefault = Convert.ToBoolean(listofKeys[4]);
        bool isPublic = Convert.ToBoolean(listofKeys[5]);
        if (listofKeys.Length == 7)
        {
            string dateType = listofKeys[6];
            var   Isthere8 = unitOfWork.ReportView.Get(g => g.ViewName == reportViewname && g.ReportId == reportType &&( g.UserId == uid ||g.IsPublic==true)&& g.ClientId == clientid && g.DateType == dateType).FirstOrDefault();
           return Isthere8.Description;
        }
        else
        {
            var Isthere = unitOfWork.ReportView.Get(g => g.ViewName == reportViewname && g.ReportId == reportType && (g.UserId == uid || g.IsPublic == true) && g.ClientId == clientid).FirstOrDefault();
            return Isthere.Description;
        }
    } 


}
