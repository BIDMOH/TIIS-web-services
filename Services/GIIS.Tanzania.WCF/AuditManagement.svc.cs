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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AuditManagement" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AuditManagement.svc or AuditManagement.svc.cs at the Solution Explorer and start debugging.
    public class AuditManagement : IAuditManagement
    {
        public IntReturnValue RegisterAudit (string table, string recordId, int userId, DateTime date, int activityId)
        {
            //AuditTable at = new AuditTable();
            //at.DbTable = table;
            //at.RecordIdOnTable = recordId;
            //at.UserId = userId;
            //at.Date = date;
            //at.ActivityId = activityId;

            int inserted = AuditTable.InsertEntity(table, recordId, activityId, date, userId);

            IntReturnValue irv = new IntReturnValue();
            irv.id = inserted;
            return irv;
        }

        public List<Config> GetConfiguration()
        {
            List<Config> config = new List<Config>();

            Configuration days = Configuration.GetConfigurationByName("LimitNumberOfDaysBeforeExpire");
            Config nodays = new Config();
            nodays.key = days.Name;
            nodays.value = days.Value;
            config.Add(nodays);

            Configuration vaccination = Configuration.GetConfigurationByName("EligibleForVaccination");
            Config vacc = new Config();
            vacc.key = vaccination.Name;
            vacc.value = vaccination.Value;
            config.Add(vacc);

            return config;
        }
    }
}
