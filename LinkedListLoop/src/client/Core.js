var Core = {
    OverlayProcess: 0,

    IsArray: function (obj) {
        return Object.prototype.toString.call(obj) === '[object Array]';
    },

    IsFunction: function (functionToCheck) {
        var getType = {};
        return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
    },

    IsUndefined: function (value) {
        return (typeof value === "undefined");
    },

    DynamicCall: function (func, args) {
        var myObject;
        myObject = func.apply(myObject, args);
        return myObject;
    },

    ShowOverlay: function () {
        var overlayEl = $('#MasterOverlay');

        if (overlayEl) {
            Core.OverlayProcess++;
            overlayEl.show();
        }
    },

    HideOverlay: function () {
        var overlayEl = $('#MasterOverlay');

        if (overlayEl) {
            Core.OverlayProcess--;

            if (Core.OverlayProcess == 0) {
                overlayEl.hide();
            }
        }
    },

    Redirect: function (location) {
        window.location.href = location;
    },

    IsVisible: function (id) {
        var item = $('#' + id);
        return item && item.is(':visible');
    },

    RefreshPage: function () {
        location.reload();
    }
}