var Core = {
    IsArray: function (obj) {
        return Object.prototype.toString.call(obj) === '[object Array]';
    },

    IsFunction: function (functionToCheck) {
        var getType = {};
        return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
    },

    IsUndefined: function (value)
    {
        return (typeof value === "undefined");
    },

    DynamicCall: function (func, args) {
        var myObject;
        myObject = func.apply(myObject, args);
    },

    ShowOverlay: function () {
        var overlayEl = $('#MasterOverlay');

        if (overlayEl) {
            overlayEl.show();
        }
    },

    HideOverlay: function () {
        var overlayEl = $('#MasterOverlay');

        if (overlayEl) {
            overlayEl.hide();
        }
    }
}