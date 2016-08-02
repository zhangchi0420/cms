<%@ Page Title="部门详情" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DepartmentDetail.aspx.cs" Inherits="Drision.Framework.Web.DepartmentDetail" %>
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientCSS/DepartmentDetail.css")))
   { %>
<link href="../ClientCSS/DepartmentDetail.css" rel="stylesheet" type="text/css"  runat="server" />
<% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% if (System.IO.File.Exists(Server.MapPath("~/ClientScripts/DepartmentDetail.js")))
   { %>
<script src="../ClientScripts/DepartmentDetail.js" type="text/javascript"></script>
<% } %>
<div class = "out_border">
<div class="main_box">
<h3>
基本信息</h3>

<div class="allcol2">

        <div class = "item_box_col1">
        
        <span>组织编码</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_Deportment_Encode" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>组织层级码</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_Department_Code" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>组织名称</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_Department_Name" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>完整代码</span>
        
            <div class = "item_display"><asp:Label ID = "txt_Department_Full_CodeWW" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>父级组织</span>
        
            <div class = "item_display"><asp:Label ID = "txt_Parent_DepartmentIDJM" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_col1">
        
        <span>是否停用</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_Department_Status" runat = "server"></asp:Label></div>

        </div>
        <div class = "item_box_full">
        
        <span>备注</span>
        
            <div class = "item_display"><asp:Label ID = "ctrl_Department_Comment" runat = "server"></asp:Label></div>

        </div>
</div>
<div class = "cl"></div>
</div>
</div>
<div class="button_bar">
        <asp:LinkButton ID="ctrl_deptdetail_op_back" runat="server"  UseSubmitBehavior = "false" Text="返回" onclick = "btnBack_Click" OnClientClick=""/>
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