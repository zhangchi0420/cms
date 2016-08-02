
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Role_ViewControlYF.ascx.cs" Inherits="Drision.Framework.Web.T_Role_ViewControlYF" %>
<asp:GridControl ID="ViewControlYF" ClientInstanceName="PopUpValueText" KeyFieldName = "Role_ID" SingleSelect = "true" OnHeaderClick="ViewControlYF_HeaderClick" OnPageIndexChanging="ViewControlYF_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="ViewControlYF_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$Role_Name" Visible="False"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$Role_Name" DataTextField="a$Role_Name"  HeaderText="角色名称"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Role_Status_V" DataTextField="a$Role_Status_V"  HeaderText="是否停用"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Role_Comment" DataTextField="a$Role_Comment" ShowHeader="False" HeaderText="备注"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="RowOperationJK" runat="server" CssClass = "" CommandArgument='Role_ID' OnClientClick="if (!confirm('是否删除该记录？')) {return false;}" OnClick="RowOperationJK_Click">删除</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnViewControlYFExport" runat="server" Text="导出" Visible = "True" OnClick="btnViewControlYFExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>