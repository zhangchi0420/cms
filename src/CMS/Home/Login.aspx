<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Drision.Framework.Web.Home.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html style="background: #CCC url(../images/login_bg.png) center" xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />--%>
    <title>业务系统登录</title>
    <link href="../class/apple/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="../class/base.css" rel="stylesheet" type="text/css" />
    <link href="../class/common.css" rel="stylesheet" type="text/css" />
    <link href="../template_apple/template.css" rel="stylesheet" type="text/css" id="template" />
    <link href="../class/page.css" rel="stylesheet" type="text/css" id="frame" />
    <script type="text/javascript" src="../js/jquery-1.6.2.min.js"></script>
    <script src="../js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../js/Jcookie.js" type="text/javascript"></script> 
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/Common.js" type="text/javascript"></script>
    <script src="../js/jAlert.js" type="text/javascript"></script>         
    <link href="../class/metro.css" rel="stylesheet" type="text/css" />
    <!--[if lt IE 9]>
<script src="js/html5.js"></script>
<script src="js/PIE.js"></script>
<![endif]-->
    <!-- jquerytools -->
    <!--[if lte IE 9]>
<link rel="stylesheet" media="screen" href="class/ie.css" />
<script type="text/javascript" src="js/ie.js"></script>
<![endif]-->    
</head>
<script type="text/javascript">
    function show(sr) {
        document.getElementById("login").href = sr + ".css";
    }
    function show1(sr1) {
        document.getElementById("frame").href = sr1 + ".css";
    }
    var login = function () { };
    /*判断是否按下了enter键*/
    login.isEnter = function (ev) {
        ev = ev || window.event;
        var code = (ev.keyCode || ev.which);
        return (code == 10 || code == 13);
    };
    $(function () {
        $("form").keypress(function (ev) {
            if (login.isEnter(ev)) {
                return bValidator.validate();
            }
        });
    });


    function Forgetpwd() {
        var usercode = txtUserCode.getValue();
        var email = txtEMail.getValue();
        cbcForgetpwd.callBack({ UserCode: usercode, Email: email });
    }

    function ForgetpwdSuccess(data) {
        if (data == "已将新密码发到您的邮箱！") {
            alert(data);
            pcForgetpwd.close();
        }
        else {
            alert(data);
        }
    }

    function ForgetpwdError(data) {
        alert(data);
    }    
</script>
<body>
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="sm" runat="server">
    </asp:ScriptManager>
    <center> 
        <div class="metro_login">
            <div class="top_title">
                <div class="top_title_name">
                    <div class="logo">
                        <%--<img id="img1" src="../images/login_logo.png" style="border-width: 0px;">--%>
                        重庆海关加工贸易废料交易系统
                        </div>
                    <%--<span>
                        <%= this.WebSiteName %></span>--%>
                </div>
            </div>
            <div class="system_size">
                <!--系统名称开始 -->
                <!--系统名称结束 -->
                <div style="height: 100px">
                </div>
                <div style=" width:1100px; margin:auto;">
                <div class="login_photo">
                    <img src="../images/ei2_login_bg.png" /></div>
                    <div style=" float:left; width:500px;"><div style="float: left;margin-left:80px;">
                    <img src="../images/welcome_bg.png" width="370" height="150" /></div>
                <div class="login_main">
                    <h3>
                        用户登录</h3>
                    <!-- 表单开始 -->
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                        
                    <div class="login_form">
                        <div class="item_box_full">
                            <span>用户名:</span>
                            <asp:TextControl ClientInstanceName="txtLogin" ID="txtLogin" runat="server" Width="200px" MaxLength="50" IsRequired="true" ValidateMessage="请正确输入用户名" />
                        </div>
                        <div class="item_box_full">
                            <span>密 码:</span>
                            <asp:TextControl ClientInstanceName="txtPwd" ID="txtPwd" runat="server" MaxLength="50" Width="200px" TextType="Password" IsRequired="true" ValidateMessage="请正确输入密码" />
                        </div>
                        <div class="item_box_full" style="margin-bottom: 30px">
                            <span></span>
                            <div class="item_input">
                                <asp:CheckBox ID="loginRemind" Checked="false" runat="server" /></div>
                            <div class="item_text">
                                记住密码</div>
                        </div>
                        <div class="button_bar" style="margin-bottom: 20px">
                                <asp:Button ID="button1" Text="登录" OnClick="buttonLogin_Click" runat="server"  />
                                <%--<asp:Button ID = "btnForgetPwd" Text = "忘记密码" runat = "server"  OnClientClick = "pcForgetpwd.open();return false;" />--%>
                        </div>
                        <div class="cl">
                        </div>
                    </div>



                    <asp:CallBackControl ID="cbcForgetpwd" runat="server" ClientInstanceName="cbcForgetpwd" CallBackType="JSON"
                        OnCallBack="cbcForgetpwd_CallBack" OnComplete="ForgetpwdSuccess" OnError="ForgetpwdError" />
                    <asp:PopupControl Height="220px" ID="pcForgetpwd" ClientInstanceName="pcForgetpwd" runat="server"
                        Title="忘记密码">
                        <PopupContent ID="pContentForgetpwd" runat="server">
                            <div class="login_form">
                                <div class="item_box_full">
                                    <span>用户名:</span>
                                <asp:TextControl ClientInstanceName="txtUserCode" ID="txtUserCode" runat="server" Width="200px" TextType="String" />
                                </div>
                                <div class="item_box_full">
                                    <span>邮 箱:</span>
                                <asp:TextControl ClientInstanceName="txtEMail" ID="txtEMail" runat="server" Width="200px" TextType="String" />
                                </div>
                            </div>
                            <div class="cl">
                            </div>
                        </PopupContent>
                        <Buttons>
                            <asp:PopupButton Text="确定" OnClick="Forgetpwd" />
                        </Buttons>
                    </asp:PopupControl>





                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <!-- 表单结束 -->
                    <div style="border-top: 1px solid #999; color: #666; line-height: 40px">
                        如果忘记密码或无法对登录请联系管理员</div>
                </div></div></div>
                <div class="cl"></div>
                <div class="footer" style=" display:none;">
                    <span></span><span style="font-family: verdana">迪锐信科技 版权所有</span>
                </div>
            </div>
        </div>
    </center>





    <script type="text/javascript">
        $(document).ready(function () {
            var UserCode = $.cookie('UserCode');
            var pwd = $.cookie('User_Password');
            var ischecked = $.cookie('isChecked');
            var c = $("#loginRemind");
            if (ischecked == "True") {
                c.attr("checked", "checked");
                $("#txtLogin_txt").val(UserCode);
                $("#txtPwd_txt").val(pwd);
            }
            else {
                c.attr("checked", null);
            }
        });

        Sys.Application.add_load(function () {
            $(txtLogin._container).keyup(function () {
                var UserCode = $("#txtLogin_txt").val();
                var CookUserCode = $.cookie('UserCode');
                var Cookpwd = $.cookie('User_Password');
                var Cookischecked = $.cookie('isChecked');
                if (UserCode != CookUserCode) {
                    $("#txtPwd_txt").val(null);
                    $("#loginRemind").attr("checked", null);
                }
                else {
                    if (Cookischecked == "True") {
                        $("#loginRemind").attr("checked", "checked");
                        $("#txtPwd_txt").val(Cookpwd);
                    }
                }
            });
        });
    </script>
    <script src="../ClientScripts/Site.Master.js" type="text/javascript"></script>
    </form>
</body>
</html>
