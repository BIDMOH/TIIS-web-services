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

        public List<HealthFacility> GetHealthFacilityByList(string hList)
        {
            return HealthFacility.GetHealthFacilityByList(hList);
        }

		public int updateHealthFacilityCumulativeChildSn(int healthFacilityId, int cumulativeChildSn)
		{
			return GIIS.DataLayer.HealthFacility.updateHealthFacilityCumulativeChildSn(healthFacilityId, cumulativeChildSn);
		}

    }
}