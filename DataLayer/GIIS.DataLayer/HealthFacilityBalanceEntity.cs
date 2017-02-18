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
using System.Data;
using System.Text;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class HealthFacilityBalance
    {
        public static List<HealthFacilityBalance> GetHealthFacilityBalanceByHealthFacility(int id)
        {
            try
            {
                HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
                if (hf != null)
                {
                    string query = @"SELECT * FROM ""HEALTH_FACILITY_BALANCE"" join ""ITEM_MANUFACTURER"" using (""GTIN"") join ""ITEM"" on ""ITEM_ID"" = ""ITEM"".""ID"" join ""ITEM_LOT"" using (""GTIN"", ""LOT_NUMBER"") WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue and ""ITEM_MANUFACTURER"".""IS_ACTIVE"" = true and ""ITEM_LOT"".""IS_ACTIVE"" = true  ORDER BY ""ITEM"".""CODE"" ";
                    List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                            {
                            new NpgsqlParameter("@ParamValue", DbType.String) { Value = hf.Code }
                            };
                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

                    return GetHealthFacilityBalanceAsList(dt);
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceByHealthFacility", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }


		public static Int32 GetHealthFacilityBalanceByHealthFacilityIdAndDose(int id, string doseName)
		{
			try
			{
				HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
				if (hf != null)
				{
					string query = @"SELECT SUM(""HEALTH_FACILITY_BALANCE"".""BALANCE"") AS STOCK_ON_HAND FROM ""HEALTH_FACILITY_BALANCE"" join ""ITEM_MANUFACTURER"" using (""GTIN"") join ""ITEM"" on ""ITEM_ID"" = ""ITEM"".""ID"" join ""ITEM_LOT"" using (""GTIN"", ""LOT_NUMBER"") 
						WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue and ""ITEM_MANUFACTURER"".""IS_ACTIVE"" = true and ""ITEM_LOT"".""IS_ACTIVE"" = true and ""ITEM"".""NAME"" = @doseName AND ""ITEM_LOT"".""EXPIRE_DATE"" > now() ";
					List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
						new NpgsqlParameter("@ParamValue", DbType.String) { Value = hf.Code },
						new NpgsqlParameter("@doseName", DbType.String) { Value = doseName }
					};
					DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

					foreach (DataRow row in dt.Rows)
					{
						try
						{
							return Helper.ConvertToInt(row["STOCK_ON_HAND"]);
						}
						catch (Exception ex)
						{
							Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
							throw ex;
						}
					}
				}
				return 0;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceByHealthFacility", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}

		public static Int32 GetHealthFacilityReceivedDosesByHealthFacilityIdAndDose(int id, string doseName, DateTime from,DateTime to)
		{
			try
			{
				HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
				if (hf != null)
				{
					//TODO remove hardcoded transaction types names from the querry. coze
					string query = @"SELECT SUM(T1.""TRANSACTION_QTY_IN_BASE_UOM"") AS DOSES_RECEIVED FROM ""ITEM_TRANSACTION"" T1
									JOIN ( 
											SELECT DISTINCT ""ITEM_TRANSACTION"".""ID""  FROM  ""ITEM_TRANSACTION""
											join ""ITEM_MANUFACTURER"" using (""GTIN"") 
											join ""ITEM"" on ""ITEM_ID"" = ""ITEM"".""ID"" 
											join ""ITEM_LOT"" using (""GTIN"",""ITEM_ID"")
											join ""TRANSACTION_TYPE"" ON ""TRANSACTION_TYPE_ID"" = ""TRANSACTION_TYPE"".""ID""
												WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue AND
													""ITEM"".""CODE"" = @doseName AND
													""TRANSACTION_DATE"" >= @from AND
													""TRANSACTION_DATE"" <= @to AND 
													(
														(""TRANSACTION_TYPE"".""NAME"" = 'Transfer' AND ""TRANSACTION_QTY_IN_BASE_UOM"" > 0) OR 
														""TRANSACTION_TYPE"".""NAME"" = 'Allocation'
													) 
									)T2 ON T1.""ID"" = T2.""ID""   ";
					List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
						new NpgsqlParameter("@ParamValue", DbType.String) { Value = hf.Code },
						new NpgsqlParameter("@doseName", DbType.String) { Value = doseName },
						new NpgsqlParameter("@from", DbType.DateTime) { Value = from },
						new NpgsqlParameter("@to", DbType.DateTime) { Value = to }
					};
					DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

					foreach (DataRow row in dt.Rows)
					{
						try
						{
							return Helper.ConvertToInt(row["DOSES_RECEIVED"]);
						}
						catch (Exception ex)
						{
							Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityReceivedDosesByHealthFacilityIdAndDose", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
							throw ex;
						}
					}
				}
				return 0;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityReceivedDosesByHealthFacilityIdAndDose", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}

		public static Int32 GetHealthFacilityDoseInAllTransactions(int id, string doseName, DateTime from, DateTime to)
		{
			try
			{
				HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
				if (hf != null)
				{
					//TODO remove hardcoded transaction types names from the querry. coze
					string query = @"SELECT SUM(T1.""TRANSACTION_QTY_IN_BASE_UOM"") AS DOSES_RECEIVED FROM ""ITEM_TRANSACTION"" T1
									JOIN ( 
											SELECT DISTINCT ""ITEM_TRANSACTION"".""ID""  FROM  ""ITEM_TRANSACTION""
											join ""ITEM_MANUFACTURER"" using (""GTIN"") 
											join ""ITEM"" on ""ITEM_ID"" = ""ITEM"".""ID"" 
											join ""ITEM_LOT"" using (""GTIN"",""ITEM_ID"")
												WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue AND 
													""ITEM_MANUFACTURER"".""IS_ACTIVE"" = true AND 
													""ITEM_LOT"".""IS_ACTIVE"" = true AND 
													""ITEM"".""CODE"" = @doseName AND 
													""TRANSACTION_DATE"" >= @from AND
													""TRANSACTION_DATE"" <= @to
									)T2 ON T1.""ID"" = T2.""ID""   ";
					List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
						new NpgsqlParameter("@ParamValue", DbType.String) { Value = hf.Code },
						new NpgsqlParameter("@doseName", DbType.String) { Value = doseName },
						new NpgsqlParameter("@from", DbType.DateTime) { Value = from },
						new NpgsqlParameter("@to", DbType.DateTime) { Value = to }
					};
					DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

					foreach (DataRow row in dt.Rows)
					{
						try
						{
							return Helper.ConvertToInt(row["DOSES_RECEIVED"]);
						}
						catch (Exception ex)
						{
							Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityReceivedDosesByHealthFacilityIdAndDose", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
							throw ex;
						}
					}
				}
				return 0;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityReceivedDosesByHealthFacilityIdAndDose", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}

		public static Int32 GetHealthFacilityImmunizedChildrenCountByDose(int id, string doseName, DateTime from, DateTime to)
		{
			try
			{
				HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
				if (hf != null)
				{
					//TODO remove hardcoded transaction types names from the querry. coze
					string query = @"SELECT SUM(T1.""TRANSACTION_QTY_IN_BASE_UOM"") AS IMMUNIZED_CHILDREN FROM ""ITEM_TRANSACTION"" T1
									Join( 
										SELECT DISTINCT ""ITEM_TRANSACTION"".""ID""  FROM  ""ITEM_TRANSACTION""
										join ""ITEM_MANUFACTURER"" using (""GTIN"")
										join ""ITEM"" on ""ITEM_ID"" = ""ITEM"".""ID"" 
										join ""TRANSACTION_TYPE"" ON ""TRANSACTION_TYPE_ID"" = ""TRANSACTION_TYPE"".""ID""
											WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue AND 
												""ITEM"".""NAME"" = @doseName AND 
												""TRANSACTION_DATE"" >= @from AND
												""TRANSACTION_DATE"" <= @to AND 
												""TRANSACTION_TYPE"".""NAME"" = 'Vaccination'
									) T2 ON T1.""ID"" = T2.""ID"" ";
					List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
						new NpgsqlParameter("@ParamValue", DbType.String) { Value = hf.Code },
						new NpgsqlParameter("@doseName", DbType.String) { Value = doseName },
						new NpgsqlParameter("@from", DbType.DateTime) { Value = from },
						new NpgsqlParameter("@to", DbType.DateTime) { Value = to }
					};
					DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

					foreach (DataRow row in dt.Rows)
					{
						try
						{
							return Helper.ConvertToInt(row["IMMUNIZED_CHILDREN"]);
						}
						catch (Exception ex)
						{
							Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityReceivedDosesByHealthFacilityIdAndDose", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
							throw ex;
						}
					}
				}
				return 0;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityReceivedDosesByHealthFacilityIdAndDose", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}

		public static Int32 GetHealthFacilityDosesDiscardedUnoppened(int id, string doseName, DateTime from, DateTime to)
		{
			try
			{
				HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
				if (hf != null)
				{
					//TODO remove hardcoded transaction types names from the querry. coze
					string query = @"SELECT SUM(T1.""TRANSACTION_QTY_IN_BASE_UOM"") AS DOSES_DISCARDED_UNOPENED  FROM
									""ITEM_TRANSACTION"" T1 JOIN
									(SELECT DISTINCT ""ITEM_TRANSACTION"".""ID"" FROM  ""ITEM_TRANSACTION""
									join ""ITEM_MANUFACTURER"" using (""GTIN"") 
									join ""ITEM"" on ""ITEM_ID"" = ""ITEM"".""ID"" 
									join ""ITEM_LOT"" using (""GTIN"",""ITEM_ID"")
									join ""ADJUSTMENT_REASON"" ON  ""ADJUSTMENT_ID"" = ""ADJUSTMENT_REASON"".""ID""
									join ""TRANSACTION_TYPE"" ON ""TRANSACTION_TYPE_ID"" = ""TRANSACTION_TYPE"".""ID""
										WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue AND 
											""ITEM_MANUFACTURER"".""IS_ACTIVE"" = true AND 
											""ITEM_LOT"".""IS_ACTIVE"" = true AND 
											""ITEM"".""CODE"" = @doseName AND 
											""TRANSACTION_DATE"" >= @from AND
											""TRANSACTION_DATE"" <= @to AND 
											""TRANSACTION_TYPE"".""NAME"" = 'Adjustment' AND 
											  (""ADJUSTMENT_REASON"".""NAME"" = 'Kuisha muda wa matumizi' OR
												""ADJUSTMENT_REASON"".""NAME"" = 'Zimevunjika'  OR
												""ADJUSTMENT_REASON"".""NAME"" = 'Chanjo Kuganda'  OR
												""ADJUSTMENT_REASON"".""NAME"" = 'Mabadiliko ya VVM'
											   ) 
									)T2 ON T1.""ID"" = T2.""ID""  ";



					List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
					{
						new NpgsqlParameter("@ParamValue", DbType.String) { Value = hf.Code },
						new NpgsqlParameter("@doseName", DbType.String) { Value = doseName },
						new NpgsqlParameter("@from", DbType.DateTime) { Value = from },
						new NpgsqlParameter("@to", DbType.DateTime) { Value = to }
					};
					DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

					foreach (DataRow row in dt.Rows)
					{
						try
						{
							return Helper.ConvertToInt(row["DOSES_DISCARDED_UNOPENED"]);
						}
						catch (Exception ex)
						{
							Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
							throw ex;
						}
					}
				}
				return 0;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceByHealthFacility", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}

		public static Int32 GetHealthFacilityDosesDiscardedOpened (int healthFacilityId, string doseName, DateTime from, DateTime to)
		{
			try
			{
				//TODO remove hardcoded transaction types names from the querry. coze
				string query = @"SELECT COUNT (""CHILD_ID"") AS DOSE_COUNT,""SCHEDULED_VACCINATION"".""NAME"",""VACCINATION_DATE"" FROM ""VACCINATION_EVENT"" T1 
										JOIN ""DOSE"" ON T1.""DOSE_ID""=""DOSE"".""ID""
										JOIN ""SCHEDULED_VACCINATION"" ON ""DOSE"".""SCHEDULED_VACCINATION_ID""=""SCHEDULED_VACCINATION"".""ID""
										JOIN ""USER"" ON T1.""MODIFIED_BY""=""USER"".""ID""
										JOIN
											(SELECT DISTINCT ""VACCINATION_DATE"" 
											  FROM public.""VACCINATION_EVENT"") T2 USING (""VACCINATION_DATE"") 
										WHERE 
											""SCHEDULED_VACCINATION"".""NAME"" = @doseName 		AND
											""VACCINATION_DATE"" >= @from                  		AND
											""VACCINATION_DATE"" <= @to                    		AND
											""VACCINATION_STATUS"" = @IsActive            		AND
											""USER"".""HEALTH_FACILITY_ID"" = @healthFacilityId 	
										GROUP BY ""SCHEDULED_VACCINATION"".""NAME"",""VACCINATION_DATE"" ORDER BY ""VACCINATION_DATE"" ASC ";



				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32) { Value = healthFacilityId },
					new NpgsqlParameter("@doseName", DbType.String) { Value = doseName },
					new NpgsqlParameter("@IsActive", DbType.Boolean) { Value = true },
					new NpgsqlParameter("@from", DbType.DateTime) { Value = from },
					new NpgsqlParameter("@to", DbType.DateTime) { Value = to }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);


				int count = 0;

				foreach (DataRow row in dt.Rows)
				{
					string query1 = @"SELECT ""ALT_1_QTY_PER"" FROM  ""ITEM_MANUFACTURER""
        							JOIN ""ITEM"" ON ""ITEM_MANUFACTURER"".""ITEM_ID""=""ITEM"".""ID""
        							JOIN ""SCHEDULED_VACCINATION"" ON ""SCHEDULED_VACCINATION"".""NAME""=""ITEM"".""NAME""
									WHERE ""SCHEDULED_VACCINATION"".""NAME"" = @doseName AND ""ITEM_MANUFACTURER"".""IS_ACTIVE""=true LIMIT 1";

					List<NpgsqlParameter> parameters1 = new List<NpgsqlParameter>()
					{
						new NpgsqlParameter("@doseName", DbType.String) { Value = doseName }
					};
					DataTable dt1 = DBManager.ExecuteReaderCommand(query1, CommandType.Text, parameters1);

					int quantityPerDose = 0 ;
					foreach (DataRow row2 in dt1.Rows)
					{
						quantityPerDose = Helper.ConvertToInt(row2["ALT_1_QTY_PER"]);
					}


					try
					{
						if (quantityPerDose == 0)
						{
							quantityPerDose = 1;
						}
						int numberOfDoses = Helper.ConvertToInt(row["DOSE_COUNT"]) / quantityPerDose;
						int numberOFVials = numberOfDoses * quantityPerDose;

						if (Helper.ConvertToInt(row["DOSE_COUNT"]) % quantityPerDose != 0)
						{
							numberOFVials = numberOFVials + quantityPerDose;
						}
						count += numberOFVials;

					}
					catch (Exception ex)
					{
						Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
						throw ex;
					}
				}
				return count;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceByHealthFacility", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}

		}

		public static int GetCountHealthFacilityBalanceByHealthFacility(int id)
        {
            try
            {
                HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
                if (hf != null)
                {
                    string query = @"SELECT count(*) FROM ""HEALTH_FACILITY_BALANCE"" WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue ";
                    List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                            {
                            new NpgsqlParameter("@ParamValue", DbType.String) { Value = hf.Code }
                            };

                    object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                    return int.Parse(count.ToString());
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceByHealthFacility", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetItemManufacturerBalanceForDropDown(int id)
        {
            try
            {
                HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
                if (hf != null)
                {
                    string query = @"Select  '-----' as ""GTIN"", '------' as ""VALUE"", '------' as ""CODE""  UNION  SELECT DISTINCT (""HEALTH_FACILITY_BALANCE"".""GTIN"" || ' - ' || ""ITEM"".""CODE"") as ""GTIN"", ""HEALTH_FACILITY_BALANCE"".""GTIN"" AS ""VALUE"", ""ITEM"".""CODE"" as ""CODE""  FROM ""ITEM_MANUFACTURER"" join ""HEALTH_FACILITY_BALANCE"" using (""GTIN"") join ""ITEM"" on ""ITEM_MANUFACTURER"".""ITEM_ID"" = ""ITEM"".""ID"" WHERE  ""ITEM_MANUFACTURER"".""IS_ACTIVE"" = true AND  ""HEALTH_FACILITY_CODE"" = @ParamValue and (""GTIN_PARENT"" = '' or ""GTIN_PARENT"" is null) ORDER BY ""CODE"" ;";
                    List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                    {
                    new NpgsqlParameter("@ParamValue", DbType.String) { Value = hf.Code }
                    };

                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityEntity", "GetItemManufacturerBalanceForDropDown", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetHealthFacilityBalanceForList(int id)
        {
            try
            {
                HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
                if (hf != null)
                {
                    string query = @"Select '-----' as ""GTIN"" UNION  SELECT DISTINCT ""GTIN"" FROM ""HEALTH_FACILITY_BALANCE"" join ""ITEM_MANUFACTURER"" using (""GTIN"") 
                                    WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue and ""ITEM_MANUFACTURER"".""IS_ACTIVE"" = true ";

                    List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                    {
                    new NpgsqlParameter("@ParamValue", DbType.String) { Value = hf.Code }
                    };

                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public DataTable GetChartData(int id)
        {
            try
            {
                HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
                if (hf != null)
                {
                    string query = @"SELECT VHFB.""NAME"", sum ""BALANCE"", ""SAFETY_STOCK"" FROM ""V_HEALTH_FACILITY_BALANCE_HELPER"" VHFB
                                        LEFT JOIN ""V_GTIN_HF_STOCK_POLICY_HELPER"" VSP ON VHFB.""NAME"" = VSP.""NAME""
                                        WHERE VHFB.""HEALTH_FACILITY_CODE"" = @hfCode AND 
                                        VSP.""HEALTH_FACILITY_CODE"" = @hfCode
                                        order by 1 asc";


                    List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                    {
                        new NpgsqlParameter("@hfCode", DbType.String) { Value = hf.Code }
                    };

                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public DataTable GetCoverageChart(int hfId)
        {
            try
            {
                string s = HealthFacility.GetAllChildsForOneHealthFacility(hfId);
                if (!string.IsNullOrEmpty(s))
                {
                    string query = @"with tmp as
(
SELECT SV.""NAME"" as ""Name"", date_part('month', ""SCHEDULED_DATE"") as ""Month"", date_part('year', ""SCHEDULED_DATE"")  ""Year"", COUNT(*) as ""Count"", 'Scheduled' as ""Type""

FROM ""VACCINATION_EVENT"" VE

INNER JOIN ""DOSE"" D ON VE.""DOSE_ID"" = D.""ID"" 
INNER JOIN ""SCHEDULED_VACCINATION"" SV ON SV.""ID"" = D.""SCHEDULED_VACCINATION_ID""

WHERE ""SCHEDULED_DATE"" BETWEEN (date_trunc('month', now()) - interval '3 month') AND (date_trunc('month', now())::date - 1)
AND ""HEALTH_FACILITY_ID"" IN (" + s + @")

GROUP BY SV.""NAME"", date_part('month', ""SCHEDULED_DATE""), date_part('year', ""SCHEDULED_DATE"")

union all

SELECT SV.""NAME"", date_part('month', ""VACCINATION_DATE""), date_part('year', ""VACCINATION_DATE""), COUNT(*), 'Done'

FROM ""VACCINATION_EVENT"" VE

INNER JOIN ""DOSE"" D ON VE.""DOSE_ID"" = D.""ID"" 
INNER JOIN ""SCHEDULED_VACCINATION"" SV ON SV.""ID"" = D.""SCHEDULED_VACCINATION_ID""

WHERE ""VACCINATION_DATE"" BETWEEN (date_trunc('month', now()) - interval '3 month') AND (date_trunc('month', now())::date - 1)
AND ""VACCINATION_STATUS"" = true
AND ""HEALTH_FACILITY_ID"" IN (" + s + @")

GROUP BY SV.""NAME"", date_part('month', ""VACCINATION_DATE""), date_part('year', ""VACCINATION_DATE"")
)

select t1.""Name"", t1.""Month"", t1.""Year"", trunc((t2.""Count"" / t1.""Count""::float * 100)::numeric, 2) as ""Percentage""
from tmp t1 left outer join tmp t2
on t1.""Name"" = t2.""Name"" and t1.""Month"" = t2.""Month"" and t1.""Year"" = t2.""Year"" and t1.""Type"" <> t2.""Type""
where t1.""Type"" = 'Scheduled'

order by t1.""Name"", t1.""Year"", t1.""Month""; ";


                    //List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                    //{
                    //    new NpgsqlParameter("@hfid_1", DbType.String) { Value = s },
                    //     new NpgsqlParameter("@hfid_2", DbType.String) { Value = s }
                    //};

                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}