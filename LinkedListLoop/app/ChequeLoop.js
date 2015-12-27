var loopResult = new Object();
var nodesDataset = new Object();
var edgesDataset = new Object();
var allNodes, allNodesOriginal;
var highlightActive = false;
var page = {};

function InitializePage() {
    InitTransactionTypes();

    InitSlidingMenu();
    //GetSenderList();
    //CreateSenderList();
    //GetSampleData();
    //CreateLoopList();
    SetPageEvents();
    //InitTopMenu();
    //InitContainers();
    //InitFileUpload();
    InitResultBox();
}

function InitResultBox()
{
    webix.ui({
        container: "ResultContainer",
        multi: true,
        view: "accordion",
        width:1000,
        rows: [
            {
                header: Global.GetString('LoopList'),
                body: {
                    id: "LoopDt",
                    view: "list",
                    template: "#LoopHtmlText# <span class='navnetwork'>" + Global.GetString('SeeOnNetwork') + "</span>",
                    data: [{ LoopHtmlText: 'test' }],
                    scrollY: true,
                    select: true,
                    height: 300,
                    onClick: {
                        navnetwork: function (e, id) {
                            SetLoopHighlight(this.getItem(id));
                            return false;
                        }
                    }
                }
            },
            {
                header: Global.GetString('SenderReceiverNetwork'),
                body: "<div class='cheque-network' id='ChequeNetwork'></div>",
                height: 600
            }
        ]
    });
}

function uploadFiles() {
    $$("upl1").send();
}

function cancel() {
    var id = $$("upl1").files.getFirstId();
    while (id) {
        $$("upl1").stopUpload(id);
        id = $$("upl1").files.getNextId(id);
    }
}

function GetUserFiles() {
    if (ValidateTranType()) {
        var tranType = GetSelectedTransction();
        return ServerCall.Execute({ functionName: 'GetUserFiles', requestMessage: tranType, successCallBack: null, failCallBack: null, async: false });
    }
}

function InitFileUpload() {
    if ($$('RecordUploadLayout'))
        $$('RecordUploadLayout').destructor();

    var template = new Template();
    var uploadPanel = new template.FileUpload({ uploadClick: uploadFiles, cancelClick: cancel, queryString: 'tranType=' + GetSelectedTransction() });
    var uploaderTypeConfig = uploadPanel.GetUploaderTypeConfig();
    var uploaderBodyConfig = uploadPanel.GetConfig();

    webix.type(webix.ui.list, uploaderTypeConfig);

    var sampleForm = GetSampleDataForm();
    var fileList = GetUserFiles();

    var content = webix.ui({
        container: "RecordUploadContainer",
        id: "RecordUploadLayout",
        //height: 400,
        width: 1020,
        borderless: true,
        view: "tabview",
        cells: [
            {
                header: Global.GetString('UploadFile'),
                body: uploaderBodyConfig,
                height: 240
            },
            {
                header: Global.GetString('LoadSampleData'),
                body: {
                    id: "LoadSampleDataCol",
                    view: "form",
                    type: "line",
                    rows: sampleForm,
                    padding: 20,
                    //width: 500
                },
                height: 100,
                collapsed: true
            },
            {
                header: Global.GetString('HistoryRecords'),
                body: {
                    view: "datatable",
                    id: "HistoryRecords",
                    columns: [
                        { id: "Id", header: Global.GetString('Id'), width: 230 },
                        { id: "FileName", header: Global.GetString('FileName'), width: 200 },
                        { id: "RecordCount", header: Global.GetString('RecordCount'), width: 90 },
                        { id: "TransactionType", header: Global.GetString('TransactionType'), width: 90 },
                        { id: "InsertTime", header: Global.GetString('InsertTime'), width: 150 },
                        { id: "UserInfo", header: Global.GetString('UserInfo'), width: 130 },
                        {
                            id: "",
                            template: "<input class='uploadbtn' type='button' value='" + Global.GetString('Upload') + "' />",
                            css: "padding_less",
                            width: 100
                        }

                    ],
                    select: true,
                    //autoheight: true,
                    height: 240,
                    autowidth: true,
                    scrollY: true,
                    data: fileList
                }
            }

        ]
    });

    content.show();

    $$('HistoryRecords').on_click.uploadbtn = function (e, id, trg) {
        var selectedItem = $$("HistoryRecords").getItem(id);

        if (selectedItem)
        {
            SetFileRecords(selectedItem.Id);
        }

        return false;
    };

    $$("upl1").attachEvent("onUploadComplete", function (response) {
        SetFileRecords(response.FileInfo.Id);
    });

    $$("upl1").attachEvent("onBeforeFileAdd", function (file) {
        var isUploadCheck = ServerCall.Execute({ functionName: 'CheckFileUpload', requestMessage: file.size, successCallBack: null, failCallBack: null, async: false });

        if (!isUploadCheck.IsUploadValid)
        {
            webix.message({ type: "error", text: isUploadCheck.Message });
        }

        return isUploadCheck.IsUploadValid;
    });
}

