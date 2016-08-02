<%@ Page Title="选择人员" Language="C#" MasterPageFile="~/Select.Master" AutoEventWireup="true"
    CodeBehind="UserSelect.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.UserSelect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            查询条件</h3>
        <div class="allcol2">
            <div class="item_box_col1">
                <span>帐号 </span><span class="left_star">&nbsp;</span>
                <sp:SText ID="txtUserCode" runat="server" TextType="String" FieldName="User_Code"
                    Tag="8" ControlId="1000000253">
                </sp:SText>
            </div>
            <div class="item_box_col1">
                <span>姓名 </span><span class="left_star">&nbsp;</span>
                <sp:SText ID="txtUserName" runat="server" TextType="String" FieldName="User_Name"
                    Tag="8" ControlId="1000000254">
                </sp:SText>
            </div>
            <div class="item_box_col1">
                <span>部门 </span><span class="left_star">&nbsp;</span>
                <sp:Dropdown ID="txtDeptId" runat="server"
                    FieldName="Department_ID" Tag="3" ControlId="1000000252">
                </sp:Dropdown>
            </div>
        </div>
        <div class="cl">
        </div>
    </div>
    <div class="button_bar">
        <asp:LinkButton ID="btnQuery" runat="server" OnClick="btnQuery_Click"
            Text="查询" CssClass="btnQuery" />
        <asp:LinkButton ID="btnClearCondition" runat="server" OnClick="btnClearCondition_Click"
            Text="重置" />
    </div>
    <div class="grid_title" id="divViewControlTitle">
        选择用户
    </div>
    <div id="divctrl_userselect_view" runat="server">
        <asp:GridControl ID="gridUser" ClientInstanceName="PopUpValueText" SingleSelect="true" runat="server" AutoGenerateColumns="false"
         OnPageIndexChanging="gridUser_PageIndexChanging">
            <Columns>
                <asp:FullCheckField DataTextField="User_Name" DataValueField="User_ID">
                </asp:FullCheckField>
                <asp:HeaderSortField DataTextField="User_Code" HeaderText="工号">
                </asp:HeaderSortField>
                <asp:HeaderSortField DataTextField="User_Name" HeaderText="用户姓名">
                </asp:HeaderSortField>
                <asp:HeaderSortField DataTextField="Department_ID_V" HeaderText="所属部门">
                </asp:HeaderSortField>
                <asp:HeaderSortField DataTextField="User_Mobile" HeaderText="手机号码">
                </asp:HeaderSortField>
            </Columns>
            <PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
            </PagerSettings>
        </asp:GridControl>
    </div>
</asp:Content>
