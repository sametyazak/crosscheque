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


    this.FileUpload = function (options)
    {
        this.options = options;
    }

    this.FileUpload.prototype.GetConfig = function ()
    {
        if (!this.Validate()) return;

        return {
            view: "form",
            type: "line",
            scrollY: true,
            padding: 10,
            //width: 500,
            rows: [
             {
                 view: "uploader", id: this.options.uploaderId, height: 37, align: "center", type: "iconButton", icon: "plus-circle",
                 label: Global.GetString('AddFiles'), autosend: false, link: this.options.listId, upload: $('#RootAddress').val() + '/FileUpload.aspx' + this.options.queryString
             },
             {
                 borderless: true,
                 view: "list",
                 id: this.options.listId,
                 type: this.options.uploaderTypeName,
                 scrollY: true,
                 autoheight: false,
                 minHeight: 100
             },
             {
                 id: this.options.uploadButtonId,
                 cols: [
                     { view: "button", label: Global.GetString('Upload'), type: "iconButton", icon: "upload", click: this.options.uploadClick, align: "center" },
                     { width: 5 },
                     { view: "button", label: Global.GetString('Cancel'), type: "iconButton", icon: "cancel-circle", click: this.options.cancelClick, align: "center" }

                 ]
             }
            ]
        };
    }

    this.FileUpload.prototype.GetUploaderTypeConfig = function ()
    {
        return {
            name: this.options.uploaderTypeName,
            template: function (f, type) {
                var html = "<div class='overall'><div class='name'>" + f.name + "</div>";
                html += "<div class='remove_file'><span style='color:#AAA' class='cancel_icon'></span></div>";
                html += "<div class='status'>";
                html += "<div class='progress " + f.status + "' style='width:" + (f.status == 'transfer' || f.status == "server" ? f.percent + "%" : "0px") + "'></div>";
                html += "<div class='message " + f.status + "'>" + type.status(f) + "</div>";
                html += "</div>";
                html += "<div class='size'>" + f.sizetext + "</div></div>";
                return html;
            },
            status: function (f) {
                var messages = {
                    server: Global.GetString('Done'),
                    error: Global.GetString('Error'),
                    client: Global.GetString('Ready'),
                    transfer: f.percent + "%"
                };
                return messages[f.status]

            },
            on_click: {
                "remove_file": function (ev, id) {
                    $$(this.config.uploader).files.remove(id);
                }
            },
            height: 35
        };
    }

    this.FileUpload.prototype.Validate = function ()
    {
        if (!this.options) return false;
        if (!this.options.uploaderId) this.options.uploaderId = 'upl1';
        if (!this.options.listId) this.options.listId = 'mylist';
        if (!this.options.uploadButtonId) this.options.uploadButtonId = 'uploadButtons';
        if (!this.options.uploaderTypeName) this.options.uploaderTypeName = 'myUploader';

        if (!this.options.queryString)
            this.options.queryString = '';
        else
            this.options.queryString = '?' + this.options.queryString;

        return true;
    }
};

this.Template.prototype.Init = function () {
    webix.message({ type: "error", text: Global.GetString('InvalidOOP') });
};

