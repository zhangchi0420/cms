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
    public partial class ReportSortFieldSet : BasePage
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

            if (model.IsGroup == true)
            {
                //分组的话分组字段和汇总字段都可以用来排序
                var _GroupFields = this.DataHelper.Where<SysReportGroupField>(p => p.ReportId == this.__Id);
                var _SumFields = this.DataHelper.Where<SysReportSumFiled>(p => p.ReportId == this.__Id);
                _GroupFields.ForEach(p => {
                    this.ccSortField.Items.Add(new ComboItem()
                    {
                        Text = p.QueryFieldDisplayText,
                        Value = p.QueryFieldAliases,
                    });
                });

                _SumFields.ForEach(p =>
                {
                    this.ccSortField.Items.Add(new ComboItem()
                    {
                        Text = p.DisplayText,
                        Value = p.Aliases,
                    });
                });
            }
            else
            { 
                //不分组的话只有查询字段可以用来排序
                var _QueryFields = this.DataHelper.Where<SysReportQueryField>(p => p.ReportId == this.__Id);
                _QueryFields.ForEach(p =>
                {
                    this.ccSortField.Items.Add(new ComboItem()
                    {
                        Text = p.DisplayText,
                        Value = p.Aliases,
                    });
                });
            }
            //删除垃圾字段
            this.DataHelper.Where<SysReportSortField>(p => p.ReportId == this.__Id)
                .ForEach(p =>
                    {
                        if (this.ccSortField.Items.FirstOrDefault(q => q.Value == p.Aliases) == null)
                        {
                            this.DataHelper.Delete(p);
                        }
                    });

            this.ccSortType.DataSource = typeof(ReportSortTypeEnum);
            this.ccSortType.DataBind();

            InitGrid();
        }

        private void InitGrid()
        {
            var result = this.DataHelper.Where<SysReportSortField>(p => p.ReportId == this.__Id)
                .OrderBy(p => p.OrderIndex)
                .Select(p => new
                {
                    p.SortFieldId,
                    p.OrderIndex,
                    p.Aliases,
                    SortType = EnumHelper.GetDescription(typeof(ReportSortTypeEnum), p.SortType.Value),
                    p.DisplayText,
                }).ToList();
            this.gcList.DataSource = result;
            this.gcList.DataBind();
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var model = new SysReportSortField();
                model.SortFieldId = this.DataHelper.GetNextIdentity_Int();
                model.ReportId = this.__Id;
                var _MaxOrderIndex = this.DataHelper.Where<SysReportSortField>(p => p.ReportId == this.__Id).OrderByDescending(p => p.OrderIndex).FirstOrDefault();
                if (_MaxOrderIndex == null)
                {
                    model.OrderIndex = 1;
                }
                else
                {
                    model.OrderIndex = _MaxOrderIndex.OrderIndex + 1;
                }
                model.Aliases = this.ccSortField.SelectedValue;
                model.DisplayText = this.ccSortField.SelectedText;
                model.SortType = this.ccSortType.SelectedValue.ToInt();
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
                var model = this.DataHelper.FindById<SysReportSortField>(id);
                var PreModel = this.DataHelper.Where<SysReportSortField>(p => p.ReportId == this.__Id && p.OrderIndex < model.OrderIndex).OrderByDescending(p => p.OrderIndex).FirstOrDefault();
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
                var model = this.DataHelper.FindById<SysReportSortField>(id);
                var NextModel = this.DataHelper.Where<SysReportSortField>(p => p.ReportId == this.__Id && p.OrderIndex > model.OrderIndex).OrderBy(p => p.OrderIndex).FirstOrDefault();
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
                var model = this.DataHelper.FindById<SysReportSortField>(id);
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
            var model = this.DataHelper.FindById<SysReport>(this.__Id);
            var _url = string.Format("ReportQueryConditionSet.aspx?id={0}", this.__Id);
            if (model.IsGroup == true)
            {
                _url = string.Format("ReportHavingFieldSet.aspx?id={0}", this.__Id);
                if (model.IsChangeColumn == true)
                {
                    _url = string.Format("ReportChangeColumnSet.aspx?id={0}", this.__Id);
                }
            }
            Response.Redirect(_url);
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ReportDisplayFieldSet.aspx?id={0}", this.__Id));
        }
    }
}