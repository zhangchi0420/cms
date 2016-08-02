using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Drision.Framework.Manager;
using System.Data.Common;
using Tension;
using Drision.Framework.Enum;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using System.Linq.Expressions;
using Drision.Framework.Repository.EF;
using Drision.Framework.Common;
using Drision.Framework.Entity;
using Drision.Framework.OrgLibrary;

namespace Drision.Framework.Web.SystemManagement
{
    public partial class ConfigurationAdd : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string mode = Request.QueryString["mode"];
            if (!IsPostBack)
            {
                if (mode != "add")
                {
                    string strId = Request.QueryString["id"];
                    if (!string.IsNullOrEmpty(strId))
                    {
                        BindData(Convert.ToInt32(strId));
                    }

                    if (mode == "detail")
                    {
                        DisableControl();
                    }
                }
            }
            if (mode == "add")
            {
                btnSave.Click += new EventHandler(Add);
            }
            if (mode == "edit")
            {
                btnSave.Click += new EventHandler(Edit);
            }
        }
        private void Edit(object sender, EventArgs e)
        {

            try
            {
                string strId = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(strId))
                {
                    int id = Convert.ToInt32(strId);
                    //using (BizDataContext context = new BizDataContext())
                    {
                        var data = DataHelper.FirstOrDefault<T_Configuration>(c =>
                                    c.Configuration_Id == id);
                        if (data == null)
                        {
                            this.AjaxAlert(string.Format("数据不存在，请确认id[{0}]是否正确！", id), "EnableButton();");


                            //throw new ApplicationException(string.Format("数据不存在，请确认id[{0}]是否正确！",id));
                        }
                        else
                        {
                            if (CheckKeyExist(tbT_Configuration_Key.Text.Trim(), id))
                            {
                                this.AjaxAlert("配置键已经存在！", "EnableButton();");


                                //throw new ApplicationException("配置键已经存在！");
                            }
                            else
                            {
                                data.Configuration_Description = tbT_Configuration_Description.Text.Trim();
                                data.Configuration_Title = tbT_Configuration_Title.Text.Trim();
                                data.Configuration_Value = tbT_Configuration_Value.Text.Trim();
                                data.Configuration_Key = tbT_Configuration_Key.Text.Trim();
                                data.Last_Modified = DateTime.Now;
                                data.Date_Added = DateTime.Now;
                                
                                DataHelper.Update(data);
                                this.AjaxAlert("保存成功！", "EnableButton();");
                                Response.Redirect("~/SystemManagement/ConfigurationQuery.aspx", false);
                            }
                        }
                    }
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message, "EnableButton();");
            }

        }
        private void Add(object sender, EventArgs e)
        {
            try
            {
                // using (BizDataContext context = new BizDataContext())
                {
                    if (CheckKeyExist(tbT_Configuration_Key.Text.Trim()))
                    {
                        throw new ApplicationException("配置键已经存在！");
                    }
                    T_Configuration data = new T_Configuration();
                    data.Configuration_Description = tbT_Configuration_Description.Text.Trim();
                    data.Configuration_Title = tbT_Configuration_Title.Text.Trim();
                    data.Configuration_Value = tbT_Configuration_Value.Text.Trim();
                    data.Configuration_Key = tbT_Configuration_Key.Text.Trim();
                    data.Last_Modified = DateTime.Now;
                    data.Date_Added = DateTime.Now;
                    data.Configuration_Group_Id = 0;
                    data.Sort_Order = CaculateSortOrder();
                    data.Configuration_Id = DataHelper.GetNextIdentity_Int();
                    DataHelper.Insert(data);                    
                    this.AjaxAlert("保存成功！", "EnableButton();");
                    Response.Redirect("~/SystemManagement/ConfigurationQuery.aspx", false);
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message, "EnableButton();");
            }
        }
        private bool CheckKeyExist(string key, int id = 0)
        {
            // using (BizDataContext context = new BizDataContext())
            {
                if (id == 0)
                {
                    return DataHelper.FirstOrDefault<T_Configuration>(p => p.Configuration_Key == key) != null;
                }
                return DataHelper.FirstOrDefault<T_Configuration>(p => p.Configuration_Key == key && p.Configuration_Id != id) != null;
            }
        }
        private int CaculateSortOrder()
        {
            // using (BizDataContext context = new BizDataContext())
            {
                var order = DataHelper.FetchAll<T_Configuration>().Select(p => p.Sort_Order);
                if (order.Any())
                {
                    return order.Max();
                }
                return 0;
            }
        }        

        private void DisableControl()
        {
            tbT_Configuration_Description.Enabled =
                tbT_Configuration_Key.Enabled =
                tbT_Configuration_Title.Enabled = tbT_Configuration_Value.Enabled = btnSave.Visible = false;
        }

        private void BindData(int id)
        {
            try
            {
                // using (BizDataContext context = new BizDataContext())
                {
                    var data = DataHelper.FindById<T_Configuration>(id);
                    if (data == null)
                    {
                        throw new ApplicationException(string.Format("数据不存在，请确认id[{0}]是否正确！", id));
                    }
                    tbT_Configuration_Description.Text = data.Configuration_Description;
                    tbT_Configuration_Key.Text = data.Configuration_Key;
                    tbT_Configuration_Title.Text = data.Configuration_Title;
                    tbT_Configuration_Value.Text = data.Configuration_Value;
                }
            }
            catch (ApplicationException ex)
            {
                this.AjaxAlert(ex.Message, "EnableButton();");
            }
        }
    }
}