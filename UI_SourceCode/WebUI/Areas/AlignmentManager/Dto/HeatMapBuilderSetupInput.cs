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
    public class HeatMapBuilderSetupInput : Input
    {

        public HeatMapBuilderSetupInput()
        { }

        public HeatMapBuilderSetupInput(int ClientId)
            {
            _clientId = ClientId;
            }

        public int ClientId
        {
            get { return _clientId; }
            set
            { _clientId = value;}

        }


        public string HeatMapReportStatus { get; set; }

        public int ReportId
            {

            get
                {
                return _reportId;
                }
            set
                {
                _reportId = value;
                }

            }
        private int _clientId = 0;
        private int _reportId = 0;


        public ReportConfiguationInput reportConfiguationInput
            {
            get;
            set;

            }

        public ReportAlgorithmMDInput reportAlgorithmMDInput
            {
            get;
            set;
            }


        public ReportAlgorithmInput reportAlgorithmInput
            {
            get;
            set;
            }


        //public ReportGraphConfigurationInput reportGraphConfigurationInput { get; set; }
        


        public ReportFilterInput reportFilterInput
        { get; set; }


        public ReportThresholdInput reportThresholdInput
        { get; set; }

     
        public IList<ReportConfiguration> heatMapReports
            {
            get
                {
                return AlignmentHelper.GetHeatMapReports(_clientId);
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