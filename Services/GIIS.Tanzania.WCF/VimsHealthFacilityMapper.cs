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
using System.Data;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.IO;
using System.ServiceModel.Web;
using System.Collections.Specialized;

namespace GIIS.Tanzania.WCF
{
	class HealthFacilityMapper
	{
		private static string facility_mapping = "[{'name':'AAR Arusha City Dispensary','vims_id':'19259','timr_id':'12625'},{'name':'Arusha Health Centre for Women','vims_id':'19241','timr_id':'19434'},{'name':'Bama Beba Dispensary','vims_id':'19244','timr_id':'17454'},{'name':'Baraa RC Dispensary','vims_id':'19593','timr_id':'17319'},{'name':'Bikira Maria mama wa huruma Dispensary','vims_id':'19245','timr_id':'12858'},{'name':'Canosa Dispensary','vims_id':'19246','timr_id':'17469'},{'name':'Cogi Daraja 11 Dispensary','vims_id':'19247','timr_id':'15090'},{'name':'Daraja Mbili Health Centre','vims_id':'19249','timr_id':'12820'},{'name':'Eben Dispensary','vims_id':'19251','timr_id':'17461'},{'name':'Faith Arusha City Dispensary','vims_id':'19252','timr_id':'13428'},{'name':'Green Hope Dispensary','vims_id':'19253','timr_id':'13410'},{'name':'Health Master Dispensary','vims_id':'19254','timr_id':'18762'},{'name':'Hery Dispensary','vims_id':'19255','timr_id':'17459'},{'name':'Huruma Kwa Mrombo Dispensary','vims_id':'19257','timr_id':'13420'},{'name':'Kijenge  RC Dispensary','vims_id':'19258','timr_id':'18379'},{'name':'Lulu Dispensary','vims_id':'19260','timr_id':'17479'},{'name':'Magereza Arusha Dispensary','vims_id':'19266','timr_id':'13411'},{'name':'Mother Medical Care Dispensary','vims_id':'19242','timr_id':'13421'},{'name':'Nadosoito Dispensary','vims_id':'19248','timr_id':'13497'},{'name':'Oljoro Road Dispensary','vims_id':'19252','timr_id':'17457'},{'name':'Oloirien  Dispensary','vims_id':'19257','timr_id':'19824'},{'name':'Oloirien Community Clinic','vims_id':'19255','timr_id':'13438'},{'name':'Samaritan Dispensary','vims_id':'16300','timr_id':'19823'},{'name':'Sanawari Dispensary','vims_id':'19240','timr_id':'13910'},{'name':'Njiro SDA Dispensary','vims_id':'19249','timr_id':'13913'},{'name':'Shree Hindu Health Centre','vims_id':'19580','timr_id':'13405'},{'name':'Sikh Temple Dispensary','vims_id':'19581','timr_id':'13864'},{'name':'Sinon Parish Dispensary','vims_id':'19582','timr_id':'12837'},{'name':'St. Thomas Health Centre','vims_id':'19583','timr_id':'13895'},{'name':'St. Veronica Dispensary','vims_id':'19584','timr_id':'13906'},{'name':'Tanganyika Packers Dispensary','vims_id':'19587','timr_id':'13771'},{'name':'Total Care Dispensary','vims_id':'19245','timr_id':'16818'},{'name':'Tumaini Dispensary','vims_id':'19593','timr_id':'13489'},{'name':'Upendo   Dispensary','vims_id':'19595','timr_id':'17443'},{'name':'Upone Dispensary','vims_id':'19596','timr_id':'17456'},{'name':'Enduimet Health Centre','vims_id':'19140','timr_id':'15073'},{'name':'Ilorienito Dispensary','vims_id':'16489','timr_id':'13108'},{'name':'Irkaswa Dispensary','vims_id':'16553','timr_id':'13053'},{'name':'Ketumbeine Dispensary','vims_id':'16629','timr_id':'18376'},{'name':'Lerangwa Dispensary','vims_id':'16670','timr_id':'13907'},{'name':'Ngereyani Dispensary','vims_id':'19132','timr_id':'17490'},{'name':'Olmolog Dispensary','vims_id':'19770','timr_id':'18378'},{'name':'Olmot Dispensary','vims_id':'19771','timr_id':'13904'},{'name':'Sinya Dispensary','vims_id':'19772','timr_id':'13903'},{'name':'Karangai Dispensary','vims_id':'16394','timr_id':'17470'},{'name':'Kikatiti Lutheran Dispensary','vims_id':'16518','timr_id':'19465'},{'name':'Kitefu Dispensary','vims_id':'16692','timr_id':'17472'},{'name':'Kwaugoro Dispensary','vims_id':'','timr_id':'19822'},{'name':'Leguruki Lutheran Dispensary','vims_id':'16697','timr_id':'17516'},{'name':'Lumeni Cristi Dispensary','vims_id':'16686','timr_id':'12750'},{'name':'Maisha Health Care','vims_id':'16736','timr_id':'19818'},{'name':'Majengo Govt Dispensary','vims_id':'16743','timr_id':'17523'},{'name':'Makiba Rural Health Centre','vims_id':'16758','timr_id':'17522'},{'name':'Mangashinyi Dispensary','vims_id':'19234','timr_id':'12655'},{'name':'Maroroni Dispensary','vims_id':'19051','timr_id':'17513'},{'name':'Meru District Hospital','vims_id':'16302','timr_id':'13867'},{'name':'Momela Olkung`wado Dispensary','vims_id':'19103','timr_id':'17503'},{'name':'New Life Disp','vims_id':'19101','timr_id':'19821'},{'name':'Ngarenanyuki Gov','vims_id':'19102','timr_id':'19819'},{'name':'Ngarenanyuki Lutheran Dispensary','vims_id':'16355','timr_id':'19485'},{'name':'Ngongongare Dispensary','vims_id':'16554','timr_id':'17468'},{'name':'Ngurudoto RC Dispensary','vims_id':'16803','timr_id':'17521'},{'name':'Nkoarisambu Dispensary','vims_id':'19227','timr_id':'13165'},{'name':'TAC Dispensary','vims_id':'16514','timr_id':'19480'},{'name':'Tumaini CRCT Dispensary','vims_id':'16371','timr_id':'18769'},{'name':'Usa RC Health Centre','vims_id':'19598','timr_id':'19262'},{'name':'Valeska Dispensary','vims_id':'19601','timr_id':'17509'},{'name':'Endulen Hospital','vims_id':'16376','timr_id':'12986'},{'name':'Jema  Dispensary','vims_id':'16452','timr_id':'17909'},{'name':'Mgongo Dispensary','vims_id':'16739','timr_id':'15069'},{'name':'NCAA Dispensary','vims_id':'16752','timr_id':'15297'},{'name':'Oloirobi Dispensary','vims_id':'16679','timr_id':'17910'},{'name':'Piyaya Dispensary','vims_id':'19213','timr_id':'18304'},{'name':'Bangata Govt Dispensary','vims_id':'16378','timr_id':'15064'},{'name':'Bangata Lutheran Dispensary','vims_id':'16477','timr_id':'18756'},{'name':'Bwawani  Dispensary','vims_id':'16478','timr_id':'15066'},{'name':'Cogi Ngaramtoni Dispensary','vims_id':'16555','timr_id':'13175'},{'name':'Ekenywa Dispensary','vims_id':'16525','timr_id':'13310'},{'name':'Ilboru Dispensary','vims_id':'16562','timr_id':'17556'},{'name':'Imbimbia Dispensary','vims_id':'16654','timr_id':'13871'},{'name':'Kimnyaki maasai Dispensary','vims_id':'16659','timr_id':'15041'},{'name':'Kimnyaki Dispensary','vims_id':'16640','timr_id':'18745'},{'name':'Kioga Dispensary','vims_id':'16681','timr_id':'18746'},{'name':'Kiranyi Dispensary','vims_id':'16658','timr_id':'15040'},{'name':'Kisongo Luth Dispensary','vims_id':'16738','timr_id':'15044'},{'name':'Kisongo PAG Dispensary','vims_id':'16708','timr_id':'18748'},{'name':'Kisongo Charitable Health Centre','vims_id':'16707','timr_id':'13770'},{'name':'Kisongo SDA Dispensary','vims_id':'16762','timr_id':'15042'},{'name':'Lengijave Dispensary','vims_id':'16320','timr_id':'15031'},{'name':'Lesiraa Dispensary','vims_id':'16379','timr_id':'18782'},{'name':'Likamba Dispensary','vims_id':'16522','timr_id':'15059'},{'name':'Losinoni Dispensary','vims_id':'19087','timr_id':'15039'},{'name':'M & M Kiwera Dispensary','vims_id':'16561','timr_id':'15043'},{'name':'Manyire Dispensary','vims_id':'16419','timr_id':'15048'},{'name':'Mbuyuni  Dispensary','vims_id':'16406','timr_id':'18076'},{'name':'Mirongoine Dispensary','vims_id':'16377','timr_id':'15038'},{'name':'Mlangarini Dispensary','vims_id':'19603','timr_id':'15047'},{'name':'Moivo Dispensary','vims_id':'19606','timr_id':'15032'},{'name':'Mount Meru Hospital','vims_id':'19619','timr_id':'13914'},{'name':'Mungush Dispensary','vims_id':'19604','timr_id':'13194'},{'name':'Musa Dispensary','vims_id':'19623','timr_id':'15091'},{'name':'Mwandeti Dispensarry','vims_id':'19605','timr_id':'15089'},{'name':'Nduruma Health Centre','vims_id':'16360','timr_id':'15065'},{'name':'Ng`iresi Dispensary','vims_id':'16706','timr_id':'18085'},{'name':'Ngorbob Dispensary','vims_id':'19617','timr_id':'15092'},{'name':'Oldadai Dispensary','vims_id':'19612','timr_id':'15088'},{'name':'Oldonyosambu Dispensary','vims_id':'19613','timr_id':'15281'},{'name':'Oldonyosapuku Dispensary','vims_id':'16602','timr_id':'18747'},{'name':'Oldonyowasi Dispensary','vims_id':'19614','timr_id':'15078'},{'name':'Olgilai Dispensary','vims_id':'16516','timr_id':'13455'},{'name':'Oljoro JKT Dispensary','vims_id':'19618','timr_id':'15103'},{'name':'Olkokola Health Centre','vims_id':'19602','timr_id':'18057'},{'name':'Olkokola RC  Dispensary','vims_id':'19616','timr_id':'15062'},{'name':'Olmotonyi Dispensary','vims_id':'19620','timr_id':'18078'},{'name':'Oltrument Hospital','vims_id':'19622','timr_id':'18757'},{'name':'Sambasha Dispensary','vims_id':'19610','timr_id':'15093'},{'name':'Selian CDH Hospital','vims_id':'16683','timr_id':'15094'},{'name':'Siha Dispensary','vims_id':'19624','timr_id':'13315'},{'name':'SOS Dipensary','vims_id':'19625','timr_id':'13375'},{'name':'Themi ya Simba Dispensary','vims_id':'19628','timr_id':'15074'},{'name':'Askofu Hhando  Health Centre','vims_id':'16324','timr_id':'17462'},{'name':'Ayalabe Dispensary','vims_id':'16409','timr_id':'19474'},{'name':'Basodawish Dispensary','vims_id':'16403','timr_id':'15266'},{'name':'Buger Dispensary','vims_id':'16449','timr_id':'17481'},{'name':'Chemchem  Dispensary','vims_id':'16444','timr_id':'17837'},{'name':'Dumbechand Dispensary','vims_id':'16437','timr_id':'17253'},{'name':'Endabash Health Centre','vims_id':'16445','timr_id':'18308'},{'name':'Endallah Dispensary','vims_id':'16464','timr_id':'19478'},{'name':'Endamaghang Dispensary','vims_id':'16482','timr_id':'17497'},{'name':'Endamarariek Health Centre','vims_id':'16491','timr_id':'17442'},{'name':'Endashangwet Dispensary','vims_id':'16496','timr_id':'17452'},{'name':'Fame Health Centre','vims_id':'16549','timr_id':'17451'},{'name':'Getamock Dispensary','vims_id':'16570','timr_id':'17496'},{'name':'Gidamilanda Dispensary','vims_id':'16598','timr_id':'13276'},{'name':'Gongali Dispensary','vims_id':'16613','timr_id':'19477'},{'name':'Jobaj Dispensary','vims_id':'16773','timr_id':'17450'},{'name':'Kambi  Simba Health Center','vims_id':'16751','timr_id':'17478'},{'name':'Kansay Health Centre','vims_id':'14106','timr_id':'17492'},{'name':'Karatu Health Centre','vims_id':'16597','timr_id':'17445'},{'name':'Karatu Lutheran DD Hospital','vims_id':'19081','timr_id':'17491'},{'name':'Karatu RC Dispensary','vims_id':'19082','timr_id':'17449'},{'name':'Khusumay Dispensary','vims_id':'16334','timr_id':'15085'},{'name':'Kilimamoja Dispensary','vims_id':'19078','timr_id':'17877'},{'name':'Laghangarer Dispensary','vims_id':'16446','timr_id':'19555'},{'name':'Lositete Dispensary','vims_id':'16623','timr_id':'18766'},{'name':'Makhoromba Dispensary','vims_id':'16354','timr_id':'17845'},{'name':'Malekchand Dispensary','vims_id':'16375','timr_id':'17876'},{'name':'Mang`ola Barazani Dispensary','vims_id':'19198','timr_id':'19197'},{'name':'Mang`ola Juu Dispensary','vims_id':'19198','timr_id':'15082'},{'name':'Mang`ola Prison Dispensary','vims_id':'16767','timr_id':'15077'},{'name':'Matala Dispensary','vims_id':'19199','timr_id':'15083'},{'name':'Mbuganyekundu Health Centre','vims_id':'19775','timr_id':'17875'},{'name':'Ngaibara Dispensay','vims_id':'19777','timr_id':'13309'},{'name':'Oldean Health Centre','vims_id':'19778','timr_id':'13751'},{'name':'Qangded Dispensary','vims_id':'19779','timr_id':'17494'},{'name':'Qaru Dispensary','vims_id':'19781','timr_id':'15081'},{'name':'Qurus Dispensary','vims_id':'19782','timr_id':'19476'},{'name':'Rhotia Health Centre','vims_id':'19783','timr_id':'15076'},{'name':'Slahhamo Dispensary','vims_id':'19788','timr_id':'17495'},{'name':'Tloma Dispensary','vims_id':'19789','timr_id':'15080'}]";

