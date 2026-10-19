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

    //2182014 Raj

    public class BusinessCapabilitiesLevel1Input : Input
    {
        public string Level1 { get; set; }
        public int SubVerticalId { get; set; }
        [ForeignKey("SubVerticalId")]
        public SubVerticals SubVertical { get; set; }
        public string Description1 { get; set; }
        public string InternalExternal { get; set; }
    }

    ////public class BusinessCapabilitiesLevel2Input : Input
    ////{
    ////    public string Level2 { get; set; }
    ////    public int Level1Id { get; set; }
    ////    [ForeignKey("Level1Id")]
    ////    public BusinessCapabilitiesLevel1 BCLevel1 { get; set; }
    ////    public string Description2 { get; set; }
    ////}

    ////public class BusinessCapabilitiesLevel3Input : Input
    ////{
    ////    public string Level3 { get; set; }
    ////    public int Level2Id { get; set; }
    ////    [ForeignKey("Level2Id")]
    ////    public BusinessCapabilitiesLevel2 BCLevel2 { get; set; }
    ////    public string Description3 { get; set; }
    ////}
    public class BusinessCapabilitiesLevelsInput : Input
    {
        public int Level1Id { get; set; }
        public int Level2Id { get; set; }
        public int Level3Id { get; set; }
        public string Level1Name { get; set; }
        public string Level2Name { get; set; }
        public string Level3Name { get; set; }
        public string Level1Create { get; set; }
        public string Level2Create { get; set; }
        public string Level3Create { get; set; }
        public string InExValue { get; set; }
        public int subVId { get; set; }
        public string title { get; set; }


        public BusinessCapabilitiesLevel1Input businessCapabilitiesLevel1Input
        {
            get;
            set;
        }
        ////public BusinessCapabilitiesLevel2Input businessCapabilitiesLevel2Input
        ////{
        ////    get;
        ////    set;
        ////}
        ////public BusinessCapabilitiesLevel3Input businessCapabilitiesLevel3Input
        ////{
        ////    get;
        ////    set;
        ////}

        ////public IQueryable<Domain.Entities.BusinessCapabilitiesLevel1> BClevel1InternalList
        ////{
        ////    get
        ////    {
        ////        return IenumberableLists.BusinessCapabilitiesLevel1DbSet.Where(x => x.InternalExternal == "Internal" && x.SubVerticalId == this.subVId);
        ////    }


        ////}
        ////public IQueryable<Domain.Entities.BusinessCapabilitiesLevel1> BClevel1ExternalList
        ////{
        ////    get
        ////    {
        ////        return IenumberableLists.BusinessCapabilitiesLevel1DbSet.Where(x => x.InternalExternal == "External" && x.SubVerticalId == this.subVId);
        ////    }


        ////}

        ////public IQueryable<Domain.Entities.BusinessCapabilitiesLevel2> BusinessCapabilitiesLevel2List
        ////{

        ////    get
        ////    {

        ////        Db db = new Db();

        ////        IQueryable<Domain.Entities.BusinessCapabilitiesLevel2> businessCapabilitiesLevel2 = from r in db.BusinessCapabilitiesLevel2
        ////                                                                                            where (r.Level1Id == this.Level1Id)
        ////                                                                                            select r;
        ////        return businessCapabilitiesLevel2;
        ////    }
        ////}

        ////public IQueryable<Domain.Entities.BusinessCapabilitiesLevel3> BusinessCapabilitiesLevel3List
        ////{

        ////    get
        ////    {

        ////        Db db = new Db();

        ////        IQueryable<Domain.Entities.BusinessCapabilitiesLevel3> businessCapabilitiesLevel3 = from r in db.BusinessCapabilitiesLevel3
        ////                                                                                            where (r.Level2Id == this.Level2Id)
        ////                                                                                            select r;
        ////        return businessCapabilitiesLevel3;
        ////    }
        ////}

        public string titleName
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.SubVerticals> subVerticals = new Data.Repo<Domain.Entities.SubVerticals>(Context);
                Repo<NTTData.AMOPortal.Domain.Entities.Verticals> verticals = new Data.Repo<Domain.Entities.Verticals>(Context);
                var sv = subVerticals.Get(this.subVId);
                var v = verticals.Get(sv.VerticalId);
                return v.VerticalName + " / " + sv.SubVerticalName;
            }
        }

    }
}