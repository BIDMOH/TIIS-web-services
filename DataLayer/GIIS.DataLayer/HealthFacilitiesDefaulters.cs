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
    public partial class HealthFacilityDefaulters
    {

        #region Properties
        public Int32 childId { get; set; }
		public string childBarcodeId { get; set; }
		public string childName { get; set; }
		public string gudianName { get; set; }
        public string gudianContact { get; set; }
		public string village { get; set; }
		public string MissedVaccines { get; set; }
		public string HealthFacility { get; set; }
		public Int32 January { get; set; }
		public Int32 February { get; set; }
		public Int32 March { get; set; }
		public Int32 April { get; set; }
		public Int32 May { get; set; }
		public Int32 June { get; set; }
		public Int32 July { get; set; }
		public Int32 August { get; set; }
		public Int32 September { get; set; }
		public Int32 October { get; set; }
		public Int32 November { get; set; }
		public Int32 December { get; set; }


		#endregion

		#region GetData
		public static List<HealthFacilityDefaulters> GetHealthFacilityDefaultersList(string hfid, DateTime fromDate, DateTime toDate)
        {
			if (hfid.Equals(""))
			{
				return new List<HealthFacilityDefaulters>();
			}
			try
			{
				string query = @"SELECT ""CHILD"".""ID"",""FIRSTNAME1"", ""FIRSTNAME2"", ""LASTNAME1"", ""LASTNAME2"", 
                             ""HEALTHCENTER_ID"", ""PLACE"".""NAME"" AS village, 
                               ""PHONE"", ""MOBILE"", ""EMAIL"", ""MOTHER_FIRSTNAME"", ""MOTHER_LASTNAME"",""CHILD"".""MODIFIED_ON"",""CHILD"".""MODIFIED_BY"", ""BARCODE_ID"",array_to_string(array_agg(""FULLNAME""), ',') AS MISSED_VACCINES 
                               FROM ""CHILD"" 
									INNER JOIN ""VACCINATION_EVENT"" on ""CHILD"".""ID"" = ""VACCINATION_EVENT"".""CHILD_ID"" 
									LEFT JOIN   ""NONVACCINATION_REASON"" ON ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = ""NONVACCINATION_REASON"".""ID""
									INNER JOIN ""DOSE"" ON ""VACCINATION_EVENT"".""DOSE_ID""=""DOSE"".""ID"" 
	                                inner join  ""PLACE"" on ""CHILD"".""DOMICILE_ID"" = ""PLACE"".""ID""
										WHERE 
										""CHILD"".""STATUS_ID"" = 1 and 
										""HEALTHCENTER_ID"" IN (SELECT DISTINCT A.""ID"" FROM ""HEALTH_FACILITY"" AS A
							LEFT JOIN ""HEALTH_FACILITY"" AS B ON A.""ID"" = B.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS C ON B.""ID"" = C.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS D ON C.""ID"" = D.""PARENT_ID""
							WHERE 
							A.""ID"" = @hfid
							UNION
							SELECT DISTINCT B.""ID"" FROM ""HEALTH_FACILITY"" AS A
							LEFT JOIN ""HEALTH_FACILITY"" AS B ON A.""ID"" = B.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS C ON B.""ID"" = C.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS D ON C.""ID"" = D.""PARENT_ID""
							WHERE 
							A.""ID"" = @hfid
							UNION
							SELECT DISTINCT C.""ID"" FROM ""HEALTH_FACILITY"" AS A
							LEFT JOIN ""HEALTH_FACILITY"" AS B ON A.""ID"" = B.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS C ON B.""ID"" = C.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS D ON C.""ID"" = D.""PARENT_ID""
							WHERE 
							A.""ID"" = @hfid
							UNION
							SELECT DISTINCT D.""ID"" FROM ""HEALTH_FACILITY"" AS A
							LEFT JOIN ""HEALTH_FACILITY"" AS B ON A.""ID"" = B.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS C ON B.""ID"" = C.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS D ON C.""ID"" = D.""PARENT_ID""
							WHERE 
							A.""ID"" = @hfid) AND 
										""VACCINATION_EVENT"".""SCHEDULED_DATE"" >= @fromDate  AND ""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= @toDate and 
										""VACCINATION_STATUS"" = false AND ""VACCINATION_EVENT"".""IS_ACTIVE"" = true 
										GROUP BY ""CHILD"".""ID"",""PLACE"".""NAME""   ORDER BY ""CHILD"".""ID"",""PLACE"".""NAME""";
				;
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

			
				return GetHealthFacilityDefaultersAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

		public static List<HealthFacilityDefaulters> GetHealthFacilityDefaultersListByVillage(string hfid, string village, DateTime fromDate, DateTime toDate)
		{
			if (hfid.Equals(""))
			{
				return new List<HealthFacilityDefaulters>();
			}
			try
			{
				string query = @"SELECT ""CHILD"".""ID"",""FIRSTNAME1"", ""FIRSTNAME2"", ""LASTNAME1"", ""LASTNAME2"", 
                             ""HEALTHCENTER_ID"", ""PLACE"".""NAME"" AS village, 
                               ""PHONE"", ""MOBILE"", ""EMAIL"", ""MOTHER_FIRSTNAME"", ""MOTHER_LASTNAME"",""CHILD"".""MODIFIED_ON"",""CHILD"".""MODIFIED_BY"", ""BARCODE_ID"",array_to_string(array_agg(""FULLNAME""), ',') AS MISSED_VACCINES 
                               FROM ""CHILD"" 
									INNER JOIN ""VACCINATION_EVENT"" on ""CHILD"".""ID"" = ""VACCINATION_EVENT"".""CHILD_ID"" 
									LEFT JOIN   ""NONVACCINATION_REASON"" ON ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = ""NONVACCINATION_REASON"".""ID""
									INNER JOIN ""DOSE"" ON ""VACCINATION_EVENT"".""DOSE_ID""=""DOSE"".""ID"" 
	                                inner join  ""PLACE"" on ""CHILD"".""DOMICILE_ID"" = ""PLACE"".""ID""
										WHERE 
										""CHILD"".""STATUS_ID"" = 1 and 
										""HEALTHCENTER_ID"" IN (SELECT DISTINCT A.""ID"" FROM ""HEALTH_FACILITY"" AS A
							LEFT JOIN ""HEALTH_FACILITY"" AS B ON A.""ID"" = B.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS C ON B.""ID"" = C.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS D ON C.""ID"" = D.""PARENT_ID""
							WHERE 
							A.""ID"" = @hfid
							UNION
							SELECT DISTINCT B.""ID"" FROM ""HEALTH_FACILITY"" AS A
							LEFT JOIN ""HEALTH_FACILITY"" AS B ON A.""ID"" = B.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS C ON B.""ID"" = C.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS D ON C.""ID"" = D.""PARENT_ID""
							WHERE 
							A.""ID"" = @hfid
							UNION
							SELECT DISTINCT C.""ID"" FROM ""HEALTH_FACILITY"" AS A
							LEFT JOIN ""HEALTH_FACILITY"" AS B ON A.""ID"" = B.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS C ON B.""ID"" = C.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS D ON C.""ID"" = D.""PARENT_ID""
							WHERE 
							A.""ID"" = @hfid
							UNION
							SELECT DISTINCT D.""ID"" FROM ""HEALTH_FACILITY"" AS A
							LEFT JOIN ""HEALTH_FACILITY"" AS B ON A.""ID"" = B.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS C ON B.""ID"" = C.""PARENT_ID""
							LEFT JOIN ""HEALTH_FACILITY"" AS D ON C.""ID"" = D.""PARENT_ID""
							WHERE 
							A.""ID"" = @hfid) AND ""PLACE"".""NAME"" = @village AND
										""VACCINATION_EVENT"".""SCHEDULED_DATE"" >= @fromDate  AND ""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= @toDate and 
										""VACCINATION_STATUS"" = false AND ""VACCINATION_EVENT"".""IS_ACTIVE"" = true 
										GROUP BY ""CHILD"".""ID"",""PLACE"".""NAME""   ORDER BY ""CHILD"".""ID"",""PLACE"".""NAME""";
				;
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@village", DbType.String) { Value = village },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);


				return GetHealthFacilityDefaultersAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacility", "GetHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}
		public static List<HealthFacilityDefaulters> GetHealthFacilityDefaultersByDistrictListAndDose(int districtCouncilId, string doseName, DateTime fromDate, DateTime toDate)
		{
			if (districtCouncilId.Equals(""))
			{
				return new List<HealthFacilityDefaulters>();
			}
			try
			{
				string query = @"SELECT * "+
                               "FROM   crosstab($$ SELECT t1.\"NAME\",t1.Month,t1.defaulter FROM (SELECT T2.Month,T2.\"NAME\", COUNT(T2.\"ID\") as DEFAULTER FROM" +
							   " (SELECT \"CHILD\".\"ID\",EXTRACT(MONTH FROM \"SCHEDULED_DATE\") AS Month,\"HEALTH_FACILITY\".\"NAME\",\"CHILD\".\"MODIFIED_BY\", \"BARCODE_ID\" "+
							   " FROM \"CHILD\" " +
							   "inner join \"VACCINATION_EVENT\" on \"CHILD\".\"ID\" = \"VACCINATION_EVENT\".\"CHILD_ID\" LEFT JOIN \"NONVACCINATION_REASON\" ON \"VACCINATION_EVENT\".\"NONVACCINATION_REASON_ID\" = \"NONVACCINATION_REASON\".\"ID\" " +
							   "inner join \"DOSE\" on \"VACCINATION_EVENT\".\"DOSE_ID\" = \"DOSE\".\"ID\" " +
							   "inner join \"HEALTH_FACILITY\" ON \"CHILD\".\"HEALTHCENTER_ID\" = \"HEALTH_FACILITY\".\"ID\" " +
							   "WHERE \"CHILD\".\"STATUS_ID\" = 1 and (\"HEALTH_FACILITY\".\"ID\" = " + districtCouncilId + " OR \"HEALTH_FACILITY\".\"PARENT_ID\" = " + districtCouncilId + ") and \"FULLNAME\" = "+doseName+" and \"VACCINATION_EVENT\".\"SCHEDULED_DATE\" <= NOW() AND\"VACCINATION_EVENT\".\"SCHEDULED_DATE\" >= '" + fromDate.ToString() + "'  AND \"VACCINATION_EVENT\".\"SCHEDULED_DATE\" <= '"+toDate.ToString() + "' and \"VACCINATION_STATUS\" = false GROUP BY \"CHILD\".\"ID\",\"SCHEDULED_DATE\",\"HEALTH_FACILITY\".\"ID\",\"CHILD\".\"MODIFIED_BY\", \"BARCODE_ID\"  ) AS T2 GROUP BY T2.Month ,T2.\"NAME\" ORDER BY  \"NAME\",Month ) AS t1  $$) " +
							   " AS   final_result(\"NAME\" text, \"JAN\" bigint,\"FEB\" bigint,\"MAR\" bigint,\"APR\" bigint, \"MAY\" bigint,\"JUN\" bigint , \"JUL\" bigint , \"AUG\" bigint , \"SEP\" bigint, \"OCT\" bigint , \"NOV\" bigint, \"DEC\" bigint ) ";
				
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@districtCouncilId", DbType.Int32) { Value = districtCouncilId },
					new NpgsqlParameter("@doseName", DbType.String) { Value = doseName },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

				DateTime n = DateTime.Now;
				

				return GetHealthFacilityDefaultersByDistrictAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacility", "GetHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static List<HealthFacilityDefaulters> GetHealthFacilityDefaultersByDistrictList(string districtCouncilId, DateTime fromDate, DateTime toDate)
		{
			try
			{
				string query = @"SELECT * " +
							   "FROM   crosstab($$ SELECT t1.\"NAME\",t1.Month,t1.defaulter FROM (SELECT T2.Month,T2.\"NAME\", COUNT(T2.\"ID\") as DEFAULTER FROM" +
							   " (SELECT \"CHILD\".\"ID\",EXTRACT(MONTH FROM \"SCHEDULED_DATE\") AS Month,\"HEALTH_FACILITY\".\"NAME\",\"CHILD\".\"MODIFIED_BY\", \"BARCODE_ID\" " +
							   " FROM \"CHILD\" " +
							   "inner join \"VACCINATION_EVENT\" on \"CHILD\".\"ID\" = \"VACCINATION_EVENT\".\"CHILD_ID\" LEFT JOIN \"NONVACCINATION_REASON\" ON \"VACCINATION_EVENT\".\"NONVACCINATION_REASON_ID\" = \"NONVACCINATION_REASON\".\"ID\" " +

							   "inner join \"HEALTH_FACILITY\" ON \"CHILD\".\"HEALTHCENTER_ID\" = \"HEALTH_FACILITY\".\"ID\" " +
							   "WHERE \"CHILD\".\"STATUS_ID\" = 1 and (\"HEALTH_FACILITY\".\"ID\" = " + districtCouncilId + " OR \"HEALTH_FACILITY\".\"PARENT_ID\" = " + districtCouncilId + ") and \"VACCINATION_EVENT\".\"SCHEDULED_DATE\" <= NOW() AND\"VACCINATION_EVENT\".\"SCHEDULED_DATE\" >= '" + fromDate.ToString() + "'  AND \"VACCINATION_EVENT\".\"SCHEDULED_DATE\" <= '" + toDate.ToString() + "' and \"VACCINATION_STATUS\" = false GROUP BY \"CHILD\".\"ID\",\"SCHEDULED_DATE\",\"HEALTH_FACILITY\".\"ID\",\"CHILD\".\"MODIFIED_BY\", \"BARCODE_ID\"  ) AS T2 GROUP BY T2.Month ,T2.\"NAME\" ORDER BY  \"NAME\",Month ) AS t1  $$) " +
							   " AS   final_result(\"NAME\" text, \"JAN\" bigint,\"FEB\" bigint,\"MAR\" bigint,\"APR\" bigint, \"MAY\" bigint,\"JUN\" bigint , \"JUL\" bigint , \"AUG\" bigint , \"SEP\" bigint, \"OCT\" bigint , \"NOV\" bigint, \"DEC\" bigint ) ";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = districtCouncilId },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);


				return GetHealthFacilityDefaultersByDistrictAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacility", "GetHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		#endregion

		

        #region Helper Methods       

		public static List<HealthFacilityDefaulters> GetHealthFacilityDefaultersAsList(DataTable dt)
        {
            List<HealthFacilityDefaulters> oList = new List<HealthFacilityDefaulters>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacilityDefaulters o = new HealthFacilityDefaulters();
					try
					{
						o.childId = Helper.ConvertToInt(row["ID"]);
					}
					catch
					{
						o.childId = 0;
					}
					o.childBarcodeId = row["BARCODE_ID"].ToString();
					o.childName = row["FIRSTNAME1"].ToString()+  " " + row["LASTNAME1"].ToString(); 
					o.gudianName = row["MOTHER_FIRSTNAME"].ToString() + " " + row["MOTHER_LASTNAME"].ToString();
					o.gudianContact = row["PHONE"].ToString()+" "+ row["MOBILE"].ToString()+" "+ row["EMAIL"].ToString();   
					o.village = row["village"].ToString();
					o.MissedVaccines = row["MISSED_VACCINES"].ToString();
					oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacilityDefaulters", "HealthFacilityDefaultersAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }

		public static List<HealthFacilityDefaulters> GetHealthFacilityDefaultersByDistrictAsList(DataTable dt)
		{
			List<HealthFacilityDefaulters> oList = new List<HealthFacilityDefaulters>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityDefaulters o = new HealthFacilityDefaulters();

					o.HealthFacility = row["NAME"].ToString();

					if (!row["JAN"].ToString().Equals("")) {
						o.January = Helper.ConvertToInt(row["JAN"]);
					}

					if (!row["FEB"].ToString().Equals(""))
					{
						o.February = Helper.ConvertToInt(row["FEB"]);
					}
					 if (!row["MAR"].ToString().Equals(""))
					{
						o.March = Helper.ConvertToInt(row["MAR"]);
					}
					if (!row["APR"].ToString().Equals(""))
					{
						o.April = Helper.ConvertToInt(row["APR"]);
					}
					if (!row["MAY"].ToString().Equals(""))
					{
						o.May = Helper.ConvertToInt(row["MAY"]);
					}
					if (!row["JUN"].ToString().Equals(""))
					{
						o.June = Helper.ConvertToInt(row["JUN"]);
					}
					 if (!row["JUL"].ToString().Equals(""))
					{
						o.July = Helper.ConvertToInt(row["JUL"]);
					}
					if (!row["AUG"].ToString().Equals(""))
					{
						o.August = Helper.ConvertToInt(row["AUG"]);
					}
					if (!row["SEP"].ToString().Equals(""))
					{
						o.September = Helper.ConvertToInt(row["SEP"]);
					}
					 if (!row["OCT"].ToString().Equals(""))
					{
						o.October = Helper.ConvertToInt(row["OCT"]);
					}
					if (!row["NOV"].ToString().Equals(""))
					{
						o.November = Helper.ConvertToInt(row["NOV"]);
					}
					 if (!row["DEC"].ToString().Equals(""))
					{
						o.December = Helper.ConvertToInt(row["DEC"]);
					}

					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityDefaulters", "HealthFacilityDefaultersByDistrictAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return oList;
		}



		


        #endregion

    }
}

