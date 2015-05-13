<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskControl.ascx.cs" Inherits="Speeching.UserControls.TaskControl" %>

<asp:MultiView ID="mvTaskStages" runat="server" ActiveViewIndex="0">
    <asp:View ID="v1" runat="server">
        <asp:Literal ID="litDesc1" runat="server"></asp:Literal>
        <asp:Literal ID="litSound1" runat="server"></asp:Literal>
        <asp:TextBox ID="txtTrans1" runat="server"></asp:TextBox>
    </asp:View>
    <asp:View ID="v2" runat="server">
        <asp:Literal ID="litDesc2" runat="server"></asp:Literal>
        <asp:Literal ID="litSound2" runat="server"></asp:Literal>
        <asp:TextBox ID="txtTrans2" runat="server"></asp:TextBox>
    </asp:View>
    <asp:View ID="v3" runat="server">
        <asp:Literal ID="litDesc3" runat="server"></asp:Literal>
        <asp:Literal ID="litSound3" runat="server"></asp:Literal>
        <asp:TextBox ID="txtTrans3" runat="server"></asp:TextBox>
    </asp:View>
    <asp:View ID="v4" runat="server">
        <asp:Literal ID="litDesc4" runat="server"></asp:Literal>
        <asp:Literal ID="litSound4" runat="server"></asp:Literal>
        <asp:TextBox ID="txtTrans4" runat="server"></asp:TextBox>
    </asp:View>
</asp:MultiView>
<asp:Button runat="server" ID="btnNext" OnClick="btnNext_Click" Text="Next" />

<audio id="demo" controls loop>
    <source src="../testUpload/35696.mp4" type='audio/mp4; codecs="mp4a.40.2"' />
    Your browser does not support the audio element.
</audio>