
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_CustomAreaCode_viewcontrol3W.ascx.cs" Inherits="Drision.Framework.Web.T_CustomAreaCode_viewcontrol3W" %>
<asp:GridControl ID="viewcontrol3W" ClientInstanceName="PopUpValueText" KeyFieldName = "CustomAreaCode_Id" SingleSelect = "true" OnHeaderClick="viewcontrol3W_HeaderClick" OnPageIndexChanging="viewcontrol3W_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="viewcontrol3W_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$CustomAreaCode_Name"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$CustomAreaCode" DataTextField="CustomAreaCode"  HeaderText="海关代码"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$CustomAreaCode_Name" DataTextField="a$CustomAreaCode_Name"  HeaderText="海关关区"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$CreateTime" DataTextField="a$CreateTime"  HeaderText="创建时间"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="btnRowDeleteJ3" runat="server" CssClass = "" CommandArgument='CustomAreaCode_Id' OnClientClick="return confirm('是否删除该记录？',function(){ShowWaiting(false);});" OnClick="btnRowDeleteJ3_Click">删除</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnviewcontrol3WExport" runat="server" Text="导出" Visible = "True" OnClick="btnviewcontrol3WExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>