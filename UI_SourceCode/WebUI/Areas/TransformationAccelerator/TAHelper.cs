using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data;
using Domain.Entities;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using WebUI.Areas.TransformationAccelerator.Model;
using WebUI.Areas.TransformationAccelerator.Controllers;
using Kendo.Mvc.UI;
using System.Web.Configuration;
using System.Data;
using System.Web.Mvc;
using System.Data.Entity.SqlServer;
using WebUI.Dto;
using WebUI.Areas.IncidentNavigator.Models;
using WebUI.Areas.IncidentNavigator.Dto;
using Kendo.Mvc.Extensions;
using NTTData.AMOPortal.WebUI.Filters;

using System.Collections.Specialized;


namespace WebUI.Areas.TransformationAccelerator
{
    public class TAHelper
    {
        public static IEnumerable<SelectListItem> GetListOfServAgreement(int clientid)
        {
            Db db = new Db();
            //var listOfServAgreement = (from a in db.ServiceImprovementType
            //                               //join b in db.ServiceImprovementGoal on a.Id equals b.ImprovementTypeId
            //                           select new ServiceAgreementModel
            //                           {
            //                               ImprovementTypeId = a.Id,
            //                               ImprovementType = a.ImprovementType
            //                           }).ToList();

            var listOfServAgreement = (from a in db.ServiceImprovementType
                                           //join b in db.ServiceImprovementGoal on a.Id equals b.ImprovementTypeId
                                       select new
                                       {
                                           ImprovementTypeId = a.Id,
                                           ImprovementType = a.ImprovementType

                                       }
                                       ).ToList();

            var listOfServAgreement1 = from a in listOfServAgreement
                                       select new ServiceAgreementModel
                                       {
                                           ImprovementTypeId = a.ImprovementTypeId.ToString(),
                                           ImprovementType = a.ImprovementType
                                       };

            var ServTypenames = listOfServAgreement.Select(g =>
                new SelectListItem
                {

                    Text = g.ImprovementType,
                    //Value = g.ImprovementTypeId.GetValueOrDefault().ToString()
                    Value = g.ImprovementTypeId.ToString()
                }).ToList();

            return ServTypenames;
        }


        public static List<string> GetServImpName(int servImpId)
        {
            Db db = new Db();
            List<string> list1 = (from imc in db.ServiceImprovementType
                                  where imc.Id == servImpId
                                  select imc.ImprovementType).ToList();

            return list1;
        }

        public static IList<ServiceAgreementModel> GetServiceImprovementType(int clientId)
        {

            IList<ServiceAgreementModel> serviceAgreement = new List<ServiceAgreementModel>();
            using (var contextFactory = new DbContextFactory())
            {
                
                IEnumerable<ServiceImprovementGoal> serviceimpDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                Db db = new Db();
                serviceimpDetails = (from a in db.ServiceImprovementGoal
                                     where a.ClientId == clientId
                                     select a).OrderBy(e =>e.ImprovementTypeId);

                if (serviceimpDetails != null)
                {
                    foreach (var _servImpType in serviceimpDetails)
                    {
                        ServiceAgreementModel cans = new ServiceAgreementModel();
                        cans.Id = _servImpType.Id;
                        //cans.ImprovementTypeId = _servImpType.ImprovementTypeId != null ? _servImpType.ImprovementTypeId.Value : 0;
                        cans.ImprovementTypeId = _servImpType.ImprovementTypeId.ToString();
                        cans.Goal = _servImpType.Goal;
                        cans.impNotes = _servImpType.ImprovementNotes;

                        serviceAgreement.Add(cans);
                    }
                }
                return serviceAgreement;
            }

        }

        public static int? CreateServiceImprovementType(ServiceAgreementModel serviceAgreement, int clientID)
        {
            int? id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var cns = new ServiceImprovementGoal();
                    cns.Id = Convert.ToInt32(serviceAgreement.ImprovementTypeId);
                    cns.ImprovementTypeId = Convert.ToInt32(serviceAgreement.ImprovementTypeId);
                    cns.Goal = serviceAgreement.Goal;
                    cns.ImprovementNotes = serviceAgreement.impNotes;
                    cns.ClientId = clientID;

                    unitOfWork.ServiceImprovementGoal.Insert(cns);
                    unitOfWork.Save();
                    id = cns.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateServiceImprovementType(ServiceAgreementModel serviceAgreement)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.ServiceImprovementGoal.GetByID(serviceAgreement.Id);
                    updatetitem.Goal = serviceAgreement.Goal;
                    updatetitem.ImprovementNotes = serviceAgreement.impNotes;


                    unitOfWork.ServiceImprovementGoal.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveServiceImprovementType(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.ServiceImprovementGoal.Delete(Id);
                unitOfWork.Save();
                //Db db = new Db();
                // var v = (from a in db.ServiceImprovementGoal
                //          where a.ImprovementTypeId == Id
                //          select a).FirstOrDefault();

                //db.ServiceImprovementGoal.Remove(v);
                //db.SaveChanges();
                return true;
            }
        }


    }
}