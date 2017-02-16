-- RETURNS: THE MAJOR, MINOR AND RELEASE NUMBER OF THE DATABASE SCHEMA

CREATE OR REPLACE FUNCTION GET_SCH_VER() RETURNS INTEGER AS

$$

BEGIN

	RETURN 3;

END;

$$ LANGUAGE plpgsql;







CREATE OR REPLACE FUNCTION UPDATE_DB() RETURNS VOID AS

$$

BEGIN

	/**

	 * Update: CR-001

	 * Applies to: 3

	 * Notes:

	 *	- Adds new tables for monthly reports sent from mobile

	 */

	IF GET_SCH_VER() > 2 THEN

	INSERT INTO public."MENU"("ID","PARENT_ID","TITLE","NAVIGATE_URL","IS_ACTIVE","DISPLAY_ORDER") VALUES (158,8,'Reports Configuration', 'ReportsConfiguration.aspx',TRUE,8);

	INSERT INTO public."ACTIONS"("ID","NAME","NOTES") VALUES (495,'ViewReportConfigurations', '');
	INSERT INTO public."ACTIONS"("ID","NAME","NOTES") VALUES (496,'ViewDistrictHealthFacilityLoginSessionsRatings', 'Districts health facilities ratings by login sessions');

	INSERT INTO public."REPORT_GROUP"("ID","GROUP_NAME") VALUES (5,'Login-Sessions Reports');
	
	INSERT INTO public."REPORT"("ID","JASPER_ID", "REPORT_NAME", "ACTION_ID", "GROUP_ID", "DESCRIPTION", "REPORT_TYPE") VALUES (21,'HealthFacilityChildRegistrationsRatings.aspx', 'Health Facility Children Registrations Ratings By District', 496, 5,'Non Jasper Reports','R');
	INSERT INTO public."REPORT"("ID","JASPER_ID", "REPORT_NAME", "ACTION_ID", "GROUP_ID", "DESCRIPTION", "REPORT_TYPE") VALUES (22,'HealthFacilityChildVaccinationsRatings.aspx', 'Health Facility Children Vaccinations Ratings By District',496, 5,'Non Jasper Reports','R');
	INSERT INTO public."REPORT"("ID","JASPER_ID", "REPORT_NAME", "ACTION_ID", "GROUP_ID", "DESCRIPTION", "REPORT_TYPE") VALUES (23,'HealthFacilitySessionDaysRatings.aspx', 'Health Facility Session Ratings Days By District',496, 5,'Non Jasper Reports','R');





    	ELSE

	DELETE FROM public."MENU" WHERE "TITLE"='Reports Configuration';
	DELETE FROM public."ACTIONS" WHERE "NAME"='ViewReportConfigurations';

	DELETE FROM public."REPORT_GROUP" WHERE "GROUP_NAME"='Login-Sessions Reports';
	DELETE FROM public."ACTIONS" WHERE "NAME"='ViewReportConfigurations';
	DELETE FROM public."ACTIONS" WHERE "NAME"='ViewDistrictHealthFacilityLoginSessionsRatings';

	DELETE FROM public."REPORT" WHERE "REPORT_NAME"='Health Facility Children Registrations Ratings By District';
	DELETE FROM public."REPORT" WHERE "REPORT_NAME"='Health Facility Children Vaccinations Ratings By District';
	DELETE FROM public."REPORT" WHERE "REPORT_NAME"='Health Facility Session Ratings Days By District';


	END IF;
END;

$$ LANGUAGE plpgsql;



select UPDATE_DB();