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

public partial class Pages_HealthFacilitySessionRatings : System.Web.UI.Page
{
    public static String datefromString = "";
    public static String datetoString = "";
    public String userID = "";
    public String selectedHealthFacilityID = "";
    public int hfParentID = 0;
    public int userSelectedIndex = 0;
    public HtmlGenericControl inputControl3;
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

                //grid header text
                gvHealthFacilitySessions.Columns[0].HeaderText = "Name";
                gvHealthFacilitySessions.Columns[1].HeaderText = "Child Vaccinations";
                gvHealthFacilitySessions.Columns[2].HeaderText = "View Facility Session Reports";
                
            }
            else
            {
                
                Response.Redirect("Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
    }

    protected void createInputControls(){
        //date-from controls
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


        int userId = CurrentEnvironment.LoggedUser.Id;
        UserRole role = UserRole.GetUserRoleByUserId(userId);

        string command;
        if(role.Role.Name.Equals("Middle Level Officer"))
        {
            command = "SELECT \"ID\", \"NAME\" FROM \"HEALTH_FACILITY\" WHERE \"TYPE_ID\" = 2  AND \"ID\" = "+CurrentEnvironment.LoggedUser.HealthFacilityId;
        }else{
            command = "SELECT \"ID\", \"NAME\" FROM \"HEALTH_FACILITY\" WHERE \"TYPE_ID\" = "+2;
        }

        using (var idt = DBManager.ExecuteReaderCommand(command, System.Data.CommandType.Text, contextParms))
        {
            using (var irdr = idt.CreateDataReader())
            {
                
                int count =0;
                while (irdr.Read())
                {

                    var opt = new HtmlGenericControl("option");
                    inputControl3.Controls.Add(opt);
                    opt.Attributes.Add("value", irdr[0].ToString());
                    opt.InnerText = irdr[1].ToString();

                    if(selectedHealthFacilityID.Equals(irdr[0].ToString())){
                        opt.Attributes.Add("selected","true");
                    }

                    count++;
                }
            }
        }

        inputControl3.Attributes.Add("id", "selectHealthFacility");
        inputControl3.Attributes.Add("type", "text");
        inputControl3.Attributes.Add("style", "z-index:8");
        inputControl3.Attributes.Add("name", "selectHealthFacility");
        inputControl3.Attributes.Add("Visible", "false");
        inputControl3.Attributes.Add("title", "Health Facility option description");

        //date-from controls
        var inputControl = new HtmlGenericControl("input");
        inputControl.Attributes.Add("class", "form-control");

        inputControl.Attributes.Add("id", "dateFrom");
        inputControl.Attributes.Add("type", "text");
        inputControl.Attributes.Add("style", "z-index:8");
        inputControl.Attributes.Add("name", "dateFrom");
        inputControl.Attributes.Add("value", datefromString);
        inputControl.Attributes.Add("title", "Session Report Description");

        //date-to Controls
        var inputControl2 = new HtmlGenericControl("input");
        inputControl2.Attributes.Add("class", "form-control");

        inputControl2.Attributes.Add("id", "dateTo");
        inputControl2.Attributes.Add("type", "text");
        inputControl2.Attributes.Add("style", "z-index:8");
        inputControl2.Attributes.Add("name", "dateTo");
        inputControl2.Attributes.Add("value", datetoString);
        inputControl2.Attributes.Add("title", "Session Report Description");

        // Label control
        var labelControl = new Label()
        {
            Text = "From Date"
        };

            var labelControl2 = new Label()
        {
            Text = "To Date"
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
        colMd8.Controls.Add(inputControl);
        colMd42.Controls.Add(labelControl2);
        colMd82.Controls.Add(inputControl2);
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

        string strFromDate = String.Format("{0}", Request.Form["dateFrom"]);
        string strToDate = String.Format("{0}", Request.Form["dateTo"]);

        datefromString  = strFromDate;
        datetoString    = strToDate;

        selectedHealthFacilityID = Request.Form["selectHealthFacility"];

        odsHealthFacilitySessionsVaccinations.SelectParameters.Clear();
        odsHealthFacilitySessionsVaccinations.SelectParameters.Add("districtCouncilId", selectedHealthFacilityID);
        odsHealthFacilitySessionsVaccinations.SelectParameters.Add("fromDate", strFromDate);
        odsHealthFacilitySessionsVaccinations.SelectParameters.Add("toDate", strToDate);
        odsHealthFacilitySessionsVaccinations.DataBind();
        gvHealthFacilitySessions.DataSourceID = "odsHealthFacilitySessionsVaccinations";
        gvHealthFacilitySessions.DataBind();


        createInputControls();
        
    }

    protected void gvHealthFacilitySessions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHealthFacilitySessions.PageIndex = e.NewPageIndex;
    }

    protected void gvHealthFacilitySessions_DataBound(object sender, GridViewRowEventArgs e)
    {
       if (e.Row.RowType != DataControlRowType.Header)
		{
			if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ChildrenVaccinationsMaximumThreshold")) != null && Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ChildrenVaccinationsMinimumThreshold")) != null)
			{
				if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ChildrenVaccinationsMaximumThreshold")) != 0 && Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ChildrenVaccinationsMinimumThreshold")) != 0)
				{
					if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "SessionsCount")) > Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ChildrenVaccinationsMaximumThreshold")))
					{
						e.Row.ForeColor = System.Drawing.Color.Green;
					}
					else if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "SessionsCount")) < Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ChildrenVaccinationsMinimumThreshold")))
					{
						e.Row.ForeColor = System.Drawing.Color.Red;
					}
				}
			}
		}

    }
    
}