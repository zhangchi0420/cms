
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Product_DefaultViewControl.ascx.cs" Inherits="Drision.Framework.Web.T_Product_DefaultViewControl" %>
<asp:GridControl ID="DefaultViewControl" ClientInstanceName="PopUpValueText" KeyFieldName = "Product_Id" SingleSelect = "true" OnHeaderClick="DefaultViewControl_HeaderClick" OnPageIndexChanging="DefaultViewControl_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="DefaultViewControl_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$Product_Name" Visible="False"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$Product_Name" DataTextField="a$Product_Name"  HeaderText="产品名称"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$ProductTypeId_V" DataTextField="ProductTypeId_V"  HeaderText="产品类型"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$DesignStyle" DataTextField="DesignStyle"  HeaderText="设计风格"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Designers" DataTextField="Designers"  HeaderText="主案设计师"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$State_V" DataTextField="State_V"  HeaderText="状态"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$CreateTime" DataTextField="a$CreateTime"  HeaderText="创建时间"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:OperationField>
    <Buttons>
         <asp:LinkButton ID="lbtnEdit" runat="server" CssClass = "" CommandArgument='Product_Id' OnClientClick="" OnClick="lbtnEdit_Click">编辑</asp:LinkButton>
         <asp:LinkButton ID="lbtnDel" runat="server" CssClass = "" CommandArgument='Product_Id' OnClientClick="return confirm('是否删除该记录？');" OnClick="lbtnDel_Click">删除</asp:LinkButton>
         <asp:LinkButton ID="lbtnLook" runat="server" CssClass = "" CommandArgument='Product_Id' OnClientClick="" OnClick="lbtnLook_Click">查看</asp:LinkButton>
    </Buttons>
    </asp:OperationField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnDefaultViewControlExport" runat="server" Text="导出" Visible = "False" OnClick="btnDefaultViewControlExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>