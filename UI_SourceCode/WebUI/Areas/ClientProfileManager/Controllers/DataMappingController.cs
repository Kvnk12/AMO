//using Domain.Entities.ClientProfileManager;
using Newtonsoft.Json;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.ClientProfileManager.Dto;
using WebUI.Areas.ClientProfileManager.ViewModel;
using NTTData.AMOPortal.WebUI.Filters;
using Domain.Entities.IncidentNavigator;
using WebUI.Dto;
using log4net;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class DataMappingController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataMappingController));

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public ActionResult Index()
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            {
                var MappingTemplateViewModels = ClientProfileManagerHelper.GetAllTemplatesModel(ClientId);

                if (MappingTemplateViewModels != null)
                {

                    foreach (var template in MappingTemplateViewModels)
                    {
                        if(template.BaseMappingId.HasValue)
                        {
                            template.BaseMappingTemplateViewModel= ClientProfileManagerHelper.GetTemplateModelbyId(template.BaseMappingId.Value);
                        }
                    }
                }
                var templatesInput = new dataMappingTemplatesInput();
                templatesInput.ClientId = ClientId;
                templatesInput.templates = MappingTemplateViewModels;
                return View(templatesInput);
            }
            catch (Exception ex)
            {
                return Content("Error in getting registered template information" + ex.Message);
            }
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpPost]
        public ActionResult AddNewMappingTemplate(MappingTemplateViewModel mappingTemplateViewModel)
        {

            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            mappingTemplateViewModel.ClientId = client.ClientId;
            if (!ModelState.IsValid)
            {
                return View("AddTemplate", mappingTemplateViewModel);
            }
            if (mappingTemplateViewModel == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            mappingTemplateViewModel.CreatedBy = (int)Session["UserId"];
            mappingTemplateViewModel.UpdatedBy = (int)Session["UserId"];
            var result = ClientProfileManagerHelper.AddNewMappingTemplate(mappingTemplateViewModel);

            if (!result)
            {
                ModelState.AddModelError("CreateErrMsg", "Error creating template");
            }
            else
            {
                ModelState.AddModelError("CreateErrMsg", "Template created successfully.");
                var clientid = mappingTemplateViewModel.ClientId;
                return RedirectToAction("Index", new { ClientId = clientid });
                // return View("", clientid);
            }
            return View("AddTemplate", mappingTemplateViewModel);

        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpPost]
        public ActionResult AddNewMappingTemplate2(int drpAppId, string Description, bool IsStandard, HttpPostedFileBase httpPostedTemplateFile, string FileFormat, string FileDelimiter, int? BaseMappingId)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            MappingTemplateViewModel mappingTemplateViewModel = new MappingTemplateViewModel();
            mappingTemplateViewModel.ClientId = client.ClientId;
            mappingTemplateViewModel.Description = Description;
            mappingTemplateViewModel.StoreApplicationId = drpAppId;
            mappingTemplateViewModel.IsStandard = IsStandard;
            mappingTemplateViewModel.httpPostedTemplateFile = httpPostedTemplateFile;
            mappingTemplateViewModel.FileFormat = FileFormat;
            mappingTemplateViewModel.FileDelimiter = FileDelimiter;
            mappingTemplateViewModel.BaseMappingId = BaseMappingId;
            mappingTemplateViewModel.TemplateName = httpPostedTemplateFile.FileName;
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            mappingTemplateViewModel.CreatedBy = (int)Session["UserId"];
            mappingTemplateViewModel.UpdatedBy = (int)Session["UserId"];
            Log.Info("Hit");
            var result = ClientProfileManagerHelper.AddNewMappingTemplate(mappingTemplateViewModel);
            Log.Info("result is " +result);
      
            return RedirectToAction("Index");

        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpGet]
        public ActionResult GetDataMappings(int mappingId)
        {
            try
            {

                if (mappingId <= 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var mappingDetail = new Dto.dataMappinginput();
                //Client client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                //mappingDetail.
                mappingDetail.template = ClientProfileManagerHelper.GetTemplateModelbyId(mappingId);

                if (mappingDetail.template == null || mappingDetail.template.ClientId == null || mappingDetail.template.ClientId == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Unauthorised access to mapping template or client does not exist.");
                }
                else if(mappingDetail.template.BaseMappingId.HasValue)
                {
                    //set base template detail
                    mappingDetail.template.BaseMappingTemplateViewModel= ClientProfileManagerHelper.GetTemplateModelbyId(mappingDetail.template.BaseMappingId.Value);
                }

               mappingDetail.SourceFields = XLAndTXTFileParser.GetFileColumns(mappingDetail.template);
                var detail = ClientProfileManagerHelper.GetDataMappings(mappingId);
                string stgTable = mappingDetail.template.AppStagingTable;
                if (detail == null)
                {
                    mappingDetail.templateMapping = ClientProfileManagerHelper.GetAppColumns(stgTable, mappingId);
                   // mappingDetail.SourceFields = XLAndTXTFileParser.GetFileColumns(mappingDetail.template);
                }
                else
                {
                    mappingDetail.templateMapping = detail;
                   // mappingDetail.SourceFields = detail.Select(s => new SelectListItem { Text = s.SField, Value = s.SField }).ToList(); //XLAndTXTFileParser.GetFileColumns(mappingDetail.template);
                }
     

                return View("ViewDataMapping", mappingDetail);
            }
            catch (Exception ex)
            {
                return Content("Error in getting mapping detail for the template." + ex.Message);
            }
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public FileResult DownloadTemplate(int mappingId)
        {
            MappingTemplateViewModel mappingTemplateViewModel = ClientProfileManagerHelper.GetTemplateModelbyId(mappingId);
            if (mappingTemplateViewModel.BaseMappingId == null)
                return File(mappingTemplateViewModel.TemplateFile, System.Net.Mime.MediaTypeNames.Application.Octet, mappingTemplateViewModel.GlobalMappingIdentifier + "_" + DateTime.Now.ToShortDateString() + "." + mappingTemplateViewModel.FileFormat.ToLower());
            else
            {
                mappingTemplateViewModel.BaseMappingTemplateViewModel= ClientProfileManagerHelper.GetTemplateModelbyId(mappingTemplateViewModel.BaseMappingId.Value);
                return File(mappingTemplateViewModel.BaseMappingTemplateViewModel.TemplateFile, System.Net.Mime.MediaTypeNames.Application.Octet, mappingTemplateViewModel.GlobalMappingIdentifier + "_" + DateTime.Now.ToShortDateString() + "." + mappingTemplateViewModel.BaseMappingTemplateViewModel.FileFormat.ToLower());
            }
        }

       
        private IList<SourceFieldConcatModel> PopulateConcatenateFields(SourceDataMappingViewModel sourDatamapping)
        {
            var sourceFlds = new List<SourceFieldConcatModel>();
            if (!string.IsNullOrEmpty(sourDatamapping.FieldConcatinations))
            {
                string[] flds = sourDatamapping.FieldConcatinations.Split('^');
                if (flds.Length > 0)
                {
                    foreach (var str in flds)
                    {
                        var fld = str.Split('~');
                        if (fld.Length > 0)
                        {
                            sourceFlds.Add(new SourceFieldConcatModel
                            {
                                MappingId = sourDatamapping.MappingId,
                                SourceField = fld[0],
                                SourceDataMappingId = sourDatamapping.Id,
                                SeparatedBy = fld[1],
                                CreatedBy = (int)Session["UserId"]
                            });
                        }
                    }

                }
            }

            return sourceFlds;
        }


        private IList<SourceFieldCalculateModel> PopulateCalculationFields(SourceDataMappingViewModel sourDatamapping)
        {
            var sourceFlds = new List<SourceFieldCalculateModel>();
            if (!string.IsNullOrEmpty(sourDatamapping.FieldCalculations))
            {
                string[] flds = sourDatamapping.FieldCalculations.Split('^');
                if (flds.Length > 0)
                {
                    foreach (var str in flds)
                    {
                        var fld = str.Split('~');
                        if (fld.Length > 0)
                        {
                            var mod = new SourceFieldCalculateModel();
                            mod.Operator = fld[0];
                            mod.SourceDataMappingId = sourDatamapping.Id;
                            mod.SourceField = fld[1];
                            // if (!string.IsNullOrEmpty(fld[2]))
                           try { mod.Constant = Convert.ToDecimal(fld[2].Trim()); }
                            catch { mod.Constant = 0; }
                                
                            mod.CreatedBy = (int)Session["UserId"];
                            mod.UpdatedBy = (int)Session["UserId"];
                            mod.CreatedDt = DateTime.Now;
                            mod.UpdatedDt = DateTime.Now;
                            sourceFlds.Add(mod);
                        }
                    }

                }
            }

            return sourceFlds;
        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpPost]
        public ActionResult UpdateMappingDetail(dataMappinginput mappingInput, string btnSubmit)
        {
            ////NTTData.AMOPortal.WebUI.Dto.HomeInput menuItemsInput = (NTTData.AMOPortal.WebUI.Dto.HomeInput)Session["MenuItemsInput"];
            ////if (menuItemsInput.UserRole.ToLower() != "sysadmin" && menuItemsInput.UserRole.ToLower() != "admin")
            ////{
            ////    return Content("Not authorised to access data mapping module");
            ////}
            TempData.Clear();
            ModelState.Clear();
            if (btnSubmit == "Validate" || btnSubmit == "Acknowledge")
            {
                int count = 0;
                foreach (var item in mappingInput.templateMapping)
                {
                    if (item.IsMandatory && string.IsNullOrEmpty(item.SField) && string.IsNullOrEmpty(item.FieldConcatinations) && (string.IsNullOrEmpty(item.DefVal) || item.DefVal == "0")) //&& string.IsNullOrEmpty
                    {
                        ModelState.AddModelError(item.MappingCol + count.ToString() + "mandate", item.MappingCol + "- either source field should be mapped OR default value should be provided.");
                        count++;
                    }
                    int value;
                    if (string.IsNullOrEmpty(item.SField) && item.DataType.ToLower() == "int" && !int.TryParse(item.DefVal, out value) && !string.IsNullOrEmpty(item.DefVal))
                    {
                        ModelState.AddModelError(item.MappingCol + count.ToString() + "defvalint", item.MappingCol + "- Default value is not integer.");
                        count++;
                    }
                    DateTime dt;
                    if (string.IsNullOrEmpty(item.SField) && item.DataType.ToLower() == "datetime" && !DateTime.TryParse(item.DefVal, out dt) && !string.IsNullOrEmpty(item.DefVal))
                    {
                        ModelState.AddModelError(item.MappingCol + count.ToString() + "defvaldt", item.MappingCol + "- Default value is not datetime.");
                        count++;
                    }
                    if (!string.IsNullOrEmpty(item.DefVal) && item.DLength > 0 && item.DLength < item.DefVal.Length)
                    {
                        ModelState.AddModelError(item.MappingCol + count.ToString() + "defvalLen", item.MappingCol + "- Please check the length of default value.");
                        count++;
                    }
                    if (string.IsNullOrEmpty(item.DisplayName))
                    {
                        ModelState.AddModelError(item.MappingCol + count.ToString() + "displayval", item.MappingCol + "- Display value should not be blank.");
                        count++;
                    }

                }
            }
            var success = false;
            var isAcknowledge = false;
            if (btnSubmit == "Save" || (btnSubmit == "Acknowledge" && ModelState.IsValid))
            {

                foreach (SourceDataMappingViewModel item in mappingInput.templateMapping)
                {
                    item.SourceFieldConcats = PopulateConcatenateFields(item);
                    item.SourceFieldCalculate = PopulateCalculationFields(item);
                    item.UpdatedBy = (int)Session["UserId"];
                    //to avoid updating created while updating
                    if (item.Id <= 0)
                        item.CreatedBy = (int)Session["UserId"];
                    item.MappingId = mappingInput.template.MappingId;
                    if (btnSubmit == "Acknowledge")
                        isAcknowledge = true;
                }
                success = ClientProfileManagerHelper.AddUpdateDataMappings(mappingInput.templateMapping, isAcknowledge);
            }

            bool exist = false;            
            foreach (SourceDataMappingViewModel item in mappingInput.templateMapping)
            {
                if (item.DataMasking)
                {
                    exist = true;
                    break;
                }
            }


            if (success)
            {
                string strSuccessmsg = "";
                if (btnSubmit == "Save")
                {
                    if (!exist)
                    {
                        strSuccessmsg = "Mapping detail saved successfully (Not yet acknowledged).";
                    }
                    else
                    {
                        strSuccessmsg = "Mapping detail saved successfully (Not yet acknowledged);Please note, you have chosen one or more source fields to be masked; there could be possibility of normal date fields getting masked(not only DOB dates).";
                    }
                    TempData.Add("ValidationSuccessMsg", strSuccessmsg);
                    if (mappingInput.templateMapping[0].Id <= 0)
                    {
                        return RedirectToAction("GetDataMappings", new { mappingId = mappingInput.template.MappingId });
                    }
                }
                else
                {
                    if (!exist)
                    {
                        strSuccessmsg = "Mapping detail saved and Acknowledged successfully; please send source file to AM–Optimization tool for data upload process.";
                    }
                    else
                    {
                        strSuccessmsg = "Mapping detail saved and Acknowledged successfully; please send source file to AM–Optimization tool for data upload process. (Please note, you have chosen one or more source fields to be masked; there could be possibility of normal date fields getting masked(not only DOB dates)).";
                    }
                    TempData.Add("ValidationSuccessMsgACK", strSuccessmsg);
                    if (mappingInput.templateMapping[0].Id <= 0)
                    {
                        return RedirectToAction("GetDataMappings", new { mappingId = mappingInput.template.MappingId });
                    }
                }
            }
            else
            {
                if (btnSubmit == "Validate" && ModelState.IsValid)
                    TempData.Add("ValidationSuccessMsg", "Validation successful.");
                //else if()
                else if ((btnSubmit == "Validate" || btnSubmit == "Acknowledge") && ModelState.IsValid)
                    ModelState.AddModelError("SaveErrMsg", "Error while saving mapping detail.");
            }

            try
            {
               // var entityTemplate = ClientProfileManagerHelper.GetTemplatebyId(mappingInput.template.MappingId);
                MappingTemplateViewModel updatedTemplate = ClientProfileManagerHelper.GetTemplateModelbyId(mappingInput.template.MappingId);
                if(updatedTemplate.BaseMappingId.HasValue)
                {
                    updatedTemplate.BaseMappingTemplateViewModel= ClientProfileManagerHelper.GetTemplateModelbyId(updatedTemplate.BaseMappingId.Value);
                }
                mappingInput.SourceFields = XLAndTXTFileParser.GetFileColumns(updatedTemplate);
                mappingInput.template = updatedTemplate;
                if (mappingInput.templateMapping == null && mappingInput.templateMapping.Count <= 0)
                {
                    mappingInput.templateMapping = ClientProfileManagerHelper.GetDataMappings(mappingInput.template.MappingId);
                    if (mappingInput.templateMapping == null && mappingInput.templateMapping.Count <= 0)
                    {
                        mappingInput.templateMapping = ClientProfileManagerHelper.GetAppColumns(updatedTemplate.AppStagingTable, updatedTemplate.MappingId);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("RetrievalErrMsg", "Error while getting updated mapping detail;" + ex.Message);
            }

            return View("ViewDataMapping", mappingInput);

        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpPost]
        public ActionResult AddNewDataMapping(SourceDataMappingViewModel sourceDataMappingViewModel)
        {

            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var success = ClientProfileManagerHelper.AddUpdateDataMappings(new List<SourceDataMappingViewModel> { sourceDataMappingViewModel }, false);

            if (success)
                return View();

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        ////[CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        ////public ActionResult GetSourceFieldConcats(int SourceDataMappingId)
        ////{
        ////    if (SourceDataMappingId <= 0)
        ////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        ////    var dataMappingCalculations = ClientProfileManagerHelper.GetSourceFieldConcats(SourceDataMappingId);

        ////    return View(dataMappingCalculations);
        ////}

        ////[CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        ////[HttpPost]
        ////public ActionResult AddUpdateDataMappingCalculation(List<SourceFieldConcatModel> sourceFieldConcatsModel)
        ////{

        ////    var currentTime = DateTime.Now;
        ////    foreach (var field in sourceFieldConcatsModel)
        ////    {
        ////        field.CreatedBy = (int)Session["UserId"];
        ////        if (field.Id <= 0)
        ////            field.CreatedOn = currentTime;
        ////    }

        ////    if (sourceFieldConcatsModel == null || sourceFieldConcatsModel.Count <= 0 || !ModelState.IsValid)
        ////    {
        ////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        ////    }

        ////    var success = ClientProfileManagerHelper.AddUpdateSourceFieldConcats(sourceFieldConcatsModel);

        ////    if (success)
        ////        return View();

        ////    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        ////}

        
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public ActionResult AddNewTemplate(int ClientId)
        {
            
            MappingTemplateViewModel mappingTemplateViewModel = new MappingTemplateViewModel();
            mappingTemplateViewModel.ClientId = ClientId;
            return View("AddTemplate", mappingTemplateViewModel);
        }


    }
}
