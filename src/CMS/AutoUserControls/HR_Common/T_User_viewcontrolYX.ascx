
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_User_viewcontrolYX.ascx.cs" Inherits="Drision.Framework.Web.T_User_viewcontrolYX" %>
<asp:GridControl ID="viewcontrolYX" ClientInstanceName="PopUpValueText" KeyFieldName = "User_ID" SingleSelect = "true" OnHeaderClick="viewcontrolYX_HeaderClick" OnPageIndexChanging="viewcontrolYX_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="viewcontrolYX_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$User_Name"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$Department_ID_V" DataTextField="a$Department_ID_V"  HeaderText="所属部门"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$User_Code" DataTextField="a$User_Code"  HeaderText="工号"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$User_Name" DataTextField="a$User_Name"  HeaderText="用户姓名"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Is_Attend_V" DataTextField="a$Is_Attend_V"  HeaderText="是否考勤"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="btnRowEditHE" runat="server" CssClass = "" CommandArgument='User_ID' OnClientClick="" OnClick="btnRowEditHE_Click">设置考勤</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnviewcontrolYXExport" runat="server" Text="导出" Visible = "True" OnClick="btnviewcontrolYXExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>