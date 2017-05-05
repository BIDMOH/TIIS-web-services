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

using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

public partial class Pages_HealthFacilityChildrenRegistrationsDefaulters : System.Web.UI.Page
{
    public static String datefromString = "";
    public static String datetoString = "";
    public String userID = "";
    public String selectedHealthFacilityID = "";
    public String selectedVillage = "";
    public int hfParentID = 0;
    public int userSelectedIndex = 0;
    public HtmlGenericControl inputControl3,inputControl4,inputControl2;
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

                //date-from controls
                inputControl3 = new HtmlGenericControl("select");

                inputControl3.Attributes.Add("class", "form-control");

                var contextParms = new List<NpgsqlParameter>() {
                    new NpgsqlParameter("@FacilityId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.HealthFacilityId },
                    new NpgsqlParameter("@FacilityCode", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.HealthFacility.Code },
                    new NpgsqlParameter("@UserId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.Id }
                };


                hfParentID = HealthFacility.GetHealthFacilityById(CurrentEnvironment.LoggedUser.HealthFacilityId).ParentId;


                string command="";

                int userId = CurrentEnvironment.LoggedUser.Id;
                UserRole role = UserRole.GetUserRoleByUserId(userId);
                 if(role.Role.Name.Equals("Middle Level Officer"))
                {
                    command = "SELECT \"ID\", \"NAME\" FROM \"HEALTH_FACILITY\" WHERE \"ID\" =  "+CurrentEnvironment.LoggedUser.HealthFacilityId+" OR  \"PARENT_ID\" = "+CurrentEnvironment.LoggedUser.HealthFacilityId+" ORDER BY \"NAME\" ";
                }else{
                    command = "SELECT \"ID\", \"NAME\" FROM \"HEALTH_FACILITY\" ORDER BY \"NAME\" ";
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

                //vilage-form control
                inputControl4 = new HtmlGenericControl("select");
                inputControl4.Attributes.Add("class", "form-control");
                string query = "SELECT  \"NAME\" FROM \"PLACE\" ORDER BY \"NAME\" ";

                var opt1 = new HtmlGenericControl("option");
                inputControl4.Controls.Add(opt1);
                opt1.Attributes.Add("value", "all");
                opt1.InnerText = "all";

                if(selectedVillage.Equals("all")){
                    opt1.Attributes.Add("selected","true");
                }
                using (var idt2 = DBManager.ExecuteReaderCommand(query, System.Data.CommandType.Text, null))
                {
                    using (var irdr1 = idt2.CreateDataReader())
                    {

                        while (irdr1.Read())
                        {

                            var opt = new HtmlGenericControl("option");
                            inputControl4.Controls.Add(opt);
                            opt.Attributes.Add("value", irdr1[0].ToString());
                            opt.InnerText = irdr1[0].ToString();

                            if(selectedVillage.Equals(irdr1[0].ToString())){
                                opt.Attributes.Add("selected","true");
                            }
                        }
                    }
                }

                inputControl4.Attributes.Add("id", "selectVillage");
                inputControl4.Attributes.Add("type", "text");
                inputControl4.Attributes.Add("style", "z-index:8");
                inputControl4.Attributes.Add("name", "selectVillage");
                inputControl4.Attributes.Add("Visible", "false");
                inputControl4.Attributes.Add("title", "Dose");

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
                    Text = "From"
                };

                    var labelControl2 = new Label()
                {
                    Text = "To"
                };

                    var labelControl3 = new Label()
                {
                    Text = "Health Facility"
                };

                 var labelControl4 = new Label()
                {
                    Text = "Village"
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

                var colMd44 = new HtmlGenericControl("div");
                colMd44.Attributes.Add("class", "col-md-1");

                var colMd8 = new HtmlGenericControl("div");
                colMd8.Attributes.Add("class", "col-md-2");
                var colMd82 = new HtmlGenericControl("div");
                colMd82.Attributes.Add("class", "col-md-2");
                var colMd83 = new HtmlGenericControl("div");
                colMd83.Attributes.Add("class", "col-md-2");
                var colMd84 = new HtmlGenericControl("div");
                colMd84.Attributes.Add("class", "col-md-2");

                row.Controls.Add(colMd4);
                row.Controls.Add(colMd8);
                row.Controls.Add(colMd42);
                row.Controls.Add(colMd82);
                row.Controls.Add(colMd43);
                row.Controls.Add(colMd83);
                row.Controls.Add(colMd44);
                row.Controls.Add(colMd84);

                colMd4.Controls.Add(labelControl);
                colMd8.Controls.Add(inputControl);
                colMd42.Controls.Add(labelControl2);
                colMd82.Controls.Add(inputControl2);
                colMd43.Controls.Add(labelControl3);
                colMd83.Controls.Add(inputControl3);
                colMd44.Controls.Add(labelControl4);
                colMd84.Controls.Add(inputControl4);

                this.reportInputs.Controls.Add(row);

                Page.RegisterStartupScript("dateFrom",
                            string.Format(
                            "<script type=\"text/javascript\">Sys.Application.add_init(function() {{$create(Sys.Extended.UI.CalendarBehavior, {{\"format\":\"MM-dd-yyyy\",\"id\":\"dateFrom\"}}, null, null, $get(\"{0}\"));}});</script>", "dateFrom"));
                Page.RegisterStartupScript("dateTo",
                            string.Format(
                            "<script type=\"text/javascript\">Sys.Application.add_init(function() {{$create(Sys.Extended.UI.CalendarBehavior, {{\"format\":\"MM-dd-yyyy\",\"id\":\"dateTo\"}}, null, null, $get(\"{0}\"));}});</script>", "dateTo"));
            }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

         string sessionvar = "_healthfacility_" + CurrentEnvironment.LoggedUser.HealthFacilityId.ToString();
                string s;
                if (Session[sessionvar] != null)
                {
                    s = Session[sessionvar].ToString();
                }
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
        selectedVillage = Request.Form["selectVillage"];

        if(selectedVillage != "all"){
                odsHealthFacilityDefaultersByVillage.SelectParameters.Clear();
                odsHealthFacilityDefaultersByVillage.SelectParameters.Add("hfid", selectedHealthFacilityID);
                odsHealthFacilityDefaultersByVillage.SelectParameters.Add("village", selectedVillage);
                odsHealthFacilityDefaultersByVillage.SelectParameters.Add("fromDate", strFromDate);
                odsHealthFacilityDefaultersByVillage.SelectParameters.Add("toDate", strToDate);
                odsHealthFacilityDefaultersByVillage.DataBind();
                gvHealthFacilityDefaulters.DataSourceID = "odsHealthFacilityDefaultersByVillage";
        }
        else{
                odsHealthFacilityDefaulters.SelectParameters.Clear();
                odsHealthFacilityDefaulters.SelectParameters.Add("hfid", selectedHealthFacilityID);
                odsHealthFacilityDefaulters.SelectParameters.Add("fromDate", strFromDate);
                odsHealthFacilityDefaulters.SelectParameters.Add("toDate", strToDate);
                odsHealthFacilityDefaulters.DataBind();
                gvHealthFacilityDefaulters.DataSourceID = "odsHealthFacilityDefaulters";
        }


        gvHealthFacilityDefaulters.DataBind();

        createInputControls();
        
    }

    protected void gvHealthFacilityDefaulters_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHealthFacilityDefaulters.PageIndex = e.NewPageIndex;
    }

