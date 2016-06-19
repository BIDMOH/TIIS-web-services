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
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
	partial class User
	{
		public static ICollection<User> GetUserByStatus(bool s)
		{
			try
			{
				string query = @"SELECT * FROM ""USER"" WHERE ""ID"" <> 1 AND ""IS_ACTIVE"" = @Status ORDER BY ""USERNAME"" ";

				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@Status", DbType.Boolean) { Value = s }
				};

				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetUserAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("User", "GetUserByStatus", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static ICollection<User> GetUsersByHealthFacilityId(int hf_id)
		{
			try
			{
				string query = @"SELECT * FROM ""USER"" WHERE ""ID"" <> 1 AND (( ""HEALTH_FACILITY_ID"" = ANY( CAST( string_to_array(@HealthFacilityId, ',' ) AS INTEGER[] ))) or @HealthFacilityId = '' or @HealthFacilityId is null);";

				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@HealthFacilityId", DbType.String) { Value = hf_id.ToString() }
				};

				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetUserAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("User", "GetUsersByHealthFacilityId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static User GetDataByUsernameAndPasswordWithGcmId(string username, string password, string gcmid)
		{
			try
			{

				User user = GetByUsername(username);
				if (user != null)
				{
					if (Helper.VerifyHash(password, user.Password))
					{
						//Call a method to Update the gcm table
						checkAndUpdateGcmTable(gcmid, user.HealthFacilityId);
						return user;
					}
					else
						return null;
				}
				else
					return null;

			}
			catch (Exception ex)
			{
				Log.InsertEntity("User", "GetDataByUsernameAndPassword", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;

			}
		}

		public static User GetDataByUsernameAndPassword(string username, string password)
		{
			try
			{

				User user = GetByUsername(username);
				if (user != null)
				{
					if (Helper.VerifyHash(password, user.Password))
						return user;
					else
						return null;
				}
				else
					return null;

			}
			catch (Exception ex)
			{
				Log.InsertEntity("User", "GetDataByUsernameAndPassword", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;

			}
		}

		public static void checkAndUpdateGcmTable(string gcmID, int healthFacilityId)
		{

			try
			{
				string query = @"SELECT ""GCM_USERS"".""HEALTH_FACILITY_ID"" FROM ""GCM_USERS"" WHERE ""GCM_ID"" = @gcmID";

				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@gcmID", DbType.String) { Value = gcmID }
				};

				int hfid = 0;
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

				if (dt.Rows.Count > 0)
				{
					foreach (DataRow row in dt.Rows)
					{
						try
						{
							hfid = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
						}
						catch (Exception ex)
						{
							Log.InsertEntity("User", "checkAndUpdateGcmTable", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
							throw ex;
						}
					}

					if (hfid != healthFacilityId && hfid != 0)
					{
						//Update the gcmID with the new health facilithy ID
						string updateQuery = @"UPDATE ""GCM_USERS"" SET ""HEALTH_FACILITY_ID"" = @healthFacilityId WHERE ""GCM_ID"" = @gcmId";

						List<Npgsql.NpgsqlParameter> Updateparameters = new List<NpgsqlParameter>(){
								new NpgsqlParameter("@healthFacilityId", DbType.Int32) { Value = healthFacilityId },
								new NpgsqlParameter("@gcmId", DbType.String) { Value = gcmID }
							};
						int rowAffected = DBManager.ExecuteNonQueryCommand(updateQuery, CommandType.Text, Updateparameters);
						AuditTable.InsertEntity("User", gcmID, 3, DateTime.Now, 1);
					}
				}
				else {
					string insertQuery = @"INSERT INTO ""GCM_USERS"" (""GCM_ID"", ""HEALTH_FACILITY_ID"") VALUES (@gcmId, @healthFacilityId) returning ""GCM_ID"" ";
					List<Npgsql.NpgsqlParameter> insertParameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@gcmId", DbType.String)  { Value = gcmID },
						new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId }
					};

					object id = DBManager.ExecuteScalarCommand(insertQuery, CommandType.Text, insertParameters);
					AuditTable.InsertEntity("User", id.ToString(), 1, DateTime.Now, 1);
				}


			}
			catch (Exception ex)
			{
				Log.InsertEntity("User", "checkAndUpdateGcmTable", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}

		public static User GetByUsername(string username)
		{
			try
			{
				string query = string.Format("SELECT * FROM \"USER\" " + "WHERE UPPER(\"USERNAME\") = @Username AND \"IS_ACTIVE\" = 'True' ");

				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@Username", DbType.String) { Value = username.ToUpper() }
				};

				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetUserAsObject(dt);

			}
			catch (Exception ex)
			{
				Log.InsertEntity("User", "GetDataByUsername", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;

			}

		}

		public static List<User> GetPagedUserList(string username, int hfid, ref int maximumRows, ref int startRowIndex)
		{
			try
			{
				//string wsearch = txtUsername.Text.Replace("'", @"''");
				//string where;
				//int hcId = CurrentEnvironment.LoggedUser.HealthFacilityId;
				//string s = HealthFacility.GetAllChildsForOneHealthFacility(hcId);

				//where = @" UPPER(""USERNAME"") like '%" + wsearch.ToUpper() + "%'";
				//where += string.Format(@" AND ""HEALTH_FACILITY_ID"" in ({0}) ", s);

				//string query = @"SELECT * FROM ""USER"" WHERE " + where + @" ORDER BY ""USERNAME"" OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
				string s = string.Empty;
				HealthFacility hf = HealthFacility.GetHealthFacilityById(hfid);
				string query = @"SELECT * FROM ""USER"" WHERE 1 = 1 "
				+ @" AND ( UPPER(""USERNAME"") like '" + username + "%')";
				if (!hf.TopLevel)
				{
					s = HealthFacility.GetAllChildsForOneHealthFacility(hfid);
					//query +=  @" AND (( ""HEALTH_FACILITY_ID"" = ANY( CAST( string_to_array(" + s + ", ',' ) AS INTEGER[] ))) )";
					query += @" AND (( ""HEALTH_FACILITY_ID"" in (" + s + ")))";
				}

				query += @" ORDER BY ""USERNAME"" OFFSET " + startRowIndex + " LIMIT " + maximumRows;

				//List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				//{
				//    new NpgsqlParameter("@Username", DbType.String) { Value = "%" + username + "%" },
				//    new NpgsqlParameter("@HealthFacilityId", DbType.String) { Value = hfid.ToString() },
				//    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
				//    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
				//};

				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
				return GetUserAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("User", "GetPagedUserList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static int GetCountUserList(string username, int hfid)
		{
			try
			{
				string s = string.Empty;
				HealthFacility hf = HealthFacility.GetHealthFacilityById(hfid);
				string query = @"SELECT COUNT(*) FROM ""USER"" WHERE 1 = 1 "
				   + @" AND ( UPPER(""USERNAME"") like '" + username + "%')";
				if (!hf.TopLevel)
				{
					s = HealthFacility.GetAllChildsForOneHealthFacility(hfid);
					//query += @" AND (( ""HEALTH_FACILITY_ID"" = ANY( CAST( string_to_array(" + hfid.ToString() + ", ',' ) AS INTEGER[] ))) )";
					query += @" AND (( ""HEALTH_FACILITY_ID"" in (" + s + ")))";
				}


				//List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				//{
				//    new NpgsqlParameter("@Username", DbType.String) { Value = ("%" + username + "%") },
				//    new NpgsqlParameter("@HealthFacilityId", DbType.String) { Value = hfid }
				//};

				object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
				return int.Parse(count.ToString());
			}
			catch (Exception ex)
			{
				Log.InsertEntity("User", "GetCountUserList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}
	}
}