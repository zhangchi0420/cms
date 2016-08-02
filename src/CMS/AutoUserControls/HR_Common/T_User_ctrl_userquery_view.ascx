
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_User_ctrl_userquery_view.ascx.cs" Inherits="Drision.Framework.Web.T_User_ctrl_userquery_view" %>
<asp:GridControl ID="ctrl_userquery_view" ClientInstanceName="PopUpValueText" KeyFieldName = "User_ID"  OnHeaderClick="ctrl_userquery_view_HeaderClick" OnPageIndexChanging="ctrl_userquery_view_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="ctrl_userquery_view_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$User_Name"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$User_Code" DataTextField="a$User_Code"  HeaderText="帐号"  HorizontalAlign = "NotSet"><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$User_Name" DataTextField="a$User_Name"  HeaderText="姓名"  HorizontalAlign = "NotSet"><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$EntryDate" DataTextField="a$EntryDate"  HeaderText="入职日期"  HorizontalAlign = "NotSet"><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$User_EMail" DataTextField="a$User_EMail"  HeaderText="电子邮件" DisplayLength = '5' HorizontalAlign = "NotSet"><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Department_ID_V" DataTextField="a$Department_ID_V"  HeaderText="所属部门"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$User_Type_V" DataTextField="a$User_Type_V"  HeaderText="用户类别"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="edit" runat="server" CssClass = "edit" CommandArgument='User_ID' OnClientClick="" OnClick="btnEdit_Click"><span>编辑</span></asp:LinkButton>
         <asp:LinkButton ID="detail" runat="server" CssClass = "detail" CommandArgument='User_ID' OnClientClick="" OnClick="btnDetail_Click"><span>详情</span></asp:LinkButton>
         <asp:LinkButton ID="delete" runat="server" CssClass = "" CommandArgument='User_ID' OnClientClick="return confirm('确认删除？');" OnClick="btnDelete_Click">删除</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnctrl_userquery_viewExport" runat="server" Text="导出" Visible = "True" OnClick="btnctrl_userquery_viewExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>