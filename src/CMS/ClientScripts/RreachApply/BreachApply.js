function ShowPic() {
    var imglist = new Array();
    var srclist = new Array();
    var strlist = new Array();
    var index = 0;
    var mullist = $("div[id*='multiUploader_fileList']");
    var upllist = $("div[id*='uploader_fileList']");
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
        var windowWidth = window.innerWidth-100;
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