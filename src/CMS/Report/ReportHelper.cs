using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Drision.Framework.Common.EntityLibrary.Report;
using Drision.Framework.Common;
using System.Web.UI.WebControls;
using Drision.Framework.WebControls.Super;
using Drision.Framework.Entity;
using System.Data;
using System.Data.Common;
using System.Web.UI.DataVisualization.Charting;
using System.Text;
using Drision.Framework.DAL;

namespace Drision.Framework.Web.Report
{
    public class ReportHelper
    {
        private static EntityCache _entityCache = EntityCache.New();
        private BizDataContext _DataHelper { get; set; }
        private T_User LoginUser { get; set; }
        private int _ReportId { get; set; }
        private SysReport _Report { get; set; }
        private List<SysReportQueryCondition> _ReportQueryConditions { get; set; }
        private List<SysReportGroupField> _ReportGroupFields { get; set; }
        private List<SysReportSumFiled> _ReportSumFileds { get; set; }
        private List<SysReportQueryField> _ReportQueryFields { get; set; }
        private List<SysReportDisplayField> _ReportDisplayFields { get; set; }
        private List<SysReportHavingField> _ReportHavingFields { get; set; }
        private List<SysReportSortField> _ReportSortFields { get; set; }
        private SysReportGraph _ReportGraph { get; set; }
        private SysReportChangeColumn _ReportChangeColumn { get; set; }
        private List<SysReportFixedCondition> _ReportFixedConditions { get; set; }
        /// <summary>
        /// 存放RelationId 和表别名的键值对，主表Key用0，从a开始
        /// </summary>
        private Dictionary<string, string> dicRelationTableAliases;
        /// <summary>
        /// 初始化查询出所有的配置数据
        /// </summary>
        /// <param name="DataHelper"></param>
        /// <param name="ReportId"></param>
        public ReportHelper(BizDataContext DataHelper, int ReportId, T_User LoginUser)
        {
            this._DataHelper = DataHelper;
            this._ReportId = ReportId;
            this.LoginUser = LoginUser;
            this._Report = this._DataHelper.FindById<SysReport>(ReportId);
            this._ReportQueryConditions = this._DataHelper.Where<SysReportQueryCondition>(p => p.ReportId == ReportId).OrderBy(p => p.OrderIndex).ToList();
            this._ReportGroupFields = this._DataHelper.Where<SysReportGroupField>(p => p.ReportId == ReportId).ToList();
            this._ReportSumFileds = this._DataHelper.Where<SysReportSumFiled>(p => p.ReportId == ReportId).ToList();
            this._ReportQueryFields = this._DataHelper.Where<SysReportQueryField>(p => p.ReportId == ReportId).ToList();
            this._ReportDisplayFields = this._DataHelper.Where<SysReportDisplayField>(p => p.ReportId == ReportId).OrderBy(p => p.OrderIndex).ToList();
            this._ReportHavingFields = this._DataHelper.Where<SysReportHavingField>(p => p.ReportId == ReportId).ToList();
            this._ReportSortFields = this._DataHelper.Where<SysReportSortField>(p => p.ReportId == ReportId).OrderBy(p => p.OrderIndex).ToList();
            this._ReportGraph = this._DataHelper.FirstOrDefault<SysReportGraph>(p => p.ReportId == ReportId);
            this._ReportChangeColumn = this._DataHelper.FirstOrDefault<SysReportChangeColumn>(p => p.ReportId == ReportId);
            this._ReportFixedConditions = this._DataHelper.Where<SysReportFixedCondition>(p => p.ReportId == ReportId);
        }
        /// <summary>
        /// 按数据类型创建查询条件控件并加到页面中
        /// </summary>
        /// <param name="_ReportQueryConditions"></param>
        /// <param name="_ParentControl"></param>
        public void CreateQueryCondition(Control _ParentControl)
        {

            foreach (var _QueryCondition in _ReportQueryConditions)
            {
                //控件外层
                var divItem = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                if (_QueryCondition.IsFullRow == true)
                {
                    divItem.Attributes["class"] = "item_box_col2";
                }
                else
                {
                    divItem.Attributes["class"] = "item_box_col1";
                }
                //控件标题
                var divSpan = new System.Web.UI.HtmlControls.HtmlGenericControl("span");
                divSpan.InnerText = _QueryCondition.DisplayText;
                divItem.Controls.Add(divSpan);
                //控件
                Control _control = null;
                var _field = _entityCache.FindById<SysField>(_QueryCondition.FieldId);
                switch ((DataTypeEnum)_QueryCondition.DataType)
                {
                    case DataTypeEnum.pbool:
                        _control = new RadioList();
                        break;
                    case DataTypeEnum.pdatetime:
                        _control = new DateTimeControl();
                        break;
                    case DataTypeEnum.pdecimal:
                        _control = new TextControl();
                        (_control as TextControl).TextType = TextControl.TextTypes.Decimal;
                        break;
                    case DataTypeEnum.penum:
                        _control = new ComboControl();
                        var _enumControl = _control as ComboControl;
                        _enumControl.AutoSelectFirst = true;
                        _enumControl.ShowEmptyItem = true;
                        foreach(var item in _field.RefEnum.EnumItems)
                        {
                            _enumControl.Items.Add(new ComboItem() { Text = item.DisplayText, Value = item.ItemId.ToString() });
                        }
                        _enumControl.DataBind();
                        break;
                    case DataTypeEnum.pfloat:
                        _control = new TextControl();
                        (_control as TextControl).TextType = TextControl.TextTypes.Float;
                        break;
                    case DataTypeEnum.pint:
                        _control = new TextControl();
                        (_control as TextControl).TextType = TextControl.TextTypes.Int32;
                        break;
                    case DataTypeEnum.plong:
                        _control = new TextControl();
                        (_control as TextControl).TextType = TextControl.TextTypes.Int64;
                        break;
                    case DataTypeEnum.pref:
                        _control = new ComboControl();
                        var _refControl = _control as ComboControl;
                        _refControl.AutoSelectFirst = true;
                        var _EntitySchema = Drision.Framework.Manager.IEntitySchemaHelper.Get(_field.RefRelation.ParentEntity.EntityName);
                        var _DataList = this._DataHelper.FetchAll(_EntitySchema.EntityType);
                        foreach (var item in _DataList)
                        {
                            _refControl.Items.Add(new ComboItem() { Text = _EntitySchema.GetDisplayValue(item), Value = _EntitySchema.GetKeyValue(item).ToString() });
                        }
                        _refControl.ShowEmptyItem = true;
                        break;
                    case DataTypeEnum.pstring:
                    case DataTypeEnum.ptext:
                        _control = new TextControl();
                        (_control as TextControl).TextType = TextControl.TextTypes.String;
                        break;
                    default:
                        //case DataTypeEnum.MultiRef:
                        //case DataTypeEnum.pbinary:
                        //case DataTypeEnum.pfile:
                        //case DataTypeEnum.pkey:
                        //case DataTypeEnum.ppassword:
                        throw new Exception("暂不支持 多引用、二进制、附件、主键、密码等类型的字段作为查询条件！");

                }
                if (_control != null)
                {
                    _control.ID = _QueryCondition.ControlName;
                    (_control as IDrisionControl).FieldName = _QueryCondition.FieldId.ToString();
                    (_control as IDrisionControl).Tag = _QueryCondition.IsSubQuery == true ? "IsSubQuery" : "";
                }
                divItem.Controls.Add(_control);

                _ParentControl.Controls.Add(divItem);
            }
            //最后加一个cl
            var divcl = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            divcl.Attributes["class"] = "cl";
            _ParentControl.Controls.Add(divcl);
        }
        
