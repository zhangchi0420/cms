<%@ Page Title="流程设计第三步（活动权限配置）" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FormProcessDesigner_ThirdStep.aspx.cs" Inherits="Drision.Framework.Web.FormWorkflow.FormProcessDesigner_ThirdStep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .containerTable
        {
            width: 100%;
            border-collapse: collapse;
            min-height: 500px;
            margin-bottom: 20px;
        }
        .activityTd
        {
            width: 20%;
            vertical-align: top;
            border-right: 1px solid Gray;
        }
        .propertyTd
        {
            width: 80%;
            vertical-align: top;
            padding: 5px 20px 5px 20px;
        }
        .activity
        {
            width: 90%;
            margin: 10px 5% 10px 5%;
            text-align: center;
            vertical-align: middle;
            font-size: 20px;
            height: 80px;
            line-height: 80px;
            overflow: hidden;
        }
        .selected
        {
            border: 1px solid red;
        }
        .unselected
        {
            border: 1px dotted gray;
        }
    </style>
    <style type="text/css">
        .section
        {
            width:100%;
            margin: 10px 2% 20px 2%;            
            border-collapse:collapse;
        }
        
        .sectionHeader
        {
            background-color:#F0F0F0;
            font-weight:bold;
        }
        
        .section td
        {
            border:1px solid gray;
        }
        
        .field
        {
            width: 55%;            
            height: 30px;
            text-align: left;
            vertical-align: middle;
            font-size: 15px;            
        }
        .checkbox
        {
            width: 15%;            
            height: 30px;
            text-align:center;
            vertical-align:middle;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="containerTable">
        <tr>
            <td class="activityTd">
                <asp:Repeater ID="rActivity" runat="server">
                    <ItemTemplate>
                        <div onclick="SelectActivity(<%# Eval("ActivityId") %>);" class="activity <%# Eval("SelectClass") %>">
                            <%# Eval("ActivityName") %></div>
                    </ItemTemplate>
                </asp:Repeater>
            </td>
            <td class="propertyTd">
                <table class="section sectionHeader"><tr><td class="field">表单字段</td><td  class="checkbox">不可见</td><td class="checkbox">只读</td><td class="checkbox">可编辑</td></tr></table>
                <asp:Repeater ID="rSection" runat="server">
                    <ItemTemplate>                        
                        <table class="section">
                            <asp:Repeater ID="rField" runat="server" DataSource='<%# Eval("Fields") %>'>
                                <ItemTemplate>
                                    <tr>
                                        <td class="field"><asp:HiddenField ID="hf" runat="server" Value='<%# Eval("FormFieldId") %>' />
                                            <%# Eval("DisplayText") %>
                                        </td>
                                        <td class="checkbox">                                                                                    
                                            <asp:RadioButton ID="ccInVisible" runat="server" Checked='<%# Eval("Invisible") %>' GroupName='<%# Eval("FormFieldId") %>' />
                                        </td>
                                        <td class="checkbox">
                                            <asp:RadioButton ID="ccReadOnly" runat="server" Checked='<%# Eval("ReadOnly") %>' GroupName='<%# Eval("FormFieldId") %>' />
                                        </td>
                                        <td class="checkbox">
                                            <asp:RadioButton ID="ccReadWrite" runat="server" Checked='<%# Eval("ReadWrite") %>' GroupName='<%# Eval("FormFieldId") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <div class="cl">
                            </div>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
                <div class="button_bar">
                    <asp:LinkButton ID="btnSave" runat="server" Text="保存" OnClientClick="return ShowWaiting(false);" OnClick="btnSave_Click"></asp:LinkButton>
                    <asp:LinkButton ID="btnPrev" runat="server" Text="上一步" OnClick="btnPrev_Click"></asp:LinkButton>
                    <asp:LinkButton ID="btnNext" runat="server" Text="下一步" OnClick="btnNext_Click"></asp:LinkButton>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenControl ID="hc" runat="server" ClientInstanceName="hc" />
    <script type="text/javascript">
        function SelectActivity(id) {
            ShowWaiting(false);
            hc.setValue(id);
            eval(hc.getText());
        }
    </script>
</asp:Content>
