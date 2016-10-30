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
	public partial class HealthFacilityStockDistributions
	{

		#region Properties
		public Int32 FromHealthFacilityId { get; set; }
		public Int32 ToHealthFacilityId { get; set; }
		public Int32 ProgramId { get; set; }
		public DateTime DistributionDate { get; set; }
		public Int32 ProductId { get; set; }
		public Int32 ItemId { get; set; }
		public Int32 LotId { get; set; }
		public Int32 VimsLotId { get; set; }
		public string VvmStatus { get; set; }
		public Int32 Quantity { get; set; }
		public string DistributionType { get; set; }
		public string Status { get; set; }

		//HACK to also obtain and send BaseUom to mobile devices
		public string BaseUom { get; set; }
		#endregion

		#region GetData
		public static List<HealthFacilityStockDistributions> GetHealthFacilityStockDistributionsList()
		{
			try
			{
				string query = @"SELECT * FROM ""HEALTH_FACILITY_STOCK_DISTRIBUTIONS"";";
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
				return GetHealthFacilityStockDistributionsAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityStockDistributions", "GetHealthFacilityStockDistributionsList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static List<HealthFacilityStockDistributions> GetHealthFacilityStockDistributions(int toHealthFacilityId, string status)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_STOCK_DISTRIBUTIONS"" 
				JOIN ""ITEM_LOT"" ON ""HEALTH_FACILITY_STOCK_DISTRIBUTIONS"".""LOT_ID""  =""ITEM_LOT"".""ID"" 
				JOIN ""ITEM_MANUFACTURER"" ON ""ITEM_LOT"".""GTIN""  =""ITEM_MANUFACTURER"".""GTIN""
				WHERE ""TO_HEALTH_FACILITY_ID"" = @toHealthFacilityId AND 
				""STATUS"" = @status ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@toHealthFacilityId", DbType.Int32)  { Value = toHealthFacilityId },
					new NpgsqlParameter("@status", DbType.String)  { Value = status }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilityStockDistributionsAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityStockDistributions", "GetHealthFacilityStockDistributions", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}



		#endregion

		#region CRUD
		public static int Insert(HealthFacilityStockDistributions transferedStock)
		{
			try
			{
				string query = @"INSERT INTO ""HEALTH_FACILITY_STOCK_DISTRIBUTIONS"" (""FROM_HEALTH_FACILITY_ID"",""TO_HEALTH_FACILITY_ID"" ,""PROGRAM_ID"", 
				""DISTRIBUTION_DATE"", ""ITEM_ID"",
				""PRODUCT_ID"",""LOT_ID"", ""VIMS_LOT_ID"",""VVM_STATUS"", ""QUANTITY"",""STATUS"",""DISTRIBUTION_TYPE"") 
				VALUES (@FromHealthFacilityId, @ToHealthFacilityId, @ProgramId, @DistributionDate, @ItemId,
				@ProductId, @LotId, @VimsLotId,@VvmStatus, @Quantity, @Status,@DistributionType)";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@FromHealthFacilityId", DbType.Int32)  { Value = transferedStock.FromHealthFacilityId },
					new NpgsqlParameter("@ToHealthFacilityId", DbType.Int32)  { Value = transferedStock.ToHealthFacilityId },
					new NpgsqlParameter("@ProgramId", DbType.Int32)  { Value = transferedStock.ProgramId},
					new NpgsqlParameter("@DistributionDate", DbType.DateTime)  { Value = transferedStock.DistributionDate},
					new NpgsqlParameter("@ItemId", DbType.Int32)  { Value = transferedStock.ItemId},
					new NpgsqlParameter("@ProductId", DbType.Int32)  { Value = (object)transferedStock.ProductId ?? DBNull.Value },
					new NpgsqlParameter("@LotId", DbType.Int32)  { Value = (object)transferedStock.LotId ?? DBNull.Value  },
					new NpgsqlParameter("@VimsLotId", DbType.Int32)  { Value = transferedStock.VimsLotId },
					new NpgsqlParameter("@VvmStatus", DbType.String)  { Value = transferedStock.VvmStatus },
					new NpgsqlParameter("@Quantity", DbType.Int32)  { Value = transferedStock.Quantity },
					new NpgsqlParameter("@Status", DbType.String)  { Value = transferedStock.Status },
					new NpgsqlParameter("@DistributionType", DbType.String)  { Value = transferedStock.DistributionType }
				};
				DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
				return 1;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityStockDistributions", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				//throw ex;
			}
			return -1;
		}


		public static int Delete(int toHealthFacilityId, DateTime distributionDate, string status)
		{
			try
			{
				string query = @"DELETE FROM ""HEALTH_FACILITY_STOCK_DISTRIBUTIONS"" WHERE ""TO_HEALTH_FACILITY_ID"" = @ToHealthFacilityId AND ""DISTRIBUTION_DATE"" = @distributionDate AND ""STATUS"" = @status";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@toHealthFacilityId", DbType.Int32)  { Value = toHealthFacilityId },
					new NpgsqlParameter("@distributionDate", DbType.DateTime)  { Value = distributionDate },
					new NpgsqlParameter("@status", DbType.String)  { Value = status }
				};
				int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				return rowAffected;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityStockDistributions", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		public static int Update(HealthFacilityStockDistributions transferedStock)
		{
			try
			{
				string query = @"UPDATE ""HEALTH_FACILITY_STOCK_DISTRIBUTIONS"" SET ""QUANTITY"" = @quantity, ""STATUS"" = @status WHERE ""TO_HEALTH_FACILITY_ID"" = @toHealthFacilityId AND ""FROM_HEALTH_FACILITY_ID"" = @FromHealthFacilityId AND ""DISTRIBUTION_DATE"" = @DistributionDate 
				AND ""ITEM_ID""=@ItemId AND ""LOT_ID"" = @LotId AND ""PRODUCT_ID""=@ProductId  AND ""DISTRIBUTION_TYPE""=@DistributionType";

				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@FromHealthFacilityId", DbType.Int32)  { Value = transferedStock.FromHealthFacilityId },
					new NpgsqlParameter("@ToHealthFacilityId", DbType.Int32)  { Value = transferedStock.ToHealthFacilityId },
					new NpgsqlParameter("@ProgramId", DbType.Int32)  { Value = transferedStock.ProgramId},
					new NpgsqlParameter("@DistributionDate", DbType.DateTime)  { Value = transferedStock.DistributionDate},
					new NpgsqlParameter("@ItemId", DbType.Int32)  { Value = transferedStock.ItemId},
					new NpgsqlParameter("@ProductId", DbType.Int32)  { Value = (object)transferedStock.ProductId ?? DBNull.Value },
					new NpgsqlParameter("@LotId", DbType.Int32)  { Value = (object)transferedStock.LotId ?? DBNull.Value  },
					new NpgsqlParameter("@VimsLotId", DbType.Int32)  { Value = transferedStock.VimsLotId },
					new NpgsqlParameter("@VvmStatus", DbType.String)  { Value = transferedStock.VvmStatus },
					new NpgsqlParameter("@Quantity", DbType.Int32)  { Value = transferedStock.Quantity },
					new NpgsqlParameter("@Status", DbType.String)  { Value = transferedStock.Status },
					new NpgsqlParameter("@DistributionType", DbType.String)  { Value = transferedStock.DistributionType }
				};


				int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				return rowAffected;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityStockDistributions", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		#endregion

		#region Helper Methods
		public static HealthFacilityStockDistributions GetHealthFacilityStockDistributions(DataTable dt)
		{
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityStockDistributions o = new HealthFacilityStockDistributions();
					o.FromHealthFacilityId = Helper.ConvertToInt(row["FROM_HEALTH_FACILITY_ID"]);
					o.ToHealthFacilityId = Helper.ConvertToInt(row["TO_HEALTH_FACILITY_ID"]);
					o.ProgramId = Helper.ConvertToInt(row["PROGRAM_ID"]);
					o.DistributionDate = Helper.ConvertToDate(row["DISTRIBUTION_DATE"]);
					o.ItemId = Helper.ConvertToInt(row["ITEM_ID"]);
					o.ProductId = Helper.ConvertToInt(row["PRODUCT_ID"]);
					o.LotId = Helper.ConvertToInt(row["LOT_ID"]);
					o.VimsLotId = Helper.ConvertToInt(row["VIMS_LOT_ID"]);
					o.VvmStatus = (row["VVM_STATUS"]).ToString();
					o.Quantity = Helper.ConvertToInt(row["QUANTITY"]);
					o.Status = (row["STATUS"]).ToString();
					o.DistributionType = (row["DISTRIBUTION_TYPE"]).ToString();
					return o;
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityStockDistributions", "GetHealthFacilityStockDistributions", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return null;
		}

		public static List<HealthFacilityStockDistributions> GetHealthFacilityStockDistributionsAsList(DataTable dt)
		{
			List<HealthFacilityStockDistributions> oList = new List<HealthFacilityStockDistributions>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityStockDistributions o = new HealthFacilityStockDistributions();
					o.FromHealthFacilityId = Helper.ConvertToInt(row["FROM_HEALTH_FACILITY_ID"]);
					o.ToHealthFacilityId = Helper.ConvertToInt(row["TO_HEALTH_FACILITY_ID"]);
					o.ProgramId = Helper.ConvertToInt(row["PROGRAM_ID"]);
					o.DistributionDate = Helper.ConvertToDate(row["DISTRIBUTION_DATE"]);
					o.ItemId = Helper.ConvertToInt(row["ITEM_ID"]);
					o.ProductId = Helper.ConvertToInt(row["PRODUCT_ID"]);
					o.LotId = Helper.ConvertToInt(row["LOT_ID"]);
					o.VimsLotId = Helper.ConvertToInt(row["VIMS_LOT_ID"]);
					o.VvmStatus = (row["VVM_STATUS"]).ToString();
					o.Quantity = Helper.ConvertToInt(row["QUANTITY"]);
					o.Status = (row["STATUS"]).ToString();
					o.DistributionType = (row["DISTRIBUTION_TYPE"]).ToString();
					try
					{
						o.BaseUom = (row["BASE_UOM"]).ToString();
					}
					catch (Exception e) { }
					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityStockDistributions", "GetHealthFacilityStockDistributionsAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return oList;
		}
		#endregion

	}
}
