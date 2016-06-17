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
    public class BalanceEntity
    {
        public string Item { get; set; }
        public string Gtin { get; set; }
        public string LotNumber { get; set; }
        public DateTime ExpireDate { get; set; }
        public string LotId { get; set; }
        public string Balance { get; set; }
        public string ReorderQty { get; set; }
        public Boolean Gtin_IsActive { get; set; }
        public Boolean Lot_IsActive { get; set; }
    }
}