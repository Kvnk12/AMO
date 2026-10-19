using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;
using System.Data;
using WebUI.Areas.AlignmentManager.Dto;

namespace NTTData.AMOPortal.WebUI.Dto
    {
    public class ALMTreeMenuInput 
        {

        //Get Root level Items
        public List<mdmVariableType> GetRootLevelTypes()
            {
            return AlignmentHelper.GetRootLevelTypes();
            }

        public List<vwmdmvartypes> GetRootLevelTypesForDependInfo(int clientId, int variableId)
            {
             if(variableId > 0)

             return AlignmentHelper.GetRootLevelTypesForDependInfo().Where(c => c.ClientId == clientId).ToList();
             else
            return AlignmentHelper.GetRootLevelTypesForDependInfo().Where( c => 1 == 2).ToList();

            //foreach (VwMDMrelationwithlabel item in lstSource)
            //{
            //    string varType = item.VariableType;
            //    if (lst.FindAll(c => c.VariableType == varType).Count == 0)
            //    {
            //        lst.Add(item);
            //    }
            //}
            //return lst;

            }

        public List<vwmdmvartypes> GetCatTypesForDependInfo(int clientId, int variableId)
            {
            if(variableId > 0)
            return AlignmentHelper.GetCatTypesForDependInfo().Where(c => c.ClientId == clientId).ToList();
            else
                return AlignmentHelper.GetCatTypesForDependInfo().Where(c => 1==2).ToList();
            }

        public List<mdmVariableCategory> GetCatTypes()
            {
            return AlignmentHelper.GetCatTypes();
            }

        public IList<vwVariableVariableLabel> GetByType(int typeId, int clientId)
            {
            return AlignmentHelper.GetAllOfType(typeId).Where(c => c.ClientId == clientId).ToList();
            
            }

        public IList<vwVariableVariableLabel> GetByTypeByCat(int typeId, int catId, int clientId)
            {
            return AlignmentHelper.GetAllOfType(typeId).Where(c => c.ClientId == clientId && c.fkVariableCategoryId == catId).ToList();
            }


        public IList<vwVariableVariableLabel> GetByTypeByCat(int typeId, int catId, int clientId, int fkVariableId)
            {
            return AlignmentHelper.GetAllOfType(typeId).Where(c => c.ClientId == clientId && c.fkVariableCategoryId == catId && c.fkVariableLabelId == fkVariableId).ToList();
            }



        public IList<VwMDMrelationwithlabel> GetByTypeByCatForDependInfo(int typeId, int catId, int clientId, int variableId, int variableLabelId =0)
            {
            mdmVariable mdmVariable = AlignmentHelper.GetVariable(variableId);    

            //return AlignmentHelper.GetAllOfTypeForDependInfo(typeId).Where(c => c.ClientId == clientId && c.fkVariableCategoryId == catId  && c.fkVariableLabelId == (mdmVariable == null?0:mdmVariable.VariableLabelId)).OrderBy(c=> c.Id).ToList();
            return AlignmentHelper.GetAllOfTypeForDependInfo(typeId).Where(c => c.ClientId == clientId && c.fkVariableCategoryId == catId && c.Id == (mdmVariable == null ? 0 : mdmVariable.Id) && c.fkVariableLabelId == variableLabelId).OrderBy(c => c.Id).ToList();

            }



        public ALMTreeMenuInput(int id, int clientId =0)
        {
        ClientId = clientId;
        VariableId = id;
        InitializeValues(AlignmentHelper.GetvwVariable(id));
         
        }


        //public ALMTreeMenuInput(int varTypeid, int clientId = 0, string varType="")
        //    {
        //    ClientId = clientId;
        //    var = id;
        //    //InitializeValues(AlignmentHelper.GetvwVariable(id));

        //    }


        public ALMTreeMenuInput(int clientId)
            {
            ClientId = clientId;
            }

        public ALMTreeMenuInput(int varTypeid, string vartype,int clientId)
            {
            ClientId = clientId;
            VariableTypeId = varTypeid; 
            }


       


        public ALMTreeMenuInput(VwMDMrelationwithlabel variable)
            {
            
                InitializeValuesforDepend(variable);

            }


        private void InitializeValuesforDepend(VwMDMrelationwithlabel variable)
            {
            if (variable != null)
                {
                VariableForDepend = variable;
                dependVariableId = variable.Id;
                //mdmVariableLabel label = AlignmentHelper.GetLabelFor(variable);
                Name = VariableForDepend.rel2VariableLable;
                //Description = label.FullDescription;
                ParentTypeId = variable.fkVariableTypeId;
                //TypeId = variable.VariableTypeId.GetValueOrDefault();
                VariableLabelId = variable.fkVariableLabelId;
                ClientId = variable.ClientId;
                }
            }

        public ALMTreeMenuInput(vwVariableVariableLabel  variable) 
            {
            InitializeValues(variable);
                
            }



        public static vwVariableVariableLabel GetvwVariable(int id)
            {
            return AlignmentHelper.GetById<vwVariableVariableLabel>(id);
            }


        private void InitializeValues(vwVariableVariableLabel variable)
            {
            if (variable != null)
                {
                Variable = variable;
                VariableId = variable.Id;
                //mdmVariableLabel label = AlignmentHelper.GetLabelFor(variable);
                Name = variable.VariableLabel; 
                //Description = label.FullDescription;
                ParentTypeId = variable.fkVariableTypeId;
                //TypeId = variable.VariableTypeId.GetValueOrDefault();
                VariableLabelId = variable.fkVariableLabelId;
                ClientId = variable.ClientId;
                }
            }


        public int ParentTypeId { get; set; }

        public int ParentId { get; set; }

        public vwVariableVariableLabel Variable { get; set; }

        public VwMDMrelationwithlabel VariableForDepend { get; set; }

        public int dependVariableId { get; set; }

        public int VariableId { get; set; }

        public int VariableTypeId { get; set; }

        

        public int VariableLabelId { get; set; }

        public string Name { get; set; }
        public int ClientId { get; set; }


        //public PPTreeView PPTreeViewRoot
        //    {
        //    get
        //        {
        //        IList<PPTreeView> AllTreeViewItems = AlignmentHelper.GetTreeViewItems(this.ClientId);
        //        PPTreeView root = AllTreeViewItems.Where(c => c.ParentId == 0).FirstOrDefault();
        //        SetChildren(root, AllTreeViewItems);
        //        return root;
        //        }

        //    }


        //private void SetChildren(PPTreeView ppItem, IList<PPTreeView> list)
        //    {
        //    var children = list.Where(c => c.ParentId == ppItem.Id).ToList();
        //    if (children.Count > 0)
        //        {
        //        foreach (var child in children)
        //            {
        //            SetChildren(child, list);
        //            ppItem.TreeViewItems.Add(child);
        //            }
        //        }

        //    }




        }
    }