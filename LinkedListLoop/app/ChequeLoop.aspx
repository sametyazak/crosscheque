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
    <div class="content-primary__container">
        <div class="MainPageBody">
            <div id="SlidingContainer">
            </div>

            <div id="SenderList"></div>
            <div id="LoopList"></div>

            <div id="NetworkContainer">
                <div class="ColumnHeader">
                    Gönderen/Alıcı Ağı
                </div>
                <div id="ChequeNetwork">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
