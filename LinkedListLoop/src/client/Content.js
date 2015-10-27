var Content = {
    MenuItems: new Array(),
    CurrentMenuItem: null,
    ContainerId: '',
    ClickedMenuIndex: -1,
    Configuration: new Object(),

    CreateSlidingMenu: function (conf) {
        if (!conf) return;
        if (!(conf && conf.MenuItems && Core.IsArray(conf.MenuItems))) return;

        if (!conf.SideNavSpeed) conf.SideNavSpeed = 1000;
        if (!conf.TopNavSpeed) conf.TopNavSpeed = 500;

        this.Configuration = conf;
        var mainContainer = $('#' + conf.MainContainer);
        if (!mainContainer) return;
        this.ContainerId = conf.MainContainer;

        this.MenuItems = conf.MenuItems;
        this.CurrentMenuItem = 0;

        this.CreateSlidingNavigation(mainContainer);
        this.CreateSlidingContent(mainContainer);
        this.CreateSlidingMenuItems(mainContainer, conf);

        this.BindEvents(conf);
        this.SetNavigationTitle();
    },

    CreateSlidingNavigation: function (mainContainer) {
        mainContainer.append('<div id="' + this.ContainerId + '_SlidingNavigation">');
        mainContainer.append('<div class="nav-prev" id="' + this.ContainerId + '_Prev"><span class="side-nav-title" id="' + this.ContainerId + '_Prev_Title"></span> <- Prev</div>');

        for (var i = 0; i < this.MenuItems.length; i++) {
            var menuItem = this.MenuItems[i];
            if (!menuItem) return;

            if (i == 0) {
                mainContainer.append('<div class="menu-circle" style="opacity:1;" id="' + this.ContainerId + '_TopNav' + i + '">' + (i + 1) + '</div>');
            }
            else {
                mainContainer.append('<div class="menu-circle" style="opacity:0.5;" id="' + this.ContainerId + '_TopNav' + i + '">' + (i + 1) + '</div>');
            }
        }

        mainContainer.append('<div class="nav-next" id="' + this.ContainerId + '_Next">Next -> <span class="side-nav-title" id="' + this.ContainerId + '_Next_Title"></span></div>');
        mainContainer.append('</div>');
    },

    CreateSlidingContent: function (mainContainer) {
        mainContainer.append('<div class="sliding-content" id="' + this.ContainerId + '_SlidingContent">');
    },

    CreateSlidingMenuItems: function (mainContainer, conf) {
        var contentContainer = $('#' + this.ContainerId + '_SlidingContent');
        if (!contentContainer) return;

        for (var i = 0; i < conf.MenuItems.length; i++) {
            var menuItem = conf.MenuItems[i];
            if (!menuItem) return;

            var menuContentObj = $('#' + menuItem.MenuId);
            if (menuContentObj) {
                if (i != 0) {
                    menuContentObj.hide();
                }

                if (conf.MenuWidth) {
                    menuContentObj.css('width', conf.MenuWidth);
                }

                var menuText = menuContentObj[0].outerHTML;
                menuContentObj.remove();
                contentContainer.append(menuText);
            }
        }
    },

    BindEvents: function (conf) {
        this.BindSideNav(conf);
        this.BindTopNav(conf);
    },

    BindSideNav: function (conf) {
        var prevButton = $('#' + this.ContainerId + '_Prev');
        var nextButton = $('#' + this.ContainerId + '_Next');

        if (prevButton && nextButton) {
            if (Core.IsFunction(conf.OnNext)) {
                nextButton.click(function () {
                    Core.DynamicCall(conf.OnNext, null);
                });
            }

            if (Core.IsFunction(conf.OnPrev)) {
                prevButton.click(function () {
                    Core.DynamicCall(conf.OnPrev, null);
                });
            }
        }
    },

    BindTopNav: function (conf) {
        var object = this;

        $.each(conf.MenuItems, function (i, v) {
            var navItem = object.GetNavigationItem(i);

            if (Core.IsFunction(conf.OnNavigate)) {
                navItem.click(function () {
                    Core.DynamicCall(conf.OnNavigate, [i]);
                });
            }
        });
    },

    NavigatePrev: function (onComplete, speed) {
        if (Core.IsUndefined(onComplete)) onComplete = null;
        if (Core.IsUndefined(speed)) speed = this.Configuration.SideNavSpeed;

        if (this.CurrentMenuItem < 0) {
            this.CurrentMenuItem = 0;
        }

        if (this.CurrentMenuItem > 0) {
            var currentDiv = $('#' + this.MenuItems[this.CurrentMenuItem].MenuId);
            var prevDiv = $('#' + this.MenuItems[this.CurrentMenuItem - 1].MenuId);

            Core.ShowOverlay();

            var onrealComplete = function () {
                if (onComplete) {
                    Core.DynamicCall(onComplete, null);
                }

                Core.HideOverlay();
            }

            currentDiv.hide("slide", { direction: "right" }, speed);
            prevDiv.delay(speed).show("slide", { direction: "left" }, speed, onrealComplete);

            this.SetTopMenuNavItemOpacity(this.CurrentMenuItem, --this.CurrentMenuItem);
            this.SetNavigationTitle();
        }
    },

    NavigateNext: function (onComplete, speed) {
        if (Core.IsUndefined(onComplete)) onComplete = null;
        if (Core.IsUndefined(speed)) speed = this.Configuration.SideNavSpeed;

        if (this.CurrentMenuItem < 0) this.CurrentMenuItem = 0;

        if (this.CurrentMenuItem < (this.MenuItems.length - 1)) {
            var currentDiv = $('#' + this.MenuItems[this.CurrentMenuItem].MenuId);
            var nextDiv = $('#' + this.MenuItems[this.CurrentMenuItem + 1].MenuId);

            Core.ShowOverlay();

            if (!onComplete && Core.IsFunction(this.MenuItems[this.CurrentMenuItem + 1].OnStart)) {
                onComplete = this.MenuItems[this.CurrentMenuItem + 1].OnStart;
            }

            var onrealComplete = function () {
                if (onComplete) {
                    Core.DynamicCall(onComplete, null);
                }

                Core.HideOverlay();
            }

            currentDiv.hide("slide", { direction: "left" }, speed);
            nextDiv.delay(speed).show("slide", { direction: "right" }, speed, onrealComplete);

            this.SetTopMenuNavItemOpacity(this.CurrentMenuItem, ++this.CurrentMenuItem);
            this.SetNavigationTitle();
        }
    },

    NavigateTo: function (menuIndex) {
        if (Core.IsUndefined(menuIndex)) {
            menuIndex = this.ClickedMenuIndex;
        }
        else {
            this.ClickedMenuIndex = menuIndex;
        }

        if (menuIndex < this.CurrentMenuItem) {
            this.NavigatePrev(this.Configuration.OnNavigate, this.Configuration.TopNavSpeed);
        }
        else if (menuIndex > this.CurrentMenuItem) {
            this.NavigateNext(this.Configuration.OnNavigate, this.Configuration.TopNavSpeed);
        }
    },

    SetTopMenuNavItemOpacity: function (currentIndex, nextIndex) {
        var currentNavItem = this.GetNavigationItem(currentIndex);
        var nextNavItem = this.GetNavigationItem(nextIndex);

        if (currentNavItem && nextNavItem) {
            currentNavItem.css('opacity', 0.5);
            nextNavItem.css('opacity', 1);
        }
    },

    GetNavigationItem: function (i) {
        return $('#' + this.ContainerId + '_TopNav' + i)
    },

    GetPrevNavigationTitleObj: function () {
        return $('#' + this.ContainerId + '_Prev_Title')
    },

    GetNextNavigationTitleObj: function () {
        return $('#' + this.ContainerId + '_Next_Title')
    },

    SetNavigationTitle: function ()
    {
        this.NormalizeCurrentMenuItem();

        var prevText = '';
        var nextText = '';

        var prevObj = this.GetPrevNavigationTitleObj();
        var nextObj = this.GetNextNavigationTitleObj();

        if (this.CurrentMenuItem != 0)
        {
            prevText = this.MenuItems[this.CurrentMenuItem - 1].Title;
        }

        if (this.CurrentMenuItem < (this.MenuItems.length - 1))
        {
            nextText = this.MenuItems[this.CurrentMenuItem + 1].Title;
        }

        prevObj.html(prevText);
        nextObj.html(nextText);
    },

    NormalizeCurrentMenuItem: function ()
    {
        if (this.CurrentMenuItem < 0) {
            this.CurrentMenuItem = 0;
        }

        if (this.CurrentMenuItem > (this.MenuItems.length - 1)) {
            this.CurrentMenuItem = this.MenuItems.length - 1;
        }
    }
}