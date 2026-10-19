using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;


namespace NTTData.AMOPortal.WebUI.Dto
    {

    public static class IenumberableLists
        {

        public static IQueryable<Domain.Entities.AccountStatus> AccountStatusDbSet
            {
            get
                {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.AccountStatus> clientStatus = new Data.Repo<Domain.Entities.AccountStatus>(Context);
                return clientStatus.GetAll();
                }
            }

        public static IQueryable<Domain.Entities.Client> ClientDbSet
            {
            get
                {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Client> clientStatus = new Data.Repo<Domain.Entities.Client>(Context);
                return clientStatus.GetAll();
                
                }
            }

        public static IQueryable<Domain.Entities.ClientAccount> ClientAccountDbSet
            {
            get
                {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ClientAccount> clientAccount = new Data.Repo<Domain.Entities.ClientAccount>(Context);
                return clientAccount.GetAll();
                }
            }


        public static IQueryable<Domain.Entities.ClientAccountContract> ClientAccountContractDbSet
            {
            get
                {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ClientAccountContract> clientStatus = new Data.Repo<Domain.Entities.ClientAccountContract>(Context);
                return clientStatus.GetAll();
                }
            }

        public static IQueryable<Domain.Entities.Verticals> VerticalsDbSet
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Verticals> verticals = new Data.Repo<Domain.Entities.Verticals>(Context);
                return verticals.GetAll();
            }
        }
        ////public static IQueryable<Domain.Entities.Verticals> VerticalsDbSet(int clientid)
        ////{
        ////    //get
        ////    //{
        ////        DbContextFactory Context = new DbContextFactory();
        ////        Repo<NTTData.AMOPortal.Domain.Entities.Verticals> verticals = new Data.Repo<Domain.Entities.Verticals>(Context);
        ////        return verticals.Where(w=> w.ClientId== clientid);
        ////    //}
        ////}


        public static IQueryable<Domain.Entities.SubVerticals> SubVerticalsDbSet
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.SubVerticals> subVerticals = new Data.Repo<Domain.Entities.SubVerticals>(Context);
                return subVerticals.GetAll();
            }
        }

        ////public static IQueryable<Domain.Entities.BusinessCapabilitiesLevel1> BusinessCapabilitiesLevel1DbSet
        ////{
        ////    get
        ////    {
        ////        DbContextFactory Context = new DbContextFactory();
        ////        Repo<NTTData.AMOPortal.Domain.Entities.BusinessCapabilitiesLevel1> businessCapabilitiesLevel1 = new Data.Repo<Domain.Entities.BusinessCapabilitiesLevel1>(Context);
        ////        return businessCapabilitiesLevel1.GetAll();
        ////    }
        ////}

        ////public static IQueryable<Domain.Entities.BusinessCapabilitiesLevel2> BusinessCapabilitiesLevel2DbSet
        ////{
        ////    get
        ////    {
        ////        DbContextFactory Context = new DbContextFactory();
        ////        Repo<NTTData.AMOPortal.Domain.Entities.BusinessCapabilitiesLevel2> businessCapabilitiesLevel2 = new Data.Repo<Domain.Entities.BusinessCapabilitiesLevel2>(Context);
        ////        return businessCapabilitiesLevel2.GetAll();
        ////    }
        ////}

        ////public static IQueryable<Domain.Entities.BusinessCapabilitiesLevel3> BusinessCapabilitiesLevel3DbSet
        ////{
        ////    get
        ////    {
        ////        DbContextFactory Context = new DbContextFactory();
        ////        Repo<NTTData.AMOPortal.Domain.Entities.BusinessCapabilitiesLevel3> businessCapabilitiesLevel3 = new Data.Repo<Domain.Entities.BusinessCapabilitiesLevel3>(Context);
        ////        return businessCapabilitiesLevel3.GetAll();
        ////    }
        ////}

        public static IQueryable<Domain.Entities.LineOfBusinesses> LineOfBusinessDbSet
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.LineOfBusinesses> lineOfBusiness = new Data.Repo<Domain.Entities.LineOfBusinesses>(Context);
                return lineOfBusiness.GetAll();
            }
        }
        public static IQueryable<Domain.Entities.ClientBCLevel1> ClientBCLevel1DbSet
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ClientBCLevel1> clientBCLevel1 = new Data.Repo<Domain.Entities.ClientBCLevel1>(Context);
                return clientBCLevel1.GetAll();
            }
        }

        public static IQueryable<Domain.Entities.ClientBCLevel2> ClientBCLevel2DbSet
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ClientBCLevel2> clientBCLevel2 = new Data.Repo<Domain.Entities.ClientBCLevel2>(Context);
                return clientBCLevel2.GetAll();
            }
        }

        public static IQueryable<Domain.Entities.ClientBCLevel3> ClientBCLevel3DbSet
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ClientBCLevel3> clientBCLevel3 = new Data.Repo<Domain.Entities.ClientBCLevel3>(Context);
                return clientBCLevel3.GetAll();
            }
        }

        public static IQueryable<Domain.Entities.JunctionLOBClientBCs> JunctionLOBClientBCDbSet
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.JunctionLOBClientBCs> junctionLOBClientBC = new Data.Repo<Domain.Entities.JunctionLOBClientBCs>(Context);
                return junctionLOBClientBC.GetAll();
            }
        }

        public static IQueryable<Domain.Entities.JunctionAppLOBClientBCs> JunctionAppLOBClientBCDbSet
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.JunctionAppLOBClientBCs> junctionAppLOBClientBC = new Data.Repo<Domain.Entities.JunctionAppLOBClientBCs>(Context);
                return junctionAppLOBClientBC.GetAll();
            }
        }
        }
}