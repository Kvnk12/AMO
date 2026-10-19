using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;

namespace WebUI.Areas.AlignmentManager.Dto
    {
    public class PPTreeView
        {

        public int ClientId
            {
            get;
            set;
            }
        public int Id
        { get; set; }
        public int ParentId
        { get; set; }
        public string VariableName
        { get; set; }
        public int varTypeId
        { get; set; }

        public int variableId
        { get; set; }
        public int variableLabelId
        { get; set; }
        public int varcatId
        { get; set; }
        public int IsDeleted
        { get; set; }

        public IList<PPTreeView> TreeViewItems
            {
            get;
            set;

            }

        public PPTreeView()
            {
            TreeViewItems = new List<PPTreeView>();
            }

        }
    }