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
	public partial class HealthFacilityImmunizationSessions
	{

		#region Properties
		public Int32 HealthFacilityId { get; set; }
		public Int32 OutreachPlanned { get; set; }
		public Int32 FixedConducted { get; set; }
		public Int32 OutreachConducted { get; set; }
		public Int32 OutreachCanceled { get; set; }
		public string OtherMajorImmunizationActivities { get; set; }
		public Int32 ReportedMonth { get; set; }
		public Int32 ReportedYear { get; set; }
		public DateTime ModifiedOn { get; set; }
		public Int32 ModifiedBy { get; set; }
		#endregion

		#region GetData
		public static List<HealthFacilityImmunizationSessions> GetHealthFacilityImmunizationSessionsList()
		{
			try
			{
				string query = @"SELECT * FROM ""HEALTH_FACILITY_IMMUNIZATION_SESSIONS_AND_ACTIVITIES"";";
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
				return GetHealthFacilityImmunizationSessionsAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityImmunizationSessions", "GetHealthFacilityImmunizationSessionsList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static List<HealthFacilityImmunizationSessions> GetHealthFacilityImmunizationSessions(int healthFacilityId, int reportedMonth, int reportingYear)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_IMMUNIZATION_SESSIONS_AND_ACTIVITIES"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId AND ""REPORTED_MONTH"" = @reportedMonth AND ""REPORTED_YEAR"" = @reportedYear ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId },
					new NpgsqlParameter("@reportedMonth", DbType.Int32)  { Value = reportedMonth },
					new NpgsqlParameter("@reportedYear", DbType.Int32)  { Value = reportingYear }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilityImmunizationSessionsAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityImmunizationSessions", "GetHealthFacilityImmunizationSessions", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		public static List<HealthFacilityImmunizationSessions> GetHealthFacilityImmunizationSessionsAsList(int healthFacilityId)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_IMMUNIZATION_SESSIONS_AND_ACTIVITIES"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilityImmunizationSessionsAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityImmunizationSessions", "GetHealthFacilityImmunizationSessions", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		#endregion

		#region CRUD
		public static int Insert(HealthFacilityImmunizationSessions vaccinations)
		{
			try
			{
				string query = @"INSERT INTO ""HEALTH_FACILITY_IMMUNIZATION_SESSIONS_AND_ACTIVITIES"" 
				(""HEALTH_FACILITY_ID"",""OUTREACH_PLANNED"" ,""OTHER_MAJOR_IMMUNIZATION_ACTIVITIES"",
				""REPORTED_MONTH"",""REPORTED_YEAR"", ""MODIFIED_ON"",""MODIFIED_BY"") VALUES (@HealthFacilityId, @OutreachPlanned, @OtherMajorImmunizationActivities, @ReportingMonth,@ReportingYear, @ModifiedOn, @ModifiedBy)";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = vaccinations.HealthFacilityId },
					new NpgsqlParameter("@OutreachPlanned", DbType.Int32)  { Value = vaccinations.OutreachPlanned },
					new NpgsqlParameter("@OtherMajorImmunizationActivities", DbType.String)  { Value = vaccinations.OtherMajorImmunizationActivities},
					new NpgsqlParameter("@ReportingMonth", DbType.Int32)  { Value = vaccinations.ReportedMonth },
					new NpgsqlParameter("@ReportingYear", DbType.Int32)  { Value = vaccinations.ReportedYear },
					new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = vaccinations.ModifiedOn },
					new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = vaccinations.ModifiedBy }
				};
				DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilityImmunizationSessions", "1", 1, DateTime.Now, vaccinations.ModifiedBy);
				return 1;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityImmunizationSessions", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
			}
			return -1;
		}

		public static int Update(HealthFacilityImmunizationSessions vaccinations)
		{
			try
			{
				string query = @"UPDATE ""HEALTH_FACILITY_IMMUNIZATION_SESSIONS_AND_ACTIVITIES"" SET  ""OUTREACH_PLANNED"" = @OutreachPlanned, 
				""OTHER_MAJOR_IMMUNIZATION_ACTIVITIES"" = @OtherMajorImmunizationActivities,""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy 
				WHERE ""HEALTH_FACILITY_ID"" = @HealthFacilityId AND ""REPORTED_MONTH"" = @ReportingMonth AND  ""REPORTED_YEAR"" = @ReportingYear";

				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = vaccinations.HealthFacilityId },
					new NpgsqlParameter("@OutreachPlanned", DbType.Int32)  { Value = vaccinations.OutreachPlanned },
					new NpgsqlParameter("@OtherMajorImmunizationActivities", DbType.String)  { Value = vaccinations.OtherMajorImmunizationActivities},
					new NpgsqlParameter("@ReportingMonth", DbType.Int32)  { Value = vaccinations.ReportedMonth },
					new NpgsqlParameter("@ReportingYear", DbType.Int32)  { Value = vaccinations.ReportedYear },
					new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = vaccinations.ModifiedOn },
					new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = vaccinations.ModifiedBy }
				};

				int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilityImmunizationSessions", vaccinations.HealthFacilityId.ToString(), 2, DateTime.Now, vaccinations.ModifiedBy);
				return rowAffected;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityImmunizationSessions", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
			}
			return -1;
		}

		public static int Delete(int healthFacilityId, int reportedMonth, int reportingYear)
		{
			try
			{
				string query = @"DELETE FROM ""HEALTH_FACILITY_IMMUNIZATION_SESSIONS_AND_ACTIVITIES"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId AND ""REPORTED_MONTH"" = @reportedMonth AND ""REPORTED_YEAR"" = @reportedYear";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId },
					new NpgsqlParameter("@reportedMonth", DbType.Int32)  { Value = reportedMonth },
					new NpgsqlParameter("@reportedYear", DbType.Int32)  { Value = reportingYear }
				};
				int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilityImmunizationSessions", healthFacilityId.ToString(), 3, DateTime.Now, 1);
				return rowAffected;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityImmunizationSessions", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		#endregion

		#region Helper Methods
		public static HealthFacilityImmunizationSessions GetHealthFacilityImmunizationSessions(DataTable dt)
		{
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityImmunizationSessions o = new HealthFacilityImmunizationSessions();
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
					o.OtherMajorImmunizationActivities = (row["OTHER_MAJOR_IMMUNIZATION_ACTIVITIES"]).ToString();
					o.OutreachPlanned = Helper.ConvertToInt(row["OUTREACH_PLANNED"]);
					o.ReportedMonth = Helper.ConvertToInt(row["REPORTED_MONTH"]);
					o.ReportedYear = Helper.ConvertToInt(row["REPORTED_YEAR"]);
					o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
					o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
					return o;
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityImmunizationSessions", "GetHealthFacilityImmunizationSessions", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return null;
		}

		public static List<HealthFacilityImmunizationSessions> GetHealthFacilityImmunizationSessionsAsList(DataTable dt)
		{
			List<HealthFacilityImmunizationSessions> oList = new List<HealthFacilityImmunizationSessions>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityImmunizationSessions o = new HealthFacilityImmunizationSessions();
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
					o.OtherMajorImmunizationActivities = (row["OTHER_MAJOR_IMMUNIZATION_ACTIVITIES"]).ToString();
					o.OutreachPlanned = Helper.ConvertToInt(row["OUTREACH_PLANNED"]);
					o.ReportedMonth = Helper.ConvertToInt(row["REPORTED_MONTH"]);
					o.ReportedYear = Helper.ConvertToInt(row["REPORTED_YEAR"]);
					o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
					o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityImmunizationSessions", "GetHealthFacilityImmunizationSessionsAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return oList;
		}
		#endregion

	}
}
