


//(function ($) {
//    $.fn.myAccordion = function () {
//        this.each(function () {
//            var accordion = $(this);
//            accordion.addClass("ui-accordion ui-widget ui-helper-reset ui-accordion-icons");
//            var span = $("<span class=\"ui-icon ui-icon-triangle-1-e\"></span>");
//            accordion.children("h3").addClass("ui-accordion-header ui-helper-reset ui-state-default ui-corner-top")
//                        .prepend(span)
//                        .mouseover(function () {
//                            $(this).toggleClass("ui-state-hover");
//                        })
//                        .mouseout(function () {
//                            $(this).toggleClass("ui-state-hover");
//                        })
//                        .click(function () {
//                            if ($(this).hasClass("isopen") == false) {
//                                //变自己
//                                ToggleAccordion($(this));

//                                //变别人
//                                ToggleAccordion(accordion.children(".isopen").not($(this)));
//                            }
//                        });
//            accordion.children("div").hide().addClass("ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active")
//                        .first().prev().trigger("click"); //展开第一个
//        });
//    };

//    ToggleAccordion = function (controls) {
//        controls.each(function () {
//            $(this).toggleClass("ui-state-active isopen");
//            $(this).next().toggle("blind", 300);
//            $(this).find(".ui-icon").toggleClass("ui-icon-triangle-1-s").toggleClass("ui-icon-triangle-1-e");
//        });
//    };

//})(jQuery);


//屏蔽退格键(Backspace)，可输入文本框除外
$(document).keydown(function (e) {
    var doPrevent;
    var isJump = false; //是否分页跳转
    var actElement = document.activeElement;
    var focusControl;
    if (actElement != null) {
        focusControl = document.activeElement.id//焦点控件ID
        if (focusControl.indexOf("GoPage") > 0) {
            isJump = true;
        }
    }
    if (e.keyCode == 8 || e.keyCode == 13) {
        if (e.keyCode == 13) {//支持Enter功能，默认为查询，如果输入了页面，则按照页码查询相应的页
            if (isJump) {
                if ($(".grid_Pager_bar").children("a").last().length > 0) {
                    ClickElem($(".grid_Pager_bar").children("a").last()[0]);
                }
            }
            else {
                if ($(".btnQuery").length > 0) {
                    ClickElem($(".btnQuery")[0]);
                }
            }
        }
        var d = e.srcElement || e.target;
        if (d.tagName.toUpperCase() == 'INPUT' || d.tagName.toUpperCase() == 'TEXTAREA') {
            doPrevent = d.readOnly || d.disabled;
        }
        else {
            doPrevent = true;
        }
    }
    else
        doPrevent = false;

    if (doPrevent)
        e.preventDefault();
});

//模拟后台点击事件
function ClickElem(element) {
    if (element != null) {
        if (document.all && typeof (document.all) == "object")   //IE
        {
            element.click();
        }
        else {
            if (element.tagName.toLowerCase() == "a") {
                eval(element.href);
            }
            else if (element.tagName.toLowerCase() == "input") {
                eval("element.click()");
            }
            else {
                var e = document.createEvent('MouseEvent');
                e.initEvent('click', true, false);
                elemID.dispatchEvent(e);
            }
        }
    }
}


function windowHeight() {
    var de = document.documentElement;
    return self.innerHeight || (de && de.clientHeight) || document.body.clientHeight;
}
//        window.onload = window.onresize = function () {
//            initMenu();
//        }

function Login() {
    var account = txtLogin.getValue();
    var pwd = txtPassword.getValue();
    cbcLogin.callBack({ Account: account, Password: pwd });
}

function LoginSuccess(data) {
    if (data == "0") {
        pcLogin.close();
    }
    else {
        alert(data);
    }
}

function LoginError(data) {
    alert(data);
}

