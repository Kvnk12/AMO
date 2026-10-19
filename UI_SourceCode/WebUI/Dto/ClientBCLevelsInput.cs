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


namespace NTTData.AMOPortal.WebUI.Dto
{
    //public class ClientBCLevel1Input : Input
    //{
    //    public string ClientLevel1Name { get; set; }
    //    public int ClientId { get; set; }
    //    [ForeignKey("ClientId")]
    //    public Client Client { get; set; }
    //    public string ClientLevel1Des { get; set; }
    //    public string ClientLevel1Type { get; set; }
    //    public string ClientLevel1Importance { get; set; }
    //    public string ClientInternalExternal { get; set; }
    //}

    //public class ClientBCLevel2Input : Input
    //{
    //    public string ClientLevel2Name { get; set; }
    //    public int ClientLevel1Id { get; set; }
    //    [ForeignKey("ClientLevel1Id")]
    //    public ClientBCLevel1 CBCLevel1 { get; set; }
    //    public string ClientLevel2Des { get; set; }
    //    public string ClientLevel2Importance { get; set; }

    //}

    //public class ClientBCLevel3Input : Input
    //{
    //    public string ClientLevel3Name { get; set; }
    //    public int ClientLevel2Id { get; set; }
    //    [ForeignKey("ClientLevel2Id")]
    //    public ClientBCLevel2 CBCLevel2 { get; set; }
    //    public string ClientLevel3Des { get; set; }
    //    public string ClientLevel3Importance { get; set; }
    //}

    public class JunctionLOBClientBCInput : Input
    {
        public int LOBId { get; set; }
        [ForeignKey("LOBId")]
        public virtual LineOfBusinesses lineOfBusiness { get; set; }
        public int? CBCLevel1Id { get; set; }
        [ForeignKey("CBCLevel1Id")]
        public virtual ClientBCLevel1 clientBCLevel1 { get; set; }
        public int? CBCLevel2Id { get; set; }
        [ForeignKey("CBCLevel2Id")]
        public virtual ClientBCLevel2 clientBCLevel2 { get; set; }
        public int? CBCLevel3Id { get; set; }
        [ForeignKey("CBCLevel3Id")]
        public virtual ClientBCLevel3 clientBCLevel3 { get; set; }
        public bool CBCLevel1_AppSupport { get; set; }
        public bool CBCLevel1_ManualSupport { get; set; }
        public bool CBCLevel1_ExternalSupport { get; set; }
        public string CBCLevel1_SLEDesc { get; set; }
        public string CBCLevel1_AppSLEDesc { get; set; }
        public string CBCLevel1_ManualSLEDesc { get; set; }
        public string CBCLevel1_ExternalSLEDesc { get; set; }
        public bool CBCLevel2_AppSupport { get; set; }
        public bool CBCLevel2_ManualSupport { get; set; }
        public bool CBCLevel2_ExternalSupport { get; set; }
        public string CBCLevel2_AppSLEDesc { get; set; }
        public string CBCLevel2_ManualSLEDesc { get; set; }
        public string CBCLevel2_ExternalSLEDesc { get; set; }
        public bool CBCLevel3_AppSupport { get; set; }
        public bool CBCLevel3_ManualSupport { get; set; }
        public bool CBCLevel3_ExternalSupport { get; set; }
        public string CBCLevel3_AppSLEDesc { get; set; }
        public string CBCLevel3_ManualSLEDesc { get; set; }
        public string CBCLevel3_ExternalSLEDesc { get; set; }

    }

    public class JunctionAppLOBClientBCInput : Input
    {
        public int AppId { get; set; }
        [ForeignKey("AppId")]
        public virtual Application application { get; set; }
        public int LOBClientBCId { get; set; }
        [ForeignKey("LOBClientBCId")]
        public virtual JunctionLOBClientBCs junctionLOBClientBCs { get; set; }
    }
    public class ClientBCLevelsInput : Input
    {
        public int ClientId { get; set; }
        public int CLevel1Id { get; set; }
        public int CLevel2Id { get; set; }
        public int CLevel3Id { get; set; }
        public string CLevel1Name { get; set; }
        public string CLevel2Name { get; set; }
        public string CLevel3Name { get; set; }
        public string CLevel1Create { get; set; }
        public string CLevel2Create { get; set; }
        public string CLevel3Create { get; set; }
        public string InExValue { get; set; }
        public string InTabs { get; set; }
        public string verticalName { get; set; }
        public string subVerticalName { get; set; }
        public int LOBClientBCId { get; set; }
        public string LOBName { get; set; }
        public string LobCbcCreate { get; set; }
        public int AppLOBClientBCId { get; set; }
        public string AppLobCbcCreate { get; set; }

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

