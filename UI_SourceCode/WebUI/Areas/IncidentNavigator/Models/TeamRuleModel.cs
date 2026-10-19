using NTTData.AMOPortal.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class TeamRuleModel
    {
        public int TId { get; set; }
        public int TeamId { get; set; }
        public string INFieldName { get; set; }
       // [Required(ErrorMessage = "Column value is Required")]
        public string FieldValue { get; set; }
       // [Required(ErrorMessage = "Operator is Required")]
        public string Operator { get; set; }
        public string GroupStartOperator { get; set; }
        public string GroupEndOperator { get; set; }
        [Required(ErrorMessage = "Condition is Required")]
        public string Condition { get; set; }
        //public int FieldLength { get; set; }
        public string ConstantValue { get; set; }
        //public List<SelectListItem> _FieldValue
        //{

        //    get
        //    {
        //        var Id = 0;
        //        Db db = new Db();
        //        //var TempIdentifier = (from strapp in db.StoreApplication
        //        //                      join srctmp in db.MappingTemplate on strapp.MappingId equals srctmp.Id
        //        //                      // join disp in db.APRDisposition on dispandln.DispId equals disp.Id
        //        //                      where strapp.ApplicationId == ApplicationId && srctmp.ClientId == ClientId && srctmp.MappingDone == true
        //        //                      select new SelectListItem
        //        //                      {
        //        //                          Value = srctmp.GlobalMappingIdentifier,
        //        //                          Text = srctmp.GlobalMappingIdentifier

        //        //                      }
        //        //               ).ToList();
        //       var list = IncidentNavigatorHelper.getColumnValuesList(Id, INFieldName).ToList();

        //        return list.ToList();

        //    }






    }
}