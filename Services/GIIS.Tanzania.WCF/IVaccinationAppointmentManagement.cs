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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IVaccinationAppointmentManagement" in both code and config file together.
    [ServiceContract]
    public interface IVaccinationAppointmentManagement
    {
        [WebGet(UriTemplate = "RegisterVaccinationAppointment?childId={childId}&scheduledFacilityId={scheduledFacilityId}&scheduledDate={scheduledDate}&notes={notes}&isActive={isActive}&userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue RegisterVaccinationAppointment(int childId, int scheduledFacilityId, DateTime scheduledDate, string notes, bool isActive, int userId);

        [WebGet(UriTemplate = "UpdateVaccinationAppointment?childId={childId}&scheduledFacilityId={scheduledFacilityId}&scheduledDate={scheduledDate}&notes={notes}&isActive={isActive}&userId={userId}&vaccinationAppointmentId={vaccinationAppointmentId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue UpdateVaccinationAppointment(int childId, int scheduledFacilityId, DateTime scheduledDate, string notes, bool isActive, int userId,
            int vaccinationAppointmentId);

        [WebGet(UriTemplate = "UpdateVaccinationApp?outreach={outreach}&userId={userId}&barcode={barcode}&doseId={doseId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue UpdateVaccinationApp(bool outreach, int userId, string barcode, int doseId);

        [WebGet(UriTemplate = "RemoveVaccinationAppointment?id={id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int RemoveVaccinationAppointment(int id);

        [WebGet(UriTemplate = "DeleteVaccinationAppointment?id={id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int DeleteVaccinationAppointment(int id);

        [WebGet(UriTemplate = "GetVaccinationAppointmentsByChildId?childId={childId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<VaccinationAppointment> GetVaccinationAppointmentsByChildId(int childId);

        [WebGet(UriTemplate = "RegisterChildAEFI?appId={appId}&aefi={aefi}&date={date}&notes={notes}&modifiedon={modifiedon}&modifiedby={modifiedby}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue RegisterChildAEFI(int appId, bool aefi, DateTime date, string notes, DateTime modifiedOn, int modifiedBy);

        [WebGet(UriTemplate = "RegisterChildAEFIBarcode?barcode={barcode}&doseId={doseId}&aefi={aefi}&date={date}&notes={notes}&modifiedon={modifiedon}&modifiedby={modifiedby}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue RegisterChildAEFIBarcode(string barcode, int doseId, bool aefi, DateTime date, string notes, DateTime modifiedOn, int modifiedBy);

    }
}
