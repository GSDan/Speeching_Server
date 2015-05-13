<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpages/Main.Master" AutoEventWireup="true" CodeBehind="Tasks.aspx.cs" Inherits="Speeching.Tasks.Tasks" %>

<%@ Register Src="~/UserControls/TasksControl.ascx" TagPrefix="uc1" TagName="TasksControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:TasksControl runat="server" ID="TasksControl" />
</asp:Content>
