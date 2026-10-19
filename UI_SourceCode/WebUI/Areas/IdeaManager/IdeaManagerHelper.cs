using Data;
using System.Data;
using Domain.Abstract;

using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.IncidentNavigator.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Domain.Service;
using System.IO;
using System.Text;
using WebUI.Areas.IncidentNavigator.Models;

using Domain.Entities.IncidentNavigator;
using PagedList;
using PagedList.Mvc;


namespace WebUI.Areas.IdeaManager
{
    public class IdeaManagerHelper
    {

        public static DataTable IMTrendingReportList(int ClientId)
        {
            
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("IMIdeaTrendingReport", paramName, paramValue);
            }
            return dt;
        }


    }
}