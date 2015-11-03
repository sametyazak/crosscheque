function InitializePage() {
    InitializeComponents();
}

function InitializeComponents() {
    var passwordForm = [
           { view: "text", type: 'password', value: '', label: ML.OldPasswordLabel, labelAlign: 'left', labelWidth: 200, name: 'OldPassword' },
           { view: "text", type: 'password', value: '', label: ML.NewPasswordLabel, labelAlign: 'left', labelWidth: 200, name: 'NewPassword' },
           { view: "text", type: 'password', value: '', label: ML.NewPasswordConfirmLabel, labelAlign: 'left', labelWidth: 200, name: 'ConfirmPassword' },
           {
               view: "button", value: ML.Save, width: 160, align: "center", click: function () {
                   var form = this.getParentView();
                   if (form.validate()) {
                       ChangePassword(form.getValues());
                   }
               }
           }
    ];

    webix.ui({
        container: "ManageUserContainer",
        margin: 30, cols: [
            {
                margin: 30, rows: [
                  {
                      view: "form",
                      scroll: false,
                      width: 400,
                      elements: passwordForm,

                      rules: {
                          $obj: function () {
                              var data = this.getValues();

                              if (!webix.rules.isNotEmpty(data.OldPassword)) {
                                  webix.message(ML.EnterOldPassword);
                                  return false;
                              }

                              if (!webix.rules.isNotEmpty(data.NewPassword) || data.NewPassword.length < 6) {
                                  webix.message(ML.NewPasswordCharacterLongError);
                                  return false;
                              }

                              if (data.NewPassword != data.ConfirmPassword) {
                                  webix.message(ML.PasswordsNotSame);
                                  return false;
                              }

                              return true;
                          }
                      }
                  },
                ]
            }
        ]
    });
}

function ChangePassword(data)
{
    ServerCall.Execute({ functionName: 'ChangePassword', requestMessage: data, successCallBack: SetSuccessMessage, failCallBack: null });
}

function SetSuccessMessage() {
    webix.message({ text: ML.SuccessfullTransaction });
}