    protected void gvHealthFacilityDefaulters_RowDataBound(object sender, EventArgs e)
    {

    }


   protected void btnExcel_Click(object sender, EventArgs e)
    {

        selectedHealthFacilityID = Request.Form["selectHealthFacility"];
        selectedVillage = Request.Form["selectVillage"];
        string strFromDate = String.Format("{0}", Request.Form["dateFrom"]);
        string strToDate = String.Format("{0}", Request.Form["dateTo"]);

         if(selectedVillage != "all"){
         odsExportByVillage.SelectParameters.Clear();
         odsExportByVillage.SelectParameters.Add("hfid", selectedHealthFacilityID);
         odsExportByVillage.SelectParameters.Add("village", selectedVillage);
         odsExportByVillage.SelectParameters.Add("fromDate", strFromDate);
         odsExportByVillage.SelectParameters.Add("toDate", strToDate);
         odsExport.DataBind();

         gvExport.DataSourceID = "odsExportByVillage";
         }
         else{
         odsExport.SelectParameters.Clear();
         odsExport.SelectParameters.Add("hfid", selectedHealthFacilityID);
         odsExport.SelectParameters.Add("fromDate", strFromDate);
         odsExport.SelectParameters.Add("toDate", strToDate);
         odsExport.DataBind();

         gvExport.DataSourceID = "odsExport";
         }


        gvExport.DataBind();

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=DefaulterList.xls");
        Response.Charset = "";

        Response.ContentType = "application/ms-excel";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);


