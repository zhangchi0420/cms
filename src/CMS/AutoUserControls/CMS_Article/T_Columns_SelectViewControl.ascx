
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Columns_SelectViewControl.ascx.cs" Inherits="Drision.Framework.Web.T_Columns_SelectViewControl" %>
<asp:GridControl ID="SelectViewControl" ClientInstanceName="PopUpValueText" KeyFieldName = "Columns_Id" SingleSelect = "true" OnHeaderClick="SelectViewControl_HeaderClick" OnPageIndexChanging="SelectViewControl_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="SelectViewControl_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$Columns_Name"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$MenuKey" DataTextField="MenuKey"  HeaderText="菜单Key"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Columns_Name" DataTextField="a$Columns_Name"  HeaderText="栏目名称"  ><ItemStyle   />  </asp:HeaderSortField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnSelectViewControlExport" runat="server" Text="导出" Visible = "False" OnClick="btnSelectViewControlExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>