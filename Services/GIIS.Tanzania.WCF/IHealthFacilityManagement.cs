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
using System.ServiceModel.Web;
using System.Text;

namespace GIIS.Tanzania.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IHealthFacilityManagement" in both code and config file together.
    [ServiceContract]
    public interface IHealthFacilityManagement
    {
        [WebGet(UriTemplate = "GetHealthFacilityByParentId?parentId={parentId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<HealthFacility> GetHealthFacilityByParentId(int parentId);

        [WebGet(UriTemplate = "GetHealthFacilityById?id={id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<HealthFacility> GetHealthFacilityById(int id);

		[WebGet(UriTemplate = "GetHealthFacilities", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacility> GetHealthFacilities();


        [WebGet(UriTemplate = "GetHealthFacilityByList?hList={hList}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<HealthFacility> GetHealthFacilityByList(string hList);


		[WebGet(UriTemplate = "UpdateHealthFacilityCumulativeChildSn?healthFacilityId={healthFacilityId}&cumulativeChildSn={cumulativeChildSn}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		int UpdateHealthFacilityCumulativeChildSn(int healthFacilityId, int cumulativeChildSn);

		[WebGet(UriTemplate = "GetCumulativeChildId?healthFacilityId={healthFacilityId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		int GetCumulativeChildId(int healthFacilityId);


		[WebGet(UriTemplate = "GetHealthFacilityVaccinations?healthFacilityId={healthFacilityId}&fromDate={fromDate}&toDate={toDate}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		HealthFacilityVaccinationsEntity GetHealthFacilityVaccinations(int healthFacilityId, DateTime fromDate, DateTime toDate);

		[WebGet(UriTemplate = "GetHealthFacilitiesVaccinations?fromDate={fromDate}&toDate={toDate}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityVaccinationsEntity> GetHealthFacilitiesVaccinations(DateTime fromDate, DateTime toDate);




		[WebGet(UriTemplate = "StoreHealthFacilityColdChain?healthFacilityId={healthFacilityId}&tempMax={tempMax}&tempMin={tempMin}&alarmHighTemp={alarmHighTemp}&alarmLowTemp={alarmLowTemp}&reportingMonth={reportingMonth}&reportingYear={reportingYear}&userId={userId}&modifiedOn={modifiedOn}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		IntReturnValue StoreHealthFacilityColdChain(int healthFacilityId, double tempMax, double tempMin, int alarmHighTemp, int alarmLowTemp, int reportingMonth, int reportingYear, int userId, DateTime modifiedOn);


		[WebGet(UriTemplate = "GetHealthFacilityColdChain?healthFacilityId={healthFacilityId}&reportingMonth={reportingMonth}&reportingYear={reportingYear}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityColdChain> GetHealthFacilityColdChain(int healthFacilityId, int reportingMonth, int reportingYear);


		[WebGet(UriTemplate = "GetHealthFacilityColdChainAsList?healthFacilityId={healthFacilityId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityColdChain> GetHealthFacilityColdChainAsList(int healthFacilityId);

	}
}
