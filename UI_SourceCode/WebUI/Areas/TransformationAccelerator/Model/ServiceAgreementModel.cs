using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.TransformationAccelerator.Model
{
    public class ServiceAgreementModel
    {
        public int Id { get; set; }
        public string ImprovementTypeId { get; set; }
        public decimal Goal { get; set; }
        public string impNotes { get; set; }
        public int? ClientId { get; set; }

        public string ImprovementType { get; set; }
    }
}