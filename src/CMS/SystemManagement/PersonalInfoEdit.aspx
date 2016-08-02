<%@ Page Title="个人信息修改" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PersonalInfoEdit.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.PersonalInfoEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
            基本信息</h3>
        <div class="allcol2">
            <div class="item_box_col1">
                <span>帐号<span style="color: Red">*</span></span>
                <asp:TextControl Enabled="false" ID="ctrl_User_Code" ClientInstanceName="ctrl_User_Code" runat="server"
                    TextType="String" FieldName="User_Code" IsRequired="true" />
            </div>
            <div class="item_box_col1">
                <span>姓名<span style="color: Red">*</span></span>
                <asp:TextControl ID="ctrl_User_Name" ClientInstanceName="ctrl_User_Name" runat="server"
                    TextType="String" FieldName="User_Name" IsRequired="true" />
            </div>
            <div class="item_box_col1">
                <span>入职日期&nbsp;</span>
                <asp:DateTimeControl Enabled="false" ID="ctrl_EntryDate" ClientInstanceName="ctrl_EntryDate" runat="server"
                    FieldName="EntryDate" ShowType="Date" />
            </div>
            <div class="item_box_col1">
                <span>身份证号&nbsp;</span>
                <asp:TextControl ID="ctrl_Card_No" ClientInstanceName="ctrl_Card_No" runat="server"
                    TextType="String" FieldName="Card_No" />
            </div>
            <div class="item_box_col1">
                <span>手机号码&nbsp;</span>
                <asp:TextControl ID="ctrl_User_Mobile" ClientInstanceName="ctrl_User_Mobile" runat="server"
                    TextType="String" FieldName="User_Mobile" />
            </div>
            <div class="item_box_col1">
                <span>电子邮件&nbsp;</span>
                <asp:TextControl ID="ctrl_User_EMail" ClientInstanceName="ctrl_User_EMail" runat="server"
                    TextType="String" FieldName="User_EMail" />
            </div>
            <div class="item_box_col1">
                <span>是否有效<span style="color: Red">*</span></span>
                <asp:ComboControl  Enabled="false" ID="ctrl_User_Status" ClientInstanceName="ctrl_User_Status" runat="server"
                    FieldName="User_Status" DropdownType="DropdownList" IsRequired="true" ShowEmptyItem="false"
                    AutoSelectFirst="true" />
            </div>
            <div style="display:none" class="item_box_col1">
                <span>是否禁用Web端&nbsp;</span>
                <asp:ComboControl ID="ctrl_Is_Prohibit_Web" ClientInstanceName="ctrl_Is_Prohibit_Web"
                    runat="server" FieldName="Is_Prohibit_Web" DropdownType="DropdownList" ShowEmptyItem="true"
                    AutoSelectFirst="true" />
            </div>
            <div style="display:none" class="item_box_col1">
                <span>是否禁用手机端&nbsp;</span>
                <asp:ComboControl ID="ctrl_Is_Prohibit_Mobile" ClientInstanceName="ctrl_Is_Prohibit_Mobile"
                    runat="server" FieldName="Is_Prohibit_Mobile" DropdownType="DropdownList" ShowEmptyItem="true"
                    AutoSelectFirst="true" />
            </div>
            <div class="item_box_col1">
                <span>所属部门&nbsp;</span>
                <sp:Dropdown Enabled="false" ID="ctrl_Department_ID" ClientInstanceName="ctrl_Department_ID" runat="server"
                    FieldName="Department_ID" ShowEmptyItem="true">
                </sp:Dropdown>
            </div>
            <div class="item_box_col1">
                <span>用户类别&nbsp;</span>
                <asp:ComboControl Enabled="false" ID="txt_User_TypeJW" ClientInstanceName="txt_User_TypeJW" runat="server"
                    FieldName="User_Type" DropdownType="DropdownList" ShowEmptyItem="true" AutoSelectFirst="true" />
            </div>
            <div class="item_box_full">
                <span>备注&nbsp;</span>
                <asp:TextControl ID="ctrl_User_Comment" ClientInstanceName="ctrl_User_Comment" runat="server"
                    TextType="Text" FieldName="User_Comment" />
            </div>
        </div>
        <div class="cl">
        </div>
    </div>
    <div class="button_bar">
        <asp:LinkButton Visible="false" ID="btn5K" runat="server" UseSubmitBehavior="false" Text="重置密码(123)"
            OnClick="btn2H_Click" OnClientClick="" />
        <asp:LinkButton ID="ctrl_useredit_op_save" runat="server" Text="保存" OnClick="btnSave_Click"
            OnClientClick="" OnPreRender="ctrl_useredit_op_save_PreRender" />
        <asp:LinkButton ID="ctrl_useredit_op_back" runat="server" UseSubmitBehavior="false"
            Text="返回" OnClick="btnBack_Click" OnClientClick="" />
    </div>
    <asp:HiddenControl ID="hc" runat="server" ClientInstanceName="hcPostBack" />
</asp:Content>
