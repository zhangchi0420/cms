<%@ Page Title="流程提醒模板编辑" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessTemplate.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.ProcessTemplate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/iframeEdit.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width = "100%">
    <tr>
        <td rowspan = "7">
            <span style = "font-size:25px">提醒标题模板</span>
            <div style="border: 1px solid #bbb;">
                <iframe id="HtmlEditorTitle" name="HtmlEditorTitle" style="height: 100px; background-color:White;
                    width: 100%" frameborder="0" marginheight="0" marginwidth="0" src="about:blank">
                </iframe>
            </div>
            <span style = "font-size:25px">提醒内容模板</span>
            <div style="border: 1px solid #bbb;">
                <iframe id="HtmlEditorContent" name="HtmlEditorContent" style="height: 400px; background-color:White;
                    width: 100%" frameborder="0" marginheight="0" marginwidth="0" src="about:blank">
                </iframe>
            </div>
        </td>
        <td>
            <span>模板名称</span>
        </td>
        <td>
            <asp:TextControl ID = "tcTemplateName" ClientInstanceName = "tcTemplateName" runat="server" TextType = "String" IsRequired = "true" />
        </td>
    </tr>
    <tr>
        <td>
            <span>流程实体</span>
        </td>
        <td>
            <asp:ComboControl ID = "ccProcessEntityId" ClientInstanceName = "ccProcessEntityId" runat="server" AutoSelectFirst = "true"></asp:ComboControl>
        </td>
    </tr>
    <tr>
        <td>
            <span>活动实体</span>
        </td>
        <td>
            <asp:ComboControl ID = "ccActivityEntityId" ClientInstanceName = "ccActivityEntityId" runat="server" AutoSelectFirst = "true"></asp:ComboControl>
        </td>
    </tr>
    <tr>
        <td>
            <span>模板类型</span>
        </td>
        <td>
            <asp:ComboControl ID = "ccTemplateType" ClientInstanceName = "ccTemplateType" runat="server" AutoSelectFirst = "true"></asp:ComboControl>
        </td>
    </tr>
    <tr>
        <td>
            <span>执行时机</span>
        </td>
        <td>
            <asp:ComboControl ID = "ccUseTimeType" ClientInstanceName = "ccUseTimeType" runat="server" AutoPostBack = "true" AutoSelectFirst = "true" OnTextChanged = "ccUseTimeType_TextChanged"></asp:ComboControl>
        </td>
    </tr>
    <tr>
        <td>
            <span>结果类型</span>
        </td>
        <td>
            <asp:ComboControl ID = "ccResultType" ClientInstanceName = "ccResultType" runat="server" AutoSelectFirst = "true"></asp:ComboControl>
        </td>
    </tr>
    <tr>
        <td colspan = "2">
            <%--咦，有棵树--%>
            <asp:TreeControl ID="TreeParamer" runat="server"  ClientInstanceName="TreeParamer" ExpandAllNodes = "True" LoadMode = "Default"  ShowCheckBox="False" CssClass = "treebox">
            <ClientSideEvents  OnNodeClick ="TreeAddParamer"/>
            </asp:TreeControl>
        </td>
    </tr>
</table>
    <asp:HiddenControl runat = "server" ID = "hcTemplate" ClientInstanceName = "hcTemplate"/>
    <asp:Button ID="btnSave" runat="server" Text="保存"  OnClick = "btnSave_Click" OnClientClick = "return getInnerHtml();"/>
    <asp:Button ID="btnReturn" runat="server" Text="返回"  PostBackUrl = "~/SystemManagement/ProcessTemplateQuery.aspx"/>

    <script type = "text/javascript">
        Sys.Application.add_load(function () {
            setDesignModeOn('HtmlEditorTitle');
            setDesignModeOn('HtmlEditorContent');
            setInnerHtml();

        });


        function getInnerHtml() {
            var Titleiframebody = $("#HtmlEditorTitle")[0].contentWindow.document;
            var TemplateTitle = HtmlEncode($(Titleiframebody).find("body").attr("innerHTML"));
            hcTemplate.setValue(TemplateTitle);

            var Contentiframebody = $("#HtmlEditorContent")[0].contentWindow.document;
            var TemplateContent = HtmlEncode($(Contentiframebody).find("body").attr("innerHTML"));
            hcTemplate.setText(TemplateContent);
            return true;
        }

        function setInnerHtml() {
            var Titleiframebody = $("#HtmlEditorTitle")[0].contentWindow.document;
            var TemplateTitle = HtmlDecode(hcTemplate.getValue());
            $(Titleiframebody).find("body").attr("innerHTML", TemplateTitle)

            var Contentiframebody = $("#HtmlEditorContent")[0].contentWindow.document;
            var TemplateContent = HtmlDecode(hcTemplate.getText());
            $(Contentiframebody).find("body").attr("innerHTML", TemplateContent);
            return true;
        }

        function HtmlEncode(input) {
            var converter = document.createElement("DIV");
            converter.innerText = input;
            var output = converter.innerHTML;
            converter = null;
            return output;
        }

        function HtmlDecode(input) {
            var converter = document.createElement("DIV");
            converter.innerHTML = input;
            var output = converter.innerText;
            converter = null;
            return output;
        }
</script>
</asp:Content>
