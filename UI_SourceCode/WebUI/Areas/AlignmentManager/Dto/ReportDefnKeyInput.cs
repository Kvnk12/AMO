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


namespace NTTData.AMOPortal.WebUI.Dto
    {

    public class ReportDefnKeyInput : Input
        {

        public int ClientId { get; set; }
        public int ReportConfigId { get; set; }
        public string AlgorithmName { get; set; }
        public string AlgorithmSolution { get; set; }
        public string AssignedValue { get; set; }
        public string DisplayName { get; set; }
        public string KeyName { get; set; }
        public string PredefinedValues { get; set; }
        public int Order { get; set; }
        public string Type { get; set; }

        }

    }