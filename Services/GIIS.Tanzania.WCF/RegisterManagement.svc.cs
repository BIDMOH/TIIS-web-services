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
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GIIS.Tanzania.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RegisterManagement" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RegisterManagement.svc or RegisterManagement.svc.cs at the Solution Explorer and start debugging.
    public class RegisterManagement : IRegisterManagement
    {
        public string GetRegister(int healthFacilityId, string firstname, string lastname, int year, int languageId)
        {
            //string s = HealthFacility.GetAllChildsForOneHealthFacility(healthFacilityId);

            //string where = string.Format(" AND (\"ADMINISTRATION_ID\" in ({0})) ", s);

            //if (!string.IsNullOrEmpty(firstname))
            //{
            //    where += string.Format(@" AND UPPER(""FIRSTNAME"") LIKE '%{0}%' ", firstname.Replace("'", @"''").ToUpper());
            //}

            //if (!string.IsNullOrEmpty(lastname))
            //{
            //    where += string.Format(@" AND UPPER(""LASTNAME"") LIKE '%{0}%' ", lastname.Replace("'", @"''").ToUpper());
            //}

            //if (year != -1)
            //    where += string.Format(@" AND EXTRACT(YEAR FROM to_date(""BIRTHDATE"", get_date_format())) = {0} ", year);

            DataTable registerData = Register.GetRegister( healthFacilityId,  firstname,  lastname,  year,  languageId);

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(registerData);

            return sJSON;
        }
    }
}
