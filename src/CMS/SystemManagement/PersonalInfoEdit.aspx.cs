using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Drision.Framework.Manager;
using Drision.Framework.Entity;
using Drision.Framework.Enum;
using Drision.Framework.Common;
using Drision.Framework.Web.Common;
using Drision.Framework.Repository.EF;
using Drision.Framework.WorkflowEngine;
using Drision.Framework.WebControls.Super;
using Drision.Framework.Plugin.Web;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class PersonalInfoEdit : BasePage
    {
        public override long PageID
        {
            get
            {
                return 1000000120;
            }
        }
        public override string PageName
        {
            get
            {
                return "UserEdit";
            }
        }
        private int __Id
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
        //页面定义的参数

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (Request.QueryString["id"] == null)
                    {
                        //转到错误页
                        Response.Redirect("~/Error.aspx");
                    }
                    //赋值定义的参数

                    //绑定启动流程下拉选择框

                    //绑定枚举 
                    ctrl_User_Status.DataSource = typeof(Drision.Framework.Enum.EffectiveFlagEnum);
                    ctrl_User_Status.DataBind();
                    ctrl_Is_Prohibit_Web.DataSource = typeof(Drision.Framework.Enum.ForbiddenWebFlagEmum);
                    ctrl_Is_Prohibit_Web.DataBind();
                    ctrl_Is_Prohibit_Mobile.DataSource = typeof(Drision.Framework.Enum.ForbiddenMobileFlagEmum);
                    ctrl_Is_Prohibit_Mobile.DataBind();
                    txt_User_TypeJW.DataSource = typeof(Drision.Framework.Enum.User_User_TypeEnum);
                    txt_User_TypeJW.DataBind();

                    //绑定引用字段下拉 

                    using (DrisionDbContext context = new DrisionDbContext(GlobalObject.ConnString))
                    {
                        Tension.DataPage<dynamic> dataPage = new Tension.DataPage<dynamic>(1, int.MaxValue);
                        
                        dynamic result1 = null;
                        result1 = context.T_Department.ToList();

                        foreach (var m in result1)
                        {
                            var p = new DataWrapper(m);
                            ctrl_Department_ID.Items.Add(new ComboItem()
                            {
                                Text = p["Department_Name"],
                                Value = p["Department_ID"],
                            });
                        }

                    }

                    //查询并加载数据
                    __Id = Convert.ToInt32(Request.QueryString["id"]);
                    this.hc.Text = this.ClientScript.GetPostBackEventReference(this.hc, "refresh");
                    SetTabSelectedIndex();
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitData();
            }
        }
        /// <summary>
        /// 重载BasePage的刷新方法
        /// </summary>
        public override void Refresh()
        {
            InitData();
        }

        private void InitData()
        {
            Manager.GenericManager<T_User> Manager = new GenericManager<T_User>();
            var entityModel = Manager.FindById(__Id, true);

            try
            {
                ctrl_Department_ID.SetValue(entityModel.Department_ID);
            }
            catch (Exception) { }


            try
            {
                ctrl_User_Code.SetValue(entityModel.User_Code);
            }
            catch (Exception) { }


            try
            {
                ctrl_User_Name.SetValue(entityModel.User_Name);
            }
            catch (Exception) { }


            try
            {
                ctrl_EntryDate.SetValue(entityModel.EntryDate);
            }
            catch (Exception) { }


            try
            {
                ctrl_Card_No.SetValue(entityModel.Card_No);
            }
            catch (Exception) { }


            try
            {
                ctrl_User_Mobile.SetValue(entityModel.User_Mobile);
            }
            catch (Exception) { }


            try
            {
                ctrl_User_EMail.SetValue(entityModel.User_EMail);
            }
            catch (Exception) { }


            try
            {
                ctrl_User_Status.SetValue(entityModel.User_Status);
            }
            catch (Exception) { }


            try
            {
                ctrl_Is_Prohibit_Web.SetValue(entityModel.Is_Prohibit_Web);
            }
            catch (Exception) { }


            try
            {
                ctrl_Is_Prohibit_Mobile.SetValue(entityModel.Is_Prohibit_Mobile);
            }
            catch (Exception) { }


            try
            {
                ctrl_User_Comment.SetValue(entityModel.User_Comment);
            }
            catch (Exception) { }


            try
            {
                txt_User_TypeJW.SetValue(entityModel.User_Type);
            }
            catch (Exception) { }




            //设置字段的隐藏可用属性

            //设置按钮的隐藏可用属性

        }


        /// <summary>
        /// 保存按钮添加验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ctrl_useredit_op_save_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ctrl_useredit_op_save.Attributes.Add("onclick", "return Validate();");
            }
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string __RedirectURL = string.Empty;
                {
                    GenericManager<T_User> Manager = new GenericManager<T_User>();
                    List<string> FieldList = new List<string>();
                    var model = new T_User();
                    CreateModel(FieldList, model, Manager);

                    PluginEventArgs __PluginEventArg = new PluginEventArgs(model, FieldList, __RedirectURL);
                    //旧插件按钮前的操作和判断跳转
                    PagePluginFactory.InvokeBeforePlugins(sender, __PluginEventArg);
                    if (!string.IsNullOrEmpty(__PluginEventArg.Params[2].ToString()))
                    {
                        Response.Redirect(__PluginEventArg.Params[2].ToString(), false);
                    }


                    Manager.Update(model, FieldList);

                    //旧插件按钮后的操作和判断跳转
                    PagePluginFactory.InvokeAfterPlugins(sender, __PluginEventArg);
                    if (!string.IsNullOrEmpty(__PluginEventArg.Params[2].ToString()))
                    {
                        Response.Redirect(__PluginEventArg.Params[2].ToString(), false);
                    }



                    __Id = model.User_ID;

                    this.AjaxAlert("保存成功", "EnableButton();");
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex, "EnableButton();");
            }
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            string homePage = "~/Home/HomePage.aspx";
            Response.Redirect(homePage);
        }
        /// <summary>
        /// 重置密码(123)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn2H_Click(object sender, EventArgs e)
        {
            try
            {
                string __RedirectURL = string.Empty;
                {
                    GenericManager<T_User> Manager = new GenericManager<T_User>();
                    var model = Manager.FindById(Request.QueryString["id"] ?? "-1");
                    PluginEventArgs __PluginEventArg = new PluginEventArgs(model, __RedirectURL);

                    //旧插件按钮前的操作和判断跳转
                    PagePluginFactory.InvokeBeforePlugins(sender, __PluginEventArg);
                    if (!string.IsNullOrEmpty(__PluginEventArg.Params[1].ToString()))
                    {
                        Response.Redirect(__PluginEventArg.Params[1].ToString(), false);
                    }


                    //旧插件按钮后的操作和判断跳转
                    PagePluginFactory.InvokeAfterPlugins(sender, __PluginEventArg);
                    if (!string.IsNullOrEmpty(__PluginEventArg.Params[1].ToString()))
                    {
                        Response.Redirect(__PluginEventArg.Params[1].ToString(), false);
                    }
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex, "EnableButton();");
            }
        }


        private void CreateModel(List<string> FieldList, T_User model, GenericManager<T_User> Manager)
        {


            model.User_ID = __Id;//Convert.ToInt32(Request.QueryString["id"]);

            FieldList.Add("Department_ID");
            Manager.Set(model, "Department_ID", ctrl_Department_ID.GetValue());


            FieldList.Add("User_Code");
            Manager.Set(model, "User_Code", ctrl_User_Code.GetValue());


            FieldList.Add("User_Name");
            Manager.Set(model, "User_Name", ctrl_User_Name.GetValue());


            FieldList.Add("EntryDate");
            Manager.Set(model, "EntryDate", ctrl_EntryDate.GetValue());


            FieldList.Add("Card_No");
            Manager.Set(model, "Card_No", ctrl_Card_No.GetValue());


            FieldList.Add("User_Mobile");
            Manager.Set(model, "User_Mobile", ctrl_User_Mobile.GetValue());


            FieldList.Add("User_EMail");
            Manager.Set(model, "User_EMail", ctrl_User_EMail.GetValue());


            FieldList.Add("User_Status");
            Manager.Set(model, "User_Status", ctrl_User_Status.GetValue());


            FieldList.Add("Is_Prohibit_Web");
            Manager.Set(model, "Is_Prohibit_Web", ctrl_Is_Prohibit_Web.GetValue());


            FieldList.Add("Is_Prohibit_Mobile");
            Manager.Set(model, "Is_Prohibit_Mobile", ctrl_Is_Prohibit_Mobile.GetValue());


            FieldList.Add("User_Comment");
            Manager.Set(model, "User_Comment", ctrl_User_Comment.GetValue());


            FieldList.Add("User_Type");
            Manager.Set(model, "User_Type", txt_User_TypeJW.GetValue());


        }


        /// <summary>
        /// 供初始化时设置选中的标签
        /// </summary>
        private void SetTabSelectedIndex()
        {
        }
    }
}