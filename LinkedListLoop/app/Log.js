function InitializePage() {
    SetPageEvents();
    GetLogList();
    InitContainers();
}

function CreateLogGrid() {
    SetLogData([]);
}

function SetPageEvents() {
    $('#btnProcessSenderList').click(
        function () {
            GetLogList();
        }
    );
}

function SetLogData(logList) {

    if ($$("LogGrid")) {
        $$("LogGrid").clearAll();

        if (logList) {
            for (var i = 0; i < logList.length; i++) {
                $$("LogGrid").add(logList[i]);
            }

        }
    }
    else {
        var grid = {
            view: "datatable",
            id: "LogGrid",
            scrollY: true,
            select: "row",
            editable: false,
            //height: 800,
            autowidth: false,
            autoheight: false,
            height: 800,
            columns: [
                {
                    id: "Level", editor: "text", header: ["Level", {content: "textFilter", placeholder: ML.Filter, colspan: 1}], width: 60
                },
                { id: "IP", editor: "text", header: ["IP", { content: "textFilter", placeholder: ML.Filter, colspan: 1 }], width: 100 },
                { id: "ComputerName", editor: "text", header: [ML.Host, { content: "textFilter", placeholder: ML.Filter, colspan: 1 }], width: 100 },
                { id: "Reference", editor: "text", header: [ML.Referans, { content: "textFilter", placeholder: ML.Filter, colspan: 1 }], width: 100 },
                { id: "Message", editor: "text", header: [ML.Log, { content: "textFilter", placeholder: ML.Filter, colspan: 1 }], width: 300 },
                { id: "Date", editor: "text", header: [ML.Date, { content: "textFilter", placeholder: ML.Filter, colspan: 1 }], width: 180 },
                { id: "Url", editor: "text", header: [ML.Url, { content: "textFilter", placeholder: ML.Filter, colspan: 1 }], width: 330 }
            ],
            on:{
                "onItemClick":function(id, e, trg){
                    var log = this.getItem(id.row);
                    SetLogDetail(log)
                }
            },
            //pager: "bottomPager",
            ready: function () {
            },
            data: logList,
            scheme: {
                $change: function (item) {
                    if (item.Level == 'INFO')
                        item.$css = "log-info";
                    if (item.Level == 'ERROR')
                        item.$css = "log-error";
                    if (item.Level == 'FATAL')
                        item.$css = "log-fatal";
                }
            }
        };

        var logGrid = webix.ui({
            container: "LogContainer",

            type: "clear",
            rows: [
              { view: "template", css: "headline", height: 0 },
              grid,
              {
                  type: "clear", height: 46, paddingY: 8, cols: [
                  { view: "pager", id: "bottomPager", size: 11, width: 200 },
                  {}
                  ]
              }
            ]
        });

        //function equals(a, b) {
        //    a = a.toString().toLowerCase();
        //    return a.indexOf(b) !== -1;
        //}

        //webix.$$('LogGrid').filterByAll = function () {
        //    //get fitler values
        //    var text = this.getFilter("Level").value.toString().toLowerCase();
        //    text += '&&' + this.getFilter("IP").value.toString().toLowerCase();
        //    text += '&&' + this.getFilter("ComputerName").value.toString().toLowerCase();
        //    text += '&&' + this.getFilter("Reference").value.toString().toLowerCase();
        //    text += '&&' + this.getFilter("Message").value.toString().toLowerCase();
        //    text += '&&' + this.getFilter("Date").value.toString().toLowerCase();
        //    text += '&&' + this.getFilter("Url").value.toString().toLowerCase();
        //    //unfilter for empty search text
        //    if (!text) return this.filter();

        //    //filter using or logic
        //    this.filter(function (obj) {
        //        if (equals(obj.Level, text)) return true;
        //        if (equals(obj.IP, text)) return true;
        //        if (equals(obj.ComputerName, text)) return true;
        //        if (equals(obj.Reference, text)) return true;
        //        if (equals(obj.Message, text)) return true;
        //        if (equals(obj.Url, text)) return true;
        //        return false;
        //    });
        //};
    }
}

function GetLogList() {
    ServerCall.Execute({ functionName: 'GetLogList', requestMessage: null, successCallBack: SetLogData, failCallBack: null, LogRequest: false });
}

function SetLogDetail(log)
{
    var logDetail = JSON.stringify(log, undefined, 4);
    $('#LogDetail').html(logDetail);
}

function InitContainers() {
    var logOptions = {
        containerId: 'LogDetailContainer',

        header: {
            text: 'LogDetail',
            id: 'Test'
        },

        detail: {
            id: 'LogDetail',
            wrapWithPre: true
        }
    };

    var template = new Template();
    var network = new template.Container(logOptions);
}