<%@ Page Title="用户详情" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserDetail.aspx.cs" Inherits="Drision.Framework.Web.UserDetail" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

        <%@ Register src="../AutoUserControls/HR_Common/T_Role_ViewControlYF.ascx" tagname="T_Role_ViewControlYF" tagprefix="asp" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/UserDetail.css")))
   { %>
<link href="../ClientCSS/UserDetail.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/UserDetail.js")))
   { %>
<script src="../ClientScripts/UserDetail.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
基本信息</h3>

<div class="allcol2">

        <div class = "item_box_col1">
        
        <span>帐号</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_User_Code" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>姓名</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_User_Name" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>入职日期</span>
        
            <div class = "item_display"> <asp:Label ID = "ctrl_EntryDate" runat = "server"></asp:Label> </div>

        </div>
        <div class = "item_box_col1">
        
        <span>身份证号</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_Card_No" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>手机号码</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_User_Mobile" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>电子邮件</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_User_EMail" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>是否有效</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_User_Status" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>是否禁用Web端</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_Is_Prohibit_Web" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>是否禁用手机端</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_Is_Prohibit_Mobile" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>所属部门</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_Department_ID" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>用户类别</span>
        
            <div class = "item_display"><asp:Label ID = "txt_User_Type25" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_full">
        
        <span>备注</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_User_Comment" runat = "server"></asp:Label></div>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>

<div class="button_bar">
        <asp:LinkButton ID="Operation2G" runat="server"  UseSubmitBehavior = "false" Text="添加角色" onclick = "OperationTH_Click" OnClientClick=""/>
</div>



<div id = "divViewControlYF" runat = "server">
    <asp:T_Role_ViewControlYF ID = "ViewControlYF" runat="server"></asp:T_Role_ViewControlYF>
</div>

<div class="button_bar">
        <asp:LinkButton ID="ctrl_userdetail_op_back" runat="server"  UseSubmitBehavior = "false" Text="返回" onclick = "btnBack_Click" OnClientClick=""/>
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