//显示进度，IsValidate代表是否需要进行表单验证(界面上的必填项等)，一般保存按钮需要验证
function ShowWaiting(IsValidate) {
    if (IsValidate) {
        if (Validate()) {
            $.blockUI(
            //                    { message: '<iframe src="../Loading.htm"></iframe>',
            //                        css: {
            //                            height: '50px',
            //                            padding: '15px',
            //                            border: 'none',
            //                            '-webkit-border-radius': '10px',
            //                            '-moz-border-radius': '10px'
            //                        }
            //                    }

                    );
            return true;
        }
        else {
            return false;
        }
    }
    else {
        $.blockUI(
        //                { 
        //                    
        //                    message: '<iframe src="../Loading.htm"></iframe>',
        //                    css: {
        //                        height: '50px',
        //                        padding: '15px',
        //                        border: 'none',
        //                        '-webkit-border-radius': '10px',
        //                        '-moz-border-radius': '10px'
        //                    }
        //                }
                );
        return true;
    }
}

//加上按钮样式
function ButtonStyle() {
    //            $(':button').button();
    //            $(':submit').button();
}

//Tab回调完成
function OnTabSelectedIndexComplete(data) {

}
//回调出错
function OnTabSelectedIndexError(msg) {
    alert(msg);
}

//点击弹出框的确定
function OnTabSelectedIndexChange(s, e) {
    //获取Tab页选中
    var item = {
        TabSelectIndex: e.Value
    };
    //回调
    cbcTabSelectedIndex.callBack(item);
}

function qiehuan(num) {
    $(".master_menu_h > .menu_father > li > a").removeClass("nav_on");
    $(".master_menu_h > .menu_son > div").hide();

    $(".master_menu_h #qh_con" + num).show();
    $(".master_menu_h #mynav" + num).addClass("nav_on");
}

Sys.Application.add_load(function () {
    try {
        TabSelectIndex.set_onSelectChanged(function (s, e) {
            cbcTabSelectedIndex.callBack(e);
        });
    }
    catch (e) { }

    if ($("body").data("isPostBack") == null) {
        $("body").data("isPostBack", true);

        window.__doPostBackOriginal = window.__doPostBack;
        window.__doPostBack = function (target, args) {
            window.__doPostBackOriginal(target, args);
            if (target.toLowerCase().indexOf("export") < 0) {
                ShowWaiting(false);
            }
        }
    }

    doMessageActions();
});

//查询框相关
Sys.Application.add_load(function () {
    if ($("#queryCondition").length > 0) {

        $("#queryCondition").toggle(
            function () {
                $("#spIsCanShrink").html("-");
                $("#moreCondition").css("display", "block");
                hcMoreCondition.setValue("1");
            },
            function () {
                $("#spIsCanShrink").html("+");
                $("#moreCondition").css("display", "none");
                hcMoreCondition.setValue("0");
            }
        );
        $("#moreCondition").css("display", "none");

        if (hcMoreCondition.getValue() == "1") {
            $("#queryCondition").trigger("click");
        }
    }
});


//2012-7-13 zhumin调整，将手风琴的初始化移到此事件中
//Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
//    $(".accordionzdj").myAccordion();
//});

Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ShowErrorOfEndRequest);
Sys.WebForms.PageRequestManager.prototype._createPageRequestManagerTimeoutError = ShowErrorOfTimeOut;

function ShowErrorOfEndRequest(sender, args) {
    if (args.get_error() != undefined && args.get_error().httpStatusCode == '500') {
        var errorMessage = args.get_error().message;
        //    var Ex = args.get_error();
        //    var errorMessage = Ex.description;
        args.set_errorHandled(true);
        alert(errorMessage);
    }
    $.unblockUI();
}

function ShowErrorOfTimeOut(sender, eventArgs) {
    alert("与服务器的连接超时，请检查网速是否太慢或网络是否断线");
}


