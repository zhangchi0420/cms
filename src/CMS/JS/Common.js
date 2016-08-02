////下载超链接
//function DownLoadOpen(s) {
//    var id = $(s).attr("dataValue");
//    window.location.href = "../DownLoadPage.aspx?id=" + id;
//    return false;
//}

//导出
function Export() {
    eval(hcExport.getValue());
    return false;
};

//验证
function Validate() {
    var result = false;
    if (bValidator != undefined && $.isFunction(bValidator.validate)) {
        result = bValidator.validate(); //先进行可能的验证
        if (result == false) {
            EnableButton(); //验证失败，清空计数器，等待下一次验证
        }
        else {
            result = DisableButton(); //验证成功，根据计数器判断是否回发
        }
    }
    else {
        result = DisableButton(); //无验证，直接根据计数器判断是否回发
    }
    return result;
}

//验证Group
function ValidateGroup(group) {
    var result = false;
    if (bValidator != undefined && $.isFunction(bValidator.validate)) {

        var groupItems = $("body").data(group);
        if (groupItems == undefined) {
            result = true;
        }
        else {
            var temp = true;
            $.each(groupItems, function (i, p) {
                var temp1 = bValidator.validate(false, p); //先进行可能的验证
                temp = temp1 && temp;
            });
            result = temp;
        }

        if (result == false) {
            EnableButton(); //验证失败，清空计数器，等待下一次验证
        }
        else {
            result = DisableButton(); //验证成功，根据计数器判断是否回发
        }
    }
    else {
        result = DisableButton(); //无验证，直接根据计数器判断是否回发
    }
    return result;
}


function EnableButton() {
    $("body").data("btnCount", 0);
}

function DisableButton() {
    var count = $("body").data("btnCount");
    if (count == undefined) {
        count = 1;
        $("body").data("btnCount", count);
        return true;
    }
    else {
        count = count + 1;
        $("body").data("btnCount", count);
        return count == 1;
    }
}

//打开权限共享
function OpenSharePrivilege(entityName, btn) {

    var id = $(btn).attr("dataValue");
    pcSharePrivilege.setUrl("../SystemManagement/SharePrivilege.aspx?entityName=" + entityName + "&objectId=" + id);
    pcSharePrivilege.open();
    return false;
}


