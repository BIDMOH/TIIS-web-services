<%-- 
*******************************************************************************
  Copyright 2015 TIIS - Tanzania Immunization Information System

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 ******************************************************************************
--%>
<%@ Page Title="View PMTCT status Report" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeFile="HealthFacilityListPMTCTstatusReport.aspx.cs" Inherits="Pages_HealthFacilityListPMTCTstatus" MasterPageFile="~/Pages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ajax__calendar_container { z-index : 1000 ; }
        
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li class="active">
                    <a href="Report.aspx">Reports</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="PMTCT status Report" /></li>
            </ol>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-12">
            <h2><asp:Label ID="lblReportName" Text="Health Facilities PMTCT status " runat="server" /></h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <em><asp:Label runat="server" ID="lblReportDescription" Text="This report shows the PMTCT status for female and male in a given facility. "  /></em>
        </div>
    </div>

    <div class="row">
        <div class="col-md-9">
            <asp:TextBox ID="hack" runat="server" Visible="false" />
                    <input type="hidden" name="j_username" value="<%=ConfigurationManager.AppSettings["JasperUser"]%>" />
                    <input type="hidden" name="j_password" value="<%=ConfigurationManager.AppSettings["JasperPassword"]%>" />
            <div class="container-fluid" runat="server" id="reportInputs"></div>
        </div>

        <div class="col-md-3 col-lg-3 clearfix">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-raised" OnClick="btnSearch_Click" />
        </div>
    </div>


    <div class="row">
         <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix" style="overflow:auto">
            <asp:GridView ID="gvHealthFacilityListPMTCTstatus" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-responsive" AllowPaging="True" OnDataBound="gvHealthFacilityListPMTCTstatus_DataBound" OnPageIndexChanging="gvHealthFacilityListPMTCTstatus_PageIndexChanging" PageSize="25">
                    <PagerSettings Position="Top" Mode="NumericFirstLast" />
                    <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:TemplateField HeaderText="PMTCT Status">
                       <ItemTemplate>
                         <%#Eval("status")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Female">
                       <ItemTemplate>
                         <%#Eval("female")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Male">
                    <ItemTemplate>
                         <%#Eval("male")%>
                    </ItemTemplate>
                    </asp:TemplateField>
                 </Columns>
            </asp:GridView>
            <!--
                data binding of the gridview is done in connection to the Datalayer methods that queries the data EnablePaging="false"
            -->
            <asp:ObjectDataSource ID="odsHealthFacilityListPMTCTstatus" runat="server" SelectMethod="GetAllChildrenPMTCTstatus" TypeName="GIIS.DataLayer.HealthFacilityPMTCTstatus">
                <SelectParameters>
                     <asp:Parameter Name="hfid" Type="String"/>
                     <asp:Parameter Name="fromDate" Type="DateTime"/>
                     <asp:Parameter Name="toDate" Type="DateTime"/>
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive"  OnDataBound="gvHealthFacilityListPMTCTstatus_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="PMTCT Status">
                       <ItemTemplate>
                         <%#Eval("status")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Female">
                       <ItemTemplate>
                         <%#Eval("female")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Male">
                    <ItemTemplate>
                         <%#Eval("male")%>
                    </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:ObjectDataSource ID="odsExport" runat="server" SelectMethod="GetAllChildrenPMTCTstatus" TypeName="GIIS.DataLayer.HealthFacilityPMTCTstatus">
                <SelectParameters>
                     <asp:Parameter Name="hfid" Type="String"/>
                     <asp:Parameter Name="fromDate" Type="DateTime"/>
                     <asp:Parameter Name="toDate" Type="DateTime"/>
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
    <div class="row">
        <div class="col-md-8 col-xs-6 col-sm-6 col-lg-6 clearfix"></div>
        <div class="col-md-2">
            <asp:Button ID="btnPdf" runat="server" Visible="false" Text="PDF" CssClass="btn btn-success btn-raised" OnClick="btnPdf_Click" />
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnExcel" runat="server" Visible="false" Text="Excel" CssClass="btn btn-success btn-raised" OnClick="btnExcel_Click" />
        </div>
    </div>
    <ajaxToolkit:CalendarExtender TargetControlID="hack" ID="ceMain" runat="server" />
   
</asp:Content>

