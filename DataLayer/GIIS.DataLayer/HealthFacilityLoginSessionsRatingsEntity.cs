﻿//*******************************************************************************
//Copyright 2015 TIIS - Tanzania Immunization Information System
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
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public class HealthFacilityLoginSessionsRatings
    {
        public Int32 HealthFacilityId { get; set; }
		public string Name { get; set; }
		public Int32 SessionsCount { get; set; }
		public string ApkVersion { get; set; }

		public Int32 DaysMaximum { get; set; }
		public Int32 DaysMinimum { get; set; }
		public Int32 ChildrenVaccinationsMaximumThreshold { get; set; }
		public Int32 ChildrenVaccinationsMinimumThreshold { get; set; }
		public Int32 ChildrenRegistrationsMaximumThreshold { get; set; }
		public Int32 ChildrenRegistrationsMinimumThreshold { get; set; }
    }
}
