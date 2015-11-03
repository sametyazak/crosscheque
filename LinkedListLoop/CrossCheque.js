function InitMaster()
{
    Menu.Configure({ ContainerId: 'nav-primary' });
    $('#toggleLeftMenu').on('click', Global.ToogleLeftMenu);

    InitializeLanguageForm();
    SetMLTexts();
}

function AfterLogout(url) {
    Core.Redirect(url);
}

function SetMenuMain(menuList)
{
    Menu.SetMenuList(menuList);
}

function InitializeLanguageForm() {
    var template = new Template();

    var languageBox = new template.LanguageBox({ containerId: 'LanguageContainer', width:130 });
}

function SetMLTexts()
{
    Global.SetMLContent('btnLoadSample', 'LoadSampleData');
    Global.SetMLContent('btnUploadFile', 'Load');
    Global.SetMLContent('WelcomeText', 'Welcome');
    Global.SetMLContent('MasterOverlayText', 'Loading');
}