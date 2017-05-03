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
<%@ Page Title="View Coverage By Schedule Report" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeFile="CoverageByScheduleReports.aspx.cs" Inherits="Pages_CoverageReport" MasterPageFile="~/Pages/MasterPage.master" %>

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
                    <asp:Label ID="lblTitle" runat="server" Text="Coverage Report By Schedule" /></li>
            </ol>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-12">
            <h2><asp:Label ID="lblReportName" Text="Coverage Report By Schedule" runat="server" /></h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <em><asp:Label runat="server" ID="lblReportDescription" Text="This report shows the measure of percentage difference between number of children immunized with third dose and first dose for each antigen against pre-determined monthly immunization target "  /></em>
        </div>
    </div>

    <div class="row">
            <div class="col-md-9">
                <asp:TextBox ID="hack" runat="server" Visible="false" />
                        <input type="hidden" name="j_username" value="<%=ConfigurationManager.AppSettings["JasperUser"]%>" />
                        <input type="hidden" name="j_password" value="<%=ConfigurationManager.AppSettings["JasperPassword"]%>" />
                <div class="container-fluid" runat="server" id="reportInputs">
                </div>
            </div>

            <div class="col-md-3 col-lg-3 clearfix">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-raised" OnClick="btnSearch_Click" />
            </div>
        </div>
        <div class="row">
                    <div class="col-md-9 col-xs-9 col-sm-9 col-lg-9 clearfix"></div>
                    <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                        <asp:Button ID="btnExcel" runat="server" Visible="false" Text="Excel" CssClass="btn btn-success btn-raised" OnClick="btnExcel_Click" />
                    </div>
                    <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
                </div>
            <br />

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix" style="overflow:auto">
            
        <asp:GridView ID="gvCoverageReport" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover table-responsive" AllowPaging="True" OnRowDataBound="gvCoverageReport_DataBound" OnDataBound="gvOn_DataBound" OnPageIndexChanging="gvCoverageReport_PageIndexChanging" PageSize="25">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />

            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Antigen" />

                <asp:TemplateField>

                <HeaderTemplate>
                    <HeaderStyle HorizontalAlign="Center">
                        <table width="100%" border="1px" >
                              <tr>
                                <td rowspan="4" width="20%" align="center"> Doses </td>
                               </tr>
                              <tr>
                                 <td colspan="8" align="center">Health Facility Coverage Report </td>
                              </tr>
                              <tr>
                                <td colspan="3" align="center" > Within Catchment</td>
                                <td colspan="3" align="center" > Outside Catchment</td>
                                <td colspan="2" align="center"> Total Vaccination</td>
                              </tr>

                              <tr>
                                 <td width="10%" align="center"> Male </td>
                                 <td width="10%" align="center"> Female </td>
                                 <td width="10%" align="center"> Total </td>
                                 <td width="10%" align="center"> Male </td>
                                 <td width="10%" align="center"> Female </td>
                                 <td width="10%" align="center"> Total  </td>
                                 <td width="10%" align="center"> Total  </td>
                                 <td width="10%" align="center"> COV(%) </td>
                              </tr>
                         </table>
                    </HeaderStyle>

                </HeaderTemplate>
                <ItemTemplate>
                    <asp:GridView ID="gvCoverageReportDetails" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-responsive" ShowHeader="False" AllowPaging="false" OnRowDataBound="gvCoverageReportDetails_DataBound" OnPageIndexChanging="gvCoverageReportDetails_PageIndexChanging" PageSize="25">
                        <PagerSettings Position="Top" Mode="NumericFirstLast" />
                        <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                        <Columns>
                             <asp:BoundField HeaderText="DoseName" DataField="DoseName" ItemStyle-Width="20%" />
                             <asp:BoundField HeaderText="MaleWithinCatchment" DataField="MaleWithinCatchment" ItemStyle-Width="10%" />
                             <asp:BoundField HeaderText="FemaleWithinCatchment" DataField="FemaleWithinCatchment" ItemStyle-Width="10%" />
                             <asp:BoundField HeaderText="TotalWithinCatchment" DataField="TotalWithinCatchment" ItemStyle-Width="10%" />
                             <asp:BoundField HeaderText="MaleOutsideCatchment" DataField="MaleOutsideCatchment" ItemStyle-Width="10%" />
                             <asp:BoundField HeaderText="FemaleOutsideCatchment" DataField="FemaleOutsideCatchment" ItemStyle-Width="10%" />
                             <asp:BoundField HeaderText="TotalOutsideCatchment" DataField="TotalOutsideCatchment" ItemStyle-Width="10%" />
                             <asp:BoundField HeaderText="TotalVaccinations" DataField="TotalVaccinations" ItemStyle-Width="10%" />
                             <asp:BoundField HeaderText="CoveragePercentage" DataField="CoveragePercentage" ItemStyle-Width="10%" />
                        </Columns>
                    </asp:GridView>
                </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <!--
            data binding of the gridview is done in connection to the Datalayer methods that queries the data EnablePaging="false"
        -->
        <asp:ObjectDataSource ID="odsAntigens" runat="server" SelectMethod="GetScheduledVaccinationList" TypeName="GIIS.DataLayer.ScheduledVaccination">
            <SelectParameters>
            </SelectParameters>
        </asp:ObjectDataSource>

        <asp:ObjectDataSource ID="odsCoverageDetails" runat="server" SelectMethod="GetDistrictCoverageReport" TypeName="GIIS.DataLayer.CoverageReportEntity">
            <SelectParameters>
                 <asp:Parameter Name="scheduledVaccinationName" Type="String" />
                 <asp:Parameter Name="healthFacilityId" Type="String"/>
                 <asp:Parameter Name="fromDate" Type="DateTime" />
                 <asp:Parameter Name="toDate" Type="DateTime" />
                 <asp:Parameter Name="isSchedule" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>




    </div>
</div>



     <div class="row">
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
                    <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnRowDataBound="gvRowOn_DataBound" OnDataBound="gvOn_DataBound">
                      <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Antigen" />

                                    <asp:TemplateField>

                                    <HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="Center">
                                            <table width="100%" border="1px" >
                                                  <tr>
                                                    <td rowspan="4" width="20%" align="center"> Doses </td>
                                                   </tr>
                                                  <tr>
                                                     <td colspan="8" align="center">Health Facility Coverage Report </td>
                                                  </tr>
                                                  <tr>
                                                    <td colspan="3" align="center" > Within Catchment</td>
                                                    <td colspan="3" align="center" > Outside Catchment</td>
                                                    <td colspan="2" align="center"> Total Vaccination</td>
                                                  </tr>

                                                  <tr>
                                                     <td width="10%" align="center"> Male </td>
                                                     <td width="10%" align="center"> Female </td>
                                                     <td width="10%" align="center"> Total </td>
                                                     <td width="10%" align="center"> Male </td>
                                                     <td width="10%" align="center"> Female </td>
                                                     <td width="10%" align="center"> Total  </td>
                                                     <td width="10%" align="center"> Total  </td>
                                                     <td width="10%" align="center"> COV(%) </td>
                                                  </tr>
                                             </table>
                                        </HeaderStyle>

                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:GridView ID="gvExportDetails" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-responsive" ShowHeader="False" AllowPaging="false" OnRowDataBound="gvCoverageReportDetails_DataBound" OnPageIndexChanging="gvCoverageReportDetails_PageIndexChanging" PageSize="25">
                                            <PagerSettings Position="Top" Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                                            <Columns>
                                                 <asp:BoundField HeaderText="DoseName" DataField="DoseName" ItemStyle-Width="20%" />
                                                 <asp:BoundField HeaderText="MaleWithinCatchment" DataField="MaleWithinCatchment" ItemStyle-Width="10%" />
                                                 <asp:BoundField HeaderText="FemaleWithinCatchment" DataField="FemaleWithinCatchment" ItemStyle-Width="10%" />
                                                 <asp:BoundField HeaderText="TotalWithinCatchment" DataField="TotalWithinCatchment" ItemStyle-Width="10%" />
                                                 <asp:BoundField HeaderText="MaleOutsideCatchment" DataField="MaleOutsideCatchment" ItemStyle-Width="10%" />
                                                 <asp:BoundField HeaderText="FemaleOutsideCatchment" DataField="FemaleOutsideCatchment" ItemStyle-Width="10%" />
                                                 <asp:BoundField HeaderText="TotalOutsideCatchment" DataField="TotalOutsideCatchment" ItemStyle-Width="10%" />
                                                 <asp:BoundField HeaderText="TotalVaccinations" DataField="TotalVaccinations" ItemStyle-Width="10%" />
                                                 <asp:BoundField HeaderText="CoveragePercentage" DataField="CoveragePercentage" ItemStyle-Width="10%" />
                                            </Columns>
                                        </asp:GridView>
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                    </asp:GridView>
                  <asp:ObjectDataSource ID="odsExport" runat="server" SelectMethod="GetScheduledVaccinationList" TypeName="GIIS.DataLayer.ScheduledVaccination">
                    <SelectParameters>
                    </SelectParameters>
                </asp:ObjectDataSource>

                <asp:ObjectDataSource ID="odsExportDetails" runat="server" SelectMethod="GetDistrictCoverageReport" TypeName="GIIS.DataLayer.CoverageReportEntity">
                    <SelectParameters>
                         <asp:Parameter Name="scheduledVaccinationName" Type="String" />
                         <asp:Parameter Name="healthFacilityId" Type="String"/>
                         <asp:Parameter Name="fromDate" Type="DateTime" />
                         <asp:Parameter Name="toDate" Type="DateTime" />
                         <asp:Parameter Name="isSchedule" Type="Boolean" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                </div>
        </div>

    <ajaxToolkit:CalendarExtender TargetControlID="hack" ID="ceMain" runat="server" />
   
</asp:Content>

