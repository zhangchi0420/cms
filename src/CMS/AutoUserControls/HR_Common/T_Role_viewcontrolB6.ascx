
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Role_viewcontrolB6.ascx.cs" Inherits="Drision.Framework.Web.T_Role_viewcontrolB6" %>
<asp:GridControl ID="viewcontrolB6" ClientInstanceName="PopUpValueText" KeyFieldName = "Role_ID" SingleSelect = "true" OnHeaderClick="viewcontrolB6_HeaderClick" OnPageIndexChanging="viewcontrolB6_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="viewcontrolB6_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$Role_Name"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$Role_Name" DataTextField="a$Role_Name"  HeaderText="角色名称"  HorizontalAlign = "Center"><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Role_Comment" DataTextField="a$Role_Comment" ShowHeader="False" HeaderText="备注"  HorizontalAlign = "Center"><ItemStyle   />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="lbtnWN" runat="server" CssClass = "" CommandArgument='Role_ID' OnClientClick="" OnClick="lbtnWN_Click">停用</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnviewcontrolB6Export" runat="server" Text="导出" Visible = "True" OnClick="btnviewcontrolB6Export_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>