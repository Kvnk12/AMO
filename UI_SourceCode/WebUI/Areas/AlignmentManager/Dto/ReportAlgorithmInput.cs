using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;


namespace NTTData.AMOPortal.WebUI.Dto
    {

    public class ReportAlgorithmInput : Input
        {


        public int ReportConfigId { get; set; }
        public string AlgorithmName { get; set; }
        public string AlgorithmSolution { get; set; }
        public string key { get; set; }
        public string Values { get; set; }
        public string NewAlgorithmSolution { get; set; }

        public string Partitionkey { get; set; }
        public string PartitionValues { get; set; }

        public string PartitionDatekey { get; set; }
        public string PartitionDateValues { get; set; }

        public string SelectedColumns { get; set; }
        public string SelectedPartionByColumns { get; set; }
        public string SelectedPartionDateColumns { get; set; }




        public ReportGraphConfigurationInput reportGraphConfigurationInput { get; set; }
        public ReportConfigurationMDInput reportConfigurationMDInput { get; set; }

        
        public List<ReportDefnKeyInput> GetReportDefnKeys(int ReportId, string AlogorithmSolution="")
        {
        IList<ReportalgorithmDefn> defnList = AlignmentHelper.GetReportalgorithmDefnItems(AlogorithmSolution);
        ReportAlgorithm reportAlogrithm = AlignmentHelper.GetReportAlgorithm(ReportId);
        IList<ReportAlgorithmMDKey> ReportAlgorithmMDKeys = new List<ReportAlgorithmMDKey>();
        if (reportAlogrithm != null)
        {
            IList<ReportAlgorithmMD> lstAlgorMd = AlignmentHelper.GetAlogMdList(reportAlogrithm.Id);
            ReportAlgorithmMDKeys = lstAlgorMd.Select(clist => new ReportAlgorithmMDKey { Value = clist.Value, KeyNames = clist.KeyNames}).ToList();
        }

        IList<ReportAlgorithmStageMD> stgMdList = null;
         if(reportAlogrithm != null)
         stgMdList    = AlignmentHelper.GetReportAlgorithmStageMd(reportAlogrithm.Id);

        //string[] KeyNames  = new string[0];
        //string[] KeyValues = new string[0]; 
        //foreach (ReportAlgorithmMDKey item in ReportAlgorithmMDKeys)
        //    {
        //    KeyNames = item.KeyNames.Split(',').ToArray();
        //    KeyValues = item.Value.Split(',').ToArray();
        //    //KeyNames.AddRange(item.KeyNames.Split(',').ToList());
        //    //KeyValues.AddRange(item.Value.Split(',').ToList());
        //    }


      
            
            

           List<ReportDefnKeyInput> defnKeyInputList = new List<ReportDefnKeyInput>();
            foreach(ReportalgorithmDefn item in defnList)
                {

                ReportDefnKeyInput keyInput = new ReportDefnKeyInput();
                //ReportAlgorithmMDKey mdkey = ReportAlgorithmMDKeys.FirstOrDefault(c => (c.Key == item.KeyName));
                //if (mdkey != null)
                //    {
                //    string[] keys = mdkey.KeyNames.Split(',').ToArray();
                //    string[] values = mdkey.Value.Split(',').ToArray();
                //    if (keys.Contains(item.DisplayName))
                //        {
                //        keyInput.AssignedValue = values[Array.IndexOf(keys, item.DisplayName)];
                //        }
                //    }
                
                //if (KeyNames.Contains(item.DisplayName))
                //{
                //int index = Array.IndexOf(KeyNames, item.DisplayName);
                //keyInput.AssignedValue = KeyValues[index]; 
                //    //keyInput.AssignedValue = item.DisplayName;
                //     //keyInput.AssignedValue = KeyValues([KeyNames.FindIndex(item.DisplayName)];
                //}

                if (stgMdList != null &&  stgMdList.Where(c => c.key == item.DisplayName).FirstOrDefault() != null)
                    {
                    keyInput.AssignedValue = stgMdList.Where(c => c.key == item.DisplayName).FirstOrDefault().Value;
                    }
                else
                {
                keyInput.AssignedValue = "";
                    }
                
                keyInput.AlgorithmName = AlgorithmName;
                keyInput.AlgorithmSolution = "";
                keyInput.DisplayName = item.DisplayName;
                keyInput.Order = Convert.ToInt32(item.Order);
                keyInput.Type = item.Type;
                keyInput.ReportConfigId = this.ReportConfigId;
                keyInput.PredefinedValues = item.PredefinedValues;
                defnKeyInputList.Add(keyInput);
                  
                }

            return defnKeyInputList;

        }


        public IList<SelectListItem> GetAlgorithms()
            {

            return AlignmentHelper.GetAlgorithms();

            }


        public IList<SelectListItem> GetDefnAlgorithms(string selectedItemValue="")
            {

            return AlignmentHelper.GetDefnAlgorithms(selectedItemValue);

            }



        //public IList<SelectListItem> GetColumnsForAlgorithm(int ReportConfigId = 0, string keyName="")
        //    {

        //    //Get Variable Types for  Client Id (This is 

        //    IList<SelectListItem> AvailableColumns = AlignmentHelper.GetColumns(ReportConfigId);
        //    IList<SelectListItem> lst = GetValuesforReportKey(ReportConfigId, keyName);
        //    foreach(SelectListItem  lstItem in lst)
        //    {
        //    var itemToRemove = AvailableColumns.SingleOrDefault(r => r.Text == lstItem.Text);
        //    AvailableColumns.Remove(itemToRemove);
        //    }
        //    return AvailableColumns;

        //    }


        public IList<SelectListItem> GetColumnsForAlgorithm(int ClientId, int ReportConfigId = 0, string SelectedColumn ="", string DisplayName="", string Type="", string PredefinedValues ="")
            {

            //Get Variable Types for  Client Id (This is 
            IList<SelectListItem> AvailableColumns =  new List<SelectListItem>();
            if(Type.ToLower()  != "predefinedvalue")
                {
                switch (DisplayName.ToLower())
                    {
                    case "rollup":
                        AvailableColumns = AlignmentHelper.GetMDMVariableTypes(ClientId, SelectedColumn);
                        break;
                    default:
                        AvailableColumns = AlignmentHelper.GetColumns(ReportConfigId, SelectedColumn);
                        break;
                    }
                }
                else
                    {

                        List<string> lst =  PredefinedValues.Split(',').ToList();

                         AvailableColumns = lst.Select(c => new SelectListItem { Text=c, Selected = (c == SelectedColumn), Value=c }).ToList();
                    }
            //IList<SelectListItem> lst = GetValuesforReportKey(ReportConfigId, keyName);
            //foreach (SelectListItem lstItem in lst)
            //    {
            //    var itemToRemove = AvailableColumns.SingleOrDefault(r => r.Text == lstItem.Text);
            //    AvailableColumns.Remove(itemToRemove);
            //    }
            return AvailableColumns;

            }

        //public IList<SelectListItem> GetColumnsForAlgorithm(int ReportConfigId = 0)
        //    {

        //    //Get Variable Types for  Client Id (This is 

        //    IList<SelectListItem> AvailableColumns = AlignmentHelper.GetColumns(ReportConfigId);
        //    //IList<SelectListItem> lst = GetValuesforReportKey(ReportConfigId, keyName);
        //    //foreach (SelectListItem lstItem in lst)
        //    //    {
        //    //    var itemToRemove = AvailableColumns.SingleOrDefault(r => r.Text == lstItem.Text);
        //    //    AvailableColumns.Remove(itemToRemove);
        //    //    }
        //    return AvailableColumns;

        //    }


        public IList<SelectListItem> GetChartTemplates()
            {

            return AlignmentHelper.GetChartTemplates();

            }


        public IList<SelectListItem> GetColumnsForTrendChart(int ReportConfigId = 0, int ClientId = 0)
            {

            //Get Variable Types for  Client Id (This is required for Charts, as per the functional requiredment from Todd' 

            //Date 25th Aug 2014

            //The design of these chart fields is they can be any column from the getreport output. 
            //It is not source table columns. 
            //The columns you are looking for would be created as output and are in fact Variable Types which getreport determines from the MDM and 
            //using the getParent recursive lookup. The results of this lookup contains varType which is then converted to column names, which are then joined to the output. 



            IList<SelectListItem> AvailableColumns = AlignmentHelper.GetColumns(ReportConfigId);
            IList<SelectListItem> AvailbaleVarTypes = AlignmentHelper.GetVariableTypes(ClientId);
            IList<SelectListItem> AvailableAlgorithms = AlignmentHelper.GetAlogirthmNames();
            List<SelectListItem> AllColumns = AvailableColumns.ToList().Union(AvailbaleVarTypes.ToList()).ToList();
            AllColumns =  AllColumns.Union(AvailableAlgorithms.ToList()).ToList();
            return AllColumns;

            }


        public IList<SelectListItem> GetValuesforReportKey(int ReportConfigId =0, string keyName="")
        {
            string  keyValues = "";
            //string keyName= "SelectedColumn";
            int AlgorithmId = AlignmentHelper.GetAlgorithmId(ReportConfigId);
            List<SelectListItem> selectList = new List<SelectListItem>();
            keyValues = AlignmentHelper.GetValueForKeyFromAlogorithm(AlgorithmId, keyName);
            if (keyValues != "")
                {
                List<string> listValues = keyValues.Split(new Char[] { ',' }).ToList();
                foreach (string item in listValues)
                    {
                    SelectListItem lstItem = new SelectListItem();
                    lstItem.Text = item.Trim();
                    lstItem.Value = item.Trim();
                    selectList.Add(lstItem);
                    }
                }
            return selectList;

        }
      
    

        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now; 
        
        public string LastUpdatedUser { get; set; }

        public DateTime? LastUpdatedDate { get { return _appDate; } set {   _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now ; } }


        }
    }
