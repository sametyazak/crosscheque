<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LinkedListLoop.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="lib/webix/webix.css" rel="stylesheet" />
    <link href="lib/webix/webix2.css" rel="stylesheet" />
    <script src="lib/webix/webix.js"></script>
    <script src="lib/jquery/jquery-2.1.4.min.js"></script>
    <script src="Default.js"></script>
</head>
<body>
    <script type="text/javascript" charset="utf-8">
        var senderGrid = null;

        $(document).ready(function () {
            var senderList = GetSenderList();
            SetPageEvents();
        });

    </script>
    <form id="DefaultForm" runat="server">
        <div id="header">
            <h1>Cross Cheque</h1>
        </div>
        <div id="Main">
            <div id="LoopAction">
                <div class="webix_view webix_control webix_el_button" view_id="$Process" style="display: inline-block; vertical-align: top; border-width: 0px; margin-top: 8px; margin-left: -1px; width: 175px; height: 30px;">
                    <div class="webix_el_box" style="width: 175px; height: 30px">
                        <button type="button" class="webixtype_base" id="btnProcessSenderList">Process</button>
                    </div>
                </div>

            </div>
            <div id="SenderList"></div>

            <div id="LoopList"></div>
        </div>

    </form>

</body>
</html>
