using System;
using System.Collections.Generic;
using Domain.Abstract;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System.Linq;
using System.Web;
using Domain.Entities.ClientProfileManager;
using System.Web.Mvc;
using Data;
using Domain.Entities.PortfolioAnalyzer;

namespace NTTData.AMOPortal.WebUI.Dto
{
    public static class GetDropDownListItems
    {

        public static IEnumerable<SelectListItem> CountryList
        {
            get
            {
                //DbContextFactory Context = new DbContextFactory();
                //Repo<NTTData.AMOPortal.Domain.Entities.Countries> Countries = new Data.Repo<Domain.Entities.Countries>(Context);
                //List<NTTData.AMOPortal.Domain.Entities.Countries> CountryList = Countries.GetAll().ToList();
                var db = new Db();
                var CountryList = (from c in db.Countries select new { c.Id, c.CountryName }).ToList();
                    
                var Items = CountryList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.CountryName.ToString()
                });
                return Items;

            }
        }


        public static IEnumerable<SelectListItem> UserApplications(int userid)
        {

            ////DbContextFactory Context = new DbContextFactory();
            ////Repo<NTTData.AMOPortal.Domain.Entities.AppStoreApplications> apps = new Data.Repo<Domain.Entities.AppStoreApplications>(Context);
            ////Repo<NTTData.AMOPortal.Domain.Entities.UserApplications> userApps = new Data.Repo<Domain.Entities.UserApplications>(Context);
            ////Repo<NTTData.AMOPortal.Domain.Entities.UserProfile> users = new Data.Repo<Domain.Entities.UserProfile>(Context);
            var db = new Db();
            
            var usr = (from t in db.UserProfiles where t.Id == userid select t).FirstOrDefault();

            //Return all apps for super user
            if (usr.RoleId.ToLower() == "superuser")
            {
                var appsList = (from app in db.AppStore
                               where app.IsDeleted == false
                               select new { Value = app.Id, Text = app.ApplicationName }).ToList();
                var Items = appsList.Select(lst => new SelectListItem
                {
                    Value = lst.Value.ToString(),
                    Text = lst.Text.ToString()
                }).GroupBy(g => g.Value, (key, group) => group.First()).ToList();
                return Items;
            }
            else
            {

                var appsList = (from uApp in db.UserApplications
                               join app in db.AppStore on uApp.ApplicationId equals app.Id
                               where uApp.UserId == userid && app.IsDeleted == false
                               select new { Value = app.Id, Text = app.ApplicationName }).ToList();

                var Items = appsList.Select(lst => new SelectListItem
                {
                    Value = lst.Value.ToString(),
                    Text = lst.Text.ToString()
                }).GroupBy(g => g.Value, (key, group) => group.First()).ToList();
                return Items;
            }
        }

        public static IEnumerable<SelectListItem> UserApplications(int userid, int clientid)
        {
            Db db = new Db();

            //DbContextFactory Context = new DbContextFactory();
            //Repo<NTTData.AMOPortal.Domain.Entities.AppStoreApplications> apps = new Data.Repo<Domain.Entities.AppStoreApplications>(Context);
            //Repo<NTTData.AMOPortal.Domain.Entities.UserApplications> userApps = new Data.Repo<Domain.Entities.UserApplications>(Context);
            //Repo<NTTData.AMOPortal.Domain.Entities.UserProfile> users = new Data.Repo<Domain.Entities.UserProfile>(Context);
           // var userApps = (from t in db.UserApplications where t.UserId == userid && t.ClientId==clientid select t).ToList();
           // var apps = (from t in db.AppStore select t).ToList();
            var usr = (from t in db.UserProfiles where t.Id==userid select t).FirstOrDefault();

            //Return all apps for super user
            if (usr.RoleId.ToLower() == "superuser")
            {
                var appsList = (from app in db.AppStore 
                               where app.IsDeleted == false
                               select new { Value = app.Id, Text = app.ApplicationName }).ToList();
                var Items = appsList.Select(lst => new SelectListItem
                {
                    Value = lst.Value.ToString(),
                    Text = lst.Text.ToString()
                }).GroupBy(g => g.Value, (key, group) => group.First()).ToList();
                return Items;
            }
            else
            {
                var appsList = from uApp in db.UserApplications
                               join app in db.AppStore on uApp.ApplicationId equals app.Id
                               where uApp.UserId == userid && uApp.ClientId == clientid && app.IsDeleted == false
                               select new { Value = app.Id, Text = app.ApplicationName };

                var Items = appsList.Select(lst => new SelectListItem
                {
                    Value = lst.Value.ToString(),
                    Text = lst.Text.ToString()
                }).GroupBy(g => g.Value, (key, group) => group.FirstOrDefault()).ToList();
                return Items;
            }
        }

        public static IEnumerable<SelectListItem> UserClients(int userid)
        {
            ////DbContextFactory Context = new DbContextFactory();
            ////Repo<NTTData.AMOPortal.Domain.Entities.Client> clients = new Data.Repo<Domain.Entities.Client>(Context);
            ////Repo<NTTData.AMOPortal.Domain.Entities.UserApplications> userApps = new Data.Repo<Domain.Entities.UserApplications>(Context);
            ////Repo<NTTData.AMOPortal.Domain.Entities.UserProfile> users = new Data.Repo<Domain.Entities.UserProfile>(Context);
            ////// List<SelectListItem> appsList = userApps.Where(w => w.UserId == userid).ToList();
            var db = new Db();
            
            var usr =(from u in db.UserProfiles where u.Id==userid select u ).FirstOrDefault();

            if (usr.RoleId.ToLower() == "superuser")
            {
                var clientList =( from cl in db.Clients
                                 where cl.IsDeleted == false
                                 select new { Value = cl.Id, Text = cl.ClientName }).ToList();
                return clientList.Select(lst => new SelectListItem
                {
                    Value = lst.Value.ToString(),
                    Text = lst.Text.ToString()
                }).GroupBy(g => g.Value, (key, group) => group.First()).OrderBy(o => o.Text).ToList();

            }
            else
            {
                var clientList = (from uApp in db.UserApplications
                                 join cl in db.Clients on uApp.ClientId equals cl.Id
                                 where uApp.UserId == userid && cl.IsDeleted==false
                                 select new { Value = cl.Id, Text = cl.ClientName }).ToList();
                return clientList.Select(lst => new SelectListItem
                {
                    Value = lst.Value.ToString(),
                    Text = lst.Text.ToString()
                }).GroupBy(g => g.Value, (key, group) => group.First()).OrderBy(o=>o.Text).ToList();
            }
            
           // return Items;
        }

        public static SelectListItem UserRoleForClient(int userid, int clientid)
        {
            ////DbContextFactory Context = new DbContextFactory();
            ////Repo<NTTData.AMOPortal.Domain.Entities.UserRole> roles = new Data.Repo<Domain.Entities.UserRole>(Context);
            ////Repo<NTTData.AMOPortal.Domain.Entities.UserApplications> userApps = new Data.Repo<Domain.Entities.UserApplications>(Context);
            ////// List<SelectListItem> appsList = userApps.Where(w => w.UserId == userid).ToList();
            Db db = new Db();

            if (clientid > 0)
            {
                ////var clientList = from uApp in userApps.GetAll().ToList()
                ////                 join r in roles.GetAll().ToList() on uApp.RoleId equals r.Id
                ////                 where uApp.UserId == userid && uApp.ClientId == clientid && uApp.isDeleted==false
                ////                 select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }
                ////                  ;
                ///
                var rolelist = (from u in db.UserApplications
                                join r in db.UserRoles on u.RoleId equals r.Id
                                where u.ClientId == clientid && u.UserId == userid && u.IsDeleted == false
                                select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }).ToList();
                var Item = rolelist.Select(lst => new
                {
                    lst.Value,
                    lst.Text,
                    lst.Weight
                }).OrderByDescending(o => o.Weight).FirstOrDefault(); //  role with highest weightage will be considered for a client 
                if (Item != null)
                    return new SelectListItem { Value = Item.Value.ToString(), Text = Item.Text };
                else
                    return new SelectListItem { Value = "", Text = "user" }; ;
            }
            else
            {
                //var clientList = from uApp in userApps.GetAll().ToList()
                //                 join r in roles.GetAll().ToList() on uApp.RoleId equals r.Id
                //                 where uApp.UserId == userid && uApp.isDeleted == false
                //                 select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }
                //                  ;
                var rolelist = (from u in db.UserApplications
                                join r in db.UserRoles on u.RoleId equals r.Id
                                where u.ClientId == clientid && u.UserId == userid && u.IsDeleted == false
                                select new { Value = r.Id, Text = r.RoleName, Weight = r.Weightage }).ToList();

                var Item = rolelist.Select(lst => new
                {
                    lst.Value,
                    lst.Text,
                    lst.Weight
                }).OrderByDescending(o => o.Weight).FirstOrDefault();

                return new SelectListItem { Value = Item.Value.ToString(), Text = Item.Text };
            }

        }

        public static IEnumerable<SelectListItem> ClientList
        {
            get
            {
                ////DbContextFactory Context = new DbContextFactory();
                ////Repo<NTTData.AMOPortal.Domain.Entities.Client> clients = new Data.Repo<Domain.Entities.Client>(Context);
                ////List<NTTData.AMOPortal.Domain.Entities.Client> clientList = clients.GetAll().Where(i => i.IsDeleted == false).OrderBy(m => m.ClientName).ToList();
                var db = new Db();
                var clientList = (from c in db.Clients where c.IsDeleted == false select c).ToList();
                var Items = clientList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.ClientName.ToString()
                }).OrderBy(m => m.Text).ToList();
                return Items;

            }
        }


        public static IEnumerable<SelectListItem> StandardSourceTemplates()
        {
            //get
            //{
            var db = new Db();

            // var templatelist = new UnitOfWorks(contextFactory.GetContext());
            var stdTemplatesList = (from t in db.MappingTemplate where t.IsDeleted == false && t.IsStandard == true && t.BaseMappingId == null select t).ToList();
                //unitOfWork.MappingTemplate.Get().Where(i => i.IsDeleted == false && i.IsStandard == true && i.BaseMappingId == null).ToList();
              

                var Items = stdTemplatesList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.TemplateName.ToString()
                });
                return Items;
            //}
        }

        public static IEnumerable<SelectListItem> SourceTemplateFormats
        {
            get
            {
                List<string> stdTemplatesList = new List<string> { "TXT", "XLSX" };
                var Items = stdTemplatesList.Select(item => new SelectListItem
                {
                    Value = item.ToString(),
                    Text = item.ToString()
                });
                return Items;
            }
        }

        public static IEnumerable<SelectListItem> ScheduleTypes
        {
            get
            {
                List<string> stdTypes = new List<string> { "DAILY", "WEEKLY", "MONTHLY" };
                var Items = stdTypes.Select(item => new SelectListItem
                {
                    Value = item.ToString(),
                    Text = item.ToString()
                });
                return Items;
            }
        }

        public static IEnumerable<SelectListItem> WeekDays
        {
            get
            {
                List<string> stdTypes = new List<string> { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" };
                var Items = stdTypes.Select(item => new SelectListItem
                {
                    Value = item.ToString(),
                    Text = item.ToString()
                });
                return Items;
            }
        }
        public static IEnumerable<SelectListItem> MonthDays
        {
            get
            {
                List<string> stdTypes = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9","10", "11", "12", "13", "14", "15", "16", "17", "18",
                    "19" ,"20", "21", "22", "23", "24", "25", "26", "27", "28", "29","30", "31" };
                var Items = stdTypes.Select(item => new SelectListItem
                {
                    Value = item.ToString(),
                    Text = item.ToString()
                });
                return Items;
            }
        }

        //////public static IEnumerable<SelectListItem> PAReportList(int ClientId)
        //////{
        //////    DbContextFactory Context = new DbContextFactory();

        //////    Repo<NTTData.AMOPortal.Domain.Entities.ReportConfiguration> repoHeatMaps = new Data.Repo<Domain.Entities.ReportConfiguration>(Context);
        //////    List<NTTData.AMOPortal.Domain.Entities.ReportConfiguration> HeatMapReports = repoHeatMaps.GetAll().ToList(); //.Where(a => a.ClientId == ClientId).ToList();


        //////    //Repo<NTTData.AMOPortal.Domain.Entities.mdmHeatmapConfig> repoHeatMaps = new Data.Repo<Domain.Entities.mdmHeatmapConfig>(Context);
        //////    //List<NTTData.AMOPortal.Domain.Entities.mdmHeatmapConfig> HeatMapReports = repoHeatMaps.GetAll().ToList(); //.Where(a => a.ClientId == ClientId).ToList();



        //////    //var Items = HeatMapReports.Where(c => c.ClientId == ClientId && c.ReportTypeID == 2).
        //////    var Items = HeatMapReports.Where(c => c.ClientId == ClientId && c.IsDeleted == false).
        //////        Select(CList => new SelectListItem
        //////        {
        //////            Value = CList.Id.ToString(),
        //////            Text = CList.ReportName.ToString()
        //////        });
        //////    return Items;

        //////}

        ////public static IEnumerable<SelectListItem> PAReportNames(int ClientId)
        ////{
        ////    DbContextFactory Context = new DbContextFactory();

        ////    Repo<NTTData.AMOPortal.Domain.Entities.ReportConfiguration> repoHeatMaps = new Data.Repo<Domain.Entities.ReportConfiguration>(Context);
        ////    List<NTTData.AMOPortal.Domain.Entities.ReportConfiguration> HeatMapReports = repoHeatMaps.GetAll().ToList(); //.Where(a => a.ClientId == ClientId).ToList();


        ////    //Repo<NTTData.AMOPortal.Domain.Entities.mdmHeatmapConfig> repoHeatMaps = new Data.Repo<Domain.Entities.mdmHeatmapConfig>(Context);
        ////    //List<NTTData.AMOPortal.Domain.Entities.mdmHeatmapConfig> HeatMapReports = repoHeatMaps.GetAll().ToList(); //.Where(a => a.ClientId == ClientId).ToList();



        ////    //var Items = HeatMapReports.Where(c => c.ClientId == ClientId && c.ReportTypeID == 2).
        ////    var Items = HeatMapReports.Where(c => c.ClientId == ClientId && c.IsDeleted == false).
        ////        Select(CList => new SelectListItem
        ////        {
        ////            Value = CList.Id.ToString(),
        ////            Text = CList.ReportName
        ////        });
        ////    return Items;

        ////}


        ////public static IEnumerable<SelectListItem> HeatMapReportNames(int ClientId)
        ////{
        ////    DbContextFactory Context = new DbContextFactory();

        ////    Repo<NTTData.AMOPortal.Domain.Entities.ReportConfiguration> repoHeatMaps = new Data.Repo<Domain.Entities.ReportConfiguration>(Context);
        ////    List<NTTData.AMOPortal.Domain.Entities.ReportConfiguration> HeatMapReports = repoHeatMaps.GetAll().ToList(); //.Where(a => a.ClientId == ClientId).ToList();


        ////    //Repo<NTTData.AMOPortal.Domain.Entities.mdmHeatmapConfig> repoHeatMaps = new Data.Repo<Domain.Entities.mdmHeatmapConfig>(Context);
        ////    //List<NTTData.AMOPortal.Domain.Entities.mdmHeatmapConfig> HeatMapReports = repoHeatMaps.GetAll().ToList(); //.Where(a => a.ClientId == ClientId).ToList();



        ////    var Items = HeatMapReports.Where(c => c.ClientId == ClientId && (c.ReportTypeID == 1 || c.ReportTypeID == 3) && c.IsDeleted == false).
        ////        Select(CList => new SelectListItem
        ////        {
        ////            Value = CList.Id.ToString(),
        ////            Text = CList.ReportName.ToString()
        ////        });
        ////    return Items;

        ////}


    


        ////public static IEnumerable<SelectListItem> ClientAccountContractList
        ////{
        ////    get
        ////    {
        ////        //DbContextFactory Context = new DbContextFactory();
        ////        //Repo<NTTData.AMOPortal.Domain.Entities.ClientAccountContract> clientAccountContracts = new Data.Repo<Domain.Entities.ClientAccountContract>(Context);
        ////        //List<NTTData.AMOPortal.Domain.Entities.ClientAccountContract> clientAccountContractList = clientAccountContracts.GetAll().ToList();

        ////        var Items = clientAccountContractList.Select(CList => new SelectListItem
        ////        {
        ////            Value = CList.Id.ToString(),
        ////            Text = CList.ContractName.ToString()
        ////        });
        ////        return Items;

        ////    }
        ////}



        public static IEnumerable<SelectListItem> AccountStatusList
        {
            get
            {
                ////DbContextFactory Context = new DbContextFactory();
                ////Repo<NTTData.AMOPortal.Domain.Entities.AccountStatus> clientStatus = new Data.Repo<Domain.Entities.AccountStatus>(Context);
                ////List<NTTData.AMOPortal.Domain.Entities.AccountStatus> clientStatusList = clientStatus.GetAll().ToList();
                Db db = new Db();
                var accntStatus = (from acc in db.AccountStatus select new { acc.Id, acc.Status }).ToList();
                var Items = accntStatus.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.Status.ToString()
                });
                return Items;

            }
        }
        public static IEnumerable<SelectListItem> HoursOfServiceTypes
        {
            get
            {
                List<string> stdTypes = new List<string> { "SEV1", "SEV2", "SEV3", "SEV4", "SEV5" };
                var Items = stdTypes.Select(item => new SelectListItem
                {
                    Value = item.ToString(),
                    Text = item.ToString()
                });
                return Items;
            }
        }

        ////public static IEnumerable<SelectListItem> AppName
        ////{
        ////    get
        ////    {
        ////        //DbContextFactory Context = new DbContextFactory();
        ////        List<Application> stdTemplatesList;
        ////        using (var contextFactory = new DbContextFactory())
        ////        {
        ////            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        ////            stdTemplatesList = unitOfWork.Application.Get().ToList();
        ////        }

        ////        var Items = stdTemplatesList.Select(CList => new SelectListItem
        ////        {
        ////            Value = CList.Id.ToString(),
        ////            Text = CList.ApplicationName.ToString()
        ////        });
        ////        return Items;
        ////    }
        ////}




    }
}