using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;

namespace WebUI.Models
{
    public class AMOModelBase
    {
        ////public List<Application> GetApplicationsList()
        ////{
        ////    var context = new DbContextFactory();
        ////    var applications = new Repo<Application>(context);
        ////    return applications.GetAll().ToList();
        ////} 

        public List<Client> GetClients()
        {
            var context = new DbContextFactory();
            var applications = new Repo<Client>(context);
            return new List<Client>();//applications.GetAll().ToList();
        }

        public int SelectedClientID { get; set; }

        public AMOModelBase()
        {
        }

    }
}