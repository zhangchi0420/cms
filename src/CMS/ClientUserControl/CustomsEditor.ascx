<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomsEditor.ascx.cs" Inherits="Drision.Framework.Web.CustomsEditor" %>
<div class="item_box_full">
<span> 内容</span>
<span class = "left_star">&nbsp;</span>
<div style="padding-left:92px;" id="articltext">
<textarea id="editor" cols="100" rows="8" style="width:600px;height:500px;visibility:hidden;" runat="server"></textarea>
</div>
</div>
<asp:HiddenField ID="hf_EditorValue" Value =""  runat="server"/>