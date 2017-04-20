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
using System.Linq;
using Npgsql;

namespace GIIS.DataLayer
{
	public partial class HealthFacilityVaccinationSummary
	{

		#region Properties

		public Int32 registered { get; set; }
		public Int32 home { get; set; }
		public Int32 facility { get; set; }
		public Int32 vaccinated { get; set; }



		#endregion

		public static List<HealthFacilityVaccinationSummary> HealthFacilityVaccinationSummaryList(string hfid, DateTime fromDate, DateTime toDate)
		{
			try
			{
				string query = @"select COUNT(""CHILD"".""ID"") AS REGISTERED from ""CHILD"" 				
                                inner join ""HEALTH_FACILITY"" ON ""CHILD"".""HEALTHCENTER_ID"" = ""HEALTH_FACILITY"".""ID"" 
					            WHERE
					            (""HEALTH_FACILITY"".""ID"" = @hfid or ""HEALTH_FACILITY"".""PARENT_ID"" = @hfid) AND ""CHILD"".""MODIFIED_ON"" >=@fromDate AND ""CHILD"".""MODIFIED_ON"" <= @toDate ";
				
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);


				string query0 = "select * from crosstab($$ select t1.registered,t1.\"NAME\", t1.place from (select \"CHILD\".\"HEALTHCENTER_ID\" AS REGISTERED, \"BIRTHPLACE\".\"NAME\",COUNT(\"BIRTHPLACE\".\"NAME\") AS PLACE from \"CHILD\" " +
								"inner join \"BIRTHPLACE\" on \"CHILD\".\"BIRTHPLACE_ID\" = \"BIRTHPLACE\".\"ID\"  " +
								"inner join \"HEALTH_FACILITY\" ON \"CHILD\".\"HEALTHCENTER_ID\" = \"HEALTH_FACILITY\".\"ID\" " +
					            "WHERE "+
								" (\"HEALTH_FACILITY\".\"ID\" = "+ hfid + " or \"HEALTH_FACILITY\".\"PARENT_ID\" = " + hfid + ") AND \"CHILD\".\"MODIFIED_ON\" >= '" + fromDate + "' AND \"CHILD\".\"MODIFIED_ON\" <= '"+ toDate + "'  AND(\"BIRTHPLACE\".\"NAME\" = 'Home' OR \"BIRTHPLACE\".\"NAME\" = 'Health Facility') GROUP BY \"CHILD\".\"HEALTHCENTER_ID\",\"BIRTHPLACE\".\"NAME\" order by \"CHILD\".\"HEALTHCENTER_ID\",\"BIRTHPLACE\".\"NAME\"  )  as t1 $$) as final_result(\"id\" int, \"facility\" bigint, \"home\" bigint ) ";
				
				List<NpgsqlParameter> parameters0 = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt0 = DBManager.ExecuteReaderCommand(query0, CommandType.Text, parameters0);

				string query1 = @"select ""CHILD"".""HEALTHCENTER_ID"",COUNT(""CHILD"".""ID"") AS vaccinated from ""VACCINATION_EVENT""
								inner join ""CHILD"" on ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID""
								inner join ""DOSE"" on ""VACCINATION_EVENT"".""DOSE_ID"" = ""DOSE"".""ID""
								inner join ""SCHEDULED_VACCINATION"" on ""DOSE"".""SCHEDULED_VACCINATION_ID"" = ""SCHEDULED_VACCINATION"".""ID""
								inner join ""HEALTH_FACILITY"" ON ""VACCINATION_EVENT"".""HEALTH_FACILITY_ID"" = ""HEALTH_FACILITY"".""ID""
								WHERE
								""VACCINATION_STATUS"" = true AND ""SCHEDULED_DATE"" <= NOW() AND (""HEALTH_FACILITY"".""ID"" = @hfid or ""HEALTH_FACILITY"".""PARENT_ID"" = @hfid) AND ""VACCINATION_EVENT"".""VACCINATION_DATE"" >=@fromDate AND ""VACCINATION_EVENT"".""VACCINATION_DATE"" <= @toDate ORDER BY ""CHILD"".""HEALTHCENTER_ID"" ";

				List<NpgsqlParameter> parameters1 = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt1 = DBManager.ExecuteReaderCommand(query1, CommandType.Text, parameters1);

				return GetHealthFacilityVaccinationSummaryAsList(dt,dt0,dt1);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("Child", "GetHealthFacilityVaccinationSummary", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}






		#region Helper Methods       

		public static List<HealthFacilityVaccinationSummary> GetHealthFacilityVaccinationSummaryAsList(DataTable dt, DataTable dt0, DataTable dt1)
		{
			HealthFacilityVaccinationSummary o = new HealthFacilityVaccinationSummary();
			List<HealthFacilityVaccinationSummary> oList = new List<HealthFacilityVaccinationSummary>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{					
				    o.registered = Helper.ConvertToInt(row["registered"]);
					

					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityVaccinationSummary", "HealthFacilityVaccinationSummaryAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			foreach (DataRow row in dt0.Rows)
			{
				try
				{					
					o.home = Helper.ConvertToInt(row["home"]);
					o.facility = Helper.ConvertToInt(row["facility"]);
					

					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityVaccinationSummary", "HealthFacilityVaccinationSummaryAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			foreach (DataRow row in dt1.Rows)
			{
				try
				{
					
					o.vaccinated = Helper.ConvertToInt(row["vaccinated"]);

					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityVaccinationSummary", "HealthFacilityVaccinationSummaryAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return oList;
		}








		#endregion

	}
}

