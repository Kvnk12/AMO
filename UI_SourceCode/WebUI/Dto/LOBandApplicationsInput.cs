using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

/////////////////////////////////////////////////////////////////////////////////////////////////////
// Modified By: Neeraj
// Modified Date: 06-03-2014
// Comment: Added ApplicationInvInput class for Application Inventory
/////////////////////////////////////////////////////////////////////////////////////////////////////

namespace NTTData.AMOPortal.WebUI.Dto
{
    public class LineOfBusinessInput : Input
    {
        public string LOBName { get; set; }
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Clients { get; set; }
    }

    public class ApplicationLOBJunctionInput : Input
    {
        public int AppId { get; set; }
        [ForeignKey("AppId")]
        public virtual Application Applications { get; set; }
        public int LOBId { get; set; }
        [ForeignKey("lOBId")]
        public virtual LineOfBusinesses lineOfBusiness { get; set; }
    }

    public class ApplicationInvInput : Input
    {
        public bool AppIsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string ApplicationName { get; set; }
        public int ClientId { get; set; }
        public bool HasSetup { get; set; }        
    }

    public class ClientBCLevel1Input : Input
    {
        public string ClientLevel1Name { get; set; }
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        public string ClientLevel1Des { get; set; }
        public string ClientLevel1Type { get; set; }
        public string ClientLevel1Importance { get; set; }
        public string ClientInternalExternal { get; set; }
    }

    public class ClientBCLevel2Input : Input
    {
        public string ClientLevel2Name { get; set; }
        public int ClientLevel1Id { get; set; }
        [ForeignKey("ClientLevel1Id")]
        public ClientBCLevel1 CBCLevel1 { get; set; }
        public string ClientLevel2Des { get; set; }
        public string ClientLevel2Importance { get; set; }

    }

    public class ClientBCLevel3Input : Input
    {
        public string ClientLevel3Name { get; set; }
        public int ClientLevel2Id { get; set; }
        [ForeignKey("ClientLevel2Id")]
        public ClientBCLevel2 CBCLevel2 { get; set; }
        public string ClientLevel3Des { get; set; }
        public string ClientLevel3Importance { get; set; }
    }
    public class LOBandApplicationsInput :Input
    {
        public int ClientId { get; set; }
        public int LOBId { get; set; }
        public int AppId { get; set; }
        public int AppLOBId { get; set; }
        public string LOBName { get; set; }
        public string AppName { get; set; }
        public string LOBCreate { get; set; }
        public string AppCreate { get; set; }
        public string verticalName { get; set; }
        public int SubVerticalId { get; set; }
        public string subVerticalName { get; set; }
        public string tabs { get; set; }

        public LineOfBusinessInput lineOfBusinessInput
        {
            get;
            set;
        }

        public ApplicationLOBJunctionInput applicationLOBJunctionInput
        {
            get;
            set;
        }

        public ClientBCLevel1Input clientBCLevel1Input
        {
            get;
            set;
        }

        public ClientBCLevel2Input clientBCLevel2Input
        {
            get;
            set;
        }

        public ClientBCLevel3Input clientBCLevel3Input
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> ClientList
        {
            get
            {
                return GetDropDownListItems.ClientList;
            }
        }

        public List<NTTData.AMOPortal.Domain.Entities.Application> ApplicationList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Application> Applications = new Data.Repo<Domain.Entities.Application>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Application> ApplicationsList = new List<Application>();

                ApplicationsList = Applications.GetAll().Where(a => a.ClientId == this.ClientId && a.IsDeleted != true).ToList();
                return ApplicationsList;
            }
        }

        public IEnumerable<SelectListItem> AppList
        {

            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Application> Applications = new Data.Repo<Domain.Entities.Application>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Application> ApplicationsList = Applications.GetAll().Where(x => x.ClientId == this.ClientId).ToList();

                var Items = ApplicationsList.Select(AList => new SelectListItem
                {
                    Value = AList.Id.ToString(),
                    Text = AList.ApplicationName.ToString()
                });
                return Items;

            }
        }

        public Client ClientListbyId
        {
            get
            {

                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Client> Clients = new Data.Repo<Domain.Entities.Client>(Context);
                var client = Clients.Get(this.ClientId);

              


                return client;
               
            }


        }


        public IQueryable<Domain.Entities.LineOfBusinesses> LineOfBusinessList
        {
            get
            {
                return IenumberableLists.LineOfBusinessDbSet.Where(x=>x.ClientId ==this.ClientId);
            }


        }


        public IQueryable<Domain.Entities.ApplicationLOBJunctions> ApplicationLOBJunctionList
        {

            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ApplicationLOBJunctions> ApplicationLOBJns = new Data.Repo<Domain.Entities.ApplicationLOBJunctions>(Context);
                Repo<NTTData.AMOPortal.Domain.Entities.Application> application = new Data.Repo<Domain.Entities.Application>(Context);
                // List<NTTData.AMOPortal.Domain.Entities.ApplicationLOBJunctions> applicationLOBJunctionList = new List<ApplicationLOBJunctions>();
                //  Db db = new Db();

                var applicationLOBJunctionList = ApplicationLOBJns.Where(r => r.LOBId == this.LOBId);
                ////foreach (var item in applicationLOBJunctionList)
                ////{
                ////    item.Applications = application.Get(item.AppId);
                ////}

                return applicationLOBJunctionList;
            }
        }
        public IQueryable<Domain.Entities.ClientBCLevel1> ClientBClevel1InternalList
        {
            get
            {
                return IenumberableLists.ClientBCLevel1DbSet.Where(x => x.ClientInternalExternal == "Internal" && x.ClientId == this.ClientId);
            }


        }
        public IQueryable<Domain.Entities.ClientBCLevel1> ClientBClevel1ExternalList
        {
            get
            {
                return IenumberableLists.ClientBCLevel1DbSet.Where(x => x.ClientInternalExternal == "External" && x.ClientId == this.ClientId);
            }


        }

        public IQueryable<Domain.Entities.ClientBCLevel2> ClientBCLevel2List
        {

            get
            {
                return IenumberableLists.ClientBCLevel2DbSet;
            }
        }

        public IQueryable<Domain.Entities.ClientBCLevel3> ClientBCLevel3List
        {

            get
            {
                return IenumberableLists.ClientBCLevel3DbSet;
            }
        }
    
    }
}