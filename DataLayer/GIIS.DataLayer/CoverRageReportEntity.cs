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
    public partial class CoverageReportEntity
    {

        #region Properties
        public string DoseName { get; set; }
        public Int32 MaleWithinCatchment { get; set; }
		public Int32 FemaleWithinCatchment { get; set; }
		public Int32 TotalWithinCatchment { get; set; }
		public Int32 MaleOutsideCatchment { get; set; }
		public Int32 FemaleOutsideCatchment { get; set; }
		public Int32 TotalOutsideCatchment { get; set; }
		public Int32 TotalVaccinations { get; set; }
		public Int32 ExpectedTotalCatchmentsVaccinations { get; set; }
		public String CoveragePercentage { get; set; }
		public Double CoveragePercentageValue { get; set; }

		public Int32 TargetPopulation { get; set; }

        #endregion

        #region GetData
		public static List<CoverageReportEntity> GetDistrictCoverageReport(string scheduledVaccinationName, int healthFacilityId, DateTime fromDate, DateTime toDate, bool isSchedule)
        {
			
			string query = "SELECT * FROM " +
					"(SELECT * FROM CROSSTAB($$ SELECT T2.\"FULLNAME\", T2.\"GENDER\", T2.\"count\" FROM(" +
					"SELECT T1.\"GENDER\", T1.\"FULLNAME\", COUNT(T1.\"CHILD_ID\") FROM " +
					"(select * from \"VACCINATION_EVENT\" " +
					"inner join \"CHILD\" on \"VACCINATION_EVENT\".\"CHILD_ID\" = \"CHILD\".\"ID\" " +
					"inner join \"DOSE\" on \"VACCINATION_EVENT\".\"DOSE_ID\" = \"DOSE\".\"ID\" " +
					"inner join \"SCHEDULED_VACCINATION\" on \"DOSE\".\"SCHEDULED_VACCINATION_ID\" = \"SCHEDULED_VACCINATION\".\"ID\" " +
					"inner join \"HEALTH_FACILITY\" ON \"VACCINATION_EVENT\".\"HEALTH_FACILITY_ID\" = \"HEALTH_FACILITY\".\"ID\"  " +
					"WHERE " +
					" \"VACCINATION_STATUS\" = true AND " +
					" \"SCHEDULED_VACCINATION\".\"NAME\" = '" + scheduledVaccinationName + "' AND  " +
					" (\"HEALTH_FACILITY\".\"ID\" IN (SELECT DISTINCT A.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = " + healthFacilityId + "\nUNION\nSELECT DISTINCT B.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = " + healthFacilityId + "\nUNION\nSELECT DISTINCT C.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = " + healthFacilityId + "\nUNION\nSELECT DISTINCT D.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = " + healthFacilityId + ")) AND " +
				" \"CHILD\".\"HEALTHCENTER_ID\" = \"VACCINATION_EVENT\".\"HEALTH_FACILITY_ID\" AND \"VACCINATION_DATE\">='"+fromDate.ToString()+"' AND \"VACCINATION_DATE\"<='"+toDate.ToString()+"') " +
					"AS T1 GROUP BY T1.\"FULLNAME\", T1.\"GENDER\" ORDER BY T1.\"FULLNAME\", T1.\"GENDER\"  " +

					")AS T2  $$) AS final_result(DOSE TEXT, FEMALEWithin BIGINT, MALEwithin BIGINT) " +
					") AS table1  INNER JOIN " +
					"(SELECT * FROM CROSSTAB($$ SELECT T2.\"FULLNAME\", T2.\"GENDER\", T2.\"count\" FROM( " +
					"SELECT T1.\"GENDER\", T1.\"FULLNAME\", COUNT(T1.\"CHILD_ID\") FROM " +
					"(select * from \"VACCINATION_EVENT\"  " +
					"inner join \"CHILD\" on \"VACCINATION_EVENT\".\"CHILD_ID\" = \"CHILD\".\"ID\" " +
					"inner join \"DOSE\" on \"VACCINATION_EVENT\".\"DOSE_ID\" = \"DOSE\".\"ID\"  " +
					"inner join \"SCHEDULED_VACCINATION\" on \"DOSE\".\"SCHEDULED_VACCINATION_ID\" = \"SCHEDULED_VACCINATION\".\"ID\" " +
					"inner join \"HEALTH_FACILITY\" ON \"VACCINATION_EVENT\".\"HEALTH_FACILITY_ID\" = \"HEALTH_FACILITY\".\"ID\" " +
					"WHERE " +
					" \"VACCINATION_STATUS\" = true AND  " +
					" \"SCHEDULED_VACCINATION\".\"NAME\" = '" + scheduledVaccinationName + "' AND " +
					" (\"HEALTH_FACILITY\".\"ID\" IN (SELECT DISTINCT A.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = " + healthFacilityId + "\nUNION\nSELECT DISTINCT B.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = " + healthFacilityId + "\nUNION\nSELECT DISTINCT C.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = " + healthFacilityId + "\nUNION\nSELECT DISTINCT D.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = " + healthFacilityId + ") ) AND " +
					" \"CHILD\".\"HEALTHCENTER_ID\" <> \"VACCINATION_EVENT\".\"HEALTH_FACILITY_ID\"  AND \"VACCINATION_DATE\">='"+fromDate.ToString()+"' AND \"VACCINATION_DATE\"<='"+toDate.ToString()+"') " +
					"AS T1 GROUP BY T1.\"FULLNAME\", T1.\"GENDER\" ORDER BY T1.\"FULLNAME\", T1.\"GENDER\" " +
					")AS T2  $$) AS final_result(DOSE TEXT, FEMALEoutside BIGINT, MALEoutside BIGINT) " +
					") AS table2 " +
					"ON table1.\"dose\" = table2.\"dose\" " +
					" INNER JOIN(select \"FULLNAME\" AS dose, COUNT(DISTINCT \"VACCINATION_EVENT\".\"ID\") AS EXPECTED_TOTAL from \"VACCINATION_EVENT\" " +
					" inner join \"CHILD\" on \"VACCINATION_EVENT\".\"CHILD_ID\" = \"CHILD\".\"ID\" " +
					" inner join \"DOSE\" on \"VACCINATION_EVENT\".\"DOSE_ID\" = \"DOSE\".\"ID\" " +
					 " inner join \"SCHEDULED_VACCINATION\" on \"DOSE\".\"SCHEDULED_VACCINATION_ID\" = \"SCHEDULED_VACCINATION\".\"ID\" " +
					" inner join \"HEALTH_FACILITY\" ON \"VACCINATION_EVENT\".\"HEALTH_FACILITY_ID\" = \"HEALTH_FACILITY\".\"ID\"  " +
					" WHERE " +
					" \"SCHEDULED_VACCINATION\".\"NAME\" = '" + scheduledVaccinationName +"' AND " +
					" (\"HEALTH_FACILITY\".\"ID\" IN (SELECT DISTINCT A.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = " + healthFacilityId + "\nUNION\nSELECT DISTINCT B.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = " + healthFacilityId + "\nUNION\nSELECT DISTINCT C.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = " + healthFacilityId + "\nUNION\nSELECT DISTINCT D.\"ID\" FROM \"HEALTH_FACILITY\" AS A\nLEFT JOIN \"HEALTH_FACILITY\" AS B ON A.\"ID\" = B.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS C ON B.\"ID\" = C.\"PARENT_ID\"\nLEFT JOIN \"HEALTH_FACILITY\" AS D ON C.\"ID\" = D.\"PARENT_ID\"\nWHERE \nA.\"ID\" = " + healthFacilityId + ")) AND " +
					" \"CHILD\".\"HEALTHCENTER_ID\" = \"VACCINATION_EVENT\".\"HEALTH_FACILITY_ID\"  AND \"VACCINATION_DATE\">='"+fromDate.ToString()+"' AND \"VACCINATION_DATE\"<='"+toDate.ToString()+"' GROUP BY \"FULLNAME\") " +
				" AS table3 ON table1.\"dose\" = table3.\"dose\" ";


			try
            {

				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@scheduledVaccinationName", DbType.String) { Value = scheduledVaccinationName },
					new NpgsqlParameter("@healthFacilityId", DbType.Int32) { Value = healthFacilityId },
					new NpgsqlParameter("@vaccinationStatus", DbType.Boolean) { Value = true }
				};

				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

				if(isSchedule)
					return GetCoverageReport(dt, scheduledVaccinationName);
				else
					return GetCoverageReportByTargetPopulation(dt, scheduledVaccinationName,healthFacilityId);
            }
            catch (Exception ex)
            {
				Log.InsertEntity("CoverageReportEntity", "GetDistrictCoverageReport", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
            }
        }

		public static List<CoverageReportEntity> GetCoverageReport(DataTable dt,string vaccinationName)
        {
			List<CoverageReportEntity> oList = new List<CoverageReportEntity>();
			if (dt.Rows.Count != 0)
			{
				foreach (DataRow row in dt.Rows)
				{
					try
					{
						if (!row["dose"].ToString().ToLower().Equals("tt") && !row["dose"].ToString().ToLower().Equals("measles"))
						{
							CoverageReportEntity o = new CoverageReportEntity();
							o.DoseName = row["dose"].ToString();
							o.MaleWithinCatchment = Helper.ConvertToInt(row["malewithin"]);
							o.FemaleWithinCatchment = Helper.ConvertToInt(row["femalewithin"]);
							o.FemaleOutsideCatchment = Helper.ConvertToInt(row["femaleoutside"]);
							o.MaleOutsideCatchment = Helper.ConvertToInt(row["maleoutside"]);

							o.ExpectedTotalCatchmentsVaccinations = Helper.ConvertToInt(row["expected_total"]);

							o.TotalWithinCatchment = o.MaleWithinCatchment + o.FemaleWithinCatchment;
							o.TotalOutsideCatchment = o.MaleOutsideCatchment + o.FemaleOutsideCatchment;
							o.TotalVaccinations = o.TotalWithinCatchment + o.TotalOutsideCatchment;

							o.CoveragePercentage = ((o.TotalWithinCatchment * 100) / o.ExpectedTotalCatchmentsVaccinations) + "%";
							o.CoveragePercentageValue = ((o.TotalWithinCatchment * 100) / o.ExpectedTotalCatchmentsVaccinations);

							oList.Add(o);
						}
					}
					catch (Exception ex)
					{
						Log.InsertEntity("CoverageReportEntity", "GetCoverageReport", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
						throw ex;
					}
				}
			}
			else
			{

				List<Dose> doses = Dose.GetDoseByVaccinationName(vaccinationName);
				foreach (Dose dose in doses)
				{
					if (!dose.Fullname.ToLower().Equals("tt") && !dose.Fullname.ToLower().Equals("measles"))
					{
						CoverageReportEntity o = new CoverageReportEntity();
						o.DoseName = dose.Fullname;
						o.MaleWithinCatchment = 0;
						o.FemaleWithinCatchment = 0;
						o.FemaleOutsideCatchment = 0;
						o.MaleOutsideCatchment = 0;

						o.ExpectedTotalCatchmentsVaccinations = 0;

						o.TotalWithinCatchment = 0;
						o.TotalOutsideCatchment = 0;
						o.TotalVaccinations = 0;
						o.CoveragePercentage = " - ";
						o.CoveragePercentageValue = 0;
						oList.Add(o);
					}
				}
			}
    
            return oList;
        }


		public static List<CoverageReportEntity> GetCoverageReportByTargetPopulation(DataTable dt, string vaccinationName,int healthFacilityId)
		{
			List<CoverageReportEntity> oList = new List<CoverageReportEntity>();

			DateTime date = DateTime.Now;
			Cohort c = Cohort.GetCohortDataByHealthFacilityAndYear(healthFacilityId, date.Year);



			if (dt.Rows.Count != 0)
			{
				foreach (DataRow row in dt.Rows)
				{
					try
					{
						if (!row["dose"].ToString().ToLower().Equals("tt") && !row["dose"].ToString().ToLower().Equals("measles"))
						{
							CoverageReportEntity o = new CoverageReportEntity();
							o.DoseName = row["dose"].ToString();
							o.MaleWithinCatchment = Helper.ConvertToInt(row["malewithin"]);
							o.FemaleWithinCatchment = Helper.ConvertToInt(row["femalewithin"]);
							o.FemaleOutsideCatchment = Helper.ConvertToInt(row["femaleoutside"]);
							o.MaleOutsideCatchment = Helper.ConvertToInt(row["maleoutside"]);

							o.ExpectedTotalCatchmentsVaccinations = Helper.ConvertToInt(row["expected_total"]);

							o.TotalWithinCatchment = o.MaleWithinCatchment + o.FemaleWithinCatchment;
							o.TotalOutsideCatchment = o.MaleOutsideCatchment + o.FemaleOutsideCatchment;
							o.TotalVaccinations = o.TotalWithinCatchment + o.TotalOutsideCatchment;

							o.TargetPopulation = c.CohortValue;

							if (c.CohortValue == 0)
							{
								o.CoveragePercentage = " - ";
								o.CoveragePercentageValue = 0;
							}
							else
							{
								o.CoveragePercentage = ((o.TotalWithinCatchment * 100.0) / c.CohortValue) + "%";
								o.CoveragePercentageValue = ((o.TotalWithinCatchment * 100.0) / c.CohortValue);
							}


							oList.Add(o);
						}
					}
					catch (Exception ex)
					{
						Log.InsertEntity("CoverageReportEntity", "GetCoverageReport", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
						throw ex;
					}
				}
			}
			else
			{

				List<Dose> doses = Dose.GetDoseByVaccinationName(vaccinationName);
				foreach (Dose dose in doses)
				{
					if (!dose.Fullname.ToLower().Equals("tt") && !dose.Fullname.ToLower().Equals("measles"))
					{
						CoverageReportEntity o = new CoverageReportEntity();
						o.DoseName = dose.Fullname;
						o.MaleWithinCatchment = 0;
						o.FemaleWithinCatchment = 0;
						o.FemaleOutsideCatchment = 0;
						o.MaleOutsideCatchment = 0;

						o.ExpectedTotalCatchmentsVaccinations = 0;

						o.TotalWithinCatchment = 0;
						o.TotalOutsideCatchment = 0;
						o.TotalVaccinations = 0;
						o.CoveragePercentage = " - ";
						o.CoveragePercentageValue = 0;

						oList.Add(o);
					}
				}
			}

			return oList;
		}



		//This method is used by coverage reports which does not require TT and Measles
		public static List<ScheduledVaccination> GetScheduledVaccinationsWithoutTTandMeasles()
		{
			try
			{
				string query = @"SELECT * FROM ""SCHEDULED_VACCINATION"";";
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
				return GetScheduledVaccinationAsList(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("ScheduledVaccination", "GetScheduledVaccinationList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static List<ScheduledVaccination> GetScheduledVaccinationAsList(DataTable dt)
		{
			List<ScheduledVaccination> oList = new List<ScheduledVaccination>();
			foreach (DataRow row in dt.Rows)
			{
				try
				{
					if (!row["NAME"].ToString().ToLower().Equals("tt") && !row["NAME"].ToString().ToLower().Equals("measles"))
					{
						ScheduledVaccination o = new ScheduledVaccination();
						o.Id = Helper.ConvertToInt(row["ID"]);
						o.Name = row["NAME"].ToString();
						o.Code = row["CODE"].ToString();
						o.ItemId = Helper.ConvertToInt(row["ITEM_ID"]);
						o.EntryDate = Helper.ConvertToDate(row["ENTRY_DATE"]);
						o.ExitDate = Helper.ConvertToDate(row["EXIT_DATE"]);
						o.Status = Helper.ConvertToBoolean(row["STATUS"]);
						o.Deseases = row["DESEASES"].ToString();
						o.Notes = row["NOTES"].ToString();
						o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
						o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
						o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
						oList.Add(o);
					}
				}
				catch (Exception ex)
				{
					Log.InsertEntity("ScheduledVaccination", "GetScheduledVaccinationAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
					throw ex;
				}
			}
			return oList;
		}

        #endregion

    }



}
