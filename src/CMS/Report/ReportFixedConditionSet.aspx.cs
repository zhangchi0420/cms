using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Common.EntityLibrary.Report;
using System.Text;

namespace Drision.Framework.Web.Report
{
    public partial class ReportFixedConditionSet : BasePage
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
            var _entity = this.EntityCache.SysEntity.FirstOrDefault(p => p.EntityName == model.EntityName);

            this.ccCompareType.DataSource = typeof(ReportCompareTypeEnum);
            this.ccCompareType.DataBind();
            //绑定父节点
            TNode PNode = new TNode()
            {
                Text = _entity.DisplayText,
                Value = _entity.EntityName,
            };
            TNode SNode;
            foreach (var _filed in _entity.Fields)
            {
                SNode = new TNode()
                {
                    Text = _filed.DisplayText,
                    Value = string.Format("{0}-", _filed.FieldId),
                };
                SNode.Attributes["IsEntity"] = "false";
                PNode.ChildNodes.Add(SNode);
            }
            PNode.Attributes["IsEntity"] = "true";
            this.tcField.Nodes.Add(PNode);
            foreach (var _Relation in _entity.ChildOneMoreRelations)
            {
                PNode = new TNode()
                {
                    Text = _Relation.ChildField.DisplayText,
                    Value = _Relation.ParentEntity.EntityName,
                };
                foreach (var _filed in _Relation.ParentEntity.Fields)
                {
                    SNode = new TNode()
                    {
                        Text = _filed.DisplayText,
                        Value = string.Format("{0}-{1}", _filed.FieldId, _Relation.RelationId),
                    };
                    SNode.Attributes["IsEntity"] = "false";
                    PNode.ChildNodes.Add(SNode);
                }
                PNode.Attributes["IsEntity"] = "true";
                this.tcField.Nodes.Add(PNode);
            }

