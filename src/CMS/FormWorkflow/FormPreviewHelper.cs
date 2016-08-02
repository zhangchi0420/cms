using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Drision.Framework.Common;
using System.Web.UI;
using Drision.Framework.Common.Workflow;
using System.Web.UI.WebControls;
using Drision.Framework.WebControls.Super;
using Drision.Framework.Manager;
using System.Data;

namespace Drision.Framework.Web.FormWorkflow
{
    /// <summary>
    /// 表单控件解析
    /// </summary>
    public class FormPreviewHelper
    {
        private static Dictionary<DataTypeEnum, Func<SysFormField,DataTypeControlHelperBase>> dict;
        
        static FormPreviewHelper()
        {
            dict = new Dictionary<DataTypeEnum, Func<SysFormField,DataTypeControlHelperBase>>();
            dict[DataTypeEnum.pint] = ff => new pint_ControlHelper(ff);
            dict[DataTypeEnum.pstring] = ff => new pstring_ControlHelper(ff);
            dict[DataTypeEnum.pdatetime] = ff => new pdatetime_ControlHelper(ff);
            dict[DataTypeEnum.pdecimal] = ff => new pdecimal_ControlHelper(ff);
            dict[DataTypeEnum.pfloat] = ff => new pdecimal_ControlHelper(ff);//float = decimal
            dict[DataTypeEnum.ptext] = ff => new ptext_ControlHelper(ff);
            dict[DataTypeEnum.plong] = ff => new plong_ControlHelper(ff);
            dict[DataTypeEnum.pfile] = ff => new pfile_ControlHelper(ff);
            dict[DataTypeEnum.pbool] = ff => new pbool_ControlHelper(ff);
            dict[DataTypeEnum.pref] = ff => new pref_ControlHelper(ff);
            dict[DataTypeEnum.penum] = ff => new penum_ControlHelper(ff);
            dict[DataTypeEnum.ppassword] = ff => new ppassword_ControlHelper(ff);
        }

        private static DataTypeControlHelperBase GetControlHelper(SysFormField ff)
        {
            if (ff.DisplayType == (int)FormFieldDisplayType.StaticText) //显示方式为静态文本
            {
                return new plabel_ControlHelper(ff);
            }
            else if (dict.ContainsKey((DataTypeEnum)ff.DataType))
            {
                return dict[(DataTypeEnum)ff.DataType](ff);
            }
            throw new Exception(string.Format("字段【{0}】的数据类型，暂不支持", ff.Field.FieldName));
        }

        /// <summary>
        /// 加载控件（非详情页面）
        /// </summary>
        /// <param name="ff">表单字段</param>
        /// <returns></returns>
        public static Control LoadControl(SysFormField ff)
        {
            return GetControlHelper(ff).LoadControl();
        }

        /// <summary>
        /// 加载控件（详情页面）
        /// </summary>
        /// <param name="ff">表单字段</param>
        /// <returns></returns>
        public static Control LoadControlForDetailPage(SysFormField ff)
        {
            return GetControlHelper(ff).LoadControlForDetailPage();
        }

        /// <summary>
        /// 给控件赋值（非详情页面）
        /// </summary>
        /// <param name="ff">表单字段</param>
        /// <param name="c">控件</param>
        /// <param name="value">值</param>
        public static void SetValue(SysFormField ff, IDrisionControl c,object value)
        {
            GetControlHelper(ff).SetValue(c, value);
        }

        /// <summary>
        /// 给控件赋值（详情页面）
        /// </summary>
        /// <param name="ff">表单字段</param>
        /// <param name="c">控件</param>
        /// <param name="value">值</param>
        public static void SetValueForDetailPage(SysFormField ff, IDrisionControl c, object value)
        {
            GetControlHelper(ff).SetValueForDetailPage(c, value);
        }
    }

    /// <summary>
    /// 不同数据类型对应控件的解析
    /// </summary>
    public abstract class DataTypeControlHelperBase
    {
        public DataTypeControlHelperBase(SysFormField ff)
        {
            this.DataType = (DataTypeEnum)ff.DataType;
            this.IsRequired = !(ff.IsNullable ?? true);
            this.MaxLength = ff.MaxLength ?? 0;
            this.MinLength = ff.MinLength ?? 0;
            this.MaxValue = ff.MaxValue;
            this.MinValue = ff.MinValue;
            this.DefaultValue = ff.DefaultValue;
            this.ID = string.Format("ff_{0}", ff.FormFieldId);
            this.Field = ff.Field;
            this.FieldName = ff.Field.FieldName;            
        }

        #region 属性

