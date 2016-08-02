using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Common;
using System.Data;
using System.Text;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormDesigner_FourthStep : BasePage
    {
        protected long? FormId
        {
            get { return VS<long?>("FormId"); }
            set { VS<long?>("FormId", value); }
        }

        protected long? EntityId
        {
            get { return VS<long?>("EntityId"); }
            set { VS<long?>("EntityId", value); }
        }        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Initialize();
                }
                this.formPreview.LoadPreview(this.FormId.Value);
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndRedirect(ex, "FormQuery.aspx");
            }
        }

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
        }

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("FormDesigner_ThirdStep.aspx?id={0}", this.FormId));
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPublish_Click(object sender, EventArgs e)
        {
            try
            {
                SysForm form = this.DataHelper.FindById<SysForm>(this.FormId);
                if (form != null)
                {
                    using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                    {
                        using (BizDataContext db = new BizDataContext())
                        {
                            //更新表单状态
                            form.State = (int)FormState.StartUsed;
                            form.UpdateTime = DateTime.Now;
                            form.UpdateUserId = this.LoginUserID;
                            db.UpdatePartial(form, p => new { p.State, p.UpdateUserId, p.UpdateTime });

                            //发布数据库
                            SysEntity entity = this.EntityCache.FindById<SysEntity>(this.EntityId);
                            if (entity == null) //新增加的实体
                            {
                                entity = db.FindById<SysEntity>(this.EntityId);
                                if (entity == null)
                                {
                                    throw new Exception("表单关联实体不存在");
                                }
                                entity.Fields = db.Where<SysField>(p => p.EntityId == entity.EntityId);
                                
                                FormEntityPublishHelper publish = new FormEntityPublishHelper(entity,db);
                                publish.Publish();
                            }
                        }
                        ts.Complete();
                    }
                }
                else
                {
                    throw new Exception("表单不存在");
                }
                this.AjaxAlertAndRedirect("发布成功", "FormQuery.aspx");
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }
    }

    public class FormEntityPublishHelper
    {
        private SysEntity _entity;
        private List<SysField> _fieldList;
        private BizDataContext _db;
        private Drision.Framework.LiteQueryDef.DatabaseTypeEnum _dbType;


        public FormEntityPublishHelper(SysEntity entity,BizDataContext db)
        {
            this._entity = entity;
            this._fieldList = entity.Fields.ToList();
            this._db = db;
            this._dbType = db.DatabaseType;
        }

        public void Publish()        
        {
            switch (_dbType)
            {
                case LiteQueryDef.DatabaseTypeEnum.Oracle: InternalPublish_ForOracle(); break;
                case LiteQueryDef.DatabaseTypeEnum.SqlServer: InternalPublish_ForSqlServer(); break;
                default: break;
            }
        }

        private void InternalPublish_ForSqlServer()
        {
            var existTableDict = _db.ExecuteDataTable("select TABLE_NAME,COLUMN_NAME from information_schema.columns").Rows.Cast<DataRow>()
                .Select(p => new
                {
                    TableName = Convert.ToString(p["TABLE_NAME"]).ToLower(),
                    ColumnName = Convert.ToString(p["COLUMN_NAME"]).ToLower(),
                }).GroupBy(p => p.TableName).ToDictionary(p => p.Key, p => p.Select(x => x.ColumnName).ToList());

            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendLine("--------------------------创建或更新表");
            if (existTableDict.ContainsKey(_entity.EntityName.ToLower()))//表已经存在，添加字段
            {
                var existColumnList = existTableDict[_entity.EntityName.ToLower()];
                foreach (SysField _field in _fieldList)
                {
                    if (existColumnList.IndexOf(_field.FieldName.ToLower()) < 0) //字段不存在
                    {
                        switch (_field.DataType)
                        {
                            case (int)FormFieldDataType.Int: sbSQL.AppendFormat(" ALTER TABLE [{0}] ADD [{1}] int ", _entity.EntityName, _field.FieldName); break;
                            case (int)FormFieldDataType.DateTime: sbSQL.AppendFormat(" ALTER TABLE [{0}] ADD [{1}] datetime ", _entity.EntityName, _field.FieldName); break;
                            case (int)FormFieldDataType.File: sbSQL.AppendFormat(" ALTER TABLE [{0}] ADD [{1}] int ", _entity.EntityName, _field.FieldName); break;
                            case (int)FormFieldDataType.Number: sbSQL.AppendFormat(" ALTER TABLE [{0}] ADD [{1}] decimal(38,10) ", _entity.EntityName, _field.FieldName); break;
                            case (int)FormFieldDataType.String: sbSQL.AppendFormat(" ALTER TABLE [{0}] ADD [{1}] nvarchar(256) ", _entity.EntityName, _field.FieldName); break;
                            case (int)FormFieldDataType.Text: sbSQL.AppendFormat(" ALTER TABLE [{0}] ADD [{1}] ntext ", _entity.EntityName, _field.FieldName); break;
                            default: throw new Exception(string.Format("表单字段类型不支持{0}", _field.DataType));
                        }
                    }
                }
            }
            else //表不存在
            {
                sbSQL.AppendFormat("CREATE TABLE [dbo].[{0}]( ", _entity.EntityName);
                string keyName = null;
                foreach (SysField _field in _fieldList)
                {
                    switch (_field.DataType)
                    {
                        case (int)DataTypeEnum.pkey: keyName = _field.FieldName; sbSQL.AppendFormat(" [{0}] [int] NOT NULL, ", _field.FieldName); break;
                        case (int)FormFieldDataType.Int: sbSQL.AppendFormat(" [{0}] [int] NULL, ", _field.FieldName); break;                        
                        case (int)FormFieldDataType.DateTime: sbSQL.AppendFormat(" [{0}] [datetime] NULL, ",  _field.FieldName); break;
                        case (int)FormFieldDataType.File: sbSQL.AppendFormat(" [{0}] [int] NULL, ", _field.FieldName); break;
                        case (int)FormFieldDataType.Number: sbSQL.AppendFormat(" [{0}] [decimal](38, 10) NULL, ",_field.FieldName); break;
                        case (int)FormFieldDataType.String: sbSQL.AppendFormat(" [{0}] [nvarchar](256) NULL, ",  _field.FieldName); break;
                        case (int)FormFieldDataType.Text: sbSQL.AppendFormat(" [{0}] [ntext] NULL, ", _field.FieldName); break;
                        default: throw new Exception(string.Format("表单字段类型不支持{0}", _field.DataType));
                    }
                }
                sbSQL.AppendFormat(@" CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
                                (
	                                [{1}] ASC
                                )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                                ) ON [PRIMARY]", _entity.EntityName, keyName);
            }
            _db.ExecuteNonQuery(sbSQL.ToString());
        }

        private void InternalPublish_ForOracle()
        {
            string sqlUser = "select UserName from user_users"; //当前数据库登录用户
            string user = Convert.ToString(_db.ExecuteScalar(sqlUser));

            var existTableDict = _db.ExecuteDataTable(string.Format("Select TABLE_NAME,COLUMN_NAME from DBA_TAB_COLUMNS where OWNER = '{0}'", user)).Rows.Cast<DataRow>()
                .Select(p => new
                {
                    TableName = Convert.ToString(p["TABLE_NAME"]).ToLower(),
                    ColumnName = Convert.ToString(p["COLUMN_NAME"]).ToLower(),
                }).GroupBy(p => p.TableName).ToDictionary(p => p.Key, p => p.Select(x => x.ColumnName).ToList());

                   
            if (existTableDict.ContainsKey(_entity.EntityName.ToLower()))//表已经存在，添加字段
            {
                var existColumnList = existTableDict[_entity.EntityName.ToLower()];
                foreach (SysField _field in _fieldList)
                {
                    if (existColumnList.IndexOf(_field.FieldName.ToLower()) < 0) //字段不存在
                    {
                        switch (_field.DataType)
                        {
                            case (int)FormFieldDataType.Int: _db.ExecuteNonQuery(string.Format(" ALTER TABLE {0} ADD {1} number(10) ", _entity.EntityName, _field.FieldName)); break;                            
                            case (int)FormFieldDataType.DateTime: _db.ExecuteNonQuery(string.Format(" ALTER TABLE {0} ADD {1} timestamp(6) ", _entity.EntityName, _field.FieldName)); break;
                            case (int)FormFieldDataType.File: _db.ExecuteNonQuery(string.Format(" ALTER TABLE {0} ADD {1} number(10) ", _entity.EntityName, _field.FieldName)); break;
                            case (int)FormFieldDataType.Number: _db.ExecuteNonQuery(string.Format(" ALTER TABLE {0} ADD {1} number(38,10) ", _entity.EntityName, _field.FieldName)); break;
                            case (int)FormFieldDataType.String: _db.ExecuteNonQuery(string.Format(" ALTER TABLE {0} ADD {1} nvarchar2(256) ", _entity.EntityName, _field.FieldName)); break;
                            case (int)FormFieldDataType.Text: _db.ExecuteNonQuery(string.Format(" ALTER TABLE {0} ADD {1} nclob ", _entity.EntityName, _field.FieldName)); break;
                            default: throw new Exception(string.Format("表单字段类型不支持{0}", _field.DataType));
                        }
                    }
                }
            }
            else //表不存在
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendFormat("CREATE TABLE {0} ( ", _entity.EntityName);
                string keyName = null;
                foreach (SysField _field in _fieldList)
                {
                    switch (_field.DataType)
                    {
                        case (int)DataTypeEnum.pkey: keyName = _field.FieldName; sbSQL.AppendFormat(" {0} number(10) NOT NULL, ", _field.FieldName); break;
                        case (int)FormFieldDataType.Int: sbSQL.AppendFormat(" {0} number(10) NULL, ", _field.FieldName); break;
                        case (int)FormFieldDataType.DateTime: sbSQL.AppendFormat(" {0} timestamp(6) NULL, ", _field.FieldName); break;
                        case (int)FormFieldDataType.File: sbSQL.AppendFormat(" {0} number(10) NULL, ", _field.FieldName); break;
                        case (int)FormFieldDataType.Number: sbSQL.AppendFormat(" {0} number(38, 10) NULL, ", _field.FieldName); break;
                        case (int)FormFieldDataType.String: sbSQL.AppendFormat(" {0} nvarchar2(256) NULL, ", _field.FieldName); break;
                        case (int)FormFieldDataType.Text: sbSQL.AppendFormat(" {0} nclob NULL, ", _field.FieldName); break;
                        default: throw new Exception(string.Format("表单字段类型不支持{0}", _field.DataType));
                    }
                }
                sbSQL.AppendFormat(" constraint PK_{0} primary key ({1}))", _entity.EntityName, keyName);
                _db.ExecuteNonQuery(sbSQL.ToString());
            }            
        }
    }
}