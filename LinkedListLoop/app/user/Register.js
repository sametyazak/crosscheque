function InitializePage() {
    InitializeComponents();
}

function InitializeComponents() {
    var registerForm = [
			{ view: "text", label: "Kullanıcı Adı", placeholder: "(e.g. ali)", bottomLabel: "* Zorunlu alan", name: 'UserName' },
			{ view: "text", type: "password", label: "Şifre", bottomPadding: 35, bottomLabel: "* Şifre en az 6 karakter olmalıdır", name: 'Password' },
            { view: "text", type: "password", label: "Şifre Tekrar", name: 'ConfirmPassword' },
			{
			    view: "button", value: "Kaydet", width: 160, align: "center", click: function () {
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
                    webix.message("Kullanıcı adı giriniz");
                    return false;
                }

                if (!webix.rules.isNotEmpty(data.Password) || data.Password.length < 6) {
                    webix.message("Şifre en az 6 karakter olmalı");
                    return false;
                }

                if (data.Password != data.ConfirmPassword) {
                    webix.message("Şifreler aynı değil!");
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