            //删除掉垃圾数据,只要有一个Condition类型的条件中的Field是垃圾字段，整个条件就不能用了
            var _ReportFixedCondition = this.DataHelper.Where<SysReportFixedCondition>(p => p.ReportId == this.__Id);
            foreach (var p in _ReportFixedCondition.Where(p => p.FilterType == (int)FilterTypeEnum.CONDITION))
            {
                if (this.EntityCache.Where<SysField>(q => q.FieldName == p.FieldName && q.FieldId == p.FieldId).Count == 0)
                {
                    _ReportFixedCondition.ForEach(r => { this.DataHelper.Delete(r); });
                    break;
                }
            }
            InitFixedCondition();
        }

        private void InitFixedCondition()
        {
            this.gcList.DataSource = this.DataHelper.Where<SysReportFixedCondition>(p => p.ReportId == this.__Id && p.ParentId == null);
            this.gcList.DataBind();
        }

        protected void tcField_NodeClick(object sender, TNodeEventArgs e)
        {
            try
            {
                if (e.Node.Attributes["IsEntity"] == "true")
                {
                    return;
                }
                this.lbConditionField.Attributes["_FieldAndRelationId"] = e.Node.Value;
                this.lbConditionField.Text = e.Node.Text;
                //处理枚举类型
                var _Field = this.EntityCache.FindById<SysField>(e.Node.Value.Split('-')[0].ToLong());
                if (_Field != null && _Field.DataType == (int)DataTypeEnum.penum)
                {
                    this.tcConditionValue.DropdownType = ComboControl.DropdownTypes.DropdownList;
                    this.tcConditionValue.DataSource = _Field.RefEnum.EnumItems;
                    this.tcConditionValue.DataTextField = "DisplayText";
                    this.tcConditionValue.DataValueField = "ItemValue";
                    this.tcConditionValue.DataBind();
                }
                else
                {
                    this.tcConditionValue.DropdownType = ComboControl.DropdownTypes.AutoComplete;
                    this.tcConditionValue.Items.Clear();
                }
                this.tcConditionValue.SetValue(null);
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
                if (string.IsNullOrEmpty(this.lbConditionField.Text))
                {
                    this.AjaxAlertAndEnableButton("必须先选择一个字段！");
                    return;
                }
                var model = new SysReportFixedCondition();
                model.ConditionId = this.DataHelper.GetNextIdentity_Int();
                model.ReportId = this.__Id;
                var _FieldAndRelationId = this.lbConditionField.Attributes["_FieldAndRelationId"].Split('-');
                model.FieldId = _FieldAndRelationId[0].ToLong();
                model.RelationId = _FieldAndRelationId[1].ToLongNullable();
                model.CompareType = this.ccCompareType.SelectedValue.ToIntNullable();
                model.FilterType = (int)FilterTypeEnum.CONDITION;
                var _Field = this.EntityCache.FindById<SysField>(this.lbConditionField.Attributes["_FieldAndRelationId"].Split('-')[0].ToLong());
                if (_Field.DataType == (int)DataTypeEnum.penum)
                {
                    model.ConditionValue = this.tcConditionValue.SelectedValue;
                }
                else
                {
                    model.ConditionValue = this.tcConditionValue.SelectedText;
                }
                model.FieldName = _Field.FieldName;
                model.DisPlayText = string.Format("{0}{1}", this.lbConditionField.Text, string.Format(EnumHelper.GetDescription(typeof(ReportCompareTypeEnum), model.CompareType.Value), this.tcConditionValue.SelectedText));

                this.DataHelper.Insert(model);
                InitFixedCondition();
                this.AjaxAlertAndEnableButton("新增成功！");
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }

        protected void btnAnd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gcList.SelectedValues.Count < 2)
                {
                    this.AjaxAlert("必须选择两个或以上的根条件！");
                    return;
                }

                var model = new SysReportFixedCondition();
                model.ConditionId = this.DataHelper.GetNextIdentity_Int();
                model.ReportId = this.__Id;
                model.FilterType = (int)FilterTypeEnum.AND;

                int i = 0;
                StringBuilder sb = new StringBuilder();
                foreach (var _ConditionId in this.gcList.SelectedValues)
                {
                    var _ChildModel = this.DataHelper.FindById<SysReportFixedCondition>(_ConditionId.ToInt());
                    _ChildModel.ParentId = model.ConditionId;
                    this.DataHelper.UpdatePartial(_ChildModel, p => new { p.ParentId });

                    if (i > 0)
                    {
                        sb.Append(" And ");
                    }
                    sb.Append(_ChildModel.DisPlayText);
                    i++;
                }
                model.DisPlayText = string.Format("({0})", sb.ToString());
                this.DataHelper.Insert(model);

                InitFixedCondition();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected void btnOr_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gcList.SelectedValues.Count < 2)
                {
                    this.AjaxAlert("必须选择两个或以上的根条件！");
                    return;
                }

                var model = new SysReportFixedCondition();
                model.ConditionId = this.DataHelper.GetNextIdentity_Int();
                model.ReportId = this.__Id;
                model.FilterType = (int)FilterTypeEnum.OR;
                int i = 0;
                StringBuilder sb = new StringBuilder();


                foreach (var _ConditionId in this.gcList.SelectedValues)
                {
                    var _ChildModel = this.DataHelper.FindById<SysReportFixedCondition>(_ConditionId.ToInt());
                    _ChildModel.ParentId = model.ConditionId;
                    this.DataHelper.UpdatePartial(_ChildModel, p => new { p.ParentId });

                    if (i > 0)
                    {
                        sb.Append(" Or ");
                    }
                    sb.Append(_ChildModel.DisPlayText);
                    i++;
                }
                model.DisPlayText = string.Format("({0})", sb.ToString());
                this.DataHelper.Insert(model);

                InitFixedCondition();
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
                var _ConditionId = (sender as LinkButton).CommandArgument;
                DeleteCondition(_ConditionId.ToInt());
                var model = this.DataHelper.FindById<SysReportFixedCondition>(_ConditionId);
                this.DataHelper.Delete(model);

                InitFixedCondition();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        private void DeleteCondition(int ParentId)
        {
            var _FixedConditions = this.DataHelper.Where<SysReportFixedCondition>(p => p.ParentId == ParentId);
            foreach (var FixedCondition in _FixedConditions)
            {
                DeleteCondition(FixedCondition.ConditionId);
                this.DataHelper.Delete(FixedCondition);
            }
        }

        protected void btnPre_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ReportQueryFieldSet.aspx?id={0}", this.__Id));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (this.DataHelper.Where<SysReportFixedCondition>(p => p.ReportId == this.__Id && p.ParentId == null).Count > 1)
            {
                this.AjaxAlert("多条件请用And或Or合并成一条！");
                return;
            }
            Response.Redirect(string.Format("ReportQueryConditionSet.aspx?id={0}", this.__Id));
        }

        protected void ccCompareType_TextChanged(object sender, EventArgs e)
        {
            var _CompareType = this.ccCompareType.SelectedValue.ToIntNullable();
            if (_CompareType == (int)ReportCompareTypeEnum.CurrentUser
                || _CompareType == (int)ReportCompareTypeEnum.Today
                || _CompareType == (int)ReportCompareTypeEnum.ThisMonth
                || _CompareType == (int)ReportCompareTypeEnum.ThisYear)
            {
                this.tcConditionValue.ReadOnly = true;
                this.tcConditionValue.SelectedText = this.ccCompareType.SelectedText;
            }
            else
            {
                this.tcConditionValue.ReadOnly = false;
            }
            
        }
    }
}