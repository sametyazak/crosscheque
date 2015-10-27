function InitializePage() {
    ToogleLeftMenu();
    //CreateLogGrid();
    SetPageEvents();
    GetLogList()
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
                    id: "Level", editor: "text", header: ["Level", {content: "textFilter", placeholder: "Filter", colspan: 7}], width: 60
                },
                { id: "IP", editor: "text", header: ["IP", null], width: 100 },
                { id: "ComputerName", editor: "text", header: ["Host", null], width: 100 },
                { id: "Reference", editor: "text", header: ["Referans", null], width: 100 },
                { id: "Message", editor: "text", header: ["Log", null], width: 300 },
                { id: "Date", editor: "text", header: ["Tarih", null], width: 180 },
                { id: "Url", editor: "text", header: ["Url", null], width: 330 }
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

        function equals(a, b) {
            a = a.toString().toLowerCase();
            return a.indexOf(b) !== -1;
        }

        logGrid.filterByAll = function () {
            //get fitler values
            var text = this.getFilter("Level").value.toString().toLowerCase();
            //unfilter for empty search text
            if (!text) return this.filter();

            //filter using or logic
            this.filter(function (obj) {
                if (equals(obj.Level, text)) return true;
                if (equals(obj.IP, text)) return true;
                if (equals(obj.ComputerName, text)) return true;
                if (equals(obj.Reference, text)) return true;
                if (equals(obj.Message, text)) return true;
                if (equals(obj.Url, text)) return true;
                return false;
            });
        };
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