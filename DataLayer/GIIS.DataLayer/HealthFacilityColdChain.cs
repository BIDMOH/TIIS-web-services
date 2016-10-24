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
    public partial class HealthFacilityColdChain
    {

        #region Properties
        public Int32 HealthFacilityId { get; set; }
        public Double TempMax { get; set; }
		public Double TempMin { get; set; }
        public Int32 AlarmHighTemp { get; set; }
        public Int32 AlarmLowTemp { get; set; }
		public Int32 ReportedMonth { get; set; }
		public Int32 ReportedYear { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        #endregion

        #region GetData
        public static List<HealthFacilityColdChain> GetHealthFacilityColdChainList()
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_COLD_CHAIN"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
				return GetHealthFacilityColdChainAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityColdChain", "GetHealthFacilityColdChainList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<HealthFacilityColdChain> GetHealthFacilityColdChain(int healthFacilityId, int reportedMonth, int reportingYear)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_COLD_CHAIN"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId AND ""REPORTED_MONTH"" = @reportedMonth AND ""REPORTED_YEAR"" = @reportedYear ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId },
					new NpgsqlParameter("@reportedMonth", DbType.Int32)  { Value = reportedMonth },
					new NpgsqlParameter("@reportedYear", DbType.Int32)  { Value = reportingYear }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilityColdChainAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityColdChain", "GetHealthFacilityColdChain", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		public static List<HealthFacilityColdChain> GetHealthFacilityColdChainAsList(int healthFacilityId)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_COLD_CHAIN"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilityColdChainAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityColdChain", "GetHealthFacilityColdChain", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

        
        #endregion

        #region CRUD
		public static int Insert(HealthFacilityColdChain chain)
        {
            try
            {
                string query = @"INSERT INTO ""HEALTH_FACILITY_COLD_CHAIN"" (""HEALTH_FACILITY_ID"",""TEMP_MAX"" ,""TEMP_MIN"", ""ALARM_HIGH_TEMP"", ""ALARM_LOW_TEMP"", ""REPORTED_MONTH"",""REPORTED_YEAR"", ""MODIFIED_ON"",""MODIFIED_BY"") VALUES (@HealthFacilityId, @TempMax, @TempMin, @AlarmHighTemp,@AlarmLowTemp, @ReportingMonth,@ReportingYear, @ModifiedOn, @ModifiedBy)";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = chain.HealthFacilityId },
					new NpgsqlParameter("@TempMax", DbType.Double)  { Value = chain.TempMax },
					new NpgsqlParameter("@TempMin", DbType.Double)  { Value = chain.TempMin},
					new NpgsqlParameter("@AlarmHighTemp", DbType.Int32)  { Value = (object)chain.AlarmHighTemp ?? DBNull.Value },
					new NpgsqlParameter("@AlarmLowTemp", DbType.Int32)  { Value = (object)chain.AlarmLowTemp ?? DBNull.Value  },
					new NpgsqlParameter("@ReportingMonth", DbType.Int32)  { Value = chain.ReportedMonth },
					new NpgsqlParameter("@ReportingYear", DbType.Int32)  { Value = chain.ReportedYear },
					new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = chain.ModifiedOn },
					new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = chain.ModifiedBy }
				};
                DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityColdChain", "1", 1, DateTime.Now, chain.ModifiedBy);
                return 1;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityColdChain", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

		public static int Update(HealthFacilityColdChain chain)
        {
            try
            {
                string query = @"UPDATE ""HEALTH_FACILITY_COLD_CHAIN"" SET  ""TEMP_MAX"" = @TempMax, ""TEMP_MIN"" = @TempMin, ""ALARM_HIGH_TEMP"" = @AlarmHighTemp, ""ALARM_LOW_TEMP"" = @AlarmLowTemp, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy 
				WHERE ""HEALTH_FACILITY_ID"" = @HealthFacilityId AND ""REPORTED_MONTH"" = @ReportingMonth AND  ""REPORTED_YEAR"" = @ReportingYear";
                
				 List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = chain.HealthFacilityId },
					new NpgsqlParameter("@TempMax", DbType.Double)  { Value = (object)chain.TempMax ?? DBNull.Value },
					new NpgsqlParameter("@TempMin", DbType.Double)  { Value = (object)chain.TempMin ?? DBNull.Value },
					new NpgsqlParameter("@AlarmHighTemp", DbType.Int32)  { Value = (object)chain.AlarmHighTemp ?? DBNull.Value },
					new NpgsqlParameter("@AlarmLowTemp", DbType.Int32)  { Value = (object)chain.AlarmLowTemp ?? DBNull.Value  },
					new NpgsqlParameter("@ReportingMonth", DbType.Int32)  { Value = (object)chain.ReportedMonth ?? DBNull.Value },
					new NpgsqlParameter("@ReportingYear", DbType.Int32)  { Value = (object)chain.ReportedYear ?? DBNull.Value },
					new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = chain.ModifiedOn },
					new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = chain.ModifiedBy }
				};

                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilityColdChain", chain.HealthFacilityId.ToString(), 2, DateTime.Now, chain.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityColdChain", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int HealthFacilityId)
        {
            try
            {
                string query = @"DELETE FROM ""HEALTH_FACILITY_COLD_CHAIN"" WHERE ""HEALTH_FACILITY_ID"" = @HealthFacilityId";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
				new NpgsqlParameter("@HealthFacilityId", DbType.Int32) { Value = HealthFacilityId }
				};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityColdChain", HealthFacilityId.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityColdChain", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        
        #endregion

        #region Helper Methods
        public static HealthFacilityColdChain GetHealthFacilityColdChain(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacilityColdChain o = new HealthFacilityColdChain();
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
					o.TempMax = Helper.ConvertToDecimal(row["TEMP_MAX"]);
					o.TempMin = Helper.ConvertToDecimal(row["TEMP_MIN"]);
					o.AlarmHighTemp = Helper.ConvertToInt(row["ALARM_HIGH_TEMP"]);
                    o.AlarmLowTemp = Helper.ConvertToInt(row["ALARM_LOW_TEMP"]);
					o.ReportedMonth = Helper.ConvertToInt(row["REPORTED_MONTH"]);
					o.ReportedYear = Helper.ConvertToInt(row["REPORTED_YEAR"]);

                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
					o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacilityColdChain", "GetHealthFacilityColdChain", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<HealthFacilityColdChain> GetHealthFacilityColdChainAsList(DataTable dt)
        {
            List<HealthFacilityColdChain> oList = new List<HealthFacilityColdChain>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacilityColdChain o = new HealthFacilityColdChain();
                    o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
					o.TempMax = Helper.ConvertToDecimal(row["TEMP_MAX"]);
					o.TempMin = Helper.ConvertToDecimal(row["TEMP_MIN"]);
                    o.AlarmHighTemp = Helper.ConvertToInt(row["ALARM_HIGH_TEMP"]);
                    o.AlarmLowTemp = Helper.ConvertToInt(row["ALARM_LOW_TEMP"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.ReportedMonth = Helper.ConvertToInt(row["REPORTED_MONTH"]);
					o.ReportedYear = Helper.ConvertToInt(row["REPORTED_YEAR"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacilityColdChain", "GetHealthFacilityColdChainAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
