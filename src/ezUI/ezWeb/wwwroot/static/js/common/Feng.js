var Feng = {
    ctxPath: "",
    addCtx: function (ctx) {
        if (this.ctxPath == "") {
            this.ctxPath = ctx;
        }
    },
    confirm: function (tip, ensure) {//询问框
        layer.confirm(tip, {
            btn: ['确定', '取消']
        }, function (index) {
            ensure();
            layer.close(index);
        }, function (index) {
            layer.close(index);
        });
    },
    confirmDelete: function (tip, ensure) {//询问框
        layer.confirm(tip, {
            btn: ['确定删除', '取消']
            ,icon:2
            ,title:'数据删除确认'
        }, function (index) {
            ensure();
            layer.close(index);
        }, function (index) {
            layer.close(index);
        });
    },
    log: function (info) {
        console.log(info);
    },
    alert: function (info, iconIndex) {
    	layer.alert(info, {
            icon: iconIndex
        });
    },
    info: function (info, iconIndex, endFun) {
        layer.msg(info, {icon: iconIndex, time:2000}, endFun);
    },
    success: function (info, endFun) {
        Feng.info(info, 1, endFun);
    },
    error: function (info, endFun) {
        Feng.info(info, 2, endFun);
    },
    infoDetail: function (title, info) {
        var display = "";
        if (typeof info == "string") {
            display = info;
        } else {
            if (info instanceof Array) {
                for (var x in info) {
                    display = display + info[x] + "<br/>";
                }
            } else {
                display = info;
            }
        }
        layer.open({
            title: title,
            type: 1,
            skin: 'layui-layer-rim', //加上边框
            area: ['950px', '600px'], //宽高
            content: '<div style="padding: 20px;">' + display + '</div>'
        });
    },
    writeObj: function (obj) {
        var description = "";
        for (var i in obj) {
            var property = obj[i];
            description += i + " = " + property + ",";
        }
        layer.alert(description, {
            skin: 'layui-layer-molv',
            closeBtn: 0
        });
    },
    showInputTree: function (inputId, inputTreeContentId) {
        var onBodyDown = function (event) {
            if (!(event.target.id == "menuBtn" || event.target.id == inputTreeContentId || $(event.target).parents("#" + inputTreeContentId).length > 0)) {
                $("#" + inputTreeContentId).fadeOut("fast");
                $("body").unbind("mousedown", onBodyDown);// mousedown当鼠标按下就可以触发，不用弹起
            }
        };

        var inputDiv = $("#" + inputId);
        var inputDivOffset = $("#" + inputId).offset();

        $("#" + inputTreeContentId).css({
            left: inputDivOffset.left + "px",
            top: inputDivOffset.top + inputDiv.outerHeight() + "px"
        }).slideDown("fast");

        $("body").bind("mousedown", onBodyDown);
    },
    baseAjax: function (url, tip) {
        var ajax = new $ax(Feng.ctxPath + url, function (data) {
            Feng.success(tip + "成功!");
        }, function (data) {
            Feng.error(tip + "失败!" + data.responseJSON.message + "!");
        });
        return ajax;
    },
    changeAjax: function (url) {
        return Feng.baseAjax(url, "修改");
    },
    zTreeCheckedNodes: function (zTreeId) {
        var zTree = $.fn.zTree.getZTreeObj(zTreeId);
        var nodes = zTree.getCheckedNodes();
        var ids = "";
        for (var i = 0, l = nodes.length; i < l; i++) {
            ids += "," + nodes[i].id;
        }
        return ids.substring(1);
    },
    eventParseObject: function (event) {//获取点击事件的源对象
        event = event ? event : window.event;
        var obj = event.srcElement ? event.srcElement : event.target;
        return $(obj);
    },
    showSelectTree : function(posid) {
    	// 下拉选择框
        var stfObj = $("#"+posid);
        var stfPosition = $("#"+posid).offset();
        $("#menuContent").css({
            left: stfPosition.left + "px",
            top: stfPosition.top + stfObj.outerHeight() + "px"
        }).slideDown("fast");

        $("body").bind("mousedown", onBodyDown);	
    },
    hideSelectTree : function() {
        $("#menuContent").fadeOut("fast");
        $("body").unbind("mousedown", onBodyDown);// mousedown当鼠标按下就可以触发，不用弹起
    },
    openWin : function(title, url) {
        var index = layer.open({
            type: 2,
            title: title,
            area: ['80%', '80%'], //宽高
            fix: false, //不固定
            maxmin: true,
            scrollbar: false,
            content: Feng.ctxPath + url
        });
        return index;
    },
};
function onBodyDown(event) {
    if (!(event.target.id == "menuBtn" 
    	|| event.target.id == "menuContent" 
    		|| $(event.target).parents("#menuContent").length > 0)) {
    	Feng.hideSelectTree();
    }    	
}