function ShowPic() {
    var imglist = new Array();
    var srclist = new Array();
    var strlist = new Array();
    var index = 0;
    var mullist = $("div[id*='multiUploader_fileList']");
    var upllist = $("div[id*='uploader_fileList']");
   

    var muldivlist = $("div[id*='multiUploader_fileList'] div");
    for (var i = 0; i < muldivlist.length; i++) {
        var txtt=
        "<img src='/UploadControl.ashx?type=showImage&amp;attachId=62340797' id='ctl00_ContentPlaceHolder1_txtAuctionContractSF_thumbnail' width='30' height='30'>"
        $(muldivlist[i]).append();
    }

    var contractList = $("[id*='_thumbnail']");
        for (var i = 0; i < contractList.length; i++) {
            $(contractList[i]).click(function () {
                imglist = [];
                srclist = [];
                strlist = [];
                var imglist = $(contractList).find("img");
                for (var i = 0; i < imglist.length; i++) {
                    srclist[i] = $(imglist[i]).attr("src");
                    strlist[i] = $(imglist[i]).parent().text().split("(")[0];
                }
                var src = $(this).attr("src");
                var str = $(this).parent().text().split("(")[0];
                $("#showPicList").css("display", "block");
                setImg($(".fancybox-inner"), src, str);
            });
        }
    for (var i = 0; i < upllist.length; i++) {
        $(upllist[i]).bind('DOMNodeInserted', function (e) {
            $(this).find("img").click(function () {
                imglist = [];
                srclist = [];
                strlist = [];
                var imglist = $(upllist).find("img");
                for (var i = 0; i < imglist.length; i++) {
                    srclist[i] = $(imglist[i]).attr("src");
                    strlist[i] = $(imglist[i]).parent().text().split("(")[0];
                }
                var src = $(this).attr("src");
                var str = $(this).parent().text().split("(")[0];
                $("#showPicList").css("display", "block");
                setImg($(".fancybox-inner"), src, str);

            });
        });
    }
    mullist.bind('DOMNodeInserted', function (e) {
        $(this).find("img:last").click(function (e) {
            imglist = [];
            srclist = [];
            strlist = [];
            var imglist = $(mullist).find("img");
            for (var i = 0; i < imglist.length; i++) {
                srclist[i] = $(imglist[i]).attr("src");
                strlist[i] = $(imglist[i]).parent().text().split("(")[0];
            }
            index = srclist.indexOf($(this).attr("src"));
            $("#showPicList").css("display", "block");
            setImg($(".fancybox-inner"), srclist[index], strlist[index]);
        });
    });
    //Previous
    $(".fancybox-prev").click(function () {
        var cnt = srclist.length
        if (cnt > 0) {
            if (index > 0) {
                index = index - 1;
                setImg($(".fancybox-inner"), srclist[index], strlist[index]);
            }
        }
    });
    //Next
    $(".fancybox-next").click(function () {
        var cnt = srclist.length
        if (cnt > 0) {
            if (index < (cnt - 1)) {
                index = index + 1;
                setImg($(".fancybox-inner"), srclist[index], strlist[index]);
            }
        }
    });
    //close
    $(".fancybox-close").click(function () {
        $("#showPicList").css("display", "none");
    });
    $("#showPicList").dblclick(function (event) { $("#showPicList").css("display", "none"); });
    $(document).keyup(function (event) {
        switch (event.keyCode) {
            case 27:
            case 96:
                $("#showPicList").css("display", "none");
        }
    });
    //设置图片
    function setImg(obj, src, str) {
        var img = new Image();
        img.src = src;
        $(obj).html("<img class=\"fancybox-image\" src=\"\" alt=\"\">");
        $(obj).find(".fancybox-image").attr("src", src);

        $(obj).parents('.fancybox-skin').find(".child").text(str);

        var imgEntity = $(obj).find(".fancybox-image")[0];

        var windowHeight = window.innerHeight - 100;
        var windowWidth = window.innerWidth - 100;
        var popupHeight = img.height;
        var popupWidth = img.width;

        //设置高度
        if (windowHeight < popupHeight) {
            //缩放比例
            var hp = windowHeight / popupHeight;
            popupHeight = windowHeight;
            popupWidth = popupWidth * hp;
        }
        if (windowWidth < popupWidth) {
            var wp = windowWidth / popupWidth;
            popupWidth = windowWidth;
            popupHeight = popupHeight * wp;
        }
        $(imgEntity).css({ "width": popupWidth, "height": popupHeight }); //设置缩放后的宽度和高度 
        $(imgEntity).parents(".fancybox-wrap").css({
            "margin-top": (windowHeight - popupHeight) / 2,
            "margin-left": (windowWidth - popupWidth) / 2
        });
    }
};