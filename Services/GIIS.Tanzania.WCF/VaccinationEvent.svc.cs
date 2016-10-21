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
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GIIS.BusinessLogic;
using GIIS.DataLayer;

namespace GIIS.Tanzania.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "VaccinationEvent" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select VaccinationEvent.svc or VaccinationEvent.svc.cs at the Solution Explorer and start debugging.
    public class VaccinationEvent : IVaccinationEvent
    {
        public IntReturnValue RegisterVaccinationEvent(int appointmentId, int childId, int doseId, int vaccineLotId, string vaccineLotText, int healthFacilityId,
            DateTime scheduledDate, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId, bool isActive, int userId)
        {
            GIIS.DataLayer.VaccinationEvent o = new GIIS.DataLayer.VaccinationEvent();

            o.AppointmentId = appointmentId;
            o.ChildId = childId;
            o.DoseId = doseId;
            o.VaccineLotId = vaccineLotId;
            o.VaccineLotText = vaccineLotText;
            o.HealthFacilityId = healthFacilityId;
            o.ScheduledDate = scheduledDate;
            o.VaccinationDate = vaccinationDate;
            o.Notes = notes;
            o.VaccinationStatus = vaccinationStatus;
            o.NonvaccinationReasonId = nonvaccinationReasonId;
            o.IsActive = isActive;
            o.ModifiedOn = DateTime.Now;
            o.ModifiedBy = userId;

            int inserted = GIIS.DataLayer.VaccinationEvent.Insert(o);
            IntReturnValue irv = new IntReturnValue();
            irv.id = inserted;
            return irv;

        }
        public IntReturnValue RegisterVaccinationEvent2(int appointmentId, int childId, int doseId, int healthFacilityId,
           DateTime scheduledDate, DateTime vaccinationDate, string notes, bool isActive, int userId)
        {
            GIIS.DataLayer.VaccinationEvent o = new GIIS.DataLayer.VaccinationEvent();

            o.AppointmentId = appointmentId;
            o.ChildId = childId;
            o.DoseId = doseId;
            o.VaccineLotId = 0;
            //o.VaccineLotText = vaccineLotText.Replace("'", @"''");
            o.HealthFacilityId = healthFacilityId;
            o.ScheduledDate = scheduledDate;
            o.VaccinationDate = vaccinationDate;
            o.Notes = notes;
            o.VaccinationStatus = false;
            o.NonvaccinationReasonId = 0;
            o.IsActive = isActive;
            o.ModifiedOn = DateTime.Now;
            o.ModifiedBy = userId;

            int inserted = GIIS.DataLayer.VaccinationEvent.Insert(o);
            IntReturnValue irv = new IntReturnValue();
            irv.id = inserted;
            return irv;
        }

        public IntReturnValue UpdateVaccinationEvent(int vaccineLotId, int healthFacilityId, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId, int userId, int vaccinationEventId)
        {
            int updated = -1;
            GIIS.DataLayer.VaccinationEvent o = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventById(vaccinationEventId);
            if (o != null)
            {
                int childId = GetActualChildId(o.ChildId);
                o.ChildId = childId;

                o.VaccineLotId = vaccineLotId;
                o.HealthFacilityId = healthFacilityId;
                o.VaccinationDate = vaccinationDate;
                int datediff = 0;
                if (o.ScheduledDate != vaccinationDate)
                    datediff = vaccinationDate.Subtract(o.ScheduledDate).Days;

                o.Notes = notes;
                o.VaccinationStatus = vaccinationStatus;
                o.NonvaccinationReasonId = nonvaccinationReasonId;
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                updated = GIIS.DataLayer.VaccinationEvent.Update(o);
                if (updated > 0)
                {
                    UpdateNextDose(o, datediff);
                    StockManagementLogic sml = new StockManagementLogic();
                    GIIS.DataLayer.ItemTransaction it = sml.Vaccinate(o.HealthFacility, o);
                }
            }
            IntReturnValue irv = new IntReturnValue();
            irv.id = updated;
            return irv;
        }
        private void UpdateNextDose(GIIS.DataLayer.VaccinationEvent o, int datediff)
        {
            int dosenum = o.Dose.DoseNumber + 1;
            if (dosenum > 1)
            {
                int vid = GIIS.DataLayer.VaccinationEvent.NextDose(o.Dose.ScheduledVaccinationId, o.ChildId, dosenum);
                if (vid != -1)
                {
                    GIIS.DataLayer.VaccinationEvent v = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventById(vid);
                    GIIS.DataLayer.VaccinationEvent.UpdateIsActive(vid, true);
                    GIIS.DataLayer.VaccinationEvent.UpdateEvent(vid, v.ScheduledDate.AddDays(datediff));

                    //update others
                    List<GIIS.DataLayer.VaccinationEvent> vl = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventByAppointmentId(o.AppointmentId);
                    foreach (GIIS.DataLayer.VaccinationEvent ve in vl)
                    {
                        if (ve.Id != o.Id)
                        {
                            if (ve.VaccinationStatus || ve.NonvaccinationReasonId != 0)
                            {
                                if (ve.VaccinationDate < o.VaccinationDate)
                                {
                                    GIIS.DataLayer.VaccinationEvent nv = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventById(GIIS.DataLayer.VaccinationEvent.NextDose(ve.Dose.ScheduledVaccinationId, ve.ChildId, dosenum));
                                    GIIS.DataLayer.VaccinationEvent vn = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventById(vid);
                                    GIIS.DataLayer.VaccinationEvent.UpdateEvent(nv.Id, vn.ScheduledDate);
                                }
                            }
                        }
                    }
                }
            }
        }
        public IntReturnValue UpdateVaccinationEventById(int vaccinationEventId, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId)
        {
            GIIS.DataLayer.VaccinationEvent o = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventById(vaccinationEventId);

            o.VaccinationDate = vaccinationDate;
            o.Notes = notes;
            o.VaccinationStatus = vaccinationStatus;
            o.NonvaccinationReasonId = nonvaccinationReasonId;
            o.ModifiedOn = DateTime.Now;
            o.ModifiedBy = 1;

            int updated = GIIS.DataLayer.VaccinationEvent.Update(o);
            IntReturnValue irv = new IntReturnValue();
            irv.id = updated;
            return irv;
        }

        public IntReturnValue UpdateVaccinationEventChildIdAndDoseId(int childId, int doseId, int vaccineLotId, int healthFacilityId, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId, int userId)
        {
            int cId = GetActualChildId(childId);

            GIIS.DataLayer.VaccinationEvent o = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventByChildIdAndDoseId(cId, doseId);
            int i = -1;
            if (o != null)
            {
                o.VaccineLotId = vaccineLotId;
                o.HealthFacilityId = healthFacilityId;
                o.VaccinationDate = vaccinationDate;
                int datediff = 0;
                if (o.ScheduledDate != vaccinationDate)
                    datediff = vaccinationDate.Subtract(o.ScheduledDate).Days;
                o.Notes = notes;
                o.VaccinationStatus = vaccinationStatus;
                o.NonvaccinationReasonId = nonvaccinationReasonId;
                NonvaccinationReason nvr = NonvaccinationReason.GetNonvaccinationReasonById(nonvaccinationReasonId);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                i = GIIS.DataLayer.VaccinationEvent.Update(o);
                if (i > 0)
                {
                    if (vaccinationStatus || nvr.KeepChildDue == false)
                    {
                        UpdateNextDose(o, datediff);
                        if (vaccineLotId > 0)
                        {
                            StockManagementLogic sml = new StockManagementLogic();
                            sml.Vaccinate(o.HealthFacility, o);
                        }
                    }
                }
            }
            IntReturnValue irv = new IntReturnValue();
            irv.id = i;
            return irv;
        }

        public IntReturnValue UpdateVaccinationEventBarcodeAndDoseId(string barcode, int doseId, int vaccineLotId, int healthFacilityId, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId, int userId, DateTime modifiedOn)
        {
            int i = -99;
            if (!string.IsNullOrEmpty(barcode))
            {
                GIIS.DataLayer.Child child = GIIS.DataLayer.Child.GetChildByBarcode(barcode);
                //if barcode isnt found give -99
                i = -99;
                if (child != null)
                {
                    int cId = GetActualChildId(child.Id);

                    GIIS.DataLayer.VaccinationEvent o = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventByChildIdAndDoseId(cId, doseId);

					GIIS.DataLayer.NonvaccinationReason nonVaccinationreason = new NonvaccinationReason();

					if (o.NonvaccinationReasonId == 0)
					{
						nonVaccinationreason.KeepChildDue = true;
					}
					else 
					{
						nonVaccinationreason = GIIS.DataLayer.NonvaccinationReason.GetNonvaccinationReasonById(o.NonvaccinationReasonId);
					}

					if (o != null && (!o.VaccinationStatus) && nonVaccinationreason.KeepChildDue && (vaccinationStatus || nonvaccinationReasonId > 0))
					{
						o.VaccineLotId = vaccineLotId;
						o.HealthFacilityId = healthFacilityId;
						o.VaccinationDate = vaccinationDate;
						int datediff = 0;
						if (o.ScheduledDate != vaccinationDate)
							datediff = vaccinationDate.Subtract(o.ScheduledDate).Days;
						o.Notes = notes;
						o.VaccinationStatus = vaccinationStatus;
						o.NonvaccinationReasonId = nonvaccinationReasonId;
						NonvaccinationReason.GetNonvaccinationReasonById(nonvaccinationReasonId);
						o.ModifiedOn = modifiedOn;
						o.ModifiedBy = userId;


						VaccinationLogic vl = new VaccinationLogic();
						GIIS.DataLayer.VaccinationEvent ve = vl.UpdateVaccinationEvent(o.Id, vaccineLotId, vaccinationDate, healthFacilityId, vaccinationStatus, nonvaccinationReasonId, userId, modifiedOn);
						if (ve != null)
							i = 1;
					}
					else if (o.VaccinationStatus)
					{
						i = 1; //vaccination already given 
					}
					else
						i = 0; // vaccination not being modified by url
                }

            }
            IntReturnValue irv = new IntReturnValue();
            irv.id = i;
            return irv;
        }
        public IntReturnValue UpdateVaccinationEventByBarcode(string barcode, int doseId, int vaccineLotId, int healthFacilityId, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId, int userId)
        {
            GIIS.DataLayer.Child child = GIIS.DataLayer.Child.GetChildByBarcode(barcode);
            //if barcode isnt found give -99
            int i = -99;

            if (child != null)
            {
                int cId = GetActualChildId(child.Id);

                GIIS.DataLayer.VaccinationEvent o = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventByChildIdAndDoseId(cId, doseId);

                o.VaccineLotId = vaccineLotId;
                o.HealthFacilityId = healthFacilityId;
                o.VaccinationDate = vaccinationDate;
                int datediff = 0;
                if (o.ScheduledDate != vaccinationDate)
                    datediff = vaccinationDate.Subtract(o.ScheduledDate).Days;
                o.Notes = notes;
                o.VaccinationStatus = vaccinationStatus;
                o.NonvaccinationReasonId = nonvaccinationReasonId;
                NonvaccinationReason nvr = NonvaccinationReason.GetNonvaccinationReasonById(nonvaccinationReasonId);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                i = GIIS.DataLayer.VaccinationEvent.Update(o);
                if (i > 0)
                {
                    if (vaccinationStatus || nvr.KeepChildDue == false)
                    {
                        UpdateNextDose(o, datediff);
                        if (vaccineLotId > 0)
                        {
                            StockManagementLogic sml = new StockManagementLogic();
                            GIIS.DataLayer.ItemTransaction it = sml.Vaccinate(o.HealthFacility, o);
                        }
                    }
                }

            }

            IntReturnValue irv = new IntReturnValue();
            irv.id = i;
            return irv;
        }

        public IntReturnValue UpdateVaccinationEventByBarcodeVaccine(string barcodeId, int vaccineId, int vaccinelot, int healthFacilityId, DateTime vaccinationDate, string notes, bool vaccinationStatus, int nonvaccinationReasonId, int userId)
        {
            int i = -1;
            if (!string.IsNullOrEmpty(barcodeId))
            {
                GIIS.DataLayer.Child child = GIIS.DataLayer.Child.GetChildByBarcode(barcodeId);

				if (child != null)
				{
					i = 0;
					int cId = GetActualChildId(child.Id);

					GIIS.DataLayer.VaccinationEvent ve = null;
					List<GIIS.DataLayer.Dose> dl = GIIS.DataLayer.Dose.GetDoseByVaccine(vaccineId);
					bool found = false;
					foreach (GIIS.DataLayer.Dose d in dl)
					{
						GIIS.DataLayer.VaccinationEvent o = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventByChildIdAndDoseId(cId, d.Id);
						if (o.IsActive && (!o.VaccinationStatus) && o.NonvaccinationReasonId == 0)
						{
							found = true;
							ve = o;
						}

					}
					i = 4;
					//if (found && ve != null)
					//{
					//	ve.VaccineLotId = vaccinelot;
					//	ve.HealthFacilityId = healthFacilityId;
					//	ve.VaccinationDate = vaccinationDate;
					//	int datediff = 0;
					//	if (ve.ScheduledDate != vaccinationDate)
					//		datediff = vaccinationDate.Subtract(ve.ScheduledDate).Days;
					//	ve.Notes = notes;
					//	ve.VaccinationStatus = vaccinationStatus;
					//	ve.NonvaccinationReasonId = nonvaccinationReasonId;
					//	NonvaccinationReason nvr = NonvaccinationReason.GetNonvaccinationReasonById(nonvaccinationReasonId);
					//	ve.ModifiedOn = DateTime.Now;
					//	ve.ModifiedBy = userId;

					//	VaccinationLogic vl = new VaccinationLogic();
					//	GIIS.DataLayer.VaccinationEvent o = vl.UpdateVaccinationEvent(ve.Id, vaccinelot, vaccinationDate, healthFacilityId, vaccinationStatus, nonvaccinationReasonId, userId, DateTime.Now);
					//	if (o != null)
					//		i = 1;
					//	//i = GIIS.DataLayer.VaccinationEvent.Update(ve);
					//	//if (i > 0)
					//	//{
					//	//    if (vaccinationStatus || nvr.KeepChildDue == false)
					//	//    {
					//	//        UpdateNextDose(ve, datediff);
					//	//        if (vaccinelot > 0)
					//	//        {
					//	//            StockManagementLogic sml = new StockManagementLogic();
					//	//            GIIS.DataLayer.ItemTransaction it = sml.Vaccinate(ve.HealthFacility, ve);
					//	//        }
					//	//    }
					//	//}
					//}
				}
				else {
					//if barcode isnt found give -99
					i = -99;
				}
            }
            IntReturnValue irv = new IntReturnValue();
            irv.id = i;
            return irv;
        }

        public IntReturnValue UpdateVaccinationEventByBarcodeAndAppId(string barcodeId, int appointmentId, DateTime vaccinationDate, string notes, bool vaccinationStatus)
        {
            GIIS.DataLayer.Child child = GIIS.DataLayer.Child.GetChildByBarcode(barcodeId);

            List<GIIS.DataLayer.VaccinationEvent> veList = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventByAppointmentId(appointmentId);

            int updated = -1;

            if (child != null)
            {
                foreach (GIIS.DataLayer.VaccinationEvent o in veList)
                {
                    o.VaccinationDate = vaccinationDate;
                    o.Notes = notes;
                    o.VaccinationStatus = vaccinationStatus;
                    o.ModifiedOn = DateTime.Now;
                    o.ModifiedBy = 1;

                    updated = GIIS.DataLayer.VaccinationEvent.Update(o);
                }
            }
            IntReturnValue irv = new IntReturnValue();
            irv.id = updated;
            return irv;
        }

        public int RemoveVaccinationEvent(int id)
        {
            int removed = GIIS.DataLayer.VaccinationEvent.Remove(id);
            return removed;
        }

        public int DeleteVaccinationEvent(int id)
        {
            int deleted = GIIS.DataLayer.VaccinationEvent.Delete(id);
            return deleted;
        }

        public List<GIIS.DataLayer.VaccinationEvent> GetMonthlyPlan(int healthcenterId, DateTime currentDate)
        {
            //int max = Int32.MaxValue;
            //int start = 0;

            //List<GIIS.DataLayer.VaccinationEvent> vaccinationEvent = GIIS.DataLayer.VaccinationEvent.GetMonthlyPlan(ref max, ref start, healthcenterId, currentDate);
            //return GetModifiedVaccinationEventList(vaccinationEvent);

            return null;
        }

        public List<GIIS.DataLayer.VaccinationEvent> GetImmunizationCard(int childId)
        {
            List<GIIS.DataLayer.VaccinationEvent> immunizationCardList = GIIS.DataLayer.VaccinationEvent.GetImmunizationCard(childId);
            return immunizationCardList;
        }

        public List<GIIS.DataLayer.VaccinationEvent> GetVaccinationEventListByChildId(int childId)
        {
            List<GIIS.DataLayer.VaccinationEvent> vaccinationEventList = GIIS.DataLayer.VaccinationEvent.GetChildVaccinationEvent(childId);
            return vaccinationEventList;
        }

        public IntReturnValue UpdateVaccinationQueue(string barcode, int hfId, DateTime date, int userId)
        {
            GIIS.DataLayer.VaccinationQueue vcq = new DataLayer.VaccinationQueue();
            vcq.BarcodeId = barcode;
            vcq.HealthFacilityId = hfId;
            vcq.Date = date;
            vcq.UserId = userId;

            int id = DataLayer.VaccinationQueue.Insert(vcq);
            IntReturnValue irv = new IntReturnValue();
            irv.id = id;
            return irv;
        }


        public List<VaccinationEventEntity> GetDueVaccinesForChild(int childId, DateTime scheduledDate)
        {
            DataTable dt = GIIS.DataLayer.VaccinationEvent.GetDueVaccinesForChild(childId, scheduledDate);

            List<VaccinationEventEntity> veeList = new List<VaccinationEventEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                VaccinationEventEntity vee = new VaccinationEventEntity();
                vee.AppointmentId = int.Parse(dr[0].ToString());
                vee.Vaccines = dr[1].ToString();
                vee.Schedule = dr[2].ToString();
                vee.ScheduledDate = DateTime.Parse(dr[3].ToString());

                veeList.Add(vee);
            }

            return veeList;
        }

        public List<VaccinationEventEntity> GetDefaultersList()
        {
            DataTable dt = GIIS.DataLayer.VaccinationEvent.GetDefaultersList();

            List<VaccinationEventEntity> veeList = new List<VaccinationEventEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                VaccinationEventEntity vee = new VaccinationEventEntity();
                vee.ChildId = int.Parse(dr[0].ToString());
                vee.ChildName = dr[1].ToString();
                vee.Vaccines = dr[2].ToString();
                vee.ScheduledDate = DateTime.Parse(dr[3].ToString());

                veeList.Add(vee);
            }

            return veeList;
        }

        public List<IntReturnValue> GetDefaulters()
        {
            DataTable dt = GIIS.DataLayer.VaccinationEvent.GetDefaulters();

            List<IntReturnValue> veeList = new List<IntReturnValue>();
            foreach (DataRow dr in dt.Rows)
            {
                IntReturnValue vee = new IntReturnValue();
                vee.id = int.Parse(dr[0].ToString());

                veeList.Add(vee);
            }

            return veeList;
        }


        public List<List<string>> GetDefaultersWithDates()
        {
            DataTable dt = GIIS.DataLayer.VaccinationEvent.GetDefaultersWithModifiedOn();

            List<List<string>> veeList = new List<List<string>>();
            foreach (DataRow dr in dt.Rows)
            {
                List<string> vee = new List<string>();
                vee.Add(dr[0].ToString());
                vee.Add(dr[1].ToString());

                veeList.Add(vee);
            }

            return veeList;
        }

        private int GetActualChildId(int childId)
        {
            ChildMerges cm = ChildMerges.GetChildMergesBySubsumedId(childId);
            if (cm != null)
                return cm.ChildId;

            return childId;
        }

		private string TrimLastCharacter(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			else {
				return str.TrimEnd(str[str.Length - 1]);
			}
		}
    }
}
