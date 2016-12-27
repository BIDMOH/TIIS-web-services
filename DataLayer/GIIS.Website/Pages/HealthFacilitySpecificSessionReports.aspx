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
<%@ Page Title="Health Facility Session Reports" Debug="true" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="HealthFacilitySpecificSessionReports.aspx.cs" Inherits="Pages_HealthFacilitySpecificSessionReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
      <style type="text/css">
        .ajax__calendar_container { z-index : 1000 ; }
        
    </style>
    <script type="text/javascript">
        $('#myTab a').click(function(e) {
            e.preventDefault();
            $(this).tab('show');
        });

        // store the currently selected tab in the hash value
        $("ul.nav-pills > li > a").on("shown.bs.tab", function(e) {
        var id = $(e.target).attr("href").substr(1);
        window.location.hash = id;
        });

        // on load of the page: switch to the currently selected tab
        var hash = window.location.hash;
        $('#myTab a[href="' + hash + '"]').tab('show');
    </script>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

        <div class="row">
            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
                <ol class="breadcrumb">
                    <li><a href="Default.aspx">Home</a></li>
                    <li><a href="#">Reports</a></li>
                
                </ol>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h2><asp:Label ID="lblFacilityName" Text="" runat="server" /></h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
                
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
                    <ul class="nav nav-pills" id="myTab">
                        <li class="active"><a data-toggle="pill" href="#home">Session Durations Report</a></li>
                        <li><a data-toggle="pill" href="#menu1">Total Sessions Report (Duration)</a></li>
                        <li><a data-toggle="pill" href="#menu2">Total Sessions Report (Days)</a></li>
                    </ul>
                <div class="tab-content">
                    <div id="home" class="tab-pane fade in active">
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <em><asp:Label runat="server" ID="lblReportDescription" Text="This report shows in details all the sessions conducted by in specific health facility by different users, within a specific date range"  /></em>
                            </div>
                        </div>
                        <br>
                        <div class="row">
                            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix" style="overflow:auto">
                                
                        <asp:GridView ID="gvHealthFacilitySessions" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" OnDataBound="gvHealthFacilitySessions_DataBound" OnPageIndexChanging="gvHealthFacilitySessions_PageIndexChanging" PageSize="25" >
                                    <PagerSettings Position="Top" Mode="NumericFirstLast" />
                                    <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Time">
                                        <ItemTemplate>
                                            <%# Eval("LoginTime") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Duration">
                                    <ItemTemplate>
                                        <%# convertToHoursAndMinutes(Convert.ToInt32(Eval("SessionLength"))) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Name">
                                    <ItemTemplate>
                                        <%#Eval("UserName")%>
                                    </ItemTemplate>
                                    </asp:TemplateField>            
                                </Columns>
                            </asp:GridView>
                            <!--
                                data binding of the gridview is done in connection to the Datalayer methods that queries the data EnablePaging="false"
                            -->
                            <asp:ObjectDataSource ID="odsHealthFacilitySessions" runat="server" SelectMethod="GetHealthFacilitySessionsByHealthFacilityId" TypeName="GIIS.DataLayer.HealthFacilitySessions">
                                <SelectParameters>
                                    <asp:Parameter Name="hfid" Type="String" />
                                    <asp:Parameter Name="fromDate" Type="DateTime" />
                                    <asp:Parameter Name="toDate" Type="DateTime" />
                                </SelectParameters>
                            </asp:ObjectDataSource>

                            <asp:ObjectDataSource ID="odsHealthFacilitySessionsByUsers" runat="server" SelectMethod="GetHealthFacilitySessionsByHealthFacilityIdAndUserId" TypeName="GIIS.DataLayer.HealthFacilitySessions">
                                <SelectParameters>
                                    <asp:Parameter Name="hfid" Type="String" />
                                    <asp:Parameter Name="userID" Type="String" />
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
                    </div>

                    <div id="menu1" class="tab-pane fade">
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <em><asp:Label runat="server" ID="lblDurationReportDescription" Text="This report shows the total time spent by users in the mobile application in hours, within a specific date range"  /></em>
                            </div>
                        </div>
                        <br />
                        <form class="form" method="get" action="" id="launchDurationReport" >
                        <div class="row">
                            <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                                <em><asp:Label runat="server" ID="lblTotalDurations" Text="Total Duration"  /></em>
                            </div>
                            <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
                                <em><asp:Label runat="server" ID="lblTotalDurationsValue" Text=""  /></em>
                            </div>
                        </div>
                        </form>
                    </div>
                    <div id="menu2" class="tab-pane fade">
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <em><asp:Label runat="server" ID="lblDaysReportDescription" Text="Report to display the total number of days for a given date range that the application was logged in"  /></em>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                                <em><asp:Label runat="server" ID="lblTotalDays" Text="Total Session Days : "  /></em>
                            </div>
                            <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
                                <em><asp:Label runat="server" ID="lblTotalDaysValue" Text=""  /></em>
                            </div>
                        </div>
                    </div>
                </div>
               <br>
               <br>
            </div>
        </div>

        <ajaxToolkit:CalendarExtender TargetControlID="hack" ID="ceMain" runat="server" />

</asp:Content>

