var ServerCall = {
    Execute: function (functionName, requestMessage, successCallBack, failCallBack) {
        var postData = JSON.stringify({
            ServerSideMethod: functionName,
            Data: requestMessage ? JSON.stringify(requestMessage) : "{}"
        });

        var queryData = 'queryData=' + postData;
        var url = $('#RootAddress').val() + '/ServerCall.ashx';

        ServerCall.ShowOverlay();

        $.ajax({
            type: "POST",
            url: url,
            data: queryData,
            responseType: "json",
            success: function (transport, json) {
                var resultObject = JSON.parse(transport);

                if (resultObject) {
                    var returnObject = resultObject && resultObject.ResultObject ? resultObject.ResultObject : resultObject;

                    if (resultObject && resultObject.IsError) {
                        ServerCall.DynamicCall(failCallBack, resultObject.ErrorMessage);
                    }
                    else if (ServerCall.IsFunction(successCallBack)) {
                        ServerCall.DynamicCall(successCallBack, [returnObject]);
                    }
                }

                ServerCall.HideOverlay();
            },
            fail: function (transport) {
                if (ServerCall.IsFunction(failCallBack)) {
                    ServerCall.DynamicCall(failCallBack, []);
                }

                ServerCall.HideOverlay();
            }
        });
    },

    IsFunction: function (functionToCheck) {
        var getType = {};
        return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
    },

    DynamicCall: function (func, args) {
        var myObject;
        myObject = func.apply(myObject, args);
    },

    ShowOverlay: function ()
    {
        var overlayEl = $('#MasterOverlay');

        if (overlayEl)
        {
            overlayEl.show();
        }
    },

    HideOverlay: function ()
    {
        var overlayEl = $('#MasterOverlay');

        if (overlayEl) {
            overlayEl.hide();
        }
    }
};