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
using System.ServiceModel.Web;
using System.Text;

namespace GIIS.Tanzania.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IStockManagement" in both code and config file together.
    [ServiceContract]
    public interface IStockManagement
    {
        [WebGet(UriTemplate = "GetCurrentStockByLot?hfid={hfid}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<BalanceEntity> GetCurrentStockByLot(int hfId);


		[WebGet(UriTemplate = "GetCurrentStockByDose?hfid={hfid}&fromDate={fromDate}&toDate={toDate}", ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		List<HealthFacilityDoseBalanceEntity> GetCurrentStockByDose(int hfId, DateTime fromDate, DateTime toDate);


        [WebGet(UriTemplate = "GetItemLots", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<LotNumbers> GetItemLots();

        [WebGet(UriTemplate = "GetItemLotByDoseId?id={id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        int GetItemLotByDoseId(int id);

        [WebGet(UriTemplate = "GetAdjustmentReasons", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<AdjustmentReason> GetAdjustmentReasons();

        [WebGet(UriTemplate = "StockCount?gtin={gtin}&lotno={lotno}&qty={qty}&date={date}&userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue StockCount(string gtin, string lotno, int qty, DateTime date, int userId);

        [WebGet(UriTemplate = "StockAdjustment?gtin={gtin}&lotno={lotno}&qty={qty}&date={date}&reasonId={reasonId}&userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        IntReturnValue StockAdjustment(string gtin, string lotno, int qty, DateTime date, int reasonId, int userId);
    }
}
