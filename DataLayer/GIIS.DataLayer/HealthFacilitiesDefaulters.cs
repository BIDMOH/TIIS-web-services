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
using System.Linq;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class HealthFacilityDefaulters
    {

        #region Properties
        public Int32 childId { get; set; }
		public string childBarcodeId { get; set; }
		public string childName { get; set; }
		public string gudianName { get; set; }
        public string gudianContact { get; set; }
		public string village { get; set; }
		public string HealthFacility { get; set; }
		public Int32 January { get; set; }
		public Int32 February { get; set; }
		public Int32 March { get; set; }
		public Int32 April { get; set; }
		public Int32 May { get; set; }
		public Int32 June { get; set; }
		public Int32 July { get; set; }
		public Int32 August { get; set; }
		public Int32 September { get; set; }
		public Int32 October { get; set; }
		public Int32 Novemebr { get; set; }
		public Int32 Decemeber { get; set; }


		#endregion

		#region GetData
		public static List<HealthFacilityDefaulters> GetHealthFacilityDefaultersList(string hfid, DateTime fromDate, DateTime toDate)
        {
            try
            {
				string query = @"SELECT ""CHILD"".""ID"",""FIRSTNAME1"", ""FIRSTNAME2"", ""LASTNAME1"", ""LASTNAME2"", 
                             ""HEALTHCENTER_ID"", ""COMMUNITY_ID"", 
                               ""ADDRESS"", ""PHONE"", ""MOBILE"", ""EMAIL"", ""MOTHER_FIRSTNAME"", ""MOTHER_LASTNAME"",""CHILD"".""MODIFIED_ON"",""CHILD"".""MODIFIED_BY"", ""BARCODE_ID"" 
                               FROM ""CHILD"" inner join ""VACCINATION_EVENT"" on ""CHILD"".""ID"" = ""VACCINATION_EVENT"".""CHILD_ID"" LEFT JOIN 				     ""NONVACCINATION_REASON"" ON ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = ""NONVACCINATION_REASON"".""ID""
                                
                                WHERE ""CHILD"".""STATUS_ID"" = 1 and ""HEALTHCENTER_ID"" = @hfid and ""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= NOW() AND""VACCINATION_EVENT"".""SCHEDULED_DATE"" >= @fromDate  AND ""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= @toDate and ""VACCINATION_STATUS"" = false GROUP BY ""CHILD"".""ID""  ORDER BY ""BIRTHDATE"",""LASTNAME1""";
				;
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

			
				return GetHealthFacilityDefaultersAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

		public static List<HealthFacilityDefaulters> GetHealthFacilityDefaultersByDistrictList(string districtCouncilId, DateTime fromDate, DateTime toDate)
		{
			try
			{
				string query = String.Format(@"SELECT ""CHILD"".""ID"", ""SYSTEM_ID"",""CHILD_CUMULATIVE_SN"",""CHILD_REGISTRY_YEAR"", ""FIRSTNAME1"", ""FIRSTNAME2"", ""LASTNAME1"", ""LASTNAME2"", 
                               ""BIRTHDATE"", ""GENDER"", ""HEALTHCENTER_ID"", ""BIRTHPLACE_ID"", ""COMMUNITY_ID"", 
                               ""DOMICILE_ID"", ""STATUS_ID"", ""ADDRESS"", ""PHONE"", ""MOBILE"", ""EMAIL"", ""MOTHER_FIRSTNAME"", ""MOTHER_LASTNAME"",""MOTHER_HIV_STATUS"",""MOTHER_TT2_STATUS"", ""FATHER_ID"", 
                               ""FATHER_FIRSTNAME"", ""FATHER_LASTNAME"", ""CARETAKER_ID"", ""CARETAKER_FIRSTNAME"", 
                               ""CARETAKER_LASTNAME"", ""NONVACCINATION_REASON"".""NAME"" as ""NOTES"", ""CHILD"".""IS_ACTIVE"", ""CHILD"".""MODIFIED_ON"", ""CHILD"".""MODIFIED_BY"", ""BARCODE_ID"", ""TEMP_ID""
                                FROM ""CHILD"" inner join ""VACCINATION_EVENT"" on ""CHILD"".""ID"" = ""VACCINATION_EVENT"".""CHILD_ID"" LEFT JOIN ""NONVACCINATION_REASON"" ON ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = ""NONVACCINATION_REASON"".""ID""
                                WHERE ""CHILD"".""STATUS_ID"" = 1 and ""VACCINATION_STATUS"" = false {0} and ""HEALTHCENTER_ID"" = @districtCouncilId AND ""MODIFIED_ON"" >= @fromDate OR ""MODIFIED_ON"" <= @toDate  ORDER BY ""BIRTHDATE"",""LASTNAME1"" OFFSET {1} LIMIT {2} ");
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@districtCouncilId", DbType.Int32) { Value = districtCouncilId },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);


				return GetHealthFacilityDefaultersByDistrictAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacility", "GetHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static List<HealthFacilityDefaulters> GetHealthFacilityFollowUpList(string hfid, DateTime fromDate, DateTime toDate)
		{
			try
			{
				string query = String.Format(@"SELECT ""CHILD"".""ID"", ""SYSTEM_ID"",""CHILD_CUMULATIVE_SN"",""CHILD_REGISTRY_YEAR"", ""FIRSTNAME1"", ""FIRSTNAME2"", ""LASTNAME1"", ""LASTNAME2"", 
                               ""BIRTHDATE"", ""GENDER"", ""HEALTHCENTER_ID"", ""BIRTHPLACE_ID"", ""COMMUNITY_ID"", 
                               ""DOMICILE_ID"", ""STATUS_ID"", ""ADDRESS"", ""PHONE"", ""MOBILE"", ""EMAIL"", ""MOTHER_FIRSTNAME"", ""MOTHER_LASTNAME"",""MOTHER_HIV_STATUS"",""MOTHER_TT2_STATUS"", ""FATHER_ID"", 
                               ""FATHER_FIRSTNAME"", ""FATHER_LASTNAME"", ""CARETAKER_ID"", ""CARETAKER_FIRSTNAME"", 
                               ""CARETAKER_LASTNAME"", ""NONVACCINATION_REASON"".""NAME"" as ""NOTES"", ""CHILD"".""IS_ACTIVE"", ""CHILD"".""MODIFIED_ON"", ""CHILD"".""MODIFIED_BY"", ""BARCODE_ID"", ""TEMP_ID""
                                FROM ""CHILD"" inner join ""VACCINATION_EVENT"" on ""CHILD"".""ID"" = ""VACCINATION_EVENT"".""CHILD_ID"" LEFT JOIN ""NONVACCINATION_REASON"" ON ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = ""NONVACCINATION_REASON"".""ID""
                                WHERE ""CHILD"".""STATUS_ID"" = 1 and ""VACCINATION_STATUS"" = false {0} and ""HEALTHCENTER_ID"" = @hfid AND ""MODIFIED_ON"" >= @fromDate OR ""MODIFIED_ON"" <= @toDate  ORDER BY ""BIRTHDATE"",""LASTNAME1"" OFFSET {1} LIMIT {2} ");
				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
					new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
					};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);


				return GetHealthFacilityDefaultersAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacility", "GetHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		#endregion

		

        #region Helper Methods       

		public static List<HealthFacilityDefaulters> GetHealthFacilityDefaultersAsList(DataTable dt)
        {
            List<HealthFacilityDefaulters> oList = new List<HealthFacilityDefaulters>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacilityDefaulters o = new HealthFacilityDefaulters();
					try
					{
						o.childId = Helper.ConvertToInt(row["ID"]);
					}
					catch
					{
						o.childId = 0;
					}
					o.childBarcodeId = row["BARCODE_ID"].ToString();
					o.childName = row["FIRSTNAME1"].ToString()+  " " + row["LASTNAME1"].ToString(); 
					o.gudianName = row["MOTHER_FIRSTNAME"].ToString() + " " + row["MOTHER_LASTNAME"].ToString();
					o.gudianContact = row["PHONE"].ToString()+" "+ row["MOBILE"].ToString()+" "+ row["EMAIL"].ToString();   
					o.village = row["ADDRESS"].ToString();
					oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacilityDefaulters", "HealthFacilityDefaultersAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }

		public static List<HealthFacilityDefaulters> GetHealthFacilityDefaultersByDistrictAsList(DataTable dt)
		{
			List<HealthFacilityDefaulters> oList = new List<HealthFacilityDefaulters>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					HealthFacilityDefaulters o = new HealthFacilityDefaulters();

					o.HealthFacility = row["NAME"].ToString();

					if (row["jan"].ToString() != null) {
						o.January = Helper.ConvertToInt(row["jan"]);
					}else if (row["feb"].ToString() != null)
					{
						o.February = Helper.ConvertToInt(row["feb"]);
					}
					else if (row["mar"].ToString() != null)
					{
						o.March = Helper.ConvertToInt(row["mar"]);
					}
					else if (row["apr"].ToString() != null)
					{
						o.April = Helper.ConvertToInt(row["apr"]);
					}
					else if (row["may"].ToString() != null)
					{
						o.May = Helper.ConvertToInt(row["may"]);
					}
					else if (row["jun"].ToString() != null)
					{
						o.June = Helper.ConvertToInt(row["jun"]);
					}
					else if (row["jul"].ToString() != null)
					{
						o.July = Helper.ConvertToInt(row["jul"]);
					}
					else if (row["aug"].ToString() != null)
					{
						o.August = Helper.ConvertToInt(row["aug"]);
					}
					else if (row["sep"].ToString() != null)
					{
						o.September = Helper.ConvertToInt(row["sep"]);
					}
					else if (row["oct"].ToString() != null)
					{
						o.October = Helper.ConvertToInt(row["oct"]);
					}
					else if (row["nov"].ToString() != null)
					{
						o.Novemebr = Helper.ConvertToInt(row["nov"]);
					}
					else if (row["dec"].ToString() != null)
					{
						o.Decemeber = Helper.ConvertToInt(row["dec"]);
					}

					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("HealthFacilityDefaulters", "HealthFacilityDefaultersByDistrictAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return oList;
		}



		


        #endregion

    }
}

