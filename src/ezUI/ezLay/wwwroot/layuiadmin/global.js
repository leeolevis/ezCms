//加载Loading
var setT;
$(function () {
    document.onreadystatechange = subSomething;

    function subSomething() {
        if (document.readyState == "complete") { //当页面加载状态为完全结束时隐藏加载动画
            HideLoading(); //加载完后关闭
        }
    }
    setT = setTimeout(HideLoading, 1000);
});

//隐藏Loading
var HideLoading = function () {
    $('#loading').hide();
    clearTimeout(setT);
}

//iframe窗口关闭Layer
var CloseLayer = function () {
    //当你在iframe页面关闭自身时
    var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
    parent.layer.close(index); //再执行关闭 
}

//获取顶级Parent
var topWin = (function (p, c) {
    while (p !== c) {
        c = p;
        p = p.parent;
    }
    return c;
})(window.parent, window);

function goLogin(loginUrl) {
    topWin.location.href = loginUrl;
}

//父窗口
function ParentShowTip(msg, iconType) {
    parent.layer.msg(msg, {
        skin: 'layer-ext-moon demo-class1',
        icon: iconType,
        time: 1500,
        zIndex: 19892019
    });
}

function ParentShowTipLoading(msg) {
    var layerload = parent.layer.load(0, {
        time: 3000
    });
    parent.layer.msg(msg, function () { });
}

function ParentShowConfirm(msg, action) {
    parent.layer.confirm(msg, {
        skin: 'layer-ext-moon demo-class1',
        icon: 3,
        zIndex: 19891019
    }, function (index) {
        parent.layer.close(index);
        action();
    });
}

function ParentCloseMessage(isRefresh) {
    if (isRefresh == undefined) {
        isRefresh = 1;
    }
    setTimeout(ParentReload(isRefresh), 2000);
}

function ParentReload(isRefresh) {
    parent.layer.closeAll();
    if (isRefresh)
        parent.window.location.reload();
}

//当前页
function ShowTip(msg, iconType) {
    layer.msg(msg, {
        skin: 'layer-ext-moon demo-class1',
        icon: iconType,
        time: 1500,
        offset: 'auto',
        zIndex: 19892019
    });
}

function ShowTipLoading(msg) {
    var layerload = layer.load(0, {
        time: 8 * 1000
    });
    layer.msg(msg, function () { });
}

function ShowConfirm(msg, action) {
    layer.confirm(msg, {
        skin: 'layer-ext-moon demo-class1',
        icon: 3,
        zIndex: 19891019
    }, function (index) {
        layer.close(index);
        action();
    });
}

function CloseMessage(isRefresh) {
    if (isRefresh == undefined) {
        isRefresh = 1;
    }
    setTimeout(CurrentReload(isRefresh), 2000);
}

function CurrentReload(Refresh) {
    layer.closeAll();
    if (Refresh)
        window.location.reload();
}

function CloseIframe(msg) {

    if (msg.length != 0) {
        parent.layer.alert(msg, {
            icon: 1
        });
    }

    var zIndex = parent.layer.getFrameIndex(window.name);
    parent.layer.close(zIndex);
}

//下拉列表框绑定
function SelectBindById(Controller, id, CrotrolId, Key, Text, SelectKey, Form)//控制器(自定义),控件Id,值,内容,默认选中内容,重新加载
{
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/Select/" + Controller,
        data: { id: id },
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i][Key] == SelectKey)//判断是否是默认选中
                {
                    $('#' + CrotrolId).append("<option value='" + data[i][Key] + "' selected>" + data[i][Text] + "</option>");
                }
                else {
                    $('#' + CrotrolId).append("<option value='" + data[i][Key] + "'>" + data[i][Text] + "</option>");
                }
            }
            Form.render();
        }
    });
}

//下拉列表框绑定(传参)
function SelectBindParameter(Controller, CrotrolId, Key, Text, SelectKey, Form, dataParameter) //控制器(自定义),控件Id,值,内容,默认选中内容,重新加载
{
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/Select/" + Controller,
        data: {
            id: dataParameter
        },
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i][Key] === SelectKey) //判断是否是默认选中
                {
                    $("#" + CrotrolId).append("<option value='" + data[i][Key] + "' selected>" + data[i][Text] + "</option>");
                } else {
                    $("#" + CrotrolId).append("<option value='" + data[i][Key] + "'>" + data[i][Text] + "</option>");
                }
            }
            Form.render();
        }
    });
}

//获取字典
//SelectedMaxHeight   设置下拉框最大高度，超过高度，则出滚动条
function SelectDictionary(Code, CrotrolId, Key, Text, SelectKey, Form, SelectedMaxHeight) //字典代码
{
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/Select/GetDictionary",
        data: {
            Code: Code
        },
        success: function (data) {
            $('#' + CrotrolId).html('<option value=""></option>' + "请选择");
            for (var i = 0; i < data.length; i++) {
                if (data[i][Key] == SelectKey) //判断是否是默认选中
                {
                    $('#' + CrotrolId).append("<option value='" + data[i][Key] + "' selected>" + data[i][Text] + "</option>");
                } else {
                    $('#' + CrotrolId).append("<option value='" + data[i][Key] + "'>" + data[i][Text] + "</option>");
                }
            }
            Form.render();
            if (SelectedMaxHeight !== null) {
                var dlList = document.getElementsByClassName("layui-anim layui-anim-upbit");
                try { }
                catch (e) {
                    dlList[0].style.maxHeight = SelectedMaxHeight;
                }

            }
        }
    });
}

//获取扩展字段类字典
function SelectExtDictionary(Code, CrotrolId, Key, Text, SelectKey, Form) //字典代码
{
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/Select/GetExtDictionary",
        data: {
            Code: Code
        },
        success: function (data) {
            //console.log(data);
            for (var i = 0; i < data.length; i++) {
                //console.log(data[i].扩展属性.length);
                for (var j = 0; j < data[i].扩展属性.length; j++) {
                    data[i][data[i].扩展属性[j].字段名] = data[i].扩展属性[j].字段值;
                }
                //console.log(data);
                //console.log(data[i][Text]);
                if (data[i][Key] == SelectKey) //判断是否是默认选中
                {
                    $('#' + CrotrolId).append("<option value='" + data[i][Key] + "' selected>" + data[i][Text] + "</option>");
                } else {
                    $('#' + CrotrolId).append("<option value='" + data[i][Key] + "'>" + data[i][Text] + "</option>");
                }
            }
            Form.render();
        }
    });
}

//下拉列表框绑定
//SelectedMaxHeight   设置下拉框最大高度，超过高度，则出滚动条
function SelectBind(Controller, CrotrolId, Key, Text, SelectKey, Form, SelectedMaxHeight) //控制器(自定义),控件Id,值,内容,默认选中内容,重新加载
{
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/Select/" + Controller,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i][Key] == SelectKey) //判断是否是默认选中
                {
                    $('#' + CrotrolId).append("<option value='" + data[i][Key] + "' selected>" + data[i][Text] + "</option>");
                } else {
                    $('#' + CrotrolId).append("<option value='" + data[i][Key] + "'>" + data[i][Text] + "</option>");
                }
            }
            Form.render();
            if (SelectedMaxHeight !== null) {
                var dlList = document.getElementsByClassName("layui-anim layui-anim-upbit");
                try { }
                catch (e) {
                    dlList[0].style.maxHeight = SelectedMaxHeight;
                }
            }
        }
    });
}

//获取URL参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg); //获取url中"?"符后的字符串并正则匹配
    var context = "";
    if (r != null)
        context = r[2];
    reg = null;
    r = null;
    return context == null || context == "" || context == "undefined" ? "" : context;
}

//Tree选中状态
$("body").on("mousedown", ".layui-tree li a", function () {
    //if (!$(this).siblings('ul').length)
    $('.layui-tree li a').removeClass("treeClick");
    $(this).addClass("treeClick");
});

function ajaxFucHelper(url, successFun, data, dataType) {
    if (arguments[3] === '' || arguments[3] === undefined) {
        dataType = "json";
    }
    $.ajax({
        type: "POST",
        dataType: dataType,
        async: false,
        url: url,
        data: data,
        success: function (obj) {
            successFun(obj);
        }
    });
}


////表格数据加载
function renderTable(elem, url, height, cols, where, done) {
    var table = layui.table;
    table.render({
        text: {
            none: '暂无相关数据' //默认：无数据。注：该属性为 layui 2.2.5 开始新增
        }
        , elem: elem
        , url: url
        // ,id:'checkTable'
        , height: height
        , method: 'post'
        , limit: 15
        , limits: [15, 50, 100, 300, 500]
        , skin: 'nob'
        , width: 'auto' //全局定义常规单元格的最小宽度，layui 2.2.1 新增
        , cellMinWidth: 80
        , page: true
        //, initSort: sortData
        , cols: [cols]
        , even: true
        , where: where,
        done: done
    });
}


///////有多选功能的在表格加载之后需要渲染，渲染完成回调
function tableDone(res, id, checkKey) {
    var form = layui.form;
    //.假设你的表格指定的 id="maintb"，找到框架渲染的表格
    var tbl = $('#' + id).next('.layui-table-view');
    //.记下当前页数据，Ajax 请求的数据集，对应你后端返回的数据字段
    pageData = res.data;
    var len = pageData.length;
    //.遍历当前页数据，对比已选中项中的 id
    for (var i = 0; i < len; i++) {
        if (layui.data('checked', pageData[i][checkKey])) {
            //.选中它，目前版本没有任何与数据或表格 id 相关的标识，不太好搞，土办法选择它吧
            //tbl.find('table>tbody>tr').eq(i).find('td').eq(0).find('input[type=checkbox]').prop('checked', true);
            $("#key_" + pageData[i][checkKey]).prop('checked', true);
        }
    }
    //.PS：table 中点击选择后会记录到 table.cache，没暴露出来，也不能 mytbl.renderForm('checkbox');
    //.暂时只能这样渲染表单
    form.render('checkbox');
    dataRipple();
}

//监听选择，记录已选择项
function checkTable(fileter, checkKey) {
    var table = layui.table;
    table.on('checkbox(' + fileter + ')', function (obj) {
        var data = obj.type == 'one' ? [obj.data] : pageData;
        //.遍历数据
        $.each(data, function (k, v) {
            //.假设你数据中 id 是唯一关键字
            if (obj.checked) {
                //.增加已选中项
                layui.data('checked', { key: v[checkKey], value: v });
            } else {
                //.删除
                layui.data('checked', { key: v[checkKey], remove: true });
            }
        });
    });
}

//confirm 提示删除函数
//textTitle为"确认删除该项目吗？", url 路径，data：要删除对象的ID，successFun 为删除成功的回调函数
function confirmDel(textTitle, url, data, successFun) {
    layer.confirm(textTitle, { icon: 3, title: '提示' },
        function (index) {      //确认后执行的操作
            $.ajax({
                type: "POST",
                dataType: "json",
                url: url,
                data: data,
                success: successFun
            });
        });
}
