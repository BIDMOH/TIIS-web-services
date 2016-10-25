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
	public partial class HealthFacilityVitaminAStockBalance
	{

		#region Properties
		public Int32 HealthFacilityId { get; set; }
		public string VitaminName { get; set; }
		public Int32 OpeningBalance { get; set; }
		public Int32 Received { get; set; }
		public Int32 TotalAdministered { get; set; }
		public Int32 Wastage { get; set; }
		public Int32 StockInHand { get; set; }
		public Int32 ReportedMonth { get; set; }
		public Int32 ReportedYear { get; set; }
		public DateTime ModifiedOn { get; set; }
		public Int32 ModifiedBy { get; set; }
		#endregion

		#region GetData
		public static List<HealthFacilityVitaminAStockBalance> GetHealthFacilityVitaminAStockBalanceList()
		{
			try
			{
				string query = @"SELECT * FROM ""HEALTH_FACILITY_VITAMIN_A_STOCK_BALANCE"";";
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
				return GetHealthFacilityVitaminAStockBalanceAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityVitaminAStockBalance", "GetHealthFacilityVitaminAStockBalanceList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static List<HealthFacilityVitaminAStockBalance> GetHealthFacilityVitaminAStockBalance(int healthFacilityId, int reportedMonth, int reportingYear)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_VITAMIN_A_STOCK_BALANCE"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId AND 
				""REPORTED_MONTH"" = @reportedMonth AND ""REPORTED_YEAR"" = @reportedYear ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId },
					new NpgsqlParameter("@reportedMonth", DbType.Int32)  { Value = reportedMonth },
					new NpgsqlParameter("@reportedYear", DbType.Int32)  { Value = reportingYear }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilityVitaminAStockBalanceAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityVitaminAStockBalance", "GetHealthFacilityVitaminAStockBalance", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static HealthFacilityVitaminAStockBalance GetHealthFacilityVitaminAStockBalanceByVitaminName(int healthFacilityId, int reportedMonth, int reportingYear, string VitaminName)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_VITAMIN_A_STOCK_BALANCE"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId AND 
				""REPORTED_MONTH"" = @reportedMonth AND ""REPORTED_YEAR"" = @reportedYear AND ""VITAMIN_NAME"" = @VitaminName ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId },
					new NpgsqlParameter("@reportedMonth", DbType.Int32)  { Value = reportedMonth },
					new NpgsqlParameter("@reportedYear", DbType.Int32)  { Value = reportingYear },
					new NpgsqlParameter("@VitaminName", DbType.String)  { Value = VitaminName }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilityVitaminAStockBalance(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityVitaminAStockBalance", "GetHealthFacilityVitaminAStockBalanceByVitaminName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		public static List<HealthFacilityVitaminAStockBalance> GetHealthFacilityVitaminAStockBalanceAsList(int healthFacilityId)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_VITAMIN_A_STOCK_BALANCE"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilityVitaminAStockBalanceAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityVitaminAStockBalance", "GetHealthFacilityVitaminAStockBalance", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		#endregion

		#region CRUD
		public static int Insert(HealthFacilityVitaminAStockBalance balance)
		{
			try
			{
				string query = @"INSERT INTO ""HEALTH_FACILITY_VITAMIN_A_STOCK_BALANCE"" (""HEALTH_FACILITY_ID"",""VITAMIN_NAME"" ,""OPENING_BALANCE"", ""RECEIVED"", ""TOTAL_ADMINISTERED"",
				""WASTAGE"",""STOCK_ON_HAND"", ""REPORTED_MONTH"",""REPORTED_YEAR"", ""MODIFIED_ON"",""MODIFIED_BY"") 
				VALUES (@HealthFacilityId, @VitaminName, @OpeningBalance, @Received, @TotalAdministered,
				@Wastage, @StockInHand, @ReportingMonth,@ReportingYear, @ModifiedOn, @ModifiedBy)";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = balance.HealthFacilityId },
					new NpgsqlParameter("@VitaminName", DbType.String)  { Value = balance.VitaminName },
					new NpgsqlParameter("@OpeningBalance", DbType.Int32)  { Value = balance.OpeningBalance},
					new NpgsqlParameter("@Received", DbType.Int32)  { Value = balance.Received},
					new NpgsqlParameter("@TotalAdministered", DbType.Int32)  { Value = balance.TotalAdministered},
					new NpgsqlParameter("@Wastage", DbType.Int32)  { Value = (object)balance.Wastage ?? DBNull.Value },
					new NpgsqlParameter("@StockInHand", DbType.Int32)  { Value = (object)balance.StockInHand ?? DBNull.Value  },
					new NpgsqlParameter("@ReportingMonth", DbType.Int32)  { Value = balance.ReportedMonth },
					new NpgsqlParameter("@ReportingYear", DbType.Int32)  { Value = balance.ReportedYear },
					new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = balance.ModifiedOn },
					new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = balance.ModifiedBy }
				};
				DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilityVitaminAStockBalance", "1", 1, DateTime.Now, balance.ModifiedBy);
				return 1;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityVitaminAStockBalance", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
			}
			return -1;
		}

		public static int Update(HealthFacilityVitaminAStockBalance balance)
		{
			try
			{
				string query = @"UPDATE ""HEALTH_FACILITY_VITAMIN_A_STOCK_BALANCE"" SET  ""VITAMIN_NAME"" = @VitaminName, ""OPENING_BALANCE"" = @OpeningBalance, ""RECEIVED"" = @Received,
				""TOTAL_ADMINISTERED"" = @TotalAdministered,""WASTAGE"" = @Wastage, ""STOCK_ON_HAND"" = @StockInHand,
				""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy 
				WHERE ""HEALTH_FACILITY_ID"" = @HealthFacilityId AND ""REPORTED_MONTH"" = @ReportingMonth AND  ""REPORTED_YEAR"" = @ReportingYear";

				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = balance.HealthFacilityId },
					new NpgsqlParameter("@VitaminName", DbType.String)  { Value = balance.VitaminName },
					new NpgsqlParameter("@OpeningBalance", DbType.Int32)  { Value = balance.OpeningBalance},
					new NpgsqlParameter("@Received", DbType.Int32)  { Value = balance.Received},
					new NpgsqlParameter("@TotalAdministered", DbType.Int32)  { Value = balance.TotalAdministered},
					new NpgsqlParameter("@Wastage", DbType.Int32)  { Value = (object)balance.Wastage ?? DBNull.Value },
					new NpgsqlParameter("@StockInHand", DbType.Int32)  { Value = (object)balance.StockInHand ?? DBNull.Value  },
					new NpgsqlParameter("@ReportingMonth", DbType.Int32)  { Value = balance.ReportedMonth },
					new NpgsqlParameter("@ReportingYear", DbType.Int32)  { Value = balance.ReportedYear },
					new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = balance.ModifiedOn },
					new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = balance.ModifiedBy }
				};

				int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilityVitaminAStockBalance", balance.HealthFacilityId.ToString(), 2, DateTime.Now, balance.ModifiedBy);
				return rowAffected;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityVitaminAStockBalance", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
			}
			return -1;
		}

		public static int Delete(int healthFacilityId, int reportedMonth, int reportingYear)
		{
			try
			{
				string query = @"DELETE FROM ""HEALTH_FACILITY_VITAMIN_A_STOCK_BALANCE"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId AND ""REPORTED_MONTH"" = @reportedMonth AND ""REPORTED_YEAR"" = @reportedYear";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId },
					new NpgsqlParameter("@reportedMonth", DbType.Int32)  { Value = reportedMonth },
					new NpgsqlParameter("@reportedYear", DbType.Int32)  { Value = reportingYear }
				};
				int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilityVitaminAStockBalance", healthFacilityId.ToString(), 3, DateTime.Now, 1);
				return rowAffected;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityVitaminAStockBalance", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		#endregion

		#region Helper Methods
		public static HealthFacilityVitaminAStockBalance GetHealthFacilityVitaminAStockBalance(DataTable dt)
		{
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityVitaminAStockBalance o = new HealthFacilityVitaminAStockBalance();
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);


					o.VitaminName = (row["VITAMIN_NAME"]).ToString();
					o.OpeningBalance = Helper.ConvertToInt(row["OPENING_BALANCE"]);
					o.Received = Helper.ConvertToInt(row["RECEIVED"]);
					o.TotalAdministered = Helper.ConvertToInt(row["TOTAL_ADMINISTERED"]);
					o.Wastage = Helper.ConvertToInt(row["WASTAGE"]);
					o.StockInHand = Helper.ConvertToInt(row["STOCK_ON_HAND"]);
					o.ReportedMonth = Helper.ConvertToInt(row["REPORTED_MONTH"]);
					o.ReportedYear = Helper.ConvertToInt(row["REPORTED_YEAR"]);
					o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
					o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
					return o;
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityVitaminAStockBalance", "GetHealthFacilityVitaminAStockBalance", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return null;
		}

		public static List<HealthFacilityVitaminAStockBalance> GetHealthFacilityVitaminAStockBalanceAsList(DataTable dt)
		{
			List<HealthFacilityVitaminAStockBalance> oList = new List<HealthFacilityVitaminAStockBalance>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityVitaminAStockBalance o = new HealthFacilityVitaminAStockBalance();
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);

					o.VitaminName = (row["VITAMIN_NAME"]).ToString();
					o.OpeningBalance = Helper.ConvertToInt(row["OPENING_BALANCE"]);
					o.Received = Helper.ConvertToInt(row["RECEIVED"]);
					o.TotalAdministered = Helper.ConvertToInt(row["TOTAL_ADMINISTERED"]);
					o.Wastage = Helper.ConvertToInt(row["WASTAGE"]);
					o.StockInHand = Helper.ConvertToInt(row["STOCK_ON_HAND"]);
					o.ReportedMonth = Helper.ConvertToInt(row["REPORTED_MONTH"]);
					o.ReportedYear = Helper.ConvertToInt(row["REPORTED_YEAR"]);
					o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
					o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityVitaminAStockBalance", "GetHealthFacilityVitaminAStockBalanceAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return oList;
		}
		#endregion

	}
}