function GetSampleDataForm() {
    var sampleDataForm = [];

    sampleDataForm.push(
        {
            view: "button", value: Global.GetString('LoadSampleData'), width: 160, align: "center", click: function () {
                GetSampleData();
            }
        }
    );

    return sampleDataForm;
}

function CreateTransactionTypes(tranTypeList) {
    webix.ui({
        container: "TransactionTypeContainer",
        rows: [
            {
                cols: [
                    {
                        header: Global.GetString('TransactionList'), body:
                            {
                                view: "list",
                                id: 'TransactionTypes',
                                height: 400,
                                template: "{common.getText()}",
                                type: {
                                    height: 55,
                                    getText: function (obj) {
                                        return Global.GetString(obj.Value)
                                    }
                                },

                                select: true,

                                data: tranTypeList
                            }
                    },
                ]
            }
        ]
    });

}

function InitTransactionTypes() {
    ServerCall.Execute({ functionName: 'GetTransactionTypeList', requestMessage: null, successCallBack: CreateTransactionTypes, failCallBack: null });
}

function InitContainers() {
    var networkOptions = {
        containerId: 'NetworkContainer',

        header: {
            text: 'SenderReceiverNetwork'
        },

        detail: {
            id: 'ChequeNetwork',
            css: 'cheque-network'
        }
    };

    var template = new Template();
    var network = new template.Container(networkOptions);
}

function InitTopMenu() {
    var topMenuConfig = {
        id: 'TopNavUl',
        containerId: 'TopNavUlContainer',
        listItems: [
            {
                name: 'Process', contentCss: 'top-menu-process', text: '', css: 'nav-secondary__item process',
                onClick: ProcessSenderList
            },
            {
                name: 'TopMenuSample', contentCss: 'top-menu-sample', text: ML.SampleData,
                onClick: function (e) {
                    var offset = $(this).offset();
                    ToggleChildDiv('SampleDataContainer', offset);
                }
            },
            {
                name: 'TopMenuUpload', contentCss: 'top-menu-upload', text: ML.UploadFile,
                onClick: function (e) {
                    var offset = $(this).offset();
                    ToggleChildDiv('UploadDataContainer', offset);
                }
            }
        ]
    }

    var topMenu = new TopMenu(topMenuConfig);
}

function GetSenderList() {
    ServerCall.Execute({ functionName: 'GetChequeListJson', requestMessage: null, successCallBack: SetGridData, failCallBack: null });
}

function CreateSenderList() {
    SetGridData([]);
}

function CreateLoopList() {
    SetLoopResults([]);
}

function SetGridData(senderList) {
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

    }
}

