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
    public partial class Cohort
    {

        #region Properties
        public Int32 Id { get; set; }
		public Int32 HealthFacilityId { get; set; }
        public string Year { get; set; }
        public int CohortValue { get; set; }
        public string Notes { get; set; }

       
        #endregion

        #region GetData
		public static Cohort GetCohortDataByHealthFacilityAndYear(int healthFacilityId,int year)
        {
            try
            {
				string query = @"SELECT * FROM ""HEALTH_FACILITY_COHORT_DATA"" WHERE ""HEALTH_FACILITY_ID""=@healthFacilityId AND ""YEAR""=@year ;";

				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32) { Value = healthFacilityId },
					new NpgsqlParameter("@year", DbType.Int32) { Value = year }
				};

				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetCohortData(dt);
            }
            catch (Exception ex)
            {
				Log.InsertEntity("Cohort", "GetChildList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        
        #endregion

        #region CRUD
		public static Cohort GetCohortData(DataTable dt)
        {
			Cohort co = new Cohort();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    co.Id = Helper.ConvertToInt(row["ID"]);
					co.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
					co.CohortValue = Helper.ConvertToInt(row["COHORT"]);
					co.Year = row["YEAR"].ToString();
					co.Notes = row["NOTES"].ToString();
                   
                }
                catch (Exception ex)
                {
					Log.InsertEntity("Cohort", "GetCohortData", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return co;
        }

    
        #endregion

    }
}
