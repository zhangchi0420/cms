
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Department_ctrl_deptquery_view.ascx.cs" Inherits="Drision.Framework.Web.T_Department_ctrl_deptquery_view" %>
<asp:GridControl ID="ctrl_deptquery_view" ClientInstanceName="PopUpValueText" KeyFieldName = "Department_ID" SingleSelect = "true" OnHeaderClick="ctrl_deptquery_view_HeaderClick" OnPageIndexChanging="ctrl_deptquery_view_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="ctrl_deptquery_view_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$Department_Name" Visible="False"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$Department_Name" DataTextField="a$Department_Name"  HeaderText="组织名称"  HorizontalAlign = "Left"><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Department_Full_Code" DataTextField="a$Department_Full_Code"  HeaderText="完整代码"  HorizontalAlign = "Center"><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Parent_DepartmentID_V" DataTextField="a$Parent_DepartmentID_V"  HeaderText="父级组织"  HorizontalAlign = "Left"><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Department_Status_V" DataTextField="a$Department_Status_V"  HeaderText="是否停用"  HorizontalAlign = "Center"><ItemStyle   />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="RowOperationSU" runat="server" CssClass = "" CommandArgument='Department_ID' OnClientClick="" OnClick="RowOperationSU_Click">启用</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnctrl_deptquery_viewExport" runat="server" Text="导出" Visible = "True" OnClick="btnctrl_deptquery_viewExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>