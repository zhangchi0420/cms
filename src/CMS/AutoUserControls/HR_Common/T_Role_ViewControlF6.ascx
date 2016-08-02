
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Role_ViewControlF6.ascx.cs" Inherits="Drision.Framework.Web.T_Role_ViewControlF6" %>
<asp:GridControl ID="ViewControlF6" ClientInstanceName="PopUpValueText" KeyFieldName = "Role_ID" SingleSelect = "true" OnHeaderClick="ViewControlF6_HeaderClick" OnPageIndexChanging="ViewControlF6_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="ViewControlF6_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$Role_Name"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$Role_Name" DataTextField="a$Role_Name"  HeaderText="角色名称"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Role_Comment" DataTextField="a$Role_Comment" ShowHeader="False" HeaderText="备注"  ><ItemStyle   />  </asp:HeaderSortField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnViewControlF6Export" runat="server" Text="导出" Visible = "False" OnClick="btnViewControlF6Export_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>