        protected SysField Field { get; private set; }
        protected string FieldName { get; private set; }
        protected string DefaultValue { get; private set; }
        protected string ID { get; private set; }
        protected int MaxLength { get; private set; }
        protected int MinLength { get; private set; }
        protected decimal? MaxValue { get; private set; }
        protected decimal? MinValue { get; private set; }
        protected bool IsRequired { get; private set; }
        protected DataTypeEnum DataType { get; private set; }

        #endregion

        protected virtual Control CreateControl()
        {
            SLabel c = new SLabel();
            c.CssClass = "formFieldValue";
            return c;
        }

        protected virtual Control CreateControlForDetailPage()
        {
            SLabel c = new SLabel();
            c.CssClass = "formFieldValue";
            return c;
        }

        public virtual void SetValue(IDrisionControl c, object value)
        {
            c.SetValue(value);
        }

        public virtual void SetValueForDetailPage(IDrisionControl c, object value)
        {
            c.SetValue(value);
        }

        public Control LoadControl()
        {
            Control c = CreateControl();
            c.ID = this.ID;
            (c as IDrisionControl).FieldName = this.FieldName;
            (c as IDrisionControl).SetValue(this.DefaultValue); //默认值
            return c;
        }

        public Control LoadControlForDetailPage()
        {
            Control c = CreateControlForDetailPage();
            c.ID = this.ID;
            (c as IDrisionControl).FieldName = this.FieldName;            
            return c;
        }
    }

    public class plabel_ControlHelper : DataTypeControlHelperBase
    {
        public plabel_ControlHelper(SysFormField ff) : base(ff) { }        
    }

    public class pint_ControlHelper : DataTypeControlHelperBase
    {
        public pint_ControlHelper(SysFormField ff) : base(ff) { }

        protected override Control CreateControl()
        {
            TextControl c = new TextControl();
            c.TextType = TextControl.TextTypes.Int32;
            c.IsRequired = this.IsRequired;
            c.MaxValue = this.MaxValue;
            c.MinValue = this.MinValue;
            return c;
        }
    }

    public class pstring_ControlHelper : DataTypeControlHelperBase
    {
        public pstring_ControlHelper(SysFormField ff) : base(ff) { }

        protected override Control CreateControl()
        {
            TextControl c = new TextControl();
            c.TextType = TextControl.TextTypes.String;
            c.IsRequired = this.IsRequired;
            c.MaxLength = this.MaxLength;
            c.MinLength = this.MinLength;
            return c;
        }
    }

    public class pdatetime_ControlHelper : DataTypeControlHelperBase
    {
        public pdatetime_ControlHelper(SysFormField ff) : base(ff) { }

        protected override Control CreateControl()
        {
            DateTimeControl c = new DateTimeControl();
            c.IsRequired = this.IsRequired;
            c.ShowType = DateTimeControl.ShowTypes.Date;
            return c;
        }

        public override void SetValueForDetailPage(IDrisionControl c, object value)
        {
            if (value != null)
            {
                c.SetValue(string.Format("{0:yyyy-MM-dd}", value));
            }
        }
    }    

    public class pdecimal_ControlHelper : DataTypeControlHelperBase
    {
        public pdecimal_ControlHelper(SysFormField ff) : base(ff) { }

        protected override Control CreateControl()
        {
            TextControl c = new TextControl();
            c.TextType = TextControl.TextTypes.Decimal;
            c.IsRequired = this.IsRequired;
            c.MaxValue = this.MaxValue;
            c.MinValue = this.MinValue;
            return c;
        }
    }

    public class ptext_ControlHelper : DataTypeControlHelperBase
    {
        public ptext_ControlHelper(SysFormField ff) : base(ff) { }

        protected override Control CreateControl()
        {
            TextControl c = new TextControl();
            c.TextType = TextControl.TextTypes.Text;
            c.Height = 100;
            c.IsRequired = this.IsRequired;
            c.MaxLength = this.MaxLength;
            c.MinLength = this.MinLength;
            return c;
        }

        public override void SetValueForDetailPage(IDrisionControl c, object value)
        {
            c.SetValue(HttpUtility.HtmlEncode(value).Replace("\r\n", "<br />").Replace("\n", "<br />"));
        }
    }

    public class plong_ControlHelper : DataTypeControlHelperBase
    {
        public plong_ControlHelper(SysFormField ff) : base(ff) { }

        protected override Control CreateControl()
        {
            TextControl c = new TextControl();
            c.TextType = TextControl.TextTypes.Int64;
            c.IsRequired = this.IsRequired;
            c.MaxValue = this.MaxValue;
            c.MinValue = this.MinValue;
            return c;
        }
    }

    public class pfile_ControlHelper : DataTypeControlHelperBase
    {
        public pfile_ControlHelper(SysFormField ff) : base(ff) { }

