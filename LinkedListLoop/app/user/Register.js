function InitializePage() {
    InitializeComponents();
    InitializeLanguageForm();
}

function InitializeComponents() {
    var registerForm = [
			{ view: "text", label: ML.UserName, placeholder: ML.SampleUserName, bottomLabel: ML.UserNameMendatory, name: 'UserName' },
			{ view: "text", type: "password", label: ML.Password, bottomPadding: 35, bottomLabel: ML.PasswordHint, name: 'Password' },
            { view: "text", type: "password", label: ML.ConfirmPassword, name: 'ConfirmPassword' },
			{
			    view: "button", value: ML.Save, width: 160, align: "center", click: function () {
			        var form = this.getParentView();
			        if (form.validate()) {
			            SaveUser(form.getValues());
			        }
			    }
			}
    ];

    webix.ui({
        container: "LoginArea",
        view: "form",
        scroll: false,
        width: 400,

        elementsConfig: {
            labelPosition: "top"
        },

        rules: {
            $obj: function () {
                var data = this.getValues();

                if (!webix.rules.isNotEmpty(data.UserName)) {
                    webix.message(ML.EnterUserName);
                    return false;
                }

                if (!webix.rules.isNotEmpty(data.Password) || data.Password.length < 6) {
                    webix.message(ML.PasswordHint);
                    return false;
                }

                if (data.Password != data.ConfirmPassword) {
                    webix.message(ML.PasswordsNotSame2);
                    return false;
                }

                return true;
            }
        },

        elements: registerForm
    });
}

function SaveUser(data)
{
    ServerCall.Execute({ functionName: 'RegisterUser', requestMessage: data, successCallBack: AfterRegister, failCallBack: null });
}

function AfterRegister(result) {
    Core.Redirect(result);
}

function InitializeLanguageForm() {
    var template = new Template();

    var languageBox = new template.LanguageBox({ containerId: 'LanguageContainer' });
}