        /// <summary>
        /// 查询最后的汇总数据
        /// </summary>
        /// <param name="divCondition"></param>
        /// <param name="DataHelper"></param>
        /// <returns></returns>
        public DataTable GetGroupSumData(Control divCondition)
        {
            var _BaseData = GetBaseData(divCondition);
            var _result = _BaseData;
            #region 如果有分组，则分组，汇总，二次过滤
            if (this._Report.IsGroup == true)
            {
                #region 处理后的数据结构
                _result = new DataTable();
                //字段规定好类型，否则会出现排序不对的情况
                foreach (var _ReportGroupField in _ReportGroupFields)
                {
                    var _Column = new DataColumn(_ReportGroupField.QueryFieldAliases, _BaseData.Columns[_ReportGroupField.QueryFieldAliases].DataType);
                    _result.Columns.Add(_Column);
                }
                foreach (var _ReportSumFiled in _ReportSumFileds)
                {
                    var _Column = new DataColumn(_ReportSumFiled.Aliases, typeof(decimal));
                    _result.Columns.Add(_Column);
                }
                #endregion
                //有数据再分组汇总
                if (_BaseData.Rows.Count > 0)
                {
                    #region 按分组字段分成各个子表
                    //分组好的各子表
                    Dictionary<string, DataTable> _subTables = new Dictionary<string, DataTable>();
                    foreach (DataRow dr in _BaseData.Rows)
                    {
                        var _key = string.Empty;
                        foreach (var _ReportGroupField in _ReportGroupFields)
                        {
                            if (dr[_ReportGroupField.QueryFieldAliases] == DBNull.Value)
                            {
                                _key = string.Format("{0}{1}_{2}_", _key, _ReportGroupField.QueryFieldAliases, "");
                            }
                            else
                            {
                                _key = string.Format("{0}{1}_{2}_", _key, _ReportGroupField.QueryFieldAliases, dr[_ReportGroupField.QueryFieldAliases]);
                            }
                        }
                        DataTable _subTable;
                        if (_subTables.TryGetValue(_key, out _subTable))
                        {
                            _subTable.ImportRow(dr);
                        }
                        else
                        {
                            _subTable = _BaseData.Clone();
                            _subTable.ImportRow(dr);
                            _subTables.Add(_key, _subTable);
                        }
                    }
                    #endregion

                    #region 循环子表，并汇总入总表
                    foreach (var _SubTable in _subTables.Values)
                    {
                        //每个子表处理成一条汇总数据
                        var _SumRow = _result.NewRow();
                        //现将分组字段值存入
                        foreach (var _ReportGroupField in _ReportGroupFields)
                        {
                            //每行的分组字段都一样，并且一定有至少一行，所以取第一行
                            _SumRow[_ReportGroupField.QueryFieldAliases] = _SubTable.Rows[0][_ReportGroupField.QueryFieldAliases];
                        }
                        foreach (var _ReportSumFiled in _ReportSumFileds)
                        {
                            switch ((ReportSumTypeEnum)_ReportSumFiled.SumType)
                            {
                                case ReportSumTypeEnum.Avg:
                                    _SumRow[_ReportSumFiled.Aliases] = _SubTable.Average(_ReportSumFiled.QueryFieldAliases);
                                    break;
                                case ReportSumTypeEnum.Count:
                                    _SumRow[_ReportSumFiled.Aliases] = _SubTable.Count();
                                    break;
                                case ReportSumTypeEnum.Max:
                                    _SumRow[_ReportSumFiled.Aliases] = _SubTable.Max(_ReportSumFiled.QueryFieldAliases);
                                    break;
                                case ReportSumTypeEnum.Min:
                                    _SumRow[_ReportSumFiled.Aliases] = _SubTable.Min(_ReportSumFiled.QueryFieldAliases);
                                    break;
                                case ReportSumTypeEnum.Sum:
                                    _SumRow[_ReportSumFiled.Aliases] = _SubTable.Sum(_ReportSumFiled.QueryFieldAliases);
                                    break;
                            }
                        }
                        _result.Rows.Add(_SumRow);
                    }
                    #endregion

                    #region having 二次过滤
                    var strHaving = string.Empty;
                    if (this._Report.HavingRelation != null && this._ReportHavingFields.Count > 0)
                    {
                        var _HavingRelation = (ReportHavingRelationEnum)this._Report.HavingRelation;
                        if (_HavingRelation == ReportHavingRelationEnum.And)
                        {
                            strHaving = "1=1";
                            foreach (var _ReportHavingField in this._ReportHavingFields)
                            {
                                strHaving = string.Format(" {0} and {1}{2}", strHaving, _ReportHavingField.SumFieldAliases, string.Format(EnumHelper.GetDescription(typeof(ReportCompareTypeEnum), _ReportHavingField.CompareType.Value), _ReportHavingField.CompareValue));
                            }
                        }
                        else
                        {
                            strHaving = "1=2";
                            foreach (var _ReportHavingField in this._ReportHavingFields)
                            {
                                strHaving = string.Format(" {0} or {1}{2}", strHaving, _ReportHavingField.SumFieldAliases, string.Format(EnumHelper.GetDescription(typeof(ReportCompareTypeEnum), _ReportHavingField.CompareType.Value), _ReportHavingField.CompareValue));
                            }
                        }
                        var _resultView = _result.DefaultView;
                        _resultView.RowFilter = strHaving;
                        _result = _resultView.ToTable();
                    }
                    #endregion
                }
            }
            #endregion

            #region 排序
            var strSort = string.Empty;
            for (int i = 0; i < this._ReportSortFields.Count; i++)
            {
                if (i == 0)
                {
                    strSort = string.Format("{0} {1}", this._ReportSortFields[i].Aliases, (ReportSortTypeEnum)this._ReportSortFields[i].SortType);
                }
                else
                {
                    strSort = string.Format("{0},{1} {2}", strSort,this._ReportSortFields[i].Aliases, (ReportSortTypeEnum)this._ReportSortFields[i].SortType);
                }
            }
            if (!string.IsNullOrEmpty(strSort))
            {
                var _resultView = _result.DefaultView;
                _resultView.Sort = strSort;
                _result = _resultView.ToTable();
            }
            #endregion

            #region 列转行(要在排序后，并且有分组汇总才能列转行，列传行字段只能是分组字段)
            if (this._Report.IsGroup == true && this._Report.IsChangeColumn == true)
            {
                DataTable _dtChangeColumnField = _result.DefaultView.ToTable(true, this._ReportChangeColumn.ColumnFieldAliases);
                //处理后的表
                DataTable new_DataTable = new DataTable();
                // 先将显示行列加入
                DataColumn new_d_col = new DataColumn();
                new_d_col.ColumnName = this._ReportChangeColumn.RowFieldAliases;
                new_DataTable.Columns.Add(new_d_col);
                //再加入转列的列
                foreach (DataRow dr in _dtChangeColumnField.Rows)
                {
                    var _ColumnName = dr[this._ReportChangeColumn.ColumnFieldAliases].ToString();
                    if (string.IsNullOrEmpty(_ColumnName))
                    {
                        continue;
                    }
                    new_d_col = new DataColumn();
                    new_d_col.DataType = typeof(decimal);
                    new_d_col.ColumnName = dr[this._ReportChangeColumn.ColumnFieldAliases].ToString();
                    new_d_col.DefaultValue = 0;
                    new_DataTable.Columns.Add(new_d_col);
                }
                //取出关键字的所有值
                DataTable _dtKeyField = _result.DefaultView.ToTable(true, this._ReportChangeColumn.RowFieldAliases);
                foreach (DataRow dr in _dtKeyField.Rows)
                {
                    if (dr[this._ReportChangeColumn.RowFieldAliases] == DBNull.Value || string.IsNullOrEmpty(dr[this._ReportChangeColumn.RowFieldAliases].ToString()))
                    {
                        continue;
                    }
                    var new_dr = new_DataTable.NewRow();
                    new_dr[this._ReportChangeColumn.RowFieldAliases] = dr[this._ReportChangeColumn.RowFieldAliases];

                    foreach (DataRow _dr in _dtChangeColumnField.Rows)
                    {
                        //按关键字和转列字段的值查询出该行，如果找到，把这个显示值作为转列后的显示值
                        var drs = _result.Select(string.Format("{0}='{1}' and {2}='{3}'", this._ReportChangeColumn.RowFieldAliases, dr[this._ReportChangeColumn.RowFieldAliases], this._ReportChangeColumn.ColumnFieldAliases, _dr[this._ReportChangeColumn.ColumnFieldAliases]));
                        if (drs.Length != 0)
                        {
                            new_dr[_dr[this._ReportChangeColumn.ColumnFieldAliases].ToString()] = drs.Sum(p=>Convert.ToDecimal(p[this._ReportChangeColumn.ValueFieldAliases]));
                        }
                    }
                    new_DataTable.Rows.Add(new_dr);
                }
                _result = new_DataTable;
            }
            #endregion

            #region TopN
            if (this._Report.TopN != null && this._Report.TopN != 0)
            {
                var _resultCopy = _result.Clone();
                for (int i = 0; i < _result.Rows.Count; i++)
                {
                    if (i >= this._Report.TopN)
                    {
                        break;
                    }
                    _resultCopy.ImportRow(_result.Rows[i]);
                }
                _result = _resultCopy;
            }
            #endregion

            return _result;
        }
        /// <summary>
        /// 列表绑定
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="result"></param>
        public void ReportGridBind(GridView gv, DataTable result)
        {
            gv.DataSource = result;
            gv.Columns.Clear();
            //如果行转列，只有关键字列和转列后的所有列，否则就是所有的显示列
            if (this._Report.IsChangeColumn == true)
            {
                var _column = new BoundField() { DataField = this._ReportChangeColumn.RowFieldAliases, HeaderText = this._ReportChangeColumn.RowFieldDisPlayText };
                _column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                gv.Columns.Add(_column);

                foreach (DataColumn dc in result.Columns)
                {
                    if (dc.ColumnName == this._ReportChangeColumn.RowFieldAliases)
                    {
                        continue;
                    }
                    _column = new BoundField() { DataField = dc.ColumnName, HeaderText = dc.ColumnName };
                    _column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    gv.Columns.Add(_column);
                }
            }
            else
            {
                foreach (var _ReportDisplayField in this._ReportDisplayFields)
                {
                    var _column = new BoundField() { DataField = _ReportDisplayField.Aliases, HeaderText = _ReportDisplayField.DisplayText };
                    _column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    gv.Columns.Add(_column);
                }
            }
            gv.DataBind();
        }
        /// <summary>
        /// 图形绑定
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="result"></param>
        public void ReportChartBind(Chart ch, DataTable result)
        {
            if (!this.IsHaveGraph())
            {
                return;
            }
            SeriesChartType _ChartType;
            switch ((ReportChartTypeEnum)this._ReportGraph.ChartType)
            {
                case ReportChartTypeEnum.Column:
                    _ChartType = SeriesChartType.Column;
                    break;
                case ReportChartTypeEnum.Line:
                    _ChartType = SeriesChartType.Line;
                    break;
                case ReportChartTypeEnum.Pie:
                    _ChartType = SeriesChartType.Pie;
                    break;
                case ReportChartTypeEnum.Spline:
                    _ChartType = SeriesChartType.Spline;
                    break;
                default:
                    _ChartType = SeriesChartType.Column;
                    break;
            }

            Dictionary<string, Series> dicSeries = new Dictionary<string, Series>();
            Series chSeries;
            //如果是行转列，除了关键列，其他都是Series
            if (this._Report.IsChangeColumn == true)
            {
                foreach (DataColumn dc in result.Columns)
                {
                    if (dc.ColumnName == this._ReportGraph.XMember)
                    {
                        continue;
                    }
                    chSeries = new Series(dc.ColumnName);
                    chSeries.IsValueShownAsLabel = true;
                    chSeries.ChartType = _ChartType;
                    ch.Series.Add(chSeries);
                    dicSeries.Add(dc.ColumnName, chSeries);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this._ReportGraph.SeriesMember))
                {
                    var tbSeries = result.DefaultView.ToTable(true, this._ReportGraph.SeriesMember);
                    foreach (DataRow dr in tbSeries.Rows)
                    {
                        chSeries = new Series(dr[this._ReportGraph.SeriesMember].ToString());
                        chSeries.IsValueShownAsLabel = true;
                        chSeries.ChartType = _ChartType;
                        ch.Series.Add(chSeries);
                        dicSeries.Add(dr[this._ReportGraph.SeriesMember].ToString(), chSeries);
                    }
                }
                else
                {
                    chSeries = new Series();
                    chSeries.IsValueShownAsLabel = true;
                    chSeries.ChartType = _ChartType;
                    ch.Series.Add(chSeries);
                    if (_ChartType != SeriesChartType.Pie)
                    {
                        ch.Legends.Clear();
                    }
                    else
                    {
                        ch.ChartAreas[0].Area3DStyle.Enable3D = true;
                        //格式化
                        //chSeries.ToolTip = "#VALX: #VAL{C} million";
                        chSeries.ToolTip = "#VALX: #VAL";
                        chSeries.LegendToolTip = "#PERCENT";
                        chSeries["PieLabelStyle"] = "Outside";

                        chSeries.PostBackValue = "#INDEX";
                        chSeries.LegendPostBackValue = "#INDEX";
                    }
                    dicSeries.Add("", chSeries);
                }
            }