        protected override Control CreateControl()
        {
            Uploader c = new Uploader();
            c.ShowImage = false;
            c.AutoShowImage = false;
            return c;
        }

        protected override Control CreateControlForDetailPage()
        {
            Attachment c = new Attachment();
            c.ShowImage = false;
            c.AutoShowImage = false;
            return c;
        }
    }

    public class pbool_ControlHelper : DataTypeControlHelperBase
    {
        public pbool_ControlHelper(SysFormField ff) : base(ff) { }

        protected override Control CreateControl()
        {
            CheckControl c = new CheckControl();            
            return c;
        }

        public override void SetValueForDetailPage(IDrisionControl c, object value)
        {
            if (value != null)
            {
                string str = value.ToString().ToLower();
                if (str == "1" || str == "true")
                {
                    c.SetValue("是");
                }
                else
                {
                    c.SetValue("否");
                }
            }
            else
            {
                c.SetValue("否");
            }
        }
    }

    public class pref_ControlHelper : DataTypeControlHelperBase
    {
        public pref_ControlHelper(SysFormField ff) : base(ff) { }

        protected override Control CreateControl()
        {
            ComboControl c = new ComboControl();
            c.DropdownType = ComboControl.DropdownTypes.DropdownList;
            c.IsRequired = this.IsRequired;
            c.ShowEmptyItem = true;
            c.AutoSelectFirst = true;

            BindData(c);

            return c;
        }

        private DataTable GetData()
        {
            long entityId = this.Field.RefRelation.ParentEntityId.Value; //这里和元网站一致，不要用this.Field.RefEntityId

            var es = IEntitySchemaHelper.Get(entityId);

            var displayFieldName = es.DisplayName; //entity.GetDisplayFieldName();
            var keyFieldName = es.KeyName;// entity.GetKeyFieldName();

            string sql = string.Format("select {0} as ID, {1} as NAME  from {2} ", keyFieldName, displayFieldName, es.EntityName);

            DataTable data;
            using (BizDataContext db = new BizDataContext())
            {
                data = db.ExecuteDataTable(sql);
            }
            return data;
        }

        private void BindData(ComboControl c)
        {
            DataTable data = GetData();
            var source = data.Rows.Cast<DataRow>().Select(p => new
            {
                NAME = Convert.ToString(p["NAME"]),
                ID = Convert.ToString(p["ID"]),
            }).ToList();

            c.DataSource = source;// data;
            c.DataTextField = "NAME";
            c.DataValueField = "ID";
            c.DataBind();
        }

        public override void SetValueForDetailPage(IDrisionControl c, object value)
        {
            DataTable data = GetData();
            var source = data.Rows.Cast<DataRow>().Select(p => new
            {
                NAME = Convert.ToString(p["NAME"]),
                ID = Convert.ToString(p["ID"]),
            }).ToList();

            foreach (var p in source)
            {
                if (p.ID == Convert.ToString(value))
                {
                    c.SetValue(p.NAME);
                    break;
                }
            }
        }
    }

    public class penum_ControlHelper : DataTypeControlHelperBase
    {
        public penum_ControlHelper(SysFormField ff) : base(ff) { }

        protected override Control CreateControl()
        {
            ComboControl c = new ComboControl();            
            c.DropdownType = ComboControl.DropdownTypes.DropdownList;
            c.IsRequired = this.IsRequired;
            c.ShowEmptyItem = true;
            c.AutoSelectFirst = true;

            BindData(c);

            return c;
        }

        private void BindData(ComboControl c)
        {
            foreach (var p in this.Field.RefEnum.EnumItems)
            {
                c.Items.Add(new ComboItem()
                {
                    Text = p.DisplayText,
                    Value = Convert.ToString(p.ItemValue),
                });
            }
        }

        public override void SetValueForDetailPage(IDrisionControl c, object value)
        {
            foreach (var p in this.Field.RefEnum.EnumItems)
            {
                if (p.ItemValue == value.ToInt())
                {
                    c.SetValue(p.DisplayText);
                    break;
                }
            }
        }
    }

    public class ppassword_ControlHelper : DataTypeControlHelperBase
    {
        public ppassword_ControlHelper(SysFormField ff) : base(ff) { }

        protected override Control CreateControl()
        {
            TextControl c = new TextControl();
            c.TextType = TextControl.TextTypes.Password;
            c.Height = 100;
            c.IsRequired = this.IsRequired;
            c.MaxLength = this.MaxLength;
            c.MinLength = this.MinLength;
            return c;
        }

        public override void SetValueForDetailPage(IDrisionControl c, object value)
        {
            c.SetValue("*********");
        }
    }
}