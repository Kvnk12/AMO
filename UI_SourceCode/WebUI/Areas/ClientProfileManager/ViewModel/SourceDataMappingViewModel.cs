using Domain.Entities.ClientProfileManager;
using Domain.Entities.IncidentNavigator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class SourceDataMappingViewModel
    {
        public int Id { get; set; }

        [Required]
        public int MappingId { get; set; }

      //  [Required]
        [MaxLength(100)]
        public string SField { get; set; }

     //   [Required]
        [MaxLength(100)]
        public string MappingTable { get; set; }

        [MaxLength(100)]
        public string MappingCol { get; set; }

        [MaxLength(400)]
        public string MapColDesc { get; set; }
        
        public string DefVal { get; set; }

     //   [Required]
        [MaxLength(100)]
        public string DataType { get; set; }

        public int? DLength { get; set; }

        public bool IsMandatory { get; set; }

        [MaxLength(100)]
        public string DisplayName { get; set; }

        public int CreatedBy { get; set; }

        public string CreatedByUserFName { get; set; }

        string _CreatedByUserName="";
       public string CreatedByUserName
        {
            get
            {
                return CreatedByUserFName +" "+ CreatedByUserLName;
            }
            set
            {
                _CreatedByUserName = value;
            }
        }

        string _UpdatedByUserName = "";
        public string UpdatedByUserName
        {
            get
            {
                return UpdatedByUserFName +" "+ UpdatedByUserLName;
            }
            set
            {
                _UpdatedByUserName = value;
            }
        }

        public string CreatedByUserLName { get; set; }
        public string UpdatedByUserFName { get; set; }
        public string UpdatedByUserLName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int UpdatedBy { get; set; }

       // public string UpdatedByUserName { get; set; }

        public DateTime UpdatedOn { get; set; }

        public WebUI.Areas.ClientProfileManager.Dto.datConcatenationInput SFlds { get; set; }

        public string FieldConcatinations { get; set; }
        public string FieldCalculations { get; set; }

        ////public IList<SourceFieldConcat> SourceFieldConcats { get; set; }

        ////public IList<SourceFieldCalculate> SourceFieldCalculate { get; set; }
        ///

        public IList<SourceFieldConcatModel> SourceFieldConcats { get; set; }

        public IList<SourceFieldCalculateModel> SourceFieldCalculate { get; set; }

        public string FieldConcatinationsForDisplay { get; set; }
        public string FieldCalculationsForDisplay { get; set; }
        //public List<SelectListItem> SFlds { get; set; }

        public bool DataMasking { get; set; }
    }

    public class SourceFieldCalculateModel
    {
        public int Id { get; set; }
        public int SourceDataMappingId { get; set; }
        public string SourceField { get; set; }
        public string Operator { get; set; }
        public decimal? Constant { get; set; }


        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }

    public class SourceFieldConcatModel
    {
        public int Id { get; set; }

        public int SourceDataMappingId { get; set; }

        public int MappingId { get; set; }

        public string SourceField { get; set; }

        public string SeparatedBy { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }

}
