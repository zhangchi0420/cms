<%@ Page Title="配置查询" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ConfigurationQuery.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.ConfigurationQuery" %>

<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            查询条件</h3>
        <div class="allcol2">
            <div class="item_box_col1">
                <span>配置标题 </span>
                <asp:TextControl ID="tbT_Configuration_Title" runat="server" CssClass="item_input"
                    TextType="String" FieldName="Configuration_Title" />
            </div>
            <div class="item_box_col1">
                <span>配置键 </span>
                <asp:TextControl ID="tbT_Configuration_Key" runat="server" CssClass="item_input"
                    TextType="String" FieldName="Configuration_Key" />
            </div>
            <div class="cl">
            </div>
        </div>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="lbtnQuery" runat="server" OnClick="btnQuery_Click">查询</asp:LinkButton>
        <asp:LinkButton ID="lbtnClearCondition" runat="server" OnClick="btnClearCondition_Click">重置</asp:LinkButton>
    </div>
    <div class="grid_title" id="divViewControlTitle">
        配置信息</div>
    <div id="divviewcontrol" runat="server">
        <asp:GridControl OnPageIndexChanging="grid_PageChanging" ID="gcConfiguration" ClientInstanceName="gcConfiguration" runat="server"
            AutoGenerateColumns="false">
            <Columns>
                <asp:HeaderSortField ShowHeader="false" DataTextField="Configuration_Title" HeaderText="配置标题">
                </asp:HeaderSortField>
                <asp:HeaderSortField ShowHeader="false" DataTextField="Configuration_Key" HeaderText="配置键"
                    DisplayLength="10">
                </asp:HeaderSortField>
                <asp:HeaderSortField ShowHeader="false" DataTextField="Configuration_Value" HeaderText="配置值">
                </asp:HeaderSortField>
                <%--<asp:HeaderSortField ShowHeader="false" DataTextField="Configuration_Description"
                    HeaderText="配置描述">
                </asp:HeaderSortField>--%>
                <asp:HeaderSortField ShowHeader="false" DataTextField="Last_Modified" HeaderText="修改时间">
                </asp:HeaderSortField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" Text="详情" PostBackUrl='<%#"ConfigurationAdd.aspx?mode=detail&id=" + Eval("Configuration_Id")%>'
                            ></asp:LinkButton>
                        <asp:LinkButton ID="lbtnDetail" runat="server" Text="编辑" PostBackUrl='<%#"ConfigurationAdd.aspx?mode=edit&id=" + Eval("Configuration_Id")%>'
                            ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
            </PagerSettings>
        </asp:GridControl>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="lbtnAdd" runat="server" PostBackUrl="ConfigurationAdd.aspx?mode=add">新增</asp:LinkButton>
    </div>
</asp:Content>
