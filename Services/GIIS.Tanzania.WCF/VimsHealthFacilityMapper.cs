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
using System.Collections.Specialized;

namespace GIIS.Tanzania.WCF
{
	class HealthFacilityMapper
	{
		private static string facility_mapping = "[{'name':'Usariver GVT','vims_id':16394,'timr_id':17515},{'name':'Kaloleni HC','vims_id':16333,'timr_id':19472},{'name':'Kikatiti','vims_id':19221,'timr_id':19465},{'name':' Sombetini','vims_id':16753,'timr_id':13502},{'name':'St. Elizabeth','vims_id':16810,'timr_id':13909}]";

		public static int GetVimsHealthFacilityFacilityId(int TimrId)
		{
			JArray facilityMapping = JArray.Parse(facility_mapping);
			int count = facilityMapping.Count;
			for (int i = 0; i < count; i++) 
			{
				if (TimrId==((int)facilityMapping[i]["timr_id"]))
				{
					return ((int)facilityMapping[i]["vims_id"]);
				}
			}
			Exception e = new Exception("Mapping for  timr_id = "+TimrId+" does not exist");
			throw e;
		}


		public static int GetTimrHealthFacilityFacilityId(int VimsId)
		{
			JArray facilityMapping = JArray.Parse(facility_mapping);
			int count = facilityMapping.Count;
			for (int i = 0; i < count; i++)
			{
				if (VimsId==((int)facilityMapping[i]["vims_id"]))
				{
					return ((int)facilityMapping[i]["timr_id"]);
				}
			}
			Exception e = new Exception("Mapping for  vims_id = " + VimsId + " does not exist");
			throw e;
		}

	}
}