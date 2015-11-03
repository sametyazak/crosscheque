var ServerCall = {
    Execute: function (options) {
        try
        {
            var postData = JSON.stringify({
                ServerSideMethod: options.functionName,
                Data: options.requestMessage ? JSON.stringify(options.requestMessage) : "{}",
                LogRequest: Core.IsUndefined(options.LogRequest) ? true : options.LogRequest
            });

            var returnObject = {};

            var queryData = 'queryData=' + postData;
            var url = $('#RootAddress').val() + '/ServerCall.ashx';
            var async = Core.IsUndefined(options.async) ? true : options.async;

            Core.ShowOverlay();

            var ajaxOptions = {
                type: "POST",
                url: url,
                data: queryData,
                responseType: "json",
                async: async,
                success: function (transport, json) {
                    var resultObject = JSON.parse(transport);

                    if (resultObject) {
                        returnObject = resultObject && resultObject.ResultObject ? resultObject.ResultObject : resultObject;

                        if (resultObject && resultObject.IsError) {
                            if (Core.IsFunction(options.failCallBack)) {
                                Core.DynamicCall(options.failCallBack, resultObject.ErrorMessage);
                            }
                            else {
                                webix.message({ type: "error", text: resultObject.ErrorDetail });
                            }
                        }
                        else if (Core.IsFunction(options.successCallBack)) {
                            Core.DynamicCall(options.successCallBack, [returnObject]);
                        }
                    }

                    Core.HideOverlay();
                },
                fail: function (transport) {
                    if (Core.IsFunction(options.failCallBack)) {
                        Core.DynamicCall(options.failCallBack, []);
                    }

                    Core.HideOverlay();
                }
            }

            $.ajax(ajaxOptions);

            if (!async)
            {
                return returnObject;
            }
        }
        catch (ex)
        {
            Core.HideOverlay();
        }
    }
};