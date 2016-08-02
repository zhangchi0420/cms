<%@ Page Title="用户修改" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="Drision.Framework.Web.UserEdit" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/UserEdit.css")))
   { %>
<link href="../ClientCSS/UserEdit.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/UserEdit.js")))
   { %>
<script src="../ClientScripts/UserEdit.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
基本信息</h3>

<div class="allcol2">

        <div class = "item_box_col1">
        
        <span> 帐号</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "ctrl_User_Code" ClientInstanceName = "ctrl_User_Code"   runat="server" TextType = "String" FieldName = "User_Code" IsRequired = "true"  MaxLength = "50"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>姓名</span>
        <span class = "left_star">*</span>
            <asp:TextControl ID = "ctrl_User_Name" ClientInstanceName = "ctrl_User_Name"   runat="server" TextType = "String" FieldName = "User_Name" IsRequired = "true"  MaxLength = "20"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>入职日期</span>
        <span class = "left_star">&nbsp;</span>
            <asp:DateTimeControl ID = "ctrl_EntryDate" ClientInstanceName = "ctrl_EntryDate"  runat="server" FieldName = "EntryDate"  ShowType="Date"/>

        </div>
        <div class = "item_box_col1">
        
        <span>身份证号</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "ctrl_Card_No" ClientInstanceName = "ctrl_Card_No"   runat="server" TextType = "String" FieldName = "Card_No"   MaxLength = "18"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>手机号码</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "ctrl_User_Mobile" ClientInstanceName = "ctrl_User_Mobile"   runat="server" TextType = "String" FieldName = "User_Mobile"   MaxLength = "15"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>电子邮件</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "ctrl_User_EMail" ClientInstanceName = "ctrl_User_EMail"   runat="server" TextType = "String" FieldName = "User_EMail"   MaxLength = "50"  PlaceHolder = ""/>

        </div>
        <div class = "item_box_col1">
        
        <span>是否有效</span>
        <span class = "left_star">*</span>
            <asp:ComboControl ID = "ctrl_User_Status" ClientInstanceName = "ctrl_User_Status"   runat="server" FieldName = "User_Status" DropdownType = "DropdownList" IsRequired = "true" ShowEmptyItem = "true" AutoSelectFirst = "true"/>

        </div>
        <div class = "item_box_col1">
        
        <span>是否禁用Web端</span>
        <span class = "left_star">&nbsp;</span>
            <asp:ComboControl ID = "ctrl_Is_Prohibit_Web" ClientInstanceName = "ctrl_Is_Prohibit_Web"   runat="server" FieldName = "Is_Prohibit_Web" DropdownType = "DropdownList"  ShowEmptyItem = "true" AutoSelectFirst = "true"/>

        </div>
        <div class = "item_box_col1">
        
        <span>是否禁用手机端</span>
        <span class = "left_star">&nbsp;</span>
            <asp:ComboControl ID = "ctrl_Is_Prohibit_Mobile" ClientInstanceName = "ctrl_Is_Prohibit_Mobile"   runat="server" FieldName = "Is_Prohibit_Mobile" DropdownType = "DropdownList"  ShowEmptyItem = "true" AutoSelectFirst = "true"/>

        </div>
        <div class = "item_box_col1">
        
        <span>所属部门</span>
        <span class = "left_star">&nbsp;</span>
            <sp:Dropdown ID = "ctrl_Department_ID" ClientInstanceName = "ctrl_Department_ID"   runat="server" FieldName = "Department_ID"  ShowEmptyItem = "true" >  </sp:Dropdown>

        </div>
        <div class = "item_box_col1">
        
        <span>用户类别</span>
        <span class = "left_star">&nbsp;</span>
            <asp:ComboControl ID = "txt_User_TypeJW" ClientInstanceName = "txt_User_TypeJW"   runat="server" FieldName = "User_Type" DropdownType = "DropdownList"  ShowEmptyItem = "true" AutoSelectFirst = "true"/>

        </div>
        <div class = "item_box_full">
        
        <span>备注</span>
        <span class = "left_star">&nbsp;</span>
            <asp:TextControl ID = "ctrl_User_Comment" ClientInstanceName = "ctrl_User_Comment"   runat="server" TextType = "Text" FieldName = "User_Comment"   MaxLength = "256"  PlaceHolder = ""/>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
<div class="button_bar">
        <asp:LinkButton ID="btn5K" runat="server"  UseSubmitBehavior = "false" Text="重置密码(123)" onclick = "btn2H_Click" OnClientClick=""/>
        <asp:LinkButton ID="ctrl_useredit_op_save" runat="server"  Text="保存" onclick = "btnSave_Click" OnClientClick="" onprerender="ctrl_useredit_op_save_PreRender"/>
        <asp:LinkButton ID="ctrl_useredit_op_back" runat="server"  UseSubmitBehavior = "false" Text="返回" onclick = "btnBack_Click" OnClientClick=""/>
</div>

    <asp:HiddenControl ID="hcPostBack"  runat="server" ClientInstanceName="hcPostBack" />
    <asp:CallBackControl ID="cbcPatchAdd" ClientInstanceName="cbcPatchAdd" runat="server" CallBackType="JSON"
    OnCallBack="cbcPatchAdd_CallBack" OnComplete="function(data){
        if(data == '1'){
            eval(hcPostBack.getText());
        }
        else{
            alert(data);
        }
    }" OnError="function(data){alert(data);}"
    />
</asp:Content>