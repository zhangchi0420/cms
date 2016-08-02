
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_User_viewcontrolQJ.ascx.cs" Inherits="Drision.Framework.Web.T_User_viewcontrolQJ" %>
<asp:GridControl ID="viewcontrolQJ" ClientInstanceName="PopUpValueText" KeyFieldName = "User_ID"  OnHeaderClick="viewcontrolQJ_HeaderClick" OnPageIndexChanging="viewcontrolQJ_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="viewcontrolQJ_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$User_Name"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$User_Code" DataTextField="a$User_Code"  HeaderText="工号"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$User_Name" DataTextField="a$User_Name"  HeaderText="用户姓名"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Department_ID_V" DataTextField="a$Department_ID_V"  HeaderText="所属部门"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$User_Mobile" DataTextField="a$User_Mobile"  HeaderText="手机号码"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="btnRowDeleteDD" runat="server" CssClass = "" CommandArgument='User_ID' OnClientClick="return confirm('是否删除该记录？',function(){ShowWaiting(false);});" OnClick="btnRowDeleteDD_Click">删除</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnviewcontrolQJExport" runat="server" Text="导出" Visible = "False" OnClick="btnviewcontrolQJExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>