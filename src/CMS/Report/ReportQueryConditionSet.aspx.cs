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
    public partial class ReportQueryConditionSet : BasePage
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
            this.ccIsGroupSum.SetValue(model.IsGroup);
            var _entity = this.EntityCache.SysEntity.FirstOrDefault(p => p.EntityName == model.EntityName);
            this.ccCompareType.Items.Clear();
            EnumHelper.GetEnumItems(typeof(ReportCompareTypeEnum)).ForEach(p => 
            {
                if (p.Value < 10)
                {
                    this.ccCompareType.Items.Add(new ComboItem() { Text = p.Description, Value = p.Value.ToString() });
                }
            });
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

            //将一些垃圾数据删除掉，主要是可能对元数据做了修改导致要查询的列被修改或删除等
            this.DataHelper.Where<SysReportQueryCondition>(p => p.ReportId == this.__Id)
                .ForEach(p =>
                {
                    //查询条件如果找不到元数据则删除掉
                    if (this.EntityCache.Where<SysField>(q => q.FieldId == p.FieldId && q.FieldName == p.FieldName).Count == 0)
                    {
                        this.DataHelper.Delete(p);
                    }
                });

            InitGrid();
        }

        private void InitGrid()
        {
            var result = this.DataHelper.Where<SysReportQueryCondition>(p => p.ReportId == this.__Id)
                .OrderBy(p => p.OrderIndex)
                .Select(p => new
                {
                    p.ConditionId,
                    p.OrderIndex,
                    IsFullRow = p.IsFullRow == true ? "是" : "否",
                    IsSubQuery = p.IsSubQuery == true ? "是" : "否",
                    CompareType = p.CompareType == null ? "" : EnumHelper.GetDescription(typeof(ReportCompareTypeEnum), p.CompareType.Value),
                    p.DisplayText,
                    p.FieldName,
                }).ToList();
            this.gcList.DataSource = result;
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
                this.lbDisplayText.Text = e.Node.Text;
                this.tcDisplayText.SetValue(e.Node.Text);
                this.lbDisplayText.Attributes["_FieldAndRelationId"] = e.Node.Value;
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
                if (string.IsNullOrEmpty(this.lbDisplayText.Text))
                {
                    this.AjaxAlertAndEnableButton("必须先选择一个字段！");
                    return;
                }
                var _FieldAndRelationId = this.lbDisplayText.Attributes["_FieldAndRelationId"].Split('-');

                var _FieldId = _FieldAndRelationId[0].ToLong();
                var _Field = EntityCache.FindById<SysField>(_FieldId);
                if (this.ccIsSubQuery.Checked)
                {
                    if (_Field == null || _Field.DataType != (int)DataTypeEnum.pref || _Field.RefEntity.Fields.FirstOrDefault(p => p.FieldName == "SystemLevelCode") == null)
                    {
                        this.AjaxAlertAndEnableButton("含子查询的字段只能是外键字段并且所属实体必须包含SystemLevelCode字段！");
                        return;
                    }
                }

                var model = new SysReportQueryCondition();
                model.ConditionId = this.DataHelper.GetNextIdentity_Int();
                model.ReportId = this.__Id;
                var _MaxOrderIndex = this.DataHelper.Where<SysReportQueryCondition>(p => p.ReportId == this.__Id).OrderByDescending(p => p.OrderIndex).FirstOrDefault();
                if (_MaxOrderIndex == null)
                {
                    model.OrderIndex = 1;
                }
                else
                {
                    model.OrderIndex = _MaxOrderIndex.OrderIndex + 1;
                }


                model.FieldId = _FieldId;
                model.RelationId = _FieldAndRelationId[1].ToLongNullable();
                model.IsFullRow = this.ccIsFullRow.Checked;
                model.CompareType = this.ccCompareType.SelectedValue.ToIntNullable();
                model.DisplayText = this.tcDisplayText.Text;
                model.DataType = _Field.DataType;
                model.FieldName = _Field.FieldName;
                model.IsSubQuery = this.ccIsSubQuery.Checked;
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
                var model = this.DataHelper.FindById<SysReportQueryCondition>(id);
                var PreModel = this.DataHelper.Where<SysReportQueryCondition>(p => p.ReportId == this.__Id && p.OrderIndex < model.OrderIndex).OrderByDescending(p => p.OrderIndex).FirstOrDefault();
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
                var model = this.DataHelper.FindById<SysReportQueryCondition>(id);
                var NextModel = this.DataHelper.Where<SysReportQueryCondition>(p => p.ReportId == this.__Id && p.OrderIndex > model.OrderIndex).OrderBy(p => p.OrderIndex).FirstOrDefault();
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
                var model = this.DataHelper.FindById<SysReportQueryCondition>(id);
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
            Response.Redirect(string.Format("ReportFixedConditionSet.aspx?id={0}", this.__Id));
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                var model = this.DataHelper.FindById<SysReport>(this.__Id);
                model.IsGroup = this.ccIsGroupSum.Checked;
                if (model.IsGroup != true)
                {
                    model.IsChangeColumn = false;
                    model.IsGraph = false;
                }
                this.DataHelper.UpdatePartial(model, p => new { p.IsGroup, p.IsChangeColumn, p.IsGraph });
                var _url = string.Format("ReportSortFieldSet.aspx?id={0}", this.__Id);
                if (model.IsGroup == true)
                {
                    _url = string.Format("ReportGroupFieldSet.aspx?id={0}", this.__Id);
                }
                Response.Redirect(_url);
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected void ccIsSubQuery_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ccIsSubQuery.Checked)
            {
                this.ccCompareType.SetValue(null);
                this.ccCompareType.Enabled = false;
            }
            else
            {
                this.ccCompareType.Enabled = true;
            }
        }
    }
}