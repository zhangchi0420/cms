using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Common.EntityLibrary.Report;
using Drision.Framework.Entity;

namespace Drision.Framework.Web.Report
{
    public partial class ReportBaseSet : BasePage
    {
        private int __Id
        {
            get
            {
                return Convert.ToInt32(ViewState["__Id"]);
            }
            set
            {
                ViewState["__Id"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                try
                {
                    var id = Request.QueryString["id"];
                    if (!string.IsNullOrEmpty(id))
                    {
                        __Id = id.ToInt();
                    }
                    InitData();
                }
                catch (Exception ex)
                {
                    this.AjaxAlert(ex);
                }
            }
        }

        private void InitData()
        {
            this.ccReportModel.DataSource = typeof(ReportModelEnum);
            this.ccReportModel.DataBind();

            this.ccEntityName.DataSource = this.EntityCache.SysEntity;
            this.ccEntityName.DataValueField = "EntityName";
            this.ccEntityName.DataTextField = "DisplayText";
            this.ccEntityName.DataBind();

            if (__Id > 0)
            {
                var model = this.DataHelper.FindById<SysReport>(__Id);
                if (model != null)
                {
                    this.tcReportName.SetValue(model.ReportName);
                    this.ccReportModel.SetValue(model.ReportModel);
                    this.ccEntityName.SetValue(model.EntityName);
                }
                this.ccEntityName.ReadOnly = true;
            }
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                var _ReportModel = this.ccReportModel.SelectedValue.ToInt();
                if (_ReportModel != (int)ReportModelEnum.Page_Statistics)
                {
                    this.AjaxAlertAndEnableButton("暂时只支持页面统计模式！");
                    return;
                }

                SysReport model;
                if (__Id > 0)
                {
                    model = this.DataHelper.FindById<SysReport>(__Id);
                    ////如果编辑重选的实体，要删除所有配置信息哦(不删除了，直接不让修改查询实体)
                    //if (model.EntityName != this.ccEntityName.SelectedValue)
                    //{
                    //    var _reportHelper = new ReportHelper(this.DataHelper, __Id);
                    //    _reportHelper.DeleteAllSet();
                    //}
                }
                else
                {
                    model = new SysReport();
                    model.ReportId = this.DataHelper.GetNextIdentity_Int();
                    model.State = (int)ReportStateEnum.NotPublish;
                }
                model.ReportName = this.tcReportName.Text;
                model.ReportModel = _ReportModel;
                model.EntityName = this.ccEntityName.SelectedValue;
                model.EntityDisPlayText = this.ccEntityName.SelectedText;
                if (__Id > 0)
                {
                    this.DataHelper.Update(model);
                }
                else
                {
                    this.DataHelper.Insert(model);
                }
                Response.Redirect(string.Format("ReportQueryFieldSet.aspx?id={0}", model.ReportId));
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }
    }
}