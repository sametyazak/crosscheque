"use strict";

var TopMenu = function (config) {
    this.config = config;
    this.items = new Object();
    this.Init();
}

TopMenu.prototype.Init = function () {
    if (!this.Validate()) return;

    var container = $('#' + this.config.containerId);
    if (!container) return;

    var uoList = $('<ul></ul>');

    if (this.config.css) {
        uoList.addClass(this.config.css);
    }
    else {
        uoList.addClass('nav-secondary__items');
    }

    uoList.attr('id', this.config.id);

    for (var i = 0; i < this.config.listItems.length; i++)
    {
        var listItem = this.config.listItems[i];
        if (!listItem.name) continue;

        var list = $('<li></li>');

        if (listItem.css) {
            list.addClass(listItem.css);
        }
        else {
            list.addClass("nav-secondary__item");
        }

        list.attr('id', this.config.id + '_' + listItem.name);

        if (Core.IsFunction(listItem.onClick))
        {
            list.on('click', listItem.onClick);
        }

        var listContent = $('<div></div>');

        if (listItem.contentCss)
        {
            listContent.addClass(listItem.contentCss);
        }

        listContent.html(listItem.text);
        list.append(listContent);

        this.items[listItem.name] = list;
        uoList.append(list);
    }

    container.append(uoList);
};

TopMenu.prototype.Validate = function () {
    if (!this.config) return false;
    if (!this.config.listItems) return false;
    if (!Core.IsArray(this.config.listItems)) return false;
    if (!this.config.id) return false;
    if (!this.config.containerId) return false;

    return true;
}