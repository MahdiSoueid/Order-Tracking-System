<%@ Page Title="Tracking System" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="TrackingSystem.Orders" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="OrdersHolder" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class=" container-md 3">
            <div class="container-fluid-custom mt-4">
    <div class="card">
        <div class="card-header">
            <h5 class="card-title mb-0">Search Criteria</h5>
        </div>
        <div class="card-body">
            <div class="row mb-3">
                <div class="col-lg-3 col-md-4 col-sm-6">
                    <asp:Label ID="SPid" runat="server" CssClass="fw-bold form-label" Text="SPid:"></asp:Label>
                    <asp:TextBox ID="SPidtxt" runat="server" CssClass="form-control form-control-sm" Placeholder="SPid"></asp:TextBox>
                </div>
                <div class="col-lg-3 col-md-4 col-sm-6">
                    <asp:Label ID="EPid" runat="server" CssClass="fw-bold form-label" Text="EPid:"></asp:Label>
                    <asp:TextBox ID="EPidtxt" runat="server" CssClass="form-control form-control-sm" Placeholder="EPid"></asp:TextBox>
                </div>
                 <div class="col-lg-3 col-md-4 col-sm-6">
     <asp:Label ID="StartDateTime" runat="server" CssClass="fw-bold form-label" Text="StartDateTime:"></asp:Label>
     <asp:TextBox ID="StartDateTimetxt" runat="server" CssClass="form-control form-control-sm" Placeholder="StartDateTime"></asp:TextBox>
 </div>
                                <div class="col-lg-3 col-md-4 col-sm-6">
    <asp:Label ID="EndDateTime" runat="server" CssClass="fw-bold form-label" Text="EndDateTime:"></asp:Label>
    <asp:TextBox ID="EndDateTimetxt" runat="server" CssClass="form-control form-control-sm" Placeholder="EndDateTime"></asp:TextBox>
</div>
                                                <div class="col-lg-3 col-md-4 col-sm-6">
    <asp:Label ID="Status" runat="server" CssClass="fw-bold form-label" Text="Status:"></asp:Label>
   <asp:DropDownList ID="StatusOptions" runat="server" CssClass="form-control form-control-sm">
    <asp:ListItem Text="" Value="1"></asp:ListItem>
    <asp:ListItem Text="InProgress" Value="2"></asp:ListItem>
    <asp:ListItem Text="Recieved" Value="3"></asp:ListItem>
</asp:DropDownList>
</div>


                <div class="col-lg-3 col-md-4 col-sm-6 align-self-end">
                    <div class="form-group text-right">
                        <asp:Button ID="Searchbttn" runat="server" Text="Search" CssClass="btn btn-primary btn-sm" OnClick="SearchBttn_Click" />
                        <asp:Button ID="Clearbttn" runat="server" Text="Clear" CssClass="btn btn-secondary btn-sm ml-2" OnClick="ClearBttn_Click" />
                    </div>
                </div>
                </div>
            </div>
         </div>
                 </div>
                </div>
                <div class=" container-md 3">
            <telerik:RadGrid AllowSorting="true" AllowPaging="true" RenderMode="Lightweight" ID="RadGrid" runat="server" AutoGenerateColumns="false" OnNeedDataSource="RadGrid_NeedDataSource" OnItemCommand="RadGrid_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridButtonColumn CommandName="ViewDetails" UniqueName="ViewDetails" ButtonType="ImageButton" ImageUrl="Content/icon-info.png" ItemStyle-CssClass="grid-button-logo" />
                        <telerik:GridBoundColumn DataField="IDKey" HeaderText="Order ID" UniqueName="IDKey" HeaderStyle-CssClass="fw-bold" />
                        <telerik:GridBoundColumn DataField="SPName" HeaderText="Start Point" UniqueName="SPid" HeaderStyle-CssClass="fw-bold" />
                        <telerik:GridBoundColumn DataField="EPName" HeaderText="End Point" UniqueName="EPid" HeaderStyle-CssClass="fw-bold" />
                        <telerik:GridBoundColumn DataField="StartDateTime" HeaderText="StartDateTime" UniqueName="StartDateTime" HeaderStyle-CssClass="fw-bold" />
                        <telerik:GridBoundColumn DataField="EndDateTime" HeaderText="EndDateTime" UniqueName="EndDateTime" HeaderStyle-CssClass="fw-bold" />
                        <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="Status" HeaderStyle-CssClass="fw-bold" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
                    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
