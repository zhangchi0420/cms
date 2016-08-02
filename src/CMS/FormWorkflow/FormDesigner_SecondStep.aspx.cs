//表单设计第二步，配置表单区域、字段
//zhumin
//2013-8-26

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Common;
using System.Text;
using System.Web.Script.Serialization;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormDesigner_SecondStep : BasePage
    {
        /// <summary>
        /// 表单ID（不可在回调时使用）
        /// </summary>
        protected long FormId
        {
            get { return VS<long>("FormId"); }
            set { VS<long>("FormId", value); }
        }

        /// <summary>
        /// 表单对应的实体ID（不可在回调时使用）
        /// </summary>
        protected long EntityId
        {
            get { return VS<long>("EntityId"); }
            set { VS<long>("EntityId", value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Initialize();
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndRedirect(ex, "FormQuery.aspx");
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            long? id = QueryString<long?>("id");
            if (id == null)
            {
                throw new Exception("参数不正确");
            }
            SysForm form = this.DataHelper.FindById<SysForm>(id);
            if (form == null)
            {
                throw new Exception("参数不正确");
            }
            this.FormId = id.Value;
            this.EntityId = form.EntityId.Value;


            BindSection();
            BindSelectField();
            BindDrowDown();
        }

        /// <summary>
        /// 获得表单字段的显示名称
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetFormFieldDisplayText(SysFormField p)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(p.CustomLabel)) //自定义标签
            {
                result = p.CustomLabel;
            }
            else
            {
                var field = GetField(p.FieldId);
                if (field != null)
                {
                    result = field.DisplayText;
                }
            }
            return result;
        }

        /// <summary>
        /// 绑定表单区域+字段
        /// </summary>
        private void BindSection()
        {
            //所有该表单的字段，根据区域分组
            var formFieldDict = this.DataHelper.Where<SysFormField>(p => p.FormId == this.FormId).GroupBy(p => p.FormSectionId)
                .ToDictionary(p => p.Key, p => p.Select(x => new
                {
                    x.FormFieldId,
                    DisplayText = GetFormFieldDisplayText(x),
                    x.DisplayOrder,
                }).OrderBy(x => x.DisplayOrder).ToList());

            //所有该表单的区域
            var sectionList = this.DataHelper.Set<SysFormFieldSection>()
                .Where(p => p.FormId == this.FormId)
                .OrderBy(p => p.DisplayOrder)
                .Select(p => new
                {
                    p.FormId,
                    p.FormSectionId,
                    p.DisplayOrder,
                    Fields = formFieldDict.ContainsKey(p.FormSectionId) ? formFieldDict[p.FormSectionId] : null,
                });

            this.rSection.DataSource = sectionList;
            this.rSection.DataBind();
        }

        /// <summary>
        /// 绑定现有字段
        /// </summary>
        private void BindSelectField()
        {
            List<EntityAndField> source = new List<EntityAndField>();
            SysEntity entity = this.EntityCache.FindById<SysEntity>(this.EntityId);
            if (entity != null)
            {
                this.hc.Value = "Y"; //表示元数据实体，不能再新增字段
                source.Add(new EntityAndField()
                {
                    DisplayName = entity.DisplayText,
                    Fields = entity.Fields.Where(p => //2013-9-18 zhumin 过滤不支持的字段类型
                        p.DataType != (int)DataTypeEnum.pkey
                     && p.DataType != (int)DataTypeEnum.MultiRef
                     && p.DataType != (int)DataTypeEnum.pbinary
                     ).OrderBy(p => p.FieldId).ToList()
                });

                //2013-9-18 zhumin 突然发现这里用不着关联实体的字段，至少这些字段是不可编辑的，暂时不支持
                //foreach (var relation in entity.ChildOneMoreRelations)
                //{
                //    source.Add(new EntityAndField()
                //    {
                //        DisplayName = string.Format("{0}=>{1}", entity.DisplayText, relation.ChildField.FieldName),
                //        RelationId = relation.RelationId,
                //        Fields = relation.ParentEntity.Fields.OrderBy(p => p.FieldId).ToList(),
                //    });
                //}
            }
            else
            {
                this.hc.Value = "N"; //表示上一步自定义实体，可以新增字段
                entity = this.DataHelper.FindById<SysEntity>(this.EntityId);
                if (entity != null)
                {
                    source.Add(new EntityAndField()
                    {
                        DisplayName = entity.DisplayText,
                        Fields = this.DataHelper.Set<SysField>().Where(p => p.EntityId == this.EntityId).OrderBy(p => p.FieldId).ToList(),
                    });
                }
            }
            this.rRelation.DataSource = source;
            this.rRelation.DataBind();
        }

        /// <summary>
        /// 绑定下拉框 
        /// </summary>
        private void BindDrowDown()
        {
            //新增表单字段的数据类型
            this.ccFieldType.DataSource = typeof(FormFieldDataType);
            this.ccFieldType.DataBind();

            //表单字段的显示方式
            this.ccDisplayType.DataSource = typeof(FormFieldDisplayType);
            this.ccDisplayType.DataBind();
        }

        #region 操作

        /// <summary>
        /// 所有操作的回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbc_CallBack(object sender, CallBackEventArgs e)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string designerContent = string.Empty; //设计器中的HTML内容，返回前台
            object otherContent = null; //其它内容，返回前台
            string errorMsg = string.Empty; //错误消息
            bool isError = false;

            try
            {
                string type = e.Context["Type"].ToString();
                long selectedId = e.Context["SelectedId"].ToLong();
                string parameter = e.Context["Parameter"].ToStringNullable();

                switch (type)
                {
                    case "NewSection": designerContent = NewSection(selectedId); break;
                    case "Delete": Delete(selectedId); break;
                    case "SelectField": designerContent = SelectField(selectedId, parameter); break;
                    case "NewField": designerContent = NewField(selectedId, parameter, ref otherContent); break;
                    case "MoveUp": MoveUp(selectedId); break;
                    case "MoveDown": MoveDown(selectedId); break;
                    case "PropertyRead": PropertyRead(selectedId, ref otherContent); break;
                    case "PropertySave": designerContent = PropertySave(selectedId, parameter); break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                isError = true;
                errorMsg = ex.Message;
            }

            string result = js.Serialize(new
            {
                IsError = isError,
                DesignerContent = designerContent,
                OtherContent = otherContent,
                Error = errorMsg,
            });
            e.Result = result;
        }

        /// <summary>
        /// 读取表单字段的属性
        /// </summary>
        /// <param name="formFieldId"></param>
        /// <param name="otherContent"></param>
        private void PropertyRead(long formFieldId, ref object otherContent)
        {
            SysFormField ff = this.DataHelper.FindById<SysFormField>(formFieldId);
            if (ff != null)
            {
                otherContent = new
                {
                    ff.DisplayType,
                    ff.MaxLength,
                    ff.MinLength,
                    ff.MaxValue,
                    ff.MinValue,
                    ff.IsNullable,
                    ff.CustomLabel,
                    ff.DefaultValue,
                };
            }
            else
            {
                throw new Exception("找不到表单字段");
            }
        }

        /// <summary>
        /// 保存表单字段的属性
        /// </summary>
        /// <param name="formFieldId"></param>
        /// <param name="parameter"></param>
        private string PropertySave(long formFieldId, string parameter)
        {
            SysFormField ff = this.DataHelper.FindById<SysFormField>(formFieldId);
            if (ff != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var valueDict = js.DeserializeObject(parameter) as Dictionary<string, object>;
                if (valueDict == null)
                {
                    throw new Exception("回调参数不正确");
                }

                int? displayType = valueDict["DisplayType"].ToStringNullable().ToIntNullable();
                int? maxLength = valueDict["MaxLength"].ToStringNullable().ToIntNullable();
                int? minLength = valueDict["MinLength"].ToStringNullable().ToIntNullable();
                int? maxValue = valueDict["MaxValue"].ToStringNullable().ToIntNullable();
                int? minValue = valueDict["MinValue"].ToStringNullable().ToIntNullable();
                bool? isNullable = valueDict["IsNullable"].ToStringNullable().ToLower().ToBoolNullable();
                string customLabel = valueDict["CustomLabel"].ToStringNullable();
                string defaultValue = valueDict["DefaultValue"].ToStringNullable();

                if (maxLength < minLength)
                {
                    throw new Exception("最大长度不能小于最小长度");
                }

                if (maxValue < minValue)
                {
                    throw new Exception("最大值不能小于最小值");
                }

                ff.DisplayType = displayType;
                ff.MaxLength = maxLength;
                ff.MinLength = minLength;
                ff.MaxValue = maxValue;
                ff.MinValue = minValue;
                ff.IsNullable = isNullable;
                ff.CustomLabel = customLabel;
                ff.DefaultValue = defaultValue;
                this.DataHelper.UpdatePartial(ff, p => new
                {
                    p.DisplayType,
                    p.MaxLength,
                    p.MinLength,
                    p.MaxValue,
                    p.MinValue,
                    p.IsNullable,
                    p.CustomLabel,
                    p.DefaultValue,
                });

                return customLabel;
            }
            else
            {
                throw new Exception("找不到表单字段");
            }
        }

        /// <summary>
        /// 前移
        /// </summary>
        /// <param name="selectedId"></param>
        private void MoveUp(long selectedId)
        {
            var section = this.DataHelper.FindById<SysFormFieldSection>(selectedId);
            if (section != null)
            {
                var sectionList = this.DataHelper.Set<SysFormFieldSection>()
                    .Where(p => p.FormId == section.FormId).OrderBy(p => p.DisplayOrder).ToList();
                int index = 0;
                for (int i = 0; i < sectionList.Count; i++)
                {
                    sectionList[i].DisplayOrder = i;
                    if (sectionList[i].FormSectionId == section.FormSectionId)
                    {
                        index = i;
                    }
                }

                if (index > 0)
                {
                    sectionList[index].DisplayOrder--;
                    sectionList[index - 1].DisplayOrder++;

                    foreach (var p in sectionList)
                    {
                        this.DataHelper.UpdatePartial<SysFormFieldSection>(p, x => new { x.DisplayOrder });
                    }
                }
                else
                {
                    throw new Exception("该表单区域已经在最前");
                }
            }
            else
            {
                var field = this.DataHelper.FindById<SysFormField>(selectedId);
                if (field != null)
                {
                    var fieldList = this.DataHelper.Set<SysFormField>()
                        .Where(p => p.FormSectionId == field.FormSectionId).OrderBy(p => p.DisplayOrder).ToList();

                    int index = 0;
                    for (int i = 0; i < fieldList.Count; i++)
                    {
                        fieldList[i].DisplayOrder = i;
                        if (fieldList[i].FormFieldId == field.FormFieldId)
                        {
                            index = i;
                        }
                    }
                    if (index > 0)
                    {
                        fieldList[index].DisplayOrder--;
                        fieldList[index - 1].DisplayOrder++;

                        foreach (var p in fieldList)
                        {
                            this.DataHelper.UpdatePartial<SysFormField>(p, x => new { x.DisplayOrder });
                        }
                    }
                    else
                    {
                        throw new Exception("该表单字段已经在最前");
                    }
                }
            }
        }

        /// <summary>
        /// 后移
        /// </summary>
        /// <param name="selectedId"></param>
        private void MoveDown(long selectedId)
        {
            var section = this.DataHelper.FindById<SysFormFieldSection>(selectedId);
            if (section != null)
            {
                var sectionList = this.DataHelper.Set<SysFormFieldSection>()
                    .Where(p => p.FormId == section.FormId).OrderBy(p => p.DisplayOrder).ToList();
                int index = 0;
                for (int i = 0; i < sectionList.Count; i++)
                {
                    sectionList[i].DisplayOrder = i;
                    if (sectionList[i].FormSectionId == section.FormSectionId)
                    {
                        index = i;
                    }
                }

                if (index < sectionList.Count - 1)
                {
                    sectionList[index].DisplayOrder++;
                    sectionList[index + 1].DisplayOrder--;

                    foreach (var p in sectionList)
                    {
                        this.DataHelper.UpdatePartial<SysFormFieldSection>(p, x => new { x.DisplayOrder });
                    }
                }
                else
                {
                    throw new Exception("该表单区域已经在最后");
                }
            }
            else
            {
                var field = this.DataHelper.FindById<SysFormField>(selectedId);
                if (field != null)
                {
                    var fieldList = this.DataHelper.Set<SysFormField>()
                        .Where(p => p.FormSectionId == field.FormSectionId).OrderBy(p => p.DisplayOrder).ToList();

                    int index = 0;
                    for (int i = 0; i < fieldList.Count; i++)
                    {
                        fieldList[i].DisplayOrder = i;
                        if (fieldList[i].FormFieldId == field.FormFieldId)
                        {
                            index = i;
                        }
                    }
                    if (index < fieldList.Count - 1)
                    {
                        fieldList[index].DisplayOrder++;
                        fieldList[index + 1].DisplayOrder--;

                        foreach (var p in fieldList)
                        {
                            this.DataHelper.UpdatePartial<SysFormField>(p, x => new { x.DisplayOrder });
                        }
                    }
                    else
                    {
                        throw new Exception("该表单字段已经在最后");
                    }
                }
            }
        }

        /// <summary>
        /// 新增表单字段
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="parameter"></param>
        /// <param name="otherContent"></param>
        /// <returns></returns>
        private string NewField(long sectionId, string parameter, ref object otherContent)
        {
            string result = string.Empty;
            SysFormFieldSection section = this.DataHelper.FindById<SysFormFieldSection>(sectionId);
            if (section != null)
            {
                SysForm form = this.DataHelper.FindById<SysForm>(section.FormId);
                if (form == null)
                {
                    throw new Exception("表单不存在");
                }

                JavaScriptSerializer js = new JavaScriptSerializer();
                var valueDict = js.DeserializeObject(parameter) as Dictionary<string, object>;
                if (valueDict == null)
                {
                    throw new Exception("回调参数不正确");
                }
                string displayText = valueDict["DisplayText"].ToStringNullable();
                string fieldName = valueDict["FieldName"].ToStringNullable();
                int? dataType = valueDict["DataType"].ToStringNullable().ToIntNullable();

                int temp = this.DataHelper.Set<SysField>().Where(p => p.EntityId == form.EntityId
                    && (p.DisplayText == displayText || p.FieldName == fieldName)).Count();
                if (temp > 0)
                {
                    throw new Exception("当前新增的字段名称已经存在");
                }

                int maxOrder = this.DataHelper.Set<SysFormField>().Where(p => p.FormSectionId == sectionId).Max(p => p.DisplayOrder ?? 0);

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        //新增字段
                        SysField field = new SysField()
                        {
                            FieldId = db.GetNextIdentity(),
                            DisplayText = displayText,
                            FieldName = fieldName,
                            EntityId = form.EntityId,
                            Description = displayText,
                            DataType = dataType,
                            IsFormField = true,//2013-9-24 zhumin
                        };
                        db.Insert(field);

                        //新增表单字段
                        SysFormField ff = new SysFormField()
                        {
                            FormFieldId = db.GetNextIdentity(),
                            FormSectionId = sectionId,
                            EntityId = form.EntityId,
                            DisplayOrder = maxOrder + 1,
                            FormId = form.FormId,
                            FieldId = field.FieldId,
                            DataType = field.DataType,
                            IsNullable = true,

                            CreateTime = DateTime.Now,
                            CreateUserId = this.LoginUserID,
                        };
                        db.Insert(ff);

                        result = string.Format("<div id=\"{0}\" class=\"field unselected\" onclick=\"Select(this);\">{1}</div>", ff.FormFieldId, field.DisplayText);
                        otherContent = string.Format("<div class=\"divField\"><span><input type=\"checkbox\" fid=\"{0}\" /></span><span>{1}</span><span>{2}</span></div>", field.FieldId, field.DisplayText, field.FieldName);
                    }
                    ts.Complete();
                }
            }

            return result;
        }

        /// <summary>
        /// 新增表单区域
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        private string NewSection(long formId)
        {
            int maxOrder = this.DataHelper.Set<SysFormFieldSection>().Where(p => p.FormId == formId).Max(p => p.DisplayOrder ?? 0);

            SysFormFieldSection section = new SysFormFieldSection()
            {
                FormSectionId = this.DataHelper.GetNextIdentity(),
                FormId = formId,
                DisplayOrder = maxOrder + 1,
                CreateTime = DateTime.Now,
                CreateUserId = this.LoginUserID,
            };
            this.DataHelper.Insert(section);

            return string.Format("<div id=\"{0}\" class=\"section\" onclick=\"Select(this);\"></div>", section.FormSectionId);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="selectedId"></param>
        private void Delete(long selectedId)
        {
            var section = this.DataHelper.FindById<SysFormFieldSection>(selectedId);
            if (section != null)
            {
                var fieldList = this.DataHelper.Where<SysFormField>(p => p.FormSectionId == selectedId);

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        foreach (var field in fieldList)
                        {
                            db.Delete(field);
                        }
                        db.Delete(section);
                    }
                    ts.Complete();
                }
            }
            else
            {
                var field = this.DataHelper.FindById<SysFormField>(selectedId);
                if (field != null)
                {
                    this.DataHelper.Delete(field);
                }
            }
        }

        /// <summary>
        /// 选中现有字段并添加为表单字段
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private string SelectField(long sectionId, string parameter)
        {
            StringBuilder sb = new StringBuilder();

            SysFormFieldSection section = this.DataHelper.FindById<SysFormFieldSection>(sectionId);
            if (section != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                object[] fieldList = js.DeserializeObject(parameter) as object[];
                if (fieldList == null)
                {
                    throw new Exception("回调参数不正确");
                }

                var source = fieldList.Select(p => p as Dictionary<string, object>)
                .Select(p => new
                {
                    FieldId = p["FieldId"].ToLong(),
                    RelationId = p["RelationId"].ToStringNullable().ToLongNullable(),
                });

                int maxOrder = this.DataHelper.Set<SysFormField>().Where(p => p.FormSectionId == sectionId).Max(p => p.DisplayOrder ?? 0);

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        foreach (var p in source)
                        {
                            SysField field = this.EntityCache.FindById<SysField>(p.FieldId);
                            if (field == null)
                            {
                                field = db.FindById<SysField>(p.FieldId);
                            }
                            if (field != null)
                            {
                                SysFormField ff = new SysFormField()
                                {
                                    FormFieldId = db.GetNextIdentity(),
                                    FormSectionId = sectionId,
                                    FormId = section.FormId,

                                    FieldId = p.FieldId,
                                    RelationId = p.RelationId,
                                    DisplayOrder = ++maxOrder,
                                    EntityId = field.EntityId,
                                    DataType = field.DataType,
                                    IsNullable = true,

                                    CreateUserId = this.LoginUserID,
                                    CreateTime = DateTime.Now,
                                };

                                db.Insert(ff);

                                sb.AppendFormat("<div id=\"{0}\" class=\"field unselected\" onclick=\"Select(this);\">{1}</div>", ff.FormFieldId, field.DisplayText);
                            }
                            else
                            {
                                throw new Exception("找不到关联字段");
                            }
                        }
                    }
                    ts.Complete();
                }
            }
            else
            {
                throw new Exception("找不到表单段落");
            }

            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("FormDesigner_FirstStep.aspx?id={0}", this.FormId));
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("FormDesigner_ThirdStep.aspx?id={0}", this.FormId));
        }
    }

    public class EntityAndField
    {
        public string DisplayName { get; set; }
        public List<SysField> Fields { get; set; }
        public long? RelationId { get; set; }
    }
}