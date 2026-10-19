using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;
using WebUI.Areas.AlignmentManager.Dto;


namespace NTTData.AMOPortal.WebUI.Dto
    {


    public class TreeViewInput : Input
        {

        public TreeViewInput(int clientId)
        {
            this._clientId = clientId;
        }

        private int _clientId;
        public int ClientId
            {
            get { return _clientId; }
            set { _clientId = value; }
            }

        public PPTreeView  PPTreeViewRoot {
        get
            {
              IList<PPTreeView> AllTreeViewItems=  AlignmentHelper.GetTreeViewItems(this._clientId);
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

        }
    }