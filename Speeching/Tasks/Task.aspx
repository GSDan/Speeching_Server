<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpages/Main.Master" AutoEventWireup="true" CodeBehind="Task.aspx.cs" Inherits="Speeching.Tasks.Task" %>
<%@ Register Src="~/UserControls/TaskControl.ascx" TagPrefix="uc1" TagName="TaskControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:TaskControl runat="server" Id="TaskControl1" />
</asp:Content>
