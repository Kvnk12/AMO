using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using WebUI.Areas.IncidentNavigator.Models;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class INIncidentServicehoursModel
    {

        public int? ClientId { get; set; }
        public int? HasSLAManager { get; set; }
        public int? P1ServiceHours { get; set; }
        public int? P2ServiceHours { get; set; }
        public int? P3ServiceHours { get; set; }
        public int? P4ServiceHours { get; set; }
        public int? P5ServiceHours { get; set; }
        public decimal? P1Blendedrate { get; set; }
        public decimal? P2Blendedrate { get; set; }
        public decimal? P3Blendedrate { get; set; }
        public decimal? P4Blendedrate { get; set; }
        public decimal? P5Blendedrate { get; set; }

        public int? P1WeekendHours { get; set; }
        public int? P2WeekendHours { get; set; }
        public int? P3WeekendHours { get; set; }
        public int? P4WeekendHours { get; set; }
        public int? P5WeekendHours { get; set; }

        public IEnumerable<SelectListItem> Priorities
        {

            get
            {


                var Items = new List<SelectListItem>() {
              new SelectListItem() {
                  Text = "P1",
                  Value = "1"
              },
              new SelectListItem() {
                  Text = "P2",
                  Value = "2"
              },
              new SelectListItem() {
                  Text = "P3",
                  Value = "3"
              },
              new SelectListItem() {
                  Text = "P4",
                  Value = "4"
              },
              new SelectListItem() {
                  Text = "P5",
                  Value = "5"
              }
          };
                return Items;
            }
        }

        public IEnumerable<SelectListItem> Servicehours
        {

            get
            {


                var Items = new List<SelectListItem>() {
              new SelectListItem() {
                  Text = "Calender Hours(24 hrs)",
                  Value = "1"
              },
              new SelectListItem() {
                  Text = "8:00 - 5:00(9 hrs.shift)",
                  Value = "2"
              },
              new SelectListItem() {
                  Text = "8:00 - 5:00(8 hrs.shift)",
                  Value = "3"
              },
              
          };
                return Items;
            }
        }


        public IEnumerable<SelectListItem> WeekendHours
        {

            get
            {


                var Items = new List<SelectListItem>() {
              new SelectListItem() {
                  Text = "Calender Hours(24 hrs)",
                  Value = "1"
              },
              new SelectListItem() {
                  Text = "8:00 - 5:00(9 hrs.shift)",
                  Value = "2"
              },
              new SelectListItem() {
                  Text = "8:00 - 5:00(8 hrs.shift)",
                  Value = "3"
              },
               new SelectListItem() {
                  Text = "Not Applicable",
                  Value = "4"
              }
          };
                return Items;
            }
        }

        public IEnumerable<SelectListItem> HasSLAmanagerList
        {

            get
            {


                var Items = new List<SelectListItem>() {
              new SelectListItem() {
                  Text = "Yes",
                  Value = "1"
              },
              new SelectListItem() {
                  Text = "No",
                  Value = "0"
              }
          };
                return Items;
            }
        }
    }
}