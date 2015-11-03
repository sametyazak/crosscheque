var Menu = {
    options: new Object(),

    Configure: function (options) {
        this.options = options;
        ServerCall.Execute({ functionName: 'GetMenuList', requestMessage: null, successCallBack: SetMenuMain });
    },

    SetMenuList: function (menuList) {
        if (!menuList) return;
        if (!this.options) return;
        if (!this.options.ContainerId) return;

        var mainContainer = $('#' + this.options.ContainerId);
        if (!mainContainer) return;

        
        var uoList = $('<ul class="nav-primary__items"></ul>');
        
        for (var i = 0; i < menuList.length; i++)
        {
            var menuItem = menuList[i];
            var listItem = $('<li class="nav-primary__item"></li>');

            if (menuItem.Css)
            {
                listItem.addClass('nav-bg');
                listItem.addClass(menuItem.Css);
            }

            var link = $('<a id="' + this.options.ContainerId + '_' + menuItem.Name
                + '" href="' + menuItem.Link + '" class="nav-primary__link">' + menuItem.Title + '</a>');

            listItem.append(link);
            uoList.append(listItem);
        }

        mainContainer.append(uoList);
    }
}