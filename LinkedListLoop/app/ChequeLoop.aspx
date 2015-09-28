<%@ Page Title="Cross Cheque - Demo" Language="C#" MasterPageFile="~/CrossCheque.Master" AutoEventWireup="true" CodeBehind="ChequeLoop.aspx.cs" Inherits="LinkedListLoop.ChequeLoop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="ChequeLoop.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CrossChequeBody" runat="server">
    <script type="text/javascript" charset="utf-8">
        var senderGrid = null;

        $(document).ready(function () {
            InitializePage();
        });

    </script>
    <%--<div id="header">
        <h1>Cross Cheque</h1>
    </div>--%>
    <div style="height:120px;width:100%">
        
    </div>
    <div>
        <div id="Main">
            <div id="LoopAction">
                <div id="DataArea">
                    <div id="UploadArea">
                        <div class="ColumnHeader">
                            Dosya Yükle?
                        </div>
                        <div class="ColumnBody">
                            <div style="float: left; padding-top: 13px; padding-bottom: 10px;">
                                <input id="FileUpload" type="file" />
                            </div>
                            <div style="float: left; padding-bottom: 10px; padding-left: 15px;">
                                <div class="webix_view webix_control webix_el_button" style="display: inline-block; vertical-align: top; border-width: 0px; margin-top: 8px; margin-left: -1px; width: 100px; height: 30px;" view_id="$Upload">
                                    <button id="btnUploadFile" class="webixtype_base" type="button">Yükle</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="SampleArea">
                        <div>
                            <div class="ColumnHeader">
                                Örnek Veri İle İşlem?
                            </div>
                            <div class="ColumnBody">
                                <div style="float: left; height: 48px;">
                                    <div class="webix_view webix_control webix_el_button" style="display: inline-block; vertical-align: top; border-width: 0px; margin-top: 8px; margin-left: -1px; width: 150px; height: 30px;" view_id="$DownloadSample">
                                        <button id="btnDowloadSampleData" class="webixtype_base" type="button">Örnek Veri İndir</button>
                                    </div>
                                </div>

                                <div style="float: right; padding-left: 15px;">
                                    <div class="webix_view webix_control webix_el_button" style="display: inline-block; vertical-align: top; border-width: 0px; margin-top: 8px; margin-left: -1px; width: 150px; height: 30px;" view_id="$LoadSample">
                                        <button id="btnLoadSample" class="webixtype_base" type="button">Örnek Veri Yükle</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="ProcessArea">
                    <div class="webix_view webix_control webix_el_button" view_id="$Process" style="display: inline-block; vertical-align: top; border-width: 0px; margin-top: 8px; margin-left: -1px; width: 175px; height: 30px;">
                        <div class="webix_el_box" style="width: 175px; height: 30px">
                            <button type="button" class="webixtype_base" id="btnProcessSenderList">Process</button>
                        </div>
                    </div>
                </div>
            </div>
            <div id="GridArea">
                <div id="SenderList"></div>

                <div id="LoopList"></div>
            </div>

        </div>
    </div>
</asp:Content>
