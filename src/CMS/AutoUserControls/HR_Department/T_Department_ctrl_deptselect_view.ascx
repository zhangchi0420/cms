
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Department_ctrl_deptselect_view.ascx.cs" Inherits="Drision.Framework.Web.T_Department_ctrl_deptselect_view" %>
<asp:GridControl ID="ctrl_deptselect_view" ClientInstanceName="PopUpValueText" KeyFieldName = "Department_ID" SingleSelect = "true" OnHeaderClick="ctrl_deptselect_view_HeaderClick" OnPageIndexChanging="ctrl_deptselect_view_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="ctrl_deptselect_view_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$Department_Name"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$Department_Name" DataTextField="a$Department_Name"  HeaderText="部门名称"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Department_Full_Code" DataTextField="a$Department_Full_Code"  HeaderText="完整代码"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Parent_DepartmentID_V" DataTextField="a$Parent_DepartmentID_V"  HeaderText="父级组织"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnctrl_deptselect_viewExport" runat="server" Text="导出" Visible = "False" OnClick="btnctrl_deptselect_viewExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>