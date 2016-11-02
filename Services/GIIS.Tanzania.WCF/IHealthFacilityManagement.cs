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


		[WebGet(UriTemplate = "StoreHealthFacilityDeseaseSurvailance?healthFacilityId={healthFacilityId}&feverMonthlyCases={feverMonthlyCases}&feverMonthlyDeaths={feverMonthlyDeaths}&AFPMonthlyCases={AFPMonthlyCases}&AFPDeaths={AFPDeaths}&neonatalTTCases={neonatalTTCases}&neonatalTTDeaths={neonatalTTDeaths}&reportingMonth={reportingMonth}&reportingYear={reportingYear}&userId={userId}&modifiedOn={modifiedOn}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		IntReturnValue StoreHealthFacilityDeseaseSurvailance(int healthFacilityId, int feverMonthlyCases, int feverMonthlyDeaths, int AFPMonthlyCases, int AFPDeaths, int neonatalTTCases, int neonatalTTDeaths, int reportingMonth, int reportingYear, int userId, DateTime modifiedOn);


		[WebGet(UriTemplate = "GetHealthFacilityDeseaseSurvailance?healthFacilityId={healthFacilityId}&reportingMonth={reportingMonth}&reportingYear={reportingYear}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityDeseaseSurvailance> GetHealthFacilityDeseaseSurvailance(int healthFacilityId, int reportingMonth, int reportingYear);


		[WebGet(UriTemplate = "GetHealthFacilityDeseaseSurvailanceAsList?healthFacilityId={healthFacilityId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityDeseaseSurvailance> GetHealthFacilityDeseaseSurvailanceAsList(int healthFacilityId);


		[WebGet(UriTemplate = "StoreHealthFacilityImmunizationSessions?healthFacilityId={healthFacilityId}&OutreachPlanned={OutreachPlanned}&OtherMajorImmunizationActivities={OtherMajorImmunizationActivities}&reportingMonth={reportingMonth}&reportingYear={reportingYear}&userId={userId}&modifiedOn={modifiedOn}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		IntReturnValue StoreHealthFacilityImmunizationSessions(int healthFacilityId, int OutreachPlanned, string OtherMajorImmunizationActivities, int reportingMonth, int reportingYear, int userId, DateTime modifiedOn);

		[WebGet(UriTemplate = "GetHealthFacilityImmunizationSessions?healthFacilityId={healthFacilityId}&reportingMonth={reportingMonth}&reportingYear={reportingYear}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityImmunizationSessions> GetHealthFacilityImmunizationSessions(int healthFacilityId, int reportingMonth, int reportingYear);


		[WebGet(UriTemplate = "GetHealthFacilityImmunizationSessionsAsList?healthFacilityId={healthFacilityId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityImmunizationSessions> GetHealthFacilityImmunizationSessionsAsList(int healthFacilityId);



		[WebGet(UriTemplate = "StoreHealthFacilityVitaminAStockBalance?healthFacilityId={healthFacilityId}&VitaminName={VitaminName}&OpeningBalance={OpeningBalance}&Received={Received}&StockInHand={StockInHand}&TotalAdministered={TotalAdministered}&wastage={wastage}&reportingMonth={reportingMonth}&reportingYear={reportingYear}&userId={userId}&modifiedOn={modifiedOn}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		IntReturnValue StoreHealthFacilityVitaminAStockBalance(int healthFacilityId, string VitaminName, int OpeningBalance, int Received, int StockInHand, int TotalAdministered, int wastage, int reportingMonth, int reportingYear, int userId, DateTime modifiedOn);

		[WebGet(UriTemplate = "GetHealthFacilityVitaminAStockBalance?healthFacilityId={healthFacilityId}&reportingMonth={reportingMonth}&reportingYear={reportingYear}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityVitaminAStockBalance> GetHealthFacilityVitaminAStockBalance(int healthFacilityId, int reportingMonth, int reportingYear);


		[WebGet(UriTemplate = "GetHealthFacilityVitaminAStockBalanceAsList?healthFacilityId={healthFacilityId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityVitaminAStockBalance> GetHealthFacilityVitaminAStockBalanceAsList(int healthFacilityId);




		[WebGet(UriTemplate = "StoreHealthFacilitySyringesAndSafetyBoxesStockBalance?healthFacilityId={healthFacilityId}&ItemName={ItemName}&OpeningBalance={OpeningBalance}&Received={Received}&StockInHand={StockInHand}&Used={Used}&wastage={wastage}&StockedOutDays={StockedOutDays}&reportingMonth={reportingMonth}&reportingYear={reportingYear}&userId={userId}&modifiedOn={modifiedOn}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		IntReturnValue StoreHealthFacilitySyringesAndSafetyBoxesStockBalance(int healthFacilityId, string ItemName, int OpeningBalance, int Received, int StockInHand, int Used, int wastage, int StockedOutDays, int reportingMonth, int reportingYear, int userId, DateTime modifiedOn);

		[WebGet(UriTemplate = "GetHealthFacilitySyringesAndSafetyBoxesStockBalance?healthFacilityId={healthFacilityId}&reportingMonth={reportingMonth}&reportingYear={reportingYear}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilitySyringesAndSafetyBoxesStockBalance> GetHealthFacilitySyringesAndSafetyBoxesStockBalance(int healthFacilityId, int reportingMonth, int reportingYear);


		[WebGet(UriTemplate = "GetHealthFacilitySyringesAndSafetyBoxesStockBalanceAsList?healthFacilityId={healthFacilityId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilitySyringesAndSafetyBoxesStockBalance> GetHealthFacilitySyringesAndSafetyBoxesStockBalanceAsList(int healthFacilityId);



		[WebGet(UriTemplate = "StoreHealthFacilityBcgOpv0AndTTVaccinations?healthFacilityId={healthFacilityId}&DoseId={DoseId}&MaleServiceArea={MaleServiceArea}&MaleCatchmentArea={MaleCatchmentArea}&FemaleServiceArea={FemaleServiceArea}&FemaleCatchmentArea={FemaleCatchmentArea}&CoverageServiceArea={CoverageServiceArea}&CoverageCatchmentArea={CoverageCatchmentArea}&CoverageCatchmentAndServiceArea={CoverageCatchmentAndServiceArea}&reportingMonth={reportingMonth}&reportingYear={reportingYear}&userId={userId}&modifiedOn={modifiedOn}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		IntReturnValue StoreHealthFacilityBcgOpv0AndTTVaccinations(int healthFacilityId, int DoseId, int MaleServiceArea, int MaleCatchmentArea, int FemaleServiceArea, int FemaleCatchmentArea, int CoverageServiceArea, int CoverageCatchmentArea, int reportingMonth, int reportingYear, int CoverageCatchmentAndServiceArea, int userId, DateTime modifiedOn);

		[WebGet(UriTemplate = "GetHealthFacilityBcgOpv0AndTTVaccinations?healthFacilityId={healthFacilityId}&reportingMonth={reportingMonth}&reportingYear={reportingYear}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityBcgOpv0AndTTVaccinations> GetHealthFacilityBcgOpv0AndTTVaccinations(int healthFacilityId, int reportingMonth, int reportingYear);


		[WebGet(UriTemplate = "GetHealthFacilityBcgOpv0AndTTVaccinationsAsList?healthFacilityId={healthFacilityId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityBcgOpv0AndTTVaccinations> GetHealthFacilityBcgOpv0AndTTVaccinationsAsList(int healthFacilityId);


		[WebGet(UriTemplate = "getVimsProducts", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		string getVimsProducts();

		[WebGet(UriTemplate = "receiveDelivery?vimsToFacilityId={vimsToFacilityId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		IntReturnValue receiveDelivery(int vimsToFacilityId);

		[WebGet(UriTemplate = "GetHealthFacilityStockDistributions?healthFacilityId={healthFacilityId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityStockDistributions> GetHealthFacilityStockDistributions(int healthFacilityId);


		[WebGet(UriTemplate = "GetAllHealthFacilityStockDistributionsList", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityStockDistributions> GetAllHealthFacilityStockDistributionsList();

		[WebGet(UriTemplate = "DeleteHealthFacilityStockDistributions?healthFacilityId={healthFacilityId}&distributionDate={distributionDate}&status={status}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		int DeleteHealthFacilityStockDistributions(int healthFacilityId, DateTime distributionDate, string status);

		[WebGet(UriTemplate = "updateHeathFacilityStockDistributions?fromHealthFacilityId={fromHealthFacilityId}&toHealthFacilityId={toHealthFacilityId}&productId={productId}&lotId={lotId}&itemId={itemId}&distributionType={distributionType}&distributionDate={distributionDate}&quantity={quantity}&status={status}&userId={userId}&StockDistributionId={StockDistributionId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		int updateHeathFacilityStockDistributions(int fromHealthFacilityId, int toHealthFacilityId, int productId, int lotId, int itemId, string distributionType, DateTime distributionDate, int quantity, string status,int userId,int StockDistributionId);

		[WebGet(UriTemplate = "sendPOD?timrToHealthfacilityId={timrToHealthfacilityId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		string sendPOD(int timrToHealthfacilityId);
	}
}
