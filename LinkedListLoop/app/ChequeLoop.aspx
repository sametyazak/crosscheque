<%@ Page Title="Cross Cheque - Demo" Language="C#" MasterPageFile="~/CrossCheque.Master" AutoEventWireup="true" CodeBehind="ChequeLoop.aspx.cs" Inherits="LinkedListLoop.ChequeLoop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="ChequeLoop.js"></script>
    <link href="../lib/vis/vis.css" rel="stylesheet" />
    <script src="../lib/vis/vis.js"></script>

    <style>
        .overall {
            height: 100%;
            line-height: 26px;
        }

        .name {
            height: 100%;
            float: left;
            overflow: hidden;
        }

        .size {
            padding: 0 10px;
            width: 100px;
            text-align: right;
            float: right;
        }

        .remove_file {
            float: right;
            width: 15px;
            padding-left: 10px;
        }

        .status {
            float: right;
            position: relative;
            margin-top: 4px;
            width: 80px;
            height: 16px;
            line-height: 16px;
            border: 1px solid #A4BED4;
            border-radius: 2px;
            -moz-border-radius: 2px;
            -webkit-border-radius: 2px;
        }

        .progress {
            height: 100%;
            position: absolute;
            background-color: #b8e6ff;
        }

        .message {
            z-index: 1;
            width: 100%;
            text-align: center;
            position: absolute;
        }

            .message.error {
                color: #e83b3b;
            }

        #mynetwork {
            width: 900px;
            height: 900px;
            border: 1px solid lightgray;
        }

        #loadingBar {
            position: absolute;
            top: 0px;
            left: 0px;
            width: 902px;
            height: 902px;
            -webkit-transition: all 0.5s ease;
            -moz-transition: all 0.5s ease;
            -ms-transition: all 0.5s ease;
            -o-transition: all 0.5s ease;
            transition: all 0.5s ease;
            opacity: 1;
        }

        #text {
            position: absolute;
            top: 0px;
            left: 530px;
            width: 30px;
            height: 50px;
            margin: auto auto auto auto;
            font-size: 22px;
            color: #000000;
        }


        div.outerBorder {
            position: relative;
            top: 400px;
            width: 600px;
            height: 44px;
            margin: auto auto auto auto;
            border: 8px solid rgba(0,0,0,0.1);
            background: rgb(252,252,252); /* Old browsers */
            background: -moz-linear-gradient(top, rgba(252,252,252,1) 0%, rgba(237,237,237,1) 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(252,252,252,1)), color-stop(100%,rgba(237,237,237,1))); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, rgba(252,252,252,1) 0%,rgba(237,237,237,1) 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, rgba(252,252,252,1) 0%,rgba(237,237,237,1) 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, rgba(252,252,252,1) 0%,rgba(237,237,237,1) 100%); /* IE10+ */
            background: linear-gradient(to bottom, rgba(252,252,252,1) 0%,rgba(237,237,237,1) 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#fcfcfc', endColorstr='#ededed',GradientType=0 ); /* IE6-9 */
            border-radius: 72px;
            box-shadow: 0px 0px 10px rgba(0,0,0,0.2);
        }

        #border {
            position: absolute;
            top: 3px;
            left: 10px;
            width: 500px;
            height: 23px;
            margin: auto auto auto auto;
            box-shadow: 0px 0px 4px rgba(0,0,0,0.2);
            border-radius: 10px;
        }

        #bar {
            position: absolute;
            top: 0px;
            left: 0px;
            width: 20px;
            height: 20px;
            margin: auto auto auto auto;
            border-radius: 11px;
            border: 2px solid rgba(30,30,30,0.05);
            background: rgb(0, 173, 246); /* Old browsers */
            box-shadow: 2px 0px 4px rgba(0,0,0,0.4);
        }

        #ResultPerformansContainer {
            position: fixed;
            top: 100px;
            right: 20px;
            background-color: #FFFF66;
            opacity: 0.8;
            height: 350px;
            width: 250px;
            display: none;
        }
    </style>
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
    <div class="content-primary__container">
        <div class="MainPageBody">

            <div id="SlidingContainer">
            </div>

            <div id="loadingBar" style="display: none;">
                <div class="outerBorder">
                    <div id="text">0%</div>
                    <div id="border">
                        <div id="bar"></div>
                    </div>
                </div>
            </div>

            <div id="TransactionTypeContainer"></div>

            <div id="RecordsContainer">
                <div id="RecordUploadContainer"></div>
                <div id="SenderList" style="margin-top: 30px;"></div>
            </div>

            <div id="ResultContainer">
                <div class="network-result-message" id="ResultMessageContainer"></div>
            </div>



        </div>
    </div>
    <div id="ResultPerformansContainer">
    </div>
</asp:Content>