		public static int GetVimsHealthFacilityFacilityId(int TimrId)
		{
			try
			{
				JArray facilityMapping = JArray.Parse(facility_mapping);
				int count = facilityMapping.Count;
				for (int i = 0; i < count; i++)
				{
					if (TimrId == ((int)facilityMapping[i]["timr_id"]))
					{
						return ((int)facilityMapping[i]["vims_id"]);
					}
				}
				Exception e = new Exception("Mapping for  timr_id = " + TimrId + " does not exist");
				throw e;
			}
			catch
			{
				Exception e = new Exception("Mapping for  timr_id = " + TimrId + " does not exist");
				throw e;
			}
		}


		public static int GetTimrHealthFacilityFacilityId(int VimsId)
		{
			try
			{
				JArray facilityMapping = JArray.Parse(facility_mapping);
				int count = facilityMapping.Count;
				for (int i = 0; i < count; i++)
				{
					if (VimsId == ((int)facilityMapping[i]["vims_id"]))
					{
						return ((int)facilityMapping[i]["timr_id"]);
					}
				}
				Exception e = new Exception("Mapping for  vims_id = " + VimsId + " does not exist");
				throw e;
			}
			catch
			{
				Exception e = new Exception("Mapping for  vims_id = " + VimsId + " does not exist");
				throw e;
			}
		}

	}
}