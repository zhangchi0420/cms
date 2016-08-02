<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="T_Configuration_viewcontrolSB.ascx.cs" Inherits="Drision.Framework.Web.T_Configuration_viewcontrolSB" %>
<asp:Repeater ID="Repeater1" runat="server" onitemdatabound="Repeater1_ItemDataBound">
    <ItemTemplate>
        <div class="grid_title_allinfo">
            <div class="griditem_box">
            <span>配置分组:</span>
                <asp:Label ID = "lblGroupField" runat = "server" Text='<%# Eval("GroupFieldValue") %>'></asp:Label>
            </div>
        </div>
        <asp:GridControl ID="viewcontrolSB" ShowFooter="true" ClientInstanceName="PopUpValueText" KeyFieldName = "Configuration_Id" SingleSelect = "true" runat="server"  AutoGenerateColumns="false" >
<Columns>
        <asp:FullCheckField DataTextField = "Configuration_Title"></asp:FullCheckField>
    <asp:HeaderSortField ShowHeader="false" Parameter="a$Configuration_Title" DataTextField="Configuration_Title" HeaderText="标题"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField ShowHeader="false" Parameter="a$Configuration_Key" DataTextField="Configuration_Key" HeaderText="索引"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField ShowHeader="false" Parameter="a$Configuration_Value" DataTextField="Configuration_Value" HeaderText="数据值"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField ShowHeader="false" Parameter="a$Configuration_Description" DataTextField="Configuration_Description" HeaderText="描述"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField ShowHeader="false" Parameter="a$Sort_Order" DataTextField="Sort_Order" HeaderText="排序号"  ><ItemStyle   />  </asp:HeaderSortField>
    <asp:HeaderSortField ShowHeader="false" Parameter="a$Configuration_Group_Id_V" DataTextField="Configuration_Group_Id_V" HeaderText="配置分组"  ><ItemStyle   />  </asp:HeaderSortField>

</Columns>
</asp:GridControl>
    </ItemTemplate>
</asp:Repeater>
<div id="divEmptyData" runat="server" class="grid_tbody">
    <div class="ui_widget grid_content">
        <div>
            <table class="grid_content_table">
                <tr>
                    <td style="text-align:center">
                        暂无数据
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<div id="divFooter" runat="server" class="grid_foot">
<asp:PagerControl ID = "PagerControl1" runat="server" ShowPager="true" PageIndex="0" PageSize="1000" onpageindexchanged="PagerControl1_PageIndexChanged"/>
</div>