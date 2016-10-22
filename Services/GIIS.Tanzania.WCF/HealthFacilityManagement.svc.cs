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

	}
}