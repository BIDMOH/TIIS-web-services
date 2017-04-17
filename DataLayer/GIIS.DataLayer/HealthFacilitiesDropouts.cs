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
	    public partial class HealthFacilityDropout
	    {

	        #region Properties
	        public Int32 BCGVaccinations { get; set; }
		public Int32 MR1Vaccinations { get; set; }
		public Int32 Penta1Vaccinations { get; set; }
		public Int32 Penta3Vaccinations { get; set; }
	        public Int32 BCG_MR1 { get; set; }
		public double BCG_MR1_Percentage { get; set; }
		public Int32 PENTA1_PENTA3 { get; set; }
	
		public double PENTA1_PENTA3_Percentage { get; set; }
		public string Month { get; set; }


		#endregion

		#region GetData
		public static List<HealthFacilityDropout> GetHealthFacilityDropout(string hfid, DateTime fromDate, DateTime toDate)
	        {
	            try
	            {
			string query = @"SELECT * FROM
				(SELECT EXTRACT(MONTH FROM ""VACCINATION_DATE"") AS MONTH,COUNT (DISTINCT ""VACCINATION_EVENT"".""ID"") AS BCG FROM ""VACCINATION_EVENT""
					WHERE 
					""VACCINATION_EVENT"".""VACCINATION_DATE"" <= @toDate AND  ""VACCINATION_EVENT"".""VACCINATION_DATE"" >= @fromDate
					AND ""VACCINATION_EVENT"".""HEALTH_FACILITY_ID"" = @hfid
					AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = 'true'  
					AND ""VACCINATION_EVENT"".""DOSE_ID""=61  GROUP BY MONTH ) AS T1

				FULL OUTER JOIN
				 (SELECT EXTRACT(MONTH FROM ""VACCINATION_DATE"") AS MONTH,COUNT (DISTINCT ""VACCINATION_EVENT"".""ID"") AS MR1 FROM ""VACCINATION_EVENT""
						WHERE 
						""VACCINATION_EVENT"".""VACCINATION_DATE"" <= @toDate AND  ""VACCINATION_EVENT"".""VACCINATION_DATE"" >= @fromDate
						AND ""VACCINATION_EVENT"".""HEALTH_FACILITY_ID"" = @hfid 
						AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = 'true' 
						AND ""VACCINATION_EVENT"".""DOSE_ID""=77  GROUP BY MONTH) as T2 on t1.month = t2.month


				FULL OUTER JOIN
				 (SELECT EXTRACT(MONTH FROM ""VACCINATION_DATE"") AS MONTH,COUNT (DISTINCT ""VACCINATION_EVENT"".""ID"") AS PENTA1 FROM ""VACCINATION_EVENT""
						WHERE 
						""VACCINATION_EVENT"".""VACCINATION_DATE"" <= @toDate AND  ""VACCINATION_EVENT"".""VACCINATION_DATE"" >= @fromDate
						AND ""VACCINATION_EVENT"".""HEALTH_FACILITY_ID"" = @hfid 
						AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = 'true' 
						AND ""VACCINATION_EVENT"".""DOSE_ID""=66  GROUP BY MONTH) as T3 on t1.month = t3.month


				FULL OUTER JOIN
				 (SELECT EXTRACT(MONTH FROM ""VACCINATION_DATE"") AS MONTH,COUNT (DISTINCT ""VACCINATION_EVENT"".""ID"") AS PENTA3 FROM ""VACCINATION_EVENT""
						WHERE 
						""VACCINATION_EVENT"".""VACCINATION_DATE"" <= @toDate AND  ""VACCINATION_EVENT"".""VACCINATION_DATE"" >= @fromDate
						AND ""VACCINATION_EVENT"".""HEALTH_FACILITY_ID"" = @hfid 
						AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = 'true' 
						AND ""VACCINATION_EVENT"".""DOSE_ID""=68  GROUP BY MONTH) as T4 on t1.month = t4.month";

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
			{
				new NpgsqlParameter("@hfid", DbType.Int32) { Value = hfid },
				new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
				new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
			};
			DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);



			

			return GetHealthFacilityDropoutsAsList(dt);
			



	            }
	            catch (Exception ex)
	            {
	                Log.InsertEntity("HealthFacility", "GetHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
	                throw ex;
	            }
	        }



		#endregion

		

	        #region Helper Methods       

		public static List<HealthFacilityDropout> GetHealthFacilityDropoutsAsList(DataTable dt)
	        {
	            List<HealthFacilityDropout> oList = new List<HealthFacilityDropout>();
	            foreach (DataRow row in dt.Rows)
	            {
	                try
	                {
            			HealthFacilityDropout o = new HealthFacilityDropout();
				try
				{
					o.BCGVaccinations = Helper.ConvertToInt(row["BCG"]);
				}
				catch
				{
					o.BCGVaccinations = 0;
				}

				try
				{
					o.MR1Vaccinations = Helper.ConvertToInt(row["MR1"]);
				}
				catch
				{
					o.MR1Vaccinations = 0;
				}


				try
				{
					o.Penta1Vaccinations = Helper.ConvertToInt(row["PENTA1"]);
				}
				catch
				{
					o.Penta1Vaccinations = 0;
				}

				try
				{
					o.Penta3Vaccinations = Helper.ConvertToInt(row["PENTA3"]);
				}
				catch
				{
					o.Penta3Vaccinations = 0;
				}
				o.BCG_MR1 = o.BCGVaccinations - o.MR1Vaccinations;
				o.BCG_MR1_Percentage = o.BCG_MR1 * 1.0 / o.BCGVaccinations;
				o.PENTA1_PENTA3 = o.Penta1Vaccinations - o.Penta3Vaccinations;
				o.PENTA1_PENTA3_Percentage = o.PENTA1_PENTA3 * 1.0 / o.Penta1Vaccinations;
				o.Month = "January";
				oList.Add(o);
	                }
	                catch (Exception ex)
	                {
	                    Log.InsertEntity("HealthFacilityDropout", "GetHealthFacilityDropoutsAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
	                    throw ex;
	                }
	            }
	            return oList;
	        }


	        #endregion

    	}
}

