<%@ Page Title="Cross Cheque - Ana Sayfa" Language="C#" MasterPageFile="~/CrossCheque.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LinkedListLoop.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            //$('#TopMenu').hide();
            SetMenuLayout();

            //var slidingItems = new Array();
            //slidingItems.push('Menu1');
            //slidingItems.push('Menu2');
            //slidingItems.push('Menu3');
            //slidingItems.push('Menu4');

            //var conf = {
            //    MenuItems: slidingItems,
            //    MainContainer: 'SlidingContainer',
            //    OnNext: NavigateNext,
            //    OnPrev: NavigatePrev,
            //    OnNavigate: NavigateTo,
            //    MenuWidth: '100%',
            //    SideNavSpeed: 1000,
            //    TopNavSpeed: 500
            //};

            //Content.CreateSlidingMenu(conf);

            //function NavigateNext() {
            //    Content.NavigateNext();
            //}

            //function NavigatePrev() {
            //    Content.NavigatePrev();
            //}

            //function NavigateTo(i) {
            //    Content.NavigateTo(i);
            //}
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CrossChequeBody" runat="server">
    
        <div>
            <div style="">
                <iframe width="560" height="300" src="https://www.youtube.com/embed/X3HeZCur-hI" frameborder="0" allowfullscreen></iframe>
            </div>

            <%--<div id="SlidingContainer">
            
        </div>
        <div id="Menu1" style="background-color:blue;height:100px;">
            Menu1
        </div>
        <div id="Menu2" style="background-color:darkgreen;height:100px;">
            Menu2
        </div>
        <div id="Menu3" style="background-color:red;height:100px;">
            Menu3
        </div>
        <div id="Menu4" style="background-color:brown;height:100px;">
            Menu3
        </div>--%>
        </div>
</asp:Content>
