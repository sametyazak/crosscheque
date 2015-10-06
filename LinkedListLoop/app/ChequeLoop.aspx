<%@ Page Title="Cross Cheque - Demo" Language="C#" MasterPageFile="~/CrossCheque.Master" AutoEventWireup="true" CodeBehind="ChequeLoop.aspx.cs" Inherits="LinkedListLoop.ChequeLoop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="ChequeLoop.js"></script>
    <link href="../lib/vis/vis.css" rel="stylesheet" />
    <script src="../lib/vis/vis.js"></script>

    <style type="text/css">
        #ChequeNetwork {
            border: 1px solid lightgray;
            height: 600px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CrossChequeBody" runat="server">
    <script type="text/javascript" charset="utf-8">
        var senderGrid = null;

        $(document).ready(function () {
            InitializePage();
            ToogleLeftMenu();
        });

    </script>
    <%--<div id="header">
        <h1>Cross Cheque</h1>
    </div>--%>


    <div class="grid grid--has-2">
        <div class="grid-item">
            <div class="article-loop">
                <div id="SenderList"></div>

            </div>
        </div>

        <div class="grid-item">
            <div class="article-loop">
                <div id="LoopList"></div>
            </div>
        </div>

    </div>

    <%--<div>
        <div id="Main">
            <div id="LoopAction">
                <div id="DataArea">
                    <div id="UploadArea">
                        <div class="ColumnHeader">
                            Dosya Yükle?
                        </div>
                        <div class="ColumnBody">
                            <div style="float: left; padding-top: 13px; padding-bottom: 10px;">
                            </div>
                            <div style="float: left; padding-bottom: 10px; padding-left: 15px;">
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
                                </div>

                                <div style="float: right; padding-left: 15px;">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="ProcessArea">
                    
                </div>
            </div>
            <div id="GridArea">
            </div>

        </div>
    </div>--%>

    <div>
        <div class="ColumnHeader">
            Gönderen/Alıcı Ağı
        </div>
        <div id="ChequeNetwork">
        </div>
    </div>

</asp:Content>
