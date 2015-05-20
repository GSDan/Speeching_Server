<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Activities.aspx.cs" Inherits="Crowd.Service.Activities" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False">
            <Columns>
                
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkActionId" runat="server" Text='<%# Eval("Key") %>' NavigateUrl='<%# "tasks.aspx?id=" + Eval("Id") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ExternalId" HeaderText="External Id"></asp:BoundField>
                <asp:BoundField DataField="Title" HeaderText="Title"></asp:BoundField>
                <asp:BoundField DataField="Icon" HeaderText="Icon"></asp:BoundField>
                <asp:BoundField DataField="PrincipleInvestigatorKey" HeaderText="PrincipleInvestigatorKey"></asp:BoundField>
                <asp:BoundField DataField="CrowdCategoryKey" HeaderText="CrowdCategoryKey"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
