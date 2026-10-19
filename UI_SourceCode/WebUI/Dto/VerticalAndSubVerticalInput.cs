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
    public class VerticalsInput : Input
    {
        [Display(Name = "Vertical Name")]
        public string VerticalName { get; set; }
        //public int? ClientId { get; set; }
        public string Description { get; set; }
    }

    public class SubVerticalsInput : Input
    {
        [Display(Name = "SubVertical Name")]
        public string SubVerticalName { get; set; }

        public int VerticalId { get; set; }
        [ForeignKey("VerticalId")]
        public Verticals Vertical { get; set; }
        public string Description { get; set; }
    }
    public class VerticalAndSubVerticalInput : Input
    {
        public int VerticalId { get; set; }
       // public int ClientId { get; set; }
        public int SubVerticalId { get; set; }
        public string SubVerticalName { get; set; }
        public string VerticalName { get; set; }
        public string VerticalCreate { get; set; }
        public string SubVerticalCreate { get; set; }
        public int subVId { get; set; }
        public string title { get; set; }
        public string tabs { get; set; }

        public VerticalsInput verticalInput
        {
            get;
            set;
        }


        public SubVerticalsInput subVerticalInput
        {
            get;
            set;
        }

        public BusinessCapabilitiesLevel1Input BClevel1Input
        {
            get;
            set;
        }

        ////public BusinessCapabilitiesLevel2Input BClevel2Input
        ////{
        ////    get;
        ////    set;
        ////}

        ////public BusinessCapabilitiesLevel3Input BClevel3Input
        ////{
        ////    get;
        ////    set;
        ////}


        public IQueryable<Domain.Entities.Verticals> verticalList
        {
            get
            {
                return IenumberableLists.VerticalsDbSet; // (this.ClientId);
            }


        }


        public IQueryable<Domain.Entities.SubVerticals> subVerticalList
        {
            get
            {
                return IenumberableLists.SubVerticalsDbSet;
            }


        }

        public IQueryable<Domain.Entities.SubVerticals> SubVerticalsFilterList
        {

            get
            {

                Db db = new Db();

                IQueryable<Domain.Entities.SubVerticals> subVerticals = from r in db.IndustrySubVerticals
                                                                        where (r.VerticalId == this.VerticalId)
                                                                        select r;
                return subVerticals;
            }
        }

        ////public IQueryable<Domain.Entities.BusinessCapabilitiesLevel1> BClevel1InternalList
        ////{
        ////    get
        ////    {
        ////        return IenumberableLists.BusinessCapabilitiesLevel1DbSet.Where(x => x.InternalExternal == "Internal");
        ////    }


        ////}
        ////public IQueryable<Domain.Entities.BusinessCapabilitiesLevel1> BClevel1ExternalList
        ////{
        ////    get
        ////    {
        ////        return IenumberableLists.BusinessCapabilitiesLevel1DbSet.Where(x => x.InternalExternal == "External");
        ////    }


        ////}

        ////public IQueryable<Domain.Entities.BusinessCapabilitiesLevel2> BCLevel2List
        ////{
        ////    get
        ////    {
        ////        return IenumberableLists.BusinessCapabilitiesLevel2DbSet;
        ////    }


        ////}
        ////public IQueryable<Domain.Entities.BusinessCapabilitiesLevel3> BCLevel3List
        ////{
        ////    get
        ////    {
        ////        return IenumberableLists.BusinessCapabilitiesLevel3DbSet;
        ////    }


        ////}

    }
}