
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_User_viewcontrol5D.ascx.cs" Inherits="Drision.Framework.Web.T_User_viewcontrol5D" %>
<asp:GridControl ID="viewcontrol5D" ClientInstanceName="PopUpValueText" KeyFieldName = "User_ID" SingleSelect = "true" OnHeaderClick="viewcontrol5D_HeaderClick" OnPageIndexChanging="viewcontrol5D_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="viewcontrol5D_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$User_Name" Visible="False"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$User_Code" DataTextField="a$User_Code"  HeaderText="工号"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$User_Name" DataTextField="a$User_Name"  HeaderText="用户姓名"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Department_ID_V" DataTextField="a$Department_ID_V"  HeaderText="所属部门"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$User_Mobile" DataTextField="a$User_Mobile"  HeaderText="手机号码"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="btnRowMMDeleteUB" runat="server" CssClass = "" CommandArgument='User_ID' OnClientClick="if (!confirm('是否删除该记录？')) {return false;} else {return ShowWaiting(false);}" OnClick="btnRowMMDeleteUB_Click">删除</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnviewcontrol5DExport" runat="server" Text="导出" Visible = "True" OnClick="btnviewcontrol5DExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>