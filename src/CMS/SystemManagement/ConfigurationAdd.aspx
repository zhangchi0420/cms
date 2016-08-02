<%@ Page Title="配置设置" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfigurationAdd.aspx.cs" Inherits="Drision.Framework.Web.SystemManagement.ConfigurationAdd" %>
<%@ Import Namespace="Drision.Framework.Web.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            StarAlign();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(StarAlign);
        });
        function StarAlign() {
            var alian = '<%=AppSettings.PageFieldRedStarAlign %>';
            $(".left_star").addClass(alian + "_star");
            $(".right_star").removeClass("left_star");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <h3>
        配置信息</h3>
        <div class="allcol2">
            <div class="item_box_col1">
                <span>配置标题</span><span class="left_star"></span>
                <asp:TextControl ID="tbT_Configuration_Title" runat="server" CssClass="item_input"
                    TextType="String" FieldName="Configuration_Title" />
            </div>
            <div class="item_box_col1">
                <span>配置键 </span><span class="left_star">*</span>
                <asp:TextControl ID="tbT_Configuration_Key" runat="server" CssClass="item_input"
                    TextType="String" FieldName="Configuration_Key" IsRequired="True" />
            </div>
            <div class="item_box_col1">
                <span>配置值 </span><span class="left_star">*</span>
                <asp:TextControl ID="tbT_Configuration_Value" runat="server" CssClass="item_input"
                    TextType="String" FieldName="Configuration_Value" IsRequired="True" />
            </div>
            <div class="item_box_full">
                <span>配置描述 </span><span class="left_star"></span>
                <asp:TextControl ID="tbT_Configuration_Description" runat="server" CssClass="item_input"
                    TextType="Text" FieldName="Configuration_Description" />
            </div>
            <div class="cl">
        </div>
        </div>
         <div class = "cl"></div>
         
    </div>
    <div class="button_bar">
             <asp:LinkButton ID="btnSave" runat="server" OnClientClick="return Validate();">保存</asp:LinkButton>
                     <asp:LinkButton ID="btnReturn" runat="server"  UseSubmitBehavior = "false" Text="返回"  OnClientClick="javascript:history.go(-1)"/>
         </div>
</asp:Content>
