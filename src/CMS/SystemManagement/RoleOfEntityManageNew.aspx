<%@ Page Title="实体权限快捷配置" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RoleOfEntityManageNew.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.RoleOfEntityManageNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="out_border">
        <div class="main_box">
            <h3>
                实体权限</h3>
            <div class="allcol2">
                <div class="item_box_full">
                    <span>实体</span> <span class="left_star">*</span>
                    <asp:SelectControl ID="scEntity" runat="server" IsRequired="true" SelectType="List">
                         <ListSettings DataTextField="EntityName" DataValueField="EntityId" SingleSelect="true" IsGrouped="true" DataGroupField="CategoryName"></ListSettings>
                         <PopupSettings Title="请选择实体" />
                         <ClientSideEvents OnSelectChanged="function(s,e){
                            ccPrivilege.callBack(e.Value);
                         }" />
                    </asp:SelectControl>
                </div>
                <div class="item_box_full">
                    <span>角色</span> <span class="left_star">*</span>
                    <asp:SelectControl ID="scRole" runat="server" IsRequired="true" SelectType="List">
                        <ListSettings DataTextField="Role_Name" DataValueField="Role_ID"></ListSettings>
                        <PopupSettings Title="请选择角色" Height="300" />
                    </asp:SelectControl>
                </div>
                <div class="item_box_full">
                    <span>操作</span> <span class="left_star">*</span>
                    <asp:SelectControl ID="scOperation" runat="server" IsRequired="true" SelectType="List">
                        <ListSettings DataTextField="Description" DataValueField="Value"></ListSettings>
                        <PopupSettings Title="请选择操作" Height="200" />
                    </asp:SelectControl>
                </div>
                <div class="item_box_full">
                    <span>权限</span> <span class="left_star">*</span>
                    <asp:ComboControl ID="ccPrivilege" runat="server" IsRequired="true" ClientInstanceName="ccPrivilege" OnCustomCallBack="ccPrivilege_CustomCallBack"></asp:ComboControl>
                </div>                
            </div>
            <div class="cl">
            </div>
        </div>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click"/>        
    </div>
</asp:Content>
