using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Models;
using WebUI.Areas.AlignmentManager.Objects;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Controllers;
using NTTData.AMOPortal.Service;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Service;
using AutoMapper;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;


namespace WebUI.Areas.AlignmentManager.Controllers
{
    public class AlignmentManagerController : Cruder<mdmVariable, VariableInput>
    {

        //Entities
        private mdmVariable entityVariable;
        private mdmVariableLabel entityVariableLabel;
        private mdmVariableMD entityVariableMd;
        private mdmVariableREL entityVariableRel;

        //Service

        private CrudService<mdmVariable> serviceVariable;
        private CrudService<mdmVariableLabel> serviceVariableLabel;
        private CrudService<mdmVariableMD> serviceVariableMD;
        private CrudService<mdmVariableREL> serviceVariableRel;

        //Mappers

        private Mapper<mdmVariable, VariableInput> variableMapper;
        private Mapper<mdmVariable, mdmVariableInput> varMapper;
        private Mapper<mdmVariableLabel, mdmVariableLabelInput> variableLabelMapper;
        private Mapper<mdmVariableMdType, mdmVariableMdInput> variableLabelMD;
        private Mapper<mdmVariableMD, mdmVariableMdInput> variableMDMapper;
        private Mapper<mdmVariableREL, mdmVariableRelInput> variableRelMapper;


        //Repo

        private Repo<mdmVariable> repoVariable;
        private Repo<mdmVariableLabel> repoVariableLabel;
        private Repo<mdmVariableMD> repoVariableMD;
        private Repo<mdmVariableREL> repoVariableRel;

        /// 

        public AlignmentManagerController(ICrudService<mdmVariable> service, IMapper<mdmVariable, VariableInput> v)
            : base(service, v)
        {


        }



        public ActionResult DisplayHomePage(int? ClientId = null, int? variableId = null, int? variableTypeId = null, string succesMsg = "")
        {
            var model = new VariableInput()
            {
                ClientId = ClientId.HasValue ? Convert.ToInt32(ClientId) : 0,
                VariableId = variableId.HasValue ? Convert.ToInt32(variableId) : 0,
                VarTypeId = variableTypeId.HasValue ? Convert.ToInt32(variableTypeId) : 0
            };

            if (succesMsg != "")
                ModelState.AddModelError("Success", succesMsg);
            return View("AlmMainPage", model);

        }

        private int GetInt(object o)
        {
            if (o != null)
            {
                if (o is int)
                    return (int)o;
                if (o is string)
                {
                    int result;
                    if (int.TryParse((string)o, out result))
                        return result;
                }
            }
            return -1;
        }

        public ActionResult AddVariable(int clientId, int? variableId = null, int? varTypeId = null, int? varCatId = null)
        {
            VariableInput varInput = new VariableInput();
            varInput.ClientId = clientId;

            if (variableId.HasValue)
            {
                varInput.VariableId = (int)variableId;
            }
            if (varTypeId.HasValue)
            {
                varInput.VariableTypeId = (int)varTypeId;
            }
            if (varCatId.HasValue)
            {
                varInput.VariableCatId = (int)varCatId;
            }

            //using (Db db = new Db())
            //{
            //    IList<mdmVariable> mdmVariable = AlignmentHelper.GetVariablesForClientId(clientId, varInput.VariableTypeId, varInput.VariableCatId);
            //    if (mdmVariable.Count > 0)
            //    {
            //        foreach (var item in mdmVariable)
            //        {
            //            mdmVariableLabel existingVariableLabel = db.VariableLabel.FirstOrDefault(c => (c.Id == item.VariableLabelId));
            //            varInput.GetvariableLabelInput = existingVariableLabel;
            //            varInput.VariableLabelId = item.VariableLabelId;
            //            ViewData["VarName"] = varInput.GetvariableLabelInput.VariableLabel;
            //        }
            //    }
            //}

            ViewData["VarName"] = AlignmentHelper.GetVariableName(varInput.VariableId);
            return View("AddVariableWithPartialViews", varInput);
        }


