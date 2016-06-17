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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAuditManagement" in both code and config file together.
    [ServiceContract]
    public interface IAuditManagement
    {
        [WebGet(UriTemplate = "RegisterAudit?table={table}&recordId={recordId}&userId={userId}&date={date}&activityId={activityId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue RegisterAudit(string table, string recordId, int userId, DateTime date, int activityId);

        [WebGet(UriTemplate = "GetConfiguration", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Config> GetConfiguration();

    }
}
