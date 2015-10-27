<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LinkedListLoop.app.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <link href="../../lib/webix/webix.css" rel="stylesheet" />
    <link href="../../lib/webix/webix2.css" rel="stylesheet" />
    <script src='<%= Page.ResolveUrl("~/lib/webix/webix.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/lib/jquery/jquery-2.1.4.min.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/lib/jquery/jquery-ui.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/src/client/ServerCall.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/src/client/Content.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/src/client/Core.js") %>'></script>

    <script src='<%= Page.ResolveUrl("~/app/user/Register.js") %>'></script>

    <link href="../../design/CrossCheque.css" rel="stylesheet" />
    <link href="../../design/TopMenu.css" rel="stylesheet" />
    <link href="../../design/LeftMenu.css" rel="stylesheet" />

    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            InitializePage();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="RootAddress" runat="server" />

        <div class="login-container">
            <div id="LoginArea"></div>
        </div>
    </form>
</body>
</html>
