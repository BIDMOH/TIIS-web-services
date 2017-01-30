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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StockManagement" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select StockManagement.svc or StockManagement.svc.cs at the Solution Explorer and start debugging.
    public class StockManagement : IStockManagement
    {
        public List<BalanceEntity> GetCurrentStockByLot(int hfId)
        {

            List<HealthFacilityBalance> currentStock = HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacility(hfId);

            List<BalanceEntity> blist = new List<BalanceEntity>();

			if (currentStock != null)
			{
				foreach (HealthFacilityBalance hb in currentStock)
				{
					ItemManufacturer im = ItemManufacturer.GetItemManufacturerByGtin(hb.Gtin);
					BalanceEntity be = new BalanceEntity();
					//change hfcode to lotid
					be.Gtin = hb.Gtin;
					be.LotNumber = hb.LotNumber;
					be.Item = hb.GtinObject.ItemObject.Code;
					be.Balance = hb.Balance.ToString();
					ItemLot lot = ItemLot.GetItemLotByGtinAndLotNo(hb.Gtin, hb.LotNumber);
					if (lot != null)
					{
						be.ExpireDate = lot.ExpireDate;
						be.LotId = lot.Id.ToString();
					}
					be.Gtin_IsActive = im.IsActive;
					be.Lot_IsActive = lot.IsActive;
					GtinHfStockPolicy ghsp = GtinHfStockPolicy.GetGtinHfStockPolicyByHealthFacilityCodeAndGtin(hb.HealthFacilityCode, hb.Gtin);
					if (ghsp != null)
						be.ReorderQty = ghsp.SafetyStock.ToString();
					else
						be.ReorderQty = "0";
					blist.Add(be);

				}
			}

            return blist;
        }


		public List<HealthFacilityDoseBalanceEntity> GetHealthFacilityCurrentStockByDose(int hfId, DateTime fromDate, DateTime toDate)
		{
			List<HealthFacilityDoseBalanceEntity> blist = new List<HealthFacilityDoseBalanceEntity>();

			List<ScheduledVaccination> scheduledVaccines = ScheduledVaccination.GetScheduledVaccinationList();
			foreach (ScheduledVaccination v in scheduledVaccines)
			{
				if (v.IsActive)
				{
					HealthFacilityDoseBalanceEntity balanceEntity = new HealthFacilityDoseBalanceEntity();
					string antigen = v.Name;
					balanceEntity.antigen = antigen;
					balanceEntity.stockOnHand = HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacilityIdAndDose(hfId, antigen);
					balanceEntity.dosesReceived = HealthFacilityBalance.GetHealthFacilityReceivedDosesByHealthFacilityIdAndDose(hfId, antigen, fromDate, toDate);
					balanceEntity.dosesDiscardedUnopened = -1*HealthFacilityBalance.GetHealthFacilityDosesDiscardedUnoppened(hfId, antigen, fromDate, toDate);
					balanceEntity.dosesDiscardedOpened = HealthFacilityBalance.GetHealthFacilityDosesDiscardedOpened(hfId, antigen, fromDate, toDate);
					balanceEntity.childrenImmunized = HealthFacility.GetHealthFacilityVaccinationsByScheduledVaccinationId(hfId, v.Id, fromDate, toDate);

					balanceEntity.openingBalance = balanceEntity.stockOnHand - HealthFacilityBalance.GetHealthFacilityDoseInAllTransactions(hfId, antigen, fromDate, toDate);

					blist.Add(balanceEntity);
				}
			}

			return blist;
		}



        public List<LotNumbers> GetItemLots()
        {
            List<ItemLot> itemlots = ItemLot.GetItemLotList();

			try
			{
				List<LotNumbers> lots = new List<LotNumbers>();
				foreach (ItemLot il in itemlots)
				{
					LotNumbers ln = new LotNumbers();
					ln.Gtin = il.Gtin;
					ln.LotNumber = il.LotNumber;
					ln.ExpireDate = il.ExpireDate;
					ln.Id = il.Id;
					ln.ItemId = il.ItemId;
					ln.Notes = il.Notes;
					lots.Add(ln);
				}
				return lots;
			}
			catch (Exception e)
			{
				throw e;
			}
        }

        public int GetItemLotByDoseId(int id)
        {
            int count = ItemLot.GetItemLotByDoseId(id);

            return count;

        }

        public List<AdjustmentReason> GetAdjustmentReasons()
        {
            List<AdjustmentReason> arlist = AdjustmentReason.GetRealAdjustmentReasonList();
            return arlist;
        }

        public IntReturnValue StockCount(string  gtin, string lotno, int qty,  DateTime date, int userId)
        {
            int hfId = User.GetUserById(userId).HealthFacilityId;
            HealthFacility hf = HealthFacility.GetHealthFacilityById(hfId);
            GIIS.BusinessLogic.StockManagementLogic sml = new GIIS.BusinessLogic.StockManagementLogic();
            UInt32 quantity = (UInt32)qty;
            ItemTransaction st = sml.StockCount(hf, gtin, lotno, quantity, userId, date);
            int i = st.Id;

            IntReturnValue irv = new IntReturnValue();
            irv.id = i;
            return irv;
        }

        public IntReturnValue StockAdjustment(string gtin, string lotno, int qty, DateTime date, int reasonId, int userId)
        {
            int hfId = User.GetUserById(userId).HealthFacilityId;
            HealthFacility hf = HealthFacility.GetHealthFacilityById(hfId);
            AdjustmentReason adr = AdjustmentReason.GetAdjustmentReasonById(reasonId);
            GIIS.BusinessLogic.StockManagementLogic sml = new GIIS.BusinessLogic.StockManagementLogic();
            ItemTransaction st = sml.Adjust(hf, gtin, lotno, qty, adr, userId, date);

            int i = st.Id;

            IntReturnValue irv = new IntReturnValue();
            irv.id = i;
            return irv;
        }
    }
}
