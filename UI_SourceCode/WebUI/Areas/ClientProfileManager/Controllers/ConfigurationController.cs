using Data;
using Domain.Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebUI.Areas.ClientProfileManager;
using WebUI.Areas.ClientProfileManager.Dto;
using WebUI.Areas.IncidentNavigator;
using WebUI.Dto;
using System.IO;
using WebUI.Areas.IncidentNavigator.Dto;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class ConfigurationController : AsyncController
    {
        //
        // GET: /ClientProfileManager/Configuration/

        public ActionResult Index()
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            
            INClusterListModel mod = new INClusterListModel();
            mod.ClientId = client.ClientId;
            //mod.s = getconfVals(client.Id);
            IList<KeywordListInput> clusterword = IncidentNavigatorHelper.GetClientSpecificWordValue(client.ClientId);
            Db db = new Db();
            List<KeywordListInput> listOfClusteredWords = new List<KeywordListInput>();
            mod.listofExcludedwords = IncidentNavigatorHelper.GetClientExcludedWord(client.ClientId);
            foreach(KeywordListInput a in clusterword)
            {
                var word = a.keywords;
                bool exists = mod.listofExcludedwords.Any(x => x.Word == word);
                if (!exists)
                {
                    listOfClusteredWords.Add(a);
                }
            }
            mod.listofclusterword = listOfClusteredWords;
            var lastModified = mod.listofExcludedwords.Select(a => a.ExcludedDate).LastOrDefault();
            DateTime lastDt = Convert.ToDateTime(lastModified);
            mod.lastModified = lastDt.ToString("MM-dd-yyyy");
            var lastClusterDate = (from a in db.INClusterActivityLog
                                   where a.ClientId==client.ClientId && a.ActivityName.ToLower() == "programend" && a.ProgramName == "main.py"
                                   orderby a.CreatedDt descending
                                   select a.CreatedDt).FirstOrDefault();
           // var lastClusterDt = lastClusterDate.LastOrDefault();
            ViewBag.LastClusterDate = lastClusterDate.ToString("MM-dd-yyyy");
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var list = unitOfWork.INClusterConfigurationKey.Get().ToList();
                var checkValTable = unitOfWork.INClusterConfigurationValue.Get(g => g.ClientId == client.ClientId);
                foreach (var ke in list)
                {
                    bool chkExist = checkValTable.Where(w => w.KeyId == ke.ID).Any();
                    if (!chkExist)
                    {
                        INClusterConfigurationValue cv = new INClusterConfigurationValue();
                        cv.ClientId = client.ClientId;
                        cv.ActiveFlag = ke.ActiveFlag;
                        cv.CreatedBy = uid;
                        cv.UpdatedBy = uid;
                        cv.CreatedDt =DateTime.Now;
                        cv.UpdatedDt = DateTime.Now;
                        cv.Description = ke.Description;
                        cv.ParameterName = ke.ParameterName;
                        cv.MultiSelectFlag = ke.MultiSelectFlag;
                        cv.ParameterValue = string.IsNullOrEmpty(ke.DefaultValue) ? "" : ke.DefaultValue;
                        cv.KeyId = ke.ID;
                        unitOfWork.INClusterConfigurationValue.Insert(cv);
                        INClusterActivityLog log = new INClusterActivityLog();
                        log.ActivityName = "ClusterConfiguration_Created";
                        log.ProgramName = "ClientProfileManager->ConfigurationController->Index";
                        log.ClientId = client.ClientId;
                        log.CreatedBy = uid;
                        log.CreatedDt = DateTime.Now;
                        unitOfWork.INClusterActivityLog.Insert(log);
                        unitOfWork.Save();
                       
                    }
                }

            }
            mod.s = getconfVals(client.ClientId);
            foreach (var x in mod.s)
            {
                if (x.MultiSelectFlag == 1)
                {
                    List<SelectListItem> sl = new List<SelectListItem>();
                    List<string> s = x.ParameterValue.Split(',').ToList();
                    if (x.ParameterValue.IndexOf(",") <= 0)
                    {
                        SelectListItem sItem = new SelectListItem();
                        sItem.Text = x.Description;
                        sItem.Value = x.ParameterValue;
                        sl.Add(sItem);
                    }
                    else
                    {
                        foreach (var iii in s)
                        {
                            SelectListItem sItem = new SelectListItem();
                            sItem.Text = iii;
                            sItem.Value = iii;
                            sl.Add(sItem);
                        }
                    }
                    x.extraProp = sl;
                }

            }
            //return View("Index");
            return View("Index2", mod);
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult GetConfigValue(DataSourceRequest request)
        public static List<INConfigurationValueModel> getconfVals (int ClientId)
        {
            //Client client = (Client)Session["Client"];
            Db db = new Db();
            var list = (from v in db.INClusterConfigurationValue
                        join k in db.INClusterConfigurationKey on v.KeyId equals k.ID
                        where k.DisplayFlag == 1 && v.ClientId == ClientId && k.ActiveFlag == 1 && v.ActiveFlag == 1
                        select new INConfigurationValueModel
                        {
                            ClientId= v.ClientId,
                            Id = v.ID,
                            KeyId = v.KeyId,
                            ActiveFlag = v.ActiveFlag,
                            MultiSelectFlag = v.MultiSelectFlag,
                            ParameterName = k.DisplayName,
                            ParameterValue = v.ParameterValue,
                            Description = v.Description,
                            CreatedBy = v.CreatedBy,
                            CreatedDt = v.CreatedDt,
                            UpdatedBy = v.UpdatedBy,
                            UpdatedDt = v.UpdatedDt
                        } 
                        
                        
                        ).ToList();

            var list1 = list.Select(s => new INConfigurationValueModel
            {
                ClientId = s.ClientId,
                Id = s.Id,
                KeyId = s.KeyId,
                ActiveFlag = s.ActiveFlag,
                ParameterName = s.ParameterName,
                MultiSelectFlag = s.MultiSelectFlag,
                ParameterValue = s.ParameterValue,
                //extraProp  = s.ParameterValue.Split(',').ToList(),
                Description = s.Description,
                CreatedBy = s.CreatedBy,
                CreatedDt = s.CreatedDt,
                UpdatedBy = s.UpdatedBy,
                UpdatedDt = s.UpdatedDt
            }).ToList();
            //using (var contextFactory = new DbContextFactory())
            //{
            //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //    var list = unitOfWork.INClusterConfigurationValue.Get().Where(e => e.ClientId == client.Id && e.ActiveFlag == 1).ToList();

              return list1;
            //}

        }

        //[AcceptVerbs(HttpVerbs.Post)]


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editing_Update(INClusterListModel ConfigValue)
        {
            var strSuccessmsg = "";
            
                int uid = (int)Session["UserId"];
                //Client client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                bool result = false;
                if (ConfigValue.s != null) // && ModelState.IsValid)
                {
                    using (var contextFactory = new DbContextFactory())
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        // var dbConfigValue = unitOfWork.INClusterConfigurationValue.Get(g => g.ClientId == client.Id && g.ActiveFlag == 1).ToList();
                        var obj = ConfigValue.s.Where(e => e.MultiSelectFlag == 1);
                        if (obj.Any(e => e.ParameterValue == null))
                        {
                            strSuccessmsg = "Select the Parameter!";
                            TempData.Add("SaveErrMsg", strSuccessmsg);
                        }
                        else
                        {

                            ModelState.Clear();
                            var conf = ConfigValue.s;
                            foreach (var product in conf)
                            {
                                product.UpdatedBy = uid;
                                //if (product.extraProp.Count() < 1)
                                //{
                                //    product.ParameterValue = string.Join(",", product.extraProp);
                                //}
                                //if(product.selectedParams!=null)
                                //{
                                //    string ParamsAsc = "";
                                //    int[] Pids = Array.ConvertAll(product.selectedParams, int.Parse);
                                //    var SortedpIDS = (from element in Pids orderby element ascending select element).ToArray();

                                //    foreach (var id in SortedpIDS)
                                //    {
                                //        var pname = unitOfWork.INClusterParameterValue.Get(g => g.SeqNo == id && g.KeyId == product.KeyId).Select(g => g.ParameterValue).FirstOrDefault();
                                //        ParamsAsc = ParamsAsc + pname;
                                //    }
                                //    product.ParameterValue = ParamsAsc;
                                //}


                                result = ClientProfileManagerHelper.UpdateConfig(product);

                            }
                            INClusterActivityLog log = new INClusterActivityLog();
                            log.ActivityName = "ClusterConfiguration_Update";
                            log.ProgramName = "ClientProfileManager->ConfigurationController->Editing_Update";
                            log.ClientId = client.ClientId;
                            log.CreatedBy = uid;
                            log.CreatedDt = DateTime.Now;
                            unitOfWork.INClusterActivityLog.Insert(log);
                            unitOfWork.Save();
                            if (result)
                            {
                                strSuccessmsg = "Saved Changes Successfully.";
                                if(TempData.Count==0)
                                {
                                     TempData.Add("ValidationSuccessMsg", strSuccessmsg);
                                }
                        }

                            //if (result)
                            //{

                            //    Task.Factory.StartNew(() =>
                            //    {
                            //        ExcecuteCreateJobSp(client.ClientId, uid);


                            //        //ExcecuteDeleteSp(client.Id);
                            //    });
                            //    strSuccessmsg = "Saved Changes Successfully. Successfully invoked the ‘Cluster Reset’ job, which is in progress. Please check the re-clustered incident data after some time.";
                            //    TempData.Add("ValidationSuccessMsg", strSuccessmsg);

                            //}

                        }
                    }

                }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ExcecuteCreateJobSp()
        {
            int uid = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var strSuccessmsg = "";

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var paramName = new string[] { "ClientId" };
                var paramValue = new object[] { client.ClientId };
               var dt = unitOfWork.GetDataFromStoredProcedure("PY_PythonClusterJob ", paramName, paramValue);
                INClusterActivityLog log1 = new INClusterActivityLog();
                log1.ActivityName = "Python Cluster Job Creation";
                log1.ProgramName = "ClientProfileManager->ConfigurationController->ExcecuteCreateJobSp";
                log1.ClientId = client.ClientId;
                log1.CreatedBy = uid;
                log1.CreatedDt = DateTime.Now;
                unitOfWork.INClusterActivityLog.Insert(log1);
                //unitOfWork.Save();

                //to start deleteCluster
                var paramName1 = new string[] { "job_name " };
                string jonname = "PY_DeleteClusterJob" + client.ClientId.ToString().Trim();
                var paramValue1 = new object[] { jonname };
                 var dt1 = unitOfWork.GetDataFromStoredProcedure("msdb.dbo.sp_start_job", paramName1, paramValue1);
                INClusterActivityLog log12 = new INClusterActivityLog();
                log12.ActivityName = "Cluster Data Detele Job";
                log12.ProgramName = "ClientProfileManager->ConfigurationController->ExcecuteCreateJobSp";
                log12.ClientId = client.ClientId;
                log12.CreatedBy = uid;
                log12.CreatedDt = DateTime.Now;
                unitOfWork.INClusterActivityLog.Insert(log12);
                //unitOfWork.Save();

                //to start ClusterJob
                var paramName2 = new string[] { "job_name " };
                string jonname2 = "PY_StartClusterJob" + client.ClientId.ToString().Trim();
                var paramValue2 = new object[] { jonname2 };
                var dt2 = unitOfWork.GetDataFromStoredProcedure("msdb.dbo.sp_start_job", paramName2, paramValue2);
                INClusterActivityLog log123 = new INClusterActivityLog();
                log123.ActivityName = "Cluster Data Start Job";
                log123.ProgramName = "ClientProfileManager->ConfigurationController->ExcecuteCreateJobSp";
                log123.ClientId = client.ClientId;
                log123.CreatedBy = uid;
                log123.CreatedDt = DateTime.Now;
                unitOfWork.INClusterActivityLog.Insert(log123);
                unitOfWork.Save();

                strSuccessmsg = "Successfully invoked the ‘Cluster Reset’ job, which is in progress. Please check the re-clustered incident data after some time.";
                TempData.Add("ValidationSuccessMsg", strSuccessmsg);
                return Json(strSuccessmsg);

            }


        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(DataSourceRequest request, INConfigurationValueModel ConfigValue)
        {
            int uid = (int)Session["UserId"];

            if (ConfigValue != null && ModelState.IsValid)
            {
                //Client client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                ConfigValue.ClientId = client.ClientId;
                ConfigValue.CreatedBy = uid;
                ConfigValue.CreatedDt = DateTime.Now;
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var ss = unitOfWork.INClusterConfigurationValue.Get(g => g.KeyId == ConfigValue.KeyId && g.ClientId == client.ClientId).Any();
                    if(!ss)
                    {
                        var res = ClientProfileManagerHelper.AddConfig(ConfigValue);
                        ConfigValue.Id = res;
                    }
                    else
                    {
                        ModelState.AddModelError("Same","This Parameter Already Exists!");
                    }
                }

                    
        }

            return Json(new[] { ConfigValue }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Destroy(DataSourceRequest request, INConfigurationValueModel ConfigValue)
        {
            int uid = (int)Session["UserId"];
            if (ConfigValue != null)
            {
                var v = ClientProfileManagerHelper.DeleteConfig(ConfigValue.Id);
            }

            return Json(new[] { ConfigValue }.ToDataSourceResult(request, ModelState));
        }

        // Excluding words

        public ActionResult AddExcludeWords(string word,string delimiter)
        {
            int uid = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IncidentNavigatorHelper.AddClientExcludedWord(word,delimiter,uid,client.ClientId);

            INClusterListModel mod = new INClusterListModel();
            mod.ClientId = client.ClientId;
            mod.listofclusterword = IncidentNavigatorHelper.GetClientSpecificWordValue(client.ClientId);
            mod.listofExcludedwords = IncidentNavigatorHelper.GetClientExcludedWord(client.ClientId);
            mod.lastModified = mod.listofExcludedwords.Select(a => a.ExcludedDate).LastOrDefault();
            return PartialView("_excludeClusterView", mod);

        }
        public ActionResult DeleteExcludeWords(string id)
        {
            int uid = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IncidentNavigatorHelper.DeleteClientExcludedWord(Convert.ToInt32(id));

            INClusterListModel mod = new INClusterListModel();
            mod.ClientId = client.ClientId;
            mod.listofclusterword = IncidentNavigatorHelper.GetClientSpecificWordValue(client.ClientId);
            mod.listofExcludedwords = IncidentNavigatorHelper.GetClientExcludedWord(client.ClientId);
            return PartialView("_excludeClusterView", mod);

        }
        
        ///HElper Methods
        public static IEnumerable<SelectListItem> getParameterName()
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var paramList = unitOfWork.INClusterConfigurationKey.Get().ToList();
                var item = paramList.Where(g=>g.DisplayFlag==1).Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.DisplayName.ToString() });
                return item;
            }
        }
        // To Get the Details for Selected ParamId From KeyTable


        //public INClusterConfigurationKey getParamDetails(string Id)
        [HttpPost]
        public ActionResult getParamDetails(string Id)
        {
            int id = Convert.ToInt32(Id);
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var paramList = unitOfWork.INClusterConfigurationKey.Get().Where(e => e.ID == id).FirstOrDefault();
                return Json(paramList);
            }
        }
              

        public static IEnumerable<SelectListItem> INClusterParameterValueList(int KeyID, int ClientID)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var ddd = unitOfWork.INClusterParameterValue.Get().Where(x => x.ClientId == ClientID && x.KeyId == KeyID).ToList();
            var item = ddd.Select(p => new SelectListItem { Value = p.SeqNo.ToString(), Text = p.ParameterValue });
            return item;
        }

        public JsonResult ReadParamValues(int keyId)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var products = INClusterParameterValueList(keyId,client.ClientId);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

       
        public FileStreamResult ExportToText(string delimiter)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            INClusterListModel mod = new INClusterListModel();
            mod.listofExcludedwords = IncidentNavigatorHelper.GetClientExcludedWord(client.ClientId);
            delimiter = delimiter == "" ? "#" : delimiter;
            string text="" ;
            foreach(var a in mod.listofExcludedwords.ToList())
            {
                text=text+a.Word+delimiter;
            }
            text = text.Remove(text.Length - 1, 1);

            var fileBytes = Encoding.ASCII.GetBytes(text);
            var stream = new MemoryStream(fileBytes);
            string fileName = "ExcludedClusterWords.txt";          
            return File(stream, "text/plain", fileName);
          
            //var fileContents = Convert.FromBase64String(text);

            //return File(fileContents, "text/plain", "ExcludedClusterWords.txt");


        }

    }
}
