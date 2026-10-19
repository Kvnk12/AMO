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
using Data;


//namespace WebUI.Areas.AlignmentManager.Dto
namespace NTTData.AMOPortal.WebUI.Dto
{
    public class VariableInput : Input //, IValidatableObject
    {


        [Display(Name = "Variable Type")]
        [Required(ErrorMessage = "Select Variable Type")]
        public int VariableTypeId { get; set; }



        [Display(Name = "Client Name")]
        public int ClientId { get; set; }

        //public int ParentTypeId { get; set; }

        //public int ParentId { get; set; }

        //public Variable Variable { get; set; }

        public int VariableId { get; set; }

        public vwVariableVariableLabel GetVariableDetails(int clientId, int variableId)
        {

            return AlignmentHelper.GetvwVariable(variableId);

        }


        public IList<vwMetaDataDetails> GetVariableMetaDataDetails(int clientId, int variableId)
        {

            return AlignmentHelper.GetvwMetaDetails(clientId, variableId);

        }


        [Required(ErrorMessage = "Select Variable Category")]
        public int VariableCatId { get; set; }
        public string VariableType { get; set; }

        public int VariableRelationShipId { get; set; }

        public string CreateVariableRelationShip { get; set; }


        //public int VariableTypeId { get; set; }


        public mdmVariableLabelInput variableLabelInput { get; set; }

        [Required(ErrorMessage = "Select Variable Category")]
        public int VariableLabelId { get; set; }

        //[Display(Name = "Value Chain Name")]
        //public string Name { get; set; }
        //[Display(Name = "Description")]
        //public string Description { get; set; }


        public int VarTypeId { get; set; }

        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime _appDate = DateTime.Now;

        public string LastUpdatedUser { get; set; }



        public DateTime? LastUpdatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        // Added by neeraj starts
        public mdmVariableLabel variableLblInput;
        public bool IsDeleted { get; set; }
        public mdmVariableInput varmdmLblInput;
        // Added by neeraj ends

        public IEnumerable<SelectListItem> ClientList
        {
            get
            {
                return GetDropDownListItems.ClientList;
            }
        }


        public ALMTreeMenuInput AlmTreeMenuInput { get; set; }



        public IEnumerable<SelectListItem> GetCategories
        {
            get
            {
                return AlignmentHelper.GetAllVariableCategory();
            }


        }


        public IEnumerable<vwVariableVariableLabel> GetAllOfTypesForVarTypeId
        {
            get
            {
                return AlignmentHelper.GetAllOfTypeForVarTypeId(this.VarTypeId, this.ClientId);
            }
        }


        public IEnumerable<vwVariableVariableLabel> GetAllOfTypes
        {
            get
            {
                return AlignmentHelper.GetAllOfType(this.VariableTypeId, this.ClientId, this.VariableId);
            }
        }


        public vwclientinfo GetClientInfo
        {
            get
            {
                return AlignmentHelper.GetClientInfo(this.ClientId);
            }

        }



        public IEnumerable<vwVariableVariableLabel> GetAllOfOtherTypesForParentChild
        {
            get
            {
                return AlignmentHelper.GetAllOfOtherTypesForParentChild(this.VariableTypeId, this.ClientId, this.VariableId);
            }
        }


        public IEnumerable<SelectListItem> GetParents(int clientId, int valuechainTypeId)
        {

            return AlignmentHelper.GetAllParentsForClientAndValueChainType(clientId, valuechainTypeId);


        }

        public ALMTreeMenuInput SetTreeMenu(int clientId)
        {
            return new ALMTreeMenuInput(clientId);
        }


        public ALMTreeMenuInput SetDependTreeMenu(int variableId, int clientId)
        {
            return new ALMTreeMenuInput(variableId, clientId);
        }



        public ALMTreeMenuInput SetTreeMenuForVarType(int vartypeId, int clientId, string varType)
        {
            return new ALMTreeMenuInput(vartypeId, varType, clientId);
        }



        public IEnumerable<SelectListItem> GetVariableTypes
        {
            get
            {
                return AlignmentHelper.GetAllVariableTypes();
            }


        }

        public IList<mdmVariable> GetMdmVariables
        {
            get
            {
                return AlignmentHelper.GetMdmVariables();
            }

        }

        public IEnumerable<mdmVariableMdType> getVariableMdTypes
        {
            get
            {
                return AlignmentHelper.GetAllofMdType();
            }
        }
        //Added for View Trigger
        public IEnumerable<mdmVariableMdType> getnonvwVariableMdTypes
        {
            get
            {
                return AlignmentHelper.GetNonvwTrigMdType();
            }
        }
        //Added for View Trigger
        public IEnumerable<mdmVariableMdType> getvwVariableMdTypes
        {
            get
            {
                return AlignmentHelper.GetvwTrigMdType();
            }
        }




        // Added by Neeraj starts
        public mdmVariableLabel GetvariableLabelInput
        {
            get
            {
                return variableLblInput;
            }
            set
            {
                variableLblInput = value;
            }
        }

