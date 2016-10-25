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
	public partial class HealthFacilityBcgOpv0AndTTVaccinations
    {

        #region Properties
        public Int32 HealthFacilityId { get; set; }
		public Int32 DoseId { get; set; }
		public Int32 MaleServiceArea { get; set; }
        public Int32 FemaleServiceArea { get; set; }
        public Int32 CoverageServiceArea { get; set; }
		public Int32 MaleCatchmentArea { get; set; }
		public Int32 FemaleCatchmentArea { get; set; }
		public Int32 CoverageCatchmentArea { get; set; }
		public Int32 CoverageCatchmentAndServiceArea { get; set; }
		public Int32 ReportedMonth { get; set; }
		public Int32 ReportedYear { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        #endregion

        #region GetData
        public static List<HealthFacilityBcgOpv0AndTTVaccinations> GetHealthFacilityBcgOpv0AndTTVaccinationsList()
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_BCG_OPV0_TT_VACCINATIONS"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
				return GetHealthFacilityBcgOpv0AndTTVaccinationsAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBcgOpv0AndTTVaccinations", "GetHealthFacilityBcgOpv0AndTTVaccinationsList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<HealthFacilityBcgOpv0AndTTVaccinations> GetHealthFacilityBcgOpv0AndTTVaccinations(int healthFacilityId, int reportedMonth, int reportingYear)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_BCG_OPV0_TT_VACCINATIONS"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId AND 
				""REPORTED_MONTH"" = @reportedMonth AND ""REPORTED_YEAR"" = @reportedYear ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId },
					new NpgsqlParameter("@reportedMonth", DbType.Int32)  { Value = reportedMonth },
					new NpgsqlParameter("@reportedYear", DbType.Int32)  { Value = reportingYear }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilityBcgOpv0AndTTVaccinationsAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityBcgOpv0AndTTVaccinations", "GetHealthFacilityBcgOpv0AndTTVaccinations", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


		public static List<HealthFacilityBcgOpv0AndTTVaccinations> GetHealthFacilityBcgOpv0AndTTVaccinationsAsList(int healthFacilityId)
		{
			try
			{

				string query = @"SELECT * FROM ""HEALTH_FACILITY_BCG_OPV0_TT_VACCINATIONS"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId ";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
				return GetHealthFacilityBcgOpv0AndTTVaccinationsAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("HealthFacilityBcgOpv0AndTTVaccinations", "GetHealthFacilityBcgOpv0AndTTVaccinations", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

        
        #endregion

        #region CRUD
		public static int Insert(HealthFacilityBcgOpv0AndTTVaccinations vaccinations)
        {
            try
            {
                string query = @"INSERT INTO ""HEALTH_FACILITY_BCG_OPV0_TT_VACCINATIONS"" (""HEALTH_FACILITY_ID"",""DOSE_NAME"" ,""MALE_SERVICE_AREA"", ""FEMALE_SERVICE_AREA"", ""COVERAGE_SERVICE_AREA"",
				""MALE_CATCHMENT_AREA"",""FEMALE_CATCHMENT_AREA"",""COVERAGE_CATCHMENT_AREA"",""COVERAGE_CATCHMENT_AND_SERVICE_AREA"", ""REPORTED_MONTH"",""REPORTED_YEAR"", ""MODIFIED_ON"",""MODIFIED_BY"") 
				VALUES (@HealthFacilityId, @DoseId, @MaleServiceArea, @FemaleServiceArea, @CoverageServiceArea,
				@MaleCatchmentArea, @FemaleCatchmentArea, @CoverageCatchmentArea, @CoverageCatchmentAndServiceArea, @ReportingMonth,@ReportingYear, @ModifiedOn, @ModifiedBy)";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = vaccinations.HealthFacilityId },
					new NpgsqlParameter("@DoseId", DbType.Double)  { Value = vaccinations.DoseId },
					new NpgsqlParameter("@MaleServiceArea", DbType.Double)  { Value = vaccinations.MaleServiceArea},
					new NpgsqlParameter("@FemaleServiceArea", DbType.Double)  { Value = vaccinations.FemaleServiceArea},
					new NpgsqlParameter("@CoverageServiceArea", DbType.Double)  { Value = vaccinations.CoverageServiceArea},
					new NpgsqlParameter("@MaleCatchmentArea", DbType.Int32)  { Value = (object)vaccinations.MaleCatchmentArea ?? DBNull.Value },
					new NpgsqlParameter("@FemaleCatchmentArea", DbType.Int32)  { Value = (object)vaccinations.FemaleCatchmentArea ?? DBNull.Value  },
					new NpgsqlParameter("@CoverageCatchmentArea", DbType.Int32)  { Value = (object)vaccinations.CoverageCatchmentArea ?? DBNull.Value  },
					new NpgsqlParameter("@CoverageCatchmentAndServiceArea", DbType.Int32)  { Value = (object)vaccinations.CoverageCatchmentAndServiceArea ?? DBNull.Value  },
					new NpgsqlParameter("@ReportingMonth", DbType.Int32)  { Value = vaccinations.ReportedMonth },
					new NpgsqlParameter("@ReportingYear", DbType.Int32)  { Value = vaccinations.ReportedYear },
					new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = vaccinations.ModifiedOn },
					new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = vaccinations.ModifiedBy }
				};
                DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityBcgOpv0AndTTVaccinations", "1", 1, DateTime.Now, vaccinations.ModifiedBy);
                return 1;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBcgOpv0AndTTVaccinations", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

		public static int Update(HealthFacilityBcgOpv0AndTTVaccinations vaccinations)
        {
            try
            {
                string query = @"UPDATE ""HEALTH_FACILITY_BCG_OPV0_TT_VACCINATIONS"" SET  ""DOSE_NAME"" = @DoseId, ""MALE_SERVICE_AREA"" = @MaleServiceArea, ""FEMALE_SERVICE_AREA"" = @FemaleServiceArea,
				""COVERAGE_SERVICE_AREA"" = @CoverageServiceArea,""MALE_CATCHMENT_AREA"" = @MaleCatchmentArea, ""FEMALE_CATCHMENT_AREA"" = @FemaleCatchmentArea,
				""COVERAGE_CATCHMENT_AREA"" = @CoverageCatchmentArea, ""COVERAGE_CATCHMENT_AND_SERVICE_AREA"" = @CoverageCatchmentAndServiceArea, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy 
				WHERE ""HEALTH_FACILITY_ID"" = @HealthFacilityId AND ""REPORTED_MONTH"" = @ReportingMonth AND  ""REPORTED_YEAR"" = @ReportingYear";
                
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = vaccinations.HealthFacilityId },
					new NpgsqlParameter("@DoseId", DbType.Double)  { Value = vaccinations.DoseId },
					new NpgsqlParameter("@MaleServiceArea", DbType.Double)  { Value = vaccinations.MaleServiceArea},
					new NpgsqlParameter("@FemaleServiceArea", DbType.Double)  { Value = vaccinations.FemaleServiceArea},
					new NpgsqlParameter("@CoverageServiceArea", DbType.Double)  { Value = vaccinations.CoverageServiceArea},
					new NpgsqlParameter("@MaleCatchmentArea", DbType.Int32)  { Value = (object)vaccinations.MaleCatchmentArea ?? DBNull.Value },
					new NpgsqlParameter("@FemaleCatchmentArea", DbType.Int32)  { Value = (object)vaccinations.FemaleCatchmentArea ?? DBNull.Value  },
					new NpgsqlParameter("@CoverageCatchmentArea", DbType.Int32)  { Value = (object)vaccinations.CoverageCatchmentArea ?? DBNull.Value  },
					new NpgsqlParameter("@CoverageCatchmentAndServiceArea", DbType.Int32)  { Value = (object)vaccinations.CoverageCatchmentAndServiceArea ?? DBNull.Value  },
					new NpgsqlParameter("@ReportingMonth", DbType.Int32)  { Value = vaccinations.ReportedMonth },
					new NpgsqlParameter("@ReportingYear", DbType.Int32)  { Value = vaccinations.ReportedYear },
					new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = vaccinations.ModifiedOn },
					new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = vaccinations.ModifiedBy }
				};

                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
				AuditTable.InsertEntity("HealthFacilityBcgOpv0AndTTVaccinations", vaccinations.HealthFacilityId.ToString(), 2, DateTime.Now, chain.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBcgOpv0AndTTVaccinations", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int healthFacilityId,int reportedMonth, int reportingYear)
        {
            try
            {
                string query = @"DELETE FROM ""HEALTH_FACILITY_BCG_OPV0_TT_VACCINATIONS"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId AND ""REPORTED_MONTH"" = @reportedMonth AND ""REPORTED_YEAR"" = @reportedYear";
				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId },
					new NpgsqlParameter("@reportedMonth", DbType.Int32)  { Value = reportedMonth },
					new NpgsqlParameter("@reportedYear", DbType.Int32)  { Value = reportingYear }
				};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityBcgOpv0AndTTVaccinations", healthFacilityId.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBcgOpv0AndTTVaccinations", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        
        #endregion

        #region Helper Methods
        public static HealthFacilityBcgOpv0AndTTVaccinations GetHealthFacilityBcgOpv0AndTTVaccinations(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacilityBcgOpv0AndTTVaccinations o = new HealthFacilityBcgOpv0AndTTVaccinations();
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);


					o.DoseId = Helper.ConvertToInt(row["DOSE_NAME"]);
					o.MaleServiceArea = Helper.ConvertToInt(row["MALE_SERVICE_AREA"]);
					o.FemaleServiceArea = Helper.ConvertToInt(row["FEMALE_SERVICE_AREA"]);
					o.CoverageServiceArea = Helper.ConvertToInt(row["COVERAGE_SERVICE_AREA"]);

					o.MaleCatchmentArea = Helper.ConvertToInt(row["MALE_CATCHMENT_AREA"]);
					o.FemaleCatchmentArea = Helper.ConvertToInt(row["FEMALE_CATCHMENT_AREA"]);
					o.CoverageCatchmentArea = Helper.ConvertToInt(row["COVERAGE_CATCHMENT_AREA"]);
					o.CoverageCatchmentAndServiceArea = Helper.ConvertToInt(row["COVERAGE_CATCHMENT_AND_SERVICE_AREA"]);

					o.ReportedMonth = Helper.ConvertToInt(row["REPORTED_MONTH"]);
					o.ReportedYear = Helper.ConvertToInt(row["REPORTED_YEAR"]);

                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
					o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacilityBcgOpv0AndTTVaccinations", "GetHealthFacilityBcgOpv0AndTTVaccinations", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<HealthFacilityBcgOpv0AndTTVaccinations> GetHealthFacilityBcgOpv0AndTTVaccinationsAsList(DataTable dt)
        {
            List<HealthFacilityBcgOpv0AndTTVaccinations> oList = new List<HealthFacilityBcgOpv0AndTTVaccinations>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacilityBcgOpv0AndTTVaccinations o = new HealthFacilityBcgOpv0AndTTVaccinations();
					o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);


					o.DoseId = Helper.ConvertToInt(row["DOSE_NAME"]);
					o.MaleServiceArea = Helper.ConvertToInt(row["MALE_SERVICE_AREA"]);
					o.FemaleServiceArea = Helper.ConvertToInt(row["FEMALE_SERVICE_AREA"]);
					o.CoverageServiceArea = Helper.ConvertToInt(row["COVERAGE_SERVICE_AREA"]);

					o.MaleCatchmentArea = Helper.ConvertToInt(row["MALE_CATCHMENT_AREA"]);
					o.FemaleCatchmentArea = Helper.ConvertToInt(row["FEMALE_CATCHMENT_AREA"]);
					o.CoverageCatchmentArea = Helper.ConvertToInt(row["COVERAGE_CATCHMENT_AREA"]);
					o.CoverageCatchmentAndServiceArea = Helper.ConvertToInt(row["COVERAGE_CATCHMENT_AND_SERVICE_AREA"]);

					o.ReportedMonth = Helper.ConvertToInt(row["REPORTED_MONTH"]);
					o.ReportedYear = Helper.ConvertToInt(row["REPORTED_YEAR"]);

					o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
					o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacilityBcgOpv0AndTTVaccinations", "GetHealthFacilityBcgOpv0AndTTVaccinationsAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
