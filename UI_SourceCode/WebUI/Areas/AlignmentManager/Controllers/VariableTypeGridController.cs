using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using System.Reflection;
using System.Globalization;
using NTTData.AMOPortal.WebUI.Controllers;

namespace WebUI.Areas.AlignmentManager.Controllers
    {


    public class VariableTypeGridController : Cruder<VariableTypesAndMetaData, VariableTypesInput>    //BaseController  //Cruder<ClientAndAccounts, ClientAndAccountsInput>
        {

        //Entities
        private VariableTypesAndMetaData entityVariableTypesAndMetaData;

        //Service

        private CrudService<VariableTypesAndMetaData> VariableTypesAndMetaDataService;

        //Mappers

        private Mapper<VariableTypesAndMetaData, VariableTypesInput> VariableTypesAndMetaDataMapper;

        //Repo

        private Repo<VariableTypesAndMetaData> repoVariableTypesAndMetaData;



        ////For VariableType mapper

        ////Entities
        private mdmVariableType entitymdmVariableType;

        ////Service

        private CrudService<mdmVariableType> mdmVariableTypeService;

        ////Mappers

        private Mapper<mdmVariableType, mdmVariableTypeInput> mdmVariableTypeMapper;

        ////Repo

        private Repo<mdmVariableType> repomdmVariableType;







        ////For VariableCategory mapper

        ////Entities
        private mdmVariableCategory entitymdmVariableCategory;

        ////Service

        private CrudService<mdmVariableCategory> mdmVariableCategoryService;

        ////Mappers

        private Mapper<mdmVariableCategory, mdmVariableCategoryInput> mdmVariableCategoryMapper;

        ////Repo

        private Repo<mdmVariableCategory> repomdmVariableCategory;



        ////For mdmvariable

        ////Entities
        private mdmVariable entitymdmVariable;

        ////Service

        private CrudService<mdmVariable> mdmVariableService;

        ////Mappers

        private Mapper<mdmVariable, mdmVariableInput> mdmVariableMapper;

        ////Repo

        private Repo<mdmVariable> repomdmVariable;



        ////For mdmvariablemd

        ////Entities
        private mdmVariableMD entitymdmVariableMD;

        ////Service

        private CrudService<mdmVariableMD> mdmVariableMDService;

        ////Mappers

        private Mapper<mdmVariableMD, mdmVariableMdInput> mdmVariableMDMapper;

        ////Repo

        private Repo<mdmVariableMD> repomdmVariableMD;





        ////For VariableMdType mapper

        ////Entities
        private mdmVariableMdType entitymdmVariableMdType;

        ////Service

        private CrudService<mdmVariableMdType> mdmVariableMdTypeService;

        ////Mappers

        private Mapper<mdmVariableMdType, mdmVariableMdTypeInput> mdmVariableMdTypeMapper;

        ////Repo

        private Repo<mdmVariableMdType> repomdmVariableMdType;




        public VariableTypeGridController(ICrudService<VariableTypesAndMetaData> service, IMapper<VariableTypesAndMetaData, VariableTypesInput> v)
            : base(service, v)
            {


            DbContextFactory Dbcon = new DbContextFactory();
            repoVariableTypesAndMetaData = new Repo<VariableTypesAndMetaData>(Dbcon);
            VariableTypesAndMetaDataMapper = new Mapper<VariableTypesAndMetaData, VariableTypesInput>();

            entityVariableTypesAndMetaData = new VariableTypesAndMetaData();

            VariableTypesAndMetaDataService = new CrudService<VariableTypesAndMetaData>(repoVariableTypesAndMetaData);

            ///Setup mdmVariableType Service
            ///

            repomdmVariableType = new Repo<mdmVariableType>(Dbcon);
            mdmVariableTypeMapper = new Mapper<mdmVariableType, mdmVariableTypeInput>();
            entitymdmVariableType = new mdmVariableType();
            mdmVariableTypeService = new CrudService<mdmVariableType>(repomdmVariableType);



            repomdmVariableCategory = new Repo<mdmVariableCategory>(Dbcon);
            mdmVariableCategoryMapper = new Mapper<mdmVariableCategory, mdmVariableCategoryInput>();
            entitymdmVariableCategory = new mdmVariableCategory();
            mdmVariableCategoryService = new CrudService<mdmVariableCategory>(repomdmVariableCategory);



            repomdmVariableMdType = new Repo<mdmVariableMdType>(Dbcon);
            mdmVariableMdTypeMapper = new Mapper<mdmVariableMdType, mdmVariableMdTypeInput>();
            entitymdmVariableMdType = new mdmVariableMdType();
            mdmVariableMdTypeService = new CrudService<mdmVariableMdType>(repomdmVariableMdType);


            repomdmVariable = new Repo<mdmVariable>(Dbcon);
            mdmVariableMapper = new Mapper<mdmVariable, mdmVariableInput>();
            entitymdmVariable = new mdmVariable();
            mdmVariableService = new CrudService<mdmVariable>(repomdmVariable);



            repomdmVariableMD = new Repo<mdmVariableMD>(Dbcon);
            mdmVariableMDMapper = new Mapper<mdmVariableMD, mdmVariableMdInput>();
            entitymdmVariableMD = new mdmVariableMD();
            mdmVariableMDService = new CrudService<mdmVariableMD>(repomdmVariableMD);





            }



        public ActionResult DisplayVarTypeDetails(int VarTypeid = 0, string vartypeCreate = "", string GridToDisplay = "VartypeGrid")
        {

        VariableTypesInput variableTypesInput = new VariableTypesInput();
        variableTypesInput.mdmVariableTypeInput = new mdmVariableTypeInput();
        variableTypesInput.GridToDisplay = GridToDisplay;
        variableTypesInput.VarTypeCreate = vartypeCreate;
        if (VarTypeid > 0)
            {
            variableTypesInput.VarTypeId = VarTypeid;
            variableTypesInput.mdmVariableTypeInput = mdmVariableTypeMapper.MapToInput(mdmVariableTypeService.Get(VarTypeid));
            }
        if (TempData["Success"] != null)
            {
            ModelState.AddModelError("Success", "Data has been saved successfully");
            }
            return View("VariableTypeGrid", variableTypesInput);

        }



        public ActionResult DisplayVarCatDetails(int VarCatId = 0, string VarCatCreate = "", string GridToDisplay="VarcatGrid")
            {

            VariableTypesInput variableTypesInput = new VariableTypesInput();
            variableTypesInput.GridToDisplay = GridToDisplay;
            variableTypesInput.mdmVariableCategoryInput = new mdmVariableCategoryInput();
            variableTypesInput.VarCatCreate = VarCatCreate;
            if (VarCatId > 0)
                {
                variableTypesInput.VarCatId = VarCatId;
                variableTypesInput.mdmVariableCategoryInput = mdmVariableCategoryMapper.MapToInput(mdmVariableCategoryService.Get(VarCatId));
                }
            if (TempData["Success"] != null)
                {
                ModelState.AddModelError("Success", "Data has been saved successfully");
                }
            return View("VariableTypeGrid", variableTypesInput);

            }



        public ActionResult DisplayMetaDataDetails(int MetaDataId = 0, string MetaDataCreate = "", string GridToDisplay="VarmetaGrid")
            {

            VariableTypesInput variableTypesInput = new VariableTypesInput();
            variableTypesInput.GridToDisplay = GridToDisplay;
            variableTypesInput.mdmVariableMdTypeInput = new mdmVariableMdTypeInput();
            variableTypesInput.MetaDataCreate = MetaDataCreate;
            if (MetaDataId > 0)
                {
                variableTypesInput.MetaDataId = MetaDataId;
                variableTypesInput.mdmVariableMdTypeInput = mdmVariableMdTypeMapper.MapToInput(mdmVariableMdTypeService.Get(MetaDataId));
                }
            if (TempData["Success"] != null)
                {
                ModelState.AddModelError("Success", "Data has been saved successfully");
                }
            return View("VariableTypeGrid", variableTypesInput);

            }


        


        [HttpGet]
        public ActionResult DisplayVariableTypes(string GridToDisplay="")
            {
            VariableTypesInput variableTypesInput = new VariableTypesInput();
            variableTypesInput.GridToDisplay = GridToDisplay;
            return View("VariableTypeGrid", variableTypesInput);
            }


        //public ActionResult DeleteVarType(VariableTypesInput inputs, int VartypeId = 0)
        //{

        //if (ViewData.ModelState.ContainsKey("Success"))
        //    {
        //    ViewData.ModelState.Keys.Remove("Success");

        //    }


        //if (VartypeId > 0)
        //    {
        //    IEnumerable<mdmVariable> mdcatList = mdmVariableService.GetAll().Where(c => c.VariableTypeId == VartypeId);
        //    if (mdcatList.Count() > 0)
        //        {
        //            ModelState.AddModelError("Success", "Variable Type already Exists");
        //            return View("VariableTypeGrid", inputs);
        //            //return RedirectToAction("DisplayVarTypeDetails", new { VartypeId = VartypeId, GridToDisplay = "VartypeGrid" });
        //        }

        //    }
        //else
        //    {

        //    //TempData["Success"] = "Data has been saved successfully";
            
        //    }
        //return RedirectToAction("DisplayVarTypeDetails", new { GridToDisplay = "VartypeGrid" });
        //}

        [HttpPost]
        public ActionResult SaveVarTypes(VariableTypesInput inputs, int VarTypeId = 0, string vartypeCreate = "", string GridToDisplay = "VartypeGrid", string command="")
        {

        inputs.GridToDisplay = GridToDisplay;
        if (!ModelState.IsValid)
            {
            if (ViewData.ModelState.ContainsKey("Success"))
            {
                ViewData.ModelState.Keys.Remove("Success");
                    
            }
            return View("VariableTypeGrid", inputs);
            }

        if (command == "delete")
        {


        if (VarTypeId > 0)
            {
            IEnumerable<mdmVariable> mdcatList = mdmVariableService.GetAll().Where(c => c.VariableTypeId == VarTypeId);
            if (mdcatList.Count() > 0)
                {
                ModelState.AddModelError("Variable", "Variable Type is being referred by other Variables, this cannot not deleted.");
                inputs.VarTypeId = VarTypeId;
                inputs.mdmVariableTypeInput.Id = VarTypeId;
                inputs.GridToDisplay = "VartypeGrid";
                return View("VariableTypeGrid", inputs);
                //return View("VariableTypeGrid", inputs);
                //return RedirectToAction("DisplayVarTypeDetails", new { VartypeId = VartypeId, GridToDisplay = "VartypeGrid" });
                }
            else {
 
                    //Delete Record
                    mdmVariableTypeService.Delete(VarTypeId);    
                    TempData["Success"] = "Data has been saved successfully";
                    return RedirectToAction("DisplayVarTypeDetails", new { GridToDisplay = "VartypeGrid" });
                
                }

            }
        else
            {

            }
        }


            if (inputs.VarTypeCreate != "" && VarTypeId == 0)
                {


                //New Entry
                //Check for Existing Entry before Insert


                IEnumerable<mdmVariableType> mdcatList = mdmVariableTypeService.GetAll().Where(c => c.VariableType.ToLower() == inputs.mdmVariableTypeInput.VariableType.ToLower());
                if (mdcatList.Count() > 0)
                    {

                    ModelState.AddModelError("Variable", "Variable Type already Exists");

                    return View("VariableTypeGrid", inputs);
                    }

                //New Entry
                inputs.mdmVariableTypeInput.CreatedByUser = User.Identity.Name;
                inputs.mdmVariableTypeInput.CreatedDate = DateTime.Now;
                var newVarType = mdmVariableTypeService.Create(mdmVariableTypeMapper.MapToEntity(inputs.mdmVariableTypeInput, entitymdmVariableType));
                inputs.mdmVariableTypeInput.Id = Convert.ToInt32(newVarType);
                


                }
            else
            {

            IEnumerable<mdmVariableType> mdcatList = mdmVariableTypeService.GetAll().Where(c => c.Id != VarTypeId  &&  c.VariableType.ToLower() == inputs.mdmVariableTypeInput.VariableType.ToLower());
            if (mdcatList.Count() > 0)
                {

                ModelState.AddModelError("Variable", "Variable Type already Exists");

                return View("VariableTypeGrid", inputs);
                }


            inputs.mdmVariableTypeInput.Id = VarTypeId;
            inputs.mdmVariableTypeInput.LastUpdatedUser= User.Identity.Name;
            inputs.mdmVariableTypeInput.LastUpdatedDate = DateTime.Now;
            
            mdmVariableType mdmtype =   mdmVariableTypeService.Get(VarTypeId);
            inputs.mdmVariableTypeInput.CreatedDate = mdmtype.CreatedDate;
            inputs.mdmVariableTypeInput.CreatedByUser = mdmtype.CreatedByUser;
            mdmVariableTypeMapper.MapToEntity(inputs.mdmVariableTypeInput, mdmtype);
                        mdmVariableTypeService.Save();


            }

            TempData["Success"] = "Data has been saved successfully";


            return RedirectToAction("DisplayVarTypeDetails", new { VarTypeId = inputs.mdmVariableTypeInput.Id});
        }


        [HttpPost]
        public ActionResult SaveCatTypes(VariableTypesInput inputs, int VarCatId = 0, string VarCatCreate = "", string GridToDisplay = "VarcatGrid", string command = "")
            {

            inputs.GridToDisplay = GridToDisplay;
            if (!ModelState.IsValid)
                {
                if (ViewData.ModelState.ContainsKey("Success"))
                    {
                    ViewData.ModelState.Keys.Remove("Success");

                    }
                return View("VariableTypeGrid", inputs);
                }


            if (command == "delete")
                {


                if (VarCatId > 0)
                    {
                    IEnumerable<mdmVariable> mdcatList = mdmVariableService.GetAll().Where(c => c.VariableCatId == VarCatId);
                    if (mdcatList.Count() > 0)
                        {
                        ModelState.AddModelError("Variable", "Variable Type is being referred by other Variables, this cannot not deleted.");
                        inputs.VarCatId = VarCatId;
                        inputs.mdmVariableCategoryInput.Id = VarCatId;
                        inputs.GridToDisplay = "VarcatGrid";
                        return View("VariableTypeGrid", inputs);
                        //return View("VariableTypeGrid", inputs);
                        //return RedirectToAction("DisplayVarTypeDetails", new { VartypeId = VartypeId, GridToDisplay = "VartypeGrid" });
                        }
                    else
                        {

                        //Delete Record
                        mdmVariableCategoryService.Delete(VarCatId);
                        TempData["Success"] = "Data has been saved successfully";
                        return RedirectToAction("DisplayVarTypeDetails", new { GridToDisplay = "VarcatGrid" });

                        }

                    }
                else
                    {

                    }
                }




            if (inputs.VarCatCreate != "" && VarCatId == 0)
                {







                //New Entry
                //Check for Existing Entry before Insert


                IEnumerable<mdmVariableCategory> mdcatList = mdmVariableCategoryService.GetAll().Where(cat =>  cat.VariableCategory.ToLower() == inputs.mdmVariableCategoryInput.VariableCategory.ToLower());
                if (mdcatList.Count() > 0)
                    {

                    ModelState.AddModelError("Category", "Category Name already Exists");

                    return View("VariableTypeGrid", inputs);
                    }



                inputs.mdmVariableCategoryInput.CreatedByUser = User.Identity.Name;
                inputs.mdmVariableCategoryInput.CreatedDate = DateTime.Now;
                var newCatType = mdmVariableCategoryService.Create(mdmVariableCategoryMapper.MapToEntity(inputs.mdmVariableCategoryInput, entitymdmVariableCategory));
                inputs.mdmVariableCategoryInput.Id = Convert.ToInt32(newCatType);



                }
            else
                {


                mdmVariableCategory mdmcat = mdmVariableCategoryService.Get(VarCatId);

                IEnumerable<mdmVariableCategory> mdcatList =   mdmVariableCategoryService.GetAll().Where(cat => cat.Id != VarCatId && cat.VariableCategory.ToLower() == inputs.mdmVariableCategoryInput.VariableCategory.ToLower());
                if (mdcatList.Count() > 0)
                {

                ModelState.AddModelError("Category", "Category Name already Exists");

                return View("VariableTypeGrid", inputs);
                    }

                // //Edit Existing Contract


                inputs.mdmVariableCategoryInput.Id = VarCatId;
                inputs.mdmVariableCategoryInput.LastUpdatedUser = User.Identity.Name;
                inputs.mdmVariableCategoryInput.LastUpdatedDate = DateTime.Now;

                
                inputs.mdmVariableCategoryInput.CreatedDate = mdmcat.CreatedDate;
                inputs.mdmVariableCategoryInput.CreatedByUser = mdmcat.CreatedByUser;
                mdmVariableCategoryMapper.MapToEntity(inputs.mdmVariableCategoryInput, mdmcat);
                mdmVariableCategoryService.Save();


                }

            TempData["Success"] = "Data has been saved successfully";


            return RedirectToAction("DisplayVarCatDetails", new { VarCatId = inputs.mdmVariableCategoryInput.Id });
            }

         [HttpPost]
        public ActionResult SaveMetaDataTypes(VariableTypesInput inputs, int MetaDataId = 0, string MetaDataCreate = "", string GridToDisplay = "VarmetaGrid", string command = "")
            {

            inputs.GridToDisplay = GridToDisplay;
            if (!ModelState.IsValid)
                {
                if (ViewData.ModelState.ContainsKey("Success"))
                    {
                    ViewData.ModelState.Keys.Remove("Success");

                    }
                return View("VariableTypeGrid", inputs);
                }


            if (command == "delete")
                {


                if (MetaDataId > 0)
                    {
                    IEnumerable<mdmVariableMD> mdcatList = mdmVariableMDService.GetAll().Where(c => c.MetaDataData == MetaDataId);
                    if (mdcatList.Count() > 0)
                        {
                        ModelState.AddModelError("Variable", "Meta Data Type is being referred by other Variables, this cannot not deleted.");
                        inputs.MetaDataId = MetaDataId;
                        inputs.mdmVariableMdTypeInput.Id = MetaDataId;
                        inputs.GridToDisplay = "VarmetaGrid";
                        return View("VariableTypeGrid", inputs);
                        //return View("VariableTypeGrid", inputs);
                        //return RedirectToAction("DisplayVarTypeDetails", new { VartypeId = VartypeId, GridToDisplay = "VartypeGrid" });
                        }
                    else
                        {

                        //Delete Record
                        mdmVariableMdTypeService.Delete(MetaDataId);
                        TempData["Success"] = "Data has been saved successfully";
                        return RedirectToAction("DisplayVarTypeDetails", new { GridToDisplay = "VarmetaGrid" });

                        }

                    }
                else
                    {

                    }
                }





            if (inputs.MetaDataCreate != "" && MetaDataId == 0)
                {

                IEnumerable<mdmVariableMdType> mdcatList = mdmVariableMdTypeService.GetAll().Where(c => c.TypeName.ToLower() == inputs.mdmVariableMdTypeInput.TypeName.ToLower());
                if (mdcatList.Count() > 0)
                    {

                    ModelState.AddModelError("MetaData", "Meta Data Type already Exists");

                    return View("VariableTypeGrid", inputs);
                    }

                //New Entry
                inputs.mdmVariableMdTypeInput.CreatedByUser = User.Identity.Name;
                inputs.mdmVariableMdTypeInput.CreatedDate = DateTime.Now;
                var newMdType = mdmVariableMdTypeService.Create(mdmVariableMdTypeMapper.MapToEntity(inputs.mdmVariableMdTypeInput, entitymdmVariableMdType));
                inputs.mdmVariableMdTypeInput.Id = Convert.ToInt32(newMdType);



                }
            else
                {


                IEnumerable<mdmVariableMdType> mdcatList = mdmVariableMdTypeService.GetAll().Where(c => c.Id != MetaDataId && c.TypeName.ToLower() == inputs.mdmVariableMdTypeInput.TypeName.ToLower());
                if (mdcatList.Count() > 0)
                    {

                    ModelState.AddModelError("MetaData", "Meta Data Type already Exists");

                    return View("VariableTypeGrid", inputs);
                    }

                inputs.mdmVariableMdTypeInput.Id = MetaDataId;
                inputs.mdmVariableMdTypeInput.LastUpdatedUser = User.Identity.Name;
                inputs.mdmVariableMdTypeInput.LastUpdatedDate = DateTime.Now;

                mdmVariableMdType mdType = mdmVariableMdTypeService.Get(MetaDataId);
                inputs.mdmVariableMdTypeInput.CreatedDate = mdType.CreatedDate;
                inputs.mdmVariableMdTypeInput.CreatedByUser = mdType.CreatedByUser;
                mdmVariableMdTypeMapper.MapToEntity(inputs.mdmVariableMdTypeInput, mdType);
                mdmVariableMdTypeService.Save();


                }

            TempData["Success"] = "Data has been saved successfully";


            return RedirectToAction("DisplayMetaDataDetails", new { MetaDataId = inputs.mdmVariableMdTypeInput.Id });
            }

        
        }



    }