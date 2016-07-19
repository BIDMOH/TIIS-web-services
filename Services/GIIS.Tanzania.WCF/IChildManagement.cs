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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IChildManagement" in both code and config file together.
    [ServiceContract]
    public interface IChildManagement
    {
        [WebGet(UriTemplate = @"RegisterChildWithoutAppointments?barcodeid={barcodeid}&firstname1={firstname1}&lastname1={lastname1}&birthdate={birthdate}&gender={gender}&healthFacilityId={healthFacilityId}&birthplaceId={birthplaceId}&domicileId={domicileId}&address={address}&phone={phone}&motherFirstname={motherFirstname}&motherLastname={motherLastname}&notes={notes}&userId={userId}&modifiedOn={modifiedOn}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue RegisterChildWithoutAppointments(string barcodeId, string firstname1, string lastname1, DateTime birthdate, bool gender,
            int healthFacilityId, int birthplaceId, int domicileId, string address, string phone, string motherFirstname,
            string motherLastname, string notes, int userId, DateTime modifiedOn);



		[WebGet(UriTemplate = @"RegisterChildWithoutAppointmentsWithMothersHivStatusAndTT2VaccineStatus?barcodeid={barcodeid}&firstname1={firstname1}&lastname1={lastname1}&birthdate={birthdate}&gender={gender}&healthFacilityId={healthFacilityId}&birthplaceId={birthplaceId}&domicileId={domicileId}&address={address}&phone={phone}&motherFirstname={motherFirstname}&motherLastname={motherLastname}&mothersHivStatus={mothersHivStatus}&mothersTT2Status={mothersTT2Status}&notes={notes}&userId={userId}&modifiedOn={modifiedOn}&childCumulativeSn={childCumulativeSn}&childRegistryYear={childRegistryYear}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		IntReturnValue RegisterChildWithoutAppointmentsWithMothersHivStatusAndTT2VaccineStatus(string barcodeId, string firstname1, string lastname1, DateTime birthdate, bool gender,
			int healthFacilityId, int birthplaceId, int domicileId, string address, string phone, string motherFirstname,
			string motherLastname,string mothersHivStatus, string mothersTT2Status, int childCumulativeSn, int childRegistryYear, string notes, int userId, DateTime modifiedOn);


    
        [WebGet(UriTemplate = @"RegisterChildWithAppoitments?barcodeid={barcodeid}&firstname1={firstname1}&lastname1={lastname1}&birthdate={birthdate}&gender={gender}&healthFacilityId={healthFacilityId}&birthplaceId={birthplaceId}&domicileId={domicileId}&address={address}&phone={phone}&motherFirstname={motherFirstname}&motherLastname={motherLastname}&notes={notes}&userId={userId}&modifiedOn={modifiedOn}&firstname2={firstname2}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue RegisterChildWithAppoitments(string barcodeId, string firstname1, string firstname2, string lastname1, DateTime birthdate, bool gender,
            int healthFacilityId, int birthplaceId, int domicileId, string address, string phone, string motherFirstname,
            string motherLastname, string notes, int userId, DateTime modifiedOn);


		[WebGet(UriTemplate = @"RegisterChildWithAppoitmentsWithMothersHivStatusAndTT2VaccineStatus?barcodeid={barcodeid}&firstname1={firstname1}&lastname1={lastname1}&birthdate={birthdate}&gender={gender}&healthFacilityId={healthFacilityId}&birthplaceId={birthplaceId}&domicileId={domicileId}&address={address}&phone={phone}&motherFirstname={motherFirstname}&motherLastname={motherLastname}&mothersHivStatus={mothersHivStatus}&mothersTT2Status={mothersTT2Status}&notes={notes}&userId={userId}&modifiedOn={modifiedOn}&firstname2={firstname2}&childCumulativeSn={childCumulativeSn}&childRegistryYear={childRegistryYear}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		IntReturnValue RegisterChildWithAppoitmentsWithMothersHivStatusAndTT2VaccineStatus(string barcodeId, string firstname1, string firstname2, string lastname1, DateTime birthdate, bool gender,
			int healthFacilityId, int birthplaceId, int domicileId, string address, string phone, string motherFirstname,
			string motherLastname,string mothersHivStatus, string mothersTT2Status,int childCumulativeSn,int childRegistryYear, string notes, int userId, DateTime modifiedOn);



        [WebGet(UriTemplate = @"UpdateChild?barcode={barcode}&firstname1={firstname1}&lastname1={lastname1}&birthdate={birthdate}&gender={gender}&healthFacilityId={healthFacilityId}&birthplaceId={birthplaceId}&domicileId={domicileId}&statusid={statusid}&address={address}&phone={phone}&motherFirstname={motherFirstname}&motherLastname={motherLastname}&notes={notes}&userId={userId}&childId={childId}&firstname2={firstname2}&modifiedOn={modifiedOn}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue UpdateChild(string barcode, string firstname1, string firstname2, string lastname1, DateTime birthdate, bool gender,
            int healthFacilityId, int birthplaceId, int domicileId, int statusId, string address, string phone, string motherFirstname,
            string motherLastname, string notes, int userId, int childId, DateTime modifiedOn);


		[WebGet(UriTemplate = @"UpdateChildWithMothersHivStatusAndTT2VaccineStatus?barcode={barcode}&firstname1={firstname1}&lastname1={lastname1}&birthdate={birthdate}&gender={gender}&healthFacilityId={healthFacilityId}&birthplaceId={birthplaceId}&domicileId={domicileId}&statusid={statusid}&address={address}&phone={phone}&motherFirstname={motherFirstname}&motherLastname={motherLastname}&mothersHivStatus={mothersHivStatus}&mothersTT2Status={mothersTT2Status}&notes={notes}&userId={userId}&childId={childId}&firstname2={firstname2}&modifiedOn={modifiedOn}&mothersHivStatus={mothersHivStatus}&mothersTT2Status={mothersTT2Status}&notes={notes}&userId={userId}&modifiedOn={modifiedOn}&childCumulativeSn={childCumulativeSn}&childRegistryYear={childRegistryYear}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		IntReturnValue UpdateChildWithMothersHivStatusAndTT2VaccineStatus(string barcode, string firstname1, string firstname2, string lastname1, DateTime birthdate, bool gender,
			int healthFacilityId, int birthplaceId, int domicileId, int statusId, string address, string phone, string motherFirstname,
			string motherLastname,string mothersHivStatus, string mothersTT2Status, int childCumulativeSn, int childRegistryYear, string notes, int userId, int childId, DateTime modifiedOn);


        [WebGet(UriTemplate = "RemoveChild?id={id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue RemoveChild(int id);

        [WebGet(UriTemplate = "DeleteChild?id={id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue DeleteChild(int id);

        [WebGet(UriTemplate = "FindDublication?birthdateFlag={birthdateFlag}&firstnameFlag={firstnameFlag}&genderFlag={genderFlag}&healthFacilityId={healthFacilityId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Child> FindDublication(bool birthdateFlag, bool firstnameFlag, bool genderFlag, int healthFacilityId);

        [WebGet(UriTemplate = "Search?where={where}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<ChildResults> Search(string where);

        [WebGet(UriTemplate = "GetChildByHealthFacilityId?healthFacilityId={healthFacilityId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Child> GetChildByHealthFacilityId(int healthFacilityId);

        [WebGet(UriTemplate = "GetChildrenByHealthFacility?healthFacilityId={healthFacilityId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<ChildEntity> GetChildrenByHealthFacility(int healthFacilityId);

        [WebGet(UriTemplate = "GetOnlyChildrenByHealthFacility?healthFacilityId={healthFacilityId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Child> GetOnlyChildrenByHealthFacility(int healthFacilityId);

        [WebGet(UriTemplate = "GetOnlyChildrenDataByHealthFacility?healthFacilityId={healthFacilityId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        ChildListEntity GetOnlyChildrenDataByHealthFacility(int healthFacilityId);

        [WebGet(UriTemplate = "GetChildrenByHealthFacilitySinceLastLogin?idUser={idUser}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        ChildListEntity GetChildrenByHealthFacilitySinceLastLogin(int idUser);

        [WebGet(UriTemplate = "GetChildrenByHealthFacilityBeforeLastLogin?idUser={idUser}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        ChildListEntity GetChildrenByHealthFacilityBeforeLastLogin(int idUser);

        [WebGet(UriTemplate = "GetChildrenByHealthFacilityDayFirstLogin?idUser={idUser}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        ChildListEntity GetChildrenByHealthFacilityDayFirstLogin(int idUser);

        [WebGet(UriTemplate = "SearchByBarcode?barcodeId={barcodeId}", ResponseFormat = WebMessageFormat.Json)] 
        [OperationContract]
        List<ChildEntity> SearchByBarcode(string barcodeId);

        [WebGet(UriTemplate = "SearchByTempId?tempid={tempid}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<ChildEntity> SearchByTempId(string tempid);

		[WebGet(UriTemplate = "SearchByNameAndMother?firstname1={firstname1}&lastname1={lastname1}&motherfirstname={motherfirstname}&motherlastname={motherlastname}&mothersHivStatus={mothersHivStatus}&mothersTT2Status={mothersTT2Status}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
		List<ChildEntity> SearchByNameAndMother(string firstname1, string lastname1, string motherfirstname, string motherlastname,string mothersHivStatus,string mothersTT2Status);

        [WebGet(UriTemplate = "SearchByName?firstname1={firstname1}&lastname1={lastname1}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<ChildEntity> SearchByName(string firstname1, string lastname1);

        [WebGet(UriTemplate = "SearchByDate?birthdatefrom={birthdatefrom}&birthdateto={birthdateto}&birthplaceid={birthplaceid}&domicileid={domicileid}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<ChildEntity> SearchByDate(DateTime birthdatefrom, DateTime birthdateto, int birthplaceid, int domicileid);

        [WebGet(UriTemplate = "ChildExistsByLastnameAndBirthdateAndGender?lastname1={lastname1}&gender={gender}&birthdate={birthdate}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        bool ChildExistsByLastnameAndBirthdateAndGender(string lastname1, string gender, DateTime birthdate);

        [WebGet(UriTemplate = "ChildExistsByLastnameAndBirthdate?lastname1={lastname1}&birthdate={birthdate}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        bool ChildExistsByLastnameAndBirthdate(string lastname1, DateTime birthdate);

        //[WebGet(UriTemplate = "ChildExistsByMotherAndBirthdate?motherFirstname={motherFirstname}&motherLastname={motherLastname}&birthdate={birthdate}", ResponseFormat = WebMessageFormat.Json)]
        //[OperationContract]
        //bool ChildExistsByMotherAndBirthdate(string motherFirstname, string motherLastname, DateTime birthdate);

        [WebGet(UriTemplate = "ChildExistsByMotherAndBirthdateAndGender?lastname1={lastname1}&motherFirstname={motherFirstname}&motherLastname={motherLastname}&gender={gender}&birthdate={birthdate}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        bool ChildExistsByMotherAndBirthdateAndGender(string lastname1, string motherFirstname, string motherLastname, string gender, DateTime birthdate);


        [WebGet(UriTemplate = "ChildExists?where={where}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        bool ChildExists(string where);
        


        [WebGet(UriTemplate = "RegisterChildWeight?childid={childid}&date={date}&weight={weight}&modifiedon={modifiedon}&modifiedby={modifiedby}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue RegisterChildWeight(int childId, DateTime date, double weight, DateTime modifiedOn, int modifiedBy);

        [WebGet(UriTemplate = "RegisterChildWeightBarcode?barcode={barcode}&date={date}&weight={weight}&modifiedon={modifiedon}&modifiedby={modifiedby}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue RegisterChildWeightBarcode(string barcode, DateTime date, double weight, DateTime modifiedOn, int modifiedBy);

        [WebGet(UriTemplate = "GetChildById?childid={childid}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<ChildEntity> GetChildById(int childId);

        [WebGet(UriTemplate = "GetChildByIdList?childIdList={childIdList}&userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        ChildListEntity GetChildByIdList(string childIdList, int userId);

        [WebGet(UriTemplate = "GetChildByIdListSince?childIdList={childIdList}&userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        ChildListEntity GetChildByIdListSince(string childIdList, int userId);

        [WebGet(UriTemplate = "GetChildByBarcodeList?childList={childList}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<ChildEntity> GetChildByBarcodeList(string childList);


        [WebGet(UriTemplate = "GetWeight", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Weight> GetWeight();

		[WebGet(UriTemplate = "GetGcmIdsToSendDataModifiedChild?childId={childId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<string> GetGcmIdsToSendDataModifiedChild(int childId);

		//contract for a test GCM function
		[WebGet(UriTemplate = "BroadcastChildUpdates?childId={childId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		string BroadcastChildUpdates(int childId);


		[WebGet(UriTemplate = "BroadcastChildUpdatesWithBarcodeId?barcodeId={barcodeId}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		string BroadcastChildUpdatesWithBarcodeId(string barcodeId);
    }
}
