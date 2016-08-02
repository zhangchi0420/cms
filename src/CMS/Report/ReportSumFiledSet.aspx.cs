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
    public partial class ReportSumFiledSet : BasePage
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
            var _QueryFields = this.DataHelper.Where<SysReportQueryField>(p => p.ReportId == this.__Id);
            _QueryFields.ForEach(p =>
            {
                this.ccQueryField.Items.Add(new ComboItem()
                {
                    Text = p.DisplayText,
                    Value = p.Aliases,
                });
            });
            //删除垃圾字段
            this.DataHelper.Where<SysReportSumFiled>(p => p.ReportId == this.__Id)
                .ForEach(p =>
                {
                    if (_QueryFields.FirstOrDefault(q => q.Aliases == p.QueryFieldAliases) == null)
                    {
                        this.DataHelper.Delete(p);
                    }
                });
            InitGrid();
        }

        private void InitGrid()
        {
            var result = this.DataHelper.Where<SysReportSumFiled>(p => p.ReportId == this.__Id)
                .Select(p => new
                {
                    p.SumFiledId,
                    p.DisplayText,
                    p.DefaultDisplayText,
                    SumType = EnumHelper.GetDescription(typeof(SumType), p.SumType.Value),
                }).ToList();
            this.gcList.DataSource = result;
            this.gcList.DataBind();
        }


        protected void btnFunction_Click(object sender, EventArgs e)
        {
            try
            {
                var model = new SysReportSumFiled();
                model.SumFiledId = this.DataHelper.GetNextIdentity_Int();
                model.ReportId = this.__Id;

                var _FunctionType = (sender as LinkButton).CommandArgument.ToInt();
                model.SumType = _FunctionType;
                model.DefaultDisplayText = string.Format("{0}{1}", ccQueryField.SelectedText, EnumHelper.GetDescription(typeof(SumType), _FunctionType));
                model.Aliases = string.Format("{0}{1}", ccQueryField.SelectedValue, this.GetRandomCode());
                //这个别名不仅在汇总字段中绝无仅有，分组字段中也不行，因为分组字段和这个汇总字段都要包含在处理后的数据源中，别名（即列名）相同就报错了
                var _ExistSumField = this.DataHelper.FirstOrDefault<SysReportSumFiled>(p => p.ReportId == this.__Id && p.Aliases == model.Aliases);
                var _ExistGroupField = this.DataHelper.FirstOrDefault<SysReportGroupField>(p => p.ReportId == this.__Id && p.QueryFieldAliases == model.Aliases);
                while (_ExistSumField != null || _ExistGroupField != null)
                {
                    model.Aliases = string.Format("{0}{1}", ccQueryField.SelectedValue, this.GetRandomCode());
                }
                model.QueryFieldAliases = this.ccQueryField.SelectedValue;
                model.DisplayText = this.tcDisplayText.Text;

                this.DataHelper.Insert(model);
                InitGrid();
                this.AjaxAlertAndEnableButton(string.Empty);
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (sender as LinkButton).CommandArgument.ToInt();
                var model = this.DataHelper.FindById<SysReportSumFiled>(id);
                this.DataHelper.Delete(model);
                InitGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        protected void ccQueryField_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ccQueryField.SelectedValue))
            {
                this.tcDisplayText.Text = this.ccQueryField.SelectedText;
            }
        }
        protected void btnPre_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ReportGroupFieldSet.aspx?id={0}", this.__Id));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                //验证下是不是所有字段都在分组或汇总中
                var _ReportQueryFields = this.DataHelper.Where<SysReportQueryField>(p => p.ReportId == this.__Id);
                var _ReportGroupFields = this.DataHelper.Where<SysReportGroupField>(p => p.ReportId == this.__Id);
                var _ReportSumFileds = this.DataHelper.Where<SysReportSumFiled>(p => p.ReportId == this.__Id);
                var _NotFindField = string.Empty;
                foreach (var p in _ReportQueryFields)
                {
                    if (_ReportGroupFields.FirstOrDefault(q => q.QueryFieldAliases == p.Aliases) == null && _ReportSumFileds.FirstOrDefault(q => q.QueryFieldAliases == p.Aliases) == null)
                    {
                        _NotFindField = p.DisplayText;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(_NotFindField))
                {
                    this.AjaxAlert(string.Format("{0} 既不在分组中也不在汇总中，请修改配置！", _NotFindField));
                    return;
                }
                Response.Redirect(string.Format("ReportHavingFieldSet.aspx?id={0}", this.__Id));
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
    }
}