using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Drision.Framework.Common;
using System.Data;

namespace Drision.Framework.Web.SystemManagement
{
    /// <summary>
    /// 权限管理控件 RightSource + GetReptBindItem()
    /// </summary>
    public partial class RoleOfEntityControl : UserControl
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public object RightSource
        {
            get { return ViewState["RightSource"]; }
            set { ViewState["RightSource"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //控件绑定事件
            RepeaterBindEvent();

            if (!IsPostBack)
            {
                this.TrRept.DataSource = RightSource;
                this.TrRept.DataBind();
            }

        }

        /// <summary>
        /// 刷新界面数据
        /// </summary>
        public void Reflush()
        {
            this.TrRept.DataSource = RightSource;
            this.TrRept.DataBind();
        }

        public List<ReptBindItem> GetReptBindItem()
        {
            var itemList = RightSource as List<ReptBindItem>;
            ReptBindItem rept = null;

            for (int i = 0; i < itemList.Count; i++)
            {
                rept = itemList[i];

                DropDownList ddlAdd = this.TrRept.Items[i].FindControl("ddlAdd") as DropDownList;
                DropDownList ddlDel = this.TrRept.Items[i].FindControl("ddlDel") as DropDownList;
                DropDownList ddlUpdt = this.TrRept.Items[i].FindControl("ddlUpdt") as DropDownList;
                DropDownList ddlQuery = this.TrRept.Items[i].FindControl("ddlQuery") as DropDownList;

                rept.addRight = Convert.ToInt32(ddlAdd.SelectedValue);
                rept.delRight = Convert.ToInt32(ddlDel.SelectedValue);
                rept.updtRight = Convert.ToInt32(ddlUpdt.SelectedValue);
                rept.queryRight = Convert.ToInt32(ddlQuery.SelectedValue);

            }
            return itemList;
        }

        #region 子方法

        /// <summary>
        /// 控件绑定事件
        /// </summary>
        private void RepeaterBindEvent()
        {
            this.TrRept.ItemDataBound += (rept, reptEvent) =>
            {
                ReptBindItem item = reptEvent.Item.DataItem as ReptBindItem;

                DropDownList ddlAdd = reptEvent.Item.FindControl("ddlAdd") as DropDownList;
                DropDownList ddlDel = reptEvent.Item.FindControl("ddlDel") as DropDownList;
                DropDownList ddlUpdt = reptEvent.Item.FindControl("ddlUpdt") as DropDownList;
                DropDownList ddlQuery = reptEvent.Item.FindControl("ddlQuery") as DropDownList;

                ddlAdd.DataBind(SelectSource(item.PrivilegeMode), item.addRight.ToString());
                ddlDel.DataBind(SelectSource(item.PrivilegeMode), item.delRight.ToString());
                ddlUpdt.DataBind(SelectSource(item.PrivilegeMode), item.updtRight.ToString());
                ddlQuery.DataBind(SelectSource(item.PrivilegeMode), item.queryRight.ToString());

            };

        }

        /// <summary>
        /// 下拉菜单值
        /// </summary>
        /// <param name="RightLevel">权限等级</param>
        private List<HtmlSelectSource> SelectSource(int RightLevel)
        {
            var selectSource = new List<HtmlSelectSource>();

            selectSource.Add(new HtmlSelectSource() { Text = "全部权限", Value = "5" });
            if (RightLevel == (int)PrivilegeModel.Persional) //判断权限等级 如果是个人级别则显示
            {
                selectSource.AddRange(new List<HtmlSelectSource>() {
                            new HtmlSelectSource(){ Text = "部门及子部门", Value = "4" },
                            new HtmlSelectSource(){ Text = "部门", Value = "3" },
                            new HtmlSelectSource(){ Text = "个人", Value = "2" }
                        });
            }
            selectSource.Add(new HtmlSelectSource() { Text = "无权限", Value = "1" });

            return selectSource;
        }
        #endregion

    }

    [Serializable]
    public class ReptBindItem
    {
        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public string DisplayText { get; set; }
        public string Description { get; set; }
        public int PrivilegeMode { get; set; }
        public int addRight { get; set; }
        public int delRight { get; set; }
        public int updtRight { get; set; }
        public int queryRight { get; set; }
    }

    #region 下拉菜单简写扩展方式

    /// <summary>
    /// 下拉菜单类
    /// </summary>
    [Serializable]
    public class HtmlSelectSource
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    /// <summary>
    /// 下拉菜单简写扩展方式
    /// </summary>
    public static class DropDownListExtender
    {
        /// <summary>
        /// DropDownList 绑定方法
        /// </summary>
        /// <param name="select">控件名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="value">值</param>
        /// <param name="text">说明</param>
        public static void DataBind(this DropDownList select, object dataSource)
        {
            select.DataSource = dataSource;
            select.DataValueField = "Value";
            select.DataTextField = "Text";
            select.DataBind();
        }

        /// <summary>
        /// DropDownList 绑定方法
        /// </summary>
        /// <param name="select">控件名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="value">值</param>
        /// <param name="text">说明</param>
        /// <param name="selectedIndex">选中项</param>
        public static void DataBind(this DropDownList select, object dataSource, string selectedIndex)
        {
            select.DataSource = dataSource;
            select.DataValueField = "Value";
            select.DataTextField = "Text";
            select.DataBind();
            select.SelectedValue = selectedIndex;
        }

    }

    #endregion

}