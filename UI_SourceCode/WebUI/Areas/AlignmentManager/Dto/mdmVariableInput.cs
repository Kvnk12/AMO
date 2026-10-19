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
    public class mdmVariableInput : Input
    {
        public int ClientId { get; set; }

        [Column("fkVariableLabelId")]
        public int VariableLabelId { get; set; }

        [Column("fkVariableTypeId")]
        public int VariableTypeId { get; set; }

        [Column("fkVariableCategoryId")]
        public int VariableCatId { get; set; }

        public string CreatedByUser { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastUpdatedUser { get; set; }

        public DateTime LastUpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}