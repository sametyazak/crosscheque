function InitializePage() {
    InitializeComponents();
    BindEvents();
}

function InitializeComponents() {
    var loginForm = [
			{ view: "text", value: '', label: "Kullanıcı Adı", labelAlign: 'left', labelWidth: 150, name: 'UserName' },
			{ view: "text", type: 'password', value: '', label: "Şifre", labelAlign: 'left', labelWidth: 150, name: 'Password' },
            { view: "checkbox", label: "Beni Hatırla", value: 1, labelAlign: 'left', labelWidth: 150, name: 'RememberMe' },
			{
			    view: "button", value: "Giriş", width: 160, align: "center", click: function () {
			        var form = this.getParentView();
			        if (form.validate()) {
			            LoginUser(form.getValues());
			        }
			    }
			}
    ];

    webix.ui({
        container: "LoginArea",
        margin: 30, cols: [
            {
                margin: 30, rows: [
                  {
                      view: "form",
                      scroll: false,
                      width: 400,
                      elements: loginForm,

                      rules: {
                          $obj: function () {
                              var data = this.getValues();

                              if (!webix.rules.isNotEmpty(data.UserName)) {
                                  webix.message("Kullanıcı adı giriniz");
                                  return false;
                              }

                              if (!webix.rules.isNotEmpty(data.Password)) {
                                  webix.message("Şifre giriniz");
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

function LoginUser(data)
{
    ServerCall.Execute({ functionName: 'LoginUser', requestMessage: data, successCallBack: AfterLogin, failCallBack: null });
}

function AfterLogin(result)
{
    Core.Redirect(result);
}

function BindEvents()
{
    $('#GuestLogin').click(
        function () {
            ServerCall.Execute({ functionName: 'GuestLogin', requestMessage: null, successCallBack: AfterLogin, failCallBack: null });
        }
    );
}