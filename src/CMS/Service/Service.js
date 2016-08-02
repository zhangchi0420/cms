/// <reference path="../js/jquery-1.5.1-vsdoc.js" />




//全局js调后台服务接口
//汤晓华 2011 09 21
JsService = {
    //获取json数据
    getJson: function (url, parmas, custError) {
        var res = null;
        $.ajax({
            type: "POST",
            url: url,
            data: parmas,
            async: false,
            success: function (data) {
                res = jQuery.parseJSON(data);
                if (!res.Success) {
                    if (custError == null)
                        JsService.onError(res.Message);
                    else
                        custError(res.Message);
                }
            }
        });
        if (res == null || res.Success == false) {
            return null;
        }
        return res;
    },

    //执行不带返回值的请求，如修改操作 删除操作等
    exec: function (url, parmas, success, custError) {
        var res = null;
        $.ajax({
            type: "POST",
            url: url,
            data: parmas,
            success: function (data) {
                res = jQuery.parseJSON(data);
                if (!res.Success) {
                    if (custError == null)
                        JsService.onError(res.Message);
                    else
                        custError(res.Message);
                }
                else {
                    if (success) {
                        success(res);
                    }
                }
            }
        });
    },
    //统一异常
    onError: function (error) {
    },
    //新增单个实体对象
    addEntity: function (entityName, entity, success, custError) {
        this.exec("/Service/JAdd.aspx", { entity: entity, ____entityName: entityName }, success, custError);
    },
    //删除单个实体对象
    deleteEntity: function (entityName, id, success, custError) {
        this.exec("/Service/JDelete.aspx", { id: id, ____entityName: entityName }, success, custError);
    },
    //更新单个实体对象
    udpateEntity: function (entityName, entity, success, custError) {
        this.exec("/Service/JUpdate.aspx", { entity: entity, ____entityName: entityName }, success, custError);
    },
    updateEntity: function (entityName, entity, success, custError) {
        this.exec("/Service/JUpdate.aspx", { entity: entity, ____entityName: entityName }, success, custError);
    },
    //获取单个实体对象
    getEntity: function (entityName, id, custError) {
        return this.getJson("/Service/JGet.aspx", { id: id, ____entityName: entityName }, custError);
    }
};

//统一异常处理
JsService.onError = function (msg) {
    alert("请求异常：" + msg);
}

$(document).ready(function () {

    $("#get").click(function () {


        var data = JsService.getEntity("T_Department", 1);
        alert(data.Result.Department_ID);

    });

    $("#del").click(function () {
        JsService.deleteEntity("T_Department", 2, function () {
            alert("删除成功");
        });
    });

    $("#upd").click(function () {

        var obj = { "Department_ID": "1", "Deportment_Encode": "001", "Department_Code": "002" };

        JsService.udpateEntity("T_Department", obj, function () {
            alert("更新成功");
        });

    });

    $("#add").click(function () {

        var obj = { "Deportment_Encode": "001", "Department_Code": "002" };

        JsService.addEntity("T_Department", obj, function () {
            alert("添加成功");
        });

    });

});

