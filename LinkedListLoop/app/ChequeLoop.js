var loopResult = new Object();

function InitializePage() {
    InitSlidingMenu();
    //GetSenderList();
    CreateSenderList();
    //GetSampleData();
    CreateLoopList();
    SetPageEvents();

    $('#TopNavUl').show();
}

function GetSenderList() {
    ServerCall.Execute({ functionName: 'GetChequeListJson', requestMessage: null, successCallBack: SetGridData, failCallBack: null});
}

function CreateSenderList() {
    SetGridData([]);
}

function CreateLoopList() {
    SetLoopResults([]);
}

function SetGridData(senderList) {
    /*
    senderGrid = webix.ui(
        {
        container: "SenderList",
        view: "datatable",
        columns: [
            { id: "Sender", editor: "text", header: "Gönderen", width: 100 },
            { id: "Receiver", editor: "text", header: "Alıcı", width: 100 },
            { id: "Amount", editor: "text", header: "Tutar", width: 80 },
            { id: "Date", editor: "date", header: "Tarih", width: 200 },
            {
                id: "",
                template: "<input class='delbtn' type='button' value='Delete'>",
                css: "padding_less",
                width: 100
            },
        ],
        autoheight: true,
        autowidth: true,
        editable: true,

        on: {
            onBeforeLoad: function () {
                this.showOverlay("Loading...");
            },
            onAfterLoad: function () {
                this.hideOverlay();
            }
        },

        data: senderList
    });
    */

    if ($$("dt")) {
        $$("dt").clearAll();
        SetLoopResults("[]");

        if (senderList) {
            for (var i = 0; i < senderList.length; i++) {
                $$("dt").add(senderList[i]);
            }

        }
    }
    else {
        var grid = {
            view: "datatable",
            id: "dt",
            scrollY: true,
            select: "row",
            navigation: true,
            yCount: 10,
            editable: true,
            math: true,
            autoheight: false,
            height:800,
            autowidth: false,
            columns: [
                { id: "Sender", editor: "text", header: "Gönderen", width: 100 },
                { id: "Receiver", editor: "text", header: "Alıcı", width: 100 },
                { id: "Amount", editor: "text", header: "Tutar", width: 80 },
                { id: "Date", editor: "date", header: "Tarih", width: 100 },
            ],
            editaction: "dblclick",
            pager: "bottomPager",
            ready: function () {
                //InitSlidingMenu();
            },
            data: senderList,

            on: {
                onBeforeLoad: function () {

                },
                onAfterLoad: function () {

                }
            }
        };

        senderGrid = webix.ui({
            container: "SenderList",
            type: "clear",
            rows: [
              { view: "template", css: "headline", height: 0 },
              grid,
              {
                  type: "clear", height: 46, paddingY: 8, cols: [
                  { view: "pager", id: "bottomPager", size: 11, width: 200 },
                  {},
                  {
                      view: "button", value: "Ekle", width: 100, click: function () {
                          var id = $$("dt").add({ Sender: "", Receiver: "", Amount: 0, Date: "2014-01-01" });
                          $$("dt").editCell(id, "Sender");
                      }
                  },
                  {
                      view: "button", value: "Sil", width: 100, click: function () {
                          $$("dt").remove($$("dt").getSelectedId(true));
                      }
                  },
                  {
                      view: "button", value: "Temizle", width: 100, click: function () {
                          $$("dt").clearAll();
                          $$("LoopDt").clearAll();
                      }
                  },
                  ]
              }
            ]
        });
    }
}

function SetPageEvents() {
    $('#btnProcessSenderList').click(
        function () {
            ProcessSenderList();
        }
    );

    $('#btnLoadSample').click(
        function () {
            $('#SampleDataContainer').hide();
            GetSampleData();
        }
    );

    $('#btnUploadFile').click(
        function () {
            $('#UploadDataContainer').hide();
            LoadFile();
        }
    );

    $('#btnDowloadSampleData').click(
        function () {
            GetSampleDownloadLink();
        }
    );
}

function ProcessSenderList() {
    var senderList = GetGridSenderList();
    var loopList = GetProcessResults(senderList);
}

function GetGridSenderList() {

    var senderList = $$("dt").serialize();
    return senderList;
    /*
    var list = ["a", "b", "c", "d"];
    var jsonText = JSON.stringify({ list: list });

    return jsonText;
    */

}

function GetProcessResults(senderList) {
    ServerCall.Execute({ functionName: 'GetFilteredLoops', requestMessage: senderList, successCallBack: SetLoopResults, failCallBack: null });
}