        [HttpGet]
        public JsonResult BindService(string sercatId)
        {
            DbContextFactory Dbcon = new DbContextFactory();
            Db db = new Db();
            var SerNames = from r in db.Column
                           where r.TABLE_NAME == sercatId
                           select new
                           {
                               SerId = r.COLUMN_NAME,
                               SerName = r.COLUMN_NAME
                           };
            return Json(SerNames, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SaveVariableDetails(VariableInput variableInput)
        {

            if (variableInput.VariableLabelId == 0)
            {
                if (variableInput.VariableTypeId == 0 || variableInput.VariableCatId == 0)
                {
                    return View("AddVariableWithPartialViews", variableInput);
                }

                if (variableInput.VariableId <= 0)
                {
                    // the parent/child & sibling relationships are added later...
                    //var newVar = AlignmentHelper.New<Variable>();//AlignmentHelper.AddNewVariableChild(model.ParentId);

                    DbContextFactory Dbcon = new DbContextFactory();
                    mdmVariableLabel variableLabel = new mdmVariableLabel();

                    //Check for existing variable name 
                    using (Db db = new Db())
                    {

                        IList<mdmVariable> mdmVariable = AlignmentHelper.GetVariablesForClientId(variableInput.ClientId);

                        if (mdmVariable.Count > 0)
                        {
                            foreach (var item in mdmVariable)
                            {

                                vwVariableVariableLabel existingVariableLabel = db.vwVariableVariableLabel.FirstOrDefault(c => (c.ClientId == variableInput.ClientId && c.VariableLabel.ToLower().Trim() == variableInput.GetvariableLabelInput.VariableLabel.ToLower().Trim()));
                                //mdmVariableLabel existingVariableLabel = db.VariableLabel.FirstOrDefault(c => (c.VariableLabel.ToLower().Trim() == variableInput.GetvariableLabelInput.VariableLabel.ToLower().Trim()));
                                if (existingVariableLabel != null)
                                {
                                    ModelState.AddModelError("VariableLabel", "Variable Name already Exists");
                                    return View("AddVariableWithPartialViews", variableInput);
                                }

                            }
                        }

                    }


                    //Insert record in mdmvariableLabel table

                    repoVariableLabel = new Repo<mdmVariableLabel>(Dbcon);
                    variableLabelMapper = new Mapper<mdmVariableLabel, mdmVariableLabelInput>();
                    serviceVariableLabel = new CrudService<mdmVariableLabel>(repoVariableLabel);
                    entityVariableLabel = new mdmVariableLabel();
                    variableInput.variableLabelInput.FullDescription = variableInput.GetvariableLabelInput.FullDescription;
                    variableInput.variableLabelInput.VariableLabel = variableInput.GetvariableLabelInput.VariableLabel;
                    variableInput.variableLabelInput.Id = variableInput.VariableLabelId;
                    variableInput.variableLabelInput.CreatedByUser = User.Identity.Name;
                    var labelId = serviceVariableLabel.Create(variableLabelMapper.MapToEntity(variableInput.variableLabelInput, entityVariableLabel));

                    //Insert Record in mdmVariable

                    repoVariable = new Repo<mdmVariable>(Dbcon);


                    variableMapper = new Mapper<mdmVariable, VariableInput>();
                    serviceVariable = new CrudService<mdmVariable>(repoVariable);
                    entityVariable = new mdmVariable();
                    variableInput.VariableLabelId = labelId;
                    variableInput.CreatedByUser = User.Identity.Name;
                    variableInput.VariableId = serviceVariable.Create(variableMapper.MapToEntity(variableInput, entityVariable));
                    ModelState.AddModelError("CreateMsg", "Value Chain created successfully");
                    Dbcon.Dispose();
                }
            }
            return RedirectToAction("AddVariableMetadata", "AlignmentManager", new
            {
                clientId = variableInput.ClientId,
                chainCatId = variableInput.variableLabelInput.VariableCatId,
                varTypeId = variableInput.VariableTypeId,
                varLabelId = variableInput.VariableLabelId,
                VariableId = variableInput.VariableId,
                varCatId = variableInput.VariableCatId
            });
        }

        [HttpGet]
        public ActionResult AddVariableMetadata(int clientId = 164, int chainCatId = 1, int varTypeId = 1, int varLabelId = 1104, int VariableId = 1, int varCatId = 1)
        {
            VariableInput varInput = new VariableInput();
            varInput.ClientId = clientId;
            varInput.VariableId = VariableId;
            varInput.VariableTypeId = varTypeId;
            varInput.VariableLabelId = varLabelId;
            varInput.VariableCatId = varCatId;

            //using (Db db = new Db())
            //{
            //    IList<mdmVariable> mdmVariable = AlignmentHelper.GetVariablesForClientId(clientId, varTypeId, varCatId);
            //    if (mdmVariable.Count > 0)
            //    {
            //        foreach (var item in mdmVariable)
            //        {
            //            mdmVariableLabel existingVariableLabel = db.VariableLabel.FirstOrDefault(c => (c.Id == item.VariableLabelId));
            //            varInput.GetvariableLabelInput = existingVariableLabel;
            //            ViewData["VarName"] = varInput.GetvariableLabelInput.VariableLabel;
            //        }
            //    }
            //}

            DbContextFactory db = new DbContextFactory();
            Repo<mdmVariableType> vmdmVar = new Repo<mdmVariableType>(db);
            List<mdmVariableType> vmdmVarL = vmdmVar.GetAll().Where(m => m.Id == varTypeId && m.VariableType.Contains("View Trigger")).ToList();
            ViewData["VarName"] = AlignmentHelper.GetVariableName(varInput.VariableId);
            if (vmdmVarL.Count == 0)
            {
                return View("AddVariableMetaDataWithPartialView", varInput);
            }
            else
            {

                foreach (var item in varInput.getvwVariableMdTypes)
                {
                    mdmVariableMD itm = varInput.GetVariableItems(VariableId, item.Id);
                    var Values = "";
                    int vwData = 0;
                    Values = itm.MetaDataValue;
                    vwData = itm.MetaDataData;

                    if (item.TypeName == "View Trigger - View Name")
                    {
                        varInput.vwname = Values;
                    }
                    if (item.TypeName == "View Trigger - View Status")
                    {
                        varInput.vwstatus = Values;
                    }
                    if (item.TypeName == "View Trigger - Table 1 Name")
                    {
                        varInput.tblname1 = Values;
                    }
                    if (item.TypeName == "View Trigger - Table 2 Name")
                    {
                        varInput.tblname2 = Values;
                    }
                    if (item.TypeName == "View Trigger - Table 3 Name")
                    {
                        varInput.tblname3 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 1")
                    {
                        varInput.colname11 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 2")
                    {
                        varInput.colname12 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 3")
                    {
                        varInput.colname13 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 4")
                    {
                        varInput.colname14 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 5")
                    {
                        varInput.colname15 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 6")
                    {
                        varInput.colname16 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 7")
                    {
                        varInput.colname17 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 8")
                    {
                        varInput.colname18 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 9")
                    {
                        varInput.colname19 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 10")
                    {
                        varInput.colname110 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 1")
                    {
                        varInput.colname21 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 2")
                    {
                        varInput.colname22 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 3")
                    {
                        varInput.colname23 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 4")
                    {
                        varInput.colname24 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 5")
                    {
                        varInput.colname25 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 6")
                    {
                        varInput.colname26 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 7")
                    {
                        varInput.colname27 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 8")
                    {
                        varInput.colname28 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 9")
                    {
                        varInput.colname29 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 10")
                    {
                        varInput.colname210 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 1")
                    {
                        varInput.colname31 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 2")
                    {
                        varInput.colname32 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 3")
                    {
                        varInput.colname33 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 4")
                    {
                        varInput.colname34 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 5")
                    {
                        varInput.colname35 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 6")
                    {
                        varInput.colname36 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 7")
                    {
                        varInput.colname37 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 8")
                    {
                        varInput.colname38 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 9")
                    {
                        varInput.colname39 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 10")
                    {
                        varInput.colname310 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Joining Column")
                    {
                        varInput.jcolname1 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Joining Column")
                    {
                        varInput.jcolname2 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Joining Column")
                    {
                        varInput.jcolname3 = Values;
                    }
                }
                return View("AddVariableVWMetaDataWithPartialView", varInput);
            }
        }

        [HttpPost]
        public ActionResult SaveVWMetaDataDetails(FormCollection form, VariableInput varinput)
        {
            using (Db db = new Db()) // check if record exists during back button click then move to next screen without saving entry.
            {
                mdmVariableMD extVarLbl = db.VariableMdData.FirstOrDefault(c => (c.VariableID == varinput.VariableId));
                if (extVarLbl == null)
                {
                    using (DbContextFactory Dbcon = new DbContextFactory())
                    {
                        repoVariableMD = new Repo<mdmVariableMD>(Dbcon);
                        serviceVariableMD = new CrudService<mdmVariableMD>(repoVariableMD);
                        //foreach (var mdtype in varinput.getvwVariableMdTypes)
                        //{

                        for (var x = 1; x <= 3; x++)
                        {
                            string mdValx = Server.HtmlEncode(form.GetValue("tblname" + x).AttemptedValue);
                            entityVariableMd = new mdmVariableMD();
                            //mdmVariableMdInput VariableMdInput = new mdmVariableMdInput();
                            entityVariableMd.VariableID = varinput.VariableId;
                            var vwtypename = "View Trigger - Table " + x + " Name";
                            entityVariableMd.MetaDataData = db.VariableMdType.Where(c => (c.TypeName == vwtypename)).FirstOrDefault().Id;
                            entityVariableMd.MetaDataValue = mdValx;
                            entityVariableMd.CreatedByUser = User.Identity.Name;
                            entityVariableMd.CreatedDate = DateTime.Now;
                            var mdVariableIdx = serviceVariableMD.Create(entityVariableMd);

                            string mdValy = Server.HtmlEncode(form.GetValue("jcolname" + x).AttemptedValue);
                            entityVariableMd = new mdmVariableMD();
                            //mdmVariableMdInput VariableMdInput = new mdmVariableMdInput();
                            entityVariableMd.VariableID = varinput.VariableId;
                            var vwjcol = "View Trigger - (Table " + x + ") Joining Column";
                            entityVariableMd.MetaDataData = db.VariableMdType.FirstOrDefault(c => (c.TypeName == vwjcol)).Id;
                            entityVariableMd.MetaDataValue = mdValy;
                            entityVariableMd.CreatedByUser = User.Identity.Name;
                            entityVariableMd.CreatedDate = DateTime.Now;
                            var mdVariableIdy = serviceVariableMD.Create(entityVariableMd);
                        }

                        for (var x = 1; x <= 10; x++)
                        {
                            string mdVal1 = Server.HtmlEncode(form.GetValue("colname1" + x).AttemptedValue);
                            entityVariableMd = new mdmVariableMD();
                            entityVariableMd.VariableID = varinput.VariableId;
                            var vwcol1 = "View Trigger - (Table 1) Column " + x;
                            entityVariableMd.MetaDataData = db.VariableMdType.FirstOrDefault(c => (c.TypeName == vwcol1)).Id;
                            entityVariableMd.MetaDataValue = mdVal1;
                            entityVariableMd.CreatedByUser = User.Identity.Name;
                            entityVariableMd.CreatedDate = DateTime.Now;
                            var mdVariableId1 = serviceVariableMD.Create(entityVariableMd);
                            string mdVal2 = Server.HtmlEncode(form.GetValue("colname2" + x).AttemptedValue);
                            entityVariableMd = new mdmVariableMD();
                            entityVariableMd.VariableID = varinput.VariableId;
                            var vwcol2 = "View Trigger - (Table 2) Column " + x;
                            entityVariableMd.MetaDataData = db.VariableMdType.FirstOrDefault(c => (c.TypeName == vwcol2)).Id;
                            entityVariableMd.MetaDataValue = mdVal2;
                            entityVariableMd.CreatedByUser = User.Identity.Name;
                            entityVariableMd.CreatedDate = DateTime.Now;
                            var mdVariableId2 = serviceVariableMD.Create(entityVariableMd);
                            string mdVal3 = Server.HtmlEncode(form.GetValue("colname3" + x).AttemptedValue);
                            entityVariableMd = new mdmVariableMD();
                            entityVariableMd.VariableID = varinput.VariableId;
                            var vwcol3 = "View Trigger - (Table 3) Column " + x;
                            entityVariableMd.MetaDataData = db.VariableMdType.FirstOrDefault(c => (c.TypeName == vwcol3)).Id;
                            entityVariableMd.MetaDataValue = mdVal3;
                            entityVariableMd.CreatedByUser = User.Identity.Name;
                            entityVariableMd.CreatedDate = DateTime.Now;
                            var mdVariableId3 = serviceVariableMD.Create(entityVariableMd);
                        }

                        string mdVal4 = Server.HtmlEncode(form.GetValue("vwname").AttemptedValue);
                        entityVariableMd = new mdmVariableMD();
                        entityVariableMd.VariableID = varinput.VariableId;
                        entityVariableMd.MetaDataData = db.VariableMdType.FirstOrDefault(c => (c.TypeName == "View Trigger - View Name")).Id;
                        entityVariableMd.MetaDataValue = mdVal4;
                        entityVariableMd.CreatedByUser = User.Identity.Name;
                        entityVariableMd.CreatedDate = DateTime.Now;
                        var mdVariableId4 = serviceVariableMD.Create(entityVariableMd);

                        string mdVal5 = Server.HtmlEncode(form.GetValue("vwstatus").AttemptedValue);
                        entityVariableMd = new mdmVariableMD();
                        entityVariableMd.VariableID = varinput.VariableId;
                        entityVariableMd.MetaDataData = db.VariableMdType.FirstOrDefault(c => (c.TypeName == "View Trigger - View Status")).Id;
                        entityVariableMd.MetaDataValue = mdVal5;
                        entityVariableMd.CreatedByUser = User.Identity.Name;
                        entityVariableMd.CreatedDate = DateTime.Now;
                        var mdVariableId5 = serviceVariableMD.Create(entityVariableMd);



                    }

                    return RedirectToAction("AddVariableMetadata", "AlignmentManager", new
                    {
                        clientId = varinput.ClientId,
                        chainCatId = 0,
                        varTypeId = varinput.VariableTypeId,
                        varLabelId = varinput.VariableLabelId,
                        VariableId = varinput.VariableId,
                        varCatId = varinput.VariableCatId
                    });


                }
                else
                {
                    foreach (var item in db.VariableMdData.Where(c => (c.VariableID == varinput.VariableId)))
                    {
                        using (DbContextFactory Dbcon = new DbContextFactory())
                        {
                            repoVariableMD = new Repo<mdmVariableMD>(Dbcon);
                            serviceVariableMD = new CrudService<mdmVariableMD>(repoVariableMD);
                            Mapper<mdmVariableMD, mdmVariableMdInput> mdmMDMapper = new Mapper<mdmVariableMD, mdmVariableMdInput>();
                            foreach (var varmdtype in varinput.getvwVariableMdTypes.Where(m => m.Id == item.MetaDataData))
                            {
                                for (var y = 1; y <= 3; y++)
                                {
                                    if (varmdtype.TypeName == "View Trigger - Table " + y + " Name")
                                    {
                                        //mdmVariableMdInput entityVariableMdI = new mdmVariableMdInput();
                                        entityVariableMd = serviceVariableMD.Get(item.Id);
                                        //mdmMDMapper.MapToEntity(entityVariableMdI, entityVariableMd);
                                        var vwctrlname = "tblname" + y;
                                        entityVariableMd.MetaDataValue = Server.HtmlEncode(form.GetValue(vwctrlname).AttemptedValue);
                                        serviceVariableMD.Save();
                                    }
                                    if (varmdtype.TypeName == "View Trigger - (Table " + y + ") Joining Column")
                                    {
                                        //mdmVariableMdInput entityVariableMdI = new mdmVariableMdInput();
                                        //mdmMDMapper.MapToEntity(entityVariableMdI, serviceVariableMD.Get(item.Id));
                                        entityVariableMd = serviceVariableMD.Get(item.Id);
                                        var vwctrlname = "jcolname" + y;
                                        entityVariableMd.MetaDataValue = Server.HtmlEncode(form.GetValue(vwctrlname).AttemptedValue);
                                        serviceVariableMD.Save();
                                    }


                                    for (var x = 1; x <= 10; x++)
                                    {
                                        if (varmdtype.TypeName == "View Trigger - (Table " + y + ") Column " + x)
                                        {
                                            entityVariableMd = serviceVariableMD.Get(item.Id);
                                            var vwctrlname = "colname" + y + x;
                                            entityVariableMd.MetaDataValue = Server.HtmlEncode(form.GetValue(vwctrlname).AttemptedValue);
                                            serviceVariableMD.Save();
                                        }
                                    }
                                }


                                if (varmdtype.TypeName == "View Trigger - View Name")
                                {
                                    entityVariableMd = serviceVariableMD.Get(item.Id);
                                    entityVariableMd.MetaDataValue = Server.HtmlEncode(form.GetValue("vwname").AttemptedValue);
                                    serviceVariableMD.Save();
                                }

                                if (varmdtype.TypeName == "View Trigger - View Status")
                                {
                                    entityVariableMd = serviceVariableMD.Get(item.Id);
                                    entityVariableMd.MetaDataValue = Server.HtmlEncode(form.GetValue("vwstatus").AttemptedValue);
                                    serviceVariableMD.Save();
                                }
                            }

                        }
                    }

                    return RedirectToAction("EditVariableMetadata", "AlignmentManager", new
                    {
                        clientId = varinput.ClientId,
                        chainCatId = 0,
                        varTypeId = varinput.VariableTypeId,
                        varLabelId = varinput.VariableLabelId,
                        VariableId = varinput.VariableId,
                        varCatId = varinput.VariableCatId
                    });

                }

            }
        }


        [HttpPost]
        public ActionResult SaveMetaDataDetails(FormCollection form, VariableInput varinput)
        {
            using (Db db = new Db()) // check if record exists during back button click then move to next screen without saving entry.
            {
                mdmVariableMD extVarLbl = db.VariableMdData.FirstOrDefault(c => (c.VariableID == varinput.VariableLabelId));
                if (extVarLbl != null)
                {
                    return RedirectToAction("AddVariableRelationShip", "AlignmentManager", new
                    {
                        clientId = varinput.ClientId,
                        chainCatId = varinput.VariableCatId,
                        varTypeId = varinput.VariableTypeId,
                        varLabelId = varinput.VariableLabelId,
                        VariableId = varinput.VariableId
                    });
                }
            }

            using (DbContextFactory Dbcon = new DbContextFactory())
            {
                repoVariableMD = new Repo<mdmVariableMD>(Dbcon);
                serviceVariableMD = new CrudService<mdmVariableMD>(repoVariableMD);
                entityVariableMd = new mdmVariableMD();
                foreach (var mdtype in varinput.getnonvwVariableMdTypes)
                {
                    int mdId = mdtype.Id;
                    string mdValueId = mdId + "_value";
                    string mdScoreId = mdId + "_score";
                    string mdWeightId = mdId + "_weight";
                    string mdValue = Server.HtmlEncode(form.GetValue(mdValueId).AttemptedValue);

                    int mdScore = Server.HtmlEncode(form.GetValue(mdScoreId).AttemptedValue) == "" ? 0 : Convert.ToInt32(Server.HtmlEncode(form.GetValue(mdScoreId).AttemptedValue));
                    int mdWeight = Server.HtmlEncode(form.GetValue(mdWeightId).AttemptedValue) == "" ? 0 : Convert.ToInt32(Server.HtmlEncode(form.GetValue(mdWeightId).AttemptedValue));
                    if (mdValue != "" || mdScore > 0 || mdWeight > 0)
                    {
                        //Insert a record in mdVariableMD
                        entityVariableMd = new mdmVariableMD();
                        mdmVariableMdInput VariableMdInput = new mdmVariableMdInput();
                        entityVariableMd.VariableID = varinput.VariableId;
                        entityVariableMd.MetaDataData = mdId;
                        entityVariableMd.MetaDataWeight = mdWeight;
                        entityVariableMd.MetaDataValue = mdValue;
                        entityVariableMd.MetaDataScore = mdScore;
                        entityVariableMd.CreatedByUser = User.Identity.Name;
                        entityVariableMd.CreatedDate = DateTime.Now;
                        var mdVariableId = serviceVariableMD.Create(entityVariableMd);
                    }
                }
            }

            return RedirectToAction("AddVariableRelationShip", "AlignmentManager", new
            {
                clientId = varinput.ClientId,
                chainCatId = varinput.VariableCatId,
                varTypeId = varinput.VariableTypeId,
                varLabelId = varinput.VariableLabelId,
                VariableId = varinput.VariableId,
                varCatId = varinput.VariableCatId
            });



            //return RedirectToAction("VariableRelationShips", "AlignmentManager", new
            //{
            //    clientId = varinput.ClientId,
            //    chainCatId = varinput.VariableCatId,
            //    varTypeId = varinput.VariableTypeId,
            //    varLabelId = varinput.VariableLabelId,
            //    VariableId = varinput.VariableId,
            //    varCatId = varinput.VariableCatId
            //});





        }


        [HttpGet]
        public ActionResult AddVariableRelationShip(int clientId = 164, int chainCatId = 1, int varTypeId = 1, int varLabelId = 1104, int VariableId = 1, int varCatId = 1)
        {
            VariableInput varInput = new VariableInput();
            varInput.ClientId = clientId;
            varInput.VariableLabelId = varLabelId;
            varInput.VariableId = VariableId;
            varInput.VariableTypeId = varTypeId;
            varInput.VariableLabelId = varLabelId;
            varInput.VariableCatId = varCatId;

            //using (Db db = new Db())
            //{
            //    IList<mdmVariable> mdmVariable = AlignmentHelper.GetVariablesForClientId(clientId, varTypeId, varCatId);
            //    if (mdmVariable.Count > 0)
            //    {
            //        //foreach (var item in mdmVariable)
            //        //{


            //    mdmVariableLabel existingVariableLabel = db.VariableLabel.FirstOrDefault(c => c.Id == varLabelId);

            //                //db.VariableLabel.FirstOrDefault(c => (c.Id == item.VariableLabelId));
            //            varInput.GetvariableLabelInput = existingVariableLabel;
            //            ViewData["VarName"] = varInput.GetvariableLabelInput.VariableLabel;
            //        //}
            //    }
            //}

            ViewData["VarName"] = AlignmentHelper.GetVariableName(varInput.VariableId);

            return View("AddVariableRelationShipWithPartialView", varInput);
        }

        //Sibling Parent & Child Relations Save
        [HttpPost]
        public ActionResult SaveSiblingRelationShipDetails(FormCollection form, VariableInput varinput)
        {
            int[] arrParent = new int[1000];
            int[] arrChild = new int[1000];
            int i = 0;

            using (Db db = new Db()) // check if record exists during back button click then move to next screen without saving entry.
            {
                mdmVariableREL extVarLbl = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == varinput.VariableId));
                if (extVarLbl != null)
                {
                    return RedirectToAction("AddParentVariableRelationShip", "AlignmentManager", new
                    {
                        clientId = varinput.ClientId,
                        chainCatId = varinput.VariableCatId,
                        varTypeId = varinput.VariableTypeId,
                        varLabelId = varinput.VariableLabelId,
                        VariableId = varinput.VariableId,
                        varCatId = varinput.VariableCatId
                    });
                }
            }

            using (DbContextFactory Dbcon = new DbContextFactory())
            {
                repoVariableRel = new Repo<mdmVariableREL>(Dbcon);
                serviceVariableRel = new CrudService<mdmVariableREL>(repoVariableRel);
                entityVariableRel = new mdmVariableREL();

                foreach (var item in form.AllKeys)
                {
                    if (item.EndsWith("parentchkbox"))
                    {
                        if (form.GetValue(item).RawValue.GetType().IsArray)
                        {
                            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);
                            if (chkSelectedVariable)
                            {
                                arrParent[i] = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));

                            }
                        }
                    }

                    if (item.EndsWith("childchkbox"))
                    {
                        if (form.GetValue(item).RawValue.GetType().IsArray)
                        {
                            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);
                            if (chkSelectedVariable)
                            {
                                arrChild[i] = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                            }
                        }
                    }
                    i++;
                }

                bool hasDuplicate = false;
                foreach (var varA in arrParent)
                {
                    foreach (var varB in arrChild)
                    {
                        if (varA != 0 && varB != 0 && varA == varB)
                        {
                            hasDuplicate = true;
                        }
                    }
                }

                if (hasDuplicate == true)
                {
                    ModelState.AddModelError("VariableLabel", "Variable Name for Parent and Children should not be same");
                    return View("AddVariableRelationShipWithPartialView", varinput);
                }


                SaveParentAndChildRelationShips(form, varinput, "Siblings");


                //foreach (var item in form.AllKeys)
                //{
                //    if (item.EndsWith("parentchkbox"))
                //    {
                //        if (form.GetValue(item).RawValue.GetType().IsArray)
                //        {
                //            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);

                //            if (chkSelectedVariable) // checked
                //            {
                //                using (Db db = new Db())
                //                {
                //                    int relID = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                //                    mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId2 == varinput.VariableLabelId));

                //                    if (existingVariableLabel != null) // if record exists do nothing
                //                    {
                //                    }
                //                    else  // insert record
                //                    {
                //                        //Sibling Parent
                //                        entityVariableRel = new mdmVariableREL();
                //                        mdmVariableRelInput VariableRelInput = new mdmVariableRelInput();
                //                        entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Sibling_Parent);
                //                        entityVariableRel.RelationshipVariableId1 = varinput.VariableLabelId;
                //                        entityVariableRel.RelationshipVariableId2 = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                //                        entityVariableRel.CreatedByUser = User.Identity.Name;
                //                        entityVariableRel.DisplayOrder = 1;
                //                        entityVariableRel.CreatedDate = DateTime.Now;

                //                        var mdVariableRel = serviceVariableRel.Create(entityVariableRel);

                //                        //Sibling Child
                //                        entityVariableRel = new mdmVariableREL();
                //                        VariableRelInput = new mdmVariableRelInput();
                //                        entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Sibling_Child);
                //                        entityVariableRel.RelationshipVariableId2 = varinput.VariableLabelId;
                //                        entityVariableRel.RelationshipVariableId1 = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                //                        entityVariableRel.CreatedByUser = User.Identity.Name;
                //                        entityVariableRel.DisplayOrder = 1;
                //                        entityVariableRel.CreatedDate = DateTime.Now;
                //                        mdVariableRel = serviceVariableRel.Create(entityVariableRel);
                //                    }
                //                }
                //            }
                //            else // if unchecked then verify if record exists then soft delete
                //            {
                //                using (Db db = new Db())
                //                {
                //                    int relID = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                //                    int reltypeId = Convert.ToInt32(RelationshipTypeEnum.Sibling_Parent);
                //                    mdmVariableREL existingVarLbl1 = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId2 == relID && c.RelationshipVariableId1 == varinput.VariableLabelId && c.RelationshipTypeId == reltypeId));

                //                    if (existingVarLbl1 != null)
                //                    {
                //                        serviceVariableRel.Delete(existingVarLbl1.Id);
                //                        serviceVariableRel.Save();

                //                        //int RelationshipVariableId2 = Convert.ToInt32(existingVarLbl1.RelationshipVariableId2);
                //                        //mdmVariableREL existingVarLbl2 = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == relID && c.RelationshipVariableId2 == existingVarLbl1.RelationshipVariableId1));
                //                        mdmVariableREL existingVarLbl2 = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == relID && c.RelationshipVariableId2 == varinput.VariableLabelId && c.RelationshipTypeId == Convert.ToInt32(RelationshipTypeEnum.Sibling_Child)));

                //                        if (existingVarLbl2 != null)
                //                        {
                //                            serviceVariableRel.Delete(existingVarLbl2.Id);
                //                            serviceVariableRel.Save();
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                //foreach (var item in form.AllKeys)
                //{
                //    if (item.EndsWith("childchkbox"))
                //    {
                //        if (form.GetValue(item).RawValue.GetType().IsArray)
                //        {
                //            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);

                //            if (chkSelectedVariable) // checked
                //            {
                //                using (Db db = new Db())
                //                {
                //                    int relID = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                //                    mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId2 == varinput.VariableLabelId && c.RelationshipTypeId == Convert.ToInt32(RelationshipTypeEnum.Sibling_Child)));

                //                    if (existingVariableLabel != null) // if record exists do nothing
                //                    {
                //                    }
                //                    else  // insert record
                //                    {
                //                        //Sibling Child
                //                        entityVariableRel = new mdmVariableREL();
                //                        mdmVariableRelInput VariableRelInput = new mdmVariableRelInput();
                //                        entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Sibling_Child);
                //                        entityVariableRel.RelationshipVariableId1 = varinput.VariableLabelId;
                //                        entityVariableRel.RelationshipVariableId2 = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                //                        entityVariableRel.CreatedByUser = User.Identity.Name;
                //                        entityVariableRel.DisplayOrder = 2;
                //                        entityVariableRel.CreatedDate = DateTime.Now;

                //                        var mdVariableRel = serviceVariableRel.Create(entityVariableRel);

                //                        //Sibling Parent
                //                        entityVariableRel = new mdmVariableREL();
                //                        VariableRelInput = new mdmVariableRelInput();
                //                        entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Sibling_Parent);
                //                        entityVariableRel.RelationshipVariableId2 = varinput.VariableLabelId;
                //                        entityVariableRel.RelationshipVariableId1 = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                //                        entityVariableRel.CreatedByUser = User.Identity.Name;
                //                        entityVariableRel.DisplayOrder = 2;
                //                        entityVariableRel.CreatedDate = DateTime.Now;
                //                        mdVariableRel = serviceVariableRel.Create(entityVariableRel);
                //                    }
                //                }
                //            }
                //            else // if unchecked then verify if record exists then delete
                //            {
                //                using (Db db = new Db())
                //                {
                //                    int relID = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                //                    mdmVariableREL existingVarLbl1 = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == relID && c.RelationshipVariableId2 == varinput.VariableLabelId));

                //                    if (existingVarLbl1 != null)
                //                    {
                //                        serviceVariableRel.Delete(existingVarLbl1.Id);
                //                        serviceVariableRel.Save();

                //                        int RelationshipVariableId2 = Convert.ToInt32(existingVarLbl1.RelationshipVariableId2);
                //                        mdmVariableREL existingVarLbl2 = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId2 == relID && c.RelationshipVariableId1 == existingVarLbl1.RelationshipVariableId1));

                //                        if (existingVarLbl2 != null)
                //                        {
                //                            serviceVariableRel.Delete(existingVarLbl2.Id);
                //                            serviceVariableRel.Save();
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                return RedirectToAction("AddParentVariableRelationShip", "AlignmentManager", new
                {
                    clientId = varinput.ClientId,
                    chainCatId = varinput.VariableCatId,
                    varTypeId = varinput.VariableTypeId,
                    varLabelId = varinput.VariableLabelId,
                    VariableId = varinput.VariableId,
                    varCatId = varinput.VariableCatId
                });
            }
        }



        [HttpGet]
        public ActionResult AddParentVariableRelationShip(int clientId = 164, int chainCatId = 1, int varTypeId = 1, int varLabelId = 1104, int VariableId = 1, int varCatId = 1)
        {
            VariableInput varInput = new VariableInput();
            varInput.ClientId = clientId;
            varInput.VariableLabelId = varLabelId;
            varInput.VariableId = VariableId;
            varInput.VariableTypeId = varTypeId;
            varInput.VariableLabelId = varLabelId;
            varInput.VariableCatId = varCatId;

            //using (Db db = new Db())
            //{
            //    IList<mdmVariable> mdmVariable = AlignmentHelper.GetVariablesForClientId(clientId, varTypeId, varCatId);
            //    if (mdmVariable.Count > 0)
            //    {
            //        foreach (var item in mdmVariable)
            //        {
            //            mdmVariableLabel existingVariableLabel = db.VariableLabel.FirstOrDefault(c => (c.Id == item.VariableLabelId));
            //            varInput.GetvariableLabelInput = existingVariableLabel;
            //            ViewData["VarName"] = varInput.GetvariableLabelInput.VariableLabel;
            //        }
            //    }
            //}

            ViewData["VarName"] = AlignmentHelper.GetVariableName(varInput.VariableId);

            return View("AddVariableParentRelationShipWithPartialView", varInput);
        }


        private bool SaveParentAndChildRelationShips(FormCollection form, VariableInput varinput, string RelationType = "")
        {
            bool blnSuccess = true;
            int parentRelationTypeId = 0;
            int childRelationTypeId = 0;
            if (RelationType == "Siblings")
            {
                parentRelationTypeId = Convert.ToInt32(RelationshipTypeEnum.Sibling_Parent);
                childRelationTypeId = Convert.ToInt32(RelationshipTypeEnum.Sibling_Child);


            }

            if (RelationType == "Variable")
            {
                parentRelationTypeId = Convert.ToInt32(RelationshipTypeEnum.Variable_Parent);
                childRelationTypeId = Convert.ToInt32(RelationshipTypeEnum.Variable_Child);
            }

            using (DbContextFactory Dbcon = new DbContextFactory())
            {
                repoVariableRel = new Repo<mdmVariableREL>(Dbcon);
                serviceVariableRel = new CrudService<mdmVariableREL>(repoVariableRel);


                IList<mdmVariableREL> relList = serviceVariableRel.GetAll().Where(c => (c.RelationshipVariableId1 == varinput.VariableId || c.RelationshipVariableId2 == varinput.VariableId) && (c.RelationshipTypeId == parentRelationTypeId || c.RelationshipTypeId == childRelationTypeId)).ToList();
                foreach (mdmVariableREL item in relList)
                {
                    serviceVariableRel.Delete(item.Id);
                }


                //relList = serviceVariableRel.GetAll().Where(c => c.RelationshipVariableId2 == varinput.VariableLabelId && c.RelationshipTypeId == childRelationTypeId).ToList();

                //foreach (mdmVariableREL item in relList)
                //    {
                //    serviceVariableRel.Delete(item.Id);
                //    }

                foreach (var item in form.AllKeys)
                {
                    if (item.EndsWith("parentchkbox"))
                    {
                        if (form.GetValue(item).RawValue.GetType().IsArray)
                        {
                            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);
                            if (chkSelectedVariable)
                            {
                                int parentTypeId = parentRelationTypeId;
                                int childTypeId = childRelationTypeId;
                                int relVariable1 = varinput.VariableId;
                                int relVariable2 = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                                int displayOrder = 1;
                                //Save Parent Record & Child Records
                                SaveRelationShipDetails(parentTypeId, childTypeId, relVariable1, relVariable2, displayOrder);
                            }
                        }

                    }

                    if (item.EndsWith("childchkbox"))
                    {
                        if (form.GetValue(item).RawValue.GetType().IsArray)
                        {
                            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);
                            if (chkSelectedVariable)
                            {
                                int parentTypeId = parentRelationTypeId;
                                int childTypeId = childRelationTypeId;
                                int relVariable2 = varinput.VariableId;
                                int relVariable1 = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                                int displayOrder = 2;
                                //Save Parent Record & Child Records
                                SaveRelationShipDetails(parentTypeId, childTypeId, relVariable1, relVariable2, displayOrder);
                            }

                        }

                    }

                }
            }

            return blnSuccess;
        }

        private void SaveRelationShipDetails(int parentTypeId, int childTypeId, int RelVariable1, int RelVariable2, int DisplayOrder)
        {
            //Before insert record in mdmvariableRel Table, please check record existance, if not exists then insert a record

            //Parent Record
            entityVariableRel = new mdmVariableREL();
            mdmVariableRelInput VariableRelInput = new mdmVariableRelInput();
            entityVariableRel.RelationshipTypeId = parentTypeId;
            entityVariableRel.RelationshipVariableId1 = RelVariable1;
            entityVariableRel.RelationshipVariableId2 = RelVariable2;
            entityVariableRel.CreatedByUser = User.Identity.Name;
            entityVariableRel.DisplayOrder = 1;
            entityVariableRel.CreatedDate = DateTime.Now;
            var mdVariableRel = serviceVariableRel.Create(entityVariableRel);

            //Child Record

            entityVariableRel = new mdmVariableREL();
            VariableRelInput = new mdmVariableRelInput();
            entityVariableRel.RelationshipTypeId = childTypeId;
            entityVariableRel.RelationshipVariableId1 = RelVariable2;
            entityVariableRel.RelationshipVariableId2 = RelVariable1;
            entityVariableRel.CreatedByUser = User.Identity.Name;
            entityVariableRel.DisplayOrder = 2;
            entityVariableRel.CreatedDate = DateTime.Now;
            mdVariableRel = serviceVariableRel.Create(entityVariableRel);






        }


        //Variable Parent & Child Relationship Saving
        [HttpPost]
        public ActionResult SaveParentRelationShipDetails(FormCollection form, VariableInput varinput)
        {

            int[] arrParent = new int[1000];
            int[] arrChild = new int[1000];
            int i = 0;

            if (!ModelState.IsValid)
            {

                return View("AlmMainPage", varinput);
            }

            using (DbContextFactory Dbcon = new DbContextFactory())
            {
                repoVariableRel = new Repo<mdmVariableREL>(Dbcon);
                serviceVariableRel = new CrudService<mdmVariableREL>(repoVariableRel);
                entityVariableRel = new mdmVariableREL();

                foreach (var item in form.AllKeys)
                {
                    if (item.EndsWith("parentchkbox"))
                    {
                        if (form.GetValue(item).RawValue.GetType().IsArray)
                        {
                            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);
                            if (chkSelectedVariable)
                            {
                                arrParent[i] = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                            }
                        }
                    }

                    if (item.EndsWith("childchkbox"))
                    {
                        if (form.GetValue(item).RawValue.GetType().IsArray)
                        {
                            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);
                            if (chkSelectedVariable)
                            {
                                arrChild[i] = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                            }
                        }
                    }
                    i++;
                }

                bool hasDuplicate = false;
                foreach (var varA in arrParent)
                {
                    foreach (var varB in arrChild)
                    {
                        if (varA != 0 && varB != 0 && varA == varB)
                        {
                            hasDuplicate = true;
                        }
                    }
                }

                if (hasDuplicate == true)
                {
                    ModelState.AddModelError("VariableLabel", "Variable Name for Parent and Children should not be same");
                    return View("AddVariableParentRelationShipWithPartialView", varinput);
                }


                SaveParentAndChildRelationShips(form, varinput, "Variable");


                //foreach (var item in form.AllKeys)
                //{
                //    if (item.EndsWith("parentchkbox"))
                //    {
                //        if (form.GetValue(item).RawValue.GetType().IsArray)
                //        {
                //            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);
                //            if (chkSelectedVariable)
                //            {
                //                //Variable Relation Ship
                //                //Variable Parent
                //                entityVariableRel = new mdmVariableREL();
                //                mdmVariableRelInput VariableRelInput = new mdmVariableRelInput();
                //                entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Variable_Parent);
                //                entityVariableRel.RelationshipVariableId1 = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                //                entityVariableRel.RelationshipVariableId2 = varinput.VariableLabelId;
                //                entityVariableRel.CreatedByUser = User.Identity.Name;
                //                entityVariableRel.DisplayOrder = 1;
                //                entityVariableRel.CreatedDate = DateTime.Now;
                //                var mdVariableRel = serviceVariableRel.Create(entityVariableRel);

                //                //Variable Child
                //                entityVariableRel = new mdmVariableREL();
                //                VariableRelInput = new mdmVariableRelInput();
                //                entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Variable_Child);
                //                entityVariableRel.RelationshipVariableId1 = varinput.VariableLabelId;
                //                entityVariableRel.RelationshipVariableId2 = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                //                entityVariableRel.CreatedByUser = User.Identity.Name;
                //                entityVariableRel.DisplayOrder = 1;
                //                entityVariableRel.CreatedDate = DateTime.Now;
                //                mdVariableRel = serviceVariableRel.Create(entityVariableRel);
                //            }
                //        }
                //    }
                //}
                //foreach (var item in form.AllKeys)
                //{
                //    if (item.EndsWith("childchkbox"))
                //    {
                //        if (form.GetValue(item).RawValue.GetType().IsArray)
                //        {
                //            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);
                //            if (chkSelectedVariable)
                //            {
                //                //Sibling Relation Ship
                //                //Sibling Parent
                //                entityVariableRel = new mdmVariableREL();
                //                mdmVariableRelInput VariableRelInput = new mdmVariableRelInput();
                //                entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Variable_Child);
                //                entityVariableRel.RelationshipVariableId1 = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                //                entityVariableRel.RelationshipVariableId2 = varinput.VariableLabelId;
                //                entityVariableRel.CreatedByUser = User.Identity.Name;
                //                entityVariableRel.DisplayOrder = 2;
                //                entityVariableRel.CreatedDate = DateTime.Now;
                //                var mdVariableRel = serviceVariableRel.Create(entityVariableRel);

                //                //Sibling Child
                //                entityVariableRel = new mdmVariableREL();
                //                VariableRelInput = new mdmVariableRelInput();
                //                entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Variable_Parent);
                //                entityVariableRel.RelationshipVariableId1 = varinput.VariableLabelId;
                //                entityVariableRel.RelationshipVariableId2 = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                //                entityVariableRel.CreatedByUser = User.Identity.Name;
                //                entityVariableRel.DisplayOrder = 2;
                //                entityVariableRel.CreatedDate = DateTime.Now;
                //                mdVariableRel = serviceVariableRel.Create(entityVariableRel);
                //            }
                //        }
                //    }
                //}

                Dbcon.Dispose();
                return RedirectToAction("DisplayHomePage", new { ClientId = varinput.ClientId, variableId = varinput.VariableId, succesMsg = "Data has been saved successfully" });
            }

        }


        // Added By Neeraj for Edit functionality-- starts
        public ActionResult EditVariable(int clientId, int variableId)
        {
            VariableInput varInput = new VariableInput();
            varInput.ClientId = clientId;

            vwVariableVariableLabel vwVariable = AlignmentHelper.GetVWVariableLabel(variableId);
            if (vwVariable != null)
            {

                varInput.GetvariableLabelInput = AlignmentHelper.GetVariableLabel(vwVariable.fkVariableLabelId);

                varInput.VariableTypeId = vwVariable.fkVariableTypeId;
                varInput.VariableCatId = vwVariable.fkVariableCategoryId;
                varInput.VariableLabelId = vwVariable.fkVariableLabelId;
                varInput.VariableId = vwVariable.Id;
                ViewData["VarName"] = varInput.GetvariableLabelInput.VariableLabel;

            }


            //using (Db db = new Db())
            //    {
            //    IList<vwVariableVariableLabel> mdmVar = AlignmentHelper.EditGetAllTypes(clientId, variableId);


            //    if (mdmVar.Count > 0)
            //        {
            //        foreach (var item in mdmVar)
            //            {
            //            mdmVariableLabel existingVariableLabel = db.VariableLabel.FirstOrDefault(c => (c.Id == item.fkVariableLabelId));
            //            varInput.GetvariableLabelInput = existingVariableLabel;

            //            varInput.VariableTypeId = item.fkVariableTypeId;
            //            varInput.VariableCatId = item.fkVariableCategoryId;
            //            varInput.VariableLabelId = item.fkVariableLabelId;
            //            varInput.VariableId = item.Id;
            //            ViewData["VarName"] = varInput.GetvariableLabelInput.VariableLabel;
            //            }
            //        }
            //    }

            //ViewData["VarName"] = AlignmentHelper.GetVariableName(varInput.VariableId);
            return View("EditVariableWithPartialViews", varInput);
        }


        [HttpPost]
        public ActionResult EditVariableDetails(VariableInput variableInput)
        {
            DbContextFactory Dbcon = new DbContextFactory();
            mdmVariableLabel variableLabel = new mdmVariableLabel();

            //Check for existing variable name 
            using (Db db = new Db())
            {
                IList<mdmVariable> mdmVariable = AlignmentHelper.GetVariablesForClientId(variableInput.ClientId, variableInput.VariableTypeId, variableInput.VariableCatId);
                if (mdmVariable.Count > 0)
                {
                    foreach (var item in mdmVariable)
                    {
                        //mdmVariableLabel existingVariableLabel = db.VariableLabel.FirstOrDefault(c => (c.VariableLabel.ToLower().Trim() == variableInput.GetvariableLabelInput.VariableLabel.ToLower().Trim() && c.Id != variableInput.VariableLabelId));
                        vwVariableVariableLabel existingVariableLabel = db.vwVariableVariableLabel.FirstOrDefault(c => (c.Id != variableInput.VariableId && c.ClientId == variableInput.ClientId && c.VariableLabel.ToLower().Trim() == variableInput.GetvariableLabelInput.VariableLabel.ToLower().Trim()));

                        if (existingVariableLabel != null)
                        {
                            ModelState.AddModelError("VariableLabel", "Value Chain Name already Exists");
                            return View("EditVariableWithPartialViews", variableInput);
                        }
                    }
                }
            }

            //Update record in mdmvariableLabel table
            repoVariableLabel = new Repo<mdmVariableLabel>(Dbcon);
            variableLabelMapper = new Mapper<mdmVariableLabel, mdmVariableLabelInput>();
            serviceVariableLabel = new CrudService<mdmVariableLabel>(repoVariableLabel);
            entityVariableLabel = new mdmVariableLabel();
            variableInput.variableLabelInput.FullDescription = variableInput.GetvariableLabelInput.FullDescription;
            variableInput.variableLabelInput.VariableLabel = variableInput.GetvariableLabelInput.VariableLabel;
            variableInput.variableLabelInput.Id = variableInput.VariableLabelId;
            variableLabelMapper.MapToEntity(variableInput.variableLabelInput, serviceVariableLabel.Get(variableInput.VariableLabelId));
            serviceVariableLabel.Save();

            ModelState.AddModelError("CreateMsg", "Value Chain updated successfully");
            Dbcon.Dispose();

            return RedirectToAction("EditVariableMetadata", "AlignmentManager", new
            {
                clientId = variableInput.ClientId,
                varTypeId = variableInput.VariableTypeId,
                varLabelId = variableInput.VariableLabelId,
                VariableId = variableInput.VariableId,
                varCatId = variableInput.VariableCatId
            });
        }

        [HttpGet]
        public ActionResult EditVariableMetadata(int clientId, int varTypeId, int varLabelId, int VariableId, int varCatId)
        {
            VariableInput varInput = new VariableInput();
            varInput.ClientId = clientId;
            varInput.VariableTypeId = varTypeId;
            varInput.VariableLabelId = varLabelId;
            varInput.VariableCatId = varCatId;
            varInput.VariableId = VariableId;
            ViewData["VarName"] = AlignmentHelper.GetVariableName(varInput.VariableId);



            DbContextFactory db = new DbContextFactory();
            Repo<mdmVariableType> vmdmVar = new Repo<mdmVariableType>(db);
            List<mdmVariableType> vmdmVarL = vmdmVar.GetAll().Where(m => m.Id == varTypeId && m.VariableType.Contains("View Trigger")).ToList();
            ViewData["VarName"] = AlignmentHelper.GetVariableName(varInput.VariableId);
            if (vmdmVarL.Count == 0)
            {
                return View("EditVariableMetaDataWithPartialView", varInput);
            }
            else
            {

                foreach (var item in varInput.getvwVariableMdTypes)
                {
                    mdmVariableMD itm = varInput.GetVariableItems(VariableId, item.Id);
                    var Values = "";
                    int vwData = 0;
                    Values = itm.MetaDataValue;
                    vwData = itm.MetaDataData;

                    if (item.TypeName == "View Trigger - View Name")
                    {
                        varInput.vwname = Values;
                    }
                    if (item.TypeName == "View Trigger - View Status")
                    {
                        varInput.vwstatus = Values;
                    }
                    if (item.TypeName == "View Trigger - Table 1 Name")
                    {
                        varInput.tblname1 = Values;
                    }
                    if (item.TypeName == "View Trigger - Table 2 Name")
                    {
                        varInput.tblname2 = Values;
                    }
                    if (item.TypeName == "View Trigger - Table 3 Name")
                    {
                        varInput.tblname3 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 1")
                    {
                        varInput.colname11 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 2")
                    {
                        varInput.colname12 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 3")
                    {
                        varInput.colname13 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 4")
                    {
                        varInput.colname14 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 5")
                    {
                        varInput.colname15 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 6")
                    {
                        varInput.colname16 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 7")
                    {
                        varInput.colname17 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 8")
                    {
                        varInput.colname18 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 9")
                    {
                        varInput.colname19 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Column 10")
                    {
                        varInput.colname110 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 1")
                    {
                        varInput.colname21 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 2")
                    {
                        varInput.colname22 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 3")
                    {
                        varInput.colname23 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 4")
                    {
                        varInput.colname24 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 5")
                    {
                        varInput.colname25 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 6")
                    {
                        varInput.colname26 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 7")
                    {
                        varInput.colname27 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 8")
                    {
                        varInput.colname28 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 9")
                    {
                        varInput.colname29 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Column 10")
                    {
                        varInput.colname210 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 1")
                    {
                        varInput.colname31 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 2")
                    {
                        varInput.colname32 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 3")
                    {
                        varInput.colname33 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 4")
                    {
                        varInput.colname34 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 5")
                    {
                        varInput.colname35 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 6")
                    {
                        varInput.colname36 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 7")
                    {
                        varInput.colname37 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 8")
                    {
                        varInput.colname38 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 9")
                    {
                        varInput.colname39 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Column 10")
                    {
                        varInput.colname310 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 1) Joining Column")
                    {
                        varInput.jcolname1 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 2) Joining Column")
                    {
                        varInput.jcolname2 = Values;
                    }
                    if (item.TypeName == "View Trigger - (Table 3) Joining Column")
                    {
                        varInput.jcolname3 = Values;
                    }
                }
                return View("AddVariableVWMetaDataWithPartialView", varInput);
            }







        }

        [HttpPost]
        public ActionResult EditMetaDataDetails(FormCollection form, VariableInput varinput)
        {
            int iID = 0;
            using (DbContextFactory Dbcon = new DbContextFactory())
            {
                repoVariableMD = new Repo<mdmVariableMD>(Dbcon);
                serviceVariableMD = new CrudService<mdmVariableMD>(repoVariableMD);
                variableMDMapper = new Mapper<mdmVariableMD, mdmVariableMdInput>();
                entityVariableMd = new mdmVariableMD();
                foreach (var mdtype in varinput.getnonvwVariableMdTypes)
                {
                    int mdId = mdtype.Id;
                    string mdValueId = mdId + "_value";
                    string mdScoreId = mdId + "_score";
                    string mdWeightId = mdId + "_weight";
                    string mdValue = Server.HtmlEncode(form.GetValue(mdValueId).AttemptedValue);

                    int mdScore = Server.HtmlEncode(form.GetValue(mdScoreId).AttemptedValue) == "" ? 0 : Convert.ToInt32(Server.HtmlEncode(form.GetValue(mdScoreId).AttemptedValue));
                    int mdWeight = Server.HtmlEncode(form.GetValue(mdWeightId).AttemptedValue) == "" ? 0 : Convert.ToInt32(Server.HtmlEncode(form.GetValue(mdWeightId).AttemptedValue));

                    using (Db db = new Db())
                    {
                        mdmVariableMD existingVariableLabel = db.VariableMdData.FirstOrDefault(c => (c.VariableID == varinput.VariableLabelId && c.MetaDataData == mdId));

                        if (existingVariableLabel != null)
                        {
                            iID = existingVariableLabel.Id;
                            if (mdValue != "" || mdScore > 0 || mdWeight > 0)
                            {
                                //Insert a record in mdVariableMD
                                entityVariableMd = new mdmVariableMD();
                                mdmVariableMdInput VariableMdInput = new mdmVariableMdInput();

                                entityVariableMd.VariableID = varinput.VariableId;
                                entityVariableMd.MetaDataData = mdId;
                                entityVariableMd.MetaDataWeight = mdWeight;
                                entityVariableMd.MetaDataValue = mdValue;
                                entityVariableMd.MetaDataScore = mdScore;
                                entityVariableMd.CreatedByUser = User.Identity.Name;
                                entityVariableMd.CreatedDate = DateTime.Now;
                                entityVariableMd.Id = iID;

                                VariableMdInput.VariableID = varinput.VariableId;
                                VariableMdInput.MetaDataData = mdId;
                                VariableMdInput.MetaDataWeight = mdWeight;
                                VariableMdInput.MetaDataValue = mdValue;
                                VariableMdInput.MetaDataScore = mdScore;
                                VariableMdInput.CreatedByUser = User.Identity.Name;
                                VariableMdInput.CreatedDate = DateTime.Now;
                                VariableMdInput.Id = iID;
                                variableMDMapper.MapToEntity(VariableMdInput, serviceVariableMD.Get(iID));
                                serviceVariableMD.Save();
                            }
                        }
                        else
                        {
                            if (mdValue != "" || mdScore > 0 || mdWeight > 0)
                            {
                                //Insert a record in mdVariableMD
                                entityVariableMd = new mdmVariableMD();
                                mdmVariableMdInput VariableMdInput = new mdmVariableMdInput();
                                entityVariableMd.VariableID = varinput.VariableId;
                                entityVariableMd.MetaDataData = mdId;
                                entityVariableMd.MetaDataWeight = mdWeight;
                                entityVariableMd.MetaDataValue = mdValue;
                                entityVariableMd.MetaDataScore = mdScore;
                                entityVariableMd.CreatedByUser = User.Identity.Name;
                                entityVariableMd.CreatedDate = DateTime.Now;
                                var mdVariableId = serviceVariableMD.Create(entityVariableMd);
                            }
                        }
                    }


                }

                return RedirectToAction("EditVariableRelationShip", "AlignmentManager", new
                {
                    clientId = varinput.ClientId,
                    chainCatId = varinput.VariableCatId,
                    varTypeId = varinput.VariableTypeId,
                    varLabelId = varinput.VariableLabelId,
                    VariableId = varinput.VariableId,
                    varCatId = varinput.VariableCatId
                });
            }
        }

        [HttpGet]
        public ActionResult EditVariableRelationShip(int clientId = 164, int chainCatId = 1, int varTypeId = 1, int varLabelId = 1104, int VariableId = 1, int varCatId = 1)
        {
            VariableInput varInput = new VariableInput();
            varInput.ClientId = clientId;
            varInput.VariableLabelId = varLabelId;
            varInput.VariableTypeId = varTypeId;
            varInput.VariableCatId = varCatId;
            varInput.VariableId = VariableId;
            ViewData["VarName"] = AlignmentHelper.GetVariableName(VariableId);

            //using (Db db = new Db())
            //{
            //    IList<mdmVariable> mdmVariable = AlignmentHelper.GetVariablesForClientId(clientId, varTypeId, varCatId);
            //    if (mdmVariable.Count > 0)
            //    {
            //        foreach (var item in mdmVariable)
            //        {
            //            mdmVariableLabel existingVariableLabel = db.VariableLabel.FirstOrDefault(c => (c.Id == item.VariableLabelId));
            //            varInput.GetvariableLabelInput = existingVariableLabel;
            //            ViewData["VarName"] = varInput.GetvariableLabelInput.VariableLabel;
            //        }
            //    }
            //}

            return View("EditVariableRelationShipWithPartialView", varInput);
        }

        [HttpPost]
        public ActionResult EditRelationShipDetails(FormCollection form, VariableInput varinput)
        {
            int[] arrParent = new int[100];
            int[] arrChild = new int[100];
            int i = 0;

            using (DbContextFactory Dbcon = new DbContextFactory())
            {
                repoVariableRel = new Repo<mdmVariableREL>(Dbcon);
                serviceVariableRel = new CrudService<mdmVariableREL>(repoVariableRel);
                entityVariableRel = new mdmVariableREL();

                foreach (var item in form.AllKeys)
                {
                    if (item.EndsWith("parentchkbox"))
                    {
                        if (form.GetValue(item).RawValue.GetType().IsArray)
                        {
                            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);
                            if (chkSelectedVariable)
                            {
                                arrParent[i] = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                            }
                        }
                    }

                    if (item.EndsWith("childchkbox"))
                    {
                        if (form.GetValue(item).RawValue.GetType().IsArray)
                        {
                            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);
                            if (chkSelectedVariable)
                            {
                                arrChild[i] = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                            }
                        }
                    }
                    i++;
                }

                bool hasDuplicate = false;
                foreach (var varA in arrParent)
                {
                    foreach (var varB in arrChild)
                    {
                        if (varA != 0 && varB != 0 && varA == varB)
                        {
                            hasDuplicate = true;
                        }
                    }
                }

                if (hasDuplicate == true)
                {
                    ModelState.AddModelError("VariableLabel", "Variable Name for Parent and Children should not be same");
                    return View("EditVariableRelationShipWithPartialView", varinput);
                }


                SaveParentAndChildRelationShips(form, varinput, "Siblings");

                //foreach (var item in form.AllKeys)
                //{
                //    if (item.EndsWith("parentchkbox"))
                //    {
                //        if (form.GetValue(item).RawValue.GetType().IsArray)
                //        {
                //            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);

                //            if (chkSelectedVariable) // checked
                //            {
                //                using (Db db = new Db())
                //                {
                //                    mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId2 == varinput.VariableLabelId && c.DisplayOrder == 1));

                //                    if (existingVariableLabel != null) // if record exists do nothing
                //                    {
                //                    }
                //                    else  // insert record
                //                    {
                //                        //Sibling Parent
                //                        entityVariableRel = new mdmVariableREL();
                //                        mdmVariableRelInput VariableRelInput = new mdmVariableRelInput();
                //                        entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Sibling_Parent);
                //                        entityVariableRel.RelationshipVariableId2 = varinput.VariableLabelId;
                //                        entityVariableRel.RelationshipVariableId1 = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                //                        entityVariableRel.CreatedByUser = User.Identity.Name;
                //                        entityVariableRel.DisplayOrder = 1;
                //                        entityVariableRel.CreatedDate = DateTime.Now;

                //                        var mdVariableRel = serviceVariableRel.Create(entityVariableRel);

                //                        //Sibling Child
                //                        entityVariableRel = new mdmVariableREL();
                //                        VariableRelInput = new mdmVariableRelInput();
                //                        entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Sibling_Child);
                //                        entityVariableRel.RelationshipVariableId1 = varinput.VariableLabelId;
                //                        entityVariableRel.RelationshipVariableId2 = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                //                        entityVariableRel.CreatedByUser = User.Identity.Name;
                //                        entityVariableRel.DisplayOrder = 1;
                //                        entityVariableRel.CreatedDate = DateTime.Now;
                //                        mdVariableRel = serviceVariableRel.Create(entityVariableRel);
                //                    }
                //                }
                //            }
                //            else // if unchecked then verify if record exists then delete
                //            {
                //                using (Db db = new Db())
                //                {
                //                    int relID = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                //                    mdmVariableREL existingVarLbl1 = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == relID && c.RelationshipVariableId2 == varinput.VariableLabelId && c.DisplayOrder == 1));

                //                    if (existingVarLbl1 != null)
                //                    {
                //                        serviceVariableRel.Delete(existingVarLbl1.Id);
                //                        serviceVariableRel.Save();

                //                        int RelationshipVariableId2 = Convert.ToInt32(existingVarLbl1.RelationshipVariableId2);
                //                        mdmVariableREL existingVarLbl2 = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId2 == relID && c.RelationshipVariableId1 == existingVarLbl1.RelationshipVariableId1 && c.DisplayOrder == 1));

                //                        if (existingVarLbl2 != null)
                //                        {
                //                            serviceVariableRel.Delete(existingVarLbl2.Id);
                //                            serviceVariableRel.Save();
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                //foreach (var item in form.AllKeys)
                //{
                //    if (item.EndsWith("childchkbox"))
                //    {
                //        if (form.GetValue(item).RawValue.GetType().IsArray)
                //        {
                //            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);

                //            if (chkSelectedVariable) // checked
                //            {
                //                using (Db db = new Db())
                //                {
                //                    mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId2 == varinput.VariableLabelId && c.DisplayOrder == 2));

                //                    if (existingVariableLabel != null) // if record exists do nothing
                //                    {
                //                    }
                //                    else  // insert record
                //                    {
                //                        //Sibling Parent
                //                        entityVariableRel = new mdmVariableREL();
                //                        mdmVariableRelInput VariableRelInput = new mdmVariableRelInput();
                //                        entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Sibling_Child);
                //                        entityVariableRel.RelationshipVariableId2 = varinput.VariableLabelId;
                //                        entityVariableRel.RelationshipVariableId1 = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                //                        entityVariableRel.CreatedByUser = User.Identity.Name;
                //                        entityVariableRel.DisplayOrder = 2;
                //                        entityVariableRel.CreatedDate = DateTime.Now;

                //                        var mdVariableRel = serviceVariableRel.Create(entityVariableRel);

                //                        //Sibling Child
                //                        entityVariableRel = new mdmVariableREL();
                //                        VariableRelInput = new mdmVariableRelInput();
                //                        entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Sibling_Parent);
                //                        entityVariableRel.RelationshipVariableId1 = varinput.VariableLabelId;
                //                        entityVariableRel.RelationshipVariableId2 = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                //                        entityVariableRel.CreatedByUser = User.Identity.Name;
                //                        entityVariableRel.DisplayOrder = 2;
                //                        entityVariableRel.CreatedDate = DateTime.Now;
                //                        mdVariableRel = serviceVariableRel.Create(entityVariableRel);
                //                    }
                //                }
                //            }
                //            else // if unchecked then verify if record exists then delete
                //            {
                //                using (Db db = new Db())
                //                {
                //                    int relID = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                //                    mdmVariableREL existingVarLbl1 = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == relID && c.RelationshipVariableId2 == varinput.VariableLabelId && c.DisplayOrder == 2));

                //                    if (existingVarLbl1 != null)
                //                    {
                //                        serviceVariableRel.Delete(existingVarLbl1.Id);
                //                        serviceVariableRel.Save();

                //                        int RelationshipVariableId2 = Convert.ToInt32(existingVarLbl1.RelationshipVariableId2);
                //                        mdmVariableREL existingVarLbl2 = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId2 == relID && c.RelationshipVariableId1 == existingVarLbl1.RelationshipVariableId1 && c.DisplayOrder == 2));

                //                        if (existingVarLbl2 != null)
                //                        {
                //                            serviceVariableRel.Delete(existingVarLbl2.Id);
                //                            serviceVariableRel.Save();
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                return RedirectToAction("EditParentVariableRelationShip", "AlignmentManager", new
                {
                    clientId = varinput.ClientId,
                    chainCatId = varinput.VariableCatId,
                    varTypeId = varinput.VariableTypeId,
                    varLabelId = varinput.VariableLabelId,
                    VariableId = varinput.VariableId,
                    varCatId = varinput.VariableCatId
                });
            }
        }

        [HttpGet] // PP11
        public ActionResult EditParentVariableRelationShip(int clientId = 164, int chainCatId = 1, int varTypeId = 1, int varLabelId = 1104, int VariableId = 1, int varCatId = 1)
        {
            VariableInput varInput = new VariableInput();
            varInput.ClientId = clientId;
            varInput.VariableLabelId = varLabelId;
            varInput.VariableTypeId = varTypeId;
            varInput.VariableId = VariableId;
            varInput.VariableCatId = varCatId;
            ViewData["VarName"] = AlignmentHelper.GetVariableName(VariableId);

            //using (Db db = new Db())
            //{
            //    IList<mdmVariable> mdmVariable = AlignmentHelper.GetVariablesForClientId(clientId, varTypeId, varCatId);
            //    if (mdmVariable.Count > 0)
            //    {
            //        foreach (var item in mdmVariable)
            //        {
            //            mdmVariableLabel existingVariableLabel = db.VariableLabel.FirstOrDefault(c => (c.Id == item.VariableLabelId));
            //            varInput.GetvariableLabelInput = existingVariableLabel;
            //            ViewData["VarName"] = varInput.GetvariableLabelInput.VariableLabel;
            //        }
            //    }
            //}

            return View("EditVariableParentRelationShipWithPartialView", varInput);
        }

        [HttpPost]
        public ActionResult EditParentRelationShipDetails(FormCollection form, VariableInput varinput)
        {
            int[] arrParent = new int[100];
            int[] arrChild = new int[100];
            int i = 0;

            using (DbContextFactory Dbcon = new DbContextFactory())
            {
                repoVariableRel = new Repo<mdmVariableREL>(Dbcon);
                serviceVariableRel = new CrudService<mdmVariableREL>(repoVariableRel);
                entityVariableRel = new mdmVariableREL();

                foreach (var item in form.AllKeys)
                {
                    if (item.EndsWith("parentchkbox"))
                    {
                        if (form.GetValue(item).RawValue.GetType().IsArray)
                        {
                            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);
                            if (chkSelectedVariable)
                            {
                                arrParent[i] = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                            }
                        }
                    }

                    if (item.EndsWith("childchkbox"))
                    {
                        if (form.GetValue(item).RawValue.GetType().IsArray)
                        {
                            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);
                            if (chkSelectedVariable)
                            {
                                arrChild[i] = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                            }
                        }
                    }
                    i++;
                }

                bool hasDuplicate = false;
                foreach (var varA in arrParent)
                {
                    foreach (var varB in arrChild)
                    {
                        if (varA != 0 && varB != 0 && varA == varB)
                        {
                            hasDuplicate = true;
                        }
                    }
                }

                if (hasDuplicate == true)
                {
                    ModelState.AddModelError("VariableLabel", "Variable Name for Parent and Children should not be same");
                    return View("EditVariableParentRelationShipWithPartialView", varinput);
                }

                SaveParentAndChildRelationShips(form, varinput, "Variable");

                //foreach (var item in form.AllKeys)
                //{
                //    if (item.EndsWith("parentchkbox"))
                //    {
                //        if (form.GetValue(item).RawValue.GetType().IsArray)
                //        {
                //            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);

                //            if (chkSelectedVariable) // checked
                //            {
                //                using (Db db = new Db())
                //                {
                //                    int relID = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                //                    mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == relID && c.RelationshipTypeId == 1));

                //                    if (existingVariableLabel != null) // if record exists do nothing
                //                    {
                //                    }
                //                    else  // insert record
                //                    {
                //                        //Sibling Parent
                //                        entityVariableRel = new mdmVariableREL();
                //                        mdmVariableRelInput VariableRelInput = new mdmVariableRelInput();
                //                        entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Variable_Parent);
                //                        entityVariableRel.RelationshipVariableId1 = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                //                        entityVariableRel.RelationshipVariableId2 = varinput.VariableLabelId;
                //                        entityVariableRel.CreatedByUser = User.Identity.Name;
                //                        entityVariableRel.DisplayOrder = 1;
                //                        entityVariableRel.CreatedDate = DateTime.Now;

                //                        var mdVariableRel = serviceVariableRel.Create(entityVariableRel);

                //                        //Sibling Child
                //                        entityVariableRel = new mdmVariableREL();
                //                        VariableRelInput = new mdmVariableRelInput();
                //                        entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Variable_Child);
                //                        entityVariableRel.RelationshipVariableId1 = varinput.VariableLabelId;
                //                        entityVariableRel.RelationshipVariableId2 = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                //                        entityVariableRel.CreatedByUser = User.Identity.Name;
                //                        entityVariableRel.DisplayOrder = 1;
                //                        entityVariableRel.CreatedDate = DateTime.Now;
                //                        mdVariableRel = serviceVariableRel.Create(entityVariableRel);
                //                    }
                //                }
                //            }
                //            else // if unchecked then verify if record exists then delete
                //            {
                //                using (Db db = new Db())
                //                {
                //                    int relID = Convert.ToInt32(item.Substring(0, item.IndexOf("parentchkbox")));
                //                    mdmVariableREL existingVarLbl1 = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == relID && c.RelationshipTypeId == 1 && c.DisplayOrder == 1));

                //                    if (existingVarLbl1 != null)
                //                    {
                //                        serviceVariableRel.Delete(existingVarLbl1.Id);
                //                        serviceVariableRel.Save();

                //                        int RelationshipVariableId2 = Convert.ToInt32(existingVarLbl1.RelationshipVariableId2);
                //                        mdmVariableREL existingVarLbl2 = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == existingVarLbl1.RelationshipVariableId1 && c.RelationshipVariableId2 == relID && c.RelationshipTypeId == 2));

                //                        if (existingVarLbl2 != null)
                //                        {
                //                            serviceVariableRel.Delete(existingVarLbl2.Id);
                //                            serviceVariableRel.Save();
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                //foreach (var item in form.AllKeys)
                //{
                //    if (item.EndsWith("childchkbox"))
                //    {
                //        if (form.GetValue(item).RawValue.GetType().IsArray)
                //        {
                //            bool chkSelectedVariable = Convert.ToBoolean(((string[])(form.GetValue(item).RawValue))[0]);

                //            if (chkSelectedVariable) // checked
                //            {
                //                using (Db db = new Db())
                //                {
                //                    int relID = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                //                    mdmVariableREL existingVariableLabel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == relID && c.RelationshipTypeId == 2));

                //                    if (existingVariableLabel != null) // if record exists do nothing
                //                    {
                //                    }
                //                    else  // insert record
                //                    {
                //                        //Sibling Parent
                //                        entityVariableRel = new mdmVariableREL();
                //                        mdmVariableRelInput VariableRelInput = new mdmVariableRelInput();
                //                        entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Variable_Child);
                //                        entityVariableRel.RelationshipVariableId1 = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                //                        entityVariableRel.RelationshipVariableId2 = varinput.VariableLabelId;
                //                        entityVariableRel.CreatedByUser = User.Identity.Name;
                //                        entityVariableRel.DisplayOrder = 2;
                //                        entityVariableRel.CreatedDate = DateTime.Now;

                //                        var mdVariableRel = serviceVariableRel.Create(entityVariableRel);

                //                        //Sibling Child
                //                        entityVariableRel = new mdmVariableREL();
                //                        VariableRelInput = new mdmVariableRelInput();
                //                        entityVariableRel.RelationshipTypeId = Convert.ToInt32(RelationshipTypeEnum.Variable_Parent);
                //                        entityVariableRel.RelationshipVariableId1 = varinput.VariableLabelId;
                //                        entityVariableRel.RelationshipVariableId2 = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                //                        entityVariableRel.CreatedByUser = User.Identity.Name;
                //                        entityVariableRel.DisplayOrder = 2;
                //                        entityVariableRel.CreatedDate = DateTime.Now;
                //                        mdVariableRel = serviceVariableRel.Create(entityVariableRel);
                //                    }
                //                }
                //            }
                //            else // if unchecked then verify if record exists then delete
                //            {
                //                using (Db db = new Db())
                //                {
                //                    int relID = Convert.ToInt32(item.Substring(0, item.IndexOf("childchkbox")));
                //                    mdmVariableREL existingVarLbl1 = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == relID && c.RelationshipTypeId == 1 && c.DisplayOrder == 2));

                //                    if (existingVarLbl1 != null)
                //                    {
                //                        serviceVariableRel.Delete(existingVarLbl1.Id);
                //                        serviceVariableRel.Save();

                //                        int RelationshipVariableId2 = Convert.ToInt32(existingVarLbl1.RelationshipVariableId2);
                //                        mdmVariableREL existingVarLbl2 = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId1 == existingVarLbl1.RelationshipVariableId1 && c.RelationshipVariableId2 == relID && c.RelationshipTypeId == 2));

                //                        if (existingVarLbl2 != null)
                //                        {
                //                            serviceVariableRel.Delete(existingVarLbl2.Id);
                //                            serviceVariableRel.Save();
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                Dbcon.Dispose();
                return RedirectToAction("DisplayHomePage", new { ClientId = varinput.ClientId, variableId = varinput.VariableId, succesMsg = "Data has been updated successfully" });
            }
        }


        public ActionResult MoveGridItems()
        {

            return View("MoveGridItems");
        }
        public ActionResult DeleteVariable(int clientId = 0, int variableId = 0)
        {
            mdmVariableLabelInput varInput = new mdmVariableLabelInput();
            VariableInput varInputLbl = new VariableInput();
            mdmVariableRelInput varRelInput = new mdmVariableRelInput();
            VariableInput varInputs = new VariableInput();  // model returning to view

            using (Db db = new Db())
            {
                IList<vwVariableVariableLabel> mdmVar = AlignmentHelper.EditGetAllTypes(clientId, variableId);
                if (mdmVar.Count > 0)
                {
                    foreach (var item in mdmVar)
                    {
                        mdmVariableLabel existingVariableLabel = db.VariableLabel.FirstOrDefault(c => (c.Id == item.fkVariableLabelId));
                        varInputs.GetvariableLabelInput = existingVariableLabel;
                        varInputs.VariableId = item.Id;
                        varInputs.ClientId = clientId;
                        varInputs.VariableLabelId = item.fkVariableLabelId;

                        using (DbContextFactory Dbcon = new DbContextFactory())
                        {
                            // For soft delete in mdmVariableLabel table
                            repoVariableLabel = new Repo<mdmVariableLabel>(Dbcon);
                            variableLabelMapper = new Mapper<mdmVariableLabel, mdmVariableLabelInput>();
                            serviceVariableLabel = new CrudService<mdmVariableLabel>(repoVariableLabel);
                            //varInput.variableLabelInput = varInput.variableLabelInput == null ? new NTTData.AMOPortal.WebUI.Dto.mdmVariableLabelInput() : varInput.variableLabelInput;
                            varInput.FullDescription = existingVariableLabel.FullDescription;
                            varInput.VariableLabel = existingVariableLabel.VariableLabel;
                            varInput.IsDeleted = true;
                            varInput.Id = existingVariableLabel.Id;
                            varInput.CreatedByUser = User.Identity.Name;
                            varInput.LastUpdatedUser = User.Identity.Name;
                            varInput.LastUpdatedDate = DateTime.Now;
                            variableLabelMapper.MapToEntity(varInput, serviceVariableLabel.Get(existingVariableLabel.Id));
                            serviceVariableLabel.Save();

                            // For soft delete in mdmVariable table
                            repoVariable = new Repo<mdmVariable>(Dbcon);
                            varMapper = new Mapper<mdmVariable, mdmVariableInput>();
                            serviceVariable = new CrudService<mdmVariable>(repoVariable);
                            varInputLbl.ClientId = clientId;
                            varInputLbl.varmdmLblInput = varInputLbl.varmdmLblInput == null ? new NTTData.AMOPortal.WebUI.Dto.mdmVariableInput() : varInputLbl.varmdmLblInput;
                            varInputLbl.varmdmLblInput.VariableLabelId = item.fkVariableLabelId;
                            varInputLbl.varmdmLblInput.VariableTypeId = item.fkVariableTypeId;
                            varInputLbl.varmdmLblInput.VariableCatId = item.fkVariableCategoryId;
                            varInputLbl.varmdmLblInput.IsDeleted = true;
                            varInputLbl.varmdmLblInput.Id = item.Id;
                            varInputLbl.varmdmLblInput.ClientId = clientId;
                            varInputLbl.varmdmLblInput.CreatedDate = DateTime.Now;
                            varInputLbl.varmdmLblInput.LastUpdatedDate = DateTime.Now;
                            varInputLbl.varmdmLblInput.LastUpdatedUser = User.Identity.Name;

                            varMapper.MapToEntity(varInputLbl.varmdmLblInput, serviceVariable.Get(item.Id));
                            serviceVariable.Save();

                            // For soft delete in mdmVariableREL table
                            IList<mdmVariableREL> mdmRel = AlignmentHelper.GetAllRelTypes(item.fkVariableLabelId);
                            if (mdmRel.Count > 0)
                            {
                                int Rel1 = 0; int Rel2 = 0; int DisOrder = 0;
                                foreach (var items in mdmRel)
                                {
                                    // for first row
                                    if (items.RelationshipVariableId1.HasValue) { Rel1 = (int)items.RelationshipVariableId1; }
                                    if (items.RelationshipVariableId2.HasValue) { Rel2 = (int)items.RelationshipVariableId2; }
                                    if (items.DisplayOrder.HasValue) { DisOrder = (int)items.DisplayOrder; }

                                    repoVariableRel = new Repo<mdmVariableREL>(Dbcon);
                                    variableRelMapper = new Mapper<mdmVariableREL, mdmVariableRelInput>();
                                    serviceVariableRel = new CrudService<mdmVariableREL>(repoVariableRel);
                                    varRelInput.RelationshipTypeId = items.RelationshipTypeId;
                                    varRelInput.RelationshipVariableId1 = Rel1;
                                    varRelInput.RelationshipVariableId2 = Rel2;
                                    varRelInput.DisplayOrder = DisOrder;
                                    varRelInput.IsDeleted = true;
                                    varRelInput.Id = items.Id;
                                    varRelInput.LastUpdatedUser = User.Identity.Name;
                                    varRelInput.LastUpdatedDate = DateTime.Now;
                                    variableRelMapper.MapToEntity(varRelInput, serviceVariableRel.Get(items.Id));
                                    serviceVariableRel.Save();

                                    // for second row
                                    varRelInput = new mdmVariableRelInput();
                                    mdmVariableREL mdmVariableRel = db.VariableRel.FirstOrDefault(c => (c.RelationshipVariableId2 == Rel1 && c.RelationshipVariableId1 == Rel2 && c.DisplayOrder == DisOrder));
                                    if (mdmVariableRel != null)
                                    {
                                        if (mdmVariableRel.RelationshipVariableId1.HasValue) { Rel1 = (int)mdmVariableRel.RelationshipVariableId1; }
                                        if (mdmVariableRel.RelationshipVariableId2.HasValue) { Rel2 = (int)mdmVariableRel.RelationshipVariableId2; }
                                        if (mdmVariableRel.DisplayOrder.HasValue) { DisOrder = (int)mdmVariableRel.DisplayOrder; }

                                        varRelInput.RelationshipTypeId = mdmVariableRel.RelationshipTypeId;
                                        varRelInput.RelationshipVariableId1 = Rel1;
                                        varRelInput.RelationshipVariableId2 = Rel2;
                                        varRelInput.DisplayOrder = DisOrder;
                                        varRelInput.IsDeleted = true;
                                        varRelInput.Id = mdmVariableRel.Id;
                                        varRelInput.LastUpdatedUser = User.Identity.Name;
                                        varRelInput.LastUpdatedDate = DateTime.Now;
                                        variableRelMapper.MapToEntity(varRelInput, serviceVariableRel.Get(mdmVariableRel.Id));
                                        serviceVariableRel.Save();
                                    }
                                }
                            }
                            Dbcon.Dispose();
                        }
                    }
                }
            }
            return RedirectToAction("DisplayHomePage", new { ClientId = varInputs.ClientId, succesMsg = "Data has been deleted successfully" });
        }
        // Added By Neeraj for Edit functionality-- ends
    }
}