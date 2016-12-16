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
<%@ Page Title="View Session Report" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeFile="SessionReport.aspx.cs" Inherits="Pages_SessionReport" MasterPageFile="~/Pages/MasterPage.master" %>

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
            <h2><asp:Label ID="lblReportName" Text="Session Durations Report" runat="server" /></h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <em><asp:Label runat="server" ID="lblReportDescription" Text="This will be the description of the text Session Report"  /></em>
        </div>
    </div>
    <br />
    <form class="form" method="get" action="" id="launchReport" >
    <div class="row">
        <div class="col-md-12">        
            <asp:TextBox ID="hack" runat="server" Visible="false" />
                    <input type="hidden" name="j_username" value="<%=ConfigurationManager.AppSettings["JasperUser"]%>" />
                    <input type="hidden" name="j_password" value="<%=ConfigurationManager.AppSettings["JasperPassword"]%>" />
            <div class="container-fluid" runat="server" id="reportInputs">
            </div>
        </div>
    </div>
    <br />

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix" style="overflow:auto">
            
       <asp:GridView ID="gvHealthFacilitySessions" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" OnDataBound="gvHealthFacilitySessions_DataBound" OnPageIndexChanging="gvHealthFacilitySessions_PageIndexChanging" >
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:TemplateField HeaderText="Time">
                    <ItemTemplate>
                        <%# Eval("LoginTime") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Duration">
                   <ItemTemplate>
                        <%#Eval("SessionLength")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="User Name">
                    <ItemTemplate>
                        <%#Eval("")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" Visible ="false" />
            </Columns>
        </asp:GridView>
        <!--
            data binding of the gridview is done in connection to the Datalayer methods that queries the data
        -->
        <asp:ObjectDataSource ID="odsHealthFacilitySessions" runat="server" EnablePaging="true" SelectMethod="GetHealthFacilitySessionsByHealthFacilityId" TypeName="GIIS.DataLayer.HealthFacilitiesSessions">
            <SelectParameters>
                 <asp:Parameter Name="hfid" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>


    </div>
</div>

    <!--<div class="row">
        <div class="col-md-12">
            <input type="submit" class="btn btn-primary" value="Download <%=Request.QueryString["format"] %>" />
        </div>
    </div>-->

    </form>

    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix" style="overflow:auto">
        </div>
    </div>
    <br />

    <ajaxToolkit:CalendarExtender TargetControlID="hack" ID="ceMain" runat="server" />
   
</asp:Content>

