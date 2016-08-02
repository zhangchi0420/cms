<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoleOfEntityManage.aspx.cs"
    Inherits="Drision.Framework.Web.SystemManagement.RoleOfEntityManage" Title="实体权限" %>

<%@ Register Src="RoleOfEntityControl.ascx" TagName="RoleOfEntityControl" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- 表单开始，这里开始是内页 -->
    
        <div class="grid_title">
            <span>实体列表</span>
        <div class = "grid_filter">
            <asp:Label runat="server" ID="lblRoleName" Text="角色名称：" />
            <asp:DropDownList ID="ddlRoleList" runat="server" Width="130px" AutoPostBack="true"
                 OnSelectedIndexChanged="ddlRoleList_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:Label runat="server" ID="lblCategory" Text="子系统：" />
            <asp:DropDownList ID="ddlCategory" runat="server" Width="130px" AutoPostBack="true"
                 OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
            </asp:DropDownList>            
        </div>
        
        </div>
        <div class="grid_tbody">
        <div class="grid_content">
        <uc1:RoleOfEntityControl ID="RoleOfEntityControl1" runat="server" />
        </div>        
    </div>

    <div class="button_bar">
        <%--<input type="button" value="重置" onclick="javascript:Reset();alert('重置页面成功,点击保存!');" />--%>
        <asp:LinkButton ID="lbtnReset" runat="server" OnClientClick = "return Reset();">重置</asp:LinkButton>
        <asp:LinkButton ID="lbtnSaveAA" runat="server" OnClick="btnSaveAA_Click">保存</asp:LinkButton>
        <%--<asp:Button ID="btnReset" runat="server" Text="重置" OnClientClick = "return Reset();" />
        <asp:Button ID="btnSaveAA" runat="server" Text="保存" OnClick="btnSaveAA_Click" />--%>
        <%--<asp:Button ID="btn" runat="server" Text="退出" OnClick="btnOff_Click" />--%>
    </div>
</asp:Content>
