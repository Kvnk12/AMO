using System.Collections.Generic;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;
using WebUI.Models;
using System.Web.Mvc;
using NTTData.AMOPortal.WebUI.Dto;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.AlignmentManager.Models
{
    public class VariableModel : AMOModelBase
    {
        [Display(Name = "Client Name")]
        public int ClientId { get; set; }
        [Display(Name = "Chain Name")]
        public int VarCatId { get; set; }

        public IEnumerable<SelectListItem> ClientList
            {
            get
                {
                return GetDropDownListItems.ClientList;
                }
            }

        public VariableModel() { }

        public VariableModel(int id)
            {
            InitializeValues(AlignmentHelper.GetVariable(id));
            }

        public VariableModel(mdmVariable variable)
            {
            InitializeValues(variable);
            }

        private void InitializeValues(mdmVariable variable)
            {
            if (variable != null)
                {
                Variable = variable;
                VariableId = variable.Id;
                mdmVariableLabel label = AlignmentHelper.GetLabelFor(variable);
                Name = label.VariableLabel;
                Description = label.FullDescription;
                //ParentTypeId = AlignmentHelper.GetParentTypeId(variable.VariableTypeId.GetValueOrDefault(0));
                //TypeId = variable.VariableTypeId.GetValueOrDefault();
                }
            }

        //public List<VariableType> GetRootLevelTypes()
        //{
        //    return AlignmentHelper.GetRootLevelTypes();
        //}

        public IList<vwVariableVariableLabel> GetByType(int typeId, int clientId, int varLabelId)
        {
            return AlignmentHelper.GetAllOfType(typeId, clientId, varLabelId);
        }

        //public IList<Variable> GetRootLevelItems()
        //{
        //    return AlignmentHelper.GetRootLevelItems();
        //}

        public IList<mdmVariable> GetChildren()
        {
            return AlignmentHelper.GetChildren(VariableId);
        }

        public IEnumerable<SelectListItem> GetCategories
        {
            get
                {
                return AlignmentHelper.GetAllVariableCategory(); 
                }

        
        }

        public int ParentTypeId { get; set; }

        public int ParentId { get; set; }

        public mdmVariable Variable { get; set; }

        [Required()]
        public int VariableId { get; set; }

        [Required()]
        public int TypeId { get; set; }

        public int VariableLabelId { get; set; }
        [Display(Name = "Value Chain Name")]
        [Required(ErrorMessage="Select Value Chain Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }

    }
}