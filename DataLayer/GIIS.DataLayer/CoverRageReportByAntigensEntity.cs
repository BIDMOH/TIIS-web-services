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
    public partial class CoverageReportByAntigensEntity
    {

        #region Properties
        public String Month { get; set; }
		public String DoseName { get; set; }
		public Int32 vaccinations { get; set; }

        #endregion

        #region GetData
		public static List<CoverageReportByAntigensEntity> GetCoverageByHealthFacilityAndDose(string doseName, Int32 healthFacilityId, DateTime fromDate, DateTime toDate)
        {

			string query = @"select  to_char(""VACCINATION_DATE"",'Mon') as mon,extract(month from ""VACCINATION_DATE""),extract(year from ""VACCINATION_DATE"") as yyyy, ""FULLNAME"",COUNT(DISTINCT ""CHILD_ID"") 
							from ""VACCINATION_EVENT""
							inner join ""CHILD"" on ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID"" 
							inner join ""DOSE"" on ""VACCINATION_EVENT"".""DOSE_ID"" = ""DOSE"".""ID""
							inner join ""SCHEDULED_VACCINATION"" on ""DOSE"".""SCHEDULED_VACCINATION_ID"" = ""SCHEDULED_VACCINATION"".""ID""
							inner join ""HEALTH_FACILITY"" ON ""VACCINATION_EVENT"".""HEALTH_FACILITY_ID"" = ""HEALTH_FACILITY"".""ID"" 
							WHERE 
								""VACCINATION_STATUS"" = true AND 
								""FULLNAME"" = @doseName AND
								 (""HEALTH_FACILITY"".""ID"" = @healthFacilityId OR ""HEALTH_FACILITY"".""PARENT_ID"" = @healthFacilityId) AND 
								""CHILD"".""HEALTHCENTER_ID"" = ""VACCINATION_EVENT"".""HEALTH_FACILITY_ID"" AND 
								""VACCINATION_DATE"">=@fromDate AND 
								""VACCINATION_DATE""<@toDate
							GROUP BY 1,2,3,4
							ORDER BY 3,2 ASC";


			try
            {

				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@doseName", DbType.String) { Value = doseName },
					new NpgsqlParameter("@healthFacilityId", DbType.Int32) { Value = healthFacilityId },
					new NpgsqlParameter("@fromDate", DbType.DateTime) { Value = fromDate },
					new NpgsqlParameter("@toDate", DbType.DateTime) { Value = toDate }
				};

				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

				return GetCoverageReport(dt);
            }
            catch (Exception ex)
            {
				Log.InsertEntity("CoverageReportByAntigensEntity", "GetCoverageByHealthFacilityAndAntigen", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
            }
        }

		public static List<List<CoverageReportByAntigensEntity>> GetCoverageByHealthFacilityAndScheduledVaccination(string scheduledVaccinationName, Int32 healthFacilityId, DateTime fromDate, DateTime toDate)
		{
			List<List<CoverageReportByAntigensEntity>> list = new List<List<CoverageReportByAntigensEntity>>();
			List<Dose> doses = Dose.GetDoseByVaccinationName(scheduledVaccinationName);

			foreach (Dose d in doses)
			{
				list.Add(GetCoverageByHealthFacilityAndDose(d.Fullname, healthFacilityId, fromDate, toDate));
			}
			return list;

		}


		public static List<CoverageReportByAntigensEntity> GetCoverageReport(DataTable dt)
        {
			List<CoverageReportByAntigensEntity> oList = new List<CoverageReportByAntigensEntity>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					CoverageReportByAntigensEntity o = new CoverageReportByAntigensEntity();
					o.Month = row["mon"].ToString();
					o.DoseName = row["FULLNAME"].ToString();
					o.vaccinations = Helper.ConvertToInt(row["count"]);

					oList.Add(o);
				}
				catch (Exception ex)
				{
					Log.InsertEntity("CoverageReportByAntigensEntity", "GetCoverageReport", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
    
            return oList;
        }


        #endregion

    }



}
