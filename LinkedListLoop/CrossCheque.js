function InitMaster()
{
    Menu.Configure({ ContainerId: 'nav-primary' });
}

function AfterLogout(url) {
    Core.Redirect(url);
}

function SetMenuMain(menuList)
{
    Menu.SetMenuList(menuList);
}