function SetLoopResults(loopListResult) {
    var loopList = loopListResult.LoopList;

    if ($$("LoopDt")) {
        $$("LoopDt").clearAll();

        if (loopList) {
            for (var i = 0; i < loopList.length; i++) {
                $$("LoopDt").add(loopList[i]);
            }

            if (!$$("LoopDt").count()) {
                $$("LoopDt").showOverlay("Döngü Bulunamadı!");
            }
            else {
                $$("LoopDt").hideOverlay();
            }
        }
    }
    else {
        var loopGrid = webix.ui(
            {
                id: "LoopDt",
                container: "LoopList",
                view: "datatable",
                columns: [
                    { id: "LoopHtmlText", header: "İşlem Döngüsü", width: 700 }
                ],
                autoheight: true,
                autowidth: true,
                editable: false,
                fixedRowHeight:false,

                on: {
                    onBeforeLoad: function () {
                        //this.showOverlay("Loading...");
                    },
                    onAfterLoad: function () {
                        //this.hideOverlay();

                        if (!this.count())
                            this.showOverlay("Döngü Bulunamadı!");

                    }
                },

                data: loopList
            });
    }

    loopResult = loopListResult;

}

function GetSampleData() {
    ServerCall.Execute({ functionName: 'GetSampleData', requestMessage: null, successCallBack: SetGridData, failCallBack: null });
}

function LoadFile() {
    var data = new FormData();

    var files = $("#FileUpload").get(0).files;

    if (files.length > 0) {
        data.append("UploadedFile", files[0]);
    }

    var ajaxRequest = $.ajax({
        type: "POST",
        url: $('#RootAddress').val() + '/FileUploader.ashx',
        contentType: false,
        processData: false,
        data: data,
        success: function (fileRecords) {
            if (fileRecords) {
                SetFileRecords(fileRecords);
            }
        },
        fail: function (msg) {
            alert(msg);
        }
    });
}

function SetFileRecords(path) {
    ServerCall.Execute({ functionName: 'GetFileRecords', requestMessage: path, successCallBack: SetGridData, failCallBack: null });
}

function GetSampleDownloadLink() {
    ServerCall.Execute({ functionName: 'GetSampleDownloadLink', requestMessage: null, successCallBack: null, failCallBack: null });
}

function GetNetworkOptions()
{
    var options = {
        
        nodes: {
            shape: 'dot',
            scaling:{
                label: {
                    min:8,
                    max:20
                }
            },
            shadow: {
                "enabled": true
            }
        },

        "edges": {
            "arrows": {
                "to": {
                    "enabled": true
                }
            },
            "scaling": {
                "min": 2,
                "max": 10
            },
            "shadow": {
                "enabled": true
            }
        }
    }

    return options;
}

function SetChequeNetwork() {
    var loopListResult = loopResult;

    if (loopListResult && loopListResult.NodeList) {
        var nodeList = loopListResult.NodeList;
        var networkList = loopListResult.NetworkList;

        var nodeArr = new Array();

        for (var i = 0; i < nodeList.length; i++) {
            var node = { id: nodeList[i].Id, label: nodeList[i].Name, value: nodeList[i].Value, title: nodeList[i].Text};
            nodeArr.push(node);
        }

        var edges = new Array();

        for (var i = 0; i < networkList.length; i++) {
            var edge = { from: networkList[i].Sender, to: networkList[i].Receiver, value: networkList[i].NodeValue, title: networkList[i].EdgeTitle };
            edges.push(edge);
        }


        // create a network
        var container = document.getElementById('ChequeNetwork');

        // provide the data in the vis format
        var data = {
            nodes: nodeArr,
            edges: edges
        };
        var options = GetNetworkOptions();

        // initialize your network!
        var network = new vis.Network(container, data, options);
    }
}

function InitSlidingMenu() {
    var slidingItems = new Array();

    var senderList = { MenuId: 'SenderList', OnStart: null, OnComplete: null, Title: 'Gönderen Alıcı Listesi' };
    var loopList = { MenuId: 'LoopList', OnStart: ProcessSenderList, OnComplete: null, Title: 'İşlem Döngüsü' };
    var networkContainer = { MenuId: 'NetworkContainer', OnStart: SetChequeNetwork, OnComplete: null, Title: 'Gönderen Alıcı Ağı' };

    slidingItems.push(senderList);
    slidingItems.push(loopList);
    slidingItems.push(networkContainer);

    var conf = {
        MenuItems: slidingItems,
        MainContainer: 'SlidingContainer',
        OnNext: NavigateNext,
        OnPrev: NavigatePrev,
        OnNavigate: NavigateTo,
        MenuWidth: '100%',
        SideNavSpeed: 1000,
        TopNavSpeed: 500
    };

    Content.CreateSlidingMenu(conf);

    function NavigateNext() {
        Content.NavigateNext();
    }

    function NavigatePrev() {
        Content.NavigatePrev();
    }

    function NavigateTo(i) {
        Content.NavigateTo(i);
    }
}