function SetPageEvents() {

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
    if (ValidateProcessFile()) {
        var loopList = GetProcessResults(page.FileId);
    }
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

function GetProcessResults(fileId) {
    if (ValidateTranType()) {
        var tranType = GetSelectedTransction();
        ServerCall.Execute({ functionName: 'ProcessSenderFile', requestMessage: { TranTypeName: tranType, FileId: fileId }, successCallBack: SetProcessResults, failCallBack: null, LogRequest: false });
    }
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
                $$("LoopDt").add({ LoopHtmlText: Global.GetString('NoLoopFound') });
            }
        }
    }

    loopResult = loopListResult;

}

function GetSampleData() {
    if (ValidateTranType()) {
        var tranType = GetSelectedTransction();
        ServerCall.Execute({ functionName: 'GetSampleData', requestMessage: tranType, successCallBack: SetFileRecords, failCallBack: null });
    }
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

function SetFileRecords(fileId) {
    if (ValidateTranType()) {
        page.FileId = fileId;

        var tranType = GetSelectedTransction();
        ServerCall.Execute({ functionName: 'GetFileRecords', requestMessage: { FileId: fileId, TranTypeName: tranType, DeleteFile: true }, successCallBack: SetGridData, failCallBack: null });
    }
}

function GetSampleDownloadLink() {
    ServerCall.Execute({ functionName: 'GetSampleDownloadLink', requestMessage: null, successCallBack: null, failCallBack: null });
}

function GetNetworkOptions() {
    var options = {

        nodes: {
            shape: 'dot',
            scaling: {
                label: {
                    min: 8,
                    max: 20
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
            },
            "smooth": {
                "type": 'continuous'
            }
        },
        physics: {
            stabilization: {
                enabled: true,
                iterations: 2000,
                updateInterval: 25
            }
        }
    };

    return options;
}

function SetChequeNetwork() {
    var loopListResult = loopResult;

    if (loopResult)
    {
        $('#ResultMessageContainer').html(loopResult.Message);
    }

    if (loopListResult && loopListResult.NetworkList) {
        var nodes = new Object();
        var nodesArr = new Array();
        var edges = loopListResult.NetworkList;
        var networkList = loopListResult.NetworkList;

        if (networkList.length > 0)
        {
            document.getElementById('loadingBar').style.display = 'block';
        }

        edgesDataset = new vis.DataSet(edges);

        for (var i = 0; i < networkList.length; i++) {
            var nodeCounter = 0;

            networkList[i].title = 'Edge'

            if (!nodes[networkList[i].from])
            {
                var nodeFrom = {
                    id: networkList[i].from, label: networkList[i].from, value: networkList[i].value, title: 'Node' /*networkList[i].title*/,
                    color: { background: 'rgba(123,170,255,1)', border: 'rgba(49,115,233,1)' },
                    originalColor: { background: 'rgba(123,170,255,1)', border: 'rgba(49,115,233,1)' }
                };

                nodes[networkList[i].from] = nodeFrom;
                nodesArr.push(nodeFrom);
            }

            if (!nodes[networkList[i].to])
            {
                var nodeTo = {
                    id: networkList[i].to, label: networkList[i].to, value: networkList[i].value, title: 'Node' /*networkList[i].title*/,
                    color: { background: 'rgba(123,170,255,1)', border: 'rgba(49,115,233,1)' },
                    originalColor: { background: 'rgba(123,170,255,1)', border: 'rgba(49,115,233,1)' }
                };

                nodes[networkList[i].to] = nodeTo;
                nodesArr.push(nodeTo);
            }
        }


        // create a network
        var container = document.getElementById('ChequeNetwork');

        nodesDataset = new vis.DataSet(nodesArr);

        // provide the data in the vis format
        var data = {
            nodes: nodesDataset,
            edges: edgesDataset
        };
        var options = GetNetworkOptions();

        // initialize your network!
        var network = new vis.Network(container, data, options);

        allNodes = nodesDataset.get({ returnType: "Object" });
        allNodesOriginal = allNodes;

        network.on("click", ResetNetworkHighlight);

        network.on("stabilizationProgress", function (params) {
            var maxWidth = 496;
            var minWidth = 20;
            var widthFactor = params.iterations / params.total;
            var width = Math.max(minWidth, maxWidth * widthFactor);

            document.getElementById('bar').style.width = width + 'px';
            document.getElementById('text').innerHTML = Math.round(widthFactor * 100) + '%';
        });
        network.once("stabilizationIterationsDone", function () {
            document.getElementById('text').innerHTML = '100%';
            document.getElementById('bar').style.width = '496px';
            document.getElementById('loadingBar').style.opacity = 0;
            // really clean the dom element
            setTimeout(function () { document.getElementById('loadingBar').style.display = 'none'; }, 500);
        });

        SetPerformansResult(loopListResult)
    }
}

function ResetNetworkHighlight(params)
{
    if (params.nodes.length == 0 && highlightActive) {
        for (var nodeId in allNodes) {
            allNodes[nodeId].color = allNodes[nodeId].originalColor;
            if (allNodes[nodeId].hiddenLabel !== undefined) {
                allNodes[nodeId].label = allNodes[nodeId].hiddenLabel;
                allNodes[nodeId].hiddenLabel = undefined;
            }
        }

        highlightActive = false;

        UpdateNetworkHighlight();
    }
}

function ValidateTranType() {
    var item = GetSelectedTransction();

    if (!item) {
        webix.message('SelectTransactionType');
        return false;
    }

    return true;
}

function GetSelectedTransction() {
    var tranTypeId = $$('TransactionTypes').getSelectedId();

    if (tranTypeId) {
        return $$("TransactionTypes").getItem(tranTypeId).Name;
    }

    return null;
}

function InitSlidingMenu() {
    var slidingItems = new Array();

    var tranType = {
        MenuId: 'TransactionTypeContainer',
        OnStart: null,
        OnValidate: function () {
            return ValidateTranType();
        },
        Title: Global.GetString('TranType'),
        Hint: Global.GetString('SelectTranType')
    };

    var upload = {
        MenuId: 'RecordsContainer',
        OnStart: InitAfterTranTypeSelect,
        OnValidate: function () {
            var senderList = GetGridSenderList();

            if (senderList && senderList.length > 0) {
                return true;
            }
            else {
                webix.message(Global.GetString('LoadAnyDataToContinue'));
                return false;
            }
        },
        Title: Global.GetString('RecordUploadContainer'),
        Hint: Global.GetString('SelectUploadType')
    };

    var senderList = {
        MenuId: 'SenderList',
        OnStart: GetSenderGridColumns,
        OnValidate: ValidateProcessFile,
        Title: ML.SenderReceiverList,
        Hint: Global.GetString('UploadAFileOrLoadSampleData')
    };

    //var loopList = { MenuId: 'LoopList', OnStart: ProcessSenderList, OnValidate: null, Title: ML.TransactionLoop, Hint: '' };
    //var networkContainer = { MenuId: 'NetworkContainer', OnStart: SetChequeNetwork, OnValidate: null, Title: ML.SenderReceiverNetwork, Hint: '' };

    var resultBox = { MenuId: 'ResultContainer', OnStart: ProcessResults, OnValidate: null, Title: Global.GetString('ProcessResult'), Hint: '' };

    slidingItems.push(tranType);
    slidingItems.push(upload);
    //slidingItems.push(senderList);
    slidingItems.push(resultBox);

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

function InitAfterTranTypeSelect()
{
    InitFileUpload();
    GetSenderGridColumns();
}

function ProcessResults()
{
    ProcessSenderList();
}

function SetPerformansResult(processResult)
{
    if (processResult && processResult.CanSeePerformanceResult)
    {
        var performancePanel = $('#ResultPerformansContainer');
        performancePanel.html('');
        performancePanel.show();

        for (performanceItem in processResult.ProcessPerformace)
        {
            var perfObj = processResult.ProcessPerformace[performanceItem];
            var perfContent = $('<div class="performance-item"></div>');
            perfContent.html('<strong>' + perfObj.Key + ':</strong> ' + perfObj.Value);

            performancePanel.append(perfContent);
        }
    }
}

function SetProcessResults(loopListResult)
{
    SetLoopResults(loopListResult);
    SetChequeNetwork();
}

function GetSenderGridColumns() {
    if (ValidateTranType()) {
        ServerCall.Execute({
            functionName: 'GetTransactionTypeColumnList', requestMessage: GetSelectedTransction(),
            successCallBack: InitSenderGrid, failCallBack: null
        });
    }
}

function InitSenderGrid(columnList) {
    if ($$('GridLayout'))
        $$('GridLayout').destructor();


    var columns = [];

    for (var i = 0; i < columnList.length; i++) {
        var columnItem = { id: columnList[i].Name, editor: columnList[i].EntryType, header: Global.GetString(columnList[i].Header), width: columnList[i].Width };
        columns.push(columnItem);
    }

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
        height: 800,
        autowidth: false,
        columns: columns,
        editaction: "dblclick",
        pager: "bottomPager",
        data: []
    };

    var layoutRows = [
          {
              view: "template", css: "headline", height: 0
          },
          grid,
          {
              type: "clear", height: 46, paddingY: 8, cols: [
                  {
                      view: "pager", id: "bottomPager", size: 11, width: 200
                  },
                  {

                  },
                  {
                      view: "button", value: ML.Add, width: 100, click: function () {
                          var id = $$("dt").add({ Sender: "", Receiver: "", Amount: 0, Date: "2014-01-01" });
                          $$("dt").editCell(id, "Sender");
                      }
                  },
                  {
                      view: "button", value: ML.Delete, width: 100, click: function () {
                          $$("dt").remove($$("dt").getSelectedId(true));
                      }
                  },
                  {
                      view: "button", value: ML.Clean, width: 100, click: function () {
                          $$("dt").clearAll();
                          $$("LoopDt").clearAll();
                      }
                  },
              ]
          }
    ]

    var gridConfig = {
        id: 'GridLayout',
        container: "SenderList",
        type: "clear",
        rows: layoutRows
    };

    //return gridConfig;
    senderGrid = webix.ui(gridConfig);
}

function SetLoopHighlight(selectedListItem)
{
    //ResetNetworkHighlight({ nodes: [{}]});

    if (!selectedListItem || !selectedListItem.Loop)
    {
        return false;
    }

    for (var nodeId in allNodes) {
        allNodes[nodeId].hiddenColor = allNodes[nodeId].color;
        allNodes[nodeId].color = { background: 'rgba(200,200,200,0.6)', border: 'rgba(200,200,200,0.6)' };
        if (allNodes[nodeId].hiddenLabel === undefined) {
            allNodes[nodeId].hiddenLabel = allNodes[nodeId].label;
            allNodes[nodeId].label = undefined;
        }
    }

    for (i = 0; i < selectedListItem.Loop.length; i++) {
        var loopItem = selectedListItem.Loop[i];

        allNodes[loopItem].color = { background: 'rgba(215, 40, 40, 0.9)', border: 'rgba(215, 40, 40, 0.9)' };
        if (allNodes[loopItem].hiddenLabel !== undefined) {
            allNodes[loopItem].label = allNodes[loopItem].hiddenLabel;
            allNodes[loopItem].hiddenLabel = undefined;
        }
    }

    highlightActive = true;
    
    UpdateNetworkHighlight();
}

function UpdateNetworkHighlight()
{
    var updateArray = [];

    for (nodeId in allNodes) {
        if (allNodes.hasOwnProperty(nodeId)) {
            updateArray.push(allNodes[nodeId]);
        }
    }

    nodesDataset.update(updateArray);
}

function ValidateProcessFile()
{
    var senderList = GetGridSenderList();
    if (senderList && senderList.length > 0 && page.FileId) {
        return true;
    }
    else {
        webix.message(Global.GetString('LoadAnyDataToContinue'));
        return false;
    }
}