            var dv = result.DefaultView;
            foreach (var _KeyValue in dicSeries)
            {
                //如果是行转列，其实就没有所谓值列了，是转后的列
                if (this._Report.IsChangeColumn == true)
                {
                    _KeyValue.Value.Points.DataBindXY(dv, this._ReportGraph.XMember, dv, _KeyValue.Key);
                }
                else
                {
                    if (!string.IsNullOrEmpty(this._ReportGraph.SeriesMember))
                    {
                        dv.RowFilter = string.Format("{0}='{1}'", this._ReportGraph.SeriesMember, _KeyValue.Key);
                    }
                    _KeyValue.Value.Points.DataBindXY(dv, this._ReportGraph.XMember, dv, this._ReportGraph.YMember);
                }
            }
        }
        /// <summary>
        /// 是否是饼状图
        /// </summary>
        /// <returns></returns>
        public bool IsPie()
        {
            return this._ReportGraph.ChartType == (int)ReportChartTypeEnum.Pie;
        }
        /// <summary>
        /// 是否配置图形
        /// </summary>
        /// <returns></returns>
        public bool IsHaveGraph()
        {
            return this._Report.IsGraph == true;
        }
        /// <summary>
        /// 收集条件，查询出基本数据
        /// </summary>
        /// <param name="_Report"></param>
        /// <param name="divCondition"></param>
        /// <param name="DataHelper"></param>
        /// <param name="_ReportQueryConditions"></param>
        /// <returns></returns>
        private DataTable GetBaseData(Control divCondition)
        {
            var _Parameters = new List<DbParameter>();
            foreach (var _Condition in _ReportQueryConditions)
            {
                var _control = divCondition.FindControl(_Condition.ControlName) as IDrisionControl;
                if (_control != null)
                {
                    var _value = _control.GetValue();
                    //是空值什么都不管，直接传空值查询
                    if (_value == null || string.IsNullOrEmpty(_value.ToString()))
                    {
                        _value = DBNull.Value;
                    }
                    //不是空值，如果是含子查询则将SysLevelCode查出来传进去，否则_value还是输入框的值
                    else if (_control.Tag == "IsSubQuery")
                    {
                        //这个控件的FieldName其实存的是FieldId
                        var _Field = _entityCache.FindById<SysField>(_control.FieldName.ToLong());
                        Type entityType = TableCache.GetTableType(_Field.RefEntity.EntityName);
                        //查出实体
                        var obj = this._DataHelper.FindById(entityType, _value.ToInt());
                        //获取SysLevelCode的值
                        _value = TableCache.GetTableOrCreate(entityType).GetColumnByColumnName("SystemLevelCode").GetFunction(obj);
                    }
                    _Parameters.Add(this._DataHelper.CreateParameter(this._DataHelper.AddPrefixToParameterName(_Condition.ControlName), _value));
                }
            }
            var strSQL = _Report.SQLScript.Replace("@CurrentUser", this.LoginUser.User_ID.ToString());
            //if (LoadEmpty)
            //{
            //    var _EntitySchema = Drision.Framework.Manager.IEntitySchemaHelper.Get(this._Report.EntityName);
            //    if (this._ReportQueryConditions.Count == 0 && this._ReportFixedConditions.Count == 0)
            //    {
            //        strSQL = string.Format("{0} where a.{1} = -1", strSQL, _EntitySchema.KeyName);
            //    }
            //    else
            //    {
            //        strSQL = string.Format("{0} and a.{1} = -1", strSQL, _EntitySchema.KeyName);
            //    }
            //}
            return this._DataHelper.ExecuteDataTable(strSQL, _Parameters.ToArray());
        }
        /// <summary>
        /// 保存处理菜单，并生成SQL语句
        /// </summary>
        /// <param name="tc"></param>
        public void CreateSQL()
        {
            //Select
            var sbSelect = new StringBuilder();
            int i = 0;
            foreach (var _ReportQueryField in this._ReportQueryFields)
            {
                if (i > 0)
                {
                    sbSelect.Append(",");
                }
                i++;
                sbSelect.AppendFormat("{0} as {1}", CreateSelect(_ReportQueryField), _ReportQueryField.Aliases);
            }
            sbSelect.AppendLine();
            //where
            var sbWhere = new StringBuilder();
            //固定条件
            var ParentFixedCondition = this._ReportFixedConditions.FirstOrDefault(p => p.ParentId == null);
            if (ParentFixedCondition != null)
            {
                sbWhere.AppendLine();
                sbWhere.Append("and ");
            }
            ReportFixedConditionFilter(ParentFixedCondition, sbWhere);
            sbWhere.AppendLine();
            //查询条件
            foreach (var _ReportQueryCondition in this._ReportQueryConditions)
            {
                if (_ReportQueryCondition.IsSubQuery == true)
                {
                    //将这个字段所在的表和这个字段所关联的表都加到Join中，然后取关联的表别名
                    var _RelationId = string.Format("{0}-{1}", _ReportQueryCondition.RelationId, _entityCache.FindById<SysField>(_ReportQueryCondition.FieldId).RefRelationId);
                    var _RefEntityAliase = GetTableAliase(_RelationId);
                    sbWhere.AppendFormat("and ({0}.SystemLevelCode Like @{1}+'%' or @{1} is null)", _RefEntityAliase, _ReportQueryCondition.ControlName);
                }
                else
                {
                    var _CompareTypeAndPara = string.Format(EnumHelper.GetDescription(typeof(ReportCompareTypeEnum), _ReportQueryCondition.CompareType.Value), "@" + _ReportQueryCondition.ControlName);
                    sbWhere.AppendFormat("and ({0}.{1} {3} or @{2} is null)", GetTableAliase(_ReportQueryCondition.RelationId), _ReportQueryCondition.FieldName, _ReportQueryCondition.ControlName, _CompareTypeAndPara);
                }
                sbWhere.AppendLine();
            }
            //from
            var sbFrom = new StringBuilder(string.Format("{0} a ",this._Report.EntityName));
            foreach (var _KeyValue in this.dicRelationTableAliases)
            {
                var _relationId = _KeyValue.Key;
                var _lastIndex = _KeyValue.Key.LastIndexOf('-');
                if (_lastIndex >= 0)
                {
                    _relationId = _KeyValue.Key.Substring(_lastIndex + 1);
                }
                var _relation = _entityCache.FindById<SysOneMoreRelation>(_relationId.ToLong());
                sbFrom.AppendLine();
                sbFrom.AppendFormat("left join {0} {1} on {4}.{2} = {1}.{3}", 
                    _relation.ParentEntity.EntityName, 
                    _KeyValue.Value, 
                    _relation.ChildField.FieldName, 
                    _relation.ParentField.FieldName, 
                    GetChildTableAliase(_KeyValue.Key));
            }
            sbFrom.AppendLine();

            this._Report.SQLScript = string.Format("Select {0}From {1}Where 1 = 1 {2}", sbSelect, sbFrom, sbWhere);
            this._DataHelper.UpdatePartial(this._Report, p => new { p.SQLScript });
        }

