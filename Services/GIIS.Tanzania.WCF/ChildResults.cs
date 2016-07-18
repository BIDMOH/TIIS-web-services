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
using System.Linq;
using System.Web;

namespace GIIS.Tanzania.WCF
{
    public class ChildResults
    {
        public Int32 Id { get; set; }
        public string Firstname1 { get; set; }
        public string Lastname1 { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Gender { get; set; }
        public string HealthcenterId { get; set; }
        public string DomicileId { get; set; }
        public string MotherFirstname { get; set; }
        public string MotherLastname { get; set; }
		public string MothersHivStatus { get; set;}
		public string MothersTT2Status { get; set; }
        public string BarcodeId { get; set; }
        public string Firstname2 { get; set; }
    }
}