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
<%@ Page Title="Configuration" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ReportsConfiguration.aspx.cs" Inherits="_Configuration" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Configuration</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Configuration" /></li>
            </ol>
        </div>
    </div>

    <div class="row">
        <div class="col-md-5">
            <h3><asp:Label ID="lblHoursThresholds" Text="Hours Thresholds for Health Facility Sessions" runat="server" /></h3>
        </div>

        <div class="col-md-5">
            <h3><asp:Label ID="lblDaysThresholds" Text="Days Thresholds for Health Facility Sessions" runat="server" /></h3>
        </div>
    </div>
    <br />
    <br />
    <div class="row">
        <div class="col-md-5">
            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                <asp:Label ID="lblMaxThreshold" runat="server" Text="Maximum"></asp:Label>
            </div>

            <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                <div class="form-group">
                    <asp:TextBox ID="txtMAxThreshold" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div> 

            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                <asp:Label ID="lblMinThreshold" runat="server" Text="Minimum"></asp:Label>
            </div>

            <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                <div class="form-group">
                    <asp:TextBox ID="txtMinThreshold" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <br />
            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
                <asp:Button ID="btnSaveHoursThreshold" runat="server" Text="Save" CssClass="btn btn-raised btn-primary" OnClick="btnSaveHoursThreshold_Click" ValidationGroup="saveConfiguration" />
            </div>
        </div>

        <div class="col-md-5">
            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                <asp:Label ID="lblMaxThresholdDays" runat="server" Text="Maximum"></asp:Label>
            </div>

            <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                <div class="form-group">
                    <asp:TextBox ID="txtMaxThresholdDays" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div> 

            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                <asp:Label ID="lblMinThresholdDays" runat="server" Text="Minimum"></asp:Label>
            </div>

            <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                <div class="form-group">
                    <asp:TextBox ID="txtMinThresholdDays" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <br />
            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
                <asp:Button ID="btnSaveDaysThreshold" runat="server" Text="Save" CssClass="btn btn-raised btn-primary" OnClick="btnSaveDaysThreshold_Click" ValidationGroup="saveConfiguration" />
            </div>
        </div>
    </div> 

    <br />

    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix"></div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
            <asp:Label ID="lblSuccess" runat="server" Text="Success" CssClass="label label-success" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" Text="Warning" CssClass="label label-warning" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblError" runat="server" Text="Error" CssClass="label label-danger" Font-Size="Small" Visible="false" />
        </div>
    </div>
    <br />
</asp:Content>
