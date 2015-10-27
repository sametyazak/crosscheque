<%@ Page Title="Cross Cheque - Şifre Güncelleme" Language="C#" MasterPageFile="~/CrossCheque.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="LinkedListLoop.app.user.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="ChangePassword.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CrossChequeBody" runat="server">
    <script type="text/javascript" charset="utf-8">

        $(document).ready(function () {
            InitializePage();
            ToogleLeftMenu();
        });

    </script>
    <div class="content-primary__container">
        <div class="MainPageBody">
            
            <div id="ManageUserContainer" class="center-content">
            </div>

        </div>
    </div>
</asp:Content>