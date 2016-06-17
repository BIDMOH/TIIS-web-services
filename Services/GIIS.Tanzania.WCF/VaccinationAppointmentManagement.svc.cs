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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "VaccinationAppointmentManagement" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select VaccinationAppointmentManagement.svc or VaccinationAppointmentManagement.svc.cs at the Solution Explorer and start debugging.
    public class VaccinationAppointmentManagement : IVaccinationAppointmentManagement
    {
        public IntReturnValue RegisterVaccinationAppointment(int childId, int scheduledFacilityId, DateTime scheduledDate, string notes, bool isActive, int userId)
        {
            VaccinationAppointment o = new VaccinationAppointment();

            o.ChildId = childId;
            o.ScheduledFacilityId = scheduledFacilityId;
            o.ScheduledDate = scheduledDate;
            o.Notes = notes.Replace("'", @"''");
            o.IsActive = isActive;
            o.ModifiedOn = DateTime.Now;
            o.ModifiedBy = userId;

            int inserted = VaccinationAppointment.Insert(o);
            IntReturnValue irv = new IntReturnValue();
            irv.id = inserted;
            return irv;
         
        }

        public IntReturnValue UpdateVaccinationAppointment(int childId, int scheduledFacilityId, DateTime scheduledDate, string notes, bool isActive, int userId,
            int vaccinationAppointmentId)
        {
            VaccinationAppointment o = VaccinationAppointment.GetVaccinationAppointmentById(vaccinationAppointmentId);

            o.ChildId = childId;
            o.ScheduledFacilityId = scheduledFacilityId;
            o.ScheduledDate = scheduledDate;
            o.Notes = notes.Replace("'", @"''");
            o.IsActive = isActive;
            o.ModifiedOn = DateTime.Now;
            o.ModifiedBy = userId;

            int updated = VaccinationAppointment.Update(o);
            IntReturnValue irv = new IntReturnValue();
            irv.id = updated;
            return irv;
        }

        public IntReturnValue UpdateVaccinationApp(bool outreach, int userId, string barcode, int doseId)
        {

            GIIS.DataLayer.Child child = GIIS.DataLayer.Child.GetChildByBarcode(barcode);
            //if barcode isnt found give -99
            int i = -99;

            if (child != null)
            {
               
                GIIS.DataLayer.VaccinationEvent ve = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventByChildIdAndDoseId(child.Id, doseId);
                VaccinationAppointment o = VaccinationAppointment.GetVaccinationAppointmentById(ve.AppointmentId);

                o.Outreach = outreach;
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                i = VaccinationAppointment.Update(o);
               
            }
         
            IntReturnValue irv = new IntReturnValue();
            irv.id = i;
            return irv;
        }

        public int RemoveVaccinationAppointment(int id)
        {
            int removed = VaccinationAppointment.Remove(id);
            return removed;
        }

        public int DeleteVaccinationAppointment(int id)
        {
            int deleted = VaccinationAppointment.Delete(id);
            return deleted;
        }

        public List<VaccinationAppointment> GetVaccinationAppointmentsByChildId(int childId)
        {
            List<VaccinationAppointment> vaccinationAppList = VaccinationAppointment.GetVaccinationAppointmentsByChild(childId);

            return vaccinationAppList;
        }

        public IntReturnValue RegisterChildAEFI(int appId, bool aefi, DateTime date, string notes, DateTime modifiedOn, int modifiedBy)
        {
            int updated = -1;
            VaccinationAppointment va = VaccinationAppointment.GetVaccinationAppointmentById(appId);
            if (va != null)
            {
                va.Aefi = aefi;
                va.AefiDate = date;
                va.Notes = notes.Replace("'", @"''");
                va.ModifiedOn = modifiedOn;
                va.ModifiedBy = modifiedBy;

               updated = VaccinationAppointment.Update(va);
            }
                IntReturnValue irv = new IntReturnValue();
                irv.id = updated;
                return irv;
        }
        public IntReturnValue RegisterChildAEFIBarcode(string barcode, int doseId, bool aefi, DateTime date, string notes, DateTime modifiedOn, int modifiedBy)
        {
            GIIS.DataLayer.Child c = GIIS.DataLayer.Child.GetChildByBarcode(barcode);
            int updated = -99;
            if (c != null)
            {
                //VaccinationAppointment va = VaccinationAppointment.GetVaccinationAppointmentById(appId);
                GIIS.DataLayer.VaccinationEvent o = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventByChildIdAndDoseId(c.Id, doseId);
                if (o != null)
                {
                    VaccinationAppointment va = VaccinationAppointment.GetVaccinationAppointmentById(o.AppointmentId);
                    va.Aefi = aefi;
                    va.AefiDate = date;
                    va.Notes = notes.Replace("'", @"''");
                    va.ModifiedOn = modifiedOn;
                    va.ModifiedBy = modifiedBy;

                    updated = VaccinationAppointment.Update(va);
                }
            }
            IntReturnValue irv = new IntReturnValue();
            irv.id = updated;
            return irv;
        }
    }
}