        public JunctionLOBClientBCInput junctionLOBClientBCInput
        {
            get;
            set;
        }

        public JunctionAppLOBClientBCInput junctionAppLOBClientBCInput
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

                Db db = new Db();

                IQueryable<Domain.Entities.ClientBCLevel2> clientBCLevel2 = from r in db.ClientBCLevel2
                                                                            where (r.ClientLevel1Id == this.CLevel1Id)
                                                                            select r;
                return clientBCLevel2;
            }
        }

        public IQueryable<Domain.Entities.ClientBCLevel3> ClientBCLevel3List
        {

            get
            {

                Db db = new Db();

                IQueryable<Domain.Entities.ClientBCLevel3> clientBCLevel3 = from r in db.ClientBCLevel3
                                                                            where (r.ClientLevel2Id == this.CLevel2Id)
                                                                            select r;
                return clientBCLevel3;
            }
        }

        public IQueryable<Domain.Entities.JunctionLOBClientBCs> JunctionLOBClientBCLevel1List
        {
            get
            {
                return IenumberableLists.JunctionLOBClientBCDbSet.Where(x => x.CBCLevel1Id == this.CLevel1Id);
            }


        }

        public IQueryable<Domain.Entities.JunctionLOBClientBCs> JunctionLOBClientBCLevel2List
        {
            get
            {
                return IenumberableLists.JunctionLOBClientBCDbSet.Where(x => x.CBCLevel2Id == this.CLevel2Id);
            }


        }

        public IQueryable<Domain.Entities.JunctionLOBClientBCs> JunctionLOBClientBCLevel3List
        {
            get
            {
                return IenumberableLists.JunctionLOBClientBCDbSet.Where(x => x.CBCLevel3Id == this.CLevel3Id);
            }


        }

        public IQueryable<Domain.Entities.JunctionAppLOBClientBCs> JunctionAppLOBClientBCList
        {
            get
            {
                return IenumberableLists.JunctionAppLOBClientBCDbSet.Where(x => x.LOBClientBCId == this.LOBClientBCId);
            }


        }

        public IQueryable<Domain.Entities.LineOfBusinesses> LineOfBusinessList
        {
            get
            {
                return IenumberableLists.LineOfBusinessDbSet.Where(x => x.ClientId == this.ClientId);
            }


        }

        public IEnumerable<SelectListItem> LOBList
        {

            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.LineOfBusinesses> lineOfBusinesses = new Data.Repo<Domain.Entities.LineOfBusinesses>(Context);
                List<NTTData.AMOPortal.Domain.Entities.LineOfBusinesses> lineOfBusinessesList = lineOfBusinesses.GetAll().Where(x => x.ClientId == this.ClientId).ToList();

                var Items = lineOfBusinessesList.Select(AList => new SelectListItem
                {
                    Value = AList.Id.ToString(),
                    Text = AList.LOBName.ToString()
                });
                return Items;

            }
        }

        ////public IEnumerable<SelectListItem> AppLobList
        ////{

        ////    get
        ////    {
        ////        DbContextFactory Context = new DbContextFactory();
        ////        Repo<NTTData.AMOPortal.Domain.Entities.Application> applications = new Data.Repo<Domain.Entities.Application>(Context);
        ////        List<NTTData.AMOPortal.Domain.Entities.Application> applicationList = applications.GetAll().Where(x => x.ClientId == this.ClientId).ToList();

        ////        var Items = applicationList.Select(AList => new SelectListItem
        ////        {
        ////            Value = AList.Id.ToString(),
        ////            Text = AList.ApplicationName.ToString()
        ////        });
        ////        return Items;

        ////    }
        ////}
    }
}