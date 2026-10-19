using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using NTTData.AMOPortal.WebUI.Controllers;
using Telerik.Web.UI;
using System.Collections;


namespace WebUI.Reports
{
    public partial class DashbordOverallSummaryConfigReport : AspReportpagesBaseClass
    {
        private Db _dataContext;

        protected Db DbContext
        {
            get
            {
                if (_dataContext == null)
                {
                    _dataContext = new Db();
                }
                return _dataContext;
            }
        }

        public override void Dispose()
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
            }
            base.Dispose();
        }

        //private int? ClientID
        //{
        //    get
        //    {
        //        return Convert.ToInt32(Request.QueryString["ClientID"].ToString());
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void save()
        {
            //var list = DbContext.tblOverallSummery.ToList();
            //foreach (var v in list)
            //{
            //    DbContext.tblOverallSummery.Remove(v);
            //}

            foreach (GridDataItem item in RadGrid1.Items)
            {
                int? id = null;
                int? parentId = null;
                if (!string.IsNullOrEmpty(item["summaryID"].Text) && item["summaryID"].Text != "&nbsp;")
                {
                    id = Convert.ToInt32(item["summaryID"].Text);
                }
                if (!string.IsNullOrEmpty(item["ParentID"].Text) && item["ParentID"].Text != "&nbsp;")
                {
                    parentId = Convert.ToInt32(item["ParentID"].Text);
                }
                CheckBox IsIncludeInDashboard = (CheckBox)item["IsIncludeInDashboard"].Controls[0];
                // int? parentId = (int?)item["ParentID"].Text;
                if (id.HasValue)
                {

                    using (var db = new Db())
                    {
                        var p = db.tblOverallSummery.Where(x => x.ID == id).First();

                        if (IsIncludeInDashboard.Checked)
                        {
                            p.IsIncludeInDashboard = true;
                        }
                        else
                        {
                            p.IsIncludeInDashboard = false;
                        }
                      //  p.Target = item["ComplianceTarget"].Text;

                        if (item["ComplianceTarget"].Text == "&nbsp;")
                        {
                            p.Target = null;
                            
                        }
                        else
                        {
                            p.Target = item["CustomSLAName"].Text;
                           
                        }

                        if (item["ParentID"].Text == "&nbsp;")
                        {
                            p.IsSLA = false;
                            p.OriginalMetricName = item["MetricName"].Text;
                            if (item["CustomSLAName"].Text == "&nbsp;")
                            {
                                p.CustomMetricName = null;
                                p.CustomSLAName = null;

                            }
                            else
                            {
                                p.CustomMetricName = item["CustomSLAName"].Text;
                                p.CustomSLAName = item["CustomSLAName"].Text;
                            }


                        }
                        else
                        {
                            var mname = DbContext.tblOverallSummery.Where(x => x.SLAID == parentId && x.IsSLA == false).FirstOrDefault();
                            p.IsSLA = true;
                            p.OriginalSLAName = item["MetricName"].Text;
                            p.OriginalMetricName = item["MetricName"].Text;
                            if (mname != null)
                            {
                                if (!string.IsNullOrEmpty(mname.CustomSLAName))
                                {
                                    p.CustomMetricName = mname.CustomSLAName;
                                }
                                else
                                {
                                    p.CustomMetricName = mname.OriginalMetricName;
                                }

                            }

                           // p.CustomSLAName = item["CustomSLAName"].Text;
                            if (item["CustomSLAName"].Text == "&nbsp;")
                            {
                               
                                p.CustomSLAName = null;

                            }
                            else
                            {
                               
                                p.CustomSLAName = item["CustomSLAName"].Text;
                            }
                        }
                        db.SaveChanges();
                        try
                        {
                            //save chanages to Db

                        }
                        catch (System.Exception)
                        {
                        }
                    }
                }
                //do update
                else
                {
                    tblOverallSummery newItem = new tblOverallSummery();
                    if (IsIncludeInDashboard.Checked)
                    {
                        newItem.IsIncludeInDashboard = true;
                    }
                    else
                    {
                        newItem.IsIncludeInDashboard = false;
                    }

                    if (!string.IsNullOrEmpty(item["ViewSpecID"].Text))
                    {
                        newItem.SLAID = Convert.ToInt32(item["ViewSpecID"].Text);
                    }

                  //  newItem.Target = item["ComplianceTarget"].Text;
                    if (item["ComplianceTarget"].Text == "&nbsp;")
                    {
                        newItem.Target = null;

                    }
                    else
                    {
                        newItem.Target = item["CustomSLAName"].Text;

                    }
                    if (item["ParentID"].Text == "&nbsp;")
                    {
                        newItem.IsSLA = false;
                        newItem.OriginalMetricName = item["MetricName"].Text;
                      //  newItem.CustomMetricName = item["CustomSLAName"].Text;
                      //  newItem.CustomSLAName = item["CustomSLAName"].Text;

                        if (item["CustomSLAName"].Text == "&nbsp;")
                        {
                            newItem.CustomMetricName = null;
                            newItem.CustomSLAName = null;

                        }
                        else
                        {
                            newItem.CustomMetricName = item["CustomSLAName"].Text;
                            newItem.CustomSLAName = item["CustomSLAName"].Text;
                        }
                    }
                    else
                    {

                        var mname = DbContext.tblOverallSummery.Where(x => x.SLAID == parentId && x.IsSLA == false).FirstOrDefault();
                        newItem.IsSLA = true;
                        newItem.OriginalSLAName = item["MetricName"].Text;
                        newItem.OriginalMetricName = item["MetricName"].Text;
                        if (mname != null)
                        {
                            if (!string.IsNullOrEmpty(mname.CustomSLAName))
                            {
                                newItem.CustomMetricName = mname.CustomSLAName;
                            }
                            else
                            {
                                newItem.CustomMetricName = mname.OriginalMetricName;
                            }

                        }

                       // newItem.CustomSLAName = item["CustomSLAName"].Text;
                        if (item["CustomSLAName"].Text == "&nbsp;")
                        {

                            newItem.CustomSLAName = null;

                        }
                        else
                        {

                            newItem.CustomSLAName = item["CustomSLAName"].Text;
                        }
                    }
                    DbContext.tblOverallSummery.Add(newItem);

                    try
                    {
                        //save chanages to Db
                        DbContext.SaveChanges();
                        //command.Item.Edit = false;
                    }
                    catch (System.Exception)
                    {
                    }
                }
            }



        }
        protected void RadGrid1_BatchEditCommand(object sender, Telerik.Web.UI.GridBatchEditingEventArgs e)
        {
            save();
            int s = e.Commands.Count();
            foreach (GridBatchEditingCommand command in e.Commands)
            {

                Hashtable newValues = command.NewValues;
                Hashtable oldValues = command.OldValues;
                int? id = (int?)newValues["summaryID"];
                var newFirstName = newValues["IsIncludeInDashboard"];
                int? parentId = (int?)newValues["ParentID"];
                int? ViewSpecID = (int?)newValues["ViewSpecID"];

                if (newValues["ParentID"] == null)
                {
                    using (var db1 = new Db())
                    {
                        var p1 = db1.tblOverallSummery.Where(x => x.SLAID == ViewSpecID && x.IsSLA == false).FirstOrDefault();

                        if (p1 != null)
                        {
                            if (newValues["IsIncludeInDashboard"] != null)
                            {
                                p1.IsIncludeInDashboard = Convert.ToBoolean(newValues["IsIncludeInDashboard"]);
                            }
                            else
                            {
                                p1.IsIncludeInDashboard = false;
                            }
                            p1.Target = (string)newValues["ComplianceTarget"];
                            //if (newValues["ParentID"] == null)
                            //{
                            //    // p.IsSLA = false;
                            p1.OriginalMetricName = (string)newValues["MetricName"];
                            p1.CustomMetricName = (string)newValues["CustomSLAName"];
                            p1.CustomSLAName = (string)newValues["CustomSLAName"];


                            //  }
                            db1.SaveChanges();
                            try
                            {
                                //save chanages to Db

                            }
                            catch (System.Exception)
                            {
                            }
                        }




                    }
                }
                else
                {
                    using (var db = new Db())
                    {
                        var p = db.tblOverallSummery.Where(x => x.SLAID == ViewSpecID && x.IsSLA == true).FirstOrDefault();

                        if (p != null)
                        {
                            if (newValues["IsIncludeInDashboard"] != null)
                            {
                                p.IsIncludeInDashboard = Convert.ToBoolean(newValues["IsIncludeInDashboard"]);
                            }
                            else
                            {
                                p.IsIncludeInDashboard = false;
                            }
                            p.Target = (string)newValues["ComplianceTarget"];

                            var mname = DbContext.tblOverallSummery.Where(x => x.SLAID == parentId && x.IsSLA == false).FirstOrDefault();
                            //  p.IsSLA = true;
                            p.OriginalSLAName = (string)newValues["MetricName"];
                            p.OriginalMetricName = (string)newValues["MetricName"];
                            if (mname != null)
                            {
                                if (!string.IsNullOrEmpty(mname.CustomSLAName))
                                {
                                    p.CustomMetricName = mname.CustomSLAName;
                                }
                                else
                                {
                                    p.CustomMetricName = mname.OriginalMetricName;
                                }

                            }

                            p.CustomSLAName = (string)newValues["CustomSLAName"];
                        }
                        db.SaveChanges();
                        try
                        {
                            //save chanages to Db

                        }
                        catch (System.Exception)
                        {
                        }

                    }
                }

            }
        }

        protected void RadGrid1_ItemUpdated(object sender, Telerik.Web.UI.GridUpdatedEventArgs e)
        {

        }

        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {//Is it a GridDataItem
            if (e.Item is GridDataItem)
            {
                //Get the instance of the right type
                GridDataItem dataBoundItem = e.Item as GridDataItem;

                //Check the formatting condition
                if (dataBoundItem["ParentID"].Text == "&nbsp;")
                {
                    dataBoundItem["MetricName"].ForeColor = System.Drawing.Color.Blue;
                    dataBoundItem["MetricName"].Font.Bold = true;
                    dataBoundItem["IsIncludeInDashboard"].HorizontalAlign = HorizontalAlign.Center;

                    //Customize more...
                }
                else
                {
                    dataBoundItem["IsIncludeInDashboard"].HorizontalAlign = HorizontalAlign.Right;
                }


            }

        }

        public void BindGrid()
        {
           
            using (var db = new Db())
            {
                var q =
                     from c in db.vwMetriclistWithSpecifications
                     join p in db.tblOverallSummery on new { SomeID = c.MetricName, SomeName = c.ID } equals new { SomeID = p.OriginalMetricName, SomeName=p.SLAID } into ps
                     // from ps in p.Where(f => f.otherid == 17)
                     from p in ps.DefaultIfEmpty()
                     //where c.ID == p.SLAID
                     orderby c.PrimaryID
                     select new
                     {
                         c.MetricName,
                         p.CustomSLAName,
                         p.IsIncludeInDashboard,
                         ViewSpecID = c.ID,
                         SLAID = (int?)p.SLAID,
                         summaryID = (int?)p.ID,
                         ParentID = (int?)c.ParentID,
                         p.Target,
                         c.ComplianceTarget

                   };
                //rdTreeviewMetricSpec.DataSource = s.GetMetrics();

                //rdTreeviewMetricSpec.DataBind();

                RadGrid1.DataSource = q.ToList();
                RadGrid1.DataBind();
            }

        }

        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            BindGrid();


        }
    }
}