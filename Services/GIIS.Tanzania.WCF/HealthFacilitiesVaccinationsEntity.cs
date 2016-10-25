﻿//*******************************************************************************
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
using System.Linq;
using System.Web;

namespace GIIS.Tanzania.WCF
{
    public class VaccinationsEntity
    {
		public string antigen { get; set; }
		public int dose { get; set; }
		public int balance { get; set; }
		public int serviceAreaMale { get; set; }
		public int serviceAreaFemale { get; set; }
		public int catchmentMale { get; set; }
		public int catchmentFemale { get; set; }
		public int serviceAreaCummulativeTotal { get; set; }
		public int catchmentAreaCummulativeTotal { get; set; }
    }

	public class HealthFacilityVaccinationsEntity
	{
		public List<VaccinationsEntity> healthFacilityVaccinations { get; set; }
		public string healthFacilityName { get; set; }
		public int healthFacilityId { get; set; }
	}
}