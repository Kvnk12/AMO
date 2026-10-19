using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.AlignmentManager.Objects;

namespace WebUI.Areas.AlignmentManager.Dto
    {
    public class heatMapSetupInput : Input
        {


        public PPTreeView PPTreeViewRoot
            {
            get
                {
                IList<PPTreeView> AllTreeViewItems = AlignmentHelper.GetTreeViewItems(this.ClientId);
                PPTreeView root = AllTreeViewItems.Where(c => c.ParentId == 0).FirstOrDefault();
                SetChildren(root, AllTreeViewItems);
                return root;
                }

            }


        private void SetChildren(PPTreeView ppItem, IList<PPTreeView> list)
            {
            var children = list.Where(c => c.ParentId == ppItem.Id).ToList();
            if (children.Count > 0)
                {
                foreach (var child in children)
                    {
                    SetChildren(child, list);
                    ppItem.TreeViewItems.Add(child);
                    }
                }

            }


        public int ClientId
            {
            get
                {
                return _clientId;
                }
            set
                {
                _clientId = value;
                }
            }
        private int _clientId;
        private List<heatMapInput> _lstHeatMapInput;

        public heatMapSetupInput(int clientId)
            {
            _clientId = clientId;
            _lstHeatMapInput = GetHeatMapDataForClientId();

            }
        public List<heatMapInput> heatMapInputList
            {

            get
                {
                return _lstHeatMapInput;
                }
            }

        public IEnumerable<SelectListItem> HeatMapReportNames
            {
            get
                {
                return GetDropDownListItems.HeatMapReportNames(this._clientId);
                }
            }


        public IEnumerable<SelectListItem> ClientList
            {
            get
                {
                return GetDropDownListItems.ClientList;
                }
            }

        public List<heatMapInput> GetHeatMapDataForClientId()
            {
            List<heatMapInput> ListHeatMapInput = new List<heatMapInput>();

            foreach (var item in HeatMapReportNames)
                {
                int HeatMapReportId = Convert.ToInt32(item.Value);
                heatMapInput hmapInput = new heatMapInput();
                hmapInput.ClientId = this._clientId;
                hmapInput.HeatMapReportId = HeatMapReportId;
                hmapInput.HeatMapReportName = item.Text;

                hmapInput.heatMapData = new heatMapData(this._clientId, Convert.ToInt32(item.Value));
                //Get Configured Heat Map Variable Name/Variable Type which is pointing to variable type in registeredatasetmap table(i.e, it comes from the Registerdatasetmap Table) 
                //Based on the registerdataset map table variable type, the column will be present in the Heatmap data, We need to look for the column
                //and create heat map items 

                ListHeatMapInput.Add(hmapInput);

                }
            return ListHeatMapInput;
            }
        }

    }