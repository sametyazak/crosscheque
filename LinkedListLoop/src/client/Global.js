var Global = {
    OpenLeftMenu: function () {
        if (!Core.IsVisible('LeftMenuContainer')) {
            this.ToogleLeftMenu();
        }
    },

    ToogleLeftMenu: function () {
        $('#LeftMenuContainer').toggle();
        Global.SetMenuLayout();
    },

    SetMenuLayout: function () {
        if (Core.IsVisible('LeftMenuContainer')) {
            $('#FooterMenu').show();
            $('#PrimaryContent').addClass('content-primary-push');
        }
        else {
            $('#FooterMenu').hide();
            $('#PrimaryContent').removeClass('content-primary-push');
        }
    },

    ChangeTopMenuColor: function () {
        $('#TopMenu').css('background-color', '#fff');
        $('#UserPanel').css('color', '#00648c');
    },

    GetString: function (key) {
        return ML && ML[key] ? ML[key] : key;
    },

    GetAvailableLanguages: function () {
        return ServerCall.Execute({ functionName: 'GetAvailableLanguages', requestMessage: null, successCallBack: null, failCallBack: null, async: false });
    },

    SetMLContent: function (containerId, key) {
        var container = $('#' + containerId);

        if (container)
        {
            container.html(Global.GetString(key));
        }
    }

}