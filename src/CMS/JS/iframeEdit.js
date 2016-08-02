//设置编辑模式
function setDesignModeOn(iframeId) {
    //frame
    var thisiframe = $("#" + iframeId);
    //ducument
    var _RichEditor = $(thisiframe.attr("contentDocument"));

    _RichEditor.attr("designMode", "on");

    $(_RichEditor).click(function () {
        $("iframe").each(function () {
            $(this).attr("class", "off");
        });
        $(thisiframe).attr("class", "on");
    });
}

//点击树添加按钮
function TreeAddParamer(s, e) {
    $("iframe").each(function () {
        if ($(this).attr("class") == "on") {
            AddParamer($(this).attr("id"), e.Text);
        }
    });
//    e.Value,
//    e.Text
}

function AddParamer(iframeId,value) {
    var html = "<input id=\"Button1\" type=\"button\" value=\"" + value + "\"/>";

    if(getBrowser()=='ie'){
        var Editor = window.frames[iframeId];   //IE获取iframe方法，否则图片位置跑到页面顶上去了。    
        Editor.focus();    
        o=Editor.document.selection.createRange();    
        o.pasteHTML(html);    
   }else if(getBrowser()=='chrome'){
        var Editor = document.getElementById(iframeId);
        Editor.focus();
        var rng = Editor.contentWindow.getSelection().getRangeAt(0);  
        var frg = rng.createContextualFragment(html);    
        rng.insertNode(frg);  
    }  
}

//获取浏览器版本    
function getBrowser(){    
    var agentValue = window.navigator.userAgent.toLowerCase();    
    if(agentValue.indexOf('msie')>0){    
        return "ie";    
    }else if(agentValue.indexOf('firefox')>0){    
        return "ff";    
    }else if(agentValue.indexOf('chrome')>0){    
        return "chrome";    
    }
}

