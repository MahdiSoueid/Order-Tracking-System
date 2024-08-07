<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Tracking.aspx.cs" Inherits="TrackingSystem.Tracking" %>
<asp:Content ID="Content2" ContentPlaceHolderID="TrackingHolder" runat="server">
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
      <ContentTemplate>
        <div class=" container-md">
      <telerik:RadGrid AllowSorting="true" AllowPaging="true" RenderMode="Lightweight" ID="RadGrid" runat="server" AutoGenerateColumns="false" OnNeedDataSource="RadGrid_NeedDataSource">
    <MasterTableView>
        <Columns>
            <telerik:GridBoundColumn DataField="SPName" HeaderText="Start Point" UniqueName="SPid" HeaderStyle-CssClass="fw-bold" />
            <telerik:GridBoundColumn DataField="EPName" HeaderText="End Point" UniqueName="EPid" HeaderStyle-CssClass="fw-bold" />
            <telerik:GridBoundColumn DataField="StartDateTime" HeaderText="StartDateTime" UniqueName="StartDateTime" HeaderStyle-CssClass="fw-bold" />
            <telerik:GridBoundColumn DataField="EndDateTime" HeaderText="EndDateTime" UniqueName="EndDateTime" HeaderStyle-CssClass="fw-bold" />
            <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="Status" HeaderStyle-CssClass="fw-bold" />
        </Columns>
    </MasterTableView>
</telerik:RadGrid>
            </div>
          
           
            <div id="stepperContainer" runat="server" class="stepper"></div>
            <div id="detailsContainer" runat="server" class="Details"></div>
        
     </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