        gvExport.RenderControl(htmlWrite);
        Response.Write(stringWrite.ToString());
        Response.End();
    }

     protected void btnPdf_Click(object sender, EventArgs e)
        {

            selectedHealthFacilityID = Request.Form["selectHealthFacility"];
            hfParentID = HealthFacility.GetHealthFacilityById(Convert.ToInt32(selectedHealthFacilityID)).ParentId;
            string facilityName = HealthFacility.GetHealthFacilityById(Convert.ToInt32(selectedHealthFacilityID)).Name;
            string ParentName = HealthFacility.GetHealthFacilityById(hfParentID).Name;
            selectedVillage = Request.Form["selectVillage"];
            string VillageName ="";
            if(selectedVillage != "all"){
            VillageName = Place.GetPlaceById(Convert.ToInt32(selectedVillage)).Name;
            }
            string strFromDate = String.Format("{0}", Request.Form["dateFrom"]);
            string strToDate = String.Format("{0}", Request.Form["dateTo"]);

             if(selectedVillage != "all"){
             odsExportByVillage.SelectParameters.Clear();
             odsExportByVillage.SelectParameters.Add("hfid", selectedHealthFacilityID);
             odsExportByVillage.SelectParameters.Add("village", selectedVillage);
             odsExportByVillage.SelectParameters.Add("fromDate", strFromDate);
             odsExportByVillage.SelectParameters.Add("toDate", strToDate);
             odsExport.DataBind();

             gvExport.DataSourceID = "odsExportByVillage";
             }
             else{
             odsExport.SelectParameters.Clear();
             odsExport.SelectParameters.Add("hfid", selectedHealthFacilityID);
             odsExport.SelectParameters.Add("fromDate", strFromDate);
             odsExport.SelectParameters.Add("toDate", strToDate);
             odsExport.DataBind();

             gvExport.DataSourceID = "odsExport";
             }


            gvExport.DataBind();


            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Health Facility Children Defaulters List.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvExport.RenderControl(hw);

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            iTextSharp.text.Font font20 = iTextSharp.text.FontFactory.GetFont
            (iTextSharp.text.FontFactory.HELVETICA,2);

            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            var style = new StyleSheet();
            style.LoadTagStyle("body", "size", "8px");


            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.SetStyleSheet(style);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            if(selectedVillage != "all"){
                Paragraph paragraph = new Paragraph("Health Facility Child Defaulters List");
                Paragraph paragraph2 = new Paragraph("Region : "+ ParentName);
                Paragraph paragraph3 = new Paragraph("District : "+facilityName);
                Paragraph paragraph4 = new Paragraph("Village : "+VillageName);
                Paragraph paragraph5 = new Paragraph("Reporting Period : "+strFromDate +" to "+strToDate);

                paragraph.Alignment = Element.ALIGN_CENTER;
                paragraph2.Alignment = Element.ALIGN_CENTER;
                paragraph3.Alignment = Element.ALIGN_CENTER;
                paragraph4.Alignment = Element.ALIGN_CENTER;
                paragraph5.Alignment = Element.ALIGN_CENTER;
                paragraph5.SpacingAfter = 20f;

                string imageURL = Server.MapPath("..") + "/img/logo_tiis_.png";
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.ScaleToFit(570f, 120f);

                jpg.SpacingBefore = 10f;
                //Give some space after the image
                jpg.SpacingAfter = 5f;

                pdfDoc.Add(jpg);
                pdfDoc.Add(paragraph);
                pdfDoc.Add(paragraph2);
                pdfDoc.Add(paragraph3);
                pdfDoc.Add(paragraph4);
                pdfDoc.Add(paragraph5);

            }else{
                Paragraph paragraph = new Paragraph("Health Facility Child Defaulters List");
                Paragraph paragraph2 = new Paragraph("Region : "+ ParentName);
                Paragraph paragraph3 = new Paragraph("District : "+facilityName);
                Paragraph paragraph4 = new Paragraph("Village : All");
                Paragraph paragraph5 = new Paragraph("Reporting Period : "+strFromDate +" to "+strToDate);
                paragraph.Alignment = Element.ALIGN_CENTER;
                paragraph2.Alignment = Element.ALIGN_CENTER;
                paragraph3.Alignment = Element.ALIGN_CENTER;
                paragraph4.Alignment = Element.ALIGN_CENTER;
                paragraph5.Alignment = Element.ALIGN_CENTER;
                paragraph5.SpacingAfter = 20f;

                string imageURL = Server.MapPath("..") + "/img/logo_tiis_.png";
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.ScaleToFit(570f, 120f);

                jpg.SpacingBefore = 10f;
                //Give some space after the image
                jpg.SpacingAfter = 5f;

                pdfDoc.Add(jpg);
                pdfDoc.Add(paragraph);
                pdfDoc.Add(paragraph2);
                pdfDoc.Add(paragraph3);
                pdfDoc.Add(paragraph4);
                pdfDoc.Add(paragraph5);
            }





            htmlparser.Parse(sr);

            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();

            gvExport.AllowPaging = true;
            gvExport.DataBind();
        }

    public override void VerifyRenderingInServerForm(Control control)
    {
       return;
    }

    protected void gvOn_DataBound(object sender, EventArgs e)
    {
        if (gvHealthFacilityDefaulters.Rows.Count > 0){
            btnExcel.Visible = true;
            btnPdf.Visible = true;
        }
        else{
            btnExcel.Visible = false;
            btnPdf.Visible = false;
        }
    }
}