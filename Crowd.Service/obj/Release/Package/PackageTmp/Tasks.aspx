<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tasks.aspx.cs" Inherits="Crowd.Service.Tasks" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False">
            <Columns>
                
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkActionId" runat="server" Text='<%# Eval("Key") %>' NavigateUrl='<%# "tasksContentResponse.aspx?id=" + Eval("Key") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ExternalKey" HeaderText="External Key"></asp:BoundField>
                <asp:BoundField DataField="Description" HeaderText="Description"></asp:BoundField>
                <asp:BoundField DataField="Title" HeaderText="Title"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
