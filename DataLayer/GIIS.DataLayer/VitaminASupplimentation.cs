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
using Npgsql;

namespace GIIS.DataLayer
{
	public partial class VitaminASupplimentation
	{
		public Int32 male9Months { get;set;}
		public Int32 female9Months { get; set; }
		public Int32 male15Months { get; set; }
		public Int32 female15Months { get; set; }
		public Int32 male18Months { get; set; }
		public Int32 female18Months { get; set; }


		public  static VitaminASupplimentation GetChildSupplementsByChild(int healthFacilityId, DateTime fromTime,DateTime toTime)
		{
			try
			{
				string query = @"SELECT *
                            FROM ""JRNL_CHILD_SUPPLEMENTS"" 
							INNER JOIN ""CHILD"" ON  ""JRNL_CHILD_SUPPLEMENTS"".""CHILD_ID""=""CHILD"".""ID"" 
							INNER JOIN ""USER"" ON  ""JRNL_CHILD_SUPPLEMENTS"".""MODIFIED_BY""=""USER"".""ID"" 
								WHERE 
									""JRNL_CHILD_SUPPLEMENTS"".""VitA"" = @VitA
		                            AND ""JRNL_CHILD_SUPPLEMENTS"".""DATE"" >= @fromTime 
		                            AND ""JRNL_CHILD_SUPPLEMENTS"".""DATE"" <=  @toTime
									AND ""USER"".""HEALTH_FACILITY_ID"" = @healthFacilityId";

				List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@VitA", DbType.Boolean)  { Value = true },
					new NpgsqlParameter("@fromTime", DbType.DateTime)  { Value = fromTime },
					new NpgsqlParameter("@toTime", DbType.DateTime)  { Value = toTime },
					new NpgsqlParameter("@healthFacilityId", DbType.Int32)  { Value = healthFacilityId }
				};
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

				return GetVitaminASupplimentationAsObject(dt);
			}
			catch (Exception ex)
			{
				Log.InsertEntity("ChildSupplements", "GetChildSupplementsByChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}

		public static VitaminASupplimentation GetVitaminASupplimentationAsObject(DataTable dt)
		{
			VitaminASupplimentation aSupplimentation = new VitaminASupplimentation();
			foreach (DataRow row in dt.Rows)
			{
				DateTime bdate = Helper.ConvertToDate(row["BIRTHDATE"]);
				DateTime supplimentationDate = Helper.ConvertToDate(row["BIRTHDATE"]);

				TimeSpan diff = supplimentationDate.Subtract(bdate);

				if (diff.TotalDays > 7 * 4 * 17)
				{
					if (Helper.ConvertToBoolean(row["GENDER"]))
					{
						aSupplimentation.male18Months++;
					}
					else 
					{
						aSupplimentation.female18Months++;
					}
				}
				else if (diff.TotalDays > 7 * 4 * 14)
				{
					if (Helper.ConvertToBoolean(row["GENDER"]))
					{
						aSupplimentation.male15Months++;
					}
					else
					{
						aSupplimentation.female15Months++;
					}
				}
				else {
					if (Helper.ConvertToBoolean(row["GENDER"]))
					{
						aSupplimentation.male9Months++;
					}
					else
					{
						aSupplimentation.female9Months++;
					}
				}
			}


			return aSupplimentation;

		}

    }
}
