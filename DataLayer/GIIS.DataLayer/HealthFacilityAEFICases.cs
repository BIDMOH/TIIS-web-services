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
	public partial class HealthFacilityAEFI
	{

		#region Properties
		public String Vaccine { get; set; }
		public Int32 Cases { get; set; }
		public string BatchNo { get; set; }
		public string Manufacturer { get; set; }
		public bool Investigated { get; set; }
		public string ExpiryDate { get; set; }


		#endregion

		#region GetData
		public static List<HealthFacilityAEFI> GetHealthFacilityAEFIList(DateTime fromStartDate, DateTime toEndDate, int healthFacilityId)
		{
			try
			{

				string query = @"SELECT count(*) AS CASES,""ITEM"".""NAME"",""HL7_MANUFACTURERS"".""NAME"" AS MANUFACTURER,""ITEM_LOT"".""LOT_NUMBER"",""ITEM_LOT"".""EXPIRE_DATE""
									FROM ""VACCINATION_APPOINTMENT"" 
									INNER JOIN ""VACCINATION_EVENT"" ON ""VACCINATION_APPOINTMENT"".""ID"" = ""VACCINATION_EVENT"".""APPOINTMENT_ID""
									INNER JOIN ""ITEM_LOT"" ON ""VACCINATION_EVENT"".""VACCINE_LOT_ID"" = ""ITEM_LOT"".""ID""
									INNER JOIN ""ITEM_MANUFACTURER"" ON ""ITEM_LOT"".""GTIN"" = ""ITEM_MANUFACTURER"".""GTIN""
									INNER JOIN ""ITEM"" ON ""ITEM_LOT"".""ITEM_ID"" = ""ITEM"".""ID""
									INNER JOIN ""HL7_MANUFACTURERS"" ON ""ITEM_MANUFACTURER"".""MANUFACTURER_ID"" = ""HL7_MANUFACTURERS"".""ID""
									INNER JOIN ""USER"" ON ""VACCINATION_APPOINTMENT"".""MODIFIED_BY"" = ""USER"".""ID""
									
									WHERE ""AEFI""=@Eafi  
									AND ""VACCINATION_APPOINTMENT"".""AEFI_DATE"" >= @fromStartDate
									AND ""VACCINATION_APPOINTMENT"".""AEFI_DATE"" <= @toEndDate
									AND ""USER"".""HEALTH_FACILITY_ID"" = @healthFacilityId
									
									GROUP BY ""ITEM"".""NAME"",""HL7_MANUFACTURERS"".""NAME"",""ITEM_LOT"".""LOT_NUMBER"",""ITEM_LOT"".""EXPIRE_DATE""
									HAVING count(*)>0";


				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@fromStartDate", DbType.DateTime)  { Value = fromStartDate },
					new NpgsqlParameter("@toEndDate", DbType.DateTime)  { Value = toEndDate },
					new NpgsqlParameter("@Eafi", DbType.Boolean)  { Value = true },
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId }
				};

				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilityAEFIAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityAEFI", "GetHealthFacilityAEFIList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		#endregion



		#region Helper Methods

		public static List<HealthFacilityAEFI> GetHealthFacilityAEFIAsList(DataTable dt)
		{
			List<HealthFacilityAEFI> oList = new List<HealthFacilityAEFI>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityAEFI aefi = new HealthFacilityAEFI();

					aefi.BatchNo = (row["LOT_NUMBER"]).ToString();
					aefi.Cases = Helper.ConvertToInt(row["CASES"]);
					aefi.ExpiryDate = Helper.ConvertToDate(row["EXPIRE_DATE"]).ToString("yyyy-MM-dd");;
					aefi.Manufacturer = (row["MANUFACTURER"]).ToString();
					aefi.Vaccine = (row["NAME"]).ToString();
					aefi.Investigated = false;

					oList.Add(aefi);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityAEFI", "GetHealthFacilityAEFIAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return oList;
		}
		#endregion

	}
}
