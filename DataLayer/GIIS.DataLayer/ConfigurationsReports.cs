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
	public partial class ReportsConfiguration
	{

		#region Properties
		public Int32 Id { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public string Notes { get; set; }
		#endregion


		#region GetData
		public static List<ReportsConfiguration> GetConfigurationList()
		{
			try
			{
				string query = @"SELECT * FROM ""CONFIGURATION_REPORTS"";";
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
				return GetConfigurationAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("ConfigurationReport", "GetConfigurationList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static DataTable GetConfigurationForList()
		{
			try
			{
				string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""CONFIGURATION_REPORTS"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
				return dt;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("ConfigurationReport", "GetConfigurationForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}
		public static ReportsConfiguration GetConfigurationById(Int32 i)
		{
			try
			{
				string query = @"SELECT * FROM ""CONFIGURATION_REPORTS"" WHERE ""ID"" = @ParamValue ";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				// AuditTable.InsertEntity("Configuration", i.ToString(), 4, DateTime.Now, 1);
				return GetConfigurationAsObject(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("ConfigurationReport", "GetConfigurationById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static ReportsConfiguration GetConfigurationByName(string s)
		{
			try
			{
				string query = @"SELECT * FROM ""CONFIGURATION_REPORTS"" WHERE ""NAME"" = @ParamValue ";
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				//   AuditTable.InsertEntity("Configuration", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
				return GetConfigurationAsObject(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("ConfigurationReport", "GetConfigurationByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		#endregion

		#region CRUD
		public static int Insert(ReportsConfiguration o)
		{
			try
			{
				string query = @"INSERT INTO ""CONFIGURATION_REPORTS"" (""NAME"", ""VALUE"", ""NOTES"") VALUES (@Name, @Value, @Notes) returning ""ID"" ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
					new NpgsqlParameter("@Value", DbType.String)  { Value = o.Value },
					new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value }
				};
				object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("ConfigurationReport", id.ToString(), 1, DateTime.Now, 1);
				return int.Parse(id.ToString());
			}
			catch (Exception ex)
			{
				Log.InsertEntity("ConfigurationReport", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
			}
			return -1;
		}

		public static int Update(ReportsConfiguration o)
		{
			try
			{
				string query = @"UPDATE ""CONFIGURATION_REPORTS"" SET ""ID"" = @Id, ""NAME"" = @Name, ""VALUE"" = @Value, ""NOTES"" = @Notes WHERE ""ID"" = @Id ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
					new NpgsqlParameter("@Value", DbType.String)  { Value = o.Value },
					new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
					new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
				};
				int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("ConfigurationReport", o.Id.ToString(), 2, DateTime.Now, 1);
				return rowAffected;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("ConfigurationReport", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
			}
			return -1;
		}

		public static int Delete(int id)
		{
			try
			{
				string query = @"DELETE FROM ""CONFIGURATION_REPORTS"" WHERE ""ID"" = @Id";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
				};
				int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("ConfigurationReport", id.ToString(), 3, DateTime.Now, 1);
				return rowAffected;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("ConfigurationReport", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		#endregion

		#region Helper Methods
		public static ReportsConfiguration GetConfigurationAsObject(DataTable dt)
		{
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					ReportsConfiguration o = new ReportsConfiguration();
					o.Id = Helper.ConvertToInt(row["ID"]);
					o.Name = row["NAME"].ToString();
					o.Value = row["VALUE"].ToString();
					o.Notes = row["NOTES"].ToString();
					return o;
				}
				catch (Exception ex)
				{
					Log.InsertEntity("ConfigurationReport", "GetConfigurationAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return null;
		}

		public static List<ReportsConfiguration> GetConfigurationAsList(DataTable dt)
		{
			List<ReportsConfiguration> oList = new List<ReportsConfiguration>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					ReportsConfiguration o = new ReportsConfiguration();
					o.Id = Helper.ConvertToInt(row["ID"]);
					o.Name = row["NAME"].ToString();
					o.Value = row["VALUE"].ToString();
					o.Notes = row["NOTES"].ToString();
					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("ConfigurationReport", "GetConfigurationAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return oList;
		}
		#endregion

	}
}
