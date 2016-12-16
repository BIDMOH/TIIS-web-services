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
    public partial class HealthFacilitySessions
    {

        #region Properties
        public Int32 Id { get; set; }
        public Int32 UserId { get; set; }
        public Int32 HealthFacilityId { get; set; }
		public DateTime LoginTime { get; set; }
        public Int32 SessionLength { get; set; }

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

		public static List<HealthFacilitySessions> GetHealthFacilitySessionsByHealthFacilityId(int hfid)
		{

			try
			{
				string query = @"SELECT * FROM ""HEALTH_FACILITIES_SESSIONS"" WHERE ""HEALTH_FACILITY_ID"" = @hfid ";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.String) { Value = hfid }
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
					o.Id = Helper.ConvertToInt(row["ID"]);
					o.UserId = Helper.ConvertToInt(row["USER_ID"]);
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
					o.LoginTime = Helper.ConvertToDate(row["LOGIN_TIME"]);
					o.SessionLength = Helper.ConvertToInt(row["SESSION_LENGTH"]);

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
