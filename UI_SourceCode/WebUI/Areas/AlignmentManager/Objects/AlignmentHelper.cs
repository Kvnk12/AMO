using System;
using System.Collections.Generic;
using System.Linq;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using NTTData.AMOPortal.WebUI.Dto;
using WebUI.Areas.AlignmentManager.Dto;

namespace WebUI.Areas.AlignmentManager.Objects
{
    public class AlignmentHelper
    {
        private static Dictionary<RelationshipTypeEnum, int> _relationshipTypes;
        private static Dictionary<VariableTypeEnum, int> _variableTypes;

        internal static void Initialize()
        {
            using (var context = new DbContextFactory())
            {
                _relationshipTypes = InitializeTypeIdMap<RelationshipTypeEnum, RelationshipType>(context);
                _variableTypes = InitializeTypeIdMap<VariableTypeEnum, VariableType>(context);
            }
        }

        private static Dictionary<TEnum, int> InitializeTypeIdMap<TEnum, TEntity>(DbContextFactory context)
            where TEntity : Entity, ITypeClass, new()
            where TEnum : struct
        {
            var result = new Dictionary<TEnum, int>();
            var entityRepo = new Repo<TEntity>(context);
            foreach (TEntity entity in entityRepo.GetAll())
            {
                TEnum tEnum;
                if (Enum.TryParse(entity.TypeName, out tEnum))
                {
                    if (!result.ContainsKey(tEnum))
                        result.Add(tEnum, entity.TypeId);
                    else
                        result[tEnum] = entity.TypeId;
                }
            }
            return result;
        }

        public static TEntity GetById<TEntity>(int id) where TEntity : Entity, new()
        {
            using (var context = new DbContextFactory())
                return new Repo<TEntity>(context).Get(id);
        }



        public static IList<TEntity> All<TEntity>()
            where TEntity : Entity, new()
        {
            using (var context = new DbContextFactory())
                return new Repo<TEntity>(context).GetAll().ToList();
        }

        public static IList<ReportAlgorithmMD> GetAlogMdList(int Id = 0)
        {
        using (var context = new DbContextFactory())
            return new Repo<ReportAlgorithmMD>(context).GetAll().Where(c => c.AlgorithmId == Id).ToList();

            
        }

        //public static IList<mdmVariable> GetAllOfType(VariableTypeEnum type)
        //{
        //    int typeId = _variableTypes.ContainsKey(type) ? _variableTypes[type] : 0;
        //    return GetAllOfType(typeId);
        //}

        public static IList<DataSetColumnMap> GetColumnMaps(string TableName = "", int regDataSetId = 0)
            {


            List<DataSetColumnMap> dataSetColumnMap = new List<DataSetColumnMap>();
            DataTable dtable ;
            using (DbContextFactory context = new DbContextFactory())
                {
                string constring = context.GetContext().Database.Connection.ConnectionString;

                SqlConnection sqlconnection = new SqlConnection(constring);
                //string queryString = "SELECT COLUMN_NAME  FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + TableName + "' and table_schema = 'amopp' ORDER BY COLUMN_NAME"; 
                string queryString = "SELECT COLUMN_NAME  FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + TableName + "' ORDER BY COLUMN_NAME";
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, sqlconnection);
                DataSet dSet = new DataSet();
                adapter.Fill(dSet, "tableInfo");
                dtable = dSet.Tables[0];
                }

            if (regDataSetId == 0 && dtable != null && dtable.Rows.Count > 0)
            {
            foreach (DataRow drow in dtable.Rows)
            {
            DataSetColumnMap dcolmap = new DataSetColumnMap();
            dcolmap.SourceColumnName = drow[0].ToString();
            dataSetColumnMap.Add(dcolmap);
            }
            }


           if (regDataSetId > 0 && dtable != null && dtable.Rows.Count > 0)
            {

            IList<RegisterDatasetMap> dataSetMap =   AlignmentHelper.GetRegisterDataSetMap(regDataSetId);
            foreach (DataRow drow in dtable.Rows)
                {
                string colName = drow[0].ToString().ToLower();
                    var Item = from item in dataSetMap
                           where (item.DataSetColumnName.ToLower() == colName)
                           select item;

                    if (Item.Count() > 0)
                        {
                        DataSetColumnMap dcolmap = new DataSetColumnMap();
                        dcolmap.SourceColumnName = drow[0].ToString();
                        dcolmap.VariableType = Item.First().VariableType;
                        dcolmap.Id = Item.First().Id;
                        dcolmap.isSelected = true;
                        dataSetColumnMap.Add(dcolmap);

                        }
                    else
                    {

                    DataSetColumnMap dcolmap = new DataSetColumnMap();
                    dcolmap.SourceColumnName = drow[0].ToString();
                    dataSetColumnMap.Add(dcolmap);
                    }

                }        
            }
            
            return dataSetColumnMap;
            }



        



        public static IList<DataSetColumnsForHeatMapReport> GetColumnsForHeatMapReport(int ReportConfigId, int RegisterDataSetId = 0)
            {


            List<DataSetColumnsForHeatMapReport> dataSetSelectedColumns = new List<DataSetColumnsForHeatMapReport>();

            string TableName = "";
            DataTable dtable;
            using (DbContextFactory context = new DbContextFactory())
                {

                RegisterDataset regDataSet = new Repo<RegisterDataset>(context).Where(r => r.Id == RegisterDataSetId).First(); // FirstOrDefault();
                 if (regDataSet != null)
                     TableName = regDataSet.DatabaseObjectName;
                }

                using (DbContextFactory context = new DbContextFactory())
                    {

                string constring = context.GetContext().Database.Connection.ConnectionString;

                SqlConnection sqlconnection = new SqlConnection(constring);
                string queryString = "SELECT COLUMN_NAME  FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + TableName + "' ORDER BY COLUMN_NAME"; 
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, sqlconnection);
                DataSet dSet = new DataSet();
                adapter.Fill(dSet, "tableInfo");
                dtable = dSet.Tables[0];
                }
         


