var alertMessage = null;
var scriptFunction = null;
var redirectUrl = null;
var confirmMessage = null;

function doMessageActions() {
    if (confirmMessage != null) {
        confirm(confirmMessage.replace(/@!@/g, '<br />'), function () {
            if (scriptFunction != null) {
                eval(scriptFunction);
                scriptFunction = null;
            }
        });
        confirmMessage = null;
    }
    else if (alertMessage != null) {
        alert(alertMessage.replace(/@!@/g, '<br />'));
        alertMessage = null;
    }
    else {
        if (scriptFunction != null) {
            eval(scriptFunction);
            scriptFunction = null;
        }
        if (redirectUrl != null) {
            window.location.href = redirectUrl;
            redirectUrl = null;
        }
    }
}


//2012-12-26 zhumin 修改了alert
var customMsgBoxId = "customMsgBox";
window.originalAlert = window.alert;
window.alert = function (value, title, width, height) {
    $.unblockUI(); //保险起见，把ShowWaiting的等待去掉
    var box = $("#" + customMsgBoxId);
    box.find("table td").html(value);
    if (title != undefined && title != null) {
        box.dialog("option", "title", title);
    }
    if (width != undefined && width != null) {
        box.dialog("option", "width", width);
    }
    if (height != undefined && height != null) {
        box.dialog("option", "height", height);
    }
    box.dialog("open");
}




//2013-1-8 zhumin 修改了confirm
var customConfirmBoxId = "customConfirmBox";
var customConfirmCallBack = null;
var customConfirmSubmitButton = null;
var customConfirmSubmitButtonOnClientClick = null;

window.originalConfirm = window.confirm;
window.confirm = function (value, callback, title, width, height) {
    $.unblockUI(); //保险起见，把ShowWaiting的等待去掉
    var target = event.target;

    //记住引发事件的submit按钮或a链接，以及现在的onclick（这种情况下认为是OnClientClick="return confirm("");"）
    if ($.browser.msie) {
        target = event.srcElement;
    }
    if (target && target.tagName && (
            (target.tagName.toUpperCase() == "INPUT" && $(target).attr("type") == "submit")
            ||
            target.tagName.toUpperCase() == "A")) {
        customConfirmSubmitButton = target;
        customConfirmSubmitButtonOnClientClick = $(customConfirmSubmitButton).attr("onclick");
    }
    else if (target && target.tagName && target.tagName.toUpperCase() == "SPAN" && $(target).hasClass("ui-button-text")) { //jquery的button
        customConfirmSubmitButton = target.parentElement;
        customConfirmSubmitButtonOnClientClick = $(customConfirmSubmitButton).attr("onclick");
    }

    //弹出
    var box = $("#" + customConfirmBoxId);
    box.find("table td").html(value);
    if (title != undefined && title != null) {
        box.dialog("option", "title", title);
    }
    if (width != undefined && width != null) {
        box.dialog("option", "width", width);
    }
    if (height != undefined && height != null) {
        box.dialog("option", "height", height);
    }
    //eval("customConfirmCallBack = " + callback);
    customConfirmCallBack = callback;
    box.dialog("open");
    return false;
}


$(document).ready(function () {
    $("body").append("<div id='" + customMsgBoxId + "'><table style='width:100%;height:100%'><tr><td style='text-align:center;font-size:14px;'></td></tr></table></div>");
    $("#" + customMsgBoxId).dialog({
        bgiframe: true, //兼容IE6
        autoOpen: false, //非自动打开
        modal: true, //模式窗口
        resizable: false, //不可改变大小
        width: 400,
        height: 150,
        title: "提醒",
        buttons: {
            "关闭": function () {
                $(this).dialog("close");
            }
        },
        close: function (event, ui) {
            if (scriptFunction != null) {
                eval(scriptFunction);
                scriptFunction = null;
            }
            if (redirectUrl != null) {
                window.location.href = redirectUrl;
                redirectUrl = null;
            }
            return false;
        }
    });

    $("body").append("<div id='" + customConfirmBoxId + "'><table style='width:100%;height:100%'><tr><td style='text-align:center;font-size:14px;'></td></tr></table></div>");
    $("#" + customConfirmBoxId).dialog({
        bgiframe: true, //兼容IE6
        autoOpen: false, //非自动打开
        modal: true, //模式窗口
        resizable: false, //不可改变大小
        width: 400,
        height: 150,
        title: "确认",
        buttons: {
            "确定": function () {
                //回调
                if (customConfirmCallBack != null) {
                    customConfirmCallBack();
                    customConfirmCallBack = null;
                }
                $(this).dialog("close");

                //如果由按钮触发
                if (customConfirmSubmitButton != null) {
                    var temp = customConfirmSubmitButton;
                    customConfirmSubmitButton = null;

                    temp.onclick = null; //移除onclick

                    //模拟点击
                    if ($.browser.msie) {
                        temp.click();
                    }
                    else {
                        var e = document.createEvent('MouseEvent');
                        e.initEvent('click', false, false);
                        temp.dispatchEvent(e);
                    }

                    eval("temp.onclick = " + customConfirmSubmitButtonOnClientClick); //重新加上onclick
                }


            },
            "取消": function () {
                $(this).dialog("close");
            }
        }
    });
});