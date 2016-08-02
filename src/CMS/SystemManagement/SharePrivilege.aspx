<%@ Page Title="权限共享" Language="C#" MasterPageFile="~/Iframe.Master" AutoEventWireup="true" CodeBehind="SharePrivilege.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.SharePrivilege" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
        <h3>
            权限共享</h3>
        <div class="allcol1">
            <div class="item_box_col1">
                <span>共享方式 </span>
                <asp:ComboControl ID="ccShareType" runat="server" AutoPostBack = "true" OnTextChanged = "ccShareType_OnTextChanged"/>
            </div>
            <div class="item_box_col1" runat = "server" id = "divShareRoleId" visible = "false">
                <span>共享的角色 </span>
                <asp:ComboControl ID="ccShareRoleId" runat="server" ShowEmptyItem = "true" EmptyItemText = "--请选择--" AutoSelectFirst = "true"/>
            </div>
            <div class="item_box_col1" runat = "server" id = "divShareUserId" visible = "false">
                <span>共享的人员 </span>
                <asp:ComboControl ID="ccShareUserId" runat="server" ShowEmptyItem = "true" EmptyItemText = "--请选择--" AutoSelectFirst = "true"/>
            </div>
            <div class="item_box_col1">
                <span>共享的权限 </span>
                <asp:ComboControl ID="ccPrivilege" runat="server"/>
            </div>
            <div class="cl">
            </div>
        </div>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="lbtnSave" runat="server" OnClick="lbtnSave_Click">保存</asp:LinkButton>
    </div>
    <div class="grid_title" id="divViewControlTitle">
        此条数据共享列表</div>
    <div id="divviewcontrol" runat="server">
        <asp:GridControl ID="gcProcessInstance" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:HeaderSortField ShowHeader="false" DataTextField="ShareRoleId" HeaderText="共享角色">
                </asp:HeaderSortField>
                <asp:HeaderSortField ShowHeader="false" DataTextField="ShareUserId" HeaderText="共享人员">
                </asp:HeaderSortField>
                <asp:HeaderSortField ShowHeader="false" DataTextField="Privilege" HeaderText="共享权限">
                </asp:HeaderSortField>
                <asp:HeaderSortField ShowHeader="false" DataTextField="CreateTime" HeaderText="共享时间" DataFormatString = "{0:yyyy-MM-dd}">
                </asp:HeaderSortField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnDelete" runat="server" Text="删除" CommandArgument='<%#Eval("Id")%>' OnClick = "lbtnDelete_Click"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridControl>
    </div>
</asp:Content>
