<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateFeedItem.aspx.cs" Inherits="Crowd.Service.CreateFeedItem" Async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    

</head>
<body>
    <form id="createFeedItemForm" runat="server">
    <div>
        
        <div id="createNewFeedItem">
            <p>Story Title:</p>
            <asp:TextBox runat="server" ID="newTitle"></asp:TextBox>
            <asp:RequiredFieldValidator ID="titleValid" runat="server" ControlToValidate="newTitle" ErrorMessage="Please enter a title" Display="Dynamic" /> 
            <p>Story Description:</p>
            <asp:TextBox runat="server" ID="newDesc"></asp:TextBox>
            <asp:RequiredFieldValidator ID="descValid" runat="server" ControlToValidate="newDesc" ErrorMessage="Please enter a description" Display="Dynamic" /> 

            <br/><br/>
            <asp:DropDownList runat="server" ID="newType" AutoPostBack="True" OnSelectedIndexChanged="newType_SelectedIndexChanged">
                <asp:ListItem Text="Simple" Value="0"></asp:ListItem>
                <asp:ListItem Text="Image" Value="1"></asp:ListItem>
                <asp:ListItem Text="Link" Value="2"></asp:ListItem>
            </asp:DropDownList>
            
            <div runat="server" id="imageDiv" Visible="False">
                <br/><br/>
                <asp:Label runat="server" Text="Image loc:"></asp:Label><br/>
                <asp:TextBox runat="server" ID="newImage"></asp:TextBox>
                <asp:RequiredFieldValidator ID="imageValid" runat="server" ControlToValidate="newImage" ErrorMessage="Please enter a link to an image or change story type" Display="Dynamic" /> 
            </div>
            
            <div runat="server" id="linkDiv" Visible="False">
                <br/><br/>
                <asp:Label runat="server" Text="Http link:"></asp:Label><br/>
                <asp:TextBox runat="server" ID="newLink"></asp:TextBox>
                <asp:RequiredFieldValidator ID="linkValid" runat="server" ControlToValidate="newLink" ErrorMessage="Please enter a link to a page or change story type" Display="Dynamic" /> 
            </div>
            
            <br/><br/>
            <asp:Button runat="server" Text="Submit" ID="submitBtn" OnClick="submitBtn_Click"/>
        </div>

    </div>
    </form>
</body>
</html>
