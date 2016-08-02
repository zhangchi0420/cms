<%@ Page Title="我的共享权限" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SharePrivilegeQuery.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.SharePrivilegeQuery" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
<h3>查询条件</h3>
    <div class="allcol2">
        <div class="item_box_col1">
            <span>创建时间从 </span><span class = "left_star">&nbsp;</span>
            <asp:DateTimeControl ID = "dtStartTime1" ClientInstanceName = "dtStartTime1"   runat="server"  />
        </div>
        <div class="item_box_col1">
            <span>到 </span><span class = "left_star">&nbsp;</span>
            <asp:DateTimeControl ID = "dtStartTime2" ClientInstanceName = "dtStartTime2"  runat="server" />
        </div>
        <div class="item_box_col1">
            <span>权限 </span><span class = "left_star">&nbsp;</span>
                <asp:ComboControl ID="ccPrivilege" runat="server"/>
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
        共享信息</div>
    <div id="divviewcontrol" runat="server">
        <asp:GridControl OnPageIndexChanging="grid_PageChanging" ID="gcProcessInstance" ClientInstanceName="gcProcessInstance" runat="server" AutoGenerateColumns="false" OnRowDataBound = "gcProcessInstance_RowDataBound" KeyFieldName="Id">
        <Columns>
            <asp:HeaderSortField ShowHeader="false" DataTextField="EntityName" HeaderText="实体名称" > </asp:HeaderSortField>
            <asp:HeaderSortField ShowHeader="false" DataTextField="ObjectId" HeaderText="对象" DisplayLength="10" HorizontalAlign="Left"></asp:HeaderSortField>
            <asp:HeaderSortField ShowHeader="false" DataTextField="ShareUserId" HeaderText="共享人员" > </asp:HeaderSortField>
            <asp:HeaderSortField ShowHeader="false" DataTextField="ShareRoleId" HeaderText="共享角色" > </asp:HeaderSortField>
            <asp:HeaderSortField ShowHeader="false" DataTextField="Privilege" HeaderText="权限" > </asp:HeaderSortField>
            <asp:HeaderSortField ShowHeader="false" DataTextField="CreateTime" HeaderText="创建时间" DataFormatString="{0:yyyy-MM-dd}" > </asp:HeaderSortField>
            <asp:TemplateField HeaderText="操作" ShowHeader="False">
            <ItemTemplate>
                 <asp:LinkButton ID="btnCancel"  runat="server" Text="取消共享" CommandArgument='<%#Eval("Id") %>'  OnClientClick="if (!confirm('确定要取消共享？')) {return false;}"  OnClick="btnCancel_Click"></asp:LinkButton>
            </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
        </PagerSettings>
        </asp:GridControl>
    </div>
</asp:Content>