        private string CreateSelect(SysReportQueryField _ReportQueryField)
        {
            var result = string.Empty;
            if (_ReportQueryField.IsParent == true)
            {
                var _ReportQueryField1 = this._ReportQueryFields.FirstOrDefault(p => p.QueryFieldId == _ReportQueryField.Field1Id);
                var _ReportQueryField2 = this._ReportQueryFields.FirstOrDefault(p => p.QueryFieldId == _ReportQueryField.Field2Id);
                switch ((ReportFunctionTypeEnum)_ReportQueryField.FunctionType)
                {
                    case ReportFunctionTypeEnum.Add:
                        result = string.Format("({0})+({1})", CreateSelect(_ReportQueryField1), CreateSelect(_ReportQueryField2));
                        break;
                    case ReportFunctionTypeEnum.Div:
                        result = string.Format("({0})/({1})", CreateSelect(_ReportQueryField1), CreateSelect(_ReportQueryField2));
                        break;
                    case ReportFunctionTypeEnum.ReDiv:
                        result = string.Format("({0})/({1})", CreateSelect(_ReportQueryField2), CreateSelect(_ReportQueryField1));
                        break;
                    case ReportFunctionTypeEnum.Minus:
                        result = string.Format("({0})-({1})", CreateSelect(_ReportQueryField1), CreateSelect(_ReportQueryField2));
                        break;
                    case ReportFunctionTypeEnum.ReMinus:
                        result = string.Format("({0})-({1})", CreateSelect(_ReportQueryField2), CreateSelect(_ReportQueryField1));
                        break;
                    case ReportFunctionTypeEnum.Mul:
                        result = string.Format("({0})*({1})", CreateSelect(_ReportQueryField1), CreateSelect(_ReportQueryField2));
                        break;
                }
            }
            else
            {
                var _Field = _entityCache.FindById<SysField>(_ReportQueryField.Field1Id);
                StringBuilder sb;
                switch ((ReportFunctionTypeEnum)_ReportQueryField.FunctionType)
                {
                    case ReportFunctionTypeEnum.Add:
                        result = string.Format("ISNULL({0}.{1},0) + ISNULL({2}.{3},0) ", GetTableAliase(_ReportQueryField.Relation1Id), _ReportQueryField.Field1Name, GetTableAliase(_ReportQueryField.Relation2Id), _ReportQueryField.Field2Name);
                        break;
                    case ReportFunctionTypeEnum.Div:
                        result = string.Format("case {2}.{3} when 0 then 0 else {0}.{1}/{2}.{3} end ", GetTableAliase(_ReportQueryField.Relation1Id), _ReportQueryField.Field1Name, GetTableAliase(_ReportQueryField.Relation2Id), _ReportQueryField.Field2Name);
                        break;
                    case ReportFunctionTypeEnum.ReDiv:
                        result = string.Format("case {2}.{3} when 0 then 0 else {0}.{1}/{2}.{3} end ", GetTableAliase(_ReportQueryField.Relation2Id), _ReportQueryField.Field2Name, GetTableAliase(_ReportQueryField.Relation1Id), _ReportQueryField.Field1Name);
                        break;
                    case ReportFunctionTypeEnum.GetDate:
                        result = string.Format("CONVERT(varchar(10),{0}.{1},120) ", GetTableAliase(_ReportQueryField.Relation1Id), _ReportQueryField.Field1Name);
                        break;
                    case ReportFunctionTypeEnum.GetMonth:
                        result = string.Format("CONVERT(varchar(7),{0}.{1},120)+'月' ", GetTableAliase(_ReportQueryField.Relation1Id), _ReportQueryField.Field1Name);
                        break;
                    case ReportFunctionTypeEnum.GetYear:
                        result = string.Format("CONVERT(varchar(4),{0}.{1},120)+'年' ", GetTableAliase(_ReportQueryField.Relation1Id), _ReportQueryField.Field1Name);
                        break;
                    case ReportFunctionTypeEnum.JustAdd:
                        //如果是枚举
                        if (_Field.DataType == (int)DataTypeEnum.penum)
                        {
                            sb = new StringBuilder();
                            sb.AppendFormat("case {0}.{1}", GetTableAliase(_ReportQueryField.Relation1Id), _ReportQueryField.Field1Name);
                            foreach (var item in _Field.RefEnum.EnumItems)
                            {
                                sb.AppendFormat(" when {0} then '{1}'", item.ItemValue, item.DisplayText);
                            }
                            sb.Append(" end ");
                            result = sb.ToString();
                        }
                        else if (_Field.DataType == (int)DataTypeEnum.pbool)
                        {
                            result = string.Format("case {0}.{1} when 1 then '是' else '否' end ", GetTableAliase(_ReportQueryField.Relation1Id), _ReportQueryField.Field1Name);
                        }
                        else
                        {
                            result = string.Format("{0}.{1} ", GetTableAliase(_ReportQueryField.Relation1Id), _ReportQueryField.Field1Name);
                        }
                        break;
                    case ReportFunctionTypeEnum.Minus:
                        result = string.Format("ISNULL({0}.{1},0) - ISNULL({2}.{3},0) ", GetTableAliase(_ReportQueryField.Relation1Id), _ReportQueryField.Field1Name, GetTableAliase(_ReportQueryField.Relation2Id), _ReportQueryField.Field2Name);
                        break;
                    case ReportFunctionTypeEnum.ReMinus:
                        result = string.Format("ISNULL({0}.{1},0) - ISNULL({2}.{3},0) ", GetTableAliase(_ReportQueryField.Relation2Id), _ReportQueryField.Field2Name, GetTableAliase(_ReportQueryField.Relation1Id), _ReportQueryField.Field1Name);
                        break;
                    case ReportFunctionTypeEnum.Mul:
                        result = string.Format("{0}.{1}*{2}.{3} ", GetTableAliase(_ReportQueryField.Relation1Id), _ReportQueryField.Field1Name, GetTableAliase(_ReportQueryField.Relation2Id), _ReportQueryField.Field2Name);
                        break;
                }
            }
            return result;
        }
        /// <summary>
        /// 构造固定条件
        /// </summary>
        /// <param name="_ReportFixedCondition"></param>
        /// <param name="sb"></param>
        private void ReportFixedConditionFilter(SysReportFixedCondition _ReportFixedCondition, StringBuilder sb)
        {
            if (_ReportFixedCondition == null)
            {
                return;
            }
            var ChildConditions = this._ReportFixedConditions.Where(p=>p.ParentId ==  _ReportFixedCondition.ConditionId).ToList();

            switch (_ReportFixedCondition.FilterType)
            {
                case (int)FilterTypeEnum.AND:
                    sb.Append("(");
                    for (int i = 0; i < ChildConditions.Count; i++)
                    {
                        ReportFixedConditionFilter(ChildConditions[i], sb);
                        if (i != ChildConditions.Count - 1)
                        {
                            sb.Append(" and ");
                        }
                    }
                    sb.Append(")");
                    break;
                case (int)FilterTypeEnum.OR:
                    sb.Append("(");
                    for (int i = 0; i < ChildConditions.Count; i++)
                    {
                        ReportFixedConditionFilter(ChildConditions[i], sb);
                        if (i != ChildConditions.Count - 1)
                        {
                            sb.Append(" or ");
                        }
                    }
                    sb.Append(")");
                    break;
                case (int)FilterTypeEnum.CONDITION:
                    if (_ReportFixedCondition.CompareType == (int)ReportCompareTypeEnum.CurrentUser)
                    {
                        sb.AppendFormat(" {0}.{1} = @CurrentUser ", GetTableAliase(_ReportFixedCondition.RelationId), _ReportFixedCondition.FieldName);
                    }
                    else if(_ReportFixedCondition.CompareType == (int)ReportCompareTypeEnum.Today)
                    {
                        sb.AppendFormat(" CONVERT(varchar(10),{0}.{1},120) = CONVERT(varchar(10),GetDate(),120) ", GetTableAliase(_ReportFixedCondition.RelationId), _ReportFixedCondition.FieldName);
                    }
                    else if(_ReportFixedCondition.CompareType == (int)ReportCompareTypeEnum.ThisMonth)
                    {
                        sb.AppendFormat(" CONVERT(varchar(4),{0}.{1},120) = CONVERT(varchar(4),GetDate(),120) ", GetTableAliase(_ReportFixedCondition.RelationId), _ReportFixedCondition.FieldName);
                    }
                    else if (_ReportFixedCondition.CompareType == (int)ReportCompareTypeEnum.ThisYear)
                    {
                        sb.AppendFormat(" CONVERT(varchar(7),{0}.{1},120) = CONVERT(varchar(7),GetDate(),120) ", GetTableAliase(_ReportFixedCondition.RelationId), _ReportFixedCondition.FieldName);
                    }
                    else
                    {
                        var _ConditionValue = _ReportFixedCondition.ConditionValue;
                        //判断如果日期的话加'',字符串都是like比较，所以不用加
                        var _Field = _entityCache.FindById<SysField>(_ReportFixedCondition.FieldId);
                        var _DataType = (DataTypeEnum)_Field.DataType;
                        if (_DataType == DataTypeEnum.pdatetime || _DataType == DataTypeEnum.pstring || _DataType == DataTypeEnum.ptext)
                        {
                            _ConditionValue = string.Format("'{0}'", _ConditionValue);
                        }
                        sb.AppendFormat(" {0}.{1} {2} ", GetTableAliase(_ReportFixedCondition.RelationId), _ReportFixedCondition.FieldName, string.Format(EnumHelper.GetDescription(typeof(ReportCompareTypeEnum), _ReportFixedCondition.CompareType.Value), _ConditionValue));
                    }
                    break;
            }
        }
        /// <summary>
        /// 获取表别名(有则获取，无则添加，后面的join以此为准)
        /// </summary>
        /// <param name="RelationId"></param>
        /// <returns></returns>
        private string GetTableAliase(long? RelationId)
        {
            if (this.dicRelationTableAliases == null)
            {
                this.dicRelationTableAliases = new Dictionary<string, string>();
            }
            var result = "a";
            if (RelationId != null && RelationId != 0)
            {
                if (!dicRelationTableAliases.TryGetValue(RelationId.ToString(), out result))
                {
                    result = ((char)((int)'b' + dicRelationTableAliases.Count)).ToString();
                    dicRelationTableAliases.Add(RelationId.ToString(), result);
                }
            }
            return result;
        }
        /// <summary>
        /// 支持多层Id，如100-101，它会一层一层加进去
        /// </summary>
        /// <param name="RelationId"></param>
        /// <returns></returns>
        private string GetTableAliase(string RelationId)
        {
            if (this.dicRelationTableAliases == null)
            {
                this.dicRelationTableAliases = new Dictionary<string, string>();
            }
            var result = "a";
            if (!string.IsNullOrEmpty(RelationId))
            {
                var _LastIndex = RelationId.LastIndexOf('-');
                if (_LastIndex >= 0)
                {
                    GetTableAliase(RelationId.Substring(0, _LastIndex));
                }
                if (!dicRelationTableAliases.TryGetValue(RelationId, out result))
                {
                    result = ((char)((int)'b' + dicRelationTableAliases.Count)).ToString();
                    dicRelationTableAliases.Add(RelationId, result);
                }
            }
            return result;
        }
        /// <summary>
        /// 取这个关联表与哪个join的表别名
        /// </summary>
        /// <param name="RelationId"></param>
        /// <returns></returns>
        private string GetChildTableAliase(string RelationId)
        {
            if (this.dicRelationTableAliases == null)
            {
                this.dicRelationTableAliases = new Dictionary<string, string>();
            }
            var result = "a";
            var _LastIndex = RelationId.LastIndexOf('-');
            if (_LastIndex >= 0)
            {
                result = GetTableAliase(RelationId.Substring(0, _LastIndex));
            }
            return result;
        }
        /// <summary>
        /// 删除配置的菜单角色和真正的菜单功能
        /// </summary>
        /// <param name="DataHelper"></param>
        public void RemoveFunctionRole()
        {
            //删除报表配置的角色
            var _ReportMenuRoles = this._DataHelper.Where<SysReportMenuRole>(p => p.ReportId == this._ReportId);
            _ReportMenuRoles.ForEach(p =>
            {
                this._DataHelper.Delete(p);
            });
            if (this._Report.MenuId != null)
            {
                //删除菜单
                var _Function = this._DataHelper.FindById<SysFunction>(this._Report.MenuId.Value);
                if (_Function != null)
                {
                    this._DataHelper.Delete(_Function);
                }
                //删除菜单对应的角色
                var _DeleteRoleFunctionSQL = string.Format("delete from T_Role_Function where Function_ID = {0}", this._Report.MenuId);
                this._DataHelper.ExecuteNonQuery(_DeleteRoleFunctionSQL);
                //置空报表的菜单Id
                this._Report.MenuId = null;
                this._DataHelper.UpdatePartial(this._Report, p => new { p.MenuId });
            }
        }
        /// <summary>
        /// 保存配置的菜单角色并真正应用于菜单
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="DataHelper"></param>
        public void AddFunctionRoleAndPublish(TreeControl tc, long? Permission_Type)
        {
            //先加配置的角色
            foreach (var _roleId in tc.CheckedValue)
            {
                var model = new SysReportMenuRole();
                model.MenuRoleId = this._DataHelper.GetNextIdentity_Int();
                model.ReportId = this._ReportId;
                model.RoleId = _roleId.ToInt();
                this._DataHelper.Insert(model);
            }
            //再加菜单,如果选择了父，直接保存，否则找到或创建父
            if (Permission_Type == null)
            {
                var _ReportFunctionGroup = this._DataHelper.FirstOrDefault<SysFunction>(p => p.Permission_Name == "报表查询" && p.Permission_Type == null && p.Is_Menu == 1);
                if (_ReportFunctionGroup == null)
                {
                    _ReportFunctionGroup = new SysFunction();
                    _ReportFunctionGroup.Function_ID = this._DataHelper.GetNextIdentity_Int();
                    _ReportFunctionGroup.Permission_Name = "报表查询";
                    _ReportFunctionGroup.Is_Menu = 1;
                    this._DataHelper.Insert(_ReportFunctionGroup);
                }
                Permission_Type = _ReportFunctionGroup.Function_ID;
            }
            var _ReportFunction = new SysFunction();
            _ReportFunction.Function_ID = this._DataHelper.GetNextIdentity_Int();
            _ReportFunction.Permission_Name = this._Report.ReportName;
            _ReportFunction.Permission_Type = Permission_Type;
            _ReportFunction.Is_Menu = 1;
            _ReportFunction.URL = string.Format("/Report/ReportShow.aspx?id={0}", this._ReportId);
            this._DataHelper.Insert(_ReportFunction);
            //再加菜单对应角色
            var _InsertRoleFunctionSQL = "insert into T_Role_Function (id,Role_ID,Function_ID) values ({0},{1},{2})";
            foreach (var _roleId in tc.CheckedValue)
            {
                this._DataHelper.ExecuteNonQuery(string.Format(_InsertRoleFunctionSQL, this._DataHelper.GetNextIdentity_Int(), _roleId, _ReportFunction.Function_ID));
            }
            //创建SQL
            CreateSQL();

            this._Report.State = (int)ReportStateEnum.Published;
            this._Report.MenuId = _ReportFunction.Function_ID;
            this._DataHelper.UpdatePartial(this._Report, p => new { p.State, p.MenuId });
        }

        /// <summary>
        /// 删除除基本信息外所有配置(暂时未用到)
        /// </summary>
        public void DeleteAllSet()
        {
            this._ReportQueryConditions.ForEach(p => { this._DataHelper.Delete(p); });
            this._ReportGroupFields.ForEach(p => { this._DataHelper.Delete(p); });
            this._ReportSumFileds.ForEach(p => { this._DataHelper.Delete(p); });
            this._ReportQueryFields.ForEach(p => { this._DataHelper.Delete(p); });
            this._ReportDisplayFields.ForEach(p => { this._DataHelper.Delete(p); });
            this._ReportHavingFields.ForEach(p => { this._DataHelper.Delete(p); });
            this._ReportSortFields.ForEach(p => { this._DataHelper.Delete(p); });
            if (_ReportGraph != null)
            {
                this._DataHelper.Delete(_ReportGraph);
            }
            if (_ReportChangeColumn != null)
            {
                this._DataHelper.Delete(_ReportChangeColumn);
            }
            this._ReportFixedConditions.ForEach(p => { this._DataHelper.Delete(p); });
        }
    }
}