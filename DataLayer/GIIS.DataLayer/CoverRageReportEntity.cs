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
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class CoverageReportEntity
    {

        #region Properties
        public string DoseName { get; set; }
        public Int32 MaleWithinCatchment { get; set; }
		public Int32 FemaleWithinCatchment { get; set; }
		public Int32 TotalWithinCatchment { get; set; }
		public Int32 MaleOutsideCatchment { get; set; }
		public Int32 FemaleOutsideCatchment { get; set; }
		public Int32 TotalOutsideCatchment { get; set; }
		public Int32 TotalVaccinations { get; set; }
		public Int32 ExpectedTotalCatchmentsVaccinations { get; set; }
		public Int32 CoveragePercentage { get; set; }
        #endregion

        #region GetData
		public static List<CoverageReportEntity> GetDistrictCoverageReport(string scheduledVaccinationName, int healthFacilityId, DateTime fromDate, DateTime toDate)
        {

			string query = "SELECT * FROM " +
					"(SELECT * FROM CROSSTAB($$ SELECT T2.\"FULLNAME\", T2.\"GENDER\", T2.\"count\" FROM(" +
					"SELECT T1.\"GENDER\", T1.\"FULLNAME\", COUNT(T1.\"CHILD_ID\") FROM " +
					"(select * from \"VACCINATION_EVENT\" " +
					"inner join \"CHILD\" on \"VACCINATION_EVENT\".\"CHILD_ID\" = \"CHILD\".\"ID\" " +
					"inner join \"DOSE\" on \"VACCINATION_EVENT\".\"DOSE_ID\" = \"DOSE\".\"ID\" " +
					"inner join \"SCHEDULED_VACCINATION\" on \"DOSE\".\"SCHEDULED_VACCINATION_ID\" = \"SCHEDULED_VACCINATION\".\"ID\" " +
					"inner join \"HEALTH_FACILITY\" ON \"VACCINATION_EVENT\".\"HEALTH_FACILITY_ID\" = \"HEALTH_FACILITY\".\"ID\"  " +
					"WHERE " +
					" \"VACCINATION_STATUS\" = true AND " +
					" \"SCHEDULED_VACCINATION\".\"NAME\" = '" + scheduledVaccinationName + "' AND  " +
					" (\"HEALTH_FACILITY\".\"ID\" = " + healthFacilityId + " OR \"HEALTH_FACILITY\".\"PARENT_ID\" = " + healthFacilityId + ") AND " +
					" \"CHILD\".\"HEALTHCENTER_ID\" = \"VACCINATION_EVENT\".\"HEALTH_FACILITY_ID\") " +
					"AS T1 GROUP BY T1.\"FULLNAME\", T1.\"GENDER\" ORDER BY T1.\"FULLNAME\", T1.\"GENDER\"  " +

					")AS T2  $$) AS final_result(DOSE TEXT, FEMALEWithin BIGINT, MALEwithin BIGINT) " +
					") AS table1  INNER JOIN " +
					"(SELECT * FROM CROSSTAB($$ SELECT T2.\"FULLNAME\", T2.\"GENDER\", T2.\"count\" FROM( " +
					"SELECT T1.\"GENDER\", T1.\"FULLNAME\", COUNT(T1.\"CHILD_ID\") FROM " +
					"(select * from \"VACCINATION_EVENT\"  " +
					"inner join \"CHILD\" on \"VACCINATION_EVENT\".\"CHILD_ID\" = \"CHILD\".\"ID\" " +
					"inner join \"DOSE\" on \"VACCINATION_EVENT\".\"DOSE_ID\" = \"DOSE\".\"ID\"  " +
					"inner join \"SCHEDULED_VACCINATION\" on \"DOSE\".\"SCHEDULED_VACCINATION_ID\" = \"SCHEDULED_VACCINATION\".\"ID\" " +
					"inner join \"HEALTH_FACILITY\" ON \"VACCINATION_EVENT\".\"HEALTH_FACILITY_ID\" = \"HEALTH_FACILITY\".\"ID\" " +
					"WHERE " +
					" \"VACCINATION_STATUS\" = true AND  " +
					" \"SCHEDULED_VACCINATION\".\"NAME\" = '" + scheduledVaccinationName + "' AND " +
					" (\"HEALTH_FACILITY\".\"ID\" = " + healthFacilityId + " OR \"HEALTH_FACILITY\".\"PARENT_ID\" = " + healthFacilityId + ") AND " +
					" \"CHILD\".\"HEALTHCENTER_ID\" <> \"VACCINATION_EVENT\".\"HEALTH_FACILITY_ID\") " +
					"AS T1 GROUP BY T1.\"FULLNAME\", T1.\"GENDER\" ORDER BY T1.\"FULLNAME\", T1.\"GENDER\" " +
					")AS T2  $$) AS final_result(DOSE TEXT, FEMALEoutside BIGINT, MALEoutside BIGINT) " +
					") AS table2 " +
					"ON table1.\"dose\" = table2.\"dose\" " +
					" INNER JOIN(select \"FULLNAME\" AS dose, COUNT(DISTINCT \"VACCINATION_EVENT\".\"ID\") AS EXPECTED_TOTAL from \"VACCINATION_EVENT\" " +
					" inner join \"CHILD\" on \"VACCINATION_EVENT\".\"CHILD_ID\" = \"CHILD\".\"ID\" " +
					" inner join \"DOSE\" on \"VACCINATION_EVENT\".\"DOSE_ID\" = \"DOSE\".\"ID\" " +
					 " inner join \"SCHEDULED_VACCINATION\" on \"DOSE\".\"SCHEDULED_VACCINATION_ID\" = \"SCHEDULED_VACCINATION\".\"ID\" " +
					" inner join \"HEALTH_FACILITY\" ON \"VACCINATION_EVENT\".\"HEALTH_FACILITY_ID\" = \"HEALTH_FACILITY\".\"ID\"  " +
					" WHERE " +
					" \"SCHEDULED_VACCINATION\".\"NAME\" = 'OPV' AND " +
					" (\"HEALTH_FACILITY\".\"ID\" = 12610 OR \"HEALTH_FACILITY\".\"PARENT_ID\" = 12610) AND " +
					" \"CHILD\".\"HEALTHCENTER_ID\" = \"VACCINATION_EVENT\".\"HEALTH_FACILITY_ID\"  GROUP BY \"FULLNAME\") " +
				" AS table3 ON table1.\"dose\" = table3.\"dose\" ";


			try
            {

				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@scheduledVaccinationName", DbType.String) { Value = scheduledVaccinationName },
					new NpgsqlParameter("@healthFacilityId", DbType.Int32) { Value = healthFacilityId },
					new NpgsqlParameter("@vaccinationStatus", DbType.Boolean) { Value = true }
				};

				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

                return GetCoverageReport(dt);
            }
            catch (Exception ex)
            {
				Log.InsertEntity("CoverageReportEntity", "GetDistrictCoverageReport", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw new Exception(query);
            }
        }


		public static List<CoverageReportEntity> GetCoverageReport(DataTable dt)
        {
			List<CoverageReportEntity> oList = new List<CoverageReportEntity>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
					CoverageReportEntity o = new CoverageReportEntity();
					o.DoseName = row["dose"].ToString();
					o.MaleWithinCatchment   = Helper.ConvertToInt(row["malewithin"]);
					o.FemaleWithinCatchment = Helper.ConvertToInt(row["femalewithin"]);
					o.FemaleOutsideCatchment = Helper.ConvertToInt(row["femaleoutside"]);
					o.MaleOutsideCatchment  = Helper.ConvertToInt(row["maleoutside"]);

					o.ExpectedTotalCatchmentsVaccinations = Helper.ConvertToInt(row["expected_total"]);

					o.TotalWithinCatchment  = o.MaleWithinCatchment + o.FemaleWithinCatchment;
					o.TotalOutsideCatchment = o.MaleOutsideCatchment + o.FemaleOutsideCatchment;
					o.TotalVaccinations = o.TotalWithinCatchment + o.TotalOutsideCatchment;

					o.CoveragePercentage = (o.TotalWithinCatchment * 100)/o.ExpectedTotalCatchmentsVaccinations;

                    oList.Add(o);
                }
                catch (Exception ex)
                {
					Log.InsertEntity("CoverageReportEntity", "GetCoverageReport", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
