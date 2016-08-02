<%@ Control Language="C#" AutoEventWireup="true" Inherits="Drision.Framework.Web.userSel" Codebehind="userSel.ascx.cs" %>
<div class="main_box">
<h3>选择人员</h3>
<div class="allcol1">
<div class="item_box_full">
<span>人员<span style="color:Red">*</span></span>
<asp:SelectControl ID="sc2" ClientInstanceName="sc2" runat="server" SelectType="List" IsRequired="true" >
    <ListSettings IsGrouped="true">
    </ListSettings>
</asp:SelectControl>
</div>
<div class="cl"></div>
</div>
</div>