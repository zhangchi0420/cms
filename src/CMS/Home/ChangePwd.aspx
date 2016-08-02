<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePwd.aspx.cs"
    Inherits="Drision.Framework.Web.Home.ChangePwd" Title="修改密码" %>
<%@ Import Namespace="Drision.Framework.Web.Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function Compare(twoPwd) {
            var onePwd = txtNewPwd.getText();
            if (twoPwd == onePwd) {
                return true;
            }
            return false;
        }

        function DoAjax(type, value) {
            var ret = false;
            $.ajax({
                type: 'POST',
                url: 'ChangePwd.aspx',
                async: false, //同步
                data: { Type: type, Value: value },
                success: function (data) {
                    if (data.substring(0, 4) == 'true') {
                        ret = true;
                    }
                }
            });
            return ret;
        }

        function ValidateOld(oldPwd) {
            return DoAjax("OldPwd", oldPwd);
        }

        function ValidateNew(newPwd) {
            return DoAjax("NewPwd", newPwd);
        }

        function Save() {
            var ret = bValidator.validate();
            if (ret == true) {
                var newPwd = txtNewPwd.getText();
                cbc.callBack(newPwd, function (data) {
                    if (data == "true") {
                        alert("修改成功");
                    }
                    else {
                        alert(data);
                    }
                });
            }
            return false;
        }
        $(document).ready(function () {
            var alian = '<%=AppSettings.PageFieldRedStarAlign %>';
            $(".left_star").addClass(alian + "_star");
            $(".right_star").removeClass("left_star");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
        <div class="allcol1">
            <div class="h20">
            </div>
            <div class="item_box_col1">
                <span>旧 密 码</span><span class="left_star">*</span>
                <asp:TextControl ID="txtOldPwd" runat="server" MaxLength="50" TextType="Password"
                    IsRequired="true" ValidateFunction="ValidateOld" ValidateMessage="旧密码不正确" />
            </div>
            <div class="item_box_col1">
                <span>新 密 码</span><span class="left_star">*</span>
                <asp:TextControl ID="txtNewPwd" runat="server" MaxLength="50" TextType="Password"
                    IsRequired="true" ValidateFunction="ValidateNew" ClientInstanceName="txtNewPwd" ValidateMessage="密码不符合复杂性要求" />
            </div>
            <div class="item_box_col1">
                <span>确认密码</span><span class="left_star">*</span>
                <asp:TextControl ID="txtRepeatPwd" runat="server" MaxLength="50" TextType="Password"
                    IsRequired="true" ValidateFunction="Compare" ValidateMessage="二次密码输入不一致"></asp:TextControl>
            </div>
            <div class="cl">
            </div>
        </div>
    </div>
    <div class="button_bar">
        <span class="button_blue">
            <asp:LinkButton ID="btnOK" runat="server" Text="保存" OnClientClick="return Save();"
                ></asp:LinkButton></span> <span class="button_blue"><a href="HomePage.aspx">
                    返回</a></span>
    </div>
    <div class="cl">
    </div>
    <asp:CallBackControl ID="cbc" runat="server" ClientInstanceName="cbc" OnCallBack="cbc_CallBack" />
</asp:Content>
