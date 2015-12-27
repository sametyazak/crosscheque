function InitializePage() {
    InitializeComponents();
    BindEvents();
}

function InitializeLoginForm() {
    var loginForm = [

            { view: "text", value: '', label: ML.UserName, labelAlign: 'left', labelWidth: 150, name: 'UserName' },
			{ view: "text", type: 'password', value: '', label: ML.Password, labelAlign: 'left', labelWidth: 150, name: 'Password' },
            { view: "checkbox", label: ML.RememberMe, value: 1, labelAlign: 'left', labelWidth: 150, name: 'RememberMe' },
			{
			    view: "button", value: ML.Login, width: 160, align: "center", hotkey: "enter", click: function () {
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
                      id: 'LoginForm',
                      rules: {
                          $obj: function () {
                              var data = this.getValues();

                              if (!webix.rules.isNotEmpty(data.UserName)) {
                                  webix.message(ML.EnterUserName);
                                  return false;
                              }

                              if (!webix.rules.isNotEmpty(data.Password)) {
                                  webix.message(ML.EnterPassword);
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

function InitializeComponents() {
    InitializeLoginForm();
    InitializeLanguageForm();
}

function InitializeLanguageForm() {
    var template = new Template();

    var languageBox = new template.LanguageBox({containerId: 'LanguageContainer'});
}

function LoginUser(data) {
    ServerCall.Execute({ functionName: 'LoginUser', requestMessage: data, successCallBack: AfterLogin, failCallBack: null });
}

function AfterLogin(result) {
    Core.Redirect(result);
}

function BindEvents() {
    $('#GuestLogin').click(
        function () {
            ServerCall.Execute({ functionName: 'GuestLogin', requestMessage: null, successCallBack: AfterLogin, failCallBack: null });
        }
    );
}