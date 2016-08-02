
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Role_ctrl_rolequery_view.ascx.cs" Inherits="Drision.Framework.Web.T_Role_ctrl_rolequery_view" %>
<asp:GridControl ID="ctrl_rolequery_view" ClientInstanceName="PopUpValueText" KeyFieldName = "Role_ID"  OnHeaderClick="ctrl_rolequery_view_HeaderClick" OnPageIndexChanging="ctrl_rolequery_view_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="ctrl_rolequery_view_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$Role_Name"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$Role_Name" DataTextField="a$Role_Name"  HeaderText="角色名称"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Role_Status_V" DataTextField="a$Role_Status_V"  HeaderText="是否停用"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Role_Comment" DataTextField="a$Role_Comment" ShowHeader="False" HeaderText="备注"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="lbtn8B" runat="server" CssClass = "" CommandArgument='Role_ID' OnClientClick="" OnClick="lbtn8B_Click">编辑</asp:LinkButton>
         <asp:LinkButton ID="lbtn32" runat="server" CssClass = "" CommandArgument='Role_ID' OnClientClick="if (!confirm('是否删除该记录？')) {return false;}" OnClick="lbtn32_Click">删除</asp:LinkButton>
         <asp:LinkButton ID="btnRowDetailZ5" runat="server" CssClass = "" CommandArgument='Role_ID' OnClientClick="" OnClick="btnRowDetailZ5_Click">详情</asp:LinkButton>
         <asp:LinkButton ID="lbtn6S" runat="server" CssClass = "" CommandArgument='Role_ID' OnClientClick="javascript:window.location.href='/SystemManagement/RightOfMenu.aspx?RoleId='+$(this).attr('dataValue');return false;" OnClick="lbtn6S_Click">设置功能</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnctrl_rolequery_viewExport" runat="server" Text="导出" Visible = "True" OnClick="btnctrl_rolequery_viewExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>