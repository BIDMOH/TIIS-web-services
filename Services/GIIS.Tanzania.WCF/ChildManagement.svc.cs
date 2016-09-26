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
using System.Text;

using GIIS.DataLayer;
using System.Globalization;
using System.Net;
using System.IO;
using System.ServiceModel.Web;
using System.Data;
using Newtonsoft.Json.Linq;

namespace GIIS.Tanzania.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ChildManagement" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ChildManagement.svc or ChildManagement.svc.cs at the Solution Explorer and start debugging.
    public class ChildManagement : IChildManagement
    {
		
		/**
		 * Method used to broadcast any child updates to other tablets within and outside catchment if they have a stored copy of the childs information
		 * 
		 **/
        public string BroadcastChildUpdates(int childId)
        {



			List<string> regIDs = GetGcmIdsToSendDataModifiedChild(childId);
            
			string stringregIds = null;

            //Then I use 
            stringregIds = string.Join("\",\"", regIDs);
            //To Join the values (if ever there are more than 1) with quotes and commas for the Json format below

            try
            {
                string GoogleAppID = "AIzaSyBgsthTTTiunMtHV5XT1Im6bl17i5rGR94";
                var SENDER_ID = "967487253557";
				var value = childId.ToString();
                WebRequest tRequest;
                tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));

                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

                string postData = "{\"collapse_key\":\"score_update\",\"time_to_live\":108,\"delay_while_idle\":true,\"data\": { \"message\" : " + "\"" + value + "\",\"time\": " + "\"" + System.DateTime.Now.ToString() + "\"},\"registration_ids\":[\"" + stringregIds + "\"]}";

                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;

                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = tRequest.GetResponse();

                dataStream = tResponse.GetResponseStream();

                StreamReader tReader = new StreamReader(dataStream);

                String sResponseFromServer = tReader.ReadToEnd();

                HttpWebResponse httpResponse = (HttpWebResponse)tResponse;
                string statusCode = httpResponse.StatusCode.ToString();

                tReader.Close();
                dataStream.Close();
                tResponse.Close();


				JObject o = JObject.Parse(sResponseFromServer);
				JArray a = (JArray)o["results"];
				int counter = a.Count;
				for (int i = 0; i < counter; i++)
				{
					if (a[i]["registration_id"] != null)
					{
						updateGcmId((string)a[i]["registration_id"],regIDs.ElementAt(i));
					}

				}


                return sResponseFromServer;
            }
            catch
            {
                //throw new WebFaultException<string>("Error", HttpStatusCode.ServiceUnavailable);
                return "error sending data to GCM server. Service unavailable";
            }

            
        }

		/**
		 *Method used to update gcmIds with canonical ids received from google's GCM server 
		 **/
		public void updateGcmId(string cannonicalId, string gcmId)
		{

			GIIS.DataLayer.User.updateGcmId(cannonicalId,gcmId);

		}

		/**
		 * Broadcasting child updates based on a child's barcodeid
		 **/
		public string BroadcastChildUpdatesWithBarcodeId(string barcodeId)
		{

			Child c = GIIS.DataLayer.Child.GetChildByBarcode(barcodeId);
			int childId = c.Id;

			return BroadcastChildUpdates(childId);
		}

		/**
		 * Method for obtaining a list of health facilities gcm ids to push updates to
		 **/
		public List<string> GetGcmIdsToSendDataModifiedChild(int childId)
		{
			DataTable dt = GIIS.DataLayer.VaccinationEvent.GetHfidsOfModifiedChild(childId);

			string hfids = "";
			foreach (DataRow dr in dt.Rows)
			{
				hfids = hfids + dr[0].ToString() + ",";

			}

			DataTable dt1 = GIIS.DataLayer.VaccinationEvent.GetHfidsOfChild(childId); 
			foreach (DataRow dr1 in dt1.Rows)
			{
				hfids = hfids + dr1[0].ToString();

			}
			return GetGcmIds(hfids);
		}

		public List<string> GetGcmIds(string healthFacilityIdsList)
		{
			DataTable dt = GIIS.DataLayer.VaccinationEvent.GetGcmIds(healthFacilityIdsList);
			List<string> gcmIdsList = new List<string>();
			foreach (DataRow dr in dt.Rows)
			{
				gcmIdsList.Add(dr[0].ToString());
			}

			return gcmIdsList;
		}

		public IntReturnValue RegisterChildWithoutAppointments(string barcodeId, string firstname1, string lastname1, DateTime birthdate, bool gender,
            int healthFacilityId, int birthplaceId, int domicileId, string address, string phone, string motherFirstname,
            string motherLastname, string notes, int userId, DateTime modifiedOn)
        {

			Child o = new Child();

			o.Firstname1 = firstname1;
			o.Lastname1 = lastname1;

			o.Birthdate = birthdate;
			o.Gender = gender;

			o.HealthcenterId = healthFacilityId;
			o.BirthplaceId = birthplaceId;
			o.DomicileId = domicileId;
			o.Address = address;
			o.Phone = phone;
			o.MotherFirstname = motherFirstname;
			o.MotherLastname = motherLastname;
			o.Notes = notes;
			o.ModifiedOn = modifiedOn;
			o.ModifiedBy = userId;

			o.SystemId = DateTime.Now.ToString("yyMMddhhmmss");
			o.BarcodeId = barcodeId;

			o.StatusId = 1;

			int childInserted = Child.Insert(o);

			IntReturnValue irv = new IntReturnValue();
			irv.id = childInserted;
			return irv;

        }


		public IntReturnValue RegisterChildWithoutAppointmentsWithMothersHivStatusAndTT2VaccineStatus(string barcodeId, string firstname1, string lastname1, DateTime birthdate, bool gender,
		                                                                                              int healthFacilityId, int birthplaceId, int domicileId, string address, string phone, string motherFirstname, string motherLastname, string mothersHivStatus, string mothersTT2Status, string childCumulativeSn, string childRegistryYear, string notes, int userId, DateTime modifiedOn)
		{
			Child o = new Child();

			o.Firstname1 = firstname1;
			o.Lastname1 = lastname1;

			o.Birthdate = birthdate;
			o.Gender = gender;

			o.HealthcenterId = healthFacilityId;
			o.BirthplaceId = birthplaceId;
			o.DomicileId = domicileId;

			o.Address = address;
			o.Phone = phone;


			if (!childCumulativeSn.Equals("") && childRegistryYear.Equals(""))
			{
				o.ChildCumulativeSn = Int32.Parse(childCumulativeSn);
				o.ChildRegistryYear = Int32.Parse(childRegistryYear);
			}
			else {
				o.ChildCumulativeSn = GIIS.DataLayer.HealthFacility.GetAndIncrementCumulativeChildId(healthFacilityId);
				o.ChildRegistryYear = Int32.Parse(DateTime.Now.Year.ToString());

			}

			o.MotherHivStatus = mothersHivStatus;
			o.MotherTT2Status = mothersTT2Status;
			o.MotherFirstname = motherFirstname;
			o.MotherLastname = motherLastname;
			o.Notes = notes;
			o.ModifiedOn = modifiedOn;
			o.ModifiedBy = userId;

			o.SystemId = DateTime.Now.ToString("yyMMddhhmmss");
			o.BarcodeId = barcodeId;

			o.StatusId = 1;

			int childInserted = Child.Insert(o);

			IntReturnValue irv = new IntReturnValue();
			irv.id = childInserted;
			return irv;

		}


		public IntReturnValue RegisterChildWithAppoitments(string barcodeId, string firstname1, string firstname2, string lastname1, DateTime birthdate, bool gender,
            int healthFacilityId, int birthplaceId, int domicileId, string address, string phone, string motherFirstname,
            string motherLastname, string notes, int userId, DateTime modifiedOn)
        {
			Child o = new Child();

			o.Firstname1 = firstname1;
			o.Lastname1 = lastname1;
			o.Firstname2 = firstname2;
			o.Birthdate = birthdate;
			o.Gender = gender;

			o.HealthcenterId = healthFacilityId;
			o.BirthplaceId = birthplaceId;
			o.DomicileId = domicileId;

			o.Address = address;
			o.Phone = phone;
			o.MotherFirstname = motherFirstname;
			o.MotherLastname = motherLastname;
			o.Notes = notes;
			o.ModifiedOn = modifiedOn;
			o.ModifiedBy = userId;

			o.SystemId = DateTime.Now.ToString("yyMMddhhmmss");
			o.BarcodeId = barcodeId;
			o.IsActive = true;
			o.StatusId = 1;

			int childInserted = Child.Insert(o);

			if (childInserted > 0)
			{
				//add appointments
				VaccinationAppointment.InsertVaccinationsForChild(childInserted, userId);
				BroadcastChildUpdates(childInserted);
			}

			IntReturnValue irv = new IntReturnValue();
			irv.id = childInserted;
			return irv;
        }

		public IntReturnValue RegisterChildWithAppoitmentsWithMothersHivStatusAndTT2VaccineStatus(string barcodeId, string firstname1, string firstname2, string lastname1, DateTime birthdate, bool gender,
		   int healthFacilityId, int birthplaceId, int domicileId, string address, string phone, string motherFirstname,
																								  string motherLastname, string mothersHivStatus, string mothersTT2Status, string childCumulativeSn, string childRegistryYear, string notes, int userId, DateTime modifiedOn)

		{
			int catchment = 1;
			return RegisterChildWithAppoitmentsWithMothersHivStatusAndTT2VaccineStatusAndCatchment(barcodeId,firstname1,firstname2, lastname1, birthdate, gender,
			                                                                                       healthFacilityId,  birthplaceId,  domicileId,  address,  phone,  motherFirstname, motherLastname,  mothersHivStatus,  mothersTT2Status,  childCumulativeSn, childRegistryYear, notes,  userId, modifiedOn,catchment);
		}

		public IntReturnValue RegisterChildWithAppoitmentsWithMothersHivStatusAndTT2VaccineStatusAndCatchment(string barcodeId, string firstname1, string firstname2, string lastname1, DateTime birthdate, bool gender,
		   int healthFacilityId, int birthplaceId, int domicileId, string address, string phone, string motherFirstname,
		                                                                                          string motherLastname, string mothersHivStatus, string mothersTT2Status,string childCumulativeSn, string childRegistryYear, string notes, int userId, DateTime modifiedOn, int catchment)
		{
			Child o = new Child();

			o.Firstname1 = firstname1;
			o.Lastname1 = lastname1;
			o.Firstname2 = firstname2;
			o.Birthdate = birthdate;
			o.Gender = gender;

			//if (childExists(o.Lastname1, o.Gender, o.Birthdate))
			//    return -1;

			o.HealthcenterId = healthFacilityId;
			o.BirthplaceId = birthplaceId;
			o.DomicileId = domicileId;

			o.Address = address;
			o.Phone = phone;
			o.MotherFirstname = motherFirstname;
			o.MotherLastname = motherLastname;
			o.Notes = notes;
			o.ModifiedOn = modifiedOn;
			o.ModifiedBy = userId;


			o.MotherHivStatus = mothersHivStatus;
			o.MotherTT2Status = mothersTT2Status;

			if (catchment != -1)
			{
				if (childCumulativeSn!=null && childRegistryYear!=null && !childCumulativeSn.Equals("") && !childRegistryYear.Equals(""))
				{
					o.ChildCumulativeSn = Int32.Parse(childCumulativeSn);
					o.ChildRegistryYear = Int32.Parse(childRegistryYear);

				}
				else {
					o.ChildCumulativeSn = GIIS.DataLayer.HealthFacility.GetAndIncrementCumulativeChildId(healthFacilityId);
					o.ChildRegistryYear = Int32.Parse(DateTime.Now.Year.ToString());

				}
			}


			o.SystemId = DateTime.Now.ToString("yyMMddhhmmss");
			o.BarcodeId = barcodeId;
			o.IsActive = true;
			o.StatusId = 1;

			int childInserted = Child.Insert(o);

			if (childInserted > 0)
			{
				//add appointments
				VaccinationAppointment.InsertVaccinationsForChild(childInserted, userId);
				BroadcastChildUpdates(childInserted);
			}

			IntReturnValue irv = new IntReturnValue();
			irv.id = catchment;
			return irv;
		}


        public IntReturnValue UpdateChild(string barcode, string firstname1, string firstname2, string lastname1, DateTime birthdate, bool gender,
             int healthFacilityId, int birthplaceId, int domicileId, int statusId, string address, string phone, string motherFirstname,
             string motherLastname, string notes, int userId, int childId, DateTime modifiedOn)
        {
			Child o = null;
			int n;
			int healthcenter = 0;
			int datediff = Int32.MaxValue;
			bool isNumeric = int.TryParse(childId.ToString(), out n);
			if (isNumeric && childId != 0)
				o = Child.GetChildById(childId);
			else if (!string.IsNullOrEmpty(barcode))
				o = Child.GetChildByBarcode(barcode);

			if (o != null)
			{
				o.Firstname1 = firstname1;
				o.Lastname1 = lastname1;
				o.Firstname2 = firstname2;

				if (o.Birthdate != birthdate)
					datediff = birthdate.Subtract(o.Birthdate).Days;

				o.Birthdate = birthdate;
				o.Gender = gender;

				if (o.HealthcenterId != healthFacilityId)
				{
					healthcenter = healthFacilityId;
				}
				o.HealthcenterId = healthFacilityId;
				o.BirthplaceId = birthplaceId;
				o.DomicileId = domicileId;
				o.CommunityId = null;
				o.StatusId = statusId;
				o.Address = address;
				o.Phone = phone;
				o.MotherFirstname = motherFirstname;
				o.MotherLastname = motherLastname;
				o.Notes = notes;
				o.ModifiedOn = modifiedOn; // DateTime.Now;
				o.ModifiedBy = userId;
				o.BarcodeId = barcode;
			}
			int childUpdated = Child.Update(o);

			if (childUpdated > 0)
			{
				bool appstatus = true;
				if (o.StatusId != 1)
					appstatus = false;

				List<VaccinationAppointment> applist = VaccinationAppointment.GetVaccinationAppointmentsByChildNotModified(childId);
				List<VaccinationAppointment> applistall = VaccinationAppointment.GetVaccinationAppointmentsByChild(childId);
				if (!appstatus)
				{
					foreach (VaccinationAppointment app in applist)
						VaccinationAppointment.Update(appstatus, app.Id);
				}

				if (healthcenter != 0)
				{
					foreach (VaccinationAppointment app in applist)
					{
						VaccinationAppointment.Update(o.HealthcenterId, app.Id);
						GIIS.DataLayer.VaccinationEvent.Update(app.Id, o.HealthcenterId);
					}
				}
				if (datediff != Int32.MaxValue)
				{
					bool done = false;
					foreach (VaccinationAppointment app in applistall)
					{
						GIIS.DataLayer.VaccinationEvent ve = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventByAppointmentId(app.Id)[0];
						if (ve.VaccinationStatus || ve.NonvaccinationReasonId != 0)
						{
							done = true;
							break;
						}
					}

					foreach (VaccinationAppointment app in applist)
					{
						if (done)
							break;
						VaccinationAppointment.Update(app.ScheduledDate.AddDays(datediff), app.Id);
						GIIS.DataLayer.VaccinationEvent.Update(app.Id, app.ScheduledDate.AddDays(datediff));
					}

				}
				BroadcastChildUpdates(childId);
			}

			IntReturnValue irv = new IntReturnValue();
			irv.id = childUpdated;
			return irv;
        }


		public IntReturnValue UpdateChildWithMothersHivStatusAndTT2VaccineStatus(string barcode, string firstname1, string firstname2, string lastname1, DateTime birthdate, bool gender,
			 int healthFacilityId, int birthplaceId, int domicileId, int statusId, string address, string phone, string motherFirstname,
		                                                                         string motherLastname, string mothersHivStatus, string mothersTT2Status,string childCumulativeSn, string childRegistryYear, string notes, int userId, int childId, DateTime modifiedOn)
		{
			Child o = null;
			int n;
			int healthcenter = 0;
			int datediff = Int32.MaxValue;
			bool isNumeric = int.TryParse(childId.ToString(), out n);
			if (isNumeric && childId != 0)
				o = Child.GetChildById(childId);
			else if (!string.IsNullOrEmpty(barcode))
				o = Child.GetChildByBarcode(barcode);

			if (o != null)
			{
				o.Firstname1 = firstname1;
				o.Lastname1 = lastname1;
				o.Firstname2 = firstname2;

				if (o.Birthdate != birthdate)
					datediff = birthdate.Subtract(o.Birthdate).Days;

				o.Birthdate = birthdate;
				o.Gender = gender;

				if (o.HealthcenterId != healthFacilityId)
				{
					healthcenter = healthFacilityId;
				}
				o.HealthcenterId = healthFacilityId;
				o.BirthplaceId = birthplaceId;
				o.DomicileId = domicileId;
				o.CommunityId = null;
				o.StatusId = statusId;
				o.Address = address;
				o.Phone = phone;
				o.MotherFirstname = motherFirstname;
				o.MotherLastname = motherLastname;


				o.MotherHivStatus = mothersHivStatus;
				o.MotherTT2Status = mothersTT2Status;

				if (!childCumulativeSn.Equals("") && !childRegistryYear.Equals(""))
				{
					o.ChildCumulativeSn = Int32.Parse(childCumulativeSn);
					o.ChildRegistryYear = Int32.Parse(childRegistryYear);
				}

				o.Notes = notes;
				o.ModifiedOn = modifiedOn; // DateTime.Now;
				o.ModifiedBy = userId;
				o.BarcodeId = barcode;
			}
			int childUpdated = Child.Update(o);

			if (childUpdated > 0)
			{
				bool appstatus = true;
				if (o.StatusId != 1)
					appstatus = false;

				List<VaccinationAppointment> applist = VaccinationAppointment.GetVaccinationAppointmentsByChildNotModified(childId);
				List<VaccinationAppointment> applistall = VaccinationAppointment.GetVaccinationAppointmentsByChild(childId);
				if (!appstatus)
				{
					foreach (VaccinationAppointment app in applist)
						VaccinationAppointment.Update(appstatus, app.Id);
				}

				if (healthcenter != 0)
				{
					foreach (VaccinationAppointment app in applist)
					{
						VaccinationAppointment.Update(o.HealthcenterId, app.Id);
						GIIS.DataLayer.VaccinationEvent.Update(app.Id, o.HealthcenterId);
					}
				}
				if (datediff != Int32.MaxValue)
				{
					bool done = false;
					foreach (VaccinationAppointment app in applistall)
					{
						GIIS.DataLayer.VaccinationEvent ve = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventByAppointmentId(app.Id)[0];
						if (ve.VaccinationStatus || ve.NonvaccinationReasonId != 0)
						{
							done = true;
							break;
						}
					}

					foreach (VaccinationAppointment app in applist)
					{
						if (done)
							break;
						VaccinationAppointment.Update(app.ScheduledDate.AddDays(datediff), app.Id);
						GIIS.DataLayer.VaccinationEvent.Update(app.Id, app.ScheduledDate.AddDays(datediff));
					}

				}
				BroadcastChildUpdates(childId);
			}

			IntReturnValue irv = new IntReturnValue();
			irv.id = childUpdated;
			return irv;

		}


        public IntReturnValue RemoveChild(int id)
        {
            int removed = Child.Remove(id);
            IntReturnValue irv = new IntReturnValue();
            irv.id = removed;
            return irv;
        }

        public IntReturnValue DeleteChild(int id)
        {
            int deleted = Child.Delete(id);
            IntReturnValue irv = new IntReturnValue();
            irv.id = deleted;
            return irv;
        }

        public List<Child> FindDublication(bool birthdateFlag, bool firstnameFlag, bool genderFlag, int healthFacilityId)
        {
            List<Child> dublications = Child.GetDuplications(birthdateFlag, firstnameFlag, genderFlag, healthFacilityId);

            return dublications;
        }

        public List<Child> Search(int statusId, DateTime birthdateFrom, DateTime birthdateTo, string firstname1, string lastname1, string otherId, int healthFacilityId,
            int birthplaceId, int communityId, int domicileId, string address, string phone, string mobile)
        {
            int max = Int32.MaxValue;
            int start = 0;

            string idFields = null;
            string motherFirstname = null;
            string motherLastname = null;
			string mothersHivStatus = null;
			string mothersTT2Status = null;


            string systemId = null;
            string barcodeId = null;
            string tempId = null;

            List<Child> childList = Child.GetPagedChildList(statusId, birthdateFrom, birthdateTo, firstname1, lastname1, idFields,
                healthFacilityId.ToString(), birthplaceId, communityId, domicileId, motherFirstname, motherLastname,mothersHivStatus, mothersTT2Status, systemId, barcodeId, tempId,
                ref max, ref start);

            return childList;
        }

        public List<ChildResults> Search(string where)
        {
            int statusId = 1;
            DateTime birthdateFrom = new DateTime();
            DateTime birthdateTo = new DateTime();

            string firstname1 = null;
            string lastname1 = null;

            int birthplaceId = 0;
            int communityId = 0;
            int domicileId = 0;

            string idFields = null;
            string motherFirstname = null;
            string motherLastname = null;

			string motherHivStatus = null;
			string motherTT2Status = null;

            string systemId = null;
            string barcodeId = null;
            string tempId = null;

            string healthFacilityId = null;

            string[] s = where.Split('!');
            foreach (string s1 in s)
            {
                if (s1.ToLower().Contains("firstname1"))
                    firstname1 = s1.Substring(s1.IndexOf('=') + 1).ToUpper();

                if (s1.ToLower().Contains("lastname1"))
                    lastname1 = s1.Substring(s1.IndexOf('=') + 1).ToUpper();

                if (s1.ToLower().Contains("motherfirstname"))
                    motherFirstname = s1.Substring(s1.IndexOf('=') + 1).ToUpper();

                if (s1.ToLower().Contains("motherlastname"))
                    motherLastname = s1.Substring(s1.IndexOf('=') + 1).ToUpper();


				if (s1.ToLower().Contains("mothersHivStatus"))
					motherHivStatus = s1.Substring(s1.IndexOf('=') + 1).ToUpper();

				if (s1.ToLower().Contains("mothersTT2Status"))
					motherTT2Status = s1.Substring(s1.IndexOf('=') + 1).ToUpper();


                //if (s1.ToLower().Contains("birthdate"))
                //{
                //    string birthdate = s1.Substring(s1.IndexOf('=') + 1).ToUpper();
                //    _where += string.Format(@" AND ""BIRTHDATE"" = '{0}'", birthdate);
                //}

                if (s1.ToLower().Contains("healthfacilityid"))
                    healthFacilityId = s1.Substring(s1.IndexOf('=') + 1);

                if (s1.ToLower().Contains("birthplaceid"))
                    Int32.TryParse(s1.Substring(s1.IndexOf('=') + 1), out birthplaceId);


                if (s1.ToLower().Contains("domicileid"))
                    Int32.TryParse(s1.Substring(s1.IndexOf('=') + 1), out domicileId);

                if (s1.ToLower().Contains("statusid"))
                    Int32.TryParse(s1.Substring(s1.IndexOf('=') + 1), out statusId);

                if (s1.ToLower().Contains("birthdatefrom"))
                {
                    string bd = s1.Substring(s1.IndexOf('=') + 1).ToUpper();
                    birthdateFrom = DateTime.ParseExact(bd, "yyyy-MM-dd", CultureInfo.CurrentCulture);
                }

                if (s1.ToLower().Contains("birthdateto"))
                {
                    string bd = s1.Substring(s1.IndexOf('=') + 1).ToUpper();
                    birthdateTo = DateTime.ParseExact(bd, "yyyy-MM-dd", CultureInfo.CurrentCulture);
                }
                if (s1.ToLower().Contains("barcode"))
                    barcodeId = s1.Substring(s1.IndexOf('=') + 1).ToUpper();
                if (s1.ToLower().Contains("tempid"))
                    tempId = s1.Substring(s1.IndexOf('=') + 1).ToUpper();

            }

            int max = Int32.MaxValue;
            int start = 0;

            List<Child> childList = Child.GetPagedChildList(statusId, birthdateFrom, birthdateTo, firstname1, lastname1, idFields,
			                                                healthFacilityId, birthplaceId, communityId, domicileId, motherFirstname, motherLastname, motherHivStatus,motherTT2Status, systemId, barcodeId, tempId,ref max, ref start);
            List<ChildResults> chlist = new List<ChildResults>();
            foreach (Child ch in childList)
            {
                ChildResults chr = new ChildResults();
                chr.Id = ch.Id;
                chr.Firstname1 = ch.Firstname1;
                chr.Lastname1 = ch.Lastname1;
                chr.BarcodeId = ch.BarcodeId;
                chr.MotherFirstname = ch.MotherFirstname;
                chr.MotherLastname = ch.MotherLastname;

				chr.MothersHivStatus = ch.MotherHivStatus;
				chr.MothersTT2Status = ch.MotherTT2Status;

                chr.Gender = ch.Gender;
                chr.Birthdate = ch.Birthdate;
                chr.HealthcenterId = ch.Healthcenter.Name;
                if (ch.Domicile != null)
                    chr.DomicileId = ch.Domicile.Name;
                chr.Firstname2 = ch.Firstname2;
                chlist.Add(chr);
            }
            return chlist;
        }

        public List<Child> GetChildByHealthFacilityId(int healthFacilityId)
        {
            List<Child> childList = Child.GetChildByHealthFacilityId(healthFacilityId);
            return childList;
        }

        public List<ChildEntity> GetChildrenByHealthFacility(int healthFacilityId)
        {
            List<Child> childList = Child.GetChildByHealthFacilityId(healthFacilityId);


            List<ChildEntity> ceList = new List<ChildEntity>();

            foreach (Child child in childList)
            {
                List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(child.Id);
                List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(child.Id);

                ChildEntity ce = new ChildEntity();
                ce.childEntity = child;
                ce.vaList = vaList; // GetVaccinationAppointment(vaList);
                ce.veList = veList; // GetVaccinationEvent(veList);
                ceList.Add(ce);
            }

            return ceList;
        }



        public List<Child> GetOnlyChildrenByHealthFacility(int healthFacilityId)
        {
            List<Child> childList = Child.GetChildByHealthFacilityId(healthFacilityId);
            return childList;
        }

        public ChildListEntity GetOnlyChildrenDataByHealthFacility(int healthFacilityId)
        {
            if (healthFacilityId > 0)
            {
                ChildListEntity cle = new ChildListEntity();

                List<Child> chList = Child.GetChildByHealthFacilityId(healthFacilityId);
                List<GIIS.DataLayer.VaccinationAppointment> valist = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByHealthFacility (healthFacilityId);
                List<GIIS.DataLayer.VaccinationEvent> velist = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventByHealthFacility(healthFacilityId);
               
                cle.childList = chList;
                cle.vaList = valist;
                cle.veList = velist;

                return cle;
            }
            else
                return null;
        }

        public ChildListEntity GetChildrenByHealthFacilitySinceLastLogin(int idUser)
        {
            if (idUser > 0)
            {
                User user = User.GetUserById(idUser);
                ChildListEntity cle = new ChildListEntity();

                List<Child> childList = Child.GetChildByHealthFacilityIdSinceLastLogin(idUser);
                cle.childList = childList;

                List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(user.Lastlogin, user.Id);
                cle.vaList = vaList;

                List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(user.Lastlogin, user.Id);
                cle.veList = veList;

                //List<ChildEntity> ceList = new List<ChildEntity>();

                //foreach (Child child in childList)
                //{

                //    ChildEntity ce = new ChildEntity();
                //    ce.childEntity = child;
                //    ce.vaList = vaList; // GetVaccinationAppointment(vaList);
                //    ce.veList = veList; // GetVaccinationEvent(veList);
                //    ceList.Add(ce);
                //}

                //return ceList;
                 return cle;
            }
            else
                return null;
    }
        
        //public List<ChildEntity> GetChildrenByHealthFacilityBeforeLastLogin(int idUser)
        //{
        //    User user = User.GetUserById(idUser);

        //    List<Child> childList = Child.GetChildByHealthFacilityIdBeforeLastLogin(idUser);

        //    List<ChildEntity> ceList = new List<ChildEntity>();

        //    foreach (Child child in childList)
        //    {
        //        List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEventBefore(child.Id, user.Lastlogin, user.PrevLogin, user.Id);
        //        List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChildBefore(child.Id, user.Lastlogin, user.Id);

        //        ChildEntity ce = new ChildEntity();
        //        ce.childEntity = child;
        //        ce.vaList = vaList; // GetVaccinationAppointment(vaList);
        //        ce.veList = veList; // GetVaccinationEvent(veList);
        //        ceList.Add(ce);
        //    }

        //    return ceList;
        //}

        public ChildListEntity GetChildrenByHealthFacilityBeforeLastLogin(int idUser)
        {
            if (idUser > 0)
            {
                User user = User.GetUserById(idUser);
                ChildListEntity cle = new ChildListEntity();

                List<Child> childList = Child.GetChildByHealthFacilityIdBeforeLastLogin(idUser);
                cle.childList = childList;

                List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChildBefore(user.Lastlogin, user.PrevLogin, user.Id);
                cle.vaList = vaList;

                List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEventBefore(user.Lastlogin, user.PrevLogin , user.Id);
                cle.veList = veList;
                return cle;
            }
            else
                return null;
           
        }

        public ChildListEntity GetChildrenByHealthFacilityDayFirstLogin(int idUser)
        {
            if (idUser > 0)
            {
                User user = User.GetUserById(idUser);
                ChildListEntity cle = new ChildListEntity();

                List<Child> childList = Child.GetChildByHealthFacilityIdDayFirstLogin(idUser);
                cle.childList = childList;

                List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChildDayFirstLogin(user.Lastlogin.AddDays(-1), user.Id);
                cle.vaList = vaList;

                List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEventDayFirstLogin(user.Lastlogin.AddDays(-1), user.Id);
                cle.veList = veList;
                return cle;
            }
            else
                return null;
        }

        //public List<ChildEntity> GetChildrenByHealthFacilityDayFirstLogin(int idUser)
        //{
        //    User user = User.GetUserById(idUser);

        //    List<Child> childList = Child.GetChildByHealthFacilityIdDayFirstLogin(idUser);

        //    List<ChildEntity> ceList = new List<ChildEntity>();

        //    foreach (Child child in childList)
        //    {
        //        List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEventDayFirstLogin(child.Id, user.Lastlogin.AddDays(-1), user.Id);
        //        List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChildDayFirstLogin(child.Id, user.Lastlogin.AddDays(-1));

        //        ChildEntity ce = new ChildEntity();
        //        ce.childEntity = child;
        //        ce.vaList = vaList; // GetVaccinationAppointment(vaList);
        //        ce.veList = veList; // GetVaccinationEvent(veList);
        //        ceList.Add(ce);
        //    }

        //    return ceList;
        //}
        #region Helper
        protected bool childExists(string lastname, bool gender, DateTime birthdate)
        {
            return (Child.GetPersonByLastnameBirthdateGender(lastname, birthdate, gender) != null);
        }
       
        private List<GIIS.DataLayer.VaccinationEvent> GetVaccinationEvent(List<GIIS.DataLayer.VaccinationEvent> veList)
        {
            List<GIIS.DataLayer.VaccinationEvent> tmp = new List<GIIS.DataLayer.VaccinationEvent>();

            return tmp;
        }
        private List<GIIS.DataLayer.VaccinationAppointment> GetVaccinationAppointment(List<GIIS.DataLayer.VaccinationAppointment> vaList)
        {
            List<GIIS.DataLayer.VaccinationAppointment> tmp = new List<GIIS.DataLayer.VaccinationAppointment>();

            return tmp;
        }

        private List<ChildEntity> GetChildrenWithAppointmentAndEvents(List<Child> childList)
        {
            List<ChildEntity> ceList = new List<ChildEntity>();

            foreach (Child child in childList)
            {
                List<GIIS.DataLayer.VaccinationEvent> veList = new List<DataLayer.VaccinationEvent>(); // GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(child.Id, child.ModifiedOn);
                List<GIIS.DataLayer.VaccinationAppointment> vaList = new List<VaccinationAppointment>(); // GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(child.Id, child.ModifiedOn);

                ChildEntity ce = new ChildEntity();
                ce.childEntity = child;
                ce.vaList = GetVaccinationAppointment(vaList);
                ce.veList = GetVaccinationEvent(veList);
                ceList.Add(ce);
            }

            return ceList;
        }
        #endregion

        public List<ChildEntity> SearchByBarcode(string barcodeId)
        {
            GIIS.DataLayer.Child child = GIIS.DataLayer.Child.GetChildByBarcode(barcodeId);

            List<ChildEntity> ceList = new List<ChildEntity>();

            if (child != null)
            {
                List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(child.Id);
                List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(child.Id);

                ChildEntity ce = new ChildEntity();
                ce.childEntity = child;
                ce.vaList = vaList; // GetVaccinationAppointment(vaList);
                ce.veList = veList; // GetVaccinationEvent(veList);
                ceList.Add(ce);
            }

            return ceList;
        }

        public List<ChildEntity> SearchByTempId(string tempId)
        {
            Child child = Child.GetChildByTempId(tempId);

            List<ChildEntity> ceList = new List<ChildEntity>();

            if (child != null)
            {
                List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(child.Id);
                List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(child.Id);

                ChildEntity ce = new ChildEntity();
                ce.childEntity = child;
                ce.vaList = GetVaccinationAppointment(vaList);
                ce.veList = GetVaccinationEvent(veList);
                ceList.Add(ce);
            }

            return ceList;
        }

		public List<ChildEntity> SearchByNameAndMother(string firstname1, string lastname1, string motherfirstname, string motherlastname, string mothersHivStatus, string mothersTT2Status)
        {
            int max = Int32.MaxValue;
            int start = 0;

            int statusId = 0;
            DateTime birthdateFrom = new DateTime();
            DateTime birthdateTo = new DateTime();

            int birthplaceId = 0;
            int communityId = 0;
            int domicileId = 0;

            string idFields = null;
            string systemId = null;
            string barcodeId = null;
            string tempId = null;

            int healthFacilityId = 0;

            List<Child> childList = Child.GetPagedChildList(statusId, birthdateFrom, birthdateTo, firstname1, lastname1, idFields,
                healthFacilityId.ToString(), birthplaceId, communityId, domicileId, motherfirstname, motherlastname,mothersHivStatus,mothersTT2Status, systemId, barcodeId, tempId,
                ref max, ref start);

            return GetChildrenWithAppointmentAndEvents(childList);
        }

        public List<ChildEntity> SearchByName(string firstname1, string lastname1)
        {
            int max = Int32.MaxValue;
            int start = 0;

            int statusId = 0;
            DateTime birthdateFrom = new DateTime();
            DateTime birthdateTo = new DateTime();

            int birthplaceId = 0;
            int communityId = 0;
            int domicileId = 0;

            string motherfirstname = null;
            string motherlastname = null;
			string mothersHivStatus = null;
			string mothersTT2Status = null;

            string idFields = null;
            string systemId = null;
            string barcodeId = null;
            string tempId = null;

            string healthFacilityId = "";

            List<Child> childList = Child.GetPagedChildList(statusId, birthdateFrom, birthdateTo, firstname1, lastname1, idFields,
                healthFacilityId, birthplaceId, communityId, domicileId, motherfirstname, motherlastname,mothersHivStatus, mothersTT2Status, systemId, barcodeId, tempId,
                ref max, ref start);

            return GetChildrenWithAppointmentAndEvents(childList);
        }

        public List<ChildEntity> SearchByDate(DateTime birthdatefrom, DateTime birthdateto, int birthplaceid, int domicileid)
        {
            int max = Int32.MaxValue;
            int start = 0;

            int statusId = 0;
            int communityId = 0;

            string firstname1 = null;
            string lastname1 = null;
            string motherfirstname = null;
            string motherlastname = null;

			string mothersHivStatus = null;
			string mothersTT2Status = null;




            string idFields = null;
            string systemId = null;
            string barcodeId = null;
            string tempId = null;

            int healthFacilityId = 0;

            List<Child> childList = Child.GetPagedChildList(statusId, birthdatefrom, birthdateto, firstname1, lastname1, idFields,
                healthFacilityId.ToString(), birthplaceid, communityId, domicileid, motherfirstname, motherlastname,mothersHivStatus,mothersTT2Status, systemId, barcodeId, tempId,
                ref max, ref start);

            return GetChildrenWithAppointmentAndEvents(childList);
        }

        public bool ChildExists(string where)
        {
            string firstname1 = null;
            string lastname1 = null;
            string motherFirstname = null;
            string motherLastname = null;
            DateTime birthdate = new DateTime();
            bool gender = new bool();

            string[] s = where.Split('!');
            foreach (string s1 in s)
            {
                if (s1.Contains("lastname1"))
                {
                    lastname1 = s1.Substring(s1.IndexOf('=') + 1).ToUpper();
                }

                if (s1.Contains("firstname1"))
                {
                    lastname1 = s1.Substring(s1.IndexOf('=') + 1).ToUpper();
                }

                if (s1.Contains("motherFirstname"))
                {
                    motherFirstname = s1.Substring(s1.IndexOf('=') + 1).ToUpper();
                }

                if (s1.Contains("motherLastname"))
                {
                    motherLastname = s1.Substring(s1.IndexOf('=') + 1).ToUpper();
                }

                if (s1.Contains("birthdate"))
                {
                    string bd = s1.Substring(s1.IndexOf('=') + 1).ToUpper();
                    birthdate = DateTime.ParseExact(bd, "yyyy-MM-dd", CultureInfo.CurrentCulture);
                }

                if (s1.Contains("gender"))
                {
                    string g = s1.Substring(s1.IndexOf('=') + 1).ToUpper();
                    gender = g == "M" ? true : false;
                }
            }



            int count = Child.ChildExists(firstname1, lastname1, motherFirstname, motherLastname, birthdate, gender);

            return (count >= 1);
        }

        public bool ChildExistsByLastnameAndBirthdate(string lastname1, DateTime birthdate)
        {
            string firstname1 = null;
            string motherFirstname = null;
            string motherLastname = null;
            bool gender = new bool();

            int count = Child.ChildExists(firstname1, lastname1, motherFirstname, motherLastname, birthdate, gender);

            return (count >= 1);
        }

        public bool ChildExistsByLastnameAndBirthdateAndGender(string lastname1, string gender, DateTime birthdate)
        {
            string firstname1 = null;
            string motherFirstname = null;
            string motherLastname = null;
            bool _gender = (gender == "M");

            int count = Child.ChildExists(firstname1, lastname1, motherFirstname, motherLastname, birthdate, _gender);

            return (count >= 1);
        }

        public bool ChildExistsByMotherAndBirthdateAndGender(string lastname1, string motherFirstname, string motherLastname, string gender, DateTime birthdate)
        {
            string firstname1 = null;
            bool _gender = (gender == "M");

            int count = Child.ChildExists(firstname1, lastname1, motherFirstname, motherLastname, birthdate, _gender);

            return (count >= 1);
        }

        public IntReturnValue RegisterChildWeight(int childId, DateTime date, double weight, DateTime modifiedOn, int modifiedBy)
        {
            ChildWeight cw = new ChildWeight();

            cw.ChildId = childId;
            cw.Date = date;
            cw.Weight = weight;
            cw.ModifiedOn = modifiedOn;
            cw.ModifiedBy = modifiedBy;

            int i = ChildWeight.Insert(cw);
            IntReturnValue irv = new IntReturnValue();
            irv.id = i;
            return irv;
        }

        public IntReturnValue RegisterChildWeightBarcode(string barcode, DateTime date, double weight, DateTime modifiedOn, int modifiedBy)
        {
            GIIS.DataLayer.Child c = GIIS.DataLayer.Child.GetChildByBarcode(barcode);
            int i = -99;
            if (c != null)
            {
                int childId = GetActualChildId(c.Id);

                ChildWeight cw = ChildWeight.GetChildWeightByChildIdAndDate(childId, date);
                if (cw != null)
                {
                    cw.Weight = weight;
                    cw.ModifiedOn = modifiedOn;
                    cw.ModifiedBy = modifiedBy;
                    i = ChildWeight.Update(cw);
                }
                else
                {
                    cw = new ChildWeight();
                    cw.ChildId = childId;
                    cw.Date = date;
                    cw.Weight = weight;
                    cw.ModifiedOn = modifiedOn;
                    cw.ModifiedBy = modifiedBy;

                    i = ChildWeight.Insert(cw);
                }
            }
            IntReturnValue irv = new IntReturnValue();
            irv.id = i;
            return irv;
        }

        public List<Weight> GetWeight()
        {
            List<Weight> lw = Weight.GetWeightList();
            return lw;
        }

        public List<ChildEntity> GetChildById(int childId)
        {
            Child child = Child.GetChildById(childId);
            List<ChildEntity> ceList = new List<ChildEntity>();

            if (child != null)
            {
                List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(child.Id);
                List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(child.Id);

                ChildEntity ce = new ChildEntity();
                ce.childEntity = child;
                ce.vaList = vaList; // GetVaccinationAppointment(vaList);
                ce.veList = veList; // GetVaccinationEvent(veList);
                ceList.Add(ce);
            }

            return ceList;

        }

        public ChildListEntity GetChildByIdList(string childIdList, int userId)
        {
            if (userId > 0)
            {
                User user = User.GetUserById(userId);
                ChildListEntity cle = new ChildListEntity();

                List<Child> childList = Child.GetChildByIdList(childIdList, userId);
                cle.childList = childList;

                List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChildBefore(childIdList, user.Lastlogin, user.PrevLogin, userId);
                cle.vaList = vaList;

                List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEventBefore(childIdList, user.Lastlogin, user.PrevLogin, user.Id);
                cle.veList = veList;
                return cle;
            }
            else
                return null;
            //string[] childList = childIdList.Split(',');
            //User user = User.GetUserById(userId);

            //List<ChildEntity> ceList = new List<ChildEntity>();

            //List<Child> childList = Child.GetChildByIdList(childIdList, userId);

            //foreach (Child child in childList)
            //{
            //    List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEventBefore(child.Id, user.Lastlogin, user.PrevLogin, user.Id);
            //    List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChildBefore(child.Id, user.Lastlogin, user.Id);

            //    ChildEntity ce = new ChildEntity();
            //    ce.childEntity = child;
            //    ce.vaList = vaList;// GetVaccinationAppointment(vaList);
            //    ce.veList = veList; // GetVaccinationEvent(veList);

            //    ceList.Add(ce);
            //}

            //return ceList;
        }

        public ChildListEntity GetChildByIdListSince(string childIdList, int userId)
        {
            ////string[] childList = childIdList.Split(',');
            //User user = User.GetUserById(userId);

            //List<ChildEntity> ceList = new List<ChildEntity>();

            //List<Child> childList = Child.GetChildByIdListSince(childIdList, userId);

            //foreach (Child child in childList)
            //{
            //    List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(child.Id, user.Lastlogin, user.Id);
            //    List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(child.Id, user.Lastlogin, user.Id);

            //    ChildEntity ce = new ChildEntity();
            //    ce.childEntity = child;
            //    ce.vaList = vaList;// GetVaccinationAppointment(vaList);
            //    ce.veList = veList; // GetVaccinationEvent(veList);

            //    ceList.Add(ce);
            //}

            //return ceList;
            if (userId > 0)
            {
                User user = User.GetUserById(userId);
                ChildListEntity cle = new ChildListEntity();

                List<Child> childList = Child.GetChildByIdList(childIdList, userId);
                cle.childList = childList;

                List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(childIdList, user.Lastlogin, user.Id);
                cle.vaList = vaList;

                List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(childIdList, user.Lastlogin, user.Id);
                cle.veList = veList;
                return cle;
            }
            else
                return null;
        }
        public List<ChildEntity> GetChildByBarcodeList(string childList)
        {

            List<ChildEntity> ceList = new List<ChildEntity>();
            List<Child> chList = new List<Child>();
            string[] cList = childList.Split(',');
            foreach (string s in cList)
            {
                Child c = Child.GetChildByBarcode(s);
                chList.Add(c);
            }
            foreach (Child child in chList)
            {
                if (child != null)
                {
                    List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(child.Id);
                    List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(child.Id);

                    ChildEntity ce = new ChildEntity();
                    ce.childEntity = child;
                    ce.vaList = vaList;// GetVaccinationAppointment(vaList);
                    ce.veList = veList; // GetVaccinationEvent(veList);

                    ceList.Add(ce);
                }
            }

            return ceList;
        }

        private int GetActualChildId(int childId)
        {
            ChildMerges cm = ChildMerges.GetChildMergesBySubsumedId(childId);
            if (cm != null)
                return cm.ChildId;

            return childId;
        }






		/**
		 * All methods containing temp in their names are used as a temporary fix for old applications to ensure that they work with the new server changes, in the long run these methods should be deleted 
		 */
		public List<ChildEntityTemp> GetChildrenByHealthFacilityTemp(int healthFacilityId)
		{
			List<ChildTemp> childList = ChildTemp.GetChildByHealthFacilityId(healthFacilityId);


			List<ChildEntityTemp> ceList = new List<ChildEntityTemp>();

			foreach (ChildTemp child in childList)
			{
				List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(child.Id);
				List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(child.Id);

				ChildEntityTemp ce = new ChildEntityTemp();
				ce.childEntity = child;
				ce.vaList = vaList; // GetVaccinationAppointment(vaList);
				ce.veList = veList; // GetVaccinationEvent(veList);
				ceList.Add(ce);
			}

			return ceList;
		}


		public List<ChildTemp> SearchTemp(int statusId, DateTime birthdateFrom, DateTime birthdateTo, string firstname1, string lastname1, string otherId, int healthFacilityId,
			int birthplaceId, int communityId, int domicileId, string address, string phone, string mobile)
		{
			int max = Int32.MaxValue;
			int start = 0;

			string idFields = null;
			string motherFirstname = null;
			string motherLastname = null;
			string mothersHivStatus = null;
			string mothersTT2Status = null;


			string systemId = null;
			string barcodeId = null;
			string tempId = null;

			List<ChildTemp> childList = ChildTemp.GetPagedChildList(statusId, birthdateFrom, birthdateTo, firstname1, lastname1, idFields,
				healthFacilityId.ToString(), birthplaceId, communityId, domicileId, motherFirstname, motherLastname, mothersHivStatus, mothersTT2Status, systemId, barcodeId, tempId,
				ref max, ref start);

			return childList;
		}


		public ChildListEntityTemp GetChildrenByHealthFacilitySinceLastLoginTemp(int idUser)
		{
			if (idUser > 0)
			{
				User user = User.GetUserById(idUser);
				ChildListEntityTemp cle = new ChildListEntityTemp();

				List<ChildTemp> childList = ChildTemp.GetChildByHealthFacilityIdSinceLastLogin(idUser);
				cle.childList = childList;

				List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(user.Lastlogin, user.Id);
				cle.vaList = vaList;

				List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(user.Lastlogin, user.Id);
				cle.veList = veList;

				//List<ChildEntity> ceList = new List<ChildEntity>();

				//foreach (Child child in childList)
				//{

				//    ChildEntity ce = new ChildEntity();
				//    ce.childEntity = child;
				//    ce.vaList = vaList; // GetVaccinationAppointment(vaList);
				//    ce.veList = veList; // GetVaccinationEvent(veList);
				//    ceList.Add(ce);
				//}

				//return ceList;
				return cle;
			}
			else
				return null;
		}


		public ChildListEntityTemp GetChildrenByHealthFacilityBeforeLastLoginTemp(int idUser)
		{
			if (idUser > 0)
			{
				User user = User.GetUserById(idUser);
				ChildListEntityTemp cle = new ChildListEntityTemp();

				List<ChildTemp> childList = ChildTemp.GetChildByHealthFacilityIdBeforeLastLogin(idUser);
				cle.childList = childList;

				List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChildBefore(user.Lastlogin, user.PrevLogin, user.Id);
				cle.vaList = vaList;

				List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEventBefore(user.Lastlogin, user.PrevLogin, user.Id);
				cle.veList = veList;
				return cle;
			}
			else
				return null;

		}


		public ChildListEntityTemp GetChildrenByHealthFacilityDayFirstLoginTemp(int idUser)
		{
			if (idUser > 0)
			{
				User user = User.GetUserById(idUser);
				ChildListEntityTemp cle = new ChildListEntityTemp();

				List<ChildTemp> childList = ChildTemp.GetChildByHealthFacilityIdDayFirstLogin(idUser);
				cle.childList = childList;

				List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChildDayFirstLogin(user.Lastlogin.AddDays(-1), user.Id);
				cle.vaList = vaList;

				List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEventDayFirstLogin(user.Lastlogin.AddDays(-1), user.Id);
				cle.veList = veList;
				return cle;
			}
			else
				return null;
		}



		public List<ChildEntityTemp> SearchByBarcodeTemp(string barcodeId)
		{
			GIIS.DataLayer.ChildTemp child = GIIS.DataLayer.ChildTemp.GetChildByBarcode(barcodeId);

			List<ChildEntityTemp> ceList = new List<ChildEntityTemp>();

			if (child != null)
			{
				List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(child.Id);
				List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(child.Id);

				ChildEntityTemp ce = new ChildEntityTemp();
				ce.childEntity = child;
				ce.vaList = vaList; // GetVaccinationAppointment(vaList);
				ce.veList = veList; // GetVaccinationEvent(veList);
				ceList.Add(ce);
			}

			return ceList;
		}


		public List<ChildEntityTemp> GetChildByIdTemp(int childId)
		{
			ChildTemp child = ChildTemp.GetChildById(childId);
			List<ChildEntityTemp> ceList = new List<ChildEntityTemp>();

			if (child != null)
			{
				List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(child.Id);
				List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(child.Id);

				ChildEntityTemp ce = new ChildEntityTemp();
				ce.childEntity = child;
				ce.vaList = vaList; // GetVaccinationAppointment(vaList);
				ce.veList = veList; // GetVaccinationEvent(veList);
				ceList.Add(ce);
			}

			return ceList;

		}

		public ChildListEntityTemp GetChildByIdListTemp(string childIdList, int userId)
		{
			if (userId > 0)
			{
				User user = User.GetUserById(userId);
				ChildListEntityTemp cle = new ChildListEntityTemp();

				List<ChildTemp> childList = ChildTemp.GetChildByIdList(childIdList, userId);
				cle.childList = childList;

				List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChildBefore(childIdList, user.Lastlogin, user.PrevLogin, userId);
				cle.vaList = vaList;

				List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEventBefore(childIdList, user.Lastlogin, user.PrevLogin, user.Id);
				cle.veList = veList;
				return cle;
			}
			else
				return null;
		
		}


		public List<ChildEntityTemp> GetChildByBarcodeListTemp(string childList)
		{

			List<ChildEntityTemp> ceList = new List<ChildEntityTemp>();
			List<ChildTemp> chList = new List<ChildTemp>();
			string[] cList = childList.Split(',');
			foreach (string s in cList)
			{
				ChildTemp c = ChildTemp.GetChildByBarcode(s);
				chList.Add(c);
			}
			foreach (ChildTemp child in chList)
			{
				if (child != null)
				{
					List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(child.Id);
					List<GIIS.DataLayer.VaccinationAppointment> vaList = GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(child.Id);

					ChildEntityTemp ce = new ChildEntityTemp();
					ce.childEntity = child;
					ce.vaList = vaList;// GetVaccinationAppointment(vaList);
					ce.veList = veList; // GetVaccinationEvent(veList);

					ceList.Add(ce);
				}
			}

			return ceList;
		}


		private List<ChildEntityTemp> GetChildrenWithAppointmentAndEventsTemp(List<ChildTemp> childList)
		{
			List<ChildEntityTemp> ceList = new List<ChildEntityTemp>();

			foreach (ChildTemp child in childList)
			{
				List<GIIS.DataLayer.VaccinationEvent> veList = new List<DataLayer.VaccinationEvent>(); // GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(child.Id, child.ModifiedOn);
				List<GIIS.DataLayer.VaccinationAppointment> vaList = new List<VaccinationAppointment>(); // GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentsByChild(child.Id, child.ModifiedOn);

				ChildEntityTemp ce = new ChildEntityTemp();
				ce.childEntity = child;
				ce.vaList = GetVaccinationAppointment(vaList);
				ce.veList = GetVaccinationEvent(veList);
				ceList.Add(ce);
			}

			return ceList;
		}

	}
}
