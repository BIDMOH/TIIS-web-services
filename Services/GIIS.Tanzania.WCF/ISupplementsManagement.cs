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
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GIIS.Tanzania.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISupplementsManagement" in both code and config file together.
    [ServiceContract]
    public interface ISupplementsManagement
    {
        [WebGet(UriTemplate = "RegisterSupplements?childid={childid}&vita={vita}&mebendezol={mebendezol}&date={date}&modifiedby={modifiedby}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue RegisterSupplements(int childId, bool vita, bool mebendezol, DateTime date, int modifiedBy);

        [WebGet(UriTemplate = "RegisterSupplementsBarcode?barcode={barcode}&vita={vita}&mebendezol={mebendezol}&date={date}&modifiedby={modifiedby}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue RegisterSupplementsBarcode(string barcode, bool vita, bool mebendezol, DateTime date, int modifiedBy);

        [WebGet(UriTemplate = "UpdateSupplements?vita={vita}&mebendezol={mebendezol}&date={date}&modifiedby={modifiedby}&id={id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int UpdateSupplements(bool vita, bool mebendezol, DateTime date, int modifiedBy, int id);
    }
}
