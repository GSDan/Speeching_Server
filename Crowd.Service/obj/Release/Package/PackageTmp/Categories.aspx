<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="Crowd.Service.Categories" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:LinkButton runat="server" ID="lbtnCreate" Text="Create" OnClick="lbtnCreate_OnClick"></asp:LinkButton>
            <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False"
                DataKeyNames="Key" OnRowCancelingEdit="gvCategoreis_RowCancelingEdit"
                OnRowEditing="gvCategories_RowEditing" OnRowUpdating="gvCategories_RowUpdating">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkActionId" runat="server" Text='<%# Eval("Key") %>' NavigateUrl='<%# "activities.aspx?id=" + Eval("Key") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ExternalKey" HeaderText="External Key"></asp:BoundField>
                    <asp:BoundField DataField="Title" HeaderText="Title"></asp:BoundField>
                    <asp:BoundField DataField="Icon" HeaderText="Icon"></asp:BoundField>
                    <asp:TemplateField HeaderText="Recommended">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("Recommended")) %>'/>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("Recommended")) %>'/>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="True" ShowHeader="True" HeaderText="Edit"></asp:CommandField>

                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
