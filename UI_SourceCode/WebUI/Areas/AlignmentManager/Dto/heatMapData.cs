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
using System.Text;



namespace NTTData.AMOPortal.WebUI.Dto
    {

    public class   heatMapItem
        {
           public string heatMapCategory;
           public string heatMapItemName;
           public string  heatMapColor;
           public string heatMapDataRowDetails;
           public string id;
           
        }
    public class heatMapData 
        {

        int _clientId =0;
        int _reportId = 0;
        string heatMapColName = "";
        DataTable _heatMapDataTable;
        List<string> _Category = new List<string>();
        public heatMapData()
        { }

        public heatMapData (int clientId, int reportId)
	{
            _clientId= clientId;
            _reportId = reportId;
            heatMapColName = HeatMapColumnName(reportId, clientId);
            if (heatMapColName != "")
                {

                _heatMapDataTable = GetHeatMapData(_clientId, _reportId, heatMapColName);




                }
            else
            {
            _heatMapDataTable = null;
            }

            //ReportDataSet reportdataset = AlignmentHelper.GetReportDataSetsForReportConfigId(reportId).FirstOrDefault();
            //if (reportdataset != null)
            //    {
            //    int registerDatasetId = reportdataset.RegisterDataSetId;
            //    vwRegDataSet vwRegDataSet = AlignmentHelper.GetVwRegDataSet(this._clientId).Where(c => c.RegisterDatasetId == registerDatasetId).FirstOrDefault();
            //    string columnVarType = vwRegDataSet.VariableType;
            //    heatMapColName = columnVarType;
            //    _heatMapDataTable = GetHeatMapData(_clientId, _reportId, heatMapColName);
            //    }
            //else
            //{
            //_heatMapDataTable = null;
            //}

            
	}

        public string HeatMapColumnName(int ReportId, int ClientId)
        {
                string heatMapColumnName = "";
                ReportDataSet reportdataset = AlignmentHelper.GetReportDataSetsForReportConfigId(ReportId).FirstOrDefault();
                if (reportdataset != null)
                    {
                    int registerDatasetId = reportdataset.RegisterDataSetId;
                    vwRegDataSet vwRegDataSet = AlignmentHelper.GetVwRegDataSet(ClientId).Where(c => c.RegisterDatasetId == registerDatasetId).FirstOrDefault();
                    heatMapColumnName = vwRegDataSet.VariableType;
                    }
                    return heatMapColumnName;

        }


        public List<string> ListOfColumnsToDisplay(int ClientId, int ReportId)
            {

            List<string> columns = new List<string>();
            columns.Add("Heat");
            columns.Add("Downtime");
            IList<ReportDataSet> reportdatasets = AlignmentHelper.GetReportDataSetsForReportConfigId(ReportId);

            foreach (ReportDataSet rdset in reportdatasets)
                {
                columns.Add(rdset.SelectedColumn);
                }
            return columns;
            }

        public DataTable HeatMapReportData(int ClientId, int reportId)
        {


        _clientId = ClientId;
        _reportId = reportId;
        heatMapColName = HeatMapColumnName(reportId, ClientId);
        if (heatMapColName != "")
            {

            _heatMapDataTable = GetHeatMapData(_clientId, _reportId, heatMapColName);

            }

            DataTable htTable = GetHeatMapReportData(ClientId, reportId);


            List<heatMapItem> htmapItems = heatMapItems;
     
            string heatMapColumnName = HeatMapColumnName(reportId, ClientId);
            foreach (heatMapItem item in htmapItems)
            { 
                DataRow[] drows = htTable.Select("["+this.heatMapColName+"]"+"='"+item.heatMapItemName+"'");
                int rowsCount = drows.Length;
                for(int counter = 0; counter < rowsCount; counter++)
                {
                if (drows[counter]["Heat"].ToString().ToLower() != GetColorName(item.heatMapColor).ToLower())
                {
                drows[counter].Delete();
                }
                }

                
            }


            List<string> columns = new List<string>();
            columns.Add("Heat");
            columns.Add("Downtime");
            IList<ReportDataSet> reportdatasets = AlignmentHelper.GetReportDataSetsForReportConfigId(reportId);

            foreach (ReportDataSet rdset in reportdatasets)
                {
                columns.Add(rdset.SelectedColumn);
                }



            //string[] columnNames = (from dc in htTable.Columns.Cast<DataColumn>()
            //                        select dc.ColumnName).ToArray();

            //foreach (string colName in columnNames)
            //    {
            //    if (!columns.Contains(colName))
            //        {
            //        RemoveColFromTable(colName, htTable);
            //        }
            //    }
            htTable.AcceptChanges();

           return htTable;
        }

        public DataTable HeatMapReportFilterData(int ClientId, int reportId, string HeatMapVariableName="")
            {

            ReportDataSet reportdataset = AlignmentHelper.GetReportDataSetsForReportConfigId(reportId).FirstOrDefault();
            if (reportdataset != null)
                {
                int registerDatasetId = reportdataset.RegisterDataSetId;
                vwRegDataSet vwRegDataSet = AlignmentHelper.GetVwRegDataSet(ClientId).Where(c => c.RegisterDatasetId == registerDatasetId).FirstOrDefault();
                string columnVarType = vwRegDataSet.VariableType;
                this.heatMapColName = columnVarType;
                }

            string heatMapColumnName = HeatMapColumnName(reportId, ClientId);
            DataTable htTable = GetHeatMapReportData(ClientId, reportId, heatMapColumnName);

            DataRow[]  filterRows = htTable.Select("["+this.heatMapColName+"]"+"='"+HeatMapVariableName+"'");
            DataTable filterTable = htTable.Clone();
            foreach (DataRow drow in filterRows)
            {
                //DataRow  dnewrow = filterTable.NewRow();
                //dnewrow = drow;
                filterTable.ImportRow(drow);
            }

            //Remove unwanted columns except (downtime & heat)
            List<string> columns = new List<string>();
            columns.Add("Heat");
            columns.Add("Downtime");
            IList<ReportDataSet> reportdatasets = AlignmentHelper.GetReportDataSetsForReportConfigId(reportId);

            foreach (ReportDataSet rdset in reportdatasets)
            {
            columns.Add(rdset.SelectedColumn);
            }
            string[] columnNames = (from dc in filterTable.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();

            foreach(string colName in columnNames)
            {
                if (!columns.Contains(colName))
                {
                RemoveColFromTable(colName, filterTable);
                }
            }

            _heatMapDataTable = GetHeatMapData(ClientId, reportId, heatMapColumnName);

            DataRow[] drows = _heatMapDataTable.Select("[" + this.heatMapColName + "]" + "='" + HeatMapVariableName + "'");
            if (drows.Length > 0)
            {

                DataRow drow = drows[0];
                int rowCount = filterTable.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                if (filterTable.Rows[i]["Heat"].ToString() != drow["Heat"].ToString())
                {
                filterTable.Rows[i].Delete();
                }
                }
                filterTable.AcceptChanges();
                
            }




            //filterTable.Columns.Remove();



            
            return filterTable;

            }

        private void RemoveColFromTable(string colname, DataTable dt)
        {
        dt.Columns.Remove(colname);
        }
        public string detailId { get; set; }
      

        
        public List<string> Category



        { get { return _Category; } }



       

        public DataTable GetHeatMapDataTable
            {
            get
                {
                return _heatMapDataTable;
                }
            }


        public List<heatMapItem>  heatMapItems
            {
            get
                {
                List<heatMapItem> lstHeatMapItems = new List<heatMapItem>();
                ReportDataSet reportdataset = AlignmentHelper.GetReportDataSetsForReportConfigId(this._reportId).FirstOrDefault();
                int registerDatasetId = 0;
                if (reportdataset != null)
                    {
                    registerDatasetId = reportdataset.RegisterDataSetId;
                    }
                else
                {
                return lstHeatMapItems;
                }

                                        List<String> lstColumns = ListOfColumnsToDisplay(this._clientId, this._reportId);


                    vwRegDataSet vwRegDataSet = AlignmentHelper.GetVwRegDataSet(this._clientId).Where(c => c.RegisterDatasetId == registerDatasetId).FirstOrDefault();
                    string columnVarType = vwRegDataSet.VariableType;
                    this.heatMapColName = columnVarType;

                    
                    foreach (DataRow drow in _heatMapDataTable.Rows)
                        {
                        heatMapItem hmapItem = new heatMapItem();
                        hmapItem.heatMapCategory = "";  
                        hmapItem.heatMapColor = this.GetClassName(drow["heat"].ToString().ToUpper());
                        hmapItem.heatMapItemName = drow[columnVarType].ToString();
                        hmapItem.id = drow["IdentityId"].ToString();
                        StringBuilder sbRowDetail = new StringBuilder("<div id=htmap" + hmapItem.id +">");
                        sbRowDetail.Append("<ul class=\"list-group\">");
                        DataColumnCollection dcolCollection = drow.Table.Columns;
                        int counter = 0;
                                                                foreach(var item in drow.ItemArray)
                                         {
                                          if(lstColumns.Contains(dcolCollection[counter].ColumnName))
                                              {   

                                              sbRowDetail.Append("<li class=\"list-group-item\">");
                                              sbRowDetail.Append(dcolCollection[counter].ColumnName);
                                              sbRowDetail.Append(":<b>"+item.ToString()+"</b></li>");
                                                //rowDetail +="<li class="list-group-item"">" + dcolCollection[counter].ColumnName + :<b>@item.ToString()</b></li>"
                                              }
                                              counter++;
    }
                                                                sbRowDetail.Append("</ul>");
                                                                sbRowDetail.Append("</div>");
                        hmapItem.heatMapDataRowDetails = sbRowDetail.ToString();

                        lstHeatMapItems.Add(hmapItem);
                        }
                    return lstHeatMapItems;

                    
                }
            
            }


        public string GetClassName(string colorCode)
        { 
            string className="";

            switch (colorCode.ToUpper())
            {
                case "RED":
                className = "htred";
                break;
                case "GREEN":
                className = "htgreen";
                break;
                case "BLUE":
                className = "htblu";
                break;
                case "ORANGE":
                className = "htorange";
                break;
                case "YELLOW":
                className = "htamber";
                break;
                case "GREY":
                className = "htgrey";
                break;
                case "WHITE":
                className = "htwhite";
                break;

                default:
                className = "htgrey";
                break;
            }

            return className;
        }



        public string GetColorName(string colorCode)
            {
            string className = "";

            switch (colorCode)
                {
                case "htred":
                    className = "Red";
                    break;
                case "htgreen":
                    className = "Green";
                    break;
                case "htblu":
                    className = "Blue";
                    break;
                case "htorange":
                    className = "orange";
                    break;
                case "htamber":
                    className = "Yellow";
                    break;
                case "htgrey":
                    className = "Grey";
                    break;
                case "htwhite":
                    className = "white";
                    break;

                default:
                    className = "Grey";
                    break;
                }

            return className;
            }


        //public IEnumerable<SelectListItem> HeatMapReportNames
        //    {
        //    get
        //        {
        //        return GetDropDownListItems.HeatMapReportNames(this.ClientId);
        //        }
        //    }



        //public IEnumerable<SelectListItem> ClientList
        //    {
        //    get
        //        {
        //        return GetDropDownListItems.ClientList;
        //        }
        //    }


        public DataTable GetHeatMapReportData(int ClientId,  int reportId, string heatMapColName = "")
            {
            DataTable dataTableBusinessCap;
            DataTable dtResult = new DataTable();
            using (Db db = new Db())
                {
                DataSet ds = new DataSet();
                SqlConnection sqlcon = new SqlConnection(db.Database.Connection.ConnectionString);
                try
                    {

                    sqlcon.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommand cmd = new SqlCommand("GetReport5", sqlcon);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParam1 = new SqlParameter();
                    sqlParam1.IsNullable = true;
                    sqlParam1.ParameterName = "@ClientID";
                    sqlParam1.DbType = DbType.Int32;

                    sqlParam1.Value = ClientId;
                    cmd.Parameters.Add(sqlParam1);

                    IEnumerable<SelectListItem> list = GetDropDownListItems.HeatMapReportNames(ClientId).Where(c => c.Value == reportId.ToString());
                    string ReportName = "";
                    if (list != null)
                    {
                        ReportName = list.FirstOrDefault().Text;
                    }
                    cmd.Parameters.AddWithValue("@reportname", ReportName);
                    da.SelectCommand = cmd;


                    da.Fill(ds);
                    
                    //DataTable dtResult = new DataTable(); 
                    if (ds.Tables.Count > 0)
                        {
                        dataTableBusinessCap = ds.Tables[0];
                        //dataTableBusinessCap = ds.Tables[0];
                        dtResult = dataTableBusinessCap.Clone();
                        if (dataTableBusinessCap.Columns.Count > 0 && dataTableBusinessCap.Columns.Contains("Heat"))
                            {

                            //Get Configured Heat Map Variable Name/Variable Type which is pointing to variable type in registeredatasetmap table(i.e, it comes from the Registerdatasetmap Table) 
                            //Based on the registerdataset map table variable type, the column will be present in the Heatmap data, We need to look for the column
                            //and create heat map items 

                            //IList<vwRegDataSet> vwRegDataSet = AlignmentHelper.GetVwRegDataSet(ClientId);
                            heatMapColName = HeatMapColumnName(reportId, ClientId);
                            var distinctNames = (from row in dataTableBusinessCap.AsEnumerable()
                                                 select row.Field<string>(heatMapColName)).Distinct();

                            foreach (var name in distinctNames)
                            {
                            if (name != null)
                                {
                                IEnumerable<DataRow> rows = dataTableBusinessCap.AsEnumerable()
                                .Where(i => i.Field<String>(heatMapColName) == name);
                                rows.CopyToDataTable(dtResult, LoadOption.PreserveChanges);
                                }
                                //dtResult.ImportRow(

                            }



                            }
                        //dtResult = dataTableBusinessCap.Clone();
                        }
                    else
                        {
                        //dtResult = dataTableBusinessCap.Clone();
                        }
                    }
                finally
                    {
                    if (sqlcon.State == ConnectionState.Open)
                        {
                        sqlcon.Close();
                        }
                    }
                return dtResult;
                }


            }


        public  DataTable GetHeatMapData(int? clientId, int? reportId, string heatMapColName="")
            {
            DataTable dtResult = new DataTable();

            using (Db db = new Db())
                {
                DataSet ds = new DataSet();
                //var idParam = new SqlParameter
                //{
                //    ParameterName = "clientId",
                //    Value = 66
                //};

                //var idParam1 = new SqlParameter
                //{
                //    ParameterName = "appname",
                //    Value = "Citrix"
                //};



                //IList<AppNames> appNameList = db.Database.SqlQuery<AppNames>("GetAppNames @clientId, @appname", idParam, idParam1).ToList();
                //   return appNameList;
                SqlConnection sqlcon = new SqlConnection(db.Database.Connection.ConnectionString);
                try
                    {

                    sqlcon.Open();

                    //DataTable dt = new DataTable("tmp");

                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommand cmd = new SqlCommand("GetReport5", sqlcon);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@ClientID", null);
                    SqlParameter sqlParam1 = new SqlParameter();
                    sqlParam1.IsNullable = true;
                    sqlParam1.ParameterName = "@ClientID";
                    sqlParam1.DbType = DbType.Int32;
                    //("@ClientID",SqlDbType.Int,);



                    int intclientId = clientId == null ? 0 : Convert.ToInt32(clientId);
                    sqlParam1.Value = intclientId;
                    IEnumerable<SelectListItem> list = GetDropDownListItems.HeatMapReportNames(intclientId);
                    string ReportName = "";
                    foreach (var item in list)
                        {
                        if (item.Value == reportId.ToString())
                            {
                            ReportName = item.Text;
                            break;
                            }
                        }



                    cmd.Parameters.Add(sqlParam1);


                    cmd.Parameters.AddWithValue("@reportname", ReportName);
                    da.SelectCommand = cmd;


                    da.Fill(ds);
                    DataTable dataTableBusinessCap;
                    //DataTable dtResult = new DataTable(); 
                    if (ds.Tables.Count > 0)
                        {
                        dataTableBusinessCap = ds.Tables[0];
                        dtResult = dataTableBusinessCap.Clone();
                        }
                    else
                        {
                        dataTableBusinessCap = new DataTable();
                        }
                    if (dataTableBusinessCap.Columns.Count > 0 && dataTableBusinessCap.Columns.Contains("Heat"))
                        {

                        //Get Configured Heat Map Variable Name/Variable Type which is pointing to variable type in registeredatasetmap table(i.e, it comes from the Registerdatasetmap Table) 
                        //Based on the registerdataset map table variable type, the column will be present in the Heatmap data, We need to look for the column
                        //and create heat map items 

                        //IList<vwRegDataSet> vwRegDataSet = AlignmentHelper.GetVwRegDataSet(intclientId);

                        //Delete rows where downtime is having null values

                        //DataColumn dataColumn = dataTableBusinessCap.Columns["downtime"];

                        //dataTableBusinessCap.Rows.Cast<DataRow>().Where(
                        //r => r.ItemArray[dataColumn.Ordinal].ToString() == "").ToList().ForEach(r => r.Delete());
                        //dataTableBusinessCap.AcceptChanges();

                        var distinctNames = (from row in dataTableBusinessCap.AsEnumerable()
                                             select row.Field<string>(heatMapColName)).Distinct();

                        //_Category  = (from row in dataTableBusinessCap.AsEnumerable()
                        //                     select row.Field<string>("Business Function")).Distinct().ToList(); ;


                        foreach (var name in distinctNames)
                            {
                            if (name != null && name != "")
                                {

                                IEnumerable<DataRow> rows = dataTableBusinessCap.AsEnumerable()
                                    .Where(i => i.Field<String>(heatMapColName) == name);

                                //string dataType = dataTableBusinessCap.Columns["downtime"].DataType.ToString();
                                //IEnumerable<DataRow> rows = dataTableBusinessCap.AsEnumerable()
                                //                            .Where(i => i.Field<String>(heatMapColName) == name)
                                //                            .OrderByDescending(i => i.Field<double>("downtime"));

                                //IEnumerable<DataRow> rows = dataTableBusinessCap.AsEnumerable()
                                //                            .Where(i => i.Field<String>("ServiceArea") == name);

                                //                            //.OrderByDescending(i => i.Field<double>("downtime"));



                                //bool redRowFound = false;
                                DataRow dHeatRow = dtResult.NewRow();
                                DataRow dRow;
                                List<string> heatColors = new List<string> { "RED", "ORANGE", "YELLOW", "GREEN", "BLUE" };
                                bool blnHeatRowFound = false;
                                foreach (string heatColor in heatColors)
                                    {

                                    foreach (DataRow drow in rows)
                                        {
                                        if (drow["Heat"].ToString().ToUpper() == heatColor)
                                            {
                                            blnHeatRowFound = true;
                                            dHeatRow = drow;
                                            break;
                                            }
                                        }
                                    if (blnHeatRowFound)
                                        break;
                                    }

                                if (blnHeatRowFound)
                                    {
                                    dRow = dHeatRow;
                                    }
                                else
                                    {
                                    dRow = rows.FirstOrDefault();
                                    }

                                var newDataRow = dtResult.NewRow();
                                newDataRow.ItemArray = dRow.ItemArray;
                                dtResult.Rows.Add(newDataRow);
                                }
                            }
                        }
                    }
                catch (Exception exp)
                {
                    return dtResult;
                }
                finally
                    {
                    if (sqlcon.State == ConnectionState.Open)
                        {
                        sqlcon.Close();
                        }
                    }




                return dtResult;
                }


            }

        }
    }