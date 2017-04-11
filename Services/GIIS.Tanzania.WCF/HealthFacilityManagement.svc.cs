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

					if (vaccinationEntity.antigen.Equals("BCG") || (vaccinationEntity.antigen.Equals("bOPV") && vaccinationEntity.dose == 0))
					{
						
						try
						{
							HealthFacilityBcgOpv0AndTTVaccinations v = GIIS.DataLayer.HealthFacilityBcgOpv0AndTTVaccinations.GetHealthFacilityBcgOpv0AndTTVaccinationsByDoseId(healthFacilityId, d.Id, fromDate.Month, fromDate.Year);
						
							if (v.MaleServiceArea == 0 && v.FemaleServiceArea == 0 && v.MaleCatchmentArea == 0 && v.FemaleCatchmentArea == 0)
							{
								//Else use the values from child vaccinations data within the vaccination event tables of the facilities
								vaccinationEntity.serviceAreaMale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, true, false);
								vaccinationEntity.serviceAreaFemale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, false, false);
								vaccinationEntity.catchmentMale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, true, true);
								vaccinationEntity.catchmentFemale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, false, true);

							}
							else { 
								
								//check if the monthly report data for BCG or OPV0 vaccinations from the tablets have been filled (This data is filled from the tablet into the BcgOpv0AndTTVaccinations table), if so use the value from the monthly reports i.e BcgOpv0AndTTVaccinations table, and send it back to vims if not use the value from vaccination events
								vaccinationEntity.serviceAreaMale = v.MaleServiceArea;
								vaccinationEntity.serviceAreaFemale = v.FemaleServiceArea;
								vaccinationEntity.catchmentMale = v.MaleCatchmentArea;
								vaccinationEntity.catchmentFemale = v.FemaleCatchmentArea;
							}
						}
						catch (Exception e) { }
					}
					else if (vaccinationEntity.antigen.Equals("Tetanus Toxoid")){
						try
						{
							HealthFacilityBcgOpv0AndTTVaccinations v = GIIS.DataLayer.HealthFacilityBcgOpv0AndTTVaccinations.GetHealthFacilityBcgOpv0AndTTVaccinationsByDoseId(healthFacilityId, d.Id, fromDate.Month, fromDate.Year);
							vaccinationEntity.serviceAreaMale = v.MaleServiceArea;
							vaccinationEntity.serviceAreaFemale = v.FemaleServiceArea;
							vaccinationEntity.catchmentMale = v.MaleCatchmentArea;
							vaccinationEntity.catchmentFemale = v.FemaleCatchmentArea;
						}
						catch (Exception e) { }
					}else {
						vaccinationEntity.serviceAreaMale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, true, false);
						vaccinationEntity.serviceAreaFemale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, false, false);
						vaccinationEntity.catchmentMale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, true, true);
						vaccinationEntity.catchmentFemale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, false, true);

					}

					vaccinationsList.Add(vaccinationEntity);
				}
			}


			//TODO remove the hardcoded ids and create a mechanism of obtaining this ids.
			VaccinationsEntity vaccinationEntitytt1 = new VaccinationsEntity();
			HealthFacilityBcgOpv0AndTTVaccinations vtt1 = GIIS.DataLayer.HealthFacilityBcgOpv0AndTTVaccinations.GetHealthFacilityBcgOpv0AndTTVaccinationsByDoseId(healthFacilityId, 79, fromDate.Month, fromDate.Year);
			vaccinationEntitytt1.antigen = "Tetanus Toxoid";
			vaccinationEntitytt1.dose = 1;
			if (vtt1 != null)
			{
				vaccinationEntitytt1.serviceAreaMale = vtt1.MaleServiceArea;
				vaccinationEntitytt1.serviceAreaFemale = vtt1.FemaleServiceArea;
				vaccinationEntitytt1.catchmentMale = vtt1.MaleCatchmentArea;
				vaccinationEntitytt1.catchmentFemale = vtt1.FemaleCatchmentArea;
			}
			vaccinationsList.Add(vaccinationEntitytt1);



			VaccinationsEntity vaccinationEntitytt2 = new VaccinationsEntity();
			HealthFacilityBcgOpv0AndTTVaccinations vtt2 = GIIS.DataLayer.HealthFacilityBcgOpv0AndTTVaccinations.GetHealthFacilityBcgOpv0AndTTVaccinationsByDoseId(healthFacilityId, 80, fromDate.Month, fromDate.Year);
			vaccinationEntitytt2.antigen = "Tetanus Toxoid";
			vaccinationEntitytt2.dose = 2;
			if (vtt2 != null)
			{
				vaccinationEntitytt2.serviceAreaMale = vtt2.MaleServiceArea;
				vaccinationEntitytt2.serviceAreaFemale = vtt2.FemaleServiceArea;
				vaccinationEntitytt2.catchmentMale = vtt2.MaleCatchmentArea;
				vaccinationEntitytt2.catchmentFemale = vtt2.FemaleCatchmentArea;
			}
			vaccinationsList.Add(vaccinationEntitytt2);


			VaccinationsEntity vaccinationEntitytt3 = new VaccinationsEntity();
			HealthFacilityBcgOpv0AndTTVaccinations vtt3 = GIIS.DataLayer.HealthFacilityBcgOpv0AndTTVaccinations.GetHealthFacilityBcgOpv0AndTTVaccinationsByDoseId(healthFacilityId, 81, fromDate.Month, fromDate.Year);
			vaccinationEntitytt3.antigen = "Tetanus Toxoid";
			vaccinationEntitytt3.dose = 3;

			if (vtt3 != null)
			{
				vaccinationEntitytt3.serviceAreaMale = vtt3.MaleServiceArea;
				vaccinationEntitytt3.serviceAreaFemale = vtt3.FemaleServiceArea;
				vaccinationEntitytt3.catchmentMale = vtt3.MaleCatchmentArea;
				vaccinationEntitytt3.catchmentFemale = vtt3.FemaleCatchmentArea;
			}
			vaccinationsList.Add(vaccinationEntitytt3);



			VaccinationsEntity vaccinationEntitytt4 = new VaccinationsEntity();
			HealthFacilityBcgOpv0AndTTVaccinations vtt4 = GIIS.DataLayer.HealthFacilityBcgOpv0AndTTVaccinations.GetHealthFacilityBcgOpv0AndTTVaccinationsByDoseId(healthFacilityId, 82, fromDate.Month, fromDate.Year);
			vaccinationEntitytt4.antigen = "Tetanus Toxoid";
			vaccinationEntitytt4.dose = 4;
			if (vtt4 != null)
			{
				vaccinationEntitytt4.serviceAreaMale = vtt4.MaleServiceArea;
				vaccinationEntitytt4.serviceAreaFemale = vtt4.FemaleServiceArea;
				vaccinationEntitytt4.catchmentMale = vtt4.MaleCatchmentArea;
				vaccinationEntitytt4.catchmentFemale = vtt4.FemaleCatchmentArea;
			}
			vaccinationsList.Add(vaccinationEntitytt4);


			VaccinationsEntity vaccinationEntitytt5 = new VaccinationsEntity();
			HealthFacilityBcgOpv0AndTTVaccinations vtt5 = GIIS.DataLayer.HealthFacilityBcgOpv0AndTTVaccinations.GetHealthFacilityBcgOpv0AndTTVaccinationsByDoseId(healthFacilityId, 83, fromDate.Month, fromDate.Year);
			vaccinationEntitytt5.antigen = "Tetanus Toxoid";
			vaccinationEntitytt5.dose = 5;

			if (vtt5 != null)
			{
				vaccinationEntitytt5.serviceAreaMale = vtt5.MaleServiceArea;
				vaccinationEntitytt5.serviceAreaFemale = vtt5.FemaleServiceArea;
				vaccinationEntitytt5.catchmentMale = vtt5.MaleCatchmentArea;
				vaccinationEntitytt5.catchmentFemale = vtt5.FemaleCatchmentArea;
			}
			vaccinationsList.Add(vaccinationEntitytt5);


			VaccinationsEntity vaccinationEntitytt6 = new VaccinationsEntity();
			HealthFacilityBcgOpv0AndTTVaccinations vtt6 = GIIS.DataLayer.HealthFacilityBcgOpv0AndTTVaccinations.GetHealthFacilityBcgOpv0AndTTVaccinationsByDoseId(healthFacilityId, 84, fromDate.Month, fromDate.Year);
			vaccinationEntitytt6.antigen = "Tetanus Toxoid";
			vaccinationEntitytt6.dose = 6;
			if (vtt6 != null)
			{
				vaccinationEntitytt6.serviceAreaMale = vtt6.MaleServiceArea;
				vaccinationEntitytt6.serviceAreaFemale = vtt6.FemaleServiceArea;
				vaccinationEntitytt6.catchmentMale = vtt6.MaleCatchmentArea;
				vaccinationEntitytt6.catchmentFemale = vtt6.FemaleCatchmentArea;
			}
			vaccinationsList.Add(vaccinationEntitytt6);




			entity.healthFacilityVaccinations = vaccinationsList;
			entity.healthFacilityName = healthFacility.Name;
			entity.healthFacilityId = healthFacility.Id;
			return entity;
		}

		public List<HealthFacilityColdChain> GetHealthFacilityColdChain(int healthFacilityId, int reportingMonth, int reportingYear)
		{
			List <HealthFacilityColdChain> coldChain = GIIS.DataLayer.HealthFacilityColdChain.GetHealthFacilityColdChain(healthFacilityId, reportingMonth,reportingYear);


			JObject vimsOperationStatusObject = JObject.Parse(Program.GetSourceForMyShowsPage("/equipment/type/operational-status"));
			JArray operationStatusArray = (JArray)vimsOperationStatusObject["status"];
			int operationStatusArrayCount = operationStatusArray.Count;
			int functionalStatusId = 0;

			for (int i = 0; i < operationStatusArrayCount; i++)
			{
				if (operationStatusArray[i]["name"].Equals("Functional"))
				{
					functionalStatusId = (int)operationStatusArray[i]["id"];
				}
			}

			//JObject vimsReportDetailsObject = JObject.Parse(Program.GetSourceForMyShowsPage("/vaccine/report/get/" + reportId + ".json"));
			//JArray coldChainLineItems = (JArray)vimsReportDetailsObject["report"]["coldChainLineItems"];



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


		public string SendColdChainToVims(int vimsFacilityId, double tempMax, double tempMin, int alarmHighTemp, int alarmLowTemp)
		{
			JObject vimsOperationStatusObject = JObject.Parse(Program.GetSourceForMyShowsPage("/equipment/type/operational-status"));


			JObject obj = JObject.Parse(Program.GetSourceForMyShowsPage("/rest-api/lookup/program/Vaccine"));
			int programId = (int)obj["program"]["id"];
			JObject vimsReportsPeriodsObject = JObject.Parse(Program.GetSourceForMyShowsPage("/vaccine/report/periods/"+vimsFacilityId+"/"+programId));

			JArray vimsReportsPeriods = (JArray)vimsReportsPeriodsObject["periods"];
			int periodsCount = vimsReportsPeriods.Count;
			int reportId = -1;
			for (int i = 0; i < periodsCount; i++)
			{
				if (vimsReportsPeriods[i]["status"].ToString().Equals("DRAFT"))
				{
					reportId = (int)vimsReportsPeriods[i]["id"];
				}
			}

			if (reportId != -1)
			{
				JObject vimsReportDetailsObject = JObject.Parse(Program.GetSourceForMyShowsPage("/vaccine/report/get/" + reportId+".json"));
				JArray coldChainLineItems = (JArray)vimsReportDetailsObject["report"]["coldChainLineItems"];



				JArray operationStatusArray = (JArray)vimsOperationStatusObject["status"];
				int operationStatusArrayCount = operationStatusArray.Count;
				int functionalStatusId = 0;

				for (int i = 0; i < operationStatusArrayCount; i++)
				{
					if (operationStatusArray[i]["name"].Equals("Functional"))
					{
						functionalStatusId = (int)operationStatusArray[i]["id"];
					}
				}


				JObject facilityEquipmentsObject = JObject.Parse(Program.GetSourceForMyShowsPage("/equipment/inventory/rest-api/EquipmentInventory/{facilityId}/{programId}?facilityId=" + vimsFacilityId + "&programId=" + programId));
				JArray equipments = (JArray)facilityEquipmentsObject["Equipments"];

				int count = equipments.Count;

				for (int i = 0; i < count; i++)
				{
					JObject equipmentUpdateObj = new JObject();
					equipmentUpdateObj.Add("id", equipments[i]["id"]);
					equipmentUpdateObj.Add("facilityId", equipments[i]["facilityId"]);
					equipmentUpdateObj.Add("programId", programId);
					equipmentUpdateObj.Add("equipmentId", equipments[i]["equipmentId"]);
					equipmentUpdateObj.Add("equipment", equipments[i]["equipment"]);
					equipmentUpdateObj.Add("facility", vimsReportDetailsObject["report"]["facility"]);
					equipmentUpdateObj.Add("operationalStatusId", functionalStatusId);//ask
					equipmentUpdateObj.Add("notFunctionalStatusId", 0);//ask
					equipmentUpdateObj.Add("serialNumber", equipments[i]["serialNumber"]);
					equipmentUpdateObj.Add("yearOfInstallation", equipments[i]["yearOfInstallation"]);
					equipmentUpdateObj.Add("purchasePrice", equipments[i]["purchasePrice"]);
					equipmentUpdateObj.Add("sourceOfFund", equipments[i]["sourceOfFund"]);
					equipmentUpdateObj.Add("replacementRecommended", equipments[i]["replacementRecommended"]);
					equipmentUpdateObj.Add("reasonForReplacement", equipments[i]["reasonForReplacement"]);
					equipmentUpdateObj.Add("nameOfAssessor", equipments[i]["nameOfAssessor"]);
					equipmentUpdateObj.Add("primaryDonorId", equipments[i]["primaryDonorId"]);
					equipmentUpdateObj.Add("isActive", equipments[i]["isActive"]);
					equipmentUpdateObj.Add("dateDecommissioned", equipments[i]["dateDecommissioned"]);
					equipmentUpdateObj.Add("dateLastAssessed", new DateTime());
					equipmentUpdateObj.Add("hasStabilizer", equipments[i]["hasStabilizer"]);
					equipmentUpdateObj.Add("nameOfSparePart", equipments[i]["nameOfSparePart"]);
					equipmentUpdateObj.Add("equipmentInventoryId", equipments[i]["equipmentInventoryId"]);


					JArray coldChainLines = new JArray();
					JObject coldChainItem = new JObject();
					coldChainItem.Add("id", coldChainLineItems[i]["id"]);
					coldChainItem.Add("skipped", coldChainLineItems[i]["skipped"]);
					coldChainItem.Add("reportId", reportId);
					coldChainItem.Add("equipmentInventoryId", equipments[i]["equipmentInventoryId"]);

					coldChainItem.Add("minTemp", tempMin);
					coldChainItem.Add("maxTemp", tempMax);
					coldChainItem.Add("minEpisodeTemp", alarmLowTemp);
					coldChainItem.Add("maxEpisodeTemp", alarmHighTemp);
					coldChainItem.Add("remarks", "");
					coldChainItem.Add("operationalStatusId", functionalStatusId);
					coldChainItem.Add("equipmentName", equipments[i]["equipment"]["name"]);

					coldChainItem.Add("type", equipments[i]["equipment"]["equipmentType"]["name"]);
					coldChainItem.Add("model", equipments[i]["equipment"]["model"]);
					coldChainItem.Add("energySource", equipments[i]["equipment"]["energyType"]["name"]);
					coldChainItem.Add("serial", coldChainLineItems[i]["serialNumber"]);
					coldChainItem.Add("location_value", coldChainLineItems[i]["location_value"]);

					coldChainLines.Add(coldChainItem);
					equipmentUpdateObj.Add("coldChainLineItems", coldChainLines);

					return equipmentUpdateObj.ToString();

					Program.PostJsonToUrl(Program.url+"/equipment/inventory/rest-api/EquipmentInventory/update", equipmentUpdateObj.ToString());

				}
			}

			return "success";
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

		public IntReturnValue StoreHealthFacilityImmunizationSessions(int healthFacilityId,int fixedConducted,int outreachCanceled,int outreachConducted, int OutreachPlanned, string OtherMajorImmunizationActivities, int reportingMonth, int reportingYear, int userId, DateTime modifiedOn)
		{
			HealthFacilityImmunizationSessions deseaseSurvailance = new HealthFacilityImmunizationSessions();

			deseaseSurvailance.HealthFacilityId = healthFacilityId;
			deseaseSurvailance.OutreachPlanned = OutreachPlanned;
			deseaseSurvailance.OtherMajorImmunizationActivities = OtherMajorImmunizationActivities;
			deseaseSurvailance.FixedConducted = fixedConducted;
			deseaseSurvailance.OutreachConducted = outreachConducted;
			deseaseSurvailance.OutreachCanceled = outreachCanceled;
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
		 * Method used to broadcast messages to  devices within the same facility as the facility Id
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


		public IntReturnValue StoreHealthFacilityLoginSessions(int userId, int healthFacilityId, DateTime loginTime, int sessionLength)
		{
			HealthFacilitySessions healthFacilityLoginSessions = new HealthFacilitySessions();

			healthFacilityLoginSessions.UserId = userId;
			healthFacilityLoginSessions.HealthFacilityId = healthFacilityId;
			healthFacilityLoginSessions.LoginTime = loginTime;
			healthFacilityLoginSessions.SessionLength = sessionLength;
			healthFacilityLoginSessions.ApkVersion = "Below current version";

			int HealthFacilitySessionsInserted;
			HealthFacilitySessionsInserted = GIIS.DataLayer.HealthFacilitySessions.Insert(healthFacilityLoginSessions);

			IntReturnValue irv = new IntReturnValue();
			irv.id = HealthFacilitySessionsInserted;
			return irv;
		}


		public IntReturnValue StoreHealthFacilityLoginSessionsAndAppVersion(int userId, int healthFacilityId, DateTime loginTime, int sessionLength, string appVersion)
		{
			HealthFacilitySessions healthFacilityLoginSessions = new HealthFacilitySessions();

			healthFacilityLoginSessions.UserId = userId;
			healthFacilityLoginSessions.HealthFacilityId = healthFacilityId;
			healthFacilityLoginSessions.LoginTime = loginTime;
			healthFacilityLoginSessions.SessionLength = sessionLength;
			healthFacilityLoginSessions.ApkVersion = appVersion;

			int HealthFacilitySessionsInserted;
			HealthFacilitySessionsInserted = GIIS.DataLayer.HealthFacilitySessions.Insert(healthFacilityLoginSessions);

			IntReturnValue irv = new IntReturnValue();
			irv.id = HealthFacilitySessionsInserted;
			return irv;
		}


#region vims helper methods and api service for receiving stock, broadcasting stock tickle message to the facilities, allow health facilities to obtain the data and send Proof Of Delivery from the facility back to VIMS
		public int DeleteHealthFacilityStockDistributions(int healthFacilityId,DateTime distributionDate, string status)
		{
			int i=HealthFacilityStockDistributions.Delete(healthFacilityId, distributionDate, status);

			return i;
		}
		/**
		 * Method triggered periodically by the andoid client apps used to check for new stock from vims
		 */
		public IntReturnValue checkForNewStockFromVims(int healthFacilityId)
		{
			return receiveDelivery(HealthFacilityMapper.GetVimsHealthFacilityFacilityId(healthFacilityId));
		}


		/**
		 * Method used to receive stock distributed from vims
		 * 
		 **/
		public IntReturnValue receiveDelivery(int vimsToFacilityId)
		{

			int insertValues=0;
			JObject obj = JObject.Parse(Program.GetSourceForMyShowsPage("/vaccine/inventory/distribution/distribution-supervisorid/" + vimsToFacilityId));
			JObject o = (JObject)obj["distribution"];

			if (o["status"].ToString().Equals("PENDING"))
			{
				int fromFacitiyId = (int)o["fromFacilityId"];

				int va = (int)o["toFacilityId"];

				int toFacilityId = HealthFacilityMapper.GetTimrHealthFacilityFacilityId(va);
				int programId = 0;
				try
				{
					programId = (int)o["programId"];
				}
				catch
				{
					//setting programId to 0;
					programId = 0;
				}
				DateTime distributionDate = (DateTime)o["distributionDate"];
				string distributionType = (o["distributionType"]).ToString();

				JArray lineItems = (JArray)o["lineItems"];
				int counter = lineItems.Count;
				for (int i = 0; i < counter; i++)
				{
					int itemId = 0;
					string itemName = "";
					int alt1QtyPer = 0;
					string manufacturer = "";
					int dosageUnitId = 0;
					string gtin = "";
					int productId = (int)lineItems[i]["productId"];

					itemName = lineItems[i]["product"]["primaryName"].ToString();
					dosageUnitId = (int)lineItems[i]["product"]["dosageUnit"]["id"];
					alt1QtyPer = (int)lineItems[i]["product"]["dosesPerDispensingUnit"];

					//throw new Exception("success");

					if (itemName.Equals("Safety boxes"))
					{
						gtin = "17";
						manufacturer = "Haffkine";
						alt1QtyPer = 1;
					} 
					else
					{

						try
						{
							manufacturer = lineItems[i]["product"]["manufacturer"].ToString();
						}
						catch
						{
							manufacturer = "Unknown Manufacturer";
						}

						try
						{
							gtin = lineItems[i]["product"]["gtin"].ToString();
						}
						catch
						{
							gtin = "";
						}

					}

					Item item = Item.GetItemByName(itemName);
					Item itemByCode = Item.GetItemByCode(itemName);
					if (item != null)
					{
						itemId = item.Id;
					}
					else if (itemByCode != null)
					{
						itemId = itemByCode.Id;
					}
					//else {
					//	throw new Exception("Item "+productsArray[p]["primaryName"].ToString()+" is not in server please configure the item");
					//}


					if (itemName.Equals("Safety boxes"))
					{
						//HACK FOR HANDLING STOCK TRANSFER FOR SAFETY BOXES
						int lotId;
						int vimsLotId = 0;
						int quantity = (int)lineItems[i]["quantity"];
						int stockDistributionId = (int)lineItems[i]["id"];
						string vvmStatus = null;


						ItemLot itemLot = new ItemLot();
						itemLot.ExpireDate = new DateTime(2100, 12, 31);
						itemLot.IsActive = true;
						itemLot.LotNumber = "";
						itemLot.Gtin = gtin;
						itemLot.ItemId = itemId;
						int manufacturerId;

						ItemLot checkItem = ItemLot.GetItemLotByLotNumber(itemLot.LotNumber);
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
							else
							{
								manufacturerId = man.Id;
							}


							ItemManufacturer itemMan = ItemManufacturer.GetItemManufacturerByGtin(itemLot.Gtin);
							if (itemMan == null)
							{
								itemMan = new ItemManufacturer();
								itemMan.Gtin = itemLot.Gtin;
								itemMan.ItemId = itemId;
								itemMan.IsActive = true;
								itemMan.ManufacturerId = manufacturerId;
								itemMan.Alt1QtyPer = alt1QtyPer;
								itemMan.Notes = itemName;
								itemMan.ModifiedOn = new DateTime();
								itemMan.ModifiedBy = 1;
								itemMan.Alt1Uom = "";
								itemMan.BaseUom = lineItems[i]["product"]["dosageUnit"]["code"].ToString();
								ItemManufacturer.Insert(itemMan);

							}


							ItemLot.Insert(itemLot);
							lotId = ItemLot.GetItemLotByLotNumber(itemLot.LotNumber).Id;
						}
						else
						{
							ItemLot.Update(itemLot);
							lotId = checkItem.Id;
						}
						HealthFacilityStockDistributions distributions = new HealthFacilityStockDistributions();
						distributions.FromHealthFacilityId = fromFacitiyId;
						distributions.ToHealthFacilityId = toFacilityId;
						distributions.ProgramId = programId;
						distributions.Status = "PENDING";
						distributions.DistributionDate = distributionDate;
						distributions.DistributionType = distributionType;
						distributions.ProductId = productId;
						distributions.ItemId = itemId;
						distributions.LotId = lotId;
						distributions.VimsLotId = vimsLotId;
						distributions.StockDistributionId = stockDistributionId;
						distributions.Quantity = quantity;
						distributions.VvmStatus = vvmStatus;

						insertValues = HealthFacilityStockDistributions.Insert(distributions);

					}
					else
					{
						//handling other vaccines and syringes
						JArray lots = (JArray)lineItems[i]["lots"];
						int lotsCount = lots.Count;
						for (int j = 0; j < lotsCount; j++)
						{
							int lotId;
							int vimsLotId = (int)lots[j]["lotId"];
							int quantity = (int)lots[j]["quantity"];
							int stockDistributionId = (int)lots[j]["id"];
							string vvmStatus = "";
							try
							{
								vvmStatus = (string)lots[j]["vvmStatus"];
							}
							catch
							{
								vvmStatus = "null";
							}


							ItemLot itemLot = new ItemLot();
							itemLot.ExpireDate = (DateTime)lots[j]["lot"]["expirationDate"];
							itemLot.IsActive = (Boolean)lots[j]["lot"]["valid"];
							itemLot.LotNumber = (String)lots[j]["lot"]["lotCode"];
							itemLot.Gtin = gtin;
							itemLot.ItemId = itemId;

							if (itemLot == null)
							{
								throw new Exception("itemId = " + itemId);
							}

							ItemLot checkItem = ItemLot.GetItemLotByLotNumber(itemLot.LotNumber);
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
								else
								{
									manufacturerId = man.Id;
								}


								ItemManufacturer itemMan = ItemManufacturer.GetItemManufacturerByGtin(itemLot.Gtin);
								if (itemMan == null)
								{
									itemMan = new ItemManufacturer();
									itemMan.Gtin = itemLot.Gtin;
									itemMan.ItemId = itemId;
									itemMan.IsActive = true;
									itemMan.ManufacturerId = manufacturerId;
									itemMan.Alt1QtyPer = alt1QtyPer;
									itemMan.Notes = itemName;
									itemMan.ModifiedOn = new DateTime();
									itemMan.ModifiedBy = 1;
									itemMan.Alt1Uom = "";
									itemMan.BaseUom = lineItems[i]["product"]["dosageUnit"]["code"].ToString(); ;
									ItemManufacturer.Insert(itemMan);

								}


								ItemLot.Insert(itemLot);
								lotId = ItemLot.GetItemLotByLotNumber(itemLot.LotNumber).Id;
							}
							else
							{
								ItemLot.Update(itemLot);
								lotId = checkItem.Id;
							}


							HealthFacilityStockDistributions distributions = new HealthFacilityStockDistributions();
							distributions.FromHealthFacilityId = fromFacitiyId;
							distributions.ToHealthFacilityId = toFacilityId;
							distributions.ProgramId = programId;
							distributions.Status = "PENDING";
							distributions.DistributionDate = distributionDate;
							distributions.DistributionType = distributionType;
							distributions.ProductId = productId;
							distributions.ItemId = itemId;
							distributions.LotId = lotId;
							distributions.VimsLotId = vimsLotId;
							distributions.StockDistributionId = stockDistributionId;
							distributions.Quantity = quantity;
							distributions.VvmStatus = vvmStatus;
							distributions.DosesPerDispensingUnit = alt1QtyPer;

							insertValues = HealthFacilityStockDistributions.Insert(distributions);
						}
					}
				}

				if (insertValues > 0)
				{
					BroadcastStoredHealthFacilityData(toFacilityId, "newHealthFacilityStockDistributions");
				}
				IntReturnValue irv = new IntReturnValue();
				irv.id = insertValues;
				return irv;
			}
			IntReturnValue irv2 = new IntReturnValue();
			irv2.id = -1;
			return irv2;


		}





		/**
		 * Method used to receive all vims dosage Units
		 * 
		 **/
		public string getVimsDosageUnits()
		{
			return Program.GetSourceForMyShowsPage("/rest-api/lookup/dosage-units");
		}


		/**
		 * Method used to receive all vims products used for mapping of product ids to TIIS itemIds
		 * 
		 **/
		public string getVimsProducts()
		{
			return Program.GetSourceForMyShowsPage("/rest-api/lookup/products?paging=false");
		}


		public List<HealthFacilityStockDistributions> GetHealthFacilityStockDistributions(int healthFacilityId)
		{
			List<HealthFacilityStockDistributions> stockDistributions = GIIS.DataLayer.HealthFacilityStockDistributions.GetHealthFacilityStockDistributions(healthFacilityId);
			return stockDistributions;
		}

		public List<HealthFacilityStockDistributions> GetAllHealthFacilityStockDistributionsList()
		{
			List<HealthFacilityStockDistributions> stockDistributions = GIIS.DataLayer.HealthFacilityStockDistributions.GetHealthFacilityStockDistributionsList();
			return stockDistributions;
		}

		public List<CoverageReportEntity> GetDistrictCoverageReport(string scheduledVaccinationName, int healthFacilityId, DateTime fromDate, DateTime toDate)
		{
			List<CoverageReportEntity> stockDistributions = GIIS.DataLayer.CoverageReportEntity.GetDistrictCoverageReport(scheduledVaccinationName, healthFacilityId, fromDate, toDate,true);
			return stockDistributions;
		} 


		public int updateHeathFacilityStockDistributions(int fromHealthFacilityId, int toHealthFacilityId, int productId, int lotId, int itemId, string distributionType, DateTime distributionDate, int quantity, string status,int userId,int StockDistributionId)
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
			distributions.StockDistributionId = StockDistributionId;
			distributions.Quantity = quantity;


			int updateResults = HealthFacilityStockDistributions.UpdatePending(distributions);
			if (updateResults > 0)
			{
				ItemTransaction transaction = new ItemTransaction();
				try
				{
					ItemLot lot = ItemLot.GetItemLotById(lotId);

					//Checks if there is a valid LotItem by Id. For Safety Boxes this may be null since safety boxes have no lot
					if (lot.LotNumber != null && !lot.LotNumber.Equals("") && !lot.Gtin.Equals(""))
					{
						transaction = new BusinessLogic.StockManagementLogic().Allocate(GetHealthFacilityById(toHealthFacilityId).ElementAt(0), lot.Gtin, lot.LotNumber, quantity, null, userId);
					}
				}
				catch { }

				//Checks if all stock distributions have been received from the mobile app before sending the POD back to vims
				List<HealthFacilityStockDistributions> stockDistributions = GIIS.DataLayer.HealthFacilityStockDistributions.GetHealthFacilityStockDistributionsByStatus(toHealthFacilityId, "PENDING");
				if (stockDistributions.Count == 0)
				{
					sendPOD(toHealthFacilityId);
					BroadcastStoredHealthFacilityData(toHealthFacilityId, "proofOfDeliverySentSuccessfully");
					receiveDelivery(HealthFacilityMapper.GetVimsHealthFacilityFacilityId(toHealthFacilityId));

				}
				return transaction.Id;

			}

			return 0;
		}


		public string sendPOD(int timrToHealthfacilityId)
		{
			//obtaining the original sent stock delivery from vims inorder to obtain more data to be returned in the POD responce.
			JObject obj = JObject.Parse(Program.GetSourceForMyShowsPage("/vaccine/inventory/distribution/distribution-supervisorid/" + HealthFacilityMapper.GetVimsHealthFacilityFacilityId(timrToHealthfacilityId)));
			try
			{
				JObject o = (JObject)obj["distribution"];

				JObject podObject = new JObject();
				podObject.Add("id", o["id"]);
				podObject.Add("programId", o["programId"]);
				podObject.Add("status", "RECEIVED");
				podObject.Add("toFacilityId", HealthFacilityMapper.GetVimsHealthFacilityFacilityId(timrToHealthfacilityId));

				JArray lineItems = new JArray();

				JArray lineItemsOrg = (JArray)o["lineItems"];
				int counter = lineItemsOrg.Count;
				for (int i = 0; i < counter; i++)
				{
					JObject itemObj = new JObject();
					itemObj.Add("productId", lineItemsOrg[i]["productId"]);
					itemObj.Add("id", lineItemsOrg[i]["id"]);

					JArray lots = new JArray();

					JArray lotsOrg = (JArray)lineItemsOrg[i]["lots"];
					int lotsCounter = lotsOrg.Count;
					int quantity = 0;
					for (int j = 0; j < lotsCounter; j++)
					{
						JObject lotItem = new JObject();
						lotItem.Add("lotId", lotsOrg[j]["lotId"]);
						lotItem.Add("id", lotsOrg[j]["id"]);
						lotItem.Add("vvmStatus", lotsOrg[j]["vvmStatus"]);

						HealthFacilityStockDistributions distribution = HealthFacilityStockDistributions.GetHealthFacilityStockDistributionsById((int)lotsOrg[j]["id"]);

						if (distribution == null)
						{
							throw new Exception("StockDistributionId = " + (int)lotsOrg[j]["id"] + ", productId = " + (int)lineItemsOrg[i]["productId"] + ",toFacilityId = " + timrToHealthfacilityId + ",distributionDate = " + o["distributionDate"].ToString() + ",status=RECEIVED");
						}
						lotItem.Add("quantity", distribution.Quantity);
						quantity += distribution.Quantity;
						lots.Add(lotItem);
					}
					itemObj.Add("lots", lots);

					if (lots.Count == 0)
					{
						//handling safety boxes since they have no Id
						HealthFacilityStockDistributions distribution = HealthFacilityStockDistributions.GetHealthFacilityStockDistributionsById((int)lineItemsOrg[i]["id"]);
						if (distribution == null)
						{
							throw new Exception("StockDistributionIdForSafetyBoxes = " + (int)lineItemsOrg[i]["id"] + ", productId = " + (int)lineItemsOrg[i]["productId"] + ",toFacilityId = " + timrToHealthfacilityId + ",distributionDate = " + o["distributionDate"].ToString() + ",status=RECEIVED");
						}
						else {
							itemObj.Add("quantity", distribution.Quantity);
						}
					}
					else {
						itemObj.Add("quantity", quantity);
					}

					lineItems.Add(itemObj);
				}
				podObject.Add("lineItems", lineItems);



				string postUrl = Program.url+"/vaccine/inventory/distribution/save.json";
				return Program.PostJsonToUrl(postUrl, podObject.ToString());
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		#endregion


		#region vims connection apis for obtaining Vitamin A supplimentations per health facility

		public VitaminASupplimentation GetChildSupplementsByChild(int healthFacilityId,DateTime fromTime, DateTime toTime) 
		{
			return VitaminASupplimentation.GetChildSupplementsByChild(healthFacilityId, fromTime, toTime);
		}

		#endregion




		#region vims connection api service for obtaining AEFI reporting data

		public List<HealthFacilityAEFI> GetHealthFacilityAEFIList(int healthFacilityId, DateTime fromTime, DateTime toTime)
		{
			return HealthFacilityAEFI.GetHealthFacilityAEFIList(fromTime, toTime,healthFacilityId);
		}

		#endregion

	}
}