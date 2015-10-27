function InitializePage() {
    InitializeComponents();
}

function InitializeComponents() {
    var passwordForm = [
           { view: "text", type: 'password', value: '', label: "Eski Şifre:", labelAlign: 'left', labelWidth: 150, name: 'OldPassword' },
           { view: "text", type: 'password', value: '', label: "Yeni Şifre:", labelAlign: 'left', labelWidth: 150, name: 'NewPassword' },
           { view: "text", type: 'password', value: '', label: "Yeni Şifre Tekrar:", labelAlign: 'left', labelWidth: 150, name: 'ConfirmPassword' },
           {
               view: "button", value: "Kaydet", width: 160, align: "center", click: function () {
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
                                  webix.message("Eski şifrenizi giriniz!");
                                  return false;
                              }

                              if (!webix.rules.isNotEmpty(data.NewPassword) || data.NewPassword.length < 6) {
                                  webix.message("Yeni Şifre en az 6 karakter olmalı");
                                  return false;
                              }

                              if (data.NewPassword != data.ConfirmPassword) {
                                  webix.message("Yeni Şifreler aynı değil!");
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
    webix.message({ text: "İşlem Başarılı" });
}