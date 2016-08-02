<%@ Page Title="通讯簿" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddressBook.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.AddressBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
        <h3>
            用户查询</h3>
        <div class="allcol2">
            <div class="item_box_col1">
                <span>用户名称 </span><span class="left_star">&nbsp;</span>
                <asp:TextControl ID="txtUserName" runat="server" MaxLength="100" />
            </div>
            <div class="item_box_col1">
                <span>所属部门 </span><span class="left_star">&nbsp;</span>
                <asp:ComboControl DataTextField="Department_Name" DataValueField="Department_ID" ID="ddlDepartment" runat="server" DropdownType="DropdownList" ShowEmptyItem="true" AutoSelectFirst="true">
                </asp:ComboControl>
            </div>
            <div class="cl">
            </div>
        </div>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查询" />
        <asp:LinkButton ID="btnClearCondition" runat="server" OnClick="btnClearCondition_Click"
            Text="重置" />        
    </div>
    <div class="grid_title" id="divViewControlTitle">
        用户列表</div>
    <div id="divviewcontrol">
        <asp:GridControl ID="gcUser" runat="server" OnPageIndexChanging="gcUser_PageIndexChanging" OnHeaderClick="gcUser_HeaderClick">
            <Columns>
                <asp:HeaderSortField HeaderText="用户名称" DataTextField="User_Name">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="所属部门" DataTextField="Department_Name">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="工号" DataTextField="User_Code">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="手机" DataTextField="User_Mobile">
                </asp:HeaderSortField>
                <asp:HeaderSortField HeaderText="邮箱" DataTextField="User_Email" ItemType="Link" OnClientClick='MailTo(this);'>
                </asp:HeaderSortField>
            </Columns>
            <PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
            </PagerSettings>
        </asp:GridControl>
    </div>
    <script type="text/javascript">
        function MailTo(obj) {
            var mail = $(obj).text();
            window.location.href = "mailto:" + mail;
        }
    </script>
</asp:Content>
