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
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Pages_HealthFacilityChildrenRegistrationsDefaultersReportByDistrict : System.Web.UI.Page
{
    public static String datefromString = "";
    public static String datetoString = "";
    public String userID = "";
    public String selectedHealthFacilityID = "";
    public String selectedDose = "";
    string ToDate="";
    public int hfParentID = 0;
    public int userSelectedIndex = 0;
    public HtmlGenericControl inputControl3,inputControl4,inputControl6;
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

            if ((actionList != null) && (CurrentEnvironment.LoggedUser != null))
            {

                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);

                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["HealthFacility-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "HealthFacility");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("HealthFacility-dictionary" + language, wtList);
                }

                createInputControls();


            }
            else
            {
                
                Response.Redirect("Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }


        }
    }

    public string convertToHoursAndMinutes(int duration){
        TimeSpan t = TimeSpan.FromSeconds(duration);
        String result =  t.Hours+" Hours  "+t.Minutes+" Minutes  "+t.Seconds+" Seconds";
        return result;
    }

    protected void createInputControls(){

            inputControl3 = new HtmlGenericControl("select");
            inputControl3.Attributes.Add("class", "form-control");

            var contextParms = new List<NpgsqlParameter>() {
                new NpgsqlParameter("@FacilityId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.HealthFacilityId },
                new NpgsqlParameter("@FacilityCode", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.HealthFacility.Code },
                new NpgsqlParameter("@UserId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.Id }
            };

            // var params = new List<NpgsqlParameter>() {
            //     new NpgsqlParameter("@FacilityId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.HealthFacilityId }
            // };

            hfParentID = HealthFacility.GetHealthFacilityById(CurrentEnvironment.LoggedUser.HealthFacilityId).ParentId;


            string command="";

            int userId = CurrentEnvironment.LoggedUser.Id;
            UserRole role = UserRole.GetUserRoleByUserId(userId);
            if(role.Role.Name.Equals("Middle Level Officer"))
            {
                command = "SELECT \"ID\", \"NAME\" FROM \"HEALTH_FACILITY\" WHERE  \"TYPE_ID\" = 2 AND \"PARENT_ID\" = "+CurrentEnvironment.LoggedUser.HealthFacilityId + " ORDER BY \"NAME\" ";
            }else{
                command = "SELECT \"ID\", \"NAME\" FROM \"HEALTH_FACILITY\"  WHERE \"TYPE_ID\" = 2  ORDER BY \"NAME\" ";
            }

            using (var idt = DBManager.ExecuteReaderCommand(command, System.Data.CommandType.Text, contextParms))
            {
                using (var irdr = idt.CreateDataReader())
                {

                    while (irdr.Read())
                    {

                        var opt = new HtmlGenericControl("option");
                        inputControl3.Controls.Add(opt);
                        opt.Attributes.Add("value", irdr[0].ToString());
                        opt.InnerText = irdr[1].ToString();

                        if(selectedHealthFacilityID.Equals(irdr[0].ToString())){
                            opt.Attributes.Add("selected","true");
                        }
                    }
                }
            }

            inputControl3.Attributes.Add("id", "selectHealthFacility");
            inputControl3.Attributes.Add("type", "text");
            inputControl3.Attributes.Add("style", "z-index:8");
            inputControl3.Attributes.Add("name", "selectHealthFacility");
            inputControl3.Attributes.Add("Visible", "false");
            inputControl3.Attributes.Add("title", "Health Facility option description");


            inputControl4 = new HtmlGenericControl("select");
            inputControl4.Attributes.Add("class", "form-control");
            string query = "SELECT \"ID\",\"NAME\" FROM \"SCHEDULED_VACCINATION\"  WHERE \"IS_ACTIVE\" = true  ORDER BY \"NAME\" ";

            using (var idt2 = DBManager.ExecuteReaderCommand(query, System.Data.CommandType.Text, null))
            {
                var opt1 = new HtmlGenericControl("option");
                inputControl4.Controls.Add(opt1);
                opt1.Attributes.Add("value", "all");
                opt1.InnerText = "all";

                if(selectedDose.Equals("all")){
                    opt1.Attributes.Add("selected","true");
                }
                using (var irdr1 = idt2.CreateDataReader())
                {

                    while (irdr1.Read())
                    {

                        var opt = new HtmlGenericControl("option");
                        inputControl4.Controls.Add(opt);
                        opt.Attributes.Add("value", irdr1[0].ToString());
                        opt.InnerText = irdr1[1].ToString();

                        if(selectedDose.Equals(irdr1[0].ToString())){
                            opt.Attributes.Add("selected","true");
                        }
                    }
                }
            }

            inputControl4.Attributes.Add("id", "selectDose");
            inputControl4.Attributes.Add("type", "text");
            inputControl4.Attributes.Add("style", "z-index:8");
            inputControl4.Attributes.Add("name", "selectDose");
            inputControl4.Attributes.Add("Visible", "false");
            inputControl4.Attributes.Add("title", "Dose");



            inputControl6 = new HtmlGenericControl("select");
            inputControl6.Attributes.Add("class", "form-control");


            DateTime currentDateTime = DateTime.Now;

            for(int i=0;i<40;i++)
            {
                int p=-1*i;
                DateTime d = currentDateTime.AddMonths(p);
                string monthString = d.ToString("MMM")+ " - " + d.Year;
                var opt = new HtmlGenericControl("option");
                inputControl6.Controls.Add(opt);

                DateTime temp = d.AddMonths(1);
                DateTime newTime = new DateTime(temp.Year,temp.Month,1,0,0,0);

                opt.Attributes.Add("value", newTime.ToString());
                opt.InnerText = monthString;

                if(ToDate.Equals(newTime.ToString())){
                    opt.Attributes.Add("selected","true");
                }

            }

            inputControl6.Attributes.Add("id", "selectReportingPeriod");
            inputControl6.Attributes.Add("type", "text");
            inputControl6.Attributes.Add("style", "z-index:8");
            inputControl6.Attributes.Add("name", "selectReportingPeriod");
            inputControl6.Attributes.Add("Visible", "false");
            inputControl6.Attributes.Add("title", "Health Facility option description");


            // Label control
            var labelControl = new Label()
            {
                Text = "Report Period"
            };

                var labelControl2 = new Label()
            {
                Text = "Antigen"
            };

                var labelControl3 = new Label()
            {
                Text = "District"
            };

            var row = new HtmlGenericControl("div");
            row.Attributes.Add("class", "row");
            // row.Attributes.Add("style", "margin:5px");

            var colMd4 = new HtmlGenericControl("div");
            colMd4.Attributes.Add("class", "col-md-1");
            var colMd42 = new HtmlGenericControl("div");
            colMd42.Attributes.Add("class", "col-md-1");
            var colMd43 = new HtmlGenericControl("div");
            colMd43.Attributes.Add("class", "col-md-1");

            var colMd8 = new HtmlGenericControl("div");
            colMd8.Attributes.Add("class", "col-md-3");
            var colMd82 = new HtmlGenericControl("div");
            colMd82.Attributes.Add("class", "col-md-3");
            var colMd83 = new HtmlGenericControl("div");
            colMd83.Attributes.Add("class", "col-md-3");

            row.Controls.Add(colMd4);
            row.Controls.Add(colMd8);
            row.Controls.Add(colMd42);
            row.Controls.Add(colMd82);
            row.Controls.Add(colMd43);
            row.Controls.Add(colMd83);

            colMd4.Controls.Add(labelControl);
            colMd8.Controls.Add(inputControl6);
            colMd42.Controls.Add(labelControl2);
            colMd82.Controls.Add(inputControl4);
            colMd43.Controls.Add(labelControl3);
            colMd83.Controls.Add(inputControl3);

            this.reportInputs.Controls.Add(row);

            Page.RegisterStartupScript("dateFrom",
                        string.Format(
                        "<script type=\"text/javascript\">Sys.Application.add_init(function() {{$create(Sys.Extended.UI.CalendarBehavior, {{\"format\":\"MM-dd-yyyy\",\"id\":\"dateFrom\"}}, null, null, $get(\"{0}\"));}});</script>", "dateFrom"));
            Page.RegisterStartupScript("dateTo",
                        string.Format(
                        "<script type=\"text/javascript\">Sys.Application.add_init(function() {{$create(Sys.Extended.UI.CalendarBehavior, {{\"format\":\"MM-dd-yyyy\",\"id\":\"dateTo\"}}, null, null, $get(\"{0}\"));}});</script>", "dateTo"));
                        //  $create(Sys.Extended.UI.CalendarBehavior, {"endDate":"Thu, 28 May 2015 00:00:00 GMT","format":"dd/MM/yyyy","id":"ctl00_ContentPlaceHolder1_ceBirthdateTo"}, null, null, $get("ctl00_ContentPlaceHolder1_txtBirthdateTo"));

        }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        string sessionvar = "_healthfacility_" + CurrentEnvironment.LoggedUser.HealthFacilityId.ToString();
        string s;
        if (Session[sessionvar] != null)
            s = Session[sessionvar].ToString();
        else
        {
            s = HealthFacility.GetAllChildsForOneHealthFacility(CurrentEnvironment.LoggedUser.HealthFacilityId);
            Session[sessionvar] = s;
        }


        ToDate = Request.Form["selectReportingPeriod"];

        DateTime oDate = DateTime.Parse(ToDate);
        DateTime fromDateTime = new DateTime(oDate.Year, 1,1, 1, 1, 1);
        DateTime toDateTime = new DateTime(oDate.Year, oDate.Month,1, 1, 1, 1);

        string strToDate = toDateTime.ToString();
        string strFromDate = fromDateTime.ToString();

        selectedHealthFacilityID = Request.Form["selectHealthFacility"];
        selectedDose = Request.Form["selectDose"];



        if(selectedDose == "all"){
                odsHealthFacilityDefaultersByDistrict.SelectParameters.Clear();
                odsHealthFacilityDefaultersByDistrict.SelectParameters.Add("districtCouncilId", selectedHealthFacilityID);
                odsHealthFacilityDefaultersByDistrict.SelectParameters.Add("fromDate", strFromDate);
                odsHealthFacilityDefaultersByDistrict.SelectParameters.Add("toDate", strToDate);
                odsHealthFacilityDefaultersByDistrict.DataBind();
                gvHealthFacilityDefaultersByDistrict.DataSourceID = "odsHealthFacilityDefaultersByDistrict";
        }else{
                odsHealthFacilityDefaultersByDistrictAndDose.SelectParameters.Clear();
                odsHealthFacilityDefaultersByDistrictAndDose.SelectParameters.Add("districtCouncilId", selectedHealthFacilityID);
                odsHealthFacilityDefaultersByDistrictAndDose.SelectParameters.Add("doseName", selectedDose);
                odsHealthFacilityDefaultersByDistrictAndDose.SelectParameters.Add("fromDate", strFromDate);
                odsHealthFacilityDefaultersByDistrictAndDose.SelectParameters.Add("toDate", strToDate);
                odsHealthFacilityDefaultersByDistrictAndDose.DataBind();
                gvHealthFacilityDefaultersByDistrict.DataSourceID = "odsHealthFacilityDefaultersByDistrictAndDose";
        }




        gvHealthFacilityDefaultersByDistrict.DataBind();

        createInputControls();


            DateTime d = oDate.AddMonths(-1);
            if(d.Month == 2){
                gvHealthFacilityDefaultersByDistrict.Columns[3].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[4].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[5].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[6].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[7].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[8].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[9].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[10].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[11].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[12].Visible = false;
            }
            else if(d.Month == 3){
                gvHealthFacilityDefaultersByDistrict.Columns[4].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[5].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[6].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[7].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[8].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[9].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[10].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[11].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[12].Visible = false;
            }
            else if(d.Month == 4){
                gvHealthFacilityDefaultersByDistrict.Columns[5].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[6].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[7].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[8].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[9].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[10].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[11].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[12].Visible = false;
            }
            else if(d.Month == 5){
                 gvHealthFacilityDefaultersByDistrict.Columns[6].Visible = false;
                 gvHealthFacilityDefaultersByDistrict.Columns[7].Visible = false;
                 gvHealthFacilityDefaultersByDistrict.Columns[8].Visible = false;
                 gvHealthFacilityDefaultersByDistrict.Columns[9].Visible = false;
                 gvHealthFacilityDefaultersByDistrict.Columns[10].Visible = false;
                 gvHealthFacilityDefaultersByDistrict.Columns[11].Visible = false;
                 gvHealthFacilityDefaultersByDistrict.Columns[12].Visible = false;
            }
            else if(d.Month == 6){
                gvHealthFacilityDefaultersByDistrict.Columns[7].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[8].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[9].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[10].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[11].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[12].Visible = false;
            }
            else if(d.Month == 7){
                gvHealthFacilityDefaultersByDistrict.Columns[8].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[9].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[10].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[11].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[12].Visible = false;
            }
            else if(d.Month == 8){
                gvHealthFacilityDefaultersByDistrict.Columns[9].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[10].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[11].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[12].Visible = false;
            }
            else if(d.Month == 9){
                gvHealthFacilityDefaultersByDistrict.Columns[10].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[11].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[12].Visible = false;
            }
            else if(d.Month == 10){
                gvHealthFacilityDefaultersByDistrict.Columns[11].Visible = false;
                gvHealthFacilityDefaultersByDistrict.Columns[12].Visible = false;
            }
            else if(d.Month == 11){
                gvHealthFacilityDefaultersByDistrict.Columns[12].Visible = false;
            }


        
    }

    protected void gvHealthFacilityDefaultersByDistrict_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHealthFacilityDefaultersByDistrict.PageIndex = e.NewPageIndex;
    }

    protected void gvHealthFacilityDefaultersByDistrict_DataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {

        ToDate = Request.Form["selectReportingPeriod"];
        selectedDose = Request.Form["selectDose"];
        selectedHealthFacilityID = Request.Form["selectHealthFacility"];

        DateTime oDate = DateTime.Parse(ToDate);
        DateTime fromDateTime = new DateTime(oDate.Year, 1,1, 1, 1, 1);
        DateTime toDateTime = new DateTime(oDate.Year, oDate.Month,1, 1, 1, 1);

        string strToDate = toDateTime.ToString();
        string strFromDate = fromDateTime.ToString();

        if(selectedDose == "all"){
            odsExport.SelectParameters.Clear();
            odsExport.SelectParameters.Add("districtCouncilId", selectedHealthFacilityID);
            odsExport.SelectParameters.Add("fromDate", strFromDate);
            odsExport.SelectParameters.Add("toDate", strToDate);
            odsExport.DataBind();

            gvExport.DataSourceID = "odsExport";
        }else{
            odsExportByDose.SelectParameters.Clear();
            odsExportByDose.SelectParameters.Add("districtCouncilId", selectedHealthFacilityID);
            odsExportByDose.SelectParameters.Add("doseName", selectedDose);
            odsExportByDose.SelectParameters.Add("fromDate", strFromDate);
            odsExportByDose.SelectParameters.Add("toDate", strToDate);
            odsExportByDose.DataBind();

            gvExport.DataSourceID = "odsExportByDose";
        }



            gvExport.DataBind();

            DateTime d = oDate.AddMonths(-1);

            if(d.Month == 2){
                gvExport.Columns[3].Visible = false;
                gvExport.Columns[4].Visible = false;
                gvExport.Columns[5].Visible = false;
                gvExport.Columns[6].Visible = false;
                gvExport.Columns[7].Visible = false;
                gvExport.Columns[8].Visible = false;
                gvExport.Columns[9].Visible = false;
                gvExport.Columns[10].Visible = false;
                gvExport.Columns[11].Visible = false;
                gvExport.Columns[12].Visible = false;
            }else if(d.Month == 3){
                gvExport.Columns[4].Visible = false;
                gvExport.Columns[5].Visible = false;
                gvExport.Columns[6].Visible = false;
                gvExport.Columns[7].Visible = false;
                gvExport.Columns[8].Visible = false;
                gvExport.Columns[9].Visible = false;
                gvExport.Columns[10].Visible = false;
                gvExport.Columns[11].Visible = false;
                gvExport.Columns[12].Visible = false;
            }else if(d.Month == 4){
                gvExport.Columns[5].Visible = false;
                gvExport.Columns[6].Visible = false;
                gvExport.Columns[7].Visible = false;
                gvExport.Columns[8].Visible = false;
                gvExport.Columns[9].Visible = false;
                gvExport.Columns[10].Visible = false;
                gvExport.Columns[11].Visible = false;
                gvExport.Columns[12].Visible = false;
            }else if(d.Month == 5){
                 gvExport.Columns[6].Visible = false;
                 gvExport.Columns[7].Visible = false;
                 gvExport.Columns[8].Visible = false;
                 gvExport.Columns[9].Visible = false;
                 gvExport.Columns[10].Visible = false;
                 gvExport.Columns[11].Visible = false;
                 gvExport.Columns[12].Visible = false;
            }else if(d.Month == 6){
                gvExport.Columns[7].Visible = false;
                gvExport.Columns[8].Visible = false;
                gvExport.Columns[9].Visible = false;
                gvExport.Columns[10].Visible = false;
                gvExport.Columns[11].Visible = false;
                gvExport.Columns[12].Visible = false;
            }else if(d.Month == 7){
                gvExport.Columns[8].Visible = false;
                gvExport.Columns[9].Visible = false;
                gvExport.Columns[10].Visible = false;
                gvExport.Columns[11].Visible = false;
                gvExport.Columns[12].Visible = false;
            }else if(d.Month == 8){
                gvExport.Columns[9].Visible = false;
                gvExport.Columns[10].Visible = false;
                gvExport.Columns[11].Visible = false;
                gvExport.Columns[12].Visible = false;
            }else if(d.Month == 9){
                gvExport.Columns[10].Visible = false;
                gvExport.Columns[11].Visible = false;
                gvExport.Columns[12].Visible = false;
            }else if(d.Month == 10){
                gvExport.Columns[11].Visible = false;
                gvExport.Columns[12].Visible = false;
            }else if(d.Month == 11){
                gvExport.Columns[12].Visible = false;
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=DefaultersByDistrict.xls");
            Response.Charset = "";

            Response.ContentType = "application/ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);


            gvExport.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
           return;
        }

        protected void gvOn_DataBound(object sender, EventArgs e)
        {
            if (gvHealthFacilityDefaultersByDistrict.Rows.Count > 0)
                btnExcel.Visible = true;
            else
                btnExcel.Visible = false;
        }
}