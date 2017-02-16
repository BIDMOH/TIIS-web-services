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
    public partial class HealthFacilitySessions
    {

        #region Properties
        public Int32 Id { get; set; }
        public Int32 UserId { get; set; }
        public Int32 HealthFacilityId { get; set; }
		public DateTime LoginTime { get; set; }
        public Int32 SessionLength { get; set; }
		public string UserName { get; set;}

        #endregion

        #region GetData
        public static List<HealthFacilitySessions> GetHealthFacilitySessionsList()
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITIES_SESSIONS"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetHealthFacilitySessionsAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

		public static List<HealthFacilitySessions> GetHealthFacilitySessionsByHealthFacilityId(string hfid)
		{

			try
			{
				string query = @"SELECT * FROM ""HEALTH_FACILITIES_SESSIONS"" WHERE ""HEALTH_FACILITY_ID"" = @hfid ";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilitySessionsAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilitySessionsByHealthFacilityId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}

		public static List<HealthFacilitySessions> GetHealthFacilitySessionsLastLoginsByHealthFacilityId(string hfid,DateTime fromDate, DateTime toDate)
		{
			toDate = toDate.AddDays(1);
			try
			{
				string query = @"SELECT DISTINCT ON (""USER_ID"") ""USER_ID"",""LOGIN_TIME"",""HEALTH_FACILITY_ID"",""SESSION_LENGTH"" FROM ""HEALTH_FACILITIES_SESSIONS"" 
						WHERE ""HEALTH_FACILITY_ID"" = @hfid
							ORDER BY ""USER_ID"",""LOGIN_TIME"" DESC ";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilitySessionsAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilitySessionsByHealthFacilityId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}


		public static List<HealthFacilitySessions> GetHealthFacilitySessionsLastLoginsByHealthFacilityIdAndUserId(string hfid,string userId, DateTime fromDate, DateTime toDate)
		{
			toDate = toDate.AddDays(1);
			try
			{
				string query = @"SELECT DISTINCT ON (""USER_ID"") ""USER_ID"",""LOGIN_TIME"",""HEALTH_FACILITY_ID"",""SESSION_LENGTH"" FROM ""HEALTH_FACILITIES_SESSIONS"" 
						WHERE 
							""HEALTH_FACILITY_ID"" = @hfid AND 
							""USER_ID"" = @userId
							ORDER BY ""USER_ID"",""LOGIN_TIME"" DESC ";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@userId", DbType.Int32) { Value = userId },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilitySessionsAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilitySessionsByHealthFacilityId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}

		public static List<HealthFacilitySessions> GetHealthFacilityChildRegistrationsByHealthFacilityIdAndUserId(string hfid, string userId, DateTime fromDate, DateTime toDate)
		{

			toDate = toDate.AddDays(1);
			try
			{
				string query = @"SELECT ""USER_ID"",""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"", date_trunc('day', ""LOGIN_TIME"") AS LOGIN_TIME, COUNT ( DISTINCT ""CHILD"".""ID"") AS SESSION_LENGTH
									FROM ""HEALTH_FACILITIES_SESSIONS"" 
											LEFT JOIN ""HEALTH_FACILITY"" ON ""HEALTH_FACILITY"".""ID""=""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID""
											INNER JOIN ""CHILD"" ON ""CHILD"".""MODIFIED_BY"" = ""HEALTH_FACILITIES_SESSIONS"".""USER_ID""
									WHERE 
										""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"" = @hfid AND
										""USER_ID"" = @userId AND
										""LOGIN_TIME"" >= @fromDate AND ""LOGIN_TIME""< @toDate AND
										date_trunc('day', ""CHILD"".""MODIFIED_ON"") = date_trunc('day', ""LOGIN_TIME"")
									GROUP BY ""USER_ID"",""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"",LOGIN_TIME 
									ORDER BY 3 DESC;";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@userId", DbType.Int32) { Value = userId },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

				return GetHealthFacilitySessionsAsList(dt);

			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilityChildRegistrationsByHealthFacilityIdAndUserId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}

		public static List<HealthFacilitySessions> GetHealthFacilityChildRegistrationsByHealthFacilityId(string hfid, DateTime fromDate, DateTime toDate)
		{
			toDate = toDate.AddDays(1);
			try
			{
				string query = @"SELECT ""USER_ID"",""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"", date_trunc('day', ""LOGIN_TIME"") AS LOGIN_TIME, COUNT ( DISTINCT ""CHILD"".""ID"") AS SESSION_LENGTH
									FROM ""HEALTH_FACILITIES_SESSIONS"" 
											LEFT JOIN ""HEALTH_FACILITY"" ON ""HEALTH_FACILITY"".""ID""=""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID""
											INNER JOIN ""CHILD"" ON ""CHILD"".""MODIFIED_BY"" = ""HEALTH_FACILITIES_SESSIONS"".""USER_ID""
									WHERE 
										""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"" = @hfid AND
										""LOGIN_TIME"" >= @fromDate AND ""LOGIN_TIME""< @toDate AND
										date_trunc('day', ""CHILD"".""MODIFIED_ON"") = date_trunc('day', ""LOGIN_TIME"")
									GROUP BY ""USER_ID"",""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"",LOGIN_TIME 
									ORDER BY 3 DESC;";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

				 return GetHealthFacilitySessionsAsList(dt);

			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilityChildRegistrationsByHealthFacilityId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}


		public static List<HealthFacilitySessions> GetHealthFacilityChildVaccinationsByHealthFacilityIdAndUserId(string hfid, string userId, DateTime fromDate, DateTime toDate)
		{

			toDate = toDate.AddDays(1);
			try
			{
				string query = @"SELECT ""USER_ID"",""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"", date_trunc('day', ""LOGIN_TIME"") AS LOGIN_TIME, COUNT ( DISTINCT ""VACCINATION_EVENT"".""APPOINTMENT_ID"") AS SESSION_LENGTH
									FROM ""HEALTH_FACILITIES_SESSIONS"" 
											LEFT JOIN ""HEALTH_FACILITY"" ON ""HEALTH_FACILITY"".""ID""=""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID""
											INNER JOIN ""VACCINATION_EVENT"" ON ""VACCINATION_EVENT"".""MODIFIED_BY"" = ""HEALTH_FACILITIES_SESSIONS"".""USER_ID""
									WHERE 
										""USER_ID"" = @userId AND
										""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"" = @hfid AND
										""LOGIN_TIME"" >= @fromDate AND ""LOGIN_TIME""< @toDate AND
										date_trunc('day', ""VACCINATION_EVENT"".""VACCINATION_DATE"") = date_trunc('day', ""LOGIN_TIME"") AND
										""VACCINATION_EVENT"".""VACCINATION_STATUS"" = @status
									GROUP BY ""USER_ID"",""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"",LOGIN_TIME 
									ORDER BY 3 DESC;";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@userId", DbType.Int32) { Value = userId },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate },
					new NpgsqlParameter("@status", DbType.Boolean) { Value = true }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

				return GetHealthFacilitySessionsAsList(dt);

			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilityChildRegistrationsByHealthFacilityIdAndUserId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}

		public static List<HealthFacilityLoginSessionsRatings> GetHealthFacilityChildrenVaccinationsByDistrict(string districtCouncilId, DateTime fromDate, DateTime toDate)
		{
			toDate = toDate.AddDays(1);
			try
			{
				string query = @"SELECT ""HEALTH_FACILITY_ID"",""NAME"", SUM(SESSION_LENGTH) AS C,DaysMaximum,DaysMinimum,ChildrenVaccinationsMaximumThreshold,ChildrenVaccinationsMinimumThreshold,ChildrenRegistrationsMaximumThreshold,ChildrenRegistrationsMinimumThreshold
        						FROM

							        (SELECT ""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"",""NAME"", date_trunc('day', ""LOGIN_TIME"") AS LOGIN_TIME, COUNT ( DISTINCT ""VACCINATION_EVENT"".""APPOINTMENT_ID"") AS SESSION_LENGTH,DaysMaximum,DaysMinimum,ChildrenVaccinationsMaximumThreshold,ChildrenVaccinationsMinimumThreshold,ChildrenRegistrationsMaximumThreshold,ChildrenRegistrationsMinimumThreshold
								        FROM ""HEALTH_FACILITIES_SESSIONS""
								        LEFT JOIN ""HEALTH_FACILITY"" ON ""HEALTH_FACILITY"".""ID""=""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID""
								        INNER JOIN ""VACCINATION_EVENT"" ON ""VACCINATION_EVENT"".""MODIFIED_BY"" = ""HEALTH_FACILITIES_SESSIONS"".""USER_ID""
								        LEFT JOIN (
											SELECT ""HEALTH_FACILITY_ID"",
											SUM(CASE ""NAME"" WHEN 'DaysMaximum' THEN ""VALUE"" ELSE 0 END) AS DaysMaximum,
											SUM(CASE ""NAME"" WHEN 'DaysMinimum' THEN ""VALUE"" ELSE 0 END) AS DaysMinimum,
											SUM(CASE ""NAME"" WHEN 'ChildrenVaccinationsMaximumThreshold' THEN ""VALUE"" ELSE 0 END) AS ChildrenVaccinationsMaximumThreshold,
											SUM(CASE ""NAME"" WHEN 'ChildrenVaccinationsMinimumThreshold' THEN ""VALUE"" ELSE 0 END) AS ChildrenVaccinationsMinimumThreshold,
											SUM(CASE ""NAME"" WHEN 'ChildrenRegistrationsMaximumThreshold' THEN ""VALUE"" ELSE 0 END) AS ChildrenRegistrationsMaximumThreshold,
											SUM(CASE ""NAME"" WHEN 'ChildrenRegistrationsMinimumThreshold' THEN ""VALUE"" ELSE 0 END) AS ChildrenRegistrationsMinimumThreshold
											FROM ""CONFIGURATION_REPORTS""
											GROUP BY ""HEALTH_FACILITY_ID""
										) AS T2 ON T2.""HEALTH_FACILITY_ID"" = ""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID""

								        WHERE
									        ""PARENT_ID"" = @districtCouncilId AND
									        ""LOGIN_TIME"" >= @fromDate AND ""LOGIN_TIME""< @toDate AND
											date_trunc('day', ""VACCINATION_EVENT"".""VACCINATION_DATE"") = date_trunc('day', ""LOGIN_TIME"") AND
									        ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = @status
										GROUP BY ""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"",""NAME"",LOGIN_TIME,DaysMaximum,DaysMinimum,ChildrenVaccinationsMaximumThreshold,ChildrenVaccinationsMinimumThreshold,ChildrenRegistrationsMaximumThreshold,ChildrenRegistrationsMinimumThreshold
									) AS T1
						        GROUP BY ""HEALTH_FACILITY_ID"",""NAME"",DaysMaximum,DaysMinimum,ChildrenVaccinationsMaximumThreshold,ChildrenVaccinationsMinimumThreshold,ChildrenRegistrationsMaximumThreshold,ChildrenRegistrationsMinimumThreshold
						        ORDER BY 3 DESC;";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@districtCouncilId", DbType.Int32) { Value = Helper.ConvertToInt(districtCouncilId) },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate },
					new NpgsqlParameter("@status", DbType.Boolean) { Value = true }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

				return GetHealthFacilityLoginSessionRatingsAsList(dt);

			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilityChildrenVaccinationsByDistrict", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static List<HealthFacilitySessions> GetHealthFacilityChildVaccinationsByHealthFacilityId(string hfid, DateTime fromDate, DateTime toDate)
		{
			toDate = toDate.AddDays(1);
			try
			{
				string query = @"SELECT ""USER_ID"",""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"", date_trunc('day', ""LOGIN_TIME"") AS LOGIN_TIME, COUNT ( DISTINCT ""VACCINATION_EVENT"".""APPOINTMENT_ID"") AS SESSION_LENGTH
									FROM ""HEALTH_FACILITIES_SESSIONS"" 
											LEFT JOIN ""HEALTH_FACILITY"" ON ""HEALTH_FACILITY"".""ID""=""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID""
											INNER JOIN ""VACCINATION_EVENT"" ON ""VACCINATION_EVENT"".""MODIFIED_BY"" = ""HEALTH_FACILITIES_SESSIONS"".""USER_ID""
									WHERE 
										""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"" = @hfid AND
										""LOGIN_TIME"" >= @fromDate AND ""LOGIN_TIME""< @toDate AND
										date_trunc('day', ""VACCINATION_EVENT"".""VACCINATION_DATE"") = date_trunc('day', ""LOGIN_TIME"") AND
										""VACCINATION_EVENT"".""VACCINATION_STATUS"" = @status
									GROUP BY ""USER_ID"",""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"",LOGIN_TIME 
									ORDER BY 3 DESC;";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate },
					new NpgsqlParameter("@status", DbType.Boolean) { Value = true }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

				return GetHealthFacilitySessionsAsList(dt);

			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilityChildRegistrationsByHealthFacilityId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}





		public static int GetHealthFacilitySessionsDaysByHealthFacilityIdAndUserId(string hfid, string userId, DateTime fromDate, DateTime toDate)
		{
			try
			{
				string query = @"SELECT ""USER_ID"",""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"", date_trunc('day', ""LOGIN_TIME"") AS LOGIN_TIME, COUNT ( DISTINCT ""VACCINATION_EVENT"".""CHILD_ID"") AS SESSION_LENGTH
									FROM ""HEALTH_FACILITIES_SESSIONS"" 
											LEFT JOIN ""HEALTH_FACILITY"" ON ""HEALTH_FACILITY"".""ID""=""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID""
											INNER JOIN ""VACCINATION_EVENT"" ON ""VACCINATION_EVENT"".""MODIFIED_BY"" = ""HEALTH_FACILITIES_SESSIONS"".""USER_ID""
									WHERE 
										""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"" = @hfid AND
										""USER_ID"" = @userId  AND
										""LOGIN_TIME"" >= @fromDate AND ""LOGIN_TIME""< @toDate AND
										date_trunc('day', ""VACCINATION_EVENT"".""VACCINATION_DATE"") = date_trunc('day', ""LOGIN_TIME"") AND
										""VACCINATION_EVENT"".""VACCINATION_STATUS"" = @status
									GROUP BY ""USER_ID"",""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"",LOGIN_TIME 
									ORDER BY 3 DESC;";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@userId", DbType.Int32) { Value = userId },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate },
					new NpgsqlParameter("@status", DbType.Boolean) { Value = true }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				List<DateTime> dates = new List<DateTime>();

				foreach (DataRow row in dt.Rows)
				{
					dates.Add(Helper.ConvertToDate(row["LOGIN_TIME"]));
				}
				dates = dates.Select(x => x.Date).Distinct().ToList();

				return dates.Count;

			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilitySessionsByHealthFacilityIdAndUserId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		public static int GetHealthFacilitySessionsDaysByHealthFacilityId(string hfid, DateTime fromDate, DateTime toDate)
		{
			try
			{
				string query = @"SELECT ""LOGIN_TIME"" FROM ""HEALTH_FACILITIES_SESSIONS"" WHERE ""HEALTH_FACILITY_ID"" = @hfid AND  ""LOGIN_TIME"" >= @fromDate AND ""LOGIN_TIME""<= @toDate ";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				List<DateTime> dates = new List<DateTime>();

				foreach (DataRow row in dt.Rows)
				{
					dates.Add(Helper.ConvertToDate(row["LOGIN_TIME"]));
				}
				dates = dates.Select(x => x.Date).Distinct().ToList();

				return dates.Count;

			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilitySessionsByHealthFacilityIdAndUserId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		public static List<HealthFacilityLoginSessionsRatings> GetHealthFacilityChildrenRegistrationsByDistrict(string districtCouncilId, DateTime fromDate, DateTime toDate)
		{
			toDate = toDate.AddDays(1);
			try
			{
				string query = @"SELECT ""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"",""NAME"", COUNT( DISTINCT ""CHILD"".""ID"") AS C,DaysMaximum,DaysMinimum,ChildrenVaccinationsMaximumThreshold,ChildrenVaccinationsMinimumThreshold,ChildrenRegistrationsMaximumThreshold,ChildrenRegistrationsMinimumThreshold  
									FROM ""HEALTH_FACILITIES_SESSIONS"" 
											LEFT JOIN ""HEALTH_FACILITY"" ON ""HEALTH_FACILITY"".""ID""=""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID""
											LEFT JOIN ""CHILD"" ON ""CHILD"".""MODIFIED_BY"" = ""HEALTH_FACILITIES_SESSIONS"".""USER_ID""
											LEFT JOIN (
															SELECT ""HEALTH_FACILITY_ID"",
															       SUM(CASE ""NAME"" WHEN 'DaysMaximum' THEN ""VALUE"" ELSE 0 END) AS DaysMaximum,
															       SUM(CASE ""NAME"" WHEN 'DaysMinimum' THEN ""VALUE"" ELSE 0 END) AS DaysMinimum,
															       SUM(CASE ""NAME"" WHEN 'ChildrenVaccinationsMaximumThreshold' THEN ""VALUE"" ELSE 0 END) AS ChildrenVaccinationsMaximumThreshold,
															       SUM(CASE ""NAME"" WHEN 'ChildrenVaccinationsMinimumThreshold' THEN ""VALUE"" ELSE 0 END) AS ChildrenVaccinationsMinimumThreshold,
															       SUM(CASE ""NAME"" WHEN 'ChildrenRegistrationsMaximumThreshold' THEN ""VALUE"" ELSE 0 END) AS ChildrenRegistrationsMaximumThreshold,
															       SUM(CASE ""NAME"" WHEN 'ChildrenRegistrationsMinimumThreshold' THEN ""VALUE"" ELSE 0 END) AS ChildrenRegistrationsMinimumThreshold
															FROM ""CONFIGURATION_REPORTS""
															GROUP BY ""HEALTH_FACILITY_ID""
													) AS T1 ON T1.""HEALTH_FACILITY_ID"" = ""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID""
									WHERE ""PARENT_ID"" = @districtCouncilId AND
										""LOGIN_TIME"" >= @fromDate AND ""LOGIN_TIME""< @toDate AND
										date_trunc('day', ""CHILD"".""MODIFIED_ON"") = date_trunc('day', ""LOGIN_TIME"")
									GROUP BY ""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"",""NAME"",DaysMaximum,DaysMinimum,ChildrenVaccinationsMaximumThreshold,ChildrenVaccinationsMinimumThreshold,ChildrenRegistrationsMaximumThreshold,ChildrenRegistrationsMinimumThreshold 
									ORDER BY 3 DESC;";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@districtCouncilId", DbType.Int32) { Value = districtCouncilId },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

				return GetHealthFacilityLoginSessionRatingsAsList(dt);

			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilityChildrenRegistrationsByDistrict", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		public static List<HealthFacilityLoginSessionsRatings> GetHealthFacilitySessionsDaysRatingsByDistrict (string districtCouncilId, DateTime fromDate, DateTime toDate)
		{
			toDate = toDate.AddDays(1);
			string query = @"SELECT ""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"",""NAME"",DaysMaximum,DaysMinimum,ChildrenVaccinationsMaximumThreshold,ChildrenVaccinationsMinimumThreshold,ChildrenRegistrationsMaximumThreshold,ChildrenRegistrationsMinimumThreshold
        					FROM ""HEALTH_FACILITIES_SESSIONS""
        						LEFT JOIN ""HEALTH_FACILITY"" ON ""HEALTH_FACILITY"".""ID"" = ""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID""
								LEFT JOIN (
									SELECT ""HEALTH_FACILITY_ID"",
									       SUM(CASE ""NAME"" WHEN 'DaysMaximum' THEN ""VALUE"" ELSE 0 END) AS DaysMaximum,
									       SUM(CASE ""NAME"" WHEN 'DaysMinimum' THEN ""VALUE"" ELSE 0 END) AS DaysMinimum,
									       SUM(CASE ""NAME"" WHEN 'ChildrenVaccinationsMaximumThreshold' THEN ""VALUE"" ELSE 0 END) AS ChildrenVaccinationsMaximumThreshold,
									       SUM(CASE ""NAME"" WHEN 'ChildrenVaccinationsMinimumThreshold' THEN ""VALUE"" ELSE 0 END) AS ChildrenVaccinationsMinimumThreshold,
									       SUM(CASE ""NAME"" WHEN 'ChildrenRegistrationsMaximumThreshold' THEN ""VALUE"" ELSE 0 END) AS ChildrenRegistrationsMaximumThreshold,
									       SUM(CASE ""NAME"" WHEN 'ChildrenRegistrationsMinimumThreshold' THEN ""VALUE"" ELSE 0 END) AS ChildrenRegistrationsMinimumThreshold
									FROM ""CONFIGURATION_REPORTS""
									GROUP BY ""HEALTH_FACILITY_ID"") AS T1 ON T1.""HEALTH_FACILITY_ID"" = ""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID""
						        WHERE ""PARENT_ID"" = @districtCouncilId
								GROUP BY ""HEALTH_FACILITIES_SESSIONS"".""HEALTH_FACILITY_ID"",""NAME"",DaysMaximum,DaysMinimum,ChildrenVaccinationsMaximumThreshold,ChildrenVaccinationsMinimumThreshold,ChildrenRegistrationsMaximumThreshold,ChildrenRegistrationsMinimumThreshold";
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
			{
					new NpgsqlParameter("@districtCouncilId", DbType.Int32) { Value = Helper.ConvertToInt(districtCouncilId) }
			};

			DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

			List<HealthFacilityLoginSessionsRatings> oList = new List<HealthFacilityLoginSessionsRatings>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityLoginSessionsRatings o = new HealthFacilityLoginSessionsRatings();
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
					o.Name = (row["NAME"]).ToString();
					try
					{
						o.DaysMaximum = Helper.ConvertToInt(row["DaysMaximum"]);
					}
					catch { }
					try
					{
					o.DaysMinimum = Helper.ConvertToInt(row["DaysMinimum"]);
					}
					catch { }
					try
					{
					o.ChildrenVaccinationsMaximumThreshold = Helper.ConvertToInt(row["ChildrenVaccinationsMaximumThreshold"]);
					}catch { }

					try
					{
					o.ChildrenVaccinationsMinimumThreshold = Helper.ConvertToInt(row["ChildrenVaccinationsMinimumThreshold"]);
					}catch { }

					try
					{
					o.ChildrenRegistrationsMaximumThreshold = Helper.ConvertToInt(row["ChildrenRegistrationsMaximumThreshold"]);
					}catch { }

					try
					{
					o.ChildrenRegistrationsMinimumThreshold = Helper.ConvertToInt(row["ChildrenRegistrationsMinimumThreshold"]);
					}catch { }
					try
					{
						string query2 = @"SELECT ""LOGIN_TIME"" FROM ""HEALTH_FACILITIES_SESSIONS"" WHERE ""HEALTH_FACILITY_ID"" = @hfid AND ""LOGIN_TIME"" >= @fromDate AND ""LOGIN_TIME""<= @toDate ";
						List<NpgsqlParameter> parameters2 = new List<NpgsqlParameter>()
						{
							new NpgsqlParameter("@hfid", DbType.Int32) { Value = o.HealthFacilityId },
							new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
							new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
						};
						DataTable dt2 = DBManager.ExecuteReaderCommand(query2, CommandType.Text, parameters2);
						List<DateTime> dates = new List<DateTime>();
						foreach (DataRow row2 in dt2.Rows)
						{
							dates.Add(Helper.ConvertToDate(row2["LOGIN_TIME"]));
						}

						dates = dates.Select(x => x.Date).Distinct().ToList();
						o.SessionsCount = dates.Count;

					}
					catch (Exception ex)
					{
						Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilitySessionsByHealthFacilityIdAndUserId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
						throw ex;
					}


					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilitySessionsAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}

			List<HealthFacilityLoginSessionsRatings> orderedList = oList.OrderByDescending(o => o.SessionsCount).ToList();
			return orderedList;

		}




        #endregion

        #region CRUD
		public static int Insert(HealthFacilitySessions o)
        {
            try
            {
                string query = @"INSERT INTO ""HEALTH_FACILITIES_SESSIONS"" (""USER_ID"", ""HEALTH_FACILITY_ID"", ""LOGIN_TIME"", ""SESSION_LENGTH"") VALUES (@UserId, @HealthFacilityId, @LoginTime, @SessionLength) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@UserId", DbType.Int32)  { Value = o.UserId },
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = o.HealthFacilityId },
					new NpgsqlParameter("@LoginTime", DbType.DateTime)  { Value = o.LoginTime },
					new NpgsqlParameter("@SessionLength", DbType.Int32)  { Value = o.SessionLength },
				};
	            object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilitySessions", id.ToString(), 1, DateTime.Now, o.UserId);
	            return int.Parse(id.ToString());
			}
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilitySessions", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(HealthFacilitySessions o)
        {
            try
            {
                string query = @"UPDATE ""HEALTH_FACILITIES_SESSIONS"" SET ""USER_ID"" = @UserId, ""HEALTH_FACILITY_ID"" = @HealthFacilityId, ""LOGIN_TIME"" = @LoginTime, ""SESSION_LENGTH"" = @SessionLength";
               
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@UserId", DbType.Int32)  { Value = o.UserId },
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = o.HealthFacilityId },
					new NpgsqlParameter("@LoginTime", DbType.DateTime)  { Value = o.LoginTime },
					new NpgsqlParameter("@SessionLength", DbType.Int32)  { Value = o.SessionLength },
				};

                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilitySessions", o.Id.ToString(), 2, DateTime.Now, o.UserId);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilitySessions", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""HEALTH_FACILITIES_SESSIONS"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
				};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacility", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilitySessions", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static HealthFacilitySessions GetHealthFacilitySessionsAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacilitySessions o = new HealthFacilitySessions();
                    o.Id = Helper.ConvertToInt(row["ID"]);
					o.UserId = Helper.ConvertToInt(row["USER_ID"]);
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
					o.LoginTime = Helper.ConvertToDate(row["LOGIN_TIME"]);
					o.SessionLength = Helper.ConvertToInt(row["SESSION_LENGTH"]);
					o.UserName = User.GetUserById(o.UserId).Username;
                   
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilitySessionsAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

		public static List<HealthFacilitySessions> GetHealthFacilitySessionsAsList(DataTable dt)
        {
            List<HealthFacilitySessions> oList = new List<HealthFacilitySessions>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacilitySessions o = new HealthFacilitySessions();
					try
					{
						o.Id = Helper.ConvertToInt(row["ID"]);
					}
					catch
					{
						o.Id = 0;
					}
					o.UserId = Helper.ConvertToInt(row["USER_ID"]);
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
					o.LoginTime = Helper.ConvertToDate(row["LOGIN_TIME"]);
					o.SessionLength = Helper.ConvertToInt(row["SESSION_LENGTH"]);
					o.UserName = User.GetUserById(o.UserId).Username;
					oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilitySessionsAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }




		public static List<HealthFacilityLoginSessionsRatings> GetHealthFacilityLoginSessionRatingsAsList(DataTable dt)
		{
			List<HealthFacilityLoginSessionsRatings> oList = new List<HealthFacilityLoginSessionsRatings>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityLoginSessionsRatings o = new HealthFacilityLoginSessionsRatings();
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
					o.Name = (row["NAME"]).ToString();
					o.SessionsCount = Helper.ConvertToInt(row["C"]);
					try
					{
						o.DaysMaximum = Helper.ConvertToInt(row["DaysMaximum"]);
					}
					catch { }
					try
					{
						o.DaysMinimum = Helper.ConvertToInt(row["DaysMinimum"]);
					}
					catch { }
					try
					{
						o.ChildrenVaccinationsMaximumThreshold = Helper.ConvertToInt(row["ChildrenVaccinationsMaximumThreshold"]);
					}
					catch { }

					try
					{
						o.ChildrenVaccinationsMinimumThreshold = Helper.ConvertToInt(row["ChildrenVaccinationsMinimumThreshold"]);
					}
					catch { }

					try
					{
						o.ChildrenRegistrationsMaximumThreshold = Helper.ConvertToInt(row["ChildrenRegistrationsMaximumThreshold"]);
					}
					catch { }

					try
					{
						o.ChildrenRegistrationsMinimumThreshold = Helper.ConvertToInt(row["ChildrenRegistrationsMinimumThreshold"]);
					}
					catch { }
					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilitySessions", "GetHealthFacilitySessionsAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return oList;
		}



        #endregion

    }
}



//SELECT "HEALTH_FACILITY_ID",

//	   SUM(CASE "NAME" WHEN 'DaysMaximum' THEN "VALUE" ELSE 0 END) AS DaysMaximum,
//	   SUM(CASE "NAME" WHEN 'DaysMinimum' THEN "VALUE" ELSE 0 END) AS DaysMinimum,
//	   SUM(CASE "NAME" WHEN 'ChildrenVaccinationsMaximumThreshold' THEN "VALUE" ELSE 0 END) AS ChildrenVaccinationsMaximumThreshold,
//	   SUM(CASE "NAME" WHEN 'ChildrenVaccinationsMinimumThreshold' THEN "VALUE" ELSE 0 END) AS ChildrenVaccinationsMinimumThreshold,
//	   SUM(CASE "NAME" WHEN 'ChildrenRegistrationsMaximumThreshold' THEN "VALUE" ELSE 0 END) AS ChildrenRegistrationsMaximumThreshold,
//	   SUM(CASE "NAME" WHEN 'ChildrenRegistrationsMinimumThreshold' THEN "VALUE" ELSE 0 END) AS ChildrenRegistrationsMinimumThreshold
//FROM "CONFIGURATION_REPORTS"
//GROUP BY "HEALTH_FACILITY_ID"