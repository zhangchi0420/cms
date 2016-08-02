using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Common.EntityLibrary.Report;

namespace Drision.Framework.Web.Report
{
    public partial class ReportDisplayFieldSet : BasePage
    {
        public int __Id
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
                    __Id = Request.QueryString["id"].ToInt();
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
            var model = this.DataHelper.FindById<SysReport>(this.__Id);
            this.tcTopN.SetValue(model.TopN);
            this.ccIsGraph.SetValue(model.IsGraph);
            if (model.IsGroup == true)
            {
                //只有分组才能配置图形
                this.divIsGraph.Visible = true;
                //分组的话分组字段和汇总字段都可以用来排序
                var _GroupFields = this.DataHelper.Where<SysReportGroupField>(p => p.ReportId == this.__Id);
                var _SumFields = this.DataHelper.Where<SysReportSumFiled>(p => p.ReportId == this.__Id);
                _GroupFields.ForEach(p =>
                {
                    this.ccDisplayField.Items.Add(new ComboItem()
                    {
                        Text = p.QueryFieldDisplayText,
                        Value = p.QueryFieldAliases,
                    });
                });

                _SumFields.ForEach(p =>
                {
                    this.ccDisplayField.Items.Add(new ComboItem()
                    {
                        Text = p.DisplayText,
                        Value = p.Aliases,
                    });
                });
            }
            else
            {
                //不分组一定要把图形配置去掉
                if (model.IsGraph == true)
                {
                    model.IsGraph = false;
                    this.DataHelper.UpdatePartial(model, p => new { p.IsGraph });
                }
                //不分组的话只有查询字段可以用来排序
                var _QueryFields = this.DataHelper.Where<SysReportQueryField>(p => p.ReportId == this.__Id);
                _QueryFields.ForEach(p =>
                {
                    this.ccDisplayField.Items.Add(new ComboItem()
                    {
                        Text = p.DisplayText,
                        Value = p.Aliases,
                    });
                });
            }

            //删除垃圾字段
            this.DataHelper.Where<SysReportDisplayField>(p => p.ReportId == this.__Id)
                .ForEach(p =>
                {
                    if (this.ccDisplayField.Items.FirstOrDefault(q => q.Value == p.Aliases) == null)
                    {
                        this.DataHelper.Delete(p);
                    }
                });

            //如果是第一次配置到这里，默认把能显示的字段全部默认显示
            if (this.DataHelper.Where<SysReportDisplayField>(p => p.ReportId == this.__Id).Count == 0)
            {
                var _Index = 1;
                foreach (var item in this.ccDisplayField.Items)
                {
                    var _ReportDisplayField = new SysReportDisplayField();
                    _ReportDisplayField.DisplayFieldId = this.DataHelper.GetNextIdentity_Int();
                    _ReportDisplayField.ReportId = this.__Id;
                    _ReportDisplayField.OrderIndex = _Index;
                    _Index++;

                    _ReportDisplayField.Aliases = item.Value;
                    _ReportDisplayField.DisplayText = item.Text;
                    this.DataHelper.Insert(_ReportDisplayField);
                }
            }
            InitGrid();
        }

        private void InitGrid()
        {
            var result = this.DataHelper.Where<SysReportDisplayField>(p => p.ReportId == this.__Id).OrderBy(p => p.OrderIndex).ToList();
            this.gcList.DataSource = result;
            this.gcList.DataBind();
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var model = new SysReportDisplayField();
                model.DisplayFieldId = this.DataHelper.GetNextIdentity_Int();
                model.ReportId = this.__Id;
                var _MaxOrderIndex = this.DataHelper.Where<SysReportDisplayField>(p => p.ReportId == this.__Id).OrderByDescending(p => p.OrderIndex).FirstOrDefault();
                if (_MaxOrderIndex == null)
                {
                    model.OrderIndex = 1;
                }
                else
                {
                    model.OrderIndex = _MaxOrderIndex.OrderIndex + 1;
                }
                model.Aliases = this.ccDisplayField.SelectedValue;
                model.DisplayText = this.ccDisplayField.SelectedText;
                this.DataHelper.Insert(model);
                InitGrid();
                this.AjaxAlertAndEnableButton(string.Empty);
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }
        protected void btnMoveUp_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (sender as LinkButton).CommandArgument.ToInt();
                var model = this.DataHelper.FindById<SysReportDisplayField>(id);
                var PreModel = this.DataHelper.Where<SysReportDisplayField>(p => p.ReportId == this.__Id && p.OrderIndex < model.OrderIndex).OrderByDescending(p => p.OrderIndex).FirstOrDefault();
                if (PreModel != null)
                {
                    var PreOrderIndex = PreModel.OrderIndex;
                    PreModel.OrderIndex = model.OrderIndex;
                    model.OrderIndex = PreOrderIndex;
                    this.DataHelper.UpdatePartial(model, p => new { p.OrderIndex });
                    this.DataHelper.UpdatePartial(PreModel, p => new { p.OrderIndex });
                    InitGrid();
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected void btnMoveDown_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (sender as LinkButton).CommandArgument.ToInt();
                var model = this.DataHelper.FindById<SysReportDisplayField>(id);
                var NextModel = this.DataHelper.Where<SysReportDisplayField>(p => p.ReportId == this.__Id && p.OrderIndex > model.OrderIndex).OrderBy(p => p.OrderIndex).FirstOrDefault();
                if (NextModel != null)
                {
                    var NextOrderIndex = NextModel.OrderIndex;
                    NextModel.OrderIndex = model.OrderIndex;
                    model.OrderIndex = NextOrderIndex;
                    this.DataHelper.UpdatePartial(model, p => new { p.OrderIndex });
                    this.DataHelper.UpdatePartial(NextModel, p => new { p.OrderIndex });
                    InitGrid();
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (sender as LinkButton).CommandArgument.ToInt();
                var model = this.DataHelper.FindById<SysReportDisplayField>(id);
                this.DataHelper.Delete(model);
                InitGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected void btnPre_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ReportSortFieldSet.aspx?id={0}", this.__Id));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                var model = this.DataHelper.FindById<SysReport>(this.__Id);
                model.TopN = this.tcTopN.Text.ToIntNullable();
                model.IsGraph = this.ccIsGraph.Checked;
                this.DataHelper.UpdatePartial(model, p => new { p.TopN, p.IsGraph });
                var _url = string.Format("ReportMenuRoleSet.aspx?id={0}", this.__Id);
                if (this.ccIsGraph.Checked)
                {
                    _url = string.Format("ReportGraphSet.aspx?id={0}", this.__Id);
                }
                Response.Redirect(_url);
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
    }
}