            if (ReportConfigId > 0 && dtable != null && dtable.Rows.Count > 0)
                {

                //Get the selected Rows
                IList<ReportDataSet> dataSetColumns = AlignmentHelper.GetReportDataSets(ReportConfigId, RegisterDataSetId);
                foreach (DataRow drow in dtable.Rows)
                    {
                    string colName = drow[0].ToString().ToLower();
                    var Item = from item in dataSetColumns
                               where (item.SelectedColumn.ToLower() == colName)
                               select item;

                    if (Item.Count() > 0)
                        {
                        DataSetColumnsForHeatMapReport dcolmap = new DataSetColumnsForHeatMapReport();
                        dcolmap.ColumnName = Item.First().SelectedColumn;
                        dcolmap.Id = Item.First().Id;
                        dcolmap.isSelected = true;
                        dataSetSelectedColumns.Add(dcolmap);

                        }
                    else
                        {
                        DataSetColumnsForHeatMapReport dcolmap = new DataSetColumnsForHeatMapReport();
                        dcolmap.ColumnName = colName;
                        dataSetSelectedColumns.Add(dcolmap);
                        }

                    }
                }
            else
            {
            //Getting the columns into  a datatable as rows
            if (dtable != null && dtable.Rows.Count > 0)
                {
                foreach (DataRow drow in dtable.Rows)
                    {
                    DataSetColumnsForHeatMapReport dcolmap = new DataSetColumnsForHeatMapReport();
                    dcolmap.ColumnName = drow[0].ToString();
                    dataSetSelectedColumns.Add(dcolmap);
                    }
                }
            }

