using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;


namespace NTTData.AMOPortal.WebUI.Dto
    {
    public class VariableTypesInput : Input
        {

        //public string VariableType { get; set; }

        public mdmVariableTypeInput mdmVariableTypeInput
            {
            get;
            set;
            }


        public mdmVariableCategoryInput mdmVariableCategoryInput
            {
            get;
            set;
            }


        public mdmVariableMdTypeInput mdmVariableMdTypeInput
            {
            get;
            set;
            }


        public int VarTypeId { get; set; }
        public int VarCatId { get; set; }
        public int MetaDataId { get; set; }

        public string GridToDisplay { get; set; }

        public string VarTypeCreate{ get; set; }
        public string VarCatCreate { get; set; }

        public string MetaDataCreate { get; set; }

        public List<mdmVariableCategory> CatTypes
        {
            get{
            return AlignmentHelper.GetCatTypes();
                }
        }



        public IList<mdmVariableMdType> MDTypes
            {
            get
                {
                return AlignmentHelper.GetAllofMdType();
                }
            }



        public IList<mdmVariableType> VariableTypes
            {
            get
                {
                return AlignmentHelper.GetRootLevelTypes();
                }
            }

      

        }
    }