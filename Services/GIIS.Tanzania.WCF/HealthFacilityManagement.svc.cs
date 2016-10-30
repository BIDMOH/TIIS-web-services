//*******************************************************************************
//Copyright 2015 TIIS  Webservices - Tanzania Immunization Information System
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
using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.IO;
using System.ServiceModel.Web;

namespace GIIS.Tanzania.WCF
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HealthFacilityManagement" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select HealthFacilityManagement.svc or HealthFacilityManagement.svc.cs at the Solution Explorer and start debugging.
	public class HealthFacilityManagement : IHealthFacilityManagement
	{
		public List<HealthFacility> GetHealthFacilityByParentId(int parentId)
		{
			int? pId = parentId;
			List<HealthFacility> healthFacilityList = HealthFacility.GetHealthFacilityByParentId(pId);
			return healthFacilityList;
		}

		public List<HealthFacility> GetHealthFacilityById(int id)
		{
			HealthFacility healthFacility = HealthFacility.GetHealthFacilityById(id);

			List<HealthFacility> healthFacilityList = new List<HealthFacility>();
			healthFacilityList.Add(healthFacility);

			return healthFacilityList;
		}

		public List<HealthFacility> GetHealthFacilities()
		{
			List<HealthFacility> healthFacilityList = HealthFacility.GetHealthFacilityList();
			return healthFacilityList;
		}

		public List<HealthFacility> GetHealthFacilityByList(string hList)
		{
			return HealthFacility.GetHealthFacilityByList(hList);
		}

		public int UpdateHealthFacilityCumulativeChildSn(int healthFacilityId, int cumulativeChildSn)
		{
			return GIIS.DataLayer.HealthFacility.updateHealthFacilityCumulativeChildSn(healthFacilityId, cumulativeChildSn);
		}

		public int GetCumulativeChildId(int healthFacilityId)
		{
			return GIIS.DataLayer.HealthFacility.GetCumulativeChildId(healthFacilityId);
		}


		public List<HealthFacilityVaccinationsEntity> GetHealthFacilitiesVaccinations(DateTime fromDate, DateTime toDate)
		{
			List<HealthFacility> healthFacilityList = HealthFacility.GetHealthFacilitiesWithoutDistrictCouncils();

			List<HealthFacilityVaccinationsEntity> healthFacilityEntityList = new List<HealthFacilityVaccinationsEntity>();
			foreach (HealthFacility healthFacility in healthFacilityList)
			{
				HealthFacilityVaccinationsEntity entity = new HealthFacilityVaccinationsEntity();
				List<VaccinationsEntity> vaccinationsList = new List<VaccinationsEntity>();

				List<Dose> doseList = Dose.GetDoseList();
				foreach (Dose d in doseList)
				{
					if (d.IsActive)
					{
						VaccinationsEntity vaccinationEntity = new VaccinationsEntity();
						vaccinationEntity.antigen = ScheduledVaccination.GetScheduledVaccinationById(d.ScheduledVaccinationId).Name;
						vaccinationEntity.dose = d.DoseNumber;
						vaccinationEntity.serviceAreaMale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, true, false);
						vaccinationEntity.serviceAreaFemale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, false, false);
						vaccinationEntity.catchmentMale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, true, true);
						vaccinationEntity.catchmentFemale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, false, true);

						DateTime cummulativeFrom = new DateTime(Int32.Parse(DateTime.Now.Year.ToString()), 1, 1);
						vaccinationEntity.serviceAreaCummulativeTotal = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByCatchment(healthFacility.Id, d.Id, cummulativeFrom, toDate,  false);
						vaccinationEntity.catchmentAreaCummulativeTotal = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByCatchment(healthFacility.Id, d.Id, cummulativeFrom, toDate,  true);




						vaccinationsList.Add(vaccinationEntity);
					}
				}
				entity.healthFacilityVaccinations = vaccinationsList;
				entity.healthFacilityName = healthFacility.Name;
				entity.healthFacilityId = healthFacility.Id;
				healthFacilityEntityList.Add(entity);
			}
			return healthFacilityEntityList;
		}


		public HealthFacilityVaccinationsEntity GetHealthFacilityVaccinations(int healthFacilityId,DateTime fromDate, DateTime toDate)
		{
			HealthFacility healthFacility = HealthFacility.GetHealthFacilityById(healthFacilityId);

			HealthFacilityVaccinationsEntity entity = new HealthFacilityVaccinationsEntity();


			List<VaccinationsEntity> vaccinationsList = new List<VaccinationsEntity>();

			List<Dose> doseList = Dose.GetDoseList();
			foreach (Dose d in doseList)
			{
				if (d.IsActive)
				{
					VaccinationsEntity vaccinationEntity = new VaccinationsEntity();
					vaccinationEntity.antigen = ScheduledVaccination.GetScheduledVaccinationById(d.ScheduledVaccinationId).Name;
					vaccinationEntity.dose = d.DoseNumber;

					if (vaccinationEntity.antigen.Equals("BCG") || vaccinationEntity.antigen.Equals("TT") || (vaccinationEntity.antigen.Equals("OPV") && vaccinationEntity.dose==0))
					{
						try
						{
							HealthFacilityBcgOpv0AndTTVaccinations v = GIIS.DataLayer.HealthFacilityBcgOpv0AndTTVaccinations.GetHealthFacilityBcgOpv0AndTTVaccinationsByDoseId(healthFacilityId, d.Id, fromDate.Month, fromDate.Year);
							vaccinationEntity.serviceAreaMale = v.MaleServiceArea;
							vaccinationEntity.serviceAreaFemale = v.FemaleServiceArea;
							vaccinationEntity.catchmentMale = v.MaleCatchmentArea;
							vaccinationEntity.catchmentFemale = v.FemaleCatchmentArea;
						}
						catch (Exception e){}
					}
					else {
						vaccinationEntity.serviceAreaMale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, true, false);
						vaccinationEntity.serviceAreaFemale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, false, false);
						vaccinationEntity.catchmentMale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, true, true);
						vaccinationEntity.catchmentFemale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, false, true);

						DateTime cummulativeFrom = new DateTime(Int32.Parse(DateTime.Now.Year.ToString()), 1, 1);
						vaccinationEntity.serviceAreaCummulativeTotal = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByCatchment(healthFacility.Id, d.Id, cummulativeFrom, toDate, false);
						vaccinationEntity.catchmentAreaCummulativeTotal = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByCatchment(healthFacility.Id, d.Id, cummulativeFrom, toDate, true);
					}

					vaccinationsList.Add(vaccinationEntity);
				}
			}
			entity.healthFacilityVaccinations = vaccinationsList;
			entity.healthFacilityName = healthFacility.Name;
			entity.healthFacilityId = healthFacility.Id;
			return entity;
		}

		public List<HealthFacilityColdChain> GetHealthFacilityColdChain(int healthFacilityId, int reportingMonth, int reportingYear)
		{
			List <HealthFacilityColdChain> coldChain = GIIS.DataLayer.HealthFacilityColdChain.GetHealthFacilityColdChain(healthFacilityId, reportingMonth,reportingYear);
			return coldChain;
		}

		public List<HealthFacilityColdChain> GetHealthFacilityColdChainAsList(int healthFacilityId)
		{
			List<HealthFacilityColdChain> coldChain = GIIS.DataLayer.HealthFacilityColdChain.GetHealthFacilityColdChainAsList(healthFacilityId);
			return coldChain;
		}

		public IntReturnValue StoreHealthFacilityColdChain(int healthFacilityId, double tempMax, double tempMin, int alarmHighTemp, int alarmLowTemp, int reportingMonth, int reportingYear,int userId, DateTime modifiedOn)
		{
			HealthFacilityColdChain coldChain = new HealthFacilityColdChain();

			coldChain.HealthFacilityId = healthFacilityId;
			coldChain.TempMax = tempMax;
			coldChain.TempMin = tempMin;
			coldChain.AlarmHighTemp = alarmHighTemp;
			coldChain.AlarmLowTemp = alarmLowTemp;
			coldChain.ReportedMonth = reportingMonth;
			coldChain.ReportedYear = reportingYear;
			coldChain.ModifiedOn = modifiedOn;
			coldChain.ModifiedBy = userId;

			int healthFacilityColdChainInserted;
			List<HealthFacilityColdChain> coldChainList = GIIS.DataLayer.HealthFacilityColdChain.GetHealthFacilityColdChain(healthFacilityId, reportingMonth, reportingYear);

			if (coldChainList == null || coldChainList.Count == 0)
			{
				healthFacilityColdChainInserted = HealthFacilityColdChain.Insert(coldChain);
			}
			else 
			{
				healthFacilityColdChainInserted = HealthFacilityColdChain.Update(coldChain);

			}
			BroadcastStoredHealthFacilityData(healthFacilityId,"UpdateHealthFacilityColdChain");
			IntReturnValue irv = new IntReturnValue();
			irv.id = healthFacilityColdChainInserted;
			return irv;
		}



		public List<HealthFacilityDeseaseSurvailance> GetHealthFacilityDeseaseSurvailance(int healthFacilityId, int reportingMonth, int reportingYear)
		{
			List<HealthFacilityDeseaseSurvailance> deseaseSurvailance = GIIS.DataLayer.HealthFacilityDeseaseSurvailance.GetHealthFacilityDeseaseSurvailance(healthFacilityId, reportingMonth, reportingYear);
			return deseaseSurvailance;
		}

		public List<HealthFacilityDeseaseSurvailance> GetHealthFacilityDeseaseSurvailanceAsList(int healthFacilityId)
		{
			List<HealthFacilityDeseaseSurvailance> deseaseSurvailance = GIIS.DataLayer.HealthFacilityDeseaseSurvailance.GetHealthFacilityDeseaseSurvailanceList(healthFacilityId);
			return deseaseSurvailance;
		}

		public IntReturnValue StoreHealthFacilityDeseaseSurvailance(int healthFacilityId, int feverMonthlyCases, int feverMonthlyDeaths,int AFPMonthlyCases, int AFPDeaths, int neonatalTTCases, int neonatalTTDeaths, int reportingMonth, int reportingYear, int userId, DateTime modifiedOn)
		{
			HealthFacilityDeseaseSurvailance deseaseSurvailance = new HealthFacilityDeseaseSurvailance();

			deseaseSurvailance.HealthFacilityId = healthFacilityId;
			deseaseSurvailance.FeverMonthlyCases = feverMonthlyCases;
			deseaseSurvailance.FeverMonthlyDeaths = feverMonthlyDeaths ;
			deseaseSurvailance.AFPMonthlyCases = AFPMonthlyCases;
			deseaseSurvailance.AFPDeaths = AFPDeaths;
			deseaseSurvailance.NeonatalTTCases = neonatalTTCases;
			deseaseSurvailance.NeonatalTTDeaths = neonatalTTDeaths;
			deseaseSurvailance.ReportedMonth = reportingMonth;
			deseaseSurvailance.ReportedYear = reportingYear;
			deseaseSurvailance.ModifiedOn = modifiedOn;
			deseaseSurvailance.ModifiedBy = userId;

			int HealthFacilityDeseaseSurvailanceInserted;
			List<HealthFacilityDeseaseSurvailance> deseaseSurvailanceList = GIIS.DataLayer.HealthFacilityDeseaseSurvailance.GetHealthFacilityDeseaseSurvailance(healthFacilityId, reportingMonth, reportingYear);

			if (deseaseSurvailanceList == null || deseaseSurvailanceList.Count == 0)
			{
				HealthFacilityDeseaseSurvailanceInserted = HealthFacilityDeseaseSurvailance.Insert(deseaseSurvailance);
			}
			else
			{
				HealthFacilityDeseaseSurvailanceInserted = HealthFacilityDeseaseSurvailance.Update(deseaseSurvailance);

			}
			BroadcastStoredHealthFacilityData(healthFacilityId,"UpdateHealthFacilityDeseaseSurvailance");
			IntReturnValue irv = new IntReturnValue();
			irv.id = HealthFacilityDeseaseSurvailanceInserted;
			return irv;
		}



		public List<HealthFacilityImmunizationSessions> GetHealthFacilityImmunizationSessions(int healthFacilityId, int reportingMonth, int reportingYear)
		{
			List<HealthFacilityImmunizationSessions> immunizationSessions = GIIS.DataLayer.HealthFacilityImmunizationSessions.GetHealthFacilityImmunizationSessions(healthFacilityId, reportingMonth, reportingYear);
			return immunizationSessions;
		}

		public List<HealthFacilityImmunizationSessions> GetHealthFacilityImmunizationSessionsAsList(int healthFacilityId)
		{
			List<HealthFacilityImmunizationSessions> immunizationSessions = GIIS.DataLayer.HealthFacilityImmunizationSessions.GetHealthFacilityImmunizationSessionsAsList(healthFacilityId);
			return immunizationSessions;
		}

		public IntReturnValue StoreHealthFacilityImmunizationSessions(int healthFacilityId, int OutreachPlanned, string OtherMajorImmunizationActivities, int reportingMonth, int reportingYear, int userId, DateTime modifiedOn)
		{
			HealthFacilityImmunizationSessions deseaseSurvailance = new HealthFacilityImmunizationSessions();

			deseaseSurvailance.HealthFacilityId = healthFacilityId;
			deseaseSurvailance.OutreachPlanned = OutreachPlanned;
			deseaseSurvailance.OtherMajorImmunizationActivities = OtherMajorImmunizationActivities;
			deseaseSurvailance.ReportedMonth = reportingMonth;
			deseaseSurvailance.ReportedYear = reportingYear;
			deseaseSurvailance.ModifiedOn = modifiedOn;
			deseaseSurvailance.ModifiedBy = userId;

			int HealthFacilityImmunizationSessionsInserted;
			List<HealthFacilityImmunizationSessions> immunizationSessionsList = GIIS.DataLayer.HealthFacilityImmunizationSessions.GetHealthFacilityImmunizationSessions(healthFacilityId, reportingMonth, reportingYear);

			if (immunizationSessionsList == null || immunizationSessionsList.Count == 0)
			{
				HealthFacilityImmunizationSessionsInserted = HealthFacilityImmunizationSessions.Insert(deseaseSurvailance);
			}
			else
			{
				HealthFacilityImmunizationSessionsInserted = HealthFacilityImmunizationSessions.Update(deseaseSurvailance);

			}
			BroadcastStoredHealthFacilityData(healthFacilityId, "UpdateHealthFacilityImmunizationSessions");
			IntReturnValue irv = new IntReturnValue();
			irv.id = HealthFacilityImmunizationSessionsInserted;
			return irv;
		}


		public List<HealthFacilityVitaminAStockBalance> GetHealthFacilityVitaminAStockBalance(int healthFacilityId, int reportingMonth, int reportingYear)
		{
			List<HealthFacilityVitaminAStockBalance> healthFacilitiesVitaminAStockBalance = GIIS.DataLayer.HealthFacilityVitaminAStockBalance.GetHealthFacilityVitaminAStockBalance(healthFacilityId, reportingMonth, reportingYear);
			return healthFacilitiesVitaminAStockBalance;
		}

		public List<HealthFacilityVitaminAStockBalance> GetHealthFacilityVitaminAStockBalanceAsList(int healthFacilityId)
		{
			List<HealthFacilityVitaminAStockBalance> healthFacilitiesVitaminAStockBalance = GIIS.DataLayer.HealthFacilityVitaminAStockBalance.GetHealthFacilityVitaminAStockBalanceAsList(healthFacilityId);
			return healthFacilitiesVitaminAStockBalance;
		}

		public IntReturnValue StoreHealthFacilityVitaminAStockBalance(int healthFacilityId,string VitaminName, int OpeningBalance, int Received, int StockInHand, int TotalAdministered, int wastage, int reportingMonth, int reportingYear, int userId, DateTime modifiedOn)
		{
			HealthFacilityVitaminAStockBalance vitaminAStockBalance = new HealthFacilityVitaminAStockBalance();

			vitaminAStockBalance.HealthFacilityId = healthFacilityId;
			vitaminAStockBalance.Wastage = wastage;
			vitaminAStockBalance.VitaminName = VitaminName;
			vitaminAStockBalance.Received = Received;
			vitaminAStockBalance.StockInHand = StockInHand;
			vitaminAStockBalance.TotalAdministered = TotalAdministered;
			vitaminAStockBalance.ReportedMonth = reportingMonth;
			vitaminAStockBalance.ReportedYear = reportingYear;
			vitaminAStockBalance.ModifiedOn = modifiedOn;
			vitaminAStockBalance.ModifiedBy = userId;


			int previousMonth, previousMonthYear;

			if (reportingMonth != 1)
			{
				previousMonth = reportingMonth - 1;
				previousMonthYear = reportingYear;
			}
			else
			{
				previousMonth = 12;
				previousMonthYear = reportingYear - 1;
			}

			HealthFacilityVitaminAStockBalance previousVitaminAStock = GIIS.DataLayer.HealthFacilityVitaminAStockBalance.GetHealthFacilityVitaminAStockBalanceByVitaminName(healthFacilityId, previousMonth, previousMonthYear, VitaminName);
			if (previousVitaminAStock != null)
			{
				vitaminAStockBalance.OpeningBalance = previousVitaminAStock.StockInHand;
			}
			else {
				vitaminAStockBalance.OpeningBalance = OpeningBalance;
			}



			int HealthFacilityVitaminAStockBalanceInserted;
			HealthFacilityVitaminAStockBalance vitaminAStock = GIIS.DataLayer.HealthFacilityVitaminAStockBalance.GetHealthFacilityVitaminAStockBalanceByVitaminName(healthFacilityId, reportingMonth, reportingYear, VitaminName);

			if (vitaminAStock == null)
			{
				HealthFacilityVitaminAStockBalanceInserted = HealthFacilityVitaminAStockBalance.Insert(vitaminAStockBalance);
			}
			else
			{
				HealthFacilityVitaminAStockBalanceInserted = HealthFacilityVitaminAStockBalance.Update(vitaminAStockBalance);

			}
			BroadcastStoredHealthFacilityData(healthFacilityId, "UpdateHealthFacilityVitaminAStockBalance");
			IntReturnValue irv = new IntReturnValue();
			irv.id = HealthFacilityVitaminAStockBalanceInserted;
			return irv;
		}




		public List<HealthFacilitySyringesAndSafetyBoxesStockBalance> GetHealthFacilitySyringesAndSafetyBoxesStockBalance(int healthFacilityId, int reportingMonth, int reportingYear)
		{
			List<HealthFacilitySyringesAndSafetyBoxesStockBalance> healthFacilitiesVitaminAStockBalance = GIIS.DataLayer.HealthFacilitySyringesAndSafetyBoxesStockBalance.GetHealthFacilitySyringesAndSafetyBoxesStockBalance(healthFacilityId, reportingMonth, reportingYear);
			return healthFacilitiesVitaminAStockBalance;
		}

		public List<HealthFacilitySyringesAndSafetyBoxesStockBalance> GetHealthFacilitySyringesAndSafetyBoxesStockBalanceAsList(int healthFacilityId)
		{
			List<HealthFacilitySyringesAndSafetyBoxesStockBalance> healthFacilitiesVitaminAStockBalance = GIIS.DataLayer.HealthFacilitySyringesAndSafetyBoxesStockBalance.GetHealthFacilitySyringesAndSafetyBoxesStockBalanceAsList(healthFacilityId);
			return healthFacilitiesVitaminAStockBalance;
		}

		public IntReturnValue StoreHealthFacilitySyringesAndSafetyBoxesStockBalance(int healthFacilityId, string ItemName, int OpeningBalance, int Received, int StockInHand, int Used, int wastage, int StockedOutDays, int reportingMonth, int reportingYear, int userId, DateTime modifiedOn)
		{
			HealthFacilitySyringesAndSafetyBoxesStockBalance syringesAndSafetyBoxesStockBalance = new HealthFacilitySyringesAndSafetyBoxesStockBalance();

			syringesAndSafetyBoxesStockBalance.HealthFacilityId = healthFacilityId;
			syringesAndSafetyBoxesStockBalance.Wastage = wastage;
			syringesAndSafetyBoxesStockBalance.ItemName = ItemName;
			syringesAndSafetyBoxesStockBalance.Received = Received;
			syringesAndSafetyBoxesStockBalance.StockInHand = StockInHand;
			syringesAndSafetyBoxesStockBalance.Used = Used;
			syringesAndSafetyBoxesStockBalance.StockedOutDays = StockedOutDays;
			syringesAndSafetyBoxesStockBalance.ReportedMonth = reportingMonth;
			syringesAndSafetyBoxesStockBalance.ReportedYear = reportingYear;
			syringesAndSafetyBoxesStockBalance.ModifiedOn = modifiedOn;
			syringesAndSafetyBoxesStockBalance.ModifiedBy = userId;

			int previousMonth, previousMonthYear;

			if (reportingMonth != 1)
			{
				previousMonth = reportingMonth - 1;
				previousMonthYear = reportingYear;
			}
			else
			{
				previousMonth = 12;
				previousMonthYear = reportingYear - 1;
			}

			HealthFacilitySyringesAndSafetyBoxesStockBalance previousSyringesAndSafetyBoxesStock = GIIS.DataLayer.HealthFacilitySyringesAndSafetyBoxesStockBalance.HealthFacilitySyringesAndSafetyBoxesStockBalanceByItem(healthFacilityId, previousMonth, previousMonthYear, ItemName);
			if (previousSyringesAndSafetyBoxesStock != null)
			{
				syringesAndSafetyBoxesStockBalance.OpeningBalance = previousSyringesAndSafetyBoxesStock.StockInHand;
			}
			else {
				syringesAndSafetyBoxesStockBalance.OpeningBalance = OpeningBalance;
			}



			int HealthFacilitySyringesAndSafetyBoxesStockBalanceInserted;
			HealthFacilitySyringesAndSafetyBoxesStockBalance vitaminAStock = GIIS.DataLayer.HealthFacilitySyringesAndSafetyBoxesStockBalance.HealthFacilitySyringesAndSafetyBoxesStockBalanceByItem(healthFacilityId, reportingMonth, reportingYear, ItemName);

			if (vitaminAStock == null)
			{
				HealthFacilitySyringesAndSafetyBoxesStockBalanceInserted = HealthFacilitySyringesAndSafetyBoxesStockBalance.Insert(syringesAndSafetyBoxesStockBalance);
			}
			else
			{
				HealthFacilitySyringesAndSafetyBoxesStockBalanceInserted = HealthFacilitySyringesAndSafetyBoxesStockBalance.Update(syringesAndSafetyBoxesStockBalance);

			}
			BroadcastStoredHealthFacilityData(healthFacilityId, "UpdateHealthFacilitySyringesAndSafetyBoxesStockBalance");
			IntReturnValue irv = new IntReturnValue();
			irv.id = HealthFacilitySyringesAndSafetyBoxesStockBalanceInserted;
			return irv;
		}



		public List<HealthFacilityBcgOpv0AndTTVaccinations> GetHealthFacilityBcgOpv0AndTTVaccinations(int healthFacilityId, int reportingMonth, int reportingYear)
		{
			List<HealthFacilityBcgOpv0AndTTVaccinations> vaccinations = GIIS.DataLayer.HealthFacilityBcgOpv0AndTTVaccinations.GetHealthFacilityBcgOpv0AndTTVaccinations(healthFacilityId, reportingMonth, reportingYear);
			return vaccinations;
		}

		public List<HealthFacilityBcgOpv0AndTTVaccinations> GetHealthFacilityBcgOpv0AndTTVaccinationsAsList(int healthFacilityId)
		{
			List<HealthFacilityBcgOpv0AndTTVaccinations> vaccinationsList = GIIS.DataLayer.HealthFacilityBcgOpv0AndTTVaccinations.GetHealthFacilityBcgOpv0AndTTVaccinationsAsList(healthFacilityId);
			return vaccinationsList;
		}

		public IntReturnValue StoreHealthFacilityBcgOpv0AndTTVaccinations(int healthFacilityId, int DoseId, int MaleServiceArea, int MaleCatchmentArea, int FemaleServiceArea, int FemaleCatchmentArea, int CoverageServiceArea, int CoverageCatchmentArea, int reportingMonth, int reportingYear, int CoverageCatchmentAndServiceArea, int userId, DateTime modifiedOn)
		{
			HealthFacilityBcgOpv0AndTTVaccinations vaccinations = new HealthFacilityBcgOpv0AndTTVaccinations();

			vaccinations.HealthFacilityId = healthFacilityId;
			vaccinations.DoseId = DoseId;
			vaccinations.MaleServiceArea = MaleServiceArea;
			vaccinations.MaleCatchmentArea = MaleCatchmentArea;
			vaccinations.FemaleServiceArea = FemaleServiceArea;
			vaccinations.FemaleCatchmentArea = FemaleCatchmentArea;
			vaccinations.CoverageServiceArea = CoverageServiceArea;
			vaccinations.CoverageCatchmentArea = CoverageCatchmentArea;
			vaccinations.CoverageCatchmentAndServiceArea = CoverageCatchmentAndServiceArea;

			vaccinations.ReportedMonth = reportingMonth;
			vaccinations.ReportedYear = reportingYear;
			vaccinations.ModifiedOn = modifiedOn;
			vaccinations.ModifiedBy = userId;


			int HealthFacilityBcgOpv0AndTTVaccinationsInserted;
			HealthFacilityBcgOpv0AndTTVaccinations vitaminAStockList = GIIS.DataLayer.HealthFacilityBcgOpv0AndTTVaccinations.GetHealthFacilityBcgOpv0AndTTVaccinationsPerDoseId(healthFacilityId, reportingMonth, reportingYear,DoseId);

			if (vitaminAStockList == null)
			{
				HealthFacilityBcgOpv0AndTTVaccinationsInserted = HealthFacilityBcgOpv0AndTTVaccinations.Insert(vaccinations);
			}
			else
			{
				HealthFacilityBcgOpv0AndTTVaccinationsInserted = HealthFacilityBcgOpv0AndTTVaccinations.Update(vaccinations);

			}
			BroadcastStoredHealthFacilityData(healthFacilityId, "UpdateHealthFacilityBcgOpv0AndTTVaccinations");
			IntReturnValue irv = new IntReturnValue();
			irv.id = HealthFacilityBcgOpv0AndTTVaccinationsInserted;
			return irv;
		}



		/**
		 * Method used to broadcast Stored HealthFacilityColdChains to other tablets within the same facility
		 * 
		 **/
		public string BroadcastStoredHealthFacilityData(int healthFacilityId, string value)
		{



			List<string> regIDs = GetGcmIds(healthFacilityId.ToString());

			string stringregIds = null;

			//Then I use 
			stringregIds = string.Join("\",\"", regIDs);
			//To Join the values (if ever there are more than 1) with quotes and commas for the Json format below

			try
			{
				string GoogleAppID = "AIzaSyBgsthTTTiunMtHV5XT1Im6bl17i5rGR94";
				var SENDER_ID = "967487253557";
				WebRequest tRequest;
				tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
				tRequest.Method = "post";
				tRequest.ContentType = "application/json";
				tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));

				tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

				string postData = "{\"collapse_key\":\"score_update\",\"time_to_live\":108,\"delay_while_idle\":true,\"data\": { \"message\" : " + "\"" + value + "\",\"time\": " + "\"" + System.DateTime.Now.ToString() + "\"},\"registration_ids\":[\"" + stringregIds + "\"]}";

				Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
				tRequest.ContentLength = byteArray.Length;

				Stream dataStream = tRequest.GetRequestStream();
				dataStream.Write(byteArray, 0, byteArray.Length);
				dataStream.Close();

				WebResponse tResponse = tRequest.GetResponse();

				dataStream = tResponse.GetResponseStream();

				StreamReader tReader = new StreamReader(dataStream);

				String sResponseFromServer = tReader.ReadToEnd();

				HttpWebResponse httpResponse = (HttpWebResponse)tResponse;
				httpResponse.StatusCode.ToString();

				tReader.Close();
				dataStream.Close();
				tResponse.Close();


				JObject o = JObject.Parse(sResponseFromServer);
				JArray a = (JArray)o["results"];
				int counter = a.Count;
				for (int i = 0; i < counter; i++)
				{
					if (a[i]["registration_id"] != null)
					{
						ChildManagement.updateGcmId((string)a[i]["registration_id"], regIDs.ElementAt(i));
					}

				}


				return sResponseFromServer;
			}
			catch
			{
				//throw new WebFaultException<string>("Error", HttpStatusCode.ServiceUnavailable);
				return "error sending data to GCM server. Service unavailable";
			}


		}


		public List<string> GetGcmIds(string healthFacilityId)
		{
			DataTable dt = GIIS.DataLayer.VaccinationEvent.GetGcmIds(healthFacilityId);
			List<string> gcmIdsList = new List<string>();
			foreach (DataRow dr in dt.Rows)
			{
				gcmIdsList.Add(dr[0].ToString());
			}

			return gcmIdsList;
		}


		public int DeleteHealthFacilityStockDistributions(int healthFacilityId,DateTime distributionDate, string status)
		{
			int i=HealthFacilityStockDistributions.Delete(healthFacilityId, distributionDate, status);

			return i;
		}


		/**
		 * Method used to receive stock distributed from vims
		 * 
		 **/
		public IntReturnValue receiveDelivery(string jsonRequest)
		{
			int insertValues=0;
			JObject products = JObject.Parse(getVimsProducts());
			JObject dosageUnits = JObject.Parse(getVimsDosageUnits());

			try
			{
				JObject o = JObject.Parse(jsonRequest);

				int fromFacitiyId = (int)o["fromFacilityId"];
				int toFacilityId = HealthFacilityMapper.GetTimrHealthFacilityFacilityId((int)o["toFacilityId"]);
				int programId = (int)o["programId"];
				DateTime distributionDate = (DateTime)o["distributionDate"];
				string distributionType = (o["distributionType"]).ToString();
				string status = (o["status"]).ToString();

				JArray lineItems = (JArray)o["lineItems"];
				int counter = lineItems.Count;
				for (int i = 0; i < counter; i++)
				{
					int itemId = 0;
					string itemName="";
					int alt1QtyPer = 0;
					string manufacturer = "";
					int dosageUnitId = 0;
					string gtin = "";
					int productId = (int)lineItems[i]["productId"];


					JArray productsArray = (JArray)products["products"];
					int count = productsArray.Count;

					for (int p = 0; p < count; p++)
					{
						if (productsArray[p]["id"].Equals(lineItems[i]["productId"]))
						{
							gtin = productsArray[p]["gtin"].ToString();
							manufacturer = productsArray[p]["manufacturer"].ToString();
							dosageUnitId = (int)productsArray[p]["dosageUnitId"];
							alt1QtyPer = (int)productsArray[p]["dosesPerDispensingUnit"];
							itemName = productsArray[p]["primaryName"].ToString();

							Item item = Item.GetItemByName(productsArray[p]["primaryName"].ToString());
							Item itemByCode = Item.GetItemByCode(productsArray[p]["primaryName"].ToString());
							if (item != null)
							{
								itemId = item.Id;
							}
							else if (itemByCode != null)
							{
								itemId = itemByCode.Id;
							}
							break;
						}
					}

					JArray lots = (JArray)lineItems[i]["lots"];
					int lotsCount = lots.Count;
					for (int j = 0; j < lotsCount; j++)
					{
						int lotId;
						int vimsLotId = (int)lots[j]["lotId"];
						int quantity = (int)lots[j]["quantity"];
						string vvmStatus = (string)lots[j]["vvmStatus"];

						ItemLot item = getVimsLotsByProductId(productId,vimsLotId,gtin,itemId,fromFacitiyId);

						ItemLot checkItem = ItemLot.GetItemLotByLotNumber(item.LotNumber);
						int manufacturerId;
						if (checkItem == null)
						{
							Manufacturer man = Manufacturer.GetManufacturerByName(manufacturer);
							if (man == null)
							{
								man = new Manufacturer();
								man.IsActive = true;
								man.Name = manufacturer;
								man.Code = manufacturer;
								manufacturerId = Manufacturer.Insert(man);
							}
							else {
								manufacturerId = man.Id;
							}


							ItemManufacturer itemMan = ItemManufacturer.GetItemManufacturerByGtin(item.Gtin);
							if (itemMan == null)
							{
								itemMan = new ItemManufacturer();
								itemMan.Gtin = item.Gtin;
								itemMan.ItemId = itemId;
								itemMan.IsActive = true;
								itemMan.ManufacturerId = manufacturerId;
								itemMan.Alt1QtyPer = alt1QtyPer;
								itemMan.Notes = itemName;
								itemMan.ModifiedOn = new DateTime();
								itemMan.ModifiedBy = 1;
								itemMan.Alt1Uom = "";

								JArray doseunitsArray = (JArray)dosageUnits["dosage-units"];
								int doseCount = doseunitsArray.Count;
								for (int z = 0; z < doseCount; z++)
								{
									if ((int)doseunitsArray[z]["id"] == dosageUnitId)
									{
										itemMan.BaseUom = doseunitsArray[z]["code"].ToString();
										break;
									}
								}
								ItemManufacturer.Insert(itemMan);

							}


							ItemLot.Insert(item);
							lotId = ItemLot.GetItemLotByLotNumber(item.LotNumber).Id;
						}
						else 
						{
							ItemLot.Update(item);
							lotId = checkItem.Id;
						}


						HealthFacilityStockDistributions distributions = new HealthFacilityStockDistributions();
						distributions.FromHealthFacilityId = fromFacitiyId;
						distributions.ToHealthFacilityId = toFacilityId;
						distributions.ProgramId = programId;
						distributions.Status = status;
						distributions.DistributionDate = distributionDate;
						distributions.DistributionType = distributionType;
						distributions.ProductId = productId;
						distributions.ItemId = itemId;
						distributions.LotId = lotId;
						distributions.VimsLotId = vimsLotId;
						distributions.Quantity = quantity;
						distributions.VvmStatus = vvmStatus;

						insertValues=HealthFacilityStockDistributions.Insert(distributions);

					}
				}

				BroadcastStoredHealthFacilityData(toFacilityId, "UpdateHealthFacilityStockDistributions");
				IntReturnValue irv = new IntReturnValue();
				irv.id = insertValues;
				return irv;
			}
			catch (Exception e)
			{
				//throw new WebFaultException<string>("Error", HttpStatusCode.ServiceUnavailable);
				throw e;
			}


		}





		/**
		 * Method used to receive all vims dosage Units
		 * 
		 **/
		public string getVimsDosageUnits()
		{

			WebRequest webRequest = WebRequest.Create("http://uat.tz.elmis-dev.org:80/rest-api/lookup/dosage-units");
			webRequest.Method = "GET";
			webRequest.ContentType = "application/x-www-form-urlencoded";

			String username = "vims-divo";
			String password = "Admin123";
			String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
			webRequest.Headers.Add("Authorization", "Basic " + encoded);

			WebResponse myWebResponse = webRequest.GetResponse();
			using (Stream stream = myWebResponse.GetResponseStream())
			{
				StreamReader reader = new StreamReader(stream, Encoding.UTF8);
				String responseString = reader.ReadToEnd();
				return responseString;
			}
		}


		/**
		 * Method used to receive all vims products used for mapping of product ids to TIIS itemIds
		 * 
		 **/
		public string getVimsProducts()
		{
			return Program.GetSourceForMyShowsPage("http://uat.tz.elmis-dev.org/rest-api/lookup/products?paging=false");
		}

		/**
		 * Method used to receive all vims products used for mapping of product ids to TIIS itemIds
		 * 
		 **/
		public ItemLot getVimsLotsByProductId(int productId,int vimslotId,string gtin,int itemId,int fromFacilityId)
		{
			String responseString = Program.GetSourceForMyShowsPage("http://uat.tz.elmis-dev.org/api/v2/facilities/"+fromFacilityId+"/stockCards?entries=1&countOnly=false&includeEmptyLots=false");
			JObject o = JObject.Parse(responseString);

			JArray stockCards = (JArray)o["stockCards"];
			int counter = stockCards.Count;
			for (int i = 0; i < counter; i++)
			{

				JArray lotsOnHand = (JArray)stockCards[i]["lotsOnHand"];

				int lotsOnHandCount = lotsOnHand.Count;
				for (int j = 0; j < lotsOnHandCount; j++)
				{
					if (vimslotId == (int)lotsOnHand[j]["lotId"])
					{
						JObject lot = (JObject)lotsOnHand[j]["lot"];

						ItemLot itemLot = new ItemLot();
						itemLot.ExpireDate = (DateTime)lot["expirationDate"];
						itemLot.IsActive = (Boolean)lot["valid"];
						itemLot.LotNumber = (String)lot["lotCode"];
						itemLot.Gtin = gtin;
						itemLot.ItemId = itemId;
						return itemLot;
					}
				}



			}

			Exception e = new Exception("productId = " + productId + ",vimslotId = " + vimslotId + ",gtin=" + gtin + ",itemId=" + itemId);
			throw e;
		}

		public List<HealthFacilityStockDistributions> GetHealthFacilityStockDistributions(int healthFacilityId)
		{
			List<HealthFacilityStockDistributions> stockDistributions = GIIS.DataLayer.HealthFacilityStockDistributions.GetHealthFacilityStockDistributions(healthFacilityId,"PENDING");
			return stockDistributions;
		}

		public List<HealthFacilityStockDistributions> GetAllHealthFacilityStockDistributionsList()
		{
			List<HealthFacilityStockDistributions> stockDistributions = GIIS.DataLayer.HealthFacilityStockDistributions.GetHealthFacilityStockDistributionsList();
			return stockDistributions;
		}


		public int updateHeathFacilityStockDistributions(int fromHealthFacilityId, int toHealthFacilityId, int productId, int lotId, int itemId, string distributionType, DateTime distributionDate, int quantity, string status,int userId)
		{
			HealthFacilityStockDistributions distributions = new HealthFacilityStockDistributions();
			distributions.FromHealthFacilityId = fromHealthFacilityId;
			distributions.ToHealthFacilityId = toHealthFacilityId;
			distributions.Status = status;
			distributions.DistributionDate = distributionDate;
			distributions.DistributionType = distributionType;
			distributions.ProductId = productId;
			distributions.ItemId = itemId;
			distributions.LotId = lotId;
			distributions.Quantity = quantity;


			ItemLot lot = ItemLot.GetItemLotById(lotId);
			new BusinessLogic.StockManagementLogic().Allocate(GetHealthFacilityById(toHealthFacilityId).ElementAt(0), lot.Gtin, lot.LotNumber, quantity, null, userId);
			return HealthFacilityStockDistributions.Update(distributions);;
		}
	}
}