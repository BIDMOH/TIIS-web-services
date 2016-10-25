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
	public partial class HealthFacilitySyringesAndSafetyBoxesStockBalance
	{

		#region Properties
		public Int32 HealthFacilityId { get; set; }
		public string ItemName { get; set; }
		public Int32 OpeningBalance { get; set; }
		public Int32 Received { get; set; }
		public Int32 Used { get; set; }
		public Int32 Wastage { get; set; }
		public Int32 StockInHand { get; set; }
		public Int32 StockedOutDays { get; set; }
		public Int32 ReportedMonth { get; set; }
		public Int32 ReportedYear { get; set; }
		public DateTime ModifiedOn { get; set; }
		public Int32 ModifiedBy { get; set; }
		#endregion

		#region GetData
		public static List<HealthFacilitySyringesAndSafetyBoxesStockBalance> GetHealthFacilitySyringesAndSafetyBoxesStockBalanceList()
		{
			try
			{
				string query = @"SELECT * FROM ""HEALTH_FACILITY_SYRINGES_AND_SAFETY_BOXES"";";
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
				return GetHealthFacilitySyringesAndSafetyBoxesStockBalanceAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySyringesAndSafetyBoxesStockBalance", "GetHealthFacilitySyringesAndSafetyBoxesStockBalanceList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static List<HealthFacilitySyringesAndSafetyBoxesStockBalance> GetHealthFacilitySyringesAndSafetyBoxesStockBalance(int healthFacilityId, int reportedMonth, int reportingYear)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_SYRINGES_AND_SAFETY_BOXES"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId AND 
				""REPORTED_MONTH"" = @reportedMonth AND ""REPORTED_YEAR"" = @reportedYear ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId },
					new NpgsqlParameter("@reportedMonth", DbType.Int32)  { Value = reportedMonth },
					new NpgsqlParameter("@reportedYear", DbType.Int32)  { Value = reportingYear }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilitySyringesAndSafetyBoxesStockBalanceAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySyringesAndSafetyBoxesStockBalance", "GetHealthFacilitySyringesAndSafetyBoxesStockBalance", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		public static List<HealthFacilitySyringesAndSafetyBoxesStockBalance> GetHealthFacilitySyringesAndSafetyBoxesStockBalanceAsList(int healthFacilityId)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_SYRINGES_AND_SAFETY_BOXES"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilitySyringesAndSafetyBoxesStockBalanceAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySyringesAndSafetyBoxesStockBalance", "GetHealthFacilitySyringesAndSafetyBoxesStockBalance", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		#endregion

		#region CRUD
		public static int Insert(HealthFacilitySyringesAndSafetyBoxesStockBalance vaccinations)
		{
			try
			{
				string query = @"INSERT INTO ""HEALTH_FACILITY_SYRINGES_AND_SAFETY_BOXES"" (""HEALTH_FACILITY_ID"",""ITEM_NAME"" ,""OPENING_BALANCE"", ""RECEIVED"", ""USED"",
				""WASTAGE"",""STOCK_ON_HAND"",""STOCKED_OUT_DAYS"", ""REPORTED_MONTH"",""REPORTED_YEAR"", ""MODIFIED_ON"",""MODIFIED_BY"") 
				VALUES (@HealthFacilityId, @ItemName, @OpeningBalance, @Received, @Used,
				@Wastage, @StockInHand, @StockedOutDays, @ReportingMonth,@ReportingYear, @ModifiedOn, @ModifiedBy)";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = vaccinations.HealthFacilityId },
					new NpgsqlParameter("@ItemName", DbType.Double)  { Value = vaccinations.ItemName },
					new NpgsqlParameter("@OpeningBalance", DbType.Double)  { Value = vaccinations.OpeningBalance},
					new NpgsqlParameter("@Received", DbType.Double)  { Value = vaccinations.Received},
					new NpgsqlParameter("@Used", DbType.Double)  { Value = vaccinations.Used},
					new NpgsqlParameter("@Wastage", DbType.Int32)  { Value = (object)vaccinations.Wastage ?? DBNull.Value },
					new NpgsqlParameter("@StockInHand", DbType.Int32)  { Value = (object)vaccinations.StockInHand ?? DBNull.Value  },
					new NpgsqlParameter("@StockedOutDays", DbType.Int32)  { Value = (object)vaccinations.StockedOutDays ?? DBNull.Value  },
					new NpgsqlParameter("@ReportingMonth", DbType.Int32)  { Value = vaccinations.ReportedMonth },
					new NpgsqlParameter("@ReportingYear", DbType.Int32)  { Value = vaccinations.ReportedYear },
					new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = vaccinations.ModifiedOn },
					new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = vaccinations.ModifiedBy }
				};
				DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilitySyringesAndSafetyBoxesStockBalance", "1", 1, DateTime.Now, vaccinations.ModifiedBy);
				return 1;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySyringesAndSafetyBoxesStockBalance", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
			}
			return -1;
		}

		public static int Update(HealthFacilitySyringesAndSafetyBoxesStockBalance vaccinations)
		{
			try
			{
				string query = @"UPDATE ""HEALTH_FACILITY_SYRINGES_AND_SAFETY_BOXES"" SET  ""ITEM_NAME"" = @ItemName, ""OPENING_BALANCE"" = @OpeningBalance, ""RECEIVED"" = @Received,
				""USED"" = @Used,""WASTAGE"" = @Wastage, ""STOCK_ON_HAND"" = @StockInHand, ""STOCKED_OUT_DAYS"" = @StockedOutDays,
				""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy 
				WHERE ""HEALTH_FACILITY_ID"" = @HealthFacilityId AND ""REPORTED_MONTH"" = @ReportingMonth AND  ""REPORTED_YEAR"" = @ReportingYear";

				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = vaccinations.HealthFacilityId },
					new NpgsqlParameter("@ItemName", DbType.Double)  { Value = vaccinations.ItemName },
					new NpgsqlParameter("@OpeningBalance", DbType.Double)  { Value = vaccinations.OpeningBalance},
					new NpgsqlParameter("@Received", DbType.Double)  { Value = vaccinations.Received},
					new NpgsqlParameter("@Used", DbType.Double)  { Value = vaccinations.Used},
					new NpgsqlParameter("@Wastage", DbType.Int32)  { Value = (object)vaccinations.Wastage ?? DBNull.Value },
					new NpgsqlParameter("@StockInHand", DbType.Int32)  { Value = (object)vaccinations.StockInHand ?? DBNull.Value  },
					new NpgsqlParameter("@StockedOutDays", DbType.Int32)  { Value = (object)vaccinations.StockedOutDays ?? DBNull.Value  },
					new NpgsqlParameter("@ReportingMonth", DbType.Int32)  { Value = vaccinations.ReportedMonth },
					new NpgsqlParameter("@ReportingYear", DbType.Int32)  { Value = vaccinations.ReportedYear },
					new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = vaccinations.ModifiedOn },
					new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = vaccinations.ModifiedBy }
				};

				int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilitySyringesAndSafetyBoxesStockBalance", vaccinations.HealthFacilityId.ToString(), 2, DateTime.Now, chain.ModifiedBy);
				return rowAffected;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySyringesAndSafetyBoxesStockBalance", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
			}
			return -1;
		}

		public static int Delete(int healthFacilityId, int reportedMonth, int reportingYear)
		{
			try
			{
				string query = @"DELETE FROM ""HEALTH_FACILITY_SYRINGES_AND_SAFETY_BOXES"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId AND ""REPORTED_MONTH"" = @reportedMonth AND ""REPORTED_YEAR"" = @reportedYear";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId },
					new NpgsqlParameter("@reportedMonth", DbType.Int32)  { Value = reportedMonth },
					new NpgsqlParameter("@reportedYear", DbType.Int32)  { Value = reportingYear }
				};
				int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilitySyringesAndSafetyBoxesStockBalance", healthFacilityId.ToString(), 3, DateTime.Now, 1);
				return rowAffected;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilitySyringesAndSafetyBoxesStockBalance", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		#endregion

		#region Helper Methods
		public static HealthFacilitySyringesAndSafetyBoxesStockBalance GetHealthFacilitySyringesAndSafetyBoxesStockBalance(DataTable dt)
		{
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilitySyringesAndSafetyBoxesStockBalance o = new HealthFacilitySyringesAndSafetyBoxesStockBalance();
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);


					o.ItemName = (row["ITEM_NAME"]).ToString();
					o.OpeningBalance = Helper.ConvertToInt(row["OPENING_BALANCE"]);
					o.Received = Helper.ConvertToInt(row["RECEIVED"]);
					o.Used = Helper.ConvertToInt(row["USED"]);
					o.Wastage = Helper.ConvertToInt(row["WASTAGE"]);
					o.StockInHand = Helper.ConvertToInt(row["STOCK_ON_HAND"]);
					o.StockedOutDays = Helper.ConvertToInt(row["STOCKED_OUT_DAYS"]);
					o.ReportedMonth = Helper.ConvertToInt(row["REPORTED_MONTH"]);
					o.ReportedYear = Helper.ConvertToInt(row["REPORTED_YEAR"]);
					o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
					o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
					return o;
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilitySyringesAndSafetyBoxesStockBalance", "GetHealthFacilitySyringesAndSafetyBoxesStockBalance", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return null;
		}

		public static List<HealthFacilitySyringesAndSafetyBoxesStockBalance> GetHealthFacilitySyringesAndSafetyBoxesStockBalanceAsList(DataTable dt)
		{
			List<HealthFacilitySyringesAndSafetyBoxesStockBalance> oList = new List<HealthFacilitySyringesAndSafetyBoxesStockBalance>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilitySyringesAndSafetyBoxesStockBalance o = new HealthFacilitySyringesAndSafetyBoxesStockBalance();
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);


					o.ItemName = (row["ITEM_NAME"]).ToString();
					o.OpeningBalance = Helper.ConvertToInt(row["OPENING_BALANCE"]);
					o.Received = Helper.ConvertToInt(row["RECEIVED"]);
					o.Used = Helper.ConvertToInt(row["USED"]);
					o.Wastage = Helper.ConvertToInt(row["WASTAGE"]);
					o.StockInHand = Helper.ConvertToInt(row["STOCK_ON_HAND"]);
					o.StockedOutDays = Helper.ConvertToInt(row["STOCKED_OUT_DAYS"]);
					o.ReportedMonth = Helper.ConvertToInt(row["REPORTED_MONTH"]);
					o.ReportedYear = Helper.ConvertToInt(row["REPORTED_YEAR"]);
					o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
					o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilitySyringesAndSafetyBoxesStockBalance", "GetHealthFacilitySyringesAndSafetyBoxesStockBalanceAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return oList;
		}
		#endregion

	}
}
