
<%@ Register Assembly="Drision.Framework.WebControls" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Configuration_viewcontrol3F.ascx.cs" Inherits="Drision.Framework.Web.T_Configuration_viewcontrol3F" %>
<asp:GridControl ID="viewcontrol3F" ClientInstanceName="PopUpValueText" KeyFieldName = "Configuration_Id" SingleSelect = "true" OnHeaderClick="viewcontrol3F_HeaderClick" OnPageIndexChanging="viewcontrol3F_PageIndexChanging" runat="server"  AutoGenerateColumns="false" onrowdatabound="viewcontrol3F_RowDataBound" ShowFooter="false" >
<Columns>
    <asp:FullCheckField DataTextField = "a$Configuration_Title"></asp:FullCheckField>
    <asp:HeaderSortField Parameter="a$Configuration_Title" DataTextField="a$Configuration_Title"  HeaderText="标题"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Configuration_Key" DataTextField="a$Configuration_Key"  HeaderText="索引"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>
            <asp:HeaderSortField HeaderText="数据值" ShowHeader="False">
                <ItemStyle Width = "100" />
    	        <ItemTemplate>
                            <asp:TextControl ID = "Configuration_Value" ClientInstanceName = "Configuration_Value" runat="server" Width = "100" Height = "20" TextType = "String" FieldName = "Configuration_Value" IsRequired = "true" />

	            </ItemTemplate>
            </asp:HeaderSortField>
    <asp:HeaderSortField Parameter="a$Configuration_Description" DataTextField="a$Configuration_Description" ShowHeader="False" HeaderText="描述"  ><ItemStyle  CssClass = "wordspace_all" />  </asp:HeaderSortField>

</Columns>
<PagerSettings ShowPager="true" PageIndex="0" PageSize="10">
    <PagerButtons>
        <asp:LinkButton ID="btnviewcontrol3FExport" runat="server" Text="导出" Visible = "False" OnClick="btnviewcontrol3FExport_Click"></asp:LinkButton>
    </PagerButtons>
</PagerSettings>
</asp:GridControl>