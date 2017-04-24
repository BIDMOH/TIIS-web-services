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
<%@ Page Title="Coverage By Antigen Report" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="HealthFacilityCoverageByAntigen.aspx.cs" Inherits="Pages_HealthFacilityCoverageByFacilityAndAntigen" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Src="~/UserControls/OrderAlert.ascx" TagName="OrderAlert" TagPrefix="giis" %>
<%@ Register Src="~/UserControls/ItemLotsToExpire.ascx" TagName="ItemLotsToExpire" TagPrefix="giis" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
                <ol class="breadcrumb">
                    <li><a href="Default.aspx">Home</a></li>
                    <li class="active">
                        <a href="Report.aspx">Reports</a></li>
                    <li class="active">
                        <asp:Label ID="lblTitle" runat="server" Text="Defaulters Report" /></li>
                </ol>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h2><asp:Label ID="lblReportName" Text="Health Facilities Coverage Graph By Antigen " runat="server" /></h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <em><asp:Label runat="server" ID="lblReportDescription" Text="This report shows the graph of antigen coverage over time for a specific health facility. "  /></em>
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

            <div class="col-md-3 col-lg-3 clearfix">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-raised" OnClick="btnSearch_Click" />
            </div>
        </div>



    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:Chart ID="Chart1" runat="server" Width="850px" Palette="Pastel"                                                                                         >
                <Series>
                    <asp:Series Name="Series1"></asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BorderColor="#339966">
                         <AxisX LineColor="Gray">
                        <MajorGrid LineColor="LightGray" />
                    </AxisX>
                    <AxisY LineColor="Gray">
                        <MajorGrid LineColor="LightGray" />
                    </AxisY>
                    </asp:ChartArea>

                </ChartAreas>
            </asp:Chart>

        </div>
    </div>
    <br />
    <br />

</asp:Content>

