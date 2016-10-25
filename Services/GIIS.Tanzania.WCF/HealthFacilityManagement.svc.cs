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
					vaccinationEntity.serviceAreaMale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, true, false);
					vaccinationEntity.serviceAreaFemale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, false, false);
					vaccinationEntity.catchmentMale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, true, true);
					vaccinationEntity.catchmentFemale = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByGenderAndCatchment(healthFacility.Id, d.Id, fromDate, toDate, false, true);

					DateTime cummulativeFrom = new DateTime(Int32.Parse(DateTime.Now.Year.ToString()), 1, 1);
					vaccinationEntity.serviceAreaCummulativeTotal = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByCatchment(healthFacility.Id, d.Id, cummulativeFrom, toDate, false);
					vaccinationEntity.catchmentAreaCummulativeTotal = GIIS.DataLayer.HealthFacility.GetHealthFacilityVaccinationsByCatchment(healthFacility.Id, d.Id, cummulativeFrom, toDate, true);

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


	}
}