<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PublicTempFunction.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.PublicTempFunction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:LinkButton ID="lbtnChangeSystemLevelCode" runat="server" OnClick="btnChangeSystemLevelCode_Click">修正部门层级码</asp:LinkButton>
    <%--<asp:Button ID="btnChangeSystemLevelCode" runat="server" Text="修正部门层级码" 
        onclick="btnChangeSystemLevelCode_Click" />--%>
</asp:Content>
