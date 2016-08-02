
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Department_ViewControlC2.ascx.cs" Inherits="Drision.Framework.Web.T_Department_ViewControlC2" %>
<asp:GridControl ID="ViewControlC2" ClientInstanceName="PopUpValueText" KeyFieldName = "Department_ID" SingleSelect = "true" OnHeaderClick="ViewControlC2_HeaderClick" OnPageIndexChanging="ViewControlC2_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="ViewControlC2_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$Department_Name" Visible="False"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$Department_Name" DataTextField="a$Department_Name"  HeaderText="组织名称"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Department_Full_Code" DataTextField="a$Department_Full_Code"  HeaderText="完整代码"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Parent_DepartmentID_V" DataTextField="a$Parent_DepartmentID_V"  HeaderText="父级组织"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Department_Status_V" DataTextField="a$Department_Status_V"  HeaderText="是否停用"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="RowOperationWZ" runat="server" CssClass = "" CommandArgument='Department_ID' OnClientClick="" OnClick="RowOperationWZ_Click">停用</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnViewControlC2Export" runat="server" Text="导出" Visible = "True" OnClick="btnViewControlC2Export_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>