<%@ Page Title="报表查询字段" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportQueryFieldSet.aspx.cs" Inherits="Drision.Framework.Web.Report.ReportQueryFieldSet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main_box">
    <asp:TreeControl ID="tcField" runat="server" AutoPostBack = "true" CssClass = "treebox"  OnlyLeafCheckBox = "true" ExpandAllNodes = "false"  ShowCheckBox="True" OnNodeClick = "tcField_NodeClick"></asp:TreeControl>
    <div class= "treeform">
        <div style = "color:Red; font-size:1.3em; height:80px">
            通过勾选左侧树节点后点击功能按钮添加，或直接点击左侧树节点添加
            <br />
            注意：标注外键的字段只是数字类型的唯一标志，如果想查询名称，请选择对应实体的名称字段
        </div>
        <div class="allcol1">
            <div class = "item_box_full">
            <span>显示名称</span>
            <span class = "left_star">*</span>
                <asp:TextControl ID = "tcDisplayText" runat="server" TextType = "String" IsRequired = "true"  MaxLength = "50"/>
            </div>
        </div>
        <div class="button_bar">
            <asp:LinkButton ID="btnJustAdd" runat="server" CommandArgument = "0"  Text="直接添加" OnClientClick="return Validate();" OnClick = "btnFuntion_Click"/> 
            <asp:LinkButton ID="btnAdd" runat="server" CommandArgument = "1"  Text="加" OnClientClick="return Validate();" OnClick = "btnFuntion_Click"/> 
            <asp:LinkButton ID="btnMinus" runat="server" CommandArgument = "2" Text="减" OnClientClick="return Validate();" OnClick = "btnFuntion_Click"/> 
            <asp:LinkButton ID="btnReMinus" runat="server" CommandArgument = "9" Text="反减" OnClientClick="return Validate();" OnClick = "btnFuntion_Click"/> 
            <asp:LinkButton ID="btnMul" runat="server" CommandArgument = "3" Text="乘" OnClientClick="return Validate();" OnClick = "btnFuntion_Click"/> 
            <asp:LinkButton ID="btnDiv" runat="server" CommandArgument = "4" Text="除" OnClientClick="return Validate();" OnClick = "btnFuntion_Click"/> 
            <asp:LinkButton ID="btnReDiv" runat="server" CommandArgument = "8" Text="反除" OnClientClick="return Validate();" OnClick = "btnFuntion_Click"/> 
            <asp:LinkButton ID="btnGetYear" runat="server" CommandArgument = "5" Text="年份" OnClientClick="return Validate();" OnClick = "btnFuntion_Click"/> 
            <asp:LinkButton ID="btnGetMonth" runat="server" CommandArgument = "6" Text="月份" OnClientClick="return Validate();" OnClick = "btnFuntion_Click"/> 
            <asp:LinkButton ID="btnGetDate" runat="server" CommandArgument = "7" Text="日期" OnClientClick="return Validate();" OnClick = "btnFuntion_Click"/> 
        </div>
        <div class = "grid_title">已有的查询字段</div>
        <div class="grid_filter"></div>
        <asp:GridControl ID="gcList" runat="server"  AutoGenerateColumns="false" ShowFooter="false" SingleSelect = "false" KeyFieldName = "QueryFieldId">
            <Columns>
                <asp:FullCheckField DataTextField = "DisplayText"></asp:FullCheckField>
                <asp:BoundField  DataField ="DisplayText"  HeaderText="显示名称"  ></asp:BoundField>
                <asp:BoundField  DataField ="DefaultDisplayText"  HeaderText="查询名称"  ></asp:BoundField> 
                <asp:OperationField>
                <Buttons>
                        <asp:LinkButton ID="lbtnDel" runat="server" Text="删除" CommandArgument='QueryFieldId' OnClientClick="if (!confirm('是否删除该查询字段？')) {return false;}" OnClick = "lbtnDel_Click"></asp:LinkButton>
                </Buttons>
                </asp:OperationField>
            </Columns>
        </asp:GridControl>
        <div style = "color:Red; font-size:1.3em; height:40px">
            可以选择两个已有的查询字段进行运算，实现多个字段的无限递归运算
        </div>
        <div class="allcol1">
            <div class = "item_box_full">
            <span>二次查询名称</span>
            <span class = "left_star">*</span>
                <asp:TextControl ID = "tcDisplayTextSub" runat="server" TextType = "String"  MaxLength = "50"/>
            </div>
        </div>
        <div class="button_bar">
            <asp:LinkButton ID="btnAddSub" runat="server" CommandArgument = "1"  Text="加" OnClick = "btnQueryFieldFuntion_Click"/> 
            <asp:LinkButton ID="btnMinusSub" runat="server" CommandArgument = "2" Text="减" OnClick = "btnQueryFieldFuntion_Click"/> 
            <asp:LinkButton ID="btnReMinusSub" runat="server" CommandArgument = "9" Text="反减" OnClick = "btnQueryFieldFuntion_Click"/> 
            <asp:LinkButton ID="btnMulSub" runat="server" CommandArgument = "3" Text="乘" OnClick = "btnQueryFieldFuntion_Click"/> 
            <asp:LinkButton ID="btnDivSub" runat="server" CommandArgument = "4" Text="除" OnClick = "btnQueryFieldFuntion_Click"/> 
            <asp:LinkButton ID="btnReDivSub" runat="server" CommandArgument = "8" Text="反除" OnClick = "btnQueryFieldFuntion_Click"/> 
        </div>
    </div>
    <div class = "cl"></div>
</div>
<div class="button_bar">
    <asp:LinkButton ID="btnPre"  runat="server" Text="上一步" OnClick = "btnPre_Click"></asp:LinkButton> 
    <asp:LinkButton ID="btnNext"  runat="server" Text="下一步" OnClick = "btnNext_Click"></asp:LinkButton> 
</div>
</asp:Content>
