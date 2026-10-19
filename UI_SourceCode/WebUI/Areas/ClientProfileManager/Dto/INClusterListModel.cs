using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.IncidentNavigator.Dto;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class INClusterListModel
    {
        public int ClientId { get; set; }
        public List<INConfigurationValueModel> s { get; set; }
        public IList<KeywordListInput> listofclusterword { get; set; }
        public IList<INDictionaryList> listofExcludedwords { get; set; }
        public string lastModified { get; set; }
    }
}