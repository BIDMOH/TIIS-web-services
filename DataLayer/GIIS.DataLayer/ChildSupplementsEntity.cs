//*******************************************************************************
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
using System.Text;
using System.Data;

namespace GIIS.DataLayer
{
    public partial class ChildSupplements
    {
        public static ChildSupplements GetChildSupplementsByChild(Int32 childId)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD_SUPPLEMENTS"" WHERE ""CHILD_ID"" = " + childId + "";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildSupplementsAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildSupplements", "GetChildSupplementsByChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static ChildSupplements GetChildSupplementsByChild(Int32 childId, DateTime date)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD_SUPPLEMENTS"" WHERE ""CHILD_ID"" = " + childId + @" AND ""DATE"" = '" + date.ToString("yyyy-MM-dd") + "'";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildSupplementsAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildSupplements", "GetChildSupplementsByChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
