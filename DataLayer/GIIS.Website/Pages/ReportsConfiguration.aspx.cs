//*******************************************************************************
//Copyright 2015 TIIS - Tanzania Immunization Information System
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
 //******************************************************************************
using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Configuration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            List<string> actionList = null;
            string sessionNameAction = "";
            if (CurrentEnvironment.LoggedUser != null)
            {
                sessionNameAction = "__GIS_actionList_" + CurrentEnvironment.LoggedUser.Id;
                actionList = (List<string>)Session[sessionNameAction];
            }

            if ((actionList != null) && actionList.Contains("ViewConfiguration") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Configuration-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Configuration");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Configuration-dictionary" + language, wtList);
                }

                //message
                this.lblSuccess.Text = wtList["ConfigurationSuccessText"];
                this.lblWarning.Text = wtList["ConfigurationWarningText"];
                this.lblError.Text = wtList["ConfigurationErrorText"];

                ReportsConfiguration co = ReportsConfiguration.GetConfigurationByName("ChildrenRegistrationsMaximumThreshold");
                if (co != null){
                    txtMaxRegistationsThreshold.Text = co.Value;
                }else{
                    txtMaxRegistationsThreshold.Text = "";
                }

                co = null;
                co  = ReportsConfiguration.GetConfigurationByName("ChildrenRegistrationsMinimumThreshold");
                if (co != null){
                    txtMinRegistationsThreshold.Text = co.Value;
                }else{
                    txtMinRegistationsThreshold.Text = "";
                }
                
                co = null;
                co  = ReportsConfiguration.GetConfigurationByName("ChildrenVaccinationsMaximumThreshold");
                if (co != null){
                    txtMaxVaccinationsThreshold.Text = co.Value;
                }else{
                    txtMaxVaccinationsThreshold.Text = "";
                }
                
                co = null;
                co  = ReportsConfiguration.GetConfigurationByName("ChildrenVaccinationsMinimumThreshold");
                if (co != null){
                    txtMinVaccinationsThreshold.Text = co.Value;
                }else{
                    txtMinVaccinationsThreshold.Text = "";
                }
                

                co = null;
                co  = ReportsConfiguration.GetConfigurationByName("DaysMaximum");
                if (co != null){
                    txtMaxThresholdDays.Text = co.Value;
                }else{
                    txtMaxThresholdDays.Text = "";
                }

                co = null;
                co  = ReportsConfiguration.GetConfigurationByName("DaysMinimum");
                if (co != null){
                    txtMinThresholdDays.Text = co.Value;
                }else{
                    txtMinThresholdDays.Text = "";
                }

            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }


    protected void btnSaveChildrenRegistrationsThreshold_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {

                int i = 0;
                string maxRegistationsThreshold = txtMaxRegistationsThreshold.Text;
                string minRegistationsThreshold = txtMinRegistationsThreshold.Text;
            
                ReportsConfiguration co = new ReportsConfiguration();
                
                ReportsConfiguration flag = ReportsConfiguration.GetConfigurationByName("ChildrenRegistrationsMaximumThreshold");
                if (flag != null){
                    flag.Name = "ChildrenRegistrationsMaximumThreshold";
                    flag.Value = maxRegistationsThreshold;
                    i = ReportsConfiguration.Update(flag);
                }else{
                    co.Name = "ChildrenRegistrationsMaximumThreshold";
                    co.Value = maxRegistationsThreshold;
                    i = ReportsConfiguration.Insert(co);
                } 
                
                co = new ReportsConfiguration();
                flag = ReportsConfiguration.GetConfigurationByName("ChildrenRegistrationsMinimumThreshold");
                if (flag != null){
                    flag.Name = "ChildrenRegistrationsMinimumThreshold";
                    flag.Value = minRegistationsThreshold;
                    i = ReportsConfiguration.Update(flag);
                }else{
                    co.Name = "ChildrenRegistrationsMinimumThreshold";
                    co.Value = minRegistationsThreshold;
                    i = ReportsConfiguration.Insert(co);
                }

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false;
                    lblError.Visible = true;
                }
            
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }

    protected void btnSaveChildrenVaccinationsThreshold_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {

                int i = 0;
                string maxChildVaccinations = txtMaxVaccinationsThreshold.Text;
                string minChildVaccinations = txtMinVaccinationsThreshold.Text;

                ReportsConfiguration co = new ReportsConfiguration();

                ReportsConfiguration flag = ReportsConfiguration.GetConfigurationByName("ChildrenVaccinationsMaximumThreshold");
                if (flag != null){
                    flag.Name = "ChildrenVaccinationsMaximumThreshold";
                    flag.Value = maxChildVaccinations;
                    i = ReportsConfiguration.Update(flag);
                }else{
                    co.Name = "ChildrenVaccinationsMaximumThreshold";
                    co.Value = maxChildVaccinations;
                    i = ReportsConfiguration.Insert(co);
                }

                co = new ReportsConfiguration();
                flag = ReportsConfiguration.GetConfigurationByName("ChildrenVaccinationsMinimumThreshold");
                if (flag != null){
                    flag.Name = "ChildrenVaccinationsMinimumThreshold";
                    flag.Value = minChildVaccinations;
                    i = ReportsConfiguration.Update(flag);
                }else{
                    co.Name = "ChildrenVaccinationsMinimumThreshold";
                    co.Value = minChildVaccinations;
                    i = ReportsConfiguration.Insert(co);
                }

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false;
                    lblError.Visible = true;
                }

            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }

    protected void btnSaveDaysThreshold_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {

                int i = 0;
                string maxDays = txtMaxThresholdDays.Text;
                string minDays = txtMinThresholdDays.Text; 
                
                ReportsConfiguration co = new ReportsConfiguration();
                ReportsConfiguration flag = ReportsConfiguration.GetConfigurationByName("DaysMaximum");

                if (flag != null){
                    flag.Name = "DaysMaximum";
                    flag.Value = maxDays;
                    i = ReportsConfiguration.Update(flag);
                }else{
                    co.Name = "DaysMaximum";
                    co.Value = maxDays;
                    i = ReportsConfiguration.Insert(co);
                }
                
                co = new ReportsConfiguration();
                flag = ReportsConfiguration.GetConfigurationByName("DaysMinimum");
                if (flag != null){
                    flag.Name = "DaysMinimum";
                    flag.Value = minDays;
                    i = ReportsConfiguration.Update(flag);
                }else{
                    co.Name = "DaysMinimum";
                    co.Value = minDays;
                    i = ReportsConfiguration.Insert(co);
                } 

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false;
                    lblError.Visible = true;
                }
            
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }
 
}