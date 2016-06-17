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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPlaceManagement" in both code and config file together.
    [ServiceContract]
    public interface IPlaceManagement
    {
        [WebGet(UriTemplate = "GetPlaceByParentId?parentId={parentId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Place> GetPlaceByParentId(int parentId);

        //todo: njesoj por me hf_id
        [WebGet(UriTemplate = "GetPlaceByHealthFacilityId?hf_id={hf_id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Place> GetPlaceByHealthFacilityId(int hf_id);

        [WebGet(UriTemplate = "GetPlaceById?id={id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Place> GetPlaceById(int id);

        [WebGet(UriTemplate = "GetLeafPlaces", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Place> GetLeafPlaces();

        [WebGet(UriTemplate = "GetPlacesByHealthFacilityIdSinceLastLogin?userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Place> GetPlacesByHealthFacilityIdSinceLastLogin(int userId);

        [WebGet(UriTemplate = "GetPlacesByList?pList={pList}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Place> GetPlacesByList(string pList);

        [WebGet(UriTemplate = "GetBirthplaceList", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Birthplace> GetBirthplaceList();
    }
}
