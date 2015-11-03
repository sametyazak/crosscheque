<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LinkedListLoop.app.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="../../lib/webix/webix.css" rel="stylesheet" />
    <link href="../../lib/webix/webix2.css" rel="stylesheet" />
    <script src='<%= Page.ResolveUrl("~/lib/webix/webix.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/lib/jquery/jquery-2.1.4.min.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/lib/jquery/jquery-ui.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/src/client/ServerCall.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/src/client/Content.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/src/client/Core.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/src/client/Template.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/src/client/Global.js") %>'></script>

    <script src='<%= Page.ResolveUrl("~/app/User/Login.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/src/client/lang/" + CurrentCulture.Value + ".js") %>'></script>

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
        <asp:HiddenField ID="CurrentCulture" runat="server" />

        <div id="LanguageContainer" style="position:absolute; top:10px; right:10px;">

        </div>

        <div class="login-container">
            <div id="LoginArea"></div>
            <div style="text-align: center">
                <a href="Register.aspx">Kayıt ol </a>
                <a href="#" id="GuestLogin" style="padding-left: 20px;">Misafir Girişi</a>
            </div>
        </div>
    </form>
</body>
</html>
