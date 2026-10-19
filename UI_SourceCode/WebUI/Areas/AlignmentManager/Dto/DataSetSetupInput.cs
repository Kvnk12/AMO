using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.AlignmentManager.Objects;

namespace NTTData.AMOPortal.WebUI.Dto
    {
    public class DataSetupInput : Input
    {

        public DataSetupInput()
        { }

        public DataSetupInput(int ClientId)
            {
            _clientId = ClientId;
            }

        public int ClientId
        {
            get { return _clientId; }
            set
            { _clientId = value;}

        }


        public string regDataSetStatus { get; set; }
        
        public int DataSetId
        { 
    
            get 
                {
                return _dataSetId; 
                }
            set
            { 
                _dataSetId = value;
            }
            
        }
        private int _clientId = 0;
        private int _dataSetId = 0;


        public RegisterDatasetInput registerDataSetInput
            {
            get;
            set;

            }

      //public RegisterDatasetInput registerDataSetInput
      //      {
      //      get;
      //      set;

      //      }


        public IList<RegisterDatasetMap> getRegDataSetMap
        {
        get
            {
            return AlignmentHelper.GetRegisterDataSetMap(DataSetId);
            }
        }

        public IList<vwRegDataSet> RegDataSets
            {
            get
                {
                return AlignmentHelper.GetVwRegDataSet(_clientId);
                }
            }


        public IEnumerable<SelectListItem> ClientList
            {
            get
                {
                return GetDropDownListItems.ClientList;
                }
            }

    

    }

    }