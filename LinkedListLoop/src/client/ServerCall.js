var ServerCall = {
    ProcessCount: 0,
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

            ServerCall.PreProcess(options);

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

                    ServerCall.PostProcess(options);
                },
                fail: function (transport) {
                    if (Core.IsFunction(options.failCallBack)) {
                        Core.DynamicCall(options.failCallBack, []);
                    }

                    ServerCall.PostProcess(options);
                }
            }

            //ServerCall.ProcessCount++;
            $.ajax(ajaxOptions);

            if (!async)
            {
                return returnObject;
            }
        }
        catch (ex)
        {
            ServerCall.PostProcess(options);
        }
    },

    PreProcess: function (options)
    {
        ServerCall.ProcessCount++;
        Core.ShowOverlay();
    },

    PostProcess: function (options)
    {
        ServerCall.ProcessCount--;

        Core.HideOverlay();
    }
};