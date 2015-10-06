<%@ Page Title="Cross Cheque - Ana Sayfa" Language="C#" MasterPageFile="~/CrossCheque.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LinkedListLoop.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $('#TopMenu').hide();
            SetMenuLayout();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CrossChequeBody" runat="server">
    <div class="MainPageBody">
        <div>
            <iframe width="560" height="315" src="https://www.youtube.com/embed/X3HeZCur-hI" frameborder="0" allowfullscreen></iframe>
        </div>
    </div>
</asp:Content>