        public mdmVariableMD GetVariableItems(int VariableID, int metadataId)
        {
            return AlignmentHelper.GetVariableItem(VariableID, metadataId).FirstOrDefault();
        }




        public mdmVariableREL GetVariableRel(int VariableId, int fkVariableId, int RelType)
        {
            using (Db db = new Db())
            {
                //mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipTypeId == RelType && c.RelationshipVariableId2 == VariableLabelId && c.RelationshipVariableId1 == fkVariableLabelId && c.DisplayOrder == DisplayOrder && c.IsDeleted == false));
                mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipTypeId == RelType && c.RelationshipVariableId2 == fkVariableId && c.RelationshipVariableId1 == VariableId && c.IsDeleted == false));
                return existingVariableLabel;
            }
        }


        //public mdmVariableREL GetVariableChildren(int VariableLabelId, int fkVariableLabelId,  int RelType)
        //    {
        //    using (Db db = new Db())
        //        {
        //        //mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == fkVariableLabelId && c.DisplayOrder == DisplayOrder && c.RelationshipTypeId == RelType && c.IsDeleted == false));

        //        mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == fkVariableLabelId  && c.RelationshipTypeId == RelType && c.IsDeleted == false));
        //        //mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == fkVariableLabelId && c.RelationshipTypeId == RelType && c.IsDeleted == false));
        //        return existingVariableLabel;
        //        }
        //    }



        //public mdmVariableREL GetVariableParent(int VariableId, int fkVariableId,  int RelType)
        //{
        //    using (Db db = new Db())
        //    {
        //        //mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == fkVariableLabelId && c.DisplayOrder == DisplayOrder && c.RelationshipTypeId == RelType && c.IsDeleted == false));
        //        mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == VariableId && c.RelationshipTypeId == RelType && c.IsDeleted == false));
        //        //mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == fkVariableLabelId && c.RelationshipTypeId == RelType && c.IsDeleted == false));
        //        return existingVariableLabel;
        //    }
        //}
        // Added by Neeraj ends


        public PPTreeView PPTreeViewRoot
        {
            get
            {
                IList<PPTreeView> AllTreeViewItems = AlignmentHelper.GetTreeViewItems(this.ClientId);
                PPTreeView root = AllTreeViewItems.Where(c => c.ParentId == 0).FirstOrDefault();
                SetChildren(root, AllTreeViewItems);
                return root;
            }

        }


        private void SetChildren(PPTreeView ppItem, IList<PPTreeView> list)
        {
            var children = list.Where(c => c.ParentId == ppItem.Id).ToList();
            if (children.Count > 0)
            {
                foreach (var child in children)
                {
                    SetChildren(child, list);
                    ppItem.TreeViewItems.Add(child);
                }
            }

        }


        public PPTreeView PPTreeViewDependRoot
        {
            get
            {
                IList<PPTreeView> AllTreeViewItems = AlignmentHelper.GetTreeViewDependItems(this.ClientId, this.VariableId);
                PPTreeView root = AllTreeViewItems.Where(c => c.ParentId == 0).FirstOrDefault();
                SetDependChildren(root, AllTreeViewItems);
                return root;
            }

        }


        private void SetDependChildren(PPTreeView ppItem, IList<PPTreeView> list)
        {
            var children = list.Where(c => c.ParentId == ppItem.Id).ToList();
            if (children.Count > 0)
            {
                foreach (var child in children)
                {
                    SetChildren(child, list);
                    ppItem.TreeViewItems.Add(child);
                }
            }

        }

        public string tblname1 { get; set; }
        public string tblname2 { get; set; }
        public string tblname3 { get; set; }

        public string jcolname1 { get; set; }
        public string jcolname2 { get; set; }
        public string jcolname3 { get; set; }

        public string colname11 { get; set; }
        public string colname12 { get; set; }
        public string colname13 { get; set; }
        public string colname14 { get; set; }
        public string colname15 { get; set; }
        public string colname16 { get; set; }
        public string colname17 { get; set; }
        public string colname18 { get; set; }
        public string colname19 { get; set; }
        public string colname110 { get; set; }

        public string colname21 { get; set; }
        public string colname22 { get; set; }
        public string colname23 { get; set; }
        public string colname24 { get; set; }
        public string colname25 { get; set; }
        public string colname26 { get; set; }
        public string colname27 { get; set; }
        public string colname28 { get; set; }
        public string colname29 { get; set; }
        public string colname210 { get; set; }
        public string colname31 { get; set; }
        public string colname32 { get; set; }
        public string colname33 { get; set; }
        public string colname34 { get; set; }
        public string colname35 { get; set; }
        public string colname36 { get; set; }
        public string colname37 { get; set; }
        public string colname38 { get; set; }
        public string colname39 { get; set; }
        public string colname310 { get; set; }

        public string vwname { get; set; }
        public string vwstatus { get; set; }
        public List<SelectListItem> colnames { get; set; }

        public string joiningcolname { get; set; }


        public IEnumerable<SelectListItem> TablesLists
        {

            get
            {

                Db db = new Db();

                IQueryable<Domain.Entities.Tables> tblLevel = from r in db.Tbles
                                                              select r;
                var item = tblLevel.Select(p => new SelectListItem { Value = p.TABLE_NAME, Text = p.TABLE_NAME }).ToList();
                if (item.Count > 0)
                {
                    item.Add(new SelectListItem { Text = "All", Value = "All" });
                }
                return item;
            }
        }

        public IEnumerable<SelectListItem> colList1
        {
            get
            {

                Db db = new Db();

                IQueryable<Domain.Entities.Columns> tblLevel = from r in db.Column.Where(m => m.TABLE_NAME == tblname1)
                                                               select r;
                var item = tblLevel.Select(p => new SelectListItem { Value = p.COLUMN_NAME, Text = p.COLUMN_NAME }).ToList();
                if (item.Count > 0)
                {
                    item.Add(new SelectListItem { Text = "All", Value = "All" });
                }
                return item;
            }

        }

        public IEnumerable<SelectListItem> colList2
        {
            get
            {

                Db db = new Db();

                IQueryable<Domain.Entities.Columns> tblLevel = from r in db.Column.Where(m => m.TABLE_NAME == tblname2)
                                                               select r;
                var item = tblLevel.Select(p => new SelectListItem { Value = p.COLUMN_NAME, Text = p.COLUMN_NAME }).ToList();
                if (item.Count > 0)
                {
                    item.Add(new SelectListItem { Text = "All", Value = "All" });
                }
                return item;
            }

        }

        public IEnumerable<SelectListItem> colList3
        {
            get
            {

                Db db = new Db();

                IQueryable<Domain.Entities.Columns> tblLevel = from r in db.Column.Where(m => m.TABLE_NAME == tblname3)
                                                               select r;
                var item = tblLevel.Select(p => new SelectListItem { Value = p.COLUMN_NAME, Text = p.COLUMN_NAME }).ToList();
                if (item.Count > 0)
                {
                    item.Add(new SelectListItem { Text = "All", Value = "All" });
                }
                return item;
            }

        }

        //public IEnumerable<SelectListItem> TableLists()
        //{
        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //        var item1 = unitOfWork.TableLists.Get();
        //        var item = item1.Select(p => new SelectListItem { Value = p.TABLE_NAME.ToString(), Text = p.TABLE_NAME.ToString() }).ToList();
        //        if (item.Count > 0)
        //        {
        //            item.Add(new SelectListItem { Text = "All", Value = "All" });
        //        }
        //        return item;
        //    }

        //}

        //public IEnumerable<SelectListItem> colList()
        //{
        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //        var item1 = unitOfWork.ColumnLists.Get();
        //        var item = item1.Select(p => new SelectListItem { Value = p.COLUMN_NAME.ToString(), Text = p.COLUMN_NAME.ToString() }).ToList();
        //        if (item.Count > 0)
        //        {
        //            item.Add(new SelectListItem { Text = "All", Value = "All" });
        //        }
        //        return item;
        //    }

        //}


        //public IEnumerable<SelectListItem> TableList()
        //{
        //    DbContextFactory Context = new DbContextFactory();
        //    Repo<NTTData.AMOPortal.Domain.Entities.Tables> slaCal = new Data.Repo<Domain.Entities.Tables>(Context);
        //    var Items = slaCal.Select(CList => new SelectListItem
        //    {
        //        Value = CList.TABLE_NAME.ToString(),
        //        Text = CList.TABLE_NAME.ToString()
        //    });
        //    return Items;

        //}



        //public IEnumerable<SelectListItem> TableList
        //{
        //    get
        //    {


        //        DbContextFactory Context = new DbContextFactory();
        //        Repo<NTTData.AMOPortal.Domain.Entities.Tables> tbl = new Repo<Domain.Entities.Tables>(Context);
        //        List<NTTData.AMOPortal.Domain.Entities.Tables> tblL = tbl.GetAll().OrderBy(m => m.TABLE_NAME).ToList();
        //        var Items = tblL.Select(CList => new SelectListItem
        //        {
        //            Value = CList.TABLE_NAME.ToString(),
        //            Text = CList.TABLE_NAME.ToString()
        //        });
        //        return Items;
        //    }
        //}

        //public IEnumerable<SelectListItem> colList
        //{
        //    get
        //    {


        //        DbContextFactory Context = new DbContextFactory();
        //        Repo<NTTData.AMOPortal.Domain.Entities.Columns> tbl = new Data.Repo<Domain.Entities.Columns>(Context);
        //        List<NTTData.AMOPortal.Domain.Entities.Columns> tblL = tbl.GetAll().Where(m=>m.TABLE_NAME == tblname).OrderBy(m => m.COLUMN_NAME).ToList();
        //        var Items = tblL.Select(CList => new SelectListItem
        //        {
        //            Value = CList.COLUMN_NAME.ToString(),
        //            Text = CList.COLUMN_NAME.ToString()
        //        });
        //        return Items;
        //    }
        //}


        public mdmVariableMD mdmvariableInput { get; set; }

    }
}