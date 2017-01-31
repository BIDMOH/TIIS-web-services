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
<%@ Page Title="View Session Report" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeFile="HealthFacilityChildVaccinationsRatings.aspx.cs" Inherits="Pages_HealthFacilitySessionRatings" MasterPageFile="~/Pages/MasterPage.master" %>

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
                    <asp:Label ID="lblTitle" runat="server" Text="Session Report" /></li>
            </ol>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-12">
            <h2><asp:Label ID="lblReportName" Text="Health Facilities Child Vaccinations By District" runat="server" /></h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <em><asp:Label runat="server" ID="lblReportDescription" Text="This report shows the ranking of health facilities within the District council by their total number of children vaccinations"  /></em>
        </div>
    </div>
    <br />
    <form class="form" method="get" action="" id="launchReport" >
    <div class="row">
        <div class="col-md-9">        
            <asp:TextBox ID="hack" runat="server" Visible="false" />
                    <input type="hidden" name="j_username" value="<%=ConfigurationManager.AppSettings["JasperUser"]%>" />
                    <input type="hidden" name="j_password" value="<%=ConfigurationManager.AppSettings["JasperPassword"]%>" />
            <div class="container-fluid" runat="server" id="reportInputs">
            </div>
        </div>

        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-raised" OnClick="btnSearch_Click" />
        </div>
    </div>
    <br />

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix" style="overflow:auto">
                <asp:GridView ID="gvHealthFacilitySessions" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" OnRowDataBound="gvHealthFacilitySessions_DataBound" OnPageIndexChanging="gvHealthFacilitySessions_PageIndexChanging" PageSize="25">
                    <PagerSettings Position="Top" Mode="NumericFirstLast" />
                    <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                    <Columns>
                            <asp:TemplateField HeaderText="Name">
                               <ItemTemplate>
                                    <%#Eval("Name")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Children Registrations">
                            <ItemTemplate>
                                <%# Convert.ToInt32(Eval("SessionsCount")) %>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="View Facility Session Reports">
                                <ItemTemplate>
                                    <a href='<%# Eval("HealthFacilityId", "HealthFacilitySpecificSessionReports.aspx?hfId={0}") %>' target="_blank">
                                        <img alt='View Session Reports' src="../img/arrow_right_blue.png" />
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                </asp:GridView>

                <asp:ObjectDataSource ID="odsHealthFacilitySessionsVaccinations" runat="server" SelectMethod="GetHealthFacilityChildrenVaccinationsByDistrict" TypeName="GIIS.DataLayer.HealthFacilitySessions">
                    <SelectParameters>
                        <asp:Parameter Name="districtCouncilId" Type="String"/>
                        <asp:Parameter Name="fromDate" Type="DateTime" />
                        <asp:Parameter Name="toDate" Type="DateTime" />
                    </SelectParameters>
                </asp:ObjectDataSource>
        </div>
    </div>
    </form>
    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix" style="overflow:auto">
        </div>
    </div>
    <br />

    <ajaxToolkit:CalendarExtender TargetControlID="hack" ID="ceMain" runat="server" />
   
</asp:Content>

