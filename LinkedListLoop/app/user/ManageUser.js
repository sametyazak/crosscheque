var roleList;
var userList;

function InitializePage() {
    InitializeComponents();
}

function InitializeComponents() {
    GetRoleList();
}

function GetRoleList() {
    ServerCall.Execute({ functionName: 'GetAllRoles', requestMessage: null, successCallBack: SetRoleList, failCallBack: null });
}

function SetRoleList(role) {
    roleList = role;
    GetAllUsers();
}

function CreateForm() {
    var roleForm = GetRoleForm();

    var form1 = {
        width: 700,
        view: "form",
        scroll: false,
        id: 'RoleGrid',
        container: "ManageUserContainer",
        padding: 20,
        rows: roleForm
    };

    webix.ui(form1);

    $$("RoleGrid").elements["username"].attachEvent("onChange", function (newv, oldv) {
        GetUserRoles(newv);
    });
}

function GetRoleForm() {
    var roleForm = [];

    roleForm.push({ view: "combo", name: "username", label: "Kullanıcı Adı:", value: "", labelWidth: 150, options: userList });

    for (var i = 0; i < roleList.length; i++) {
        var roleItem = { view: "checkbox", label: roleList[i], value: 0, labelAlign: 'left', labelWidth: 150, name: roleList[i] }
        roleForm.push(roleItem);
    }

    roleForm.push(
        {
            view: "button", value: "Kaydet", width: 160, align: "center", click: function () {
                var form = this.getParentView();
                if (form.validate()) {
                    SaveUser(form.getValues());
                }
            }
        }
    );

    return roleForm;
}

function SaveUser(data) {
    var saveRequest = {
        UserName: data.username,
        Roles: new Object()
    };

    var formElements = $$("RoleGrid").elements;

    for (var key in formElements) {
        if (formElements.hasOwnProperty(key)) {
            var formItem = formElements[key];

            if (formItem && formItem.config && formItem.config.view == 'checkbox') {
                saveRequest.Roles[key] = formItem.getValue();
            }
        }
    }

    ServerCall.Execute({ functionName: 'SaveUser', requestMessage: saveRequest, successCallBack: SetSuccessMessage, failCallBack: null });
}

function SetSuccessMessage() {
    webix.message({ text: "İşlem Başarılı" });
}

function GetAllUsers() {
    ServerCall.Execute({ functionName: 'GetAllUsers', requestMessage: null, successCallBack: SetUserList, failCallBack: null });
}

function SetUserList(user) {
    var users = [];

    for (var i = 0; i < user.length; i++) {
        users.push({ id: user[i], value: user[i] });
    }

    userList = users;

    CreateForm();
}

function GetUserRoles(userName) {
    ServerCall.Execute({ functionName: 'GetUserRoles', requestMessage: userName, successCallBack: SetUserRoleList, failCallBack: null });
}

function SetUserRoleList(userRoles) {
    var formElements = $$("RoleGrid").elements;

    for (var key in formElements) {
        if (formElements.hasOwnProperty(key)) {
            var formItem = formElements[key];

            if (formItem && formItem.config && formItem.config.view == 'checkbox') {
                if ($.inArray(key, userRoles) > -1) {
                    formItem.setValue(1);
                }
                else {
                    formItem.setValue(0);
                }
            }
        }
    }
}