            return dataSetSelectedColumns;
            }



        public static IList<vwVariableVariableLabel> GetAllOfOtherTypesForParentChild(int typeId, int clientId, int variableId)
        {
            using (var context = new DbContextFactory())
                return new Repo<vwVariableVariableLabel>(context).Where(v => (v.fkVariableTypeId != typeId && v.ClientId == clientId && v.VariableId != variableId)).ToList();
        }



        public static IList<vwVariableVariableLabel> GetAllOfType(int typeId, int clientId, int variableId)
        {
            using (var context = new DbContextFactory())
                return new Repo<vwVariableVariableLabel>(context).Where(v => (v.fkVariableTypeId == typeId && v.ClientId == clientId && v.VariableId != variableId)).ToList();
        }


        public static IEnumerable<SelectListItem> RegDataList(int clientId)
            {
            using (var context = new DbContextFactory())
                {

                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.RegisterDataset> varRegRepo = new Repo<RegisterDataset>(Context);
                List<NTTData.AMOPortal.Domain.Entities.RegisterDataset> varRegList = varRegRepo.Where(c=> c.ClientId == clientId).ToList();
                var Items = varRegList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.DatabaseObjectName
                }).ToList();

                Items.Insert(0, new SelectListItem { Text = "Select Data Set Name", Value = "0" });
                return Items;

                }
            }


        //public static IList<SelectListItem> HeatMapReportNames(int clientId)
        //    {
        //    using (var context = new DbContextFactory())
        //        {

        //        DbContextFactory Context = new DbContextFactory();
        //        Repo<NTTData.AMOPortal.Domain.Entities.ReportConfiguration> varRepo = new Repo<ReportConfiguration>(Context);
        //        List<NTTData.AMOPortal.Domain.Entities.ReportConfiguration> varList = varRepo.Where(c => c.ClientId == clientId && c.IsDeleted == false).ToList();
        //        var Items = varList.Select(CList => new SelectListItem
        //        {
        //            Value = CList.Id.ToString(),
        //            Text = CList.ReportName
        //        }).ToList();
        //        return Items;
        //        }
        //    }


        public static IList<ReportConfiguration> GetHeatMapReports(int clientId)
            {
            using (var context = new DbContextFactory())
                return new Repo<ReportConfiguration>(context).GetAll().Where(c => c.ClientId == clientId && c.IsDeleted == false).ToList();
            }



        public static  IList<vwRegDataSet> GetVwRegDataSet(int clientId)
            {
            using (var context = new DbContextFactory())
                return new Repo<vwRegDataSet>(context).GetAll().Where(c => c.ClientId == clientId).ToList();
            }

        public static vwclientinfo GetClientInfo(int clientId)
        {
            using (var context = new DbContextFactory())
                return new Repo<vwclientinfo>(context).Where(v => v.Id == clientId).FirstOrDefault();
        }



        public static IList<RegisterDatasetMap> GetRegisterDataSetMap(int regDataSetId)
            {
            using (var context = new DbContextFactory())
                return new Repo<RegisterDatasetMap>(context).Where(v => v.RegisterDataSetId == regDataSetId).ToList();
            }


        public static int  GetAlgorithmId(int ReportConfigId)
            {
            int algorithmId = 0;
            using (var context = new DbContextFactory())
                {

                ReportAlgorithm ra = new Repo<ReportAlgorithm>(context).Where(v => v.ReportConfigId == ReportConfigId).FirstOrDefault();

                if (ra != null)
                {
                algorithmId= ra.Id;    
                }
                }
                return algorithmId;
            }


        public static string GetValueForKeyFromAlogorithm(int AlgorithmId, string Key)
        {
        string KeyValue = "";
        using (var context = new DbContextFactory())
            {
            ReportAlgorithmMD repoAlgorithmMd = new Repo<ReportAlgorithmMD>(context).Where(v => (v.AlgorithmId == AlgorithmId && v.key == Key)).FirstOrDefault();
            if (repoAlgorithmMd != null)
                KeyValue = repoAlgorithmMd.Value;
            else
                KeyValue = "";
            }
        return KeyValue;
        }


        public static IList<SelectListItem> GetAlogirthmNames()
            {
            using (var context = new DbContextFactory())
                {

                Repo<NTTData.AMOPortal.Domain.Entities.ReportalgorithmDefn> algor = new Repo<ReportalgorithmDefn>(context);
                List<NTTData.AMOPortal.Domain.Entities.ReportalgorithmDefn> algorlist = algor.GetAll().ToList();

                var Items = algorlist.GroupBy(m => new { m.AlgorithmName, m.AlgorithmSolution }).Select(m =>
                    new SelectListItem
                    {
                        Value = m.Key.AlgorithmName,
                        Text = m.Key.AlgorithmName
                    }).ToList();
                return Items;
                }
            }

        public static IList<SelectListItem> GetColumns(int reportConfigId)
            {
            using (var context = new DbContextFactory())
                {

                Repo<NTTData.AMOPortal.Domain.Entities.ReportDataSet> report = new Repo<NTTData.AMOPortal.Domain.Entities.ReportDataSet>(context);
                List<NTTData.AMOPortal.Domain.Entities.ReportDataSet> reportList = report.Where(c=> c.ReportConfigId == reportConfigId).ToList();
                var Items = reportList.Select(CList => new SelectListItem
                {
                    Value = CList.SelectedColumn,
                    Text = CList.SelectedColumn
                }).ToList();
                return Items;

                } 
            }

        public static IList<SelectListItem> GetMDMVariableTypes(int ClientId=0, string SelectedItem = "")
        {
        return( AlignmentHelper.GetVariableTypes(ClientId).Select(Clist => new SelectListItem
        {
            Selected = (Clist.Text == SelectedItem),
            Value = Clist.Value,
            Text = Clist.Value
        }).ToList());
        }

        public static IList<SelectListItem> GetColumns(int reportConfigId, string SelectedItem)
            {
            using (var context = new DbContextFactory())
                {

                Repo<NTTData.AMOPortal.Domain.Entities.ReportDataSet> report = new Repo<NTTData.AMOPortal.Domain.Entities.ReportDataSet>(context);
                List<NTTData.AMOPortal.Domain.Entities.ReportDataSet> reportList = report.Where(c => c.ReportConfigId == reportConfigId).ToList();
                var Items = reportList.Select(CList => new SelectListItem
                {
                    Selected = (CList.SelectedColumn == SelectedItem),
                    Value = CList.SelectedColumn,
                    Text = CList.SelectedColumn
                }).ToList();
                return Items;

                }
            }


        public static IList<SelectListItem> GetChartTemplates()
            {
            using (var context = new DbContextFactory())
                {

                Repo<NTTData.AMOPortal.Domain.Entities.ReportChartTemplates> report = new Repo<NTTData.AMOPortal.Domain.Entities.ReportChartTemplates>(context);
                List<NTTData.AMOPortal.Domain.Entities.ReportChartTemplates> reportList = report.GetAll().ToList();
                var Items = reportList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.ReportNameToDisplay
                }).ToList();
                return Items;

                }
            }


        public class SelectListItemComparer : EqualityComparer<SelectListItem>
            {
            public override bool Equals(SelectListItem x, SelectListItem y)
                {
                return x.Value.Equals(y.Value);
                }

            public override int GetHashCode(SelectListItem obj)
                {
                return obj.Value.GetHashCode();
                }
            } 


        public static IList<SelectListItem> GetVariableTypes(int ClientId)
            {
            using (var context = new DbContextFactory())
                {

                Repo<NTTData.AMOPortal.Domain.Entities.vwAlmTreeView> vwvariable = new Repo<NTTData.AMOPortal.Domain.Entities.vwAlmTreeView>(context);
                List<NTTData.AMOPortal.Domain.Entities.vwAlmTreeView> vartypeList = vwvariable.Where(c => c.ClientId == ClientId).ToList();
                var Items = vartypeList.Select(CList => new SelectListItem
                {
                    Value = CList.VARTYPE,
                    Text = CList.VARTYPE
                }).Distinct( new SelectListItemComparer()).ToList();
                return Items;

                }
            }


        public static IList<SelectListItem> GetDefnAlgorithms(string selectedItemValue="")
            {
            using (var context = new DbContextFactory())
                {
                Repo<NTTData.AMOPortal.Domain.Entities.ReportalgorithmDefn> algor = new Repo<ReportalgorithmDefn>(context);
                List<NTTData.AMOPortal.Domain.Entities.ReportalgorithmDefn> algorlist = algor.GetAll().ToList();

                var Items = algorlist.GroupBy(m=> new {m.AlgorithmName, m.AlgorithmSolution}).Select(m => 
                    
                    
                    new SelectListItem { 
                         Selected = (m.Key.AlgorithmSolution == selectedItemValue), 
                          Value = m.Key.AlgorithmSolution, 
                          Text =  m.Key.AlgorithmName}).ToList();

                Items.Insert(0, new SelectListItem { Value = "", Text = "" });
                return Items;
                }


            }
        


        public static IList<SelectListItem> GetAlgorithms()
            {
            using (var context = new DbContextFactory())
                {
                Repo<NTTData.AMOPortal.Domain.Entities.ReportAlgorithmConfiguration> algor = new Repo<ReportAlgorithmConfiguration>(context);
                List<NTTData.AMOPortal.Domain.Entities.ReportAlgorithmConfiguration> algorlist = algor.GetAll().ToList();
                var Items = algorlist.Select(CList => new SelectListItem
                {
                    Value = CList.AlgorithmSolution.ToString(),
                    Text = CList.AlgorithmName.ToString()
                }).ToList();
                return Items;
                }


            }


        public static IList<ReportAlgorithmStageMD> GetReportAlgorithmStageMd(int AlgorId)
            {
            using (var context = new DbContextFactory())
                return new Repo<ReportAlgorithmStageMD>(context).Where(v => v.AlgorithmId == AlgorId).ToList();
            }


        public static ReportAlgorithm GetReportAlgorithm(int ReportId)
            {
            using (var context = new DbContextFactory())
                return new Repo<ReportAlgorithm>(context).Where(v => v.ReportConfigId == ReportId).FirstOrDefault();
            }


        public static IList<ReportalgorithmDefn> GetReportalgorithmDefnItems(string AlgorithmSolution)
            {
            using (var context = new DbContextFactory())
                return new Repo<ReportalgorithmDefn>(context).Where(v => v.AlgorithmSolution == AlgorithmSolution && v.IsDeleted == false).OrderBy(c => c.Order).ToList();
            }


        public static ReportChartTemplates GetChartInfo(int TemplateId)
        {
            using (var context = new DbContextFactory())
              return  new Repo<ReportChartTemplates>(context).Where(v => v.Id == TemplateId).FirstOrDefault();
         }


         public static IList<ReportDataSet> GetReportDataSetsForReportConfigId(int reportConfigId)
            {
            using (var context = new DbContextFactory())
                return new Repo<ReportDataSet>(context).Where(v => v.ReportConfigId == reportConfigId).ToList();
            }

         public static IList<ReportFilter> GetReportFilterForReportConfigId(int reportConfigId)
             {
             using (var context = new DbContextFactory())
                 return new Repo<ReportFilter>(context).Where(v => v.ReportConfigId == reportConfigId).ToList();
             }



        public static IList<ReportDataSet> GetReportDataSets(int reportConfigId, int registerDataSetId)
            {
            using (var context = new DbContextFactory())
                return new Repo<ReportDataSet>(context).Where(v => v.ReportConfigId == reportConfigId && v.RegisterDataSetId == registerDataSetId).ToList();
            }


        public static IList<ReportThreshold> GetReportThresholdValues(int ReportConfigId)
            {
            using (var context = new DbContextFactory())
                return new Repo<ReportThreshold>(context).Where(v => v.ReportConfigId == ReportConfigId).ToList();
            }


        public static string GetAlgorithmName(int ReportConfigId)
            {
            string algorithmName = "";
            using (var context = new DbContextFactory())
                {
                    ReportAlgorithm rmd = new Repo<ReportAlgorithm>(context).Where(v => v.ReportConfigId == ReportConfigId).FirstOrDefault();
                    if (rmd != null)
                    {
                    algorithmName = rmd.AlgorithmName;  //new Repo<ReportAlgorithm>(context).Get(rmd.AlgorithmId).AlgorithmName;

                    }

                }
            return algorithmName;
            }



        public static IList<vwVariableVariableLabel> GetAllOfTypeForVarTypeId(int typeId, int clientId)
        {
            using (var context = new DbContextFactory())
                return new Repo<vwVariableVariableLabel>(context).Where(v => (v.fkVariableTypeId == typeId && v.ClientId == clientId)).ToList();
        }



        public static IList<vwVariableVariableLabel> GetAllOfType(int typeId)
        {
            using (var context = new DbContextFactory())
                return new Repo<vwVariableVariableLabel>(context).Where(v => (v.fkVariableTypeId == typeId)).ToList();
        }



        //public static IList<vwVariableVariableLabel> GetAllOfType(int typeId)
        //{
        //    using (var context = new DbContextFactory())
        //        return new Repo<vwVariableVariableLabel>(context).Where(v => (v.fkVariableTypeId == typeId)).ToList();
        //}



        ///
        /// Commented on 6th May as per Todd's comments, for heat map builders, all the variable types that are configured for client need to be shown not the list from the 
        /// not from the registerdataset changes have been made below the commented code
        ///
        //public static IEnumerable<SelectListItem> GetDataSetVariableTypes(int clientId)
        //{

        //using (var context = new DbContextFactory())
        //    return new Repo<vwreportvartypes>(context).Where(c=> c.ClientId == clientId).ToList().Select
        //        (
        //   CList => new SelectListItem
        //   {
        //       Value = CList.Id.ToString(),
        //       Text = CList.VariableType
        //   });

        //}



        public static IEnumerable<SelectListItem> GetDataSetVariableTypes(int clientId)
            {

            using (var context = new DbContextFactory())
                return new Repo<vwreportvartypes>(context).Where(c => c.ClientId == clientId).ToList().Select
                    (
               CList => new SelectListItem
               {
                   Value = CList.Id.ToString(),
                   Text = CList.VariableType
               });

            }


     public static int GetVariableTypeId(int reportVariableId)
         {
               using(var context = new DbContextFactory())
                   {
                    ReportVariable reportVariable = GetById<ReportVariable>(reportVariableId);
                    return reportVariable.VariableTypeId;
                   }
                
         }

     public static IEnumerable<SelectListItem> ListAllTableAndViews()
     {

           using (DbContextFactory context = new DbContextFactory())
               {
                    string constring = context.GetContext().Database.Connection.ConnectionString;

                    SqlConnection sqlconnection = new SqlConnection(constring);
                    string queryString = "SELECT * FROM information_schema.tables order by TABLE_NAME";
                    //string queryString =  "SELECT * FROM information_schema.tables where table_schema  = 'amopp' order by TABLE_NAME";
               
                    SqlDataAdapter adapter = new SqlDataAdapter(queryString, sqlconnection);
                    DataSet dSet = new DataSet();
                    adapter.Fill(dSet, "tableInfo");
                    DataTable dtable = dSet.Tables[0];
                    SelectListItem listItem;
                    List<SelectListItem> list = new List<SelectListItem>();
                    foreach (DataRow dt in dtable.Rows)
                    {
                    listItem = new SelectListItem();
                    listItem.Text = dt["TABLE_NAME"].ToString();
                    listItem.Value = dt["TABLE_NAME"].ToString();
                    list.Add(listItem);
                    }

                    return list;

                                

               }
         }



     public static IList<SelectListItem> GetColumnData(int ColId)
         {
         int registerDataSetId = 0;
         string tableName = "";
         string ColumnName = "";
          using (var context = new DbContextFactory())
              {
              ReportDataSet rdset = new Repo<ReportDataSet>(context).Where(c => c.Id == ColId).FirstOrDefault();
              if (rdset != null)
                  {
                  registerDataSetId = rdset.RegisterDataSetId;
                  ColumnName = rdset.SelectedColumn;
                  RegisterDataset regDataSet = new Repo<RegisterDataset>(context).Where(c => c.Id == registerDataSetId).FirstOrDefault();
                  tableName = regDataSet.DatabaseObjectName;
                  }
                }


          if (tableName != "" && ColumnName != "")
              {
              using (DbContextFactory context = new DbContextFactory())
                  {



                  string constring = context.GetContext().Database.Connection.ConnectionString;

                  SqlConnection sqlconnection = new SqlConnection(constring);
                  string queryString = "SELECT DISTINCT " + "[" + ColumnName + "]" + " AS COLUMNDATA  FROM  " + tableName + " WHERE " + "[" +   ColumnName + "] IS NOT NULL";
                  SqlDataAdapter adapter = new SqlDataAdapter(queryString, sqlconnection);
                  DataSet dSet = new DataSet();
                  adapter.Fill(dSet, "tableInfo");
                  DataTable dtable = dSet.Tables[0];
                  SelectListItem listItem;
                  List<SelectListItem> list = new List<SelectListItem>();
                  foreach (DataRow dt in dtable.Rows)
                      {
                      listItem = new SelectListItem();
                      listItem.Text = dt["COLUMNDATA"].ToString();
                      //listItem.Value = dt["TABLE_NAME"].ToString();
                      listItem.Value = ColId.ToString();
                      list.Add(listItem);
                      }

                  return list;

                  }

              }
          else
          {
              return new List<SelectListItem>();
           }
         }
        

        public static IList<VwMDMrelationwithlabel> GetAllOfTypeForDependInfo(int typeId)
        {
            using (var context = new DbContextFactory())
                return new Repo<VwMDMrelationwithlabel>(context).Where(v => (v.fkVariableTypeId == typeId)).ToList();
        }



        //public static IList<vwVariableVariableLabel> GetAllOfTypeByCatId(int typeId)
        //    {
        //    using (var context = new DbContextFactory())
        //        return new Repo<vwVariableVariableLabel>(context).Where(v => (v.fkVariableTypeId == typeId)).ToList();
        //    }



        public static mdmVariable GetVariable(int id)
        {
            return GetById<mdmVariable>(id);
        }



        public static IList<vwMetaDataDetails> GetvwMetaDetails(int clientId, int variableId)
        {
            using (DbContextFactory Context = new DbContextFactory())
            {
                return (new Repo<vwMetaDataDetails>(Context).Where(v => v.ClientId == clientId && v.variableId == variableId).ToList());
                //return (new Repo<vwMetaDataDetails>(Context).GetAll().ToList());
                //    //.Where(v => v.ClientId == clientId && v.Id == variableId).ToList());
            }


        }


        public static vwVariableVariableLabel GetvwVariable(int id)
        {
            return GetById<vwVariableVariableLabel>(id);
        }


        public static IList<vwAlmTreeView> GetvwALMTreeView(int clientId)
        {
            using (DbContextFactory Context = new DbContextFactory())
            {
                return (new Repo<vwAlmTreeView>(Context).Where(v => v.ClientId == clientId).ToList());
            }


        }

        public static IList<vwAlmTreeView> GetvwALMTreeViewChildren(int clientId, int childId)
        {
            using (DbContextFactory Context = new DbContextFactory())
            {
                return (new Repo<vwAlmTreeView>(Context).Where(v => v.ClientId == clientId && v.VariableLabelId == childId).ToList());
            }


        }


        public static IList<vwAlmTreeView> GetvwALMTreeViewCatItems(int clientId, int catId)
        {
            using (DbContextFactory Context = new DbContextFactory())
            {

                return (new Repo<vwAlmTreeView>(Context).Where(v => v.ClientId == clientId && v.vartypeid == catId).ToList());
            }


        }

        //Commented On 25th Aug 2014
        //public static string SSRSReportName(int ReportId)
        //{
        //string ReportName = "";
        //using (DbContextFactory Context = new DbContextFactory())
        //    {
        //    ReportConfiguration reportConfiguration = new Repo<ReportConfiguration>(Context).Where(i => i.Id == ReportId).FirstOrDefault();
        //    if (reportConfiguration != null)
        //    { 
        //        ReportName = reportConfiguration.SSRSReportName == null ? "" : reportConfiguration.SSRSReportName;
        //    }
        //    }

        //    return ReportName;
            
        //}

        public static int SSRSReportTemplateId(int ReportId)
            {
            int SSRSReportTemplateId = 0;
            using (DbContextFactory Context = new DbContextFactory())
                {
                ReportGraphConfiguration reportGraphConfiguration = new Repo<ReportGraphConfiguration>(Context).Where(i => i.ReportConfigId == ReportId).FirstOrDefault();
                if (reportGraphConfiguration != null)
                    {
                    SSRSReportTemplateId = reportGraphConfiguration.SSRSTemplateFileID;
                    }
                }

            return SSRSReportTemplateId;

            }


        public static IList<PPTreeView> GetTreeViewItems(int ClientId = 0)
        {

            List<PPTreeView> lstPpTreeView = new List<PPTreeView>();
            DataSet ds = new DataSet();
            DataTable dtTreeView; 

               using (Db db = new Db())
                {
                SqlConnection sqlcon = new SqlConnection(db.Database.Connection.ConnectionString);
                try
                    {


                    sqlcon.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommand cmd = new SqlCommand("GetTreeview", sqlcon);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParam1 = new SqlParameter();
                    sqlParam1.IsNullable = false;
                    sqlParam1.ParameterName = "@clientID";
                    sqlParam1.DbType = DbType.Int32;
                    sqlParam1.Value = ClientId;

                    cmd.Parameters.Add(sqlParam1);
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    
                    //DataTable dtResult = new DataTable(); 
                    if (ds.Tables.Count > 0)
                        {
                        dtTreeView = ds.Tables[0];
                        //dtResult = dataTableBusinessCap.Clone();
                        }
                    else
                        {
                        dtTreeView = new DataTable();
                        }
                    }
                finally
                    {
                    if (sqlcon.State == ConnectionState.Open)
                        {
                        sqlcon.Close();
                        }
                    }
                   }

               if (dtTreeView != null && dtTreeView.Rows.Count > 0)
               {
                   //Add row to list 
                   foreach (DataRow dtrow in dtTreeView.Rows)
                   {
                   PPTreeView pp = new PPTreeView();
                   pp.ClientId = ClientId;
                   pp.Id = Convert.ToInt32(dtrow["Id"].ToString());
                   pp.varcatId = Convert.ToInt32(dtrow["varcatid"].ToString());
                   pp.varTypeId = Convert.ToInt32(dtrow["vartypeid"].ToString());
                   pp.variableLabelId = Convert.ToInt32(dtrow["mdmvariableLabelId"].ToString());
                   pp.variableId = Convert.ToInt32(dtrow["mdmvarid"].ToString());
                   pp.VariableName = dtrow["VariableName"].ToString();
                   pp.ParentId = Convert.ToInt32(dtrow["ParentId"].ToString());
                   pp.IsDeleted = Convert.ToInt32(dtrow["isdeleted"].ToString());
                   lstPpTreeView.Add(pp);
                   }
               }


            return lstPpTreeView;
            
        }


        public static IList<PPTreeView> GetTreeViewDependItems(int ClientId = 0, int VariableId=0)
            {


            string VariableLabelName ="";
               
                mdmVariable  variable = GetById<mdmVariable>(VariableId);
                if (variable != null)
                    {
                    mdmVariableLabel varlabel = GetById<mdmVariableLabel>(variable.VariableLabelId);
                    if (varlabel != null)
                        {
                        VariableLabelName = varlabel.VariableLabel;
                        }
                    }
            List<PPTreeView> lstPpTreeView = new List<PPTreeView>();
            DataSet ds = new DataSet();
            DataTable dtTreeView;

            using (Db db = new Db())
                {
                SqlConnection sqlcon = new SqlConnection(db.Database.Connection.ConnectionString);
                try
                    {


                    sqlcon.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommand cmd = new SqlCommand("GetTreeviewDependInfo", sqlcon);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter sqlParam1 = new SqlParameter();
                    sqlParam1.IsNullable = false;
                    sqlParam1.ParameterName = "@clientID";
                    sqlParam1.DbType = DbType.Int32;
                    sqlParam1.Value = ClientId;
                    cmd.Parameters.Add(sqlParam1);

                    SqlParameter sqlParam2 = new SqlParameter();
                    sqlParam2.IsNullable = false;
                    sqlParam2.ParameterName = "@dependvariableName";
                    sqlParam2.DbType = DbType.String;
                    sqlParam2.Value = VariableLabelName;
                    cmd.Parameters.Add(sqlParam2);


                    
                    da.SelectCommand = cmd;
                    da.Fill(ds);

                    //DataTable dtResult = new DataTable(); 
                    if (ds.Tables.Count > 0)
                        {
                        dtTreeView = ds.Tables[0];
                        //dtResult = dataTableBusinessCap.Clone();
                        }
                    else
                        {
                        dtTreeView = new DataTable();
                        }
                    }
                finally
                    {
                    if (sqlcon.State == ConnectionState.Open)
                        {
                        sqlcon.Close();
                        }
                    }
                }

            if (dtTreeView != null && dtTreeView.Rows.Count > 0)
                {
                //Add row to list 
                foreach (DataRow dtrow in dtTreeView.Rows)
                    {
                    PPTreeView pp = new PPTreeView();
                    pp.ClientId = ClientId;
                    pp.Id = Convert.ToInt32(dtrow["Id"].ToString());
                    pp.varcatId = Convert.ToInt32(dtrow["varcatid"].ToString());
                    pp.varTypeId = Convert.ToInt32(dtrow["vartypeid"].ToString());
                    pp.variableLabelId = Convert.ToInt32(dtrow["mdmvariableLabelId"].ToString());
                    pp.variableId = Convert.ToInt32(dtrow["mdmvarid"].ToString());
                    pp.VariableName = dtrow["VariableName"].ToString();
                    pp.ParentId = Convert.ToInt32(dtrow["ParentId"].ToString());
                    pp.IsDeleted = Convert.ToInt32(dtrow["isdeleted"].ToString());
                    lstPpTreeView.Add(pp);
                    }
                }


            return lstPpTreeView;

            }


        public static IList<mdmVariableMdType> GetAllofMdType()
        {

            DbContextFactory Context = new DbContextFactory();
            Repo<NTTData.AMOPortal.Domain.Entities.mdmVariableMdType> mdType = new Repo<NTTData.AMOPortal.Domain.Entities.mdmVariableMdType>(Context);
            List<NTTData.AMOPortal.Domain.Entities.mdmVariableMdType> mdTypeList = mdType.GetAll().OrderBy(m => m.TypeName).ToList();
            return mdTypeList;


        }

        public static mdmVariableLabel GetLabelFor(mdmVariable variable)
        {
            using (var context = new DbContextFactory())
            {
                var labelRepo = new Repo<mdmVariableLabel>(context);
                mdmVariableLabel label = labelRepo.FirstOrDefault(v => v.Id == variable.VariableLabelId);
                if (label != null)
                    return label;
            }
            return new mdmVariableLabel();
        }

        public static IList<mdmVariable> GetVariablesInCategory(int categoryId)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<mdmVariable>(context);
                return varRepo.Where(v => v.VariableCatId == categoryId).ToList();
            }
        }


        public static String GetVariableName(int variableId)
            {

            using (var context = new DbContextFactory())
                {
                    vwVariableVariableLabel vwvarLabel = new Repo<vwVariableVariableLabel>(context).Where(v => (v.VariableId == variableId)).FirstOrDefault();
                    if (vwvarLabel != null)
                    {
                    return (vwvarLabel.VariableLabel);    
                    }
                    else
                            {
                            return("");
                            }
                           

                }


            }



        public static IList<mdmVariable> GetVariablesForClientId(int ClientId, int TypeId, int categoryId)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<mdmVariable>(context);
                return varRepo.Where(v => (v.VariableCatId == categoryId && v.ClientId == ClientId && v.VariableTypeId == TypeId)).ToList();
            }
        }


        public static IList<mdmVariable> GetVariablesForClientId(int ClientId)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<mdmVariable>(context);
                return varRepo.Where(v => (v.ClientId == ClientId)).ToList();
            }
        }



        public static IEnumerable<SelectListItem> GetAllVariableCategory()
        {
            using (var context = new DbContextFactory())
            {

                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.mdmVariableCategory> varCategory = new Repo<mdmVariableCategory>(Context);
                List<NTTData.AMOPortal.Domain.Entities.mdmVariableCategory> varcatList = varCategory.GetAll().ToList();
                var Items = varcatList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.VariableCategory
                }).ToList();

                //Items.Insert(Items.Count, new SelectListItem { Text = "Create New Chain", Value = "0" });
                return Items;

            }
        }



        public static IEnumerable<SelectListItem> GetAllParentsForClientAndValueChainType(int clientId, int valuechainTypeId)
        {
            DbContextFactory Context = new DbContextFactory();
            Repo<NTTData.AMOPortal.Domain.Entities.mdmVariableCategory> varCategory = new Repo<mdmVariableCategory>(Context);
            //List<NTTData.AMOPortal.Domain.Entities.mdmVariableCategory> 
            var Items = varCategory.GetAll().ToList().Select(
           CList => new SelectListItem
           {
               Value = CList.Id.ToString(),
               Text = CList.VariableCategory
           });

            //var Items = varcatList.Select(CList => new SelectListItem
            //{
            //    Value = CList.Id.ToString(),
            //    Text = CList.VariableCategory
            //});
            return Items;

        }





        public static IEnumerable<SelectListItem> GetAllVariableTypes()
        {
            using (var context = new DbContextFactory())
            {

                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.mdmVariableType> varTypes = new Repo<mdmVariableType>(Context);
                List<NTTData.AMOPortal.Domain.Entities.mdmVariableType> varTypeList = varTypes.GetAll().ToList();
                var Items = varTypeList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.VariableType
                });
                return Items;

            }






        }


        public static mdmVariableLabel GetVariableLabel(int variableLabelId)
            {
            return GetById<mdmVariableLabel>(variableLabelId);
            }

        public static mdmVariableCategory GetVariableCategory(int categoryId)
        {
            return GetById<mdmVariableCategory>(categoryId);
        }

        //public static Variable GetFirstVariableOrNew()
        //{
        //    using (var context = new DbContextFactory())
        //    {
        //        var varRepo = new Repo<Variable>(context);
        //        Variable variable = varRepo.GetAll().FirstOrDefault(v => v.VariableType.ParentVariableTypeId == null);
        //        if (variable != null)
        //            return variable;
        //        return GetDefaultNewVariable(varRepo);
        //    }
        //}

        private static mdmVariable GetDefaultNewVariable(Repo<mdmVariable> repo)
        {
            var variable = new mdmVariable
            {
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                VariableTypeId = _variableTypes[VariableTypeEnum.ValueChain]
            };
            mdmVariable result = repo.Insert(variable);
            repo.Save();
            SetLabelValuesFor(result.Id, "New Value Chain", "this is the default value chain.");
            return result;
        }

        public static void SetLabelValuesFor(int variableId, string name, string description)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<mdmVariable>(context);
                var labelRepo = new Repo<mdmVariableLabel>(context);
                mdmVariable variable = varRepo.Get(variableId);
                if (variable != null)
                {
                    //mdmVariableLabel label = variable.VariableLabelId.HasValue
                    //                             ? labelRepo.Get(variable.VariableLabelId.Value)
                    //                             : GetNewLabel(labelRepo, variable);
                    //label.VariableLabel = name;
                    //label.FullDescription = description;
                }
                labelRepo.Save();
                varRepo.Save();
            }
        }

        private static mdmVariableLabel GetNewLabel(Repo<mdmVariableLabel> repo, mdmVariable variable)
        {
            var label = new mdmVariableLabel();
            var result = repo.Insert(label);
            variable.VariableLabelId = result.Id;
            return result;
        }

        //public static Variable AddNewVariableChild(int? variableId = null)
        //{
        //    using (var context = new DbContextFactory())
        //    {
        //        var varRepo = new Repo<Variable>(context);
        //        var newVar = new Variable();
        //        Variable result = varRepo.Insert(newVar);
        //        if(variableId.HasValue)
        //            result.VariableTypeId = GetChildType(GetVariable(variableId.Value).VariableTypeId);
        //        varRepo.Save();
        //        AddChild(variableId.GetValueOrDefault(0), result.Id);
        //        return result;
        //    }
        //}

        //private static int GetChildType(int? typeId)
        //{
        //    using (var context = new DbContextFactory())
        //    {
        //        var repo = new Repo<VariableType>(context);
        //        var variableType = repo.FirstOrDefault(t => t.ParentVariableTypeId == typeId);
        //        return variableType != null ? variableType.Id : 0;
        //    }
        //}

        public static IList<mdmVariableType> GetRootLevelItems()
        {
            using (var context = new DbContextFactory())
            {
                var variablesRepo = new Repo<mdmVariableType>(context);
                return variablesRepo.GetAll().ToList();
            }
        }

        public static List<mdmVariableType> GetRootLevelTypes()
        {
            using (var context = new DbContextFactory())
            {
                var variablesRepo = new Repo<mdmVariableType>(context);
                return variablesRepo.GetAll().ToList();
            }
        }


        public static List<mdmVariable> GetMdmVariables()
            {
            using (var context = new DbContextFactory())
                {
                var variablesRepo = new Repo<mdmVariable>(context);
                return variablesRepo.GetAll().ToList();
                }
            }



        public static IEnumerable<SelectListItem> GetVariableTypes()
            {
            using (var context = new DbContextFactory())
                {

                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.mdmVariableType> varRegRepo = new Repo<mdmVariableType>(Context);
                List<NTTData.AMOPortal.Domain.Entities.mdmVariableType> varRegList = varRegRepo.Where(c => c.IsDeleted == false).ToList();
                var Items = varRegList.Select(CList => new SelectListItem
                {
                    Value = CList.VariableType,
                    Text = CList.VariableType
                }).ToList();

                Items.Insert(0, new SelectListItem { Text = "", Value = "0" });
                return Items;
                }
            }
        public static List<vwmdmvartypes> GetRootLevelTypesForDependInfo()
        {
            using (var context = new DbContextFactory())
            {
                var variablesRepo = new Repo<vwmdmvartypes>(context);
                return variablesRepo.GetAll().ToList();
            }
        }



        public static List<mdmVariableCategory> GetCatTypes()
        {
            using (var context = new DbContextFactory())
            {
                var variablesRepo = new Repo<mdmVariableCategory>(context);
                return variablesRepo.GetAll().ToList();
            }
        }



        public static List<vwmdmvartypes> GetCatTypesForDependInfo()
        {
            using (var context = new DbContextFactory())
            {
                var variablesRepo = new Repo<vwmdmvartypes>(context);
                return variablesRepo.GetAll().ToList();
            }
        }



        public static IList<mdmVariable> GetChildren(int variableId)
        {
            using (var context = new DbContextFactory())
            {
                var relationshipRepo = new Repo<mdmVariableREL>(context);
                return GetChildren(relationshipRepo, variableId);
            }
        }


        public static IList<vwVariableVariableLabel> GetVWChildren(int variableId)
        {
            using (var context = new DbContextFactory())
            {
                var relationshipRepo = new Repo<mdmVariableREL>(context);
                return GetVWChildren(relationshipRepo, variableId);
            }
        }


        public static IList<vwVariableVariableLabel> GetVWParents(int variableId)
        {
            using (var context = new DbContextFactory())
            {
                var relationshipRepo = new Repo<mdmVariableREL>(context);
                return GetVWParents(relationshipRepo, variableId);
            }
        }


        public static IList<VwMDMrelationwithlabel> GetVWChildrenForDependInfo(int variableId)
        {
            using (var context = new DbContextFactory())
            {
                var relationshipRepo = new Repo<mdmVariableREL>(context);
                return GetVWChildrenForDependInfo(relationshipRepo, variableId);
            }
        }




        private static IList<VwMDMrelationwithlabel> GetVWChildrenForDependInfo(Repo<mdmVariableREL> relRepo, int variableId)
        {
            var items = relRepo.Where(r => r.RelationshipVariableId1 == variableId && (r.RelationshipTypeId == 1 || r.RelationshipTypeId == 3)).OrderBy(r => r.DisplayOrder).ToList();
            List<VwMDMrelationwithlabel> lst = new List<VwMDMrelationwithlabel>();
            var context = new DbContextFactory();
            var varRepo = new Repo<VwMDMrelationwithlabel>(context);
            using (context)
            {
                foreach (var item in items)
                {
                    int relId = item.RelationshipVariableId2 == null ? 0 : Convert.ToInt32(item.RelationshipVariableId2);
                    lst.Add(varRepo.GetAll().Where(v => (v.fkVariableLabelId == relId)).FirstOrDefault());
                }
            }
            return lst;
        }




        private static IList<vwVariableVariableLabel> GetVWChildren(Repo<mdmVariableREL> relRepo, int variableId)
        {
            var items = relRepo.Where(r => r.RelationshipVariableId1 == variableId && (r.RelationshipTypeId == 2 || r.RelationshipTypeId == 4)).OrderBy(r => r.DisplayOrder).ToList();
            List<vwVariableVariableLabel> lst = new List<vwVariableVariableLabel>();
            var context = new DbContextFactory();
            var varRepo = new Repo<vwVariableVariableLabel>(context);
            using (context)
            {
                foreach (var item in items)
                {
                    int relId = item.RelationshipVariableId2 == null ? 0 : Convert.ToInt32(item.RelationshipVariableId2);
                    lst.Add(varRepo.GetAll().Where(v => (v.fkVariableLabelId == relId)).FirstOrDefault());
                }
            }
            return lst;
        }



        private static IList<vwVariableVariableLabel> GetVWParents(Repo<mdmVariableREL> relRepo, int variableId)
        {
            var items = relRepo.Where(r => r.RelationshipVariableId1 == variableId && (r.RelationshipTypeId == 3 || r.RelationshipTypeId == 1)).OrderBy(r => r.DisplayOrder).ToList();
            List<vwVariableVariableLabel> lst = new List<vwVariableVariableLabel>();
            var context = new DbContextFactory();
            var varRepo = new Repo<vwVariableVariableLabel>(context);
            using (context)
            {
                foreach (var item in items)
                {
                    int relId = item.RelationshipVariableId2 == null ? 0 : Convert.ToInt32(item.RelationshipVariableId2);
                    lst.Add(varRepo.GetAll().Where(v => (v.fkVariableLabelId == relId)).FirstOrDefault());
                }
            }
            return lst;
        }


        private static IList<mdmVariable> GetChildren(Repo<mdmVariableREL> relRepo, int variableId)
        {
            int parentTypeId = _relationshipTypes[RelationshipTypeEnum.Variable_Parent];
            var items = relRepo.Where(r => r.RelationshipVariableId1 == variableId && r.RelationshipTypeId == parentTypeId).OrderBy(r => r.DisplayOrder).ToList();
            return items.Select(r => GetVariable(r.RelationshipVariableId2.GetValueOrDefault())).ToList();


        }

        public static void AddChild(int parentId, int childId)
        {
            using (var context = new DbContextFactory())
            {
                var relationshipRepo = new Repo<mdmVariableREL>(context);
                AddParentChildRelationships(relationshipRepo, parentId, childId);
                AddSiblingRelationships(relationshipRepo, parentId, childId);
                relationshipRepo.Save();
            }
        }

        //public static int GetParentTypeId(int typeId)
        //{
        //    var type = GetById<VariableType>(typeId);
        //    if (type != null)
        //        return type.ParentVariableTypeId.GetValueOrDefault(-1);
        //    return -1;
        //}

        private static void AddSiblingRelationships(Repo<mdmVariableREL> relationshipRepo, int parentId, int childId)
        {
            IList<mdmVariable> otherChildren = GetChildren(parentId);
            foreach (mdmVariable otherChild in otherChildren)
            {
                AddRelationship(relationshipRepo, otherChild.Id, childId, RelationshipTypeEnum.Sibling_Child);
                AddRelationship(relationshipRepo, childId, otherChild.Id, RelationshipTypeEnum.Sibling_Child);
            }
        }

        private static void AddParentChildRelationships(Repo<mdmVariableREL> relationshipRepo, int parentId, int childId)
        {
            AddRelationship(relationshipRepo, parentId, childId, RelationshipTypeEnum.Variable_Parent);
            AddRelationship(relationshipRepo, childId, parentId, RelationshipTypeEnum.Variable_Child);
        }

        private static void AddRelationship(Repo<mdmVariableREL> relationshipRepo, int leftId, int rightId, RelationshipTypeEnum type)
        {
            if (!_relationshipTypes.ContainsKey(type))
                throw new ArgumentException(String.Format("The type: '{0}' does not exist in the database.", type.ToString()), "type");
            var relationship = new mdmVariableREL
            {
                RelationshipTypeId = _relationshipTypes[type],
                RelationshipVariableId1 = leftId,
                RelationshipVariableId2 = rightId,
                IsDeleted = false
            };
            relationship = relationshipRepo.Insert(relationship);
            relationshipRepo.Save();
        }

        public static TEntity New<TEntity>()
            where TEntity : Entity, new()
        {
            using (var context = new DbContextFactory())
            {
                var repo = new Repo<TEntity>(context);
                var newObj = new TEntity();
                TEntity result = repo.Insert(newObj);
                return result;
            }
        }

        // Added by Neeraj starts
        public static IList<mdmVariable> GetVariablesForClient(int ClientId, int TypeId, int categoryId, int varLblId)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<mdmVariable>(context);
                return varRepo.Where(v => (v.VariableCatId == categoryId && v.ClientId == ClientId && v.VariableTypeId == TypeId && v.VariableLabelId == varLblId && v.IsDeleted == false)).ToList();
            }
        }

        public static IList<mdmVariableMD> GetVariableItem(int VariableID, int metadataId)
        {
            DbContextFactory Context = new DbContextFactory();
            Repo<NTTData.AMOPortal.Domain.Entities.mdmVariableMD> mdType = new Repo<NTTData.AMOPortal.Domain.Entities.mdmVariableMD>(Context);
            List<NTTData.AMOPortal.Domain.Entities.mdmVariableMD> mdTypeList = mdType.GetAll().Where(m => m.VariableID == VariableID && m.MetaDataData == metadataId).ToList();
            if (mdTypeList.Count > 0)
            {
                return mdTypeList;
            }
            else
            {
                List<mdmVariableMD> list = new List<mdmVariableMD>();
                list.Add(new mdmVariableMD());
                return list;
            }
        }

        public mdmVariableREL GetVariableRel(int VariableLabelId)
        {
            using (Db db = new Db())
            {
                mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == VariableLabelId && c.IsDeleted == false));
                return existingVariableLabel;
            }
        }


        public static vwVariableVariableLabel GetVWVariableLabel(int VariableId)
        {

        using (var context = new DbContextFactory())
            return new Repo<vwVariableVariableLabel>(context).Where(v => (v.VariableId == VariableId)).FirstOrDefault();
            
         }

        public static IList<vwVariableVariableLabel> EditGetAllTypes(int clientId, int variableId)
        {
            using (var context = new DbContextFactory())
                return new Repo<vwVariableVariableLabel>(context).Where(v => (v.ClientId == clientId && v.VariableId == variableId)).ToList();
        }

        public static IList<mdmVariableREL> GetAllRelTypes(int varLabelId)
        {
            using (var context = new DbContextFactory())
                return new Repo<mdmVariableREL>(context).Where(v => (v.RelationshipVariableId1 == varLabelId && v.IsDeleted == false)).ToList();
        }
        // Added by Neeraj ends

        //Added for View Trigger
        public static IList<mdmVariableMdType> GetvwTrigMdType()
        {

            DbContextFactory Context = new DbContextFactory();
            Repo<NTTData.AMOPortal.Domain.Entities.mdmVariableMdType> mdType = new Repo<NTTData.AMOPortal.Domain.Entities.mdmVariableMdType>(Context);
            List<NTTData.AMOPortal.Domain.Entities.mdmVariableMdType> mdTypeList = mdType.GetAll().Where(m => m.TypeName.Contains("View Trigger")).OrderBy(m => m.TypeName).ToList();
            return mdTypeList;


        }
        //Added for View Trigger
        public static IList<mdmVariableMdType> GetNonvwTrigMdType()
        {

            DbContextFactory Context = new DbContextFactory();
            Repo<NTTData.AMOPortal.Domain.Entities.mdmVariableMdType> mdType = new Repo<NTTData.AMOPortal.Domain.Entities.mdmVariableMdType>(Context);
            List<NTTData.AMOPortal.Domain.Entities.mdmVariableMdType> mdTypeL = mdType.GetAll().ToList();
            List<NTTData.AMOPortal.Domain.Entities.mdmVariableMdType> mdvwTypeList = mdType.GetAll().Where(m => m.TypeName.Contains("View Trigger")).OrderBy(m => m.TypeName).ToList();
            List<NTTData.AMOPortal.Domain.Entities.mdmVariableMdType> mdnvwTypeList = mdTypeL.Except(mdvwTypeList).OrderBy(m => m.TypeName).ToList();
            return mdnvwTypeList;


        }

    }
}