
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Department_viewcontrol46.ascx.cs" Inherits="Drision.Framework.Web.T_Department_viewcontrol46" %>
<asp:GridControl ID="viewcontrol46" ClientInstanceName="PopUpValueText" KeyFieldName = "Department_ID" SingleSelect = "true" OnHeaderClick="viewcontrol46_HeaderClick" OnPageIndexChanging="viewcontrol46_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="viewcontrol46_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$Department_Name" Visible="False"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$Department_Name" DataTextField="a$Department_Name"  HeaderText="组织名称"  HorizontalAlign = "Left"><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Department_Full_Code" DataTextField="a$Department_Full_Code"  HeaderText="完整代码"  HorizontalAlign = "Center"><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Parent_DepartmentID_V" DataTextField="a$Parent_DepartmentID_V"  HeaderText="父级组织"  HorizontalAlign = "Left"><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Department_Status_V" DataTextField="a$Department_Status_V"  HeaderText="是否停用"  HorizontalAlign = "Center"><ItemStyle   />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="lbtn4V" runat="server" CssClass = "" CommandArgument='Department_ID' OnClientClick="" OnClick="lbtn4V_Click">详情</asp:LinkButton>
         <asp:LinkButton ID="lbtnTP" runat="server" CssClass = "" CommandArgument='Department_ID' OnClientClick="if (!confirm('是否删除该记录？')) {return false;}" OnClick="lbtnTP_Click">删除</asp:LinkButton>
         <asp:LinkButton ID="lbtnXC" runat="server" CssClass = "" CommandArgument='Department_ID' OnClientClick="javascript:window.location.href='/HR_Department/T_Department_Add.aspx?id='+$(this).attr('dataValue');" OnClick="lbtnXC_Click">编辑</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnviewcontrol46Export" runat="server" Text="导出" Visible = "True" OnClick="btnviewcontrol46Export_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>