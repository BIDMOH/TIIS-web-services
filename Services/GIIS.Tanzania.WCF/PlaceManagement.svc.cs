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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PlaceManagement" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PlaceManagement.svc or PlaceManagement.svc.cs at the Solution Explorer and start debugging.
    public class PlaceManagement : IPlaceManagement
    {
        public List<Place> GetPlaceByParentId(int parentId)
        {
            int? pId = parentId;
            List<Place> placeList = Place.GetPlaceByParentId(pId);

            return placeList;
        }

        public List<Place> GetPlaceByHealthFacilityId(int hf_id)
        {
            int? pId = hf_id;
            List<Place> placeList = Place.GetPlaceByHealthFacilityId(pId);
            Place p = Place.GetPlaceById(-100);
            placeList.Add(p);

            return placeList;
        }

        public List<Place> GetPlaceById(int id)
        {
            Place p = Place.GetPlaceById(id);
            List<Place> tmp = new List<Place>();
            tmp.Add(p);

            return tmp;
        }

        public List<Place> GetLeafPlaces()
        {
            List<Place> lp = Place.GetLeafPlaces();

            return lp;

        }

        public List<Place> GetPlacesByHealthFacilityIdSinceLastLogin(int userId)
        {
            return Place.GetPlacesByHealthFacilityIdSinceLastLogin(userId);
        }

        public List<Place> GetPlacesByList(string pList)
        {
            return Place.GetPlacesByList(pList);
        }

        public List<Birthplace> GetBirthplaceList()
        {
            List<Birthplace> lb = Birthplace.GetBirthplaceList();
            return lb;
        }
    }
}
