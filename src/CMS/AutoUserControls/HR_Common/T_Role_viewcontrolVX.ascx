
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Role_viewcontrolVX.ascx.cs" Inherits="Drision.Framework.Web.T_Role_viewcontrolVX" %>
<asp:GridControl ID="viewcontrolVX" ClientInstanceName="PopUpValueText" KeyFieldName = "Role_ID" SingleSelect = "true" OnHeaderClick="viewcontrolVX_HeaderClick" OnPageIndexChanging="viewcontrolVX_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="viewcontrolVX_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$Role_Name"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$Role_Name" DataTextField="a$Role_Name"  HeaderText="角色名称"  HorizontalAlign = "Center"><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Role_Comment" DataTextField="a$Role_Comment" ShowHeader="False" HeaderText="备注"  HorizontalAlign = "Center"><ItemStyle   />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="lbtnSY" runat="server" CssClass = "" CommandArgument='Role_ID' OnClientClick="" OnClick="lbtnSY_Click">启用</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnviewcontrolVXExport" runat="server" Text="导出" Visible = "True" OnClick="btnviewcontrolVXExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>