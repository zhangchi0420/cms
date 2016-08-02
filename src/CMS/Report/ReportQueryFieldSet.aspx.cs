using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common;
using Drision.Framework.Entity;
using Drision.Framework.Common.EntityLibrary.Report;

namespace Drision.Framework.Web.Report
{
    public partial class ReportQueryFieldSet : BasePage
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
            //绑定父节点
            TNode PNode = new TNode()
            {
                Text = _entity.DisplayText,
                Value = _entity.EntityName,
            };
            TNode SNode;
            foreach (var _filed in _entity.Fields)
            {
                var _DisplayText = _filed.DisplayText;
                if (_filed.DataType == (int)DataTypeEnum.pref)
                {
                    _DisplayText = string.Format("{0}(外键)", _filed.DisplayText);
                }
                SNode = new TNode()
                {
                    Text = _DisplayText,
                    Value = string.Format("{0}-",_filed.FieldId),
                };
                SNode.Attributes["IsEntity"] = "false";
                PNode.ChildNodes.Add(SNode);
            }
            PNode.Attributes["IsEntity"] = "true";
            this.tcField.Nodes.Add(PNode);
            //关联第一层实体
            foreach (var _Relation in _entity.ChildOneMoreRelations)
            {
                PNode = new TNode()
                {
                    Text = string.Format("{0}-{1}", _entity.DisplayText, _Relation.ChildField.DisplayText),
                    Value = _Relation.ParentEntity.EntityName,
                };
                foreach (var _filed in _Relation.ParentEntity.Fields)
                {
                    var _DisplayText = _filed.DisplayText;
                    if (_filed.DataType == (int)DataTypeEnum.pref)
                    {
                        _DisplayText = string.Format("{0}(外键)", _filed.DisplayText);
                    }
                    SNode = new TNode()
                    {
                        Text = _DisplayText,
                        Value = string.Format("{0}-{1}", _filed.FieldId, _Relation.RelationId),
                    };
                    SNode.Attributes["IsEntity"] = "false";
                    PNode.ChildNodes.Add(SNode);
                }
                PNode.Attributes["IsEntity"] = "true";
                this.tcField.Nodes.Add(PNode);
            }
            //关联第二层实体
            foreach (var _Relation in _entity.ChildOneMoreRelations)
            {
                foreach (var _SeRelation in _Relation.ParentEntity.ChildOneMoreRelations)
                {
                    PNode = new TNode()
                    {
                        Text = string.Format("{0}-{1}-{2}", _entity.DisplayText, _Relation.ChildField.DisplayText, _SeRelation.ChildField.DisplayText),
                        Value = _Relation.ParentEntity.EntityName,
                    };
                    foreach (var _Sefiled in _SeRelation.ParentEntity.Fields)
                    {
                        var _DisplayText = _Sefiled.DisplayText;
                        if (_Sefiled.DataType == (int)DataTypeEnum.pref)
                        {
                            _DisplayText = string.Format("{0}(外键)", _Sefiled.DisplayText);
                        }
                        SNode = new TNode()
                        {
                            Text = _DisplayText,
                            Value = string.Format("{0}-{1}-{2}", _Sefiled.FieldId,_Relation.RelationId, _SeRelation.RelationId),
                        };
                        SNode.Attributes["IsEntity"] = "false";
                        PNode.ChildNodes.Add(SNode);
                    }
                    PNode.Attributes["IsEntity"] = "true";
                    this.tcField.Nodes.Add(PNode);

                }
            }
            //将一些垃圾数据删除掉，主要是可能对元数据做了修改导致要查询的列被修改或删除等
            var allQueryFields = this.DataHelper.Where<SysReportQueryField>(p => p.ReportId == this.__Id);
            allQueryFields.Where(p => p.ReportId == this.__Id&&p.IsParent != true).ToList()
                .ForEach(p =>
                {
                    //查询字段如果字段1找不到元数据则删除掉
                    if (this.EntityCache.Where<SysField>(q => q.FieldId == p.Field1Id && q.FieldName == p.Field1Name).Count == 0)
                    {
                        this.DataHelper.Delete(p);
                        DeleteParentQueryField(allQueryFields, p.QueryFieldId);
                    }
                    //查询字段如果字段2找不到元数据则删除掉
                    if (p.Field2Id != null)
                    {
                        if (this.EntityCache.Where<SysField>(q => q.FieldId == p.Field2Id && q.FieldName == p.Field2Name).Count == 0)
                        {
                            this.DataHelper.Delete(p);
                            DeleteParentQueryField(allQueryFields, p.QueryFieldId);
                        }
                    }
                });
            InitGrid();
        }
        /// <summary>
        /// 只要删除掉一个，所有引用这个删除的都要删除，否则查询时就会出错
        /// </summary>
        /// <param name="allQueryFields"></param>
        /// <param name="deleteQueryFieldId"></param>
        private void DeleteParentQueryField(List<SysReportQueryField> allQueryFields, int deleteQueryFieldId)
        {
            var needDeleteQueryFields = allQueryFields.Where(p => p.IsParent == true && (p.Field1Id == deleteQueryFieldId || p.Field2Id == deleteQueryFieldId)).ToList();
            if (needDeleteQueryFields.Count > 0)
            {
                foreach (var p in needDeleteQueryFields)
                {
                    this.DataHelper.Delete(p);
                    DeleteParentQueryField(allQueryFields, p.QueryFieldId);
                }
            }
        }

        private void InitGrid()
        {
            this.gcList.DataSource = this.DataHelper.Where<SysReportQueryField>(p => p.ReportId == this.__Id);
            this.gcList.DataBind();
        }

        protected void btnQueryFieldFuntion_Click(object sender, EventArgs e)
        {
            var _FunctionType = (sender as LinkButton).CommandArgument.ToInt();
            var _SelectValue = this.gcList.SelectedValues;
            var model = new SysReportQueryField();
            if (_SelectValue.Count != 2)
            {
                this.AjaxAlertAndEnableButton("查询字段的二次操作必须先选择两个已有的查询字段！");
                return;
            }
            if (string.IsNullOrEmpty(this.tcDisplayTextSub.Text))
            {
                this.AjaxAlertAndEnableButton("二次查询字段名称不能为空！");
                return;
            }
            model.QueryFieldId = this.DataHelper.GetNextIdentity_Int();
            model.ReportId = this.__Id;
            model.Field1Id = _SelectValue[0].ToLong();
            model.Field2Id = _SelectValue[1].ToLong();
            var _QueryField1 = this.DataHelper.FindById<SysReportQueryField>(model.Field1Id);
            var _QueryField2 = this.DataHelper.FindById<SysReportQueryField>(model.Field2Id);
            model.FunctionType = _FunctionType;

            model.Aliases = string.Format("{0}_{1}", _QueryField1.Aliases, _QueryField2.Aliases);
            //保证本报表中没有别名相同的，至于它具体是什么，不要在意细节啊~
            while (this.DataHelper.FirstOrDefault<SysReportQueryField>(p => p.ReportId == this.__Id && p.Aliases == model.Aliases) != null)
            {
                model.Aliases = string.Format("{0}_{1}_{2}", _QueryField1.Aliases, _QueryField2.Aliases, this.GetRandomCode());
            }

            model.DefaultDisplayText = string.Format("{0}{1}{2}", _QueryField1.DisplayText, EnumHelper.GetDescription(typeof(ReportFunctionTypeEnum), _FunctionType), _QueryField2.DisplayText);
            model.DisplayText = this.tcDisplayTextSub.Text;
            model.IsParent = true;
            this.DataHelper.Insert(model);
            InitGrid();
        }
        protected void btnFuntion_Click(object sender, EventArgs e)
        {
            try
            {
                var _FunctionType = (sender as LinkButton).CommandArgument.ToInt();
                var _CheckedValue = this.tcField.CheckedValue;
                var model = new SysReportQueryField();
                if (_CheckedValue.Count == 0)
                {
                    this.AjaxAlertAndEnableButton("至少选择一个字段！");
                    return;
                }
                var _firstIndex = _CheckedValue[0].IndexOf('-');
                model.Field1Id = _CheckedValue[0].Substring(0, _firstIndex).ToLong();
                model.Relation1Id = _CheckedValue[0].Substring(_firstIndex + 1);
                var _Field1 = this.EntityCache.FindById<SysField>(model.Field1Id);
                var _CheckNode1 = this.tcField.GetNodeByValue(_CheckedValue[0]);
                switch ((ReportFunctionTypeEnum)_FunctionType)
                {
                    case ReportFunctionTypeEnum.JustAdd:
                        if (_CheckedValue.Count != 1)
                        {
                            this.AjaxAlertAndEnableButton("此功能只能且必须选择一个字段！");
                            return;
                        }
                        model.DefaultDisplayText = string.Format("{0}-{1}{2}",_CheckNode1.Parent.Text, _Field1.DisplayText, EnumHelper.GetDescription(typeof(ReportFunctionTypeEnum), _FunctionType));
                        break;
                    case ReportFunctionTypeEnum.GetYear:
                    case ReportFunctionTypeEnum.GetMonth:
                    case ReportFunctionTypeEnum.GetDate:
                        if (_CheckedValue.Count != 1)
                        {
                            this.AjaxAlertAndEnableButton("此功能只能且必须选择一个字段！");
                            return;
                        }
                        if (_Field1.DataType != (int)DataTypeEnum.pdatetime)
                        {
                            this.AjaxAlertAndEnableButton("此功能只支持日期类型字段！");
                            return;
                        }
                        model.DefaultDisplayText = string.Format("{0}-{1}{2}", _CheckNode1.Parent.Text, _Field1.DisplayText, EnumHelper.GetDescription(typeof(ReportFunctionTypeEnum), _FunctionType));
                        break;
                    case ReportFunctionTypeEnum.Add:
                    case ReportFunctionTypeEnum.Minus:
                    case ReportFunctionTypeEnum.Mul:
                    case ReportFunctionTypeEnum.Div:
                    case ReportFunctionTypeEnum.ReDiv:
                    case ReportFunctionTypeEnum.ReMinus:
                        if (_CheckedValue.Count != 2)
                        {
                            this.AjaxAlertAndEnableButton("此功能只能且必须选择两个字段！");
                            return;
                        }
                        _firstIndex = _CheckedValue[1].IndexOf('-');
                        model.Field2Id = _CheckedValue[1].Substring(0, _firstIndex).ToLong();
                        model.Relation2Id = _CheckedValue[1].Substring(_firstIndex + 1);

                        var _Field2 = this.EntityCache.FindById<SysField>(model.Field2Id);
                        var _CheckNode2 = this.tcField.GetNodeByValue(_CheckedValue[1]);
                        if (_Field1.DataType != (int)DataTypeEnum.pint && _Field1.DataType != (int)DataTypeEnum.plong && _Field1.DataType != (int)DataTypeEnum.pdecimal && _Field1.DataType != (int)DataTypeEnum.pfloat)
                        {
                            this.AjaxAlertAndEnableButton("此功能只支持数字类型字段！");
                            return;
                        }
                        if (_Field2.DataType != (int)DataTypeEnum.pint && _Field2.DataType != (int)DataTypeEnum.plong && _Field2.DataType != (int)DataTypeEnum.pdecimal && _Field2.DataType != (int)DataTypeEnum.pfloat)
                        {
                            this.AjaxAlertAndEnableButton("此功能只支持数字类型字段！");
                            return;
                        }

                        model.Field2Name = _Field2.FieldName;
                        model.DefaultDisplayText = string.Format("({0}-{1}){2}({3}-{4})", _CheckNode1.Parent.Text, _Field1.DisplayText, EnumHelper.GetDescription(typeof(ReportFunctionTypeEnum), _FunctionType), _CheckNode2.Parent.Text, _Field2.DisplayText);
                        break;
                }
                model.QueryFieldId = this.DataHelper.GetNextIdentity_Int();
                model.ReportId = this.__Id;
                model.FunctionType = _FunctionType;
                model.Field1Name = _Field1.FieldName;
                model.Aliases = _Field1.FieldName;
                //保证本报表中没有别名相同的，至于它具体是什么，不要在意细节啊~
                while (this.DataHelper.FirstOrDefault<SysReportQueryField>(p => p.ReportId == this.__Id && p.Aliases == model.Aliases) != null)
                {
                    model.Aliases = string.Format("{0}{1}", _Field1.FieldName, this.GetRandomCode());
                }
                model.DisplayText = this.tcDisplayText.Text;
                this.DataHelper.Insert(model);
                InitGrid();
                //清空选择
                tcField.CheckedValue = new List<string>();

                this.AjaxAlertAndEnableButton(string.Empty);
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }


        protected void tcField_NodeClick(object sender, TNodeEventArgs e)
        {
            try
            {
                if (e.Node.Attributes["IsEntity"] == "true")
                {
                    return;
                }
                var model = new SysReportQueryField();

                var _firstIndex = e.Node.Value.IndexOf('-');
                model.Field1Id = e.Node.Value.Substring(0, _firstIndex).ToLong();
                model.Relation1Id = e.Node.Value.Substring(_firstIndex + 1);

                var _Field1 = this.EntityCache.FindById<SysField>(model.Field1Id);

                model.DefaultDisplayText = string.Format("{0}-{1}", e.Node.Parent.Text, _Field1.DisplayText);
                model.QueryFieldId = this.DataHelper.GetNextIdentity_Int();
                model.ReportId = this.__Id;
                model.FunctionType = (int)ReportFunctionTypeEnum.JustAdd;
                model.Field1Name = _Field1.FieldName;
                model.Aliases = _Field1.FieldName;
                //保证本报表中没有别名相同的，至于它具体是什么，不要在意细节啊~
                while (this.DataHelper.FirstOrDefault<SysReportQueryField>(p => p.ReportId == this.__Id && p.Aliases == model.Aliases) != null)
                {
                    model.Aliases = string.Format("{0}{1}", _Field1.FieldName, this.GetRandomCode());
                }
                model.DisplayText = _Field1.DisplayText;
                this.DataHelper.Insert(model);
                InitGrid();
                //清空选择
                tcField.CheckedValue = new List<string>();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        protected void btnPre_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ReportBaseSet.aspx?id={0}", this.__Id));
        }

        protected void lbtnDel_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (sender as LinkButton).CommandArgument.ToInt();
                var model = this.DataHelper.FindById<SysReportQueryField>(id);
                var allQueryFields = this.DataHelper.Where<SysReportQueryField>(p => p.ReportId == this.__Id);
                if (model != null)
                {
                    this.DataHelper.Delete(model);
                    DeleteParentQueryField(allQueryFields, model.QueryFieldId);
                }
                InitGrid();
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ReportFixedConditionSet.aspx?id={0}", this.__Id));
        }
    }
}