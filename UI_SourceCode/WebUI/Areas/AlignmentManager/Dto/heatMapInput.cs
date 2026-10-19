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
using WebUI.Areas.AlignmentManager.Dto;


namespace NTTData.AMOPortal.WebUI.Dto
{

    //public class   heatMapItem
    //    {
    //       public string heatMapCategory;
    //       public string heatMapItemName;
    //       public string  heatMapColor;
    //       public string id;

    //    }

    [Serializable]
    public class heatMapInput : Input
    {

        public string detailId { get; set; }
        public int ClientId { get; set; }
        public int? HeatMapReportId { get; set; }
        public string HeatMapReportName { get; set; }
        public string heatMapVariableName { get; set; }
        public int heatMapVariableId { get; set; }
        public heatMapData heatMapData;
        public DataTable _heatMapDataTable;
        public JsonResult JsonHeatMapItems { get; set; }
        //List<string> _Category = new List<string>();

        //public List<string> Category



        //{ get { return _Category; } }

        public PPTreeView PPTreeViewRoot
        {
            get
            {
                IList<PPTreeView> AllTreeViewItems = AlignmentHelper.GetTreeViewItems(this.ClientId);
                PPTreeView root = AllTreeViewItems.Where(c => c.ParentId == 0).FirstOrDefault();
                SetChildren(root, AllTreeViewItems);
                return root;
            }

        }


        private void SetChildren(PPTreeView ppItem, IList<PPTreeView> list)
        {
            var children = list.Where(c => c.ParentId == ppItem.Id).ToList();
            if (children.Count > 0)
            {
                foreach (var child in children)
                {
                    SetChildren(child, list);
                    ppItem.TreeViewItems.Add(child);
                }
            }

        }


        public heatMapInput()
        {

            //_heatMapDataTable = GetHeatMapData(this.ClientId, this.HeatMapReportId);
        }

        public DataTable GetHeatMapDataTable
        {
            get
            {
                return _heatMapDataTable;
            }
        }


        //public List<heatMapItem>  heatMapItems
        //    {
        //        get {

        //                List<heatMapItem> lstHeatMapItems = new List<heatMapItem>();
        //                foreach(DataRow drow in _heatMapDataTable.Rows)
        //                    {
        //                    heatMapItem hmapItem = new heatMapItem();
        //                    hmapItem.heatMapCategory = drow["GroupCategory"].ToString();
        //                        hmapItem.heatMapColor = this.GetClassName(drow["heat"].ToString().ToUpper());
        //                        hmapItem.heatMapItemName = drow["BusinessCapability"].ToString();
        //                        hmapItem.id= drow["id"].ToString();
        //                         lstHeatMapItems.Add(hmapItem); 
        //                    }
        //                    return lstHeatMapItems;

        //            }

        //    }


        //public string GetClassName(string colorCode)
        //{ 
        //    string className="";

        //    switch (colorCode)
        //    {
        //        case "RED":
        //        className = "htred";
        //        break;
        //        case "GREEN":
        //        className = "htgreen";
        //        break;
        //        case "BLUE":
        //        className = "htblu";
        //        break;
        //        case "ORANGE":
        //        className = "htorange";
        //        break;
        //        case "YELLOW":
        //        className = "htamber";
        //        break;
        //        case "GREY":
        //        className = "htgrey";
        //        break;
        //        case "WHITE":
        //        className = "htwhite";
        //        break;

        //        default:
        //        className = "htgrey";
        //        break;
        //    }

        //    return className;
        //}






        public IEnumerable<SelectListItem> HeatMapReportNames
        {
            get
            {
                return GetDropDownListItems.HeatMapReportNames(this.ClientId);
            }
        }



        public IEnumerable<SelectListItem> ClientList
        {
            get
            {
                return GetDropDownListItems.ClientList;
            }
        }


        //public  DataTable GetHeatMapData(int? clientId, int? reportId)
        //    {
        //    DataTable dtResult = new DataTable();

        //    using (Db db = new Db())
        //        {
        //        DataSet ds = new DataSet();
        //        //var idParam = new SqlParameter
        //        //{
        //        //    ParameterName = "clientId",
        //        //    Value = 66
        //        //};

        //        //var idParam1 = new SqlParameter
        //        //{
        //        //    ParameterName = "appname",
        //        //    Value = "Citrix"
        //        //};



        //        //IList<AppNames> appNameList = db.Database.SqlQuery<AppNames>("GetAppNames @clientId, @appname", idParam, idParam1).ToList();
        //        //   return appNameList;
        //        SqlConnection sqlcon = new SqlConnection(db.Database.Connection.ConnectionString);
        //        try
        //            {

        //            sqlcon.Open();

        //            //DataTable dt = new DataTable("tmp");

        //            SqlDataAdapter da = new SqlDataAdapter();
        //            SqlCommand cmd = new SqlCommand("GetReport", sqlcon);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            //cmd.Parameters.AddWithValue("@ClientID", null);
        //            SqlParameter sqlParam1 = new SqlParameter();
        //            sqlParam1.IsNullable = true;
        //            sqlParam1.ParameterName = "@ClientID";
        //            sqlParam1.DbType = DbType.Int32;
        //            //("@ClientID",SqlDbType.Int,);

        //            sqlParam1.Value = this.ClientId;

        //            int intclientId = clientId == null ? 0 :Convert.ToInt32(clientId);

        //            IEnumerable<SelectListItem> list = GetDropDownListItems.HeatMapReportNames(intclientId);
        //            string ReportName = "";
        //            foreach (var item in list)
        //            {
        //            if (item.Value == reportId.ToString())
        //            {
        //            ReportName = item.Text;
        //            break;
        //             }
        //            }



        //            cmd.Parameters.Add(sqlParam1);


        //            cmd.Parameters.AddWithValue("@reportname", ReportName);
        //            da.SelectCommand = cmd;


        //            da.Fill(ds);
        //            DataTable dataTableBusinessCap;
        //            //DataTable dtResult = new DataTable(); 
        //            if (ds.Tables.Count > 0)
        //                {
        //                dataTableBusinessCap = ds.Tables[0];
        //                dtResult = dataTableBusinessCap.Clone();
        //                }
        //            else
        //                {
        //                dataTableBusinessCap = new DataTable();
        //                }
        //            if (dataTableBusinessCap.Columns.Count > 0 && dataTableBusinessCap.Columns.Contains("Heat"))
        //                {
        //                var distinctNames = (from row in dataTableBusinessCap.AsEnumerable()
        //                                     select row.Field<string>("ServiceArea")).Distinct();

        //                _Category  = (from row in dataTableBusinessCap.AsEnumerable()
        //                                     select row.Field<string>("ServiceSubcategory")).Distinct().ToList(); ;


        //                foreach (var name in distinctNames)
        //                    {
        //                    string dataType = dataTableBusinessCap.Columns["downtime"].DataType.ToString();
        //                    IEnumerable<DataRow> rows = dataTableBusinessCap.AsEnumerable()
        //                                                .Where(i => i.Field<String>("ServiceArea") == name)
        //                                                .OrderByDescending(i => i.Field<double>("downtime"));
        //                    DataRow drow = rows.FirstOrDefault();
        //                    var newDataRow = dtResult.NewRow();
        //                    newDataRow.ItemArray = drow.ItemArray;
        //                    dtResult.Rows.Add(newDataRow);
        //                    }
        //                }
        //            }
        //        finally
        //            {
        //            if (sqlcon.State == ConnectionState.Open)
        //                {
        //                sqlcon.Close();
        //                }
        //            }
        //        return dtResult;
        //        }


        //    }




    }
}