
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Columns_DefaultViewControl.ascx.cs" Inherits="Drision.Framework.Web.T_Columns_DefaultViewControl" %>
<asp:GridControl ID="DefaultViewControl" ClientInstanceName="PopUpValueText" KeyFieldName = "Columns_Id" SingleSelect = "true" OnHeaderClick="DefaultViewControl_HeaderClick" OnPageIndexChanging="DefaultViewControl_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="DefaultViewControl_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$Columns_Name" Visible="False"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$MenuKey" DataTextField="MenuKey"  HeaderText="菜单Key"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Columns_Name" DataTextField="a$Columns_Name"  HeaderText="栏目名称"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$CreateTime" DataTextField="a$CreateTime"  HeaderText="创建时间"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$State_V" DataTextField="State_V"  HeaderText="状态"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="lbtnEdit" runat="server" CssClass = "" CommandArgument='Columns_Id' OnClientClick="" OnClick="lbtnEdit_Click">编辑</asp:LinkButton>
         <asp:LinkButton ID="lbtnDel" runat="server" CssClass = "" CommandArgument='Columns_Id' OnClientClick="return confirm('是否删除该记录？',function(){ShowWaiting(false);});" OnClick="lbtnDel_Click">删除</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnDefaultViewControlExport" runat="server" Text="导出" Visible = "False" OnClick="btnDefaultViewControlExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>