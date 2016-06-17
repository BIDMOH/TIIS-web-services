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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GIIS.Tanzania.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IVaccinationEvent" in both code and config file together.
    [ServiceContract]
    public interface IVaccinationEvent
    {
        [WebGet(UriTemplate = "RegisterVaccinationEvent?appointmentId={appointmentId}&childId={childId}&doseId={doseId}&vaccineLotId={vaccineLotId}&vaccineLotText={vaccineLotText}&healthFacilityId={healthFacilityId}&scheduledDate={scheduledDate}&vaccinationDate={vaccinationDate}&notes={notes}&vaccinationStatus={vaccinationStatus}&nonvaccinationReasonId={nonvaccinationReasonId}&isActive={isActive}&userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue RegisterVaccinationEvent(int appointmentId, int childId, int doseId, int vaccineLotId, string vaccineLotText, int healthFacilityId,
            DateTime scheduledDate, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId, bool isActive, int userId);

        [WebGet(UriTemplate = "RegisterVaccinationEvent2?appointmentId={appointmentId}&childId={childId}&doseId={doseId}&healthFacilityId={healthFacilityId}&scheduledDate={scheduledDate}&vaccinationDate={vaccinationDate}&notes={notes}&isActive={isActive}&userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue RegisterVaccinationEvent2(int appointmentId, int childId, int doseId, int healthFacilityId,
           DateTime scheduledDate, DateTime vaccinationDate, string notes, bool isActive, int userId);


        [WebGet(UriTemplate = "UpdateVaccinationEvent?vaccineLotId={vaccineLotId}&healthFacilityId={healthFacilityId}&vaccinationDate={vaccinationDate}&notes={notes}&vaccinationStatus={vaccinationStatus}&nonvaccinationReasonId={nonvaccinationReasonId}&userId={userId}&vaccinationEventId={vaccinationEventId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue UpdateVaccinationEvent(int vaccineLotId, int healthFacilityId, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId, int userId, int vaccinationEventId);

        [WebGet(UriTemplate = "UpdateVaccinationEventById?vaccinationEventId={vaccinationEventId}&vaccinationDate={vaccinationDate}&notes={notes}&vaccinationStatus={vaccinationStatus}&nonvaccinationReasonId={nonvaccinationReasonId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue UpdateVaccinationEventById(int vaccinationEventId, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId);

        [WebGet(UriTemplate = "UpdateVaccinationEventChildIdAndDoseId?childId={childId}&doseId={doseId}&vaccineLotId={vaccineLotId}&healthFacilityId={healthFacilityId}&vaccinationDate={vaccinationDate}&notes={notes}&vaccinationStatus={vaccinationStatus}&nonvaccinationReasonId={nonvaccinationReasonId}&userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue UpdateVaccinationEventChildIdAndDoseId(int childId, int doseId, int vaccineLotId, int healthFacilityId, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId, int userId);

        [WebGet(UriTemplate = "UpdateVaccinationEventBarcodeAndDoseId?barcode={barcode}&doseId={doseId}&vaccineLotId={vaccineLotId}&healthFacilityId={healthFacilityId}&vaccinationDate={vaccinationDate}&notes={notes}&vaccinationStatus={vaccinationStatus}&nonvaccinationReasonId={nonvaccinationReasonId}&userId={userId}&modifiedOn={modifiedOn}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue UpdateVaccinationEventBarcodeAndDoseId(string barcode, int doseId, int vaccineLotId, int healthFacilityId, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId, int userId,DateTime modifiedOn);


        [WebGet(UriTemplate = "UpdateVaccinationEventByBarcode?barcode={barcode}&doseId={doseId}&vaccineLotId={vaccineLotId}&healthFacilityId={healthFacilityId}&vaccinationDate={vaccinationDate}&notes={notes}&vaccinationStatus={vaccinationStatus}&nonvaccinationReasonId={nonvaccinationReasonId}&userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue UpdateVaccinationEventByBarcode(string barcode, int doseId, int vaccineLotId, int healthFacilityId, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId, int userId);

        [WebGet(UriTemplate = "UpdateVaccinationEventByBarcodeVaccine?barcodeId={barcodeId}&vaccineId={vaccineId}&vaccinelot={vaccinelot}&healthFacilityId={healthFacilityId}&vaccinationDate={vaccinationDate}&notes={notes}&vaccinationStatus={vaccinationStatus}&nonvaccinationReasonId={nonvaccinationReasonId}&userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue UpdateVaccinationEventByBarcodeVaccine(string barcodeId, int vaccineId, int vaccinelot, int healthFacilityId, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId, int userId);


        [WebGet(UriTemplate = "UpdateVaccinationEventByBarcodeAndAppId?barcodeId={barcodeId}&appointmentId={appointmentId}&vaccinationDate={vaccinationDate}&notes={notes}&vaccinationStatus={vaccinationStatus}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue UpdateVaccinationEventByBarcodeAndAppId(string barcodeId, int appointmentId, DateTime vaccinationDate, string notes, bool vaccinationStatus);

        [WebGet(UriTemplate = "RemoveVaccinationEvent?id={id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int RemoveVaccinationEvent(int id);

        [WebGet(UriTemplate = "DeleteVaccinationEvent?id={id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int DeleteVaccinationEvent(int id);

        [WebGet(UriTemplate = "GetMonthlyPlan?healthcenterId={healthcenterId}&currentDate={currentDate}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<GIIS.DataLayer.VaccinationEvent> GetMonthlyPlan(int healthcenterId, DateTime currentDate);

        [WebGet(UriTemplate = "GetImmunizationCard?childId={childId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<GIIS.DataLayer.VaccinationEvent> GetImmunizationCard(int childId);

        [WebGet(UriTemplate = "GetVaccinationEventListByChildId?childId={childId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<GIIS.DataLayer.VaccinationEvent> GetVaccinationEventListByChildId(int childId);

        [WebGet(UriTemplate = "UpdateVaccinationQueue?barcode={barcode}&hfId={hfId}&date={date}&userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue UpdateVaccinationQueue(string barcode, int hfId, DateTime date, int userId);

        [WebGet(UriTemplate = "GetDueVaccinesForChild?childId={childId}&scheduledDate={scheduledDate}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<VaccinationEventEntity> GetDueVaccinesForChild(int childId, DateTime scheduledDate);

        [WebGet(UriTemplate = "GetDefaultersList", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<VaccinationEventEntity> GetDefaultersList();

        [WebGet(UriTemplate = "GetDefaulters", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<IntReturnValue> GetDefaulters();
    }
}