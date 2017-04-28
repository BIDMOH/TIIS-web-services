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
		public String facilityName { get; set; }
		public Int32 registered { get; set; }
		public Int32 home { get; set; }
		public Int32 facility { get; set; }
		public Int32 vaccinated { get; set; }



		#endregion

		public static List<HealthFacilityVaccinationSummary> HealthFacilityVaccinationSummaryList(string hfid, DateTime fromDate, DateTime toDate)
		{
			if (hfid.Equals(""))
			{
				return new List<HealthFacilityVaccinationSummary>();
			}
			toDate = toDate.AddDays(1);
			try
			{
				string query = "SELECT COALESCE(T1.\"NAME\", T2.\"NAME\",T3.\"NAME\") as \"NAME\", T1.REGISTERED,T2.VACCINATED, T3.\"REGISTERED_FACILITY\",T3.\"REGISTERED_HOME\" FROM (select \"HEALTH_FACILITY\".\"NAME\", COUNT(\"CHILD\".\"ID\") AS REGISTERED \n" +
								"   from \"CHILD\"     \n" +
								"   inner join \"HEALTH_FACILITY\" ON \"CHILD\".\"HEALTHCENTER_ID\" = \"HEALTH_FACILITY\".\"ID\" \n" +
								"   WHERE\n" +
								"       (\"HEALTH_FACILITY\".\"ID\" IN (SELECT DISTINCT A.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = "+hfid+"\nUNION\nSELECT DISTINCT B.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = "+hfid+"\nUNION\nSELECT DISTINCT C.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = "+hfid+"\nUNION\nSELECT DISTINCT D.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = "+hfid+")) AND \n" +
								"       \"CHILD\".\"MODIFIED_ON\" >='"+fromDate+"' AND \n" +
								"       \"CHILD\".\"MODIFIED_ON\" < '"+toDate+"' \n" +
								"      group by \"HEALTH_FACILITY\".\"NAME\" order by \"HEALTH_FACILITY\".\"NAME\"\n" +
								"  ) AS T1\n" +
								"  FULL JOIN (\n" +
								"   select \"HEALTH_FACILITY\".\"NAME\",COUNT(DISTINCT \"CHILD\".\"ID\") AS vaccinated \n" +
								"   from \"VACCINATION_EVENT\"\n" +
								"    inner join \"CHILD\" on \"VACCINATION_EVENT\".\"CHILD_ID\" = \"CHILD\".\"ID\"\n" +
								"    inner join \"HEALTH_FACILITY\" ON \"VACCINATION_EVENT\".\"HEALTH_FACILITY_ID\" = \"HEALTH_FACILITY\".\"ID\"\n" +
								"    WHERE\n" +
								"     \"VACCINATION_STATUS\" = true AND \n" +
								"     \"SCHEDULED_DATE\" <= NOW() AND \n" +
								"     (\"HEALTH_FACILITY\".\"ID\" IN (SELECT DISTINCT A.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = "+hfid+"\nUNION\nSELECT DISTINCT B.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = "+hfid+"\nUNION\nSELECT DISTINCT C.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = "+hfid+"\nUNION\nSELECT DISTINCT D.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = "+hfid+") ) AND \n" +
								"     \"VACCINATION_EVENT\".\"VACCINATION_DATE\" >='"+fromDate+"' AND \n" +
								"     \"VACCINATION_EVENT\".\"VACCINATION_DATE\" < '"+toDate+"' \n" +
								"     GROUP BY \"HEALTH_FACILITY\".\"NAME\" ORDER BY \"HEALTH_FACILITY\".\"NAME\" \n" +
								"   )AS T2 ON T1.\"NAME\"=T2.\"NAME\"\n" +
								"  FULL JOIN (\n" +
								"   select * from crosstab($$ select t1.registered,t1.\"NAME\", t1.place \n" +
								"    from ( \n" +
								"    select \"HEALTH_FACILITY\".\"NAME\" AS REGISTERED, \"BIRTHPLACE\".\"NAME\",COUNT(\"BIRTHPLACE\".\"NAME\") AS PLACE from \"CHILD\" \n" +
								"     inner join \"BIRTHPLACE\" on \"CHILD\".\"BIRTHPLACE_ID\" = \"BIRTHPLACE\".\"ID\" \n" +
								"     inner join \"HEALTH_FACILITY\" ON \"CHILD\".\"HEALTHCENTER_ID\" = \"HEALTH_FACILITY\".\"ID\" \n" +
								"    WHERE \n" +
								"      (\"HEALTH_FACILITY\".\"ID\" IN (SELECT DISTINCT A.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = "+hfid+"\nUNION\nSELECT DISTINCT B.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = "+hfid+"\nUNION\nSELECT DISTINCT C.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = "+hfid+"\nUNION\nSELECT DISTINCT D.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = "+hfid+")  ) AND \"CHILD\".\"MODIFIED_ON\" >= '"+fromDate+"' AND \"CHILD\".\"MODIFIED_ON\" < '"+toDate+"'  AND(\"BIRTHPLACE\".\"NAME\" = 'Home' OR \"BIRTHPLACE\".\"NAME\" = 'Health Facility') \n" +
								"      GROUP BY \"HEALTH_FACILITY\".\"NAME\",\"BIRTHPLACE\".\"NAME\" order by \"HEALTH_FACILITY\".\"NAME\",\"BIRTHPLACE\".\"NAME\"  \n" +
								"            \n" +
								"    )  as t1 $$) as final_result(\"NAME\" text, \"REGISTERED_FACILITY\" bigint, \"REGISTERED_HOME\" bigint ) \n" +
								"   ) AS T3 ON T1.\"NAME\" = T3.\"NAME\"";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);




				return GetHealthFacilityVaccinationSummaryAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("Child", "GetHealthFacilityVaccinationSummary", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}






		#region Helper Methods       

		public static List<HealthFacilityVaccinationSummary> GetHealthFacilityVaccinationSummaryAsList(DataTable dt)
		{
			
			List<HealthFacilityVaccinationSummary> oList = new List<HealthFacilityVaccinationSummary>();


			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityVaccinationSummary o = new HealthFacilityVaccinationSummary();
					o.facilityName = row["NAME"].ToString();
				    o.registered = Helper.ConvertToInt(row["registered"]);
					o.home = Helper.ConvertToInt(row["REGISTERED_HOME"]);
					o.facility = Helper.ConvertToInt(row["REGISTERED_FACILITY"]);
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

