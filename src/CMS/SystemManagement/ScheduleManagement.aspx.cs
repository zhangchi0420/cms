using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Repository.EF;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Manager;
using Drision.Framework.Entity;
using Drision.Framework.Enum;
using Drision.Framework.OrgLibrary;
using Drision.Framework.OrgLibrary.InternalEntities;

namespace Drision.Framework.Web.SystemManagement
{
    public enum WorkItemCreatePageType
    {
        DirectUrl = 2,
        MetaPage = 1,
        Plugin = 3,
    }

    public partial class ScheduleManagement : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            BindDDL();
            BindSchedule();
        }

        private void BindDDL()
        {
            try
            {
                using (BizDataContext context = new BizDataContext())
                {
                    var source = context.Where<Drision.Framework.OrgLibrary.InternalEntities.T_WorkItemType>(p => p.AllowManualAdd == true).ToList();
                    this.ccWorkItemType.DataSource = source;
                    this.ccWorkItemType.DataTextField = "WorkItemType_Name";
                    this.ccWorkItemType.DataValueField = "WorkItemType_Id";
                    this.ccWorkItemType.DataBind();
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var id = this.ccWorkItemType.SelectedValue.ToIntNullable();
                if (id != null)
                {
                    using (BizDataContext context = new BizDataContext())
                    {
                        var wi = context.FindById<Drision.Framework.OrgLibrary.InternalEntities.T_WorkItemType>(id);
                        if (wi != null)
                        {
                            if (wi.CreatePageType == (int)WorkItemCreatePageType.DirectUrl)
                            {
                                if (!string.IsNullOrEmpty(wi.CreatePageUserDefinePage))
                                {
                                    Response.Redirect(wi.CreatePageUserDefinePage);
                                }
                            }
                            else if (wi.CreatePageType == (int)WorkItemCreatePageType.MetaPage)
                            {
                                if (!string.IsNullOrEmpty(wi.EntityName) && !string.IsNullOrEmpty(wi.CreatePageName))
                                {
                                    var entity = this.EntityCache.SysEntity.FirstOrDefault<SysEntity>(p => p.EntityName == wi.EntityName);
                                    var page = this.EntityCache.SysPage.FirstOrDefault<SysPage>(p => p.PageName == wi.CreatePageName);
                                    if (entity != null && page != null)
                                    {
                                        string url = (this.Master as Site).GetPageUrl(entity, page);
                                        url = string.Format("{0}?typeid={1}", url, id);
                                        
                                        Response.Redirect(url);
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("暂时不支持插件方式的新增页面路径");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        private void BindSchedule()
        {
            try
            {
                using (BizDataContext context = new BizDataContext())
                {
                    var source = context.Where<Drision.Framework.OrgLibrary.InternalEntities.T_WorkItemBase>(p => p.OwnerId == LoginUserID && p.State == (int)WorkItemStatus.Created).ToList()
                        .Select(p => new ScheduleEvent()
                        {
                            StartTime = p.StartTime,
                            EndTime = p.EndTime,
                            EventId = p.WorkItemBase_Id,
                            Title = p.Title,
                            Url = p.CompletePageUrl,
                            IsAllDay = p.IsAllDay ?? false,
                            EventType = p.WorkItemTypeID == null ? ScheduleEventTypes.Blue : ScheduleEventTypes.Green,
                        });

                    foreach (var p in source)
                    {
                        this.sc.ScheduleEvents.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

    }
}