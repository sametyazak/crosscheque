<%@ Page Title="Cross Cheque - Log" Language="C#" MasterPageFile="~/CrossCheque.Master" AutoEventWireup="true" CodeBehind="Log.aspx.cs" Inherits="LinkedListLoop.app.Log" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Log.js"></script>

    <script type="text/javascript" charset="utf-8">

        $(document).ready(function () {
            InitializePage();
        });

    </script>

    <style>
        .log-info {
            background-color: #27ae60;
            color: #fff;
        }

        .log-error {
            background-color: #FFAAAA;
        }

        .log-debug {
            background-color: #EFF51A;
        }

        .log-fatal {
            background-color: #800000;
            color: #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CrossChequeBody" runat="server">
    <div class="grid grid--log">
        <div class="log-main">
            <div id="LogContainer"></div>

        </div>

        <div class="log-clear">
        </div>

        <div class="log-detail">
            <div id="Test" class="ColumnHeader">
                Log Detay
            </div>
            <pre>
                <div id="LogDetail">

                </div>
            </pre>
        </div>

    </div>
</asp:Content>
