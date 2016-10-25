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
	public partial class HealthFacilityDeseaseSurvailance
	{

		#region Properties
		public Int32 HealthFacilityId { get; set; }
		public Int32 FeverMonthlyCases { get; set; }
		public Int32 FeverMonthlyDeaths { get; set; }
		public Int32 AFPMonthlyCases { get; set; }
		public Int32 AFPDeaths { get; set; }
		public Int32 NeonatalTTCases { get; set; }
		public Int32 NeonatalTTDeaths { get; set; }
		public Int32 ReportedMonth { get; set; }
		public Int32 ReportedYear { get; set; }
		public DateTime ModifiedOn { get; set; }
		public Int32 ModifiedBy { get; set; }
		#endregion

		#region GetData
		public static List<HealthFacilityDeseaseSurvailance> GetHealthFacilityDeseaseSurvailanceList()
		{
			try
			{
				string query = @"SELECT * FROM ""HEALTH_FACILITY_DESEASE_SURVEILLANCE"";";
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
				return GetHealthFacilityDeseaseSurvailanceAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityDeseaseSurvailance", "HealthFacilityDeseaseSurvailance", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static List<HealthFacilityDeseaseSurvailance> GetHealthFacilityDeseaseSurvailance(int healthFacilityId, int reportedMonth, int reportingYear)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_DESEASE_SURVEILLANCE"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId AND ""REPORTED_MONTH"" = @reportedMonth AND ""REPORTED_YEAR"" = @reportedYear ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId },
					new NpgsqlParameter("@reportedMonth", DbType.Int32)  { Value = reportedMonth },
					new NpgsqlParameter("@reportedYear", DbType.Int32)  { Value = reportingYear }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilityDeseaseSurvailanceAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityDeseaseSurvailance", "GetHealthFacilityDeseaseSurvailance", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		public static List<HealthFacilityDeseaseSurvailance> GetHealthFacilityDeseaseSurvailanceList(int healthFacilityId)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_DESEASE_SURVEILLANCE"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilityDeseaseSurvailanceAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityDeseaseSurvailance", "GetHealthFacilityDeseaseSurvailanceList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		#endregion

		#region CRUD
		public static int Insert(HealthFacilityDeseaseSurvailance deseaseSurvailance)
		{
			try
			{
				string query = @"INSERT INTO ""HEALTH_FACILITY_DESEASE_SURVEILLANCE"" (""HEALTH_FACILITY_ID"",""FEVER_MONTHLY_CASES"" ,""FEVER_DEATHS"", ""AFP_MONTHLY_CASES"", ""AFP_DEATHS"",""NEONATAL_TT_CASES"",""NEONATAL_TT_DEATHS"", ""REPORTED_MONTH"",""REPORTED_YEAR"", ""MODIFIED_ON"",""MODIFIED_BY"") 
				VALUES (@HealthFacilityId, @FeverMonthlyCases, @FeverMonthlyDeaths, @AFPMonthlyCases,@AFPDeaths,@NeonatalTTCases,@NeonatalTTDeaths, @ReportingMonth,@ReportingYear, @ModifiedOn, @ModifiedBy)";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = deseaseSurvailance.HealthFacilityId },
					new NpgsqlParameter("@FeverMonthlyCases", DbType.Int32)  { Value = deseaseSurvailance.FeverMonthlyCases },
					new NpgsqlParameter("@FeverMonthlyDeaths", DbType.Int32)  { Value = deseaseSurvailance.FeverMonthlyDeaths},
					new NpgsqlParameter("@AFPMonthlyCases", DbType.Int32)  { Value = (object)deseaseSurvailance.AFPMonthlyCases ?? DBNull.Value },
					new NpgsqlParameter("@AFPDeaths", DbType.Int32)  { Value = (object)deseaseSurvailance.AFPDeaths ?? DBNull.Value  },

					new NpgsqlParameter("@NeonatalTTCases", DbType.Int32)  { Value = (object)deseaseSurvailance.NeonatalTTCases ?? DBNull.Value  },
					new NpgsqlParameter("@NeonatalTTDeaths", DbType.Int32)  { Value = (object)deseaseSurvailance.NeonatalTTDeaths ?? DBNull.Value  },

					new NpgsqlParameter("@ReportingMonth", DbType.Int32)  { Value = deseaseSurvailance.ReportedMonth },
					new NpgsqlParameter("@ReportingYear", DbType.Int32)  { Value = deseaseSurvailance.ReportedYear },
					new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = deseaseSurvailance.ModifiedOn },
					new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = deseaseSurvailance.ModifiedBy }
				};
				DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilityDeseaseSurvailance", "1", 1, DateTime.Now, deseaseSurvailance.ModifiedBy);
				return 1;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityDeseaseSurvailance", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
			}
			return -1;
		}

		public static int Update(HealthFacilityDeseaseSurvailance deseaseSurvailance)
		{
			try
			{
				string query = @"UPDATE ""HEALTH_FACILITY_DESEASE_SURVEILLANCE"" SET  
				""FEVER_MONTHLY_CASES"" = @FeverMonthlyCases, 
				""FEVER_DEATHS"" = @FeverMonthlyDeaths, 
				""AFP_MONTHLY_CASES"" = @AFPMonthlyCases, 
				""AFP_DEATHS"" = @AFPDeaths, 
				""NEONATAL_TT_CASES"" = @NeonatalTTCases, 
				""NEONATAL_TT_DEATHS"" = @NeonatalTTDeaths, 
				""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy 
				WHERE ""HEALTH_FACILITY_ID"" = @HealthFacilityId AND ""REPORTED_MONTH"" = @ReportingMonth AND  ""REPORTED_YEAR"" = @ReportingYear";

				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = deseaseSurvailance.HealthFacilityId },
					new NpgsqlParameter("@FeverMonthlyCases", DbType.Int32)  { Value = deseaseSurvailance.FeverMonthlyCases },
					new NpgsqlParameter("@FeverMonthlyDeaths", DbType.Int32)  { Value = deseaseSurvailance.FeverMonthlyDeaths},
					new NpgsqlParameter("@AFPMonthlyCases", DbType.Int32)  { Value = (object)deseaseSurvailance.AFPMonthlyCases ?? DBNull.Value },
					new NpgsqlParameter("@AFPDeaths", DbType.Int32)  { Value = (object)deseaseSurvailance.AFPDeaths ?? DBNull.Value  },

					new NpgsqlParameter("@NeonatalTTCases", DbType.Int32)  { Value = (object)deseaseSurvailance.NeonatalTTCases ?? DBNull.Value  },
					new NpgsqlParameter("@NeonatalTTDeaths", DbType.Int32)  { Value = (object)deseaseSurvailance.NeonatalTTDeaths ?? DBNull.Value  },

					new NpgsqlParameter("@ReportingMonth", DbType.Int32)  { Value = deseaseSurvailance.ReportedMonth },
					new NpgsqlParameter("@ReportingYear", DbType.Int32)  { Value = deseaseSurvailance.ReportedYear },
					new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = deseaseSurvailance.ModifiedOn },
					new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = deseaseSurvailance.ModifiedBy }
				};

				int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilityDeseaseSurvailance", deseaseSurvailance.HealthFacilityId.ToString(), 2, DateTime.Now, deseaseSurvailance.ModifiedBy);
				return rowAffected;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityDeseaseSurvailance", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
			}
			return -1;
		}

		public static int Delete(int HealthFacilityId)
		{
			try
			{
				string query = @"DELETE FROM ""HEALTH_FACILITY_DESEASE_SURVEILLANCE"" WHERE ""HEALTH_FACILITY_ID"" = @HealthFacilityId";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
				new NpgsqlParameter("@HealthFacilityId", DbType.Int32) { Value = HealthFacilityId }
				};
				int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilityDeseaseSurvailance", HealthFacilityId.ToString(), 3, DateTime.Now, 1);
				return rowAffected;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityDeseaseSurvailance", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		#endregion

		#region Helper Methods
		public static HealthFacilityDeseaseSurvailance GetHealthFacilityColdChain(DataTable dt)
		{
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityDeseaseSurvailance o = new HealthFacilityDeseaseSurvailance();
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
					o.FeverMonthlyCases = Helper.ConvertToInt(row["FEVER_MONTHLY_CASES"]);
					o.FeverMonthlyDeaths = Helper.ConvertToInt(row["FEVER_DEATHS"]);
					o.AFPMonthlyCases = Helper.ConvertToInt(row["AFP_MONTHLY_CASES"]);
					o.AFPDeaths = Helper.ConvertToInt(row["AFP_DEATHS"]);
					o.NeonatalTTCases = Helper.ConvertToInt(row["NEONATAL_TT_CASES"]);
					o.NeonatalTTDeaths = Helper.ConvertToInt(row["NEONATAL_TT_DEATHS"]);
					o.ReportedMonth = Helper.ConvertToInt(row["REPORTED_MONTH"]);
					o.ReportedYear = Helper.ConvertToInt(row["REPORTED_YEAR"]);

					o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
					o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
					return o;
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityDeseaseSurvailance", "HealthFacilityDeseaseSurvailance", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return null;
		}

		public static List<HealthFacilityDeseaseSurvailance> GetHealthFacilityDeseaseSurvailanceAsList(DataTable dt)
		{
			List<HealthFacilityDeseaseSurvailance> oList = new List<HealthFacilityDeseaseSurvailance>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityDeseaseSurvailance o = new HealthFacilityDeseaseSurvailance();
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
					o.FeverMonthlyCases = Helper.ConvertToInt(row["FEVER_MONTHLY_CASES"]);
					o.FeverMonthlyDeaths = Helper.ConvertToInt(row["FEVER_DEATHS"]);
					o.AFPMonthlyCases = Helper.ConvertToInt(row["AFP_MONTHLY_CASES"]);
					o.AFPDeaths = Helper.ConvertToInt(row["AFP_DEATHS"]);
					o.NeonatalTTCases = Helper.ConvertToInt(row["NEONATAL_TT_CASES"]);
					o.NeonatalTTDeaths = Helper.ConvertToInt(row["NEONATAL_TT_DEATHS"]);
					o.ReportedMonth = Helper.ConvertToInt(row["REPORTED_MONTH"]);
					o.ReportedYear = Helper.ConvertToInt(row["REPORTED_YEAR"]);

					o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
					o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityDeseaseSurvailance", "GetHealthFacilityDeseaseSurvailanceAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return oList;
		}
		#endregion

	}
}