"use strict";

var Template = function () {
    this.UniqueId = '';

    this.LanguageBox = function (options) {
        this.formId = 'LanguageBox';
        this.options = options;
        this.Init();
    }

    this.LanguageBox.prototype.Init = function () {
        if (!this.Validate()) return;

        var languageList = Global.GetAvailableLanguages();
        if (this.options.Id) this.formId = this.options.Id;

        var languageOptions = [];
        for (var i = 0; i < languageList.length; i++) {
            languageOptions.push({ id: languageList[i].Name, value: languageList[i].Text });
        }

        webix.ui({
            view: "combo",
            value: ML.culture,
            label: '',
            options: languageOptions,
            id: this.formId,
            container: this.options.containerId,
            width: this.options.width || 150
        });

        var currentObject = this;

        $$(this.formId).attachEvent("onChange", function (newv, oldv) {
            currentObject.ChangeCulture(newv);
        });
    };

    this.LanguageBox.prototype.Validate = function () {
        if (!this.options) return false;
        if (!this.options.containerId) return false;

        return true;
    };

    this.LanguageBox.prototype.ChangeCulture = function (culture) {
        ServerCall.Execute({ functionName: 'ChangeCurrentCulture', requestMessage: culture, successCallBack: Core.RefreshPage, failCallBack: null });
    };


    this.Container = function (options) {
        this.options = options;
        this.Init();
    }

    this.Container.prototype.Init = function () {
        if (!this.Validate()) return;

        var mainContainer = $('#' + this.options.containerId);
        if (!mainContainer) return;

        var header = $('<div class="ColumnHeader"></div>');
        var detail = $('<div></div>');

        if (this.options.header.id)
        {
            header.attr('id', this.options.containerId + '_' + this.options.header.id);
        }

        if (this.options.header.text)
        {
            header.html(Global.GetString(this.options.header.text));
        }

        if (this.options.detail.id)
        {
            detail.attr('id', this.options.detail.id);
        }

        if (this.options.detail.wrapWithPre)
        {
            var pre = $('<pre></pre>');
            pre.append(detail);

            detail = pre;
        }

        if (this.options.detail.css) {
            detail.addClass(this.options.detail.css);
        }

        mainContainer.append(header);
        mainContainer.append(detail);
    }

    this.Container.prototype.Validate = function () {
        if (!this.options) return false;
        if (!this.options.containerId) return false;
        if (!this.options.header) return false;
        if (!this.options.detail) return false;

        return true;
    }
};

this.Template.prototype.Init = function () {
    webix.message({ type: "error", text: Global.GetString('InvalidOOP') });
};

