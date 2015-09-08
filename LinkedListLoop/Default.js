﻿function GetSenderList() {
    var senderList = null;

    $.ajax({
        type: "POST",
        url: "Default.aspx/GetChequeListJson",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            senderList = msg.d;
            SetGridData(senderList);
        },
        fail: function (msg) {
            alert(msg);
        }
    });

    return senderList;
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

    var grid = {
        view: "datatable",
        id: "dt",
        scrollY: false,
        select: "row",
        navigation: true,
        yCount: 10,
        editable: true,
        math: true,
        autoheight: true,
        columns: [
            { id: "Sender", editor: "text", header: "Gönderen", width: 100 },
            { id: "Receiver", editor: "text", header: "Alıcı", width: 100 },
            { id: "Amount", editor: "text", header: "Tutar", width: 80 },
            { id: "Date", editor: "date", header: "Tarih", width: 100 },
        ],
        editaction: "dblclick",
        pager: "bottomPager",
        ready: function () {
            webix.UIManager.setFocus(this);
            this.select("4");
        },
        data: senderList,

        on: {
            onBeforeLoad: function () {
                this.showOverlay("Loading...");
            },
            onAfterLoad: function () {
                this.hideOverlay();
            }
        }
    };

    senderGrid = webix.ui({
        container: "SenderList",
        type: "clear",
        rows: [
          { view: "template", css: "headline", height: 5 },
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
                  }
              },
              ]
          }
        ]
    });
}

function SetPageEvents() {
    $('#btnProcessSenderList').click(
        function () {
            ProcessSenderList();
        }
    );
}

function ProcessSenderList() {
    var senderList = GetGridSenderList();
    var loopList = GetProcessResults(senderList);
}

function GetGridSenderList() {

    var senderList = $$("dt").serialize();
    return JSON.stringify({ senderList: senderList });
    /*
    var list = ["a", "b", "c", "d"];
    var jsonText = JSON.stringify({ list: list });

    return jsonText;
    */

}

function GetProcessResults(senderList) {
    $.ajax({
        type: "POST",
        url: "Default.aspx/GetFilteredLoops",
        data: senderList,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (msg) {;
            SetLoopResults(msg.d);
        },
        fail: function (msg) {
            alert(msg);
        }
    });
}

function SetLoopResults(loopList) {
    if ($$("LoopDt")) {
        $$("LoopDt").clearAll();

        if (loopList) {
            for (var i = 0; i < loopList.length; i++) {
                $$("LoopDt").add(loopList[i]);
            }

            if (!$$("LoopDt").count())
            {
                $$("LoopDt").showOverlay("Döngü Bulunamadı!");
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
                    { id: "LoopHtmlText", header: "İşlem Döngüsü", width: 500 }
                ],
                autoheight: true,
                autowidth: true,
                editable: false,

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

}