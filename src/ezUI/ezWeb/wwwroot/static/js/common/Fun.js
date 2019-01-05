//全局方法
var Fun = {};

Fun.openAddActivity = function (m, mid) {
    VAR.layIndexActivityAdd = Feng.openWin('添加动弹', '/activity/activity_add?main='+m+'&mainid='+mid);
};
Fun.openActivityRemark = function (id) {
    VAR.layIndexActivityRemark = layer.open({
        type: 2,
        title: '添加动弹备注',
        area: ['460px', '320px'], //宽高
        fix: false, //不固定
        maxmin: true,
        content: Feng.ctxPath + '/activity/remark?id='+id
    });
};

Fun.openTaskRemark = function (id) {
    VAR.layIndexTaskRemark = layer.open({
        type: 2,
        title: '添加任务备注',
        area: ['460px', '320px'], //宽高
        fix: false, //不固定
        maxmin: true,
        content: Feng.ctxPath + '/task/remark?id='+id
    });
};

Fun.openFeedback = function() {
    VAR.layIndexFeedback = layer.open({
            type: 2,
            title: '意见反馈',
            area: ['580px', '420px'], //宽高
            fix: false, //不固定
            maxmin: true,
            content: Feng.ctxPath + '/feedback'
        });
};

Fun.openAddContacts = function(custId) {
    VAR.layIndexContactsAdd = Feng.openWin('添加联系人', '/contacts/contacts_add?custId='+custId);
};

Fun.openAddContactsByOppor = function(opporId) {
    VAR.layIndexContactsAdd = Feng.openWin('添加联系人', '/contacts/contacts_add?opporId='+opporId);
};

Fun.openAddOpportunity = function(custId){
    VAR.layIndexOpporAdd = Feng.openWin('添加项目机会', '/opportunity/opportunity_add?custId='+custId);
};

Fun.openAddOpportunityByContacts = function(cactsId){
    VAR.layIndexOpporAdd = Feng.openWin('添加项目机会', '/opportunity/opportunity_add?cactsId='+cactsId);
};

Fun.openAddCustomer = function () {
    VAR.layIndexCustomerAdd = Feng.openWin('添加客户资料', '/customer/customer_add');
};

Fun.openAddCustomerByStaff = function (staffId) {
    VAR.layIndexCustomerAdd = Feng.openWin('添加客户资料', '/customer/customer_add?staffId='+staffId);
};

Fun.openAddStaff = function () {
    VAR.layIndexStaffAdd = Feng.openWin('添加团队成员', '/staff/staff_add');
};

Fun.openAddTask = function () {
    VAR.layIndexTaskAdd = Feng.openWin('添加工作任务', '/task/task_add');
};

Fun.openAddTaskByStaff = function (staffId) {
    VAR.layIndexTaskAdd = Feng.openWin('添加工作任务', '/task/task_add?staffId='+staffId);
};

Fun.openUpdateActivity = function (activityid) {
    VAR.layIndexActivityUpdate = Feng.openWin('动弹修改', '/activity/activity_update/'+activityid);
};

Fun.openUpdateCustomer = function (custid) {
    VAR.layIndexCustomerUpdate = Feng.openWin('客户资料修改', '/customer/customer_update/' + custid);
};

Fun.openUpdateContacts = function (contactsid) {
    VAR.layIndexContactsUpdate = Feng.openWin('联系人修改', '/contacts/contacts_update/'+contactsid);
};

Fun.openUpdateOpportunity = function (opporid) {
    VAR.layIndexOpporUpdate = Feng.openWin('机会/项目修改', '/opportunity/opportunity_update/'+opporid);
};

Fun.openUpdateStaff = function (staffid) {
    VAR.layIndexStaffUpdate = Feng.openWin('团队成员修改', '/staff/staff_update/'+staffid);
};

Fun.openUpdateTask = function (taskid) {
    VAR.layIndexTaskUpdate = Feng.openWin('工作任务修改', '/task/task_update/'+taskid);
};

Fun.openDetailContacts = function (contactsid) {
    VAR.layIndexContactsDetail = Feng.openWin('联系人详情查看窗口', '/contacts/detail/'+contactsid);
};

Fun.openDetailOpportunity = function (opporid) {
    VAR.layIndexOpporDetail = Feng.openWin('机会/项目详情查看窗口', '/opportunity/detail/'+opporid);
};

Fun.openDetailCustomer = function (custid) {
    VAR.layIndexCustomerDetail = Feng.openWin('客户详情查看窗口', '/customer/detail/'+custid);
};

Fun.openDetailStaff = function (staffid) {
    VAR.layIndexStaffDetail = Feng.openWin('成员详情查看窗口', '/staff/detail/'+staffid);
};

Fun.openLocationStaff = function (staffid) {
    VAR.layIndexStaffLocation = Feng.openWin('职员最近的位置记录', '/staff/locations?sid=' + staffid);
};

Fun.openChangPWD = function () {
    VAR.layIndexChangPWD = layer.open({
        type: 2,
        title: '修改登录密码',
        area: ['460px', '320px'], //宽高
        fix: false, //不固定
        maxmin: true,
        content: Feng.ctxPath + '/password/form'
    });
};

Fun.changPWDSubmit = function() {
    var form={};
    form['oldPwd']=$("#oldPwd").val();
    form['newPwd']=$("#newPwd").val();
    form['newPwd2']=$("#newPwd2").val();

    //提交信息
    var ajax = new $ax(Feng.ctxPath + "/password/reset", function(data){
        Feng.success("更改成功，退出用新密码重登录！");
        setTimeout(function () {
            parent.layer.close(window.parent.VAR.layIndexChangPWD);
        }, 1500);
    },function(data){
        Feng.error("保存失败!" + data.responseJSON.message + "!");
    });
    ajax.setData(form);
    ajax.start();
};

Fun.openContribute = function () {
    VAR.layIndexContribute = layer.open({
        type: 2,
        title: '感谢您的捐赠',
        area: ['420px', '540px'], //宽高
        fix: false, //不固定
        maxmin: true,
        content: Feng.ctxPath + '/contribute'
    });
};

Fun.openMap = function(lat, lon) {
    if(lat==null || lon == null || lat=='' || lon=='') {
        Feng.info("没有定位信息");
        return;
    }
    VAR.layIndexMap = Feng.openWin('位置地图指示', '/location/map?lat=' + lat + '&lon=' + lon);
};