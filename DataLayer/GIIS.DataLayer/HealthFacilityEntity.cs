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
using System.Data;
using System.Text;
using Npgsql;

namespace GIIS.DataLayer
{
    partial class HealthFacility
    {

        public static int ExistsTopLevel()
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""HEALTH_FACILITY"" WHERE ""TOP_LEVEL"" = 'true' ;";
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "ExistsTopLevel", 7, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static DataTable GetHealthFacilityByParentIdForList(Int32 i)
        {
            try
            {
                string query = string.Format(@"SELECT ""CODE"", ""NAME"" FROM ""HEALTH_FACILITY"" WHERE (""ID"" = {0} OR ""PARENT_ID""= {0}) AND ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;", i);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<HealthFacility> GetHealthFacilityByParentId(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY"" WHERE ""PARENT_ID"" = " + i + @" ORDER BY ""NAME"" ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetHealthFacilityAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityByParentId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<HealthFacility> GetHealthFacilityByParentId(Int32? i)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY"" ";
                if (i.HasValue)
                    query += @" WHERE ""PARENT_ID"" = " + i.Value;
                query += @" ORDER BY ""NAME"" ";

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

                return GetHealthFacilityAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityByParentId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetHealthFacilityMaxId()
        {
            try
            {
                string query = @"SELECT MAX(""ID"") FROM ""HEALTH_FACILITY"" ";
                object max = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(max.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityMaxId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        private static string s = "";

        private static void loadHealthFacility(List<HealthFacility> list, int id)
        {
            foreach (HealthFacility cat in list)
            {
                if (id == cat.Id)
                {
                    s += cat.Id + ", ";
                    if (cat.ParentId != 0)
                        loadHealthFacility(list, cat.ParentId);
                    break;
                }
            }
        }
        private static void recursiveHealthFacility(int id)
        {
            // s += id + ", ";
            if (!HealthFacility.GetHealthFacilityById(id).Leaf)
            {
                List<HealthFacility> list = HealthFacility.GetHealthFacilityByParentId(id);
                foreach (HealthFacility hf in list)
                {
                    s += hf.Id + ", ";
                    if (!hf.Leaf)
                        recursiveHealthFacility(hf.Id);
                }
            }
        }

        public static HealthFacility GetHealthFacilityByIdVaccinationPoint(Int32 i, bool vaccinationPoint)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""HEALTH_FACILITY"" WHERE ""ID"" = {0} AND ""VACCINATION_POINT"" = {1}", i, vaccinationPoint);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetHealthFacilityAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        // Get all HealthFacilities with vacc point=true
        private static void recursiveHealthFacility(int id, bool vaccinationPoint)
        {
            // s += id + ", ";
            if (!HealthFacility.GetHealthFacilityById(id).Leaf)
            {
                List<HealthFacility> list = HealthFacility.GetHealthFacilityByParentId(id);
                foreach (HealthFacility hf in list)
                {
                    if (hf.VaccinationPoint)
                        s += hf.Id + ", ";
                    if (!hf.Leaf)
                        recursiveHealthFacility(hf.Id);
                }
            }
        }
        public static string GetAllChildsForOneHealthFacility(int id, bool vaccpoint)
        {
            s = id + ", ";
            recursiveHealthFacility(id, true);
            if (s.Length >= 2)
            {
                return s.Substring(0, s.Length - 2);
            }
            else
                return s;
        }
        public static string GetAllChildsForOneHealthFacility(int id)
        {
            s = id + ", ";
            recursiveHealthFacility(id);
            if (s.Length >= 2)
            {
                return s.Substring(0, s.Length - 2);
            }
            else
                return s;
        }

        public static DataTable GetHealthFacilityList(string ids)
        {
            try
            {
                string query = string.Format(@"SELECT -1 as ""ID"", '-----' as ""NAME"" UNION SELECT ""ID"", ""NAME"" FROM ""HEALTH_FACILITY"" WHERE ""IS_ACTIVE"" = 'true' AND (""VACCINATION_POINT"" = true OR ""VACCINE_STORE"" = true) {0} ORDER BY ""NAME"";", ids);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt; //GetHealthFacilityAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static DataTable GetVaccinationPointList(string ids)
        {
            try
            {
                string query = string.Format(@"SELECT -1 as ""ID"", '-----' as ""NAME"" UNION SELECT ""ID"", ""NAME"" FROM ""HEALTH_FACILITY"" WHERE ""IS_ACTIVE"" = 'true' AND ""VACCINATION_POINT"" = true {0} ORDER BY ""NAME"";", ids);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt; //GetHealthFacilityAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetVaccinationPointList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<HealthFacility> GetPagedHealthFacilityList(string name, string code, string hfid, ref int maximumRows, ref int startRowIndex)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY"" WHERE 1 = 1 "
                            + @" AND ( UPPER(""NAME"") like @Name OR @Name is null or @Name = '')"
                            + @" AND ( UPPER(""CODE"") like @Code OR @Code is null or @Code = '')"
                            + @" AND (( ""ID"" = ANY( CAST( string_to_array(@HealthFacilityId, ',' ) AS INTEGER[] ))) or @HealthFacilityId = '' or @HealthFacilityId is null)"
                            + @" ORDER BY ""PARENT_ID"", ""NAME"" OFFSET @StartRowIndex LIMIT @MaximumRows;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Name", DbType.String) { Value = ("%" + name + "%") },
                    new NpgsqlParameter("@Code", DbType.String) { Value = ("%" + code + "%") },
                    new NpgsqlParameter("@HealthFacilityId", DbType.String) { Value = hfid },
                    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
                    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetHealthFacilityAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetPagedHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountHealthFacilityList(string name, string code, string hfid)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""HEALTH_FACILITY"" WHERE 1 = 1 "
                                  + @" AND ( UPPER(""NAME"") like @Name OR @Name is null or @Name = '')"
                                  + @" AND ( UPPER(""CODE"") like @Code OR @Code is null or @Code = '')"
                                  + @" AND (( ""ID"" = ANY( CAST( string_to_array(@HealthFacilityId, ',' ) AS INTEGER[] ))) or @HealthFacilityId = '' or @HealthFacilityId is null);";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Name", DbType.String) { Value = ("%" + name + "%") },
                    new NpgsqlParameter("@Code", DbType.String) { Value = ("%" + code + "%") },                    
                    new NpgsqlParameter("@HealthFacilityId", DbType.String) { Value = hfid }
                };

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetCountHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<HealthFacility> GetPagedHealthFacilityList(ref int maximumRows, ref int startRowIndex, string where)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY"" WHERE " + where + @" ORDER BY ""PARENT_ID"",""NAME"" OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetHealthFacilityAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetPagedHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountHealthFacilityList(string where)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""HEALTH_FACILITY"" WHERE " + where + ";";
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetCountHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<HealthFacility> GetHealthFacilityListByTypeId(int typeId)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY"" WHERE ""TYPE_ID"" = @TypeId ORDER BY ""NAME"" ;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@TypeId", DbType.Int32) { Value = typeId }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetHealthFacilityAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityListByTypeId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<HealthFacility> GetHealthFacilityByList(string hfList)
        {

            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY"" WHERE ""ID"" = ANY( CAST( string_to_array(@pList, ',' ) AS INTEGER[] ));";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@pList", DbType.String) { Value = hfList }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetHealthFacilityAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetHealthFacilityByList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }


		public static int GetCumulativeChildId(int healthFacilityId)
		{

			try
			{
				string query = @"SELECT * FROM ""HEALTH_FACILITY_CUMULATIVE_CHILD_REGISTRY_NO"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId ";

				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.String) { Value = healthFacilityId }
				};

				int cumulativeSn = 0;
				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

				if (dt.Rows.Count > 0)
				{
					foreach (DataRow row in dt.Rows)
					{
						if (Int32.Parse(row["REGISTRATION_YEAR"].ToString()) == Int32.Parse(DateTime.Now.Year.ToString()))
						{

							int no = Int32.Parse(row["CURRENT_CHILD_CUMULATIVE_NO"].ToString()) + 1;
							string updateQuery = @"UPDATE ""HEALTH_FACILITY_CUMULATIVE_CHILD_REGISTRY_NO"" SET  ""CURRENT_CHILD_CUMULATIVE_NO"" = @cummulativeNo WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId";

							List<Npgsql.NpgsqlParameter> Updateparameters = new List<NpgsqlParameter>(){
								new NpgsqlParameter("@healthFacilityId", DbType.String) { Value = healthFacilityId },
									new NpgsqlParameter("@cummulativeNo", DbType.Int32) { Value = no }
								};
							DBManager.ExecuteNonQueryCommand(updateQuery, CommandType.Text, Updateparameters);
							AuditTable.InsertEntity("HealthFacility", "GetCumulativeChildId", 3, DateTime.Now, 1);


							cumulativeSn = Int32.Parse(row["CURRENT_CHILD_CUMULATIVE_NO"].ToString());
						}
						else {
							int year = Int32.Parse(DateTime.Now.Year.ToString());
							int no = 2;
							string updateQuery = @"UPDATE ""HEALTH_FACILITY_CUMULATIVE_CHILD_REGISTRY_NO"" SET ""REGISTRATION_YEAR"" = @year, ""CURRENT_CHILD_CUMULATIVE_NO"" = @cummulativeNo WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId";

							List<Npgsql.NpgsqlParameter> Updateparameters = new List<NpgsqlParameter>(){
								new NpgsqlParameter("@healthFacilityId", DbType.String) { Value = healthFacilityId },
									new NpgsqlParameter("@year", DbType.Int32) { Value = year },
									new NpgsqlParameter("@cummulativeNo", DbType.Int32) { Value = no }
								};
							DBManager.ExecuteNonQueryCommand(updateQuery, CommandType.Text, Updateparameters);
							AuditTable.InsertEntity("HealthFacility", "GetCumulativeChildId", 3, DateTime.Now, 1);

							cumulativeSn = 1;
						}
					}

				}
				else
				{
					int year = Int32.Parse(DateTime.Now.Year.ToString());
					int no = 2;

					string insertQuery = @"INSERT INTO ""HEALTH_FACILITY_CUMULATIVE_CHILD_REGISTRY_NO"" (""REGISTRATION_YEAR"",""CURRENT_CHILD_CUMULATIVE_NO"", ""HEALTH_FACILITY_ID"") VALUES (@year, @cummulativeNo, @healthFacilityId)";

					List<Npgsql.NpgsqlParameter> insertParameters = new List<NpgsqlParameter>(){
								new NpgsqlParameter("@healthFacilityId", DbType.String) { Value = healthFacilityId },
									new NpgsqlParameter("@year", DbType.Int32) { Value = year },
									new NpgsqlParameter("@cummulativeNo", DbType.Int32) { Value = no }
								};
					DBManager.ExecuteNonQueryCommand(insertQuery, CommandType.Text, insertParameters);
					AuditTable.InsertEntity("HealthFacility", "GetCumulativeChildId", 4, DateTime.Now, 1);

					cumulativeSn = 1;


				}

				return cumulativeSn;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("Place", "GetHealthFacilityByList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}



		public static int updateHealthFacilityCumulativeChildSn(int healthFacilityId, int cumulativeChildSn)
		{
			try
			{
				int results = -999;
				string query = @"SELECT * FROM ""HEALTH_FACILITY_CUMULATIVE_CHILD_REGISTRY_NO"" WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId ";

				List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter("@healthFacilityId", DbType.String) { Value = healthFacilityId }
				};

				DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

				if (dt.Rows.Count > 0)
				{
					int year = Int32.Parse(DateTime.Now.Year.ToString());
					string updateQuery = @"UPDATE ""HEALTH_FACILITY_CUMULATIVE_CHILD_REGISTRY_NO"" SET ""REGISTRATION_YEAR"" = @year, ""CURRENT_CHILD_CUMULATIVE_NO"" = @cummulativeNo WHERE ""HEALTH_FACILITY_ID"" = @healthFacilityId";

					List<Npgsql.NpgsqlParameter> Updateparameters = new List<NpgsqlParameter>(){
								new NpgsqlParameter("@healthFacilityId", DbType.String) { Value = healthFacilityId },
									new NpgsqlParameter("@year", DbType.Int32) { Value = year },
									new NpgsqlParameter("@cummulativeNo", DbType.Int32) { Value = cumulativeChildSn }
								};
					results = DBManager.ExecuteNonQueryCommand(updateQuery, CommandType.Text, Updateparameters);
					AuditTable.InsertEntity("HealthFacility", "GetCumulativeChildId", 3, DateTime.Now, 1);

				}
				else
				{
					int year = Int32.Parse(DateTime.Now.Year.ToString());

					string insertQuery = @"INSERT INTO ""HEALTH_FACILITY_CUMULATIVE_CHILD_REGISTRY_NO"" (""REGISTRATION_YEAR"",""CURRENT_CHILD_CUMULATIVE_NO"", ""HEALTH_FACILITY_ID"") VALUES (@year, @cummulativeNo, @healthFacilityId)";

					List<Npgsql.NpgsqlParameter> insertParameters = new List<NpgsqlParameter>(){
								new NpgsqlParameter("@healthFacilityId", DbType.String) { Value = healthFacilityId },
									new NpgsqlParameter("@year", DbType.Int32) { Value = year },
									new NpgsqlParameter("@cummulativeNo", DbType.Int32) { Value = cumulativeChildSn }
								};
					results = DBManager.ExecuteNonQueryCommand(insertQuery, CommandType.Text, insertParameters);
					AuditTable.InsertEntity("HealthFacility", "updateHealthFacilityCumulativeChildId", 4, DateTime.Now, 1);

				}
				return results;
			}
			catch (Exception ex)
			{
				Log.InsertEntity("Place", "GetHealthFacilityByList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
				throw ex;
			}
		}


    }
}