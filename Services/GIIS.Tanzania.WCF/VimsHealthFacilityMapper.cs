﻿//*******************************************************************************
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
		//private static string facility_mapping = "[{'name':'Usariver GVT','vims_id':'16394','timr_id':'17515'},{'name':'Kaloleni HC','vims_id':'16333','timr_id':'19472'},{'name':'Kikatiti','vims_id':'19221','timr_id':'19465'},{'name':' Sombetini','vims_id':'16753','timr_id':'13502'},{'name':'St. Elizabeth','vims_id':'16810','timr_id':'13909'},{'name':'Aicc','vims_id':'16815','timr_id':'15037'},{'name':'Arusha Lutheran Medical Hospital ','vims_id':'16802','timr_id':'17480'},{'name':'Ithna Asheri ','vims_id':'16805','timr_id':'17460'},{'name':'Kaloleni Health Centre','vims_id':'16333','timr_id':'19472'},{'name':'Levolosi','vims_id':'16345','timr_id':'15101'},{'name':'Mkonoo ','vims_id':'16362','timr_id':'17493'},{'name':'Moivaro','vims_id':'16363','timr_id':'18768'},{'name':'Mt. Meru Hospital ','vims_id':'16300','timr_id':'13914'},{'name':'Ngarenaro UHC ','vims_id':'16372','timr_id':'19473'},{'name':' Old Arusha Healh Centre','vims_id':'16812','timr_id':'17508'},{'name':'Olkereyan ','vims_id':'16711','timr_id':'15097'},{'name':'Police Arusha MC Dispensary ','vims_id':'16715','timr_id':'13501'},{'name':'Terrat ','vims_id':'16762','timr_id':'13911'},{'name':'Themi Health Centre ','vims_id':'16389','timr_id':'17464'},{'name':'Elangatadapashi ','vims_id':'16439','timr_id':'18764'},{'name':'Engarenaibor ','vims_id':'16328','timr_id':'15070'},{'name':' Engikaret','vims_id':'16451 ','timr_id':'15071'},{'name':'Gelai Bomba ','vims_id':'16456','timr_id':'17489'},{'name':'Gelai Lumbwa ','vims_id':'16457','timr_id':'17488'},{'name':' Kamwanga','vims_id':'16489','timr_id':'13898'},{'name':'Kiseriani ','vims_id':'16533','timr_id':'15046'},{'name':'Longido Health Centre ','vims_id':'16347','timr_id':'18377'},{'name':'Longido District Hospital ','vims_id':'14133','timr_id':'12613'},{'name':'Matale','vims_id':'16614','timr_id':'15072'},{'name':'Mundarara','vims_id':'16653','timr_id':'13900'},{'name':'Namanga','vims_id':'16670','timr_id':'13901'},{'name':'Noondoto','vims_id':'16695','timr_id':'13902'},{'name':'Tingatinga','vims_id':'16765','timr_id':'17437'},{'name':'Arash','vims_id':'16399','timr_id':'19471'},{'name':'Digodigo','vims_id':'16431','timr_id':'17482'},{'name':'Esere','vims_id':'16452','timr_id':'15298'},{'name':'Irkeekpusi','vims_id':'16479','timr_id':'15296'},{'name':'Kakesio','vims_id':'16486','timr_id':'18306'},{'name':'Loliondo Health Centre','vims_id':'16346','timr_id':'12995'},{'name':'Magaiduru','vims_id':'16579','timr_id':'13057'},{'name':'Malambo HC','vims_id':'16352','timr_id':'17484'},{'name':'Nainokanoka','vims_id':'16368','timr_id':'17485'},{'name':'Ngarasero','vims_id':'16679','timr_id':'17903'},{'name':'Olbalbal','vims_id':'16704','timr_id':'13074'},{'name':'Oldonyosambu','vims_id':'16707','timr_id':'13210'},{'name':'Oloipiri','vims_id':'16712','timr_id':'15100'},{'name':'Sakala','vims_id':'16735','timr_id':'17483'},{'name':'Sale','vims_id':'16737','timr_id':'15036'},{'name':'Samunge','vims_id':'16739','timr_id':'17487'},{'name':'Sero','vims_id':'16745','timr_id':'17512'},{'name':'Soitsambu','vims_id':'16752','timr_id':'13052'},{'name':'Wasso','vims_id':'16321','timr_id':'19486'},{'name':'Kilinga ','vims_id':'16514','timr_id':'18313'},{'name':'Kimundo','vims_id':'16518','timr_id':'17471'},{'name':'Kingori','vims_id':'16521','timr_id':'19482 '},{'name':'Kipandenkoavele','vims_id':'16524','timr_id':'17465'},{'name':'Leguruki','vims_id':'16554','timr_id':'19484'},{'name':'Maji Ya Chai','vims_id':'16585','timr_id':'17476'},{'name':'Malula','vims_id':'16591','timr_id':'17466'},{'name':'Mareu','vims_id':'16355','timr_id':'13661'},{'name':'Mbuguni','vims_id':'16359','timr_id':'17531'},{'name':'Nambala Dispensary','vims_id':'17130','timr_id':'18312'},{'name':'Ngerenanyuki Health Centre','vims_id':'16371','timr_id':'19754'},{'name':'Ngyani Dispensary','vims_id':'16686','timr_id':'17463'},{'name':'Ngyeku Dispensary ','vims_id':'16688','timr_id':'15099'},{'name':'Nkoanekoli Dispensary','vims_id':'16691','timr_id':'19755'},{'name':'Nkoanrua','vims_id':'16692','timr_id':'19481'},{'name':'Nkoaranga','vims_id':'16803','timr_id':'13717'},{'name':' Nsengony','vims_id':'16697','timr_id':'17121'},{'name':'Poli','vims_id':'16382','timr_id':'17342'},{'name':'Sakila','vims_id':'16736','timr_id':'17514'},{'name':'Sura','vims_id':'16758','timr_id':'17474'},{'name':'AAR Arusha City Dispensary','vims_id':'19259','timr_id':'12625'},{'name':'Arusha Health Centre for Women','vims_id':'19241','timr_id':'19434'},{'name':'Bama Beba Dispensary','vims_id':'19244','timr_id':'17454'},{'name':'Baraa RC Dispensary','vims_id':'19593','timr_id':'17319'},{'name':'Bikira Maria mama wa huruma Dispensary','vims_id':'19245','timr_id':'12858'},{'name':'Canosa Dispensary','vims_id':'19246','timr_id':'17469'},{'name':'Cogi Daraja 11 Dispensary','vims_id':'19247','timr_id':'15090'},{'name':'Daraja Mbili Health Centre','vims_id':'19249','timr_id':'12820'},{'name':'Eben Dispensary','vims_id':'19251','timr_id':'17461'},{'name':'Faith Arusha City Dispensary','vims_id':'19252','timr_id':'13428'},{'name':'Green Hope Dispensary','vims_id':'19253','timr_id':'13410'},{'name':'Health Master Dispensary','vims_id':'19254','timr_id':'18762'},{'name':'Hery Dispensary','vims_id':'19255','timr_id':'17459'},{'name':'Huruma Kwa Mrombo Dispensary','vims_id':'19257','timr_id':'13420'},{'name':'Kijenge  RC Dispensary','vims_id':'19258','timr_id':'18379'},{'name':'Lulu Dispensary','vims_id':'19260','timr_id':'17479'},{'name':'Magereza Arusha Dispensary','vims_id':'19266','timr_id':'13411'},{'name':'Mother Medical Care Dispensary','vims_id':'19242','timr_id':'13421'},{'name':'Nadosoito Dispensary','vims_id':'19248','timr_id':'13497'},{'name':'Oljoro Road Dispensary','vims_id':'19252','timr_id':'17457'},{'name':'Oloirien  Dispensary','vims_id':'19257','timr_id':'19824'},{'name':'Oloirien Community Clinic','vims_id':'19255','timr_id':'13438'},{'name':'Samaritan Dispensary','vims_id':'16300','timr_id':'19823'},{'name':'Sanawari Dispensary','vims_id':'19240','timr_id':'13910'},{'name':'Njiro SDA Dispensary','vims_id':'19249','timr_id':'13913'},{'name':'Shree Hindu Health Centre','vims_id':'19580','timr_id':'13405'},{'name':'Sikh Temple Dispensary','vims_id':'19581','timr_id':'13864'},{'name':'Sinon Parish Dispensary','vims_id':'19582','timr_id':'12837'},{'name':'St. Thomas Health Centre','vims_id':'19583','timr_id':'13895'},{'name':'St. Veronica Dispensary','vims_id':'19584','timr_id':'13906'},{'name':'Tanganyika Packers Dispensary','vims_id':'19587','timr_id':'13771'},{'name':'Total Care Dispensary','vims_id':'19245','timr_id':'16818'},{'name':'Tumaini Dispensary','vims_id':'19593','timr_id':'13489'},{'name':'Upendo   Dispensary','vims_id':'19595','timr_id':'17443'},{'name':'Upone Dispensary','vims_id':'19596','timr_id':'17456'},{'name':'Enduimet Health Centre','vims_id':'19140','timr_id':'15073'},{'name':'Ilorienito Dispensary','vims_id':'16489','timr_id':'13108'},{'name':'Irkaswa Dispensary','vims_id':'16553','timr_id':'13053'},{'name':'Ketumbeine Dispensary','vims_id':'16629','timr_id':'18376'},{'name':'Lerangwa Dispensary','vims_id':'16670','timr_id':'13907'},{'name':'Ngereyani Dispensary','vims_id':'19132','timr_id':'17490'},{'name':'Olmolog Dispensary','vims_id':'19770','timr_id':'18378'},{'name':'Olmot Dispensary','vims_id':'19771','timr_id':'13904'},{'name':'Sinya Dispensary','vims_id':'19772','timr_id':'13903'},{'name':'Karangai Dispensary','vims_id':'16394','timr_id':'17470'},{'name':'Kikatiti Lutheran Dispensary','vims_id':'16518','timr_id':'19465'},{'name':'Kitefu Dispensary','vims_id':'16692','timr_id':'17472'},{'name':'Kwaugoro Dispensary','vims_id':\"\",'timr_id':'19822'},{'name':'Leguruki Lutheran Dispensary','vims_id':'16697','timr_id':'17516'},{'name':'Lumeni Cristi Dispensary','vims_id':'16686','timr_id':'12750'},{'name':'Maisha Health Care','vims_id':'16736','timr_id':'19818'},{'name':'Majengo Govt Dispensary','vims_id':'16743','timr_id':'17523'},{'name':'Makiba Rural Health Centre','vims_id':'16758','timr_id':'17522'},{'name':'Mangashinyi Dispensary','vims_id':'19234','timr_id':'12655'},{'name':'Maroroni Dispensary','vims_id':'19051','timr_id':'17513'},{'name':'Meru District Hospital','vims_id':'16302','timr_id':'13867'},{'name':'Momela Olkung`wado Dispensary','vims_id':'19103','timr_id':'17503'},{'name':'New Life Disp','vims_id':'19101','timr_id':'19821'},{'name':'Ngarenanyuki Gov','vims_id':'19102','timr_id':'19819'},{'name':'Ngarenanyuki Lutheran Dispensary','vims_id':'16355','timr_id':'19485'},{'name':'Ngongongare Dispensary','vims_id':'16554','timr_id':'17468'},{'name':'Ngurudoto RC Dispensary','vims_id':'16803','timr_id':'17521'},{'name':'Nkoarisambu Dispensary','vims_id':'19227','timr_id':'13165'},{'name':'TAC Dispensary','vims_id':'16514','timr_id':'19480'},{'name':'Tumaini CRCT Dispensary','vims_id':'16371','timr_id':'18769'},{'name':'Usa RC Health Centre','vims_id':'19598','timr_id':'19262'},{'name':'Valeska Dispensary','vims_id':'19601','timr_id':'17509'},{'name':'Endulen Hospital','vims_id':'16376','timr_id':'12986'},{'name':'Jema  Dispensary','vims_id':'16452','timr_id':'17909'},{'name':'Mgongo Dispensary','vims_id':'16739','timr_id':'15069'},{'name':'NCAA Dispensary','vims_id':'16752','timr_id':'15297'},{'name':'Oloirobi Dispensary','vims_id':'16679','timr_id':'17910'},{'name':'Piyaya Dispensary','vims_id':'19213','timr_id':'18304'},{'name':'Bangata Govt Dispensary','vims_id':'16378','timr_id':'15064'},{'name':'Bangata Lutheran Dispensary','vims_id':'16477','timr_id':'18756'},{'name':'Bwawani  Dispensary','vims_id':'16478','timr_id':'15066'},{'name':'Cogi Ngaramtoni Dispensary','vims_id':'16555','timr_id':'13175'},{'name':'Ekenywa Dispensary','vims_id':'16525','timr_id':'13310'},{'name':'Ilboru Dispensary','vims_id':'16562','timr_id':'17556'},{'name':'Imbimbia Dispensary','vims_id':'16654','timr_id':'13871'},{'name':'Kimnyaki maasai Dispensary','vims_id':'16659','timr_id':'15041'},{'name':'Kimnyaki Dispensary','vims_id':'16640','timr_id':'18745'},{'name':'Kioga Dispensary','vims_id':'16681','timr_id':'18746'},{'name':'Kiranyi Dispensary','vims_id':'16658','timr_id':'15040'},{'name':'Kisongo Luth Dispensary','vims_id':'16738','timr_id':'15044'},{'name':'Kisongo PAG Dispensary','vims_id':'16708','timr_id':'18748'},{'name':'Kisongo Charitable Health Centre','vims_id':'16707','timr_id':'13770'},{'name':'Kisongo SDA Dispensary','vims_id':'16762','timr_id':'15042'},{'name':'Lengijave Dispensary','vims_id':'16320','timr_id':'15031'},{'name':'Lesiraa Dispensary','vims_id':'16379','timr_id':'18782'},{'name':'Likamba Dispensary','vims_id':'16522','timr_id':'15059'},{'name':'Losinoni Dispensary','vims_id':'19087','timr_id':'15039'},{'name':'M & M Kiwera Dispensary','vims_id':'16561','timr_id':'15043'},{'name':'Manyire Dispensary','vims_id':'16419','timr_id':'15048'},{'name':'Mbuyuni  Dispensary','vims_id':'16406','timr_id':'18076'},{'name':'Mirongoine Dispensary','vims_id':'16377','timr_id':'15038'},{'name':'Mlangarini Dispensary','vims_id':'19603','timr_id':'15047'},{'name':'Moivo Dispensary','vims_id':'19606','timr_id':'15032'},{'name':'Mount Meru Hospital','vims_id':'19619','timr_id':'13914'},{'name':'Mungush Dispensary','vims_id':'19604','timr_id':'13194'},{'name':'Musa Dispensary','vims_id':'19623','timr_id':'15091'},{'name':'Mwandeti Dispensarry','vims_id':'19605','timr_id':'15089'},{'name':'Nduruma Health Centre','vims_id':'16360','timr_id':'15065'},{'name':'Ng`iresi Dispensary','vims_id':'16706','timr_id':'18085'},{'name':'Ngorbob Dispensary','vims_id':'19617','timr_id':'15092'},{'name':'Oldadai Dispensary','vims_id':'19612','timr_id':'15088'},{'name':'Oldonyosambu Dispensary','vims_id':'19613','timr_id':'15281'},{'name':'Oldonyosapuku Dispensary','vims_id':'16602','timr_id':'18747'},{'name':'Oldonyowasi Dispensary','vims_id':'19614','timr_id':'15078'},{'name':'Olgilai Dispensary','vims_id':'16516','timr_id':'13455'},{'name':'Oljoro JKT Dispensary','vims_id':'19618','timr_id':'15103'},{'name':'Olkokola Health Centre','vims_id':'19602','timr_id':'18057'},{'name':'Olkokola RC  Dispensary','vims_id':'19616','timr_id':'15062'},{'name':'Olmotonyi Dispensary','vims_id':'19620','timr_id':'18078'},{'name':'Oltrument Hospital','vims_id':'19622','timr_id':'18757'},{'name':'Sambasha Dispensary','vims_id':'19610','timr_id':'15093'},{'name':'Selian CDH Hospital','vims_id':'16683','timr_id':'15094'},{'name':'Siha Dispensary','vims_id':'19624','timr_id':'13315'},{'name':'SOS Dipensary','vims_id':'19625','timr_id':'13375'},{'name':'Themi ya Simba Dispensary','vims_id':'19628','timr_id':'15074'},{'name':'Askofu Hhando  Health Centre','vims_id':'16324','timr_id':'17462'},{'name':'Ayalabe Dispensary','vims_id':'16409','timr_id':'19474'},{'name':'Basodawish Dispensary','vims_id':'16403','timr_id':'15266'},{'name':'Buger Dispensary','vims_id':'16449','timr_id':'17481'},{'name':'Chemchem  Dispensary','vims_id':'16444','timr_id':'17837'},{'name':'Dumbechand Dispensary','vims_id':'16437','timr_id':'17253'},{'name':'Endallah Dispensary','vims_id':'16464','timr_id':'19478'},{'name':'Endamaghang Dispensary','vims_id':'16482','timr_id':'17497'},{'name':'Endamarariek Health Centre','vims_id':'16491','timr_id':'17442'},{'name':'Endashangwet Dispensary','vims_id':'16496','timr_id':'17452'},{'name':'Fame Health Centre','vims_id':'16549','timr_id':'17451'},{'name':'Getamock Dispensary','vims_id':'16570','timr_id':'17496'},{'name':'Gidamilanda Dispensary','vims_id':'16598','timr_id':'13276'},{'name':'Gongali Dispensary','vims_id':'16613','timr_id':'19477'},{'name':'Jobaj Dispensary','vims_id':'16773','timr_id':'17450'},{'name':'Kambi  Simba Health Center','vims_id':'16751','timr_id':'17478'},{'name':'Kansay Health Centre','vims_id':'14106','timr_id':'17492'},{'name':'Karatu Health Centre','vims_id':'16597','timr_id':'17445'},{'name':'Karatu Lutheran DD Hospital','vims_id':'19081','timr_id':'17491'},{'name':'Karatu RC Dispensary','vims_id':'19082','timr_id':'17449'},{'name':'Khusumay Dispensary','vims_id':'16334','timr_id':'15085'},{'name':'Kilimamoja Dispensary','vims_id':'19078','timr_id':'17877'},{'name':'Laghangarer Dispensary','vims_id':'16446','timr_id':'19555'},{'name':'Lositete Dispensary','vims_id':'16623','timr_id':'18766'},{'name':'Makhoromba Dispensary','vims_id':'16354','timr_id':'17845'},{'name':'Malekchand Dispensary','vims_id':'16375','timr_id':'17876'},{'name':'Mang`ola Barazani Dispensary','vims_id':'19198','timr_id':'19197'},{'name':'Mang`ola Juu Dispensary','vims_id':'19198','timr_id':'15082'},{'name':'Mang`ola Prison Dispensary','vims_id':'16767','timr_id':'15077'},{'name':'Matala Dispensary','vims_id':'19199','timr_id':'15083'},{'name':'Mbuganyekundu Health Centre','vims_id':'19775','timr_id':'17875'},{'name':'Ngaibara Dispensay','vims_id':'19777','timr_id':'13309'},{'name':'Oldean Health Centre','vims_id':'19778','timr_id':'13751'},{'name':'Qangded Dispensary','vims_id':'19779','timr_id':'17494'},{'name':'Qaru Dispensary','vims_id':'19781','timr_id':'15081'},{'name':'Qurus Dispensary','vims_id':'19782','timr_id':'19476'},{'name':'Rhotia Health Centre','vims_id':'19783','timr_id':'15076'},{'name':'Slahhamo Dispensary','vims_id':'19788','timr_id':'17495'},{'name':'Tloma Dispensary','vims_id':'19789','timr_id':'15080'},{'name':'Monduli Juu','vims_id':'16642','timr_id':'17504'},{'name':'Makuyuni','vims_id':'16588','timr_id':'17530'},{'name':'Engaruka','vims_id':'16450','timr_id':'17499'},{'name':'Endabash Health Centre','vims_id':'19776','timr_id':'18308'},{'name':'Oldean Health Centre','vims_id':'19778','timr_id':'13751'},{'name':'Dumbechand Dispensary','vims_id':'16437','timr_id':'17253'},{'name':'Karatu Health Centre','vims_id':'16597','timr_id':'17445'},{'name':'Karatu RC Dispensary','vims_id':'19082','timr_id':'17449'},{'name':'Monduli Hospital','vims_id':'16308','timr_id':'19520'}]";	

		private static string facility_mapping = "[{'timr_id':12625,'Name':'AAR Arusha City Dispensary','vims_id':19240},{'timr_id':15037,'Name':'AICC Hospital','vims_id':16815},{'timr_id':17480,'Name':'Arusha Lutheran Medical Hospital','vims_id':19242},{'timr_id':19471,'Name':'Arash Dispensary','vims_id':16399},{'timr_id':17498,'Name':'Arkatan Dispensary','vims_id':16400},{'timr_id':12610,'Name':'Arusha City Council','vims_id':19085},{'timr_id':12614,'Name':'Arusha District Council','vims_id':19087},{'timr_id':18287,'Name':'Arusha Medical  Health Centre','vims_id':19241},{'timr_id':17462,'Name':'Askofu Hhando  Health Centre','vims_id':16324},{'timr_id':19474,'Name':'Ayalabe Dispensary','vims_id':19777},{'timr_id':17319,'Name':'Baraa RC Dispensary','vims_id':19593},{'timr_id':17454,'Name':'Bama Beba Dispensary','vims_id':19244},{'timr_id':15064,'Name':'Bangata Govt Dispensary','vims_id':19602},{'timr_id':18756,'Name':'Bangata Lutheran Dispensary','vims_id':16406},{'timr_id':15266,'Name':'Basodawish Dispensary','vims_id':16409},{'timr_id':12858,'Name':'Bikira Maria mama wa huruma Dispensary','vims_id':19245},{'timr_id':17481,'Name':'Buger Dispensary','vims_id':19780},{'timr_id':15066,'Name':'Bwawani  Dispensary','vims_id':16419},{'timr_id':17469,'Name':'Canosa Dispensary','vims_id':19246},{'timr_id':17837,'Name':'Chemchem  Dispensary','vims_id':16424},{'timr_id':15090,'Name':'Cogi Daraja 11 Dispensary','vims_id':19247},{'timr_id':19448,'Name':'Cogi Esso Dispensary','vims_id':19248},{'timr_id':13175,'Name':'Cogi Ngaramtoni Dispensary','vims_id':19603},{'timr_id':12820,'Name':'Daraja Mbili Health Centre','vims_id':19249},{'timr_id':17482,'Name':'Digodigo Health Centre','vims_id':16431},{'timr_id':17253,'Name':'Dumbechand Dispensary','vims_id':16437},{'timr_id':17461,'Name':'Eben Dispensary','vims_id':19251},{'timr_id':13310,'Name':'Ekenywa Dispensary','vims_id':19604},{'timr_id':18764,'Name':'Elang`atadapash Dispensary','vims_id':16439},{'timr_id':19835,'Name':'Eluwai Dispensary','vims_id':19630},{'timr_id':18308,'Name':'Endabash Health Centre','vims_id':19776},{'timr_id':19478,'Name':'Endallah Dispensary','vims_id':16444},{'timr_id':17497,'Name':'Endamaghang Dispensary','vims_id':16445},{'timr_id':17442,'Name':'Endamarariek Health Centre','vims_id':16446},{'timr_id':17452,'Name':'Endashangwet Dispensary','vims_id':16449},{'timr_id':19825,'Name':'Endonyomali Dispensary','vims_id':19772},{'timr_id':15073,'Name':'Enduimet Health Centre','vims_id':19774},{'timr_id':12986,'Name':'Endulen Hospital','vims_id':19124},{'timr_id':15070,'Name':'Engarenaibor Health Centre','vims_id':16328},{'timr_id':17499,'Name':'Engaruka Dispensary','vims_id':16450},{'timr_id':15071,'Name':'Engikaret Dispensary','vims_id':16451},{'timr_id':15298,'Name':'Esere Dispensary','vims_id':16452},{'timr_id':13387,'Name':'Esilalei Dispensary','vims_id':16798},{'timr_id':13428,'Name':'Faith Arusha City Dispensary','vims_id':19252},{'timr_id':17451,'Name':'Fame Health Centre','vims_id':19197},{'timr_id':17489,'Name':'Gelai Bomba Dispensary','vims_id':16456},{'timr_id':17488,'Name':'Gelai Lumbwa Dispensary','vims_id':16457},{'timr_id':17496,'Name':'Getamock Dispensary','vims_id':16459},{'timr_id':13762,'Name':'Gidamilanda Dispensary','vims_id':16463},{'timr_id':19450,'Name':'Geykrum lambo Dispensary','vims_id':16467},{'timr_id':19477,'Name':'Gongali Dispensary','vims_id':16464},{'timr_id':13410,'Name':'Green Hope Dispensary','vims_id':19253},{'timr_id':18762,'Name':'Health Master Dispensary','vims_id':19254},{'timr_id':17459,'Name':'Heri Dispensary','vims_id':19255},{'timr_id':15098,'Name':'Huruma Kanora Dispensary','vims_id':19256},{'timr_id':13420,'Name':'Huruma Kwa Mrombo Dispensary','vims_id':19257},{'timr_id':17556,'Name':'Ilboru Dispensary','vims_id':19606},{'timr_id':13108,'Name':'IIlorienito Dispensary','vims_id':19769},{'timr_id':13871,'Name':'Imbimbia Dispensary','vims_id':16478},{'timr_id':13053,'Name':'Irkaswa Dispensary','vims_id':19135},{'timr_id':15296,'Name':'Irkepusi Dispensary','vims_id':16479},{'timr_id':17460,'Name':'Ithnaasher Dispensary','vims_id':16805},{'timr_id':17909,'Name':'Jema  Dispensary','vims_id':19212},{'timr_id':17450,'Name':'Jobaj Dispensary','vims_id':19781},{'timr_id':18306,'Name':'Kakesio Dispensary','vims_id':16486},{'timr_id':19472,'Name':'Kaloleni Health Centre','vims_id':16333},{'timr_id':17478,'Name':'Kambi  Simba Health Center','vims_id':16488},{'timr_id':13898,'Name':'Kamwanga Dispensary','vims_id':16489},{'timr_id':17492,'Name':'Kansay Health Centre','vims_id':16491},{'timr_id':17470,'Name':'Karangai Dispensary','vims_id':19101},{'timr_id':17445,'Name':'Karatu Health Centre','vims_id':19775},{'timr_id':17491,'Name':'Karatu Lutheran DD Hospital','vims_id':16317},{'timr_id':17449,'Name':'Karatu RC Dispensary','vims_id':19786},{'timr_id':18376,'Name':'Ketumbeine Dispensary','vims_id':19136},{'timr_id':15085,'Name':'Khusumay Dispensary','vims_id':16500},{'timr_id':18379,'Name':'Kijenge  RC Dispensary','vims_id':19258},{'timr_id':19465,'Name':'Kikatiti Lutheran Dispensary','vims_id':19221},{'timr_id':17877,'Name':'Kilimamoja Dispensary','vims_id':19081},{'timr_id':18313,'Name':'Kilinga Dispensary','vims_id':16514},{'timr_id':18745,'Name':'Kimnyaki Dispensary','vims_id':16516},{'timr_id':15041,'Name':'Kimnyaki maasai Dispensary','vims_id':19618},{'timr_id':19826,'Name':'Kimokouwa Dispensary','vims_id':19771},{'timr_id':17471,'Name':'Kimundo Dispensary','vims_id':16518},{'timr_id':19482,'Name':'King`ori Dispensary','vims_id':16521},{'timr_id':18746,'Name':'Kioga Dispensary','vims_id':16522},{'timr_id':17465,'Name':'Kipande Nkoavele Dispensary','vims_id':16524},{'timr_id':15040,'Name':'Kiranyi Dispensary','vims_id':16525},{'timr_id':17895,'Name':'Kirurumo Health Centre','vims_id':19201},{'timr_id':15046,'Name':'Kiserian  Dispensary','vims_id':16534},{'timr_id':13899,'Name':'Kiserian Dispensary','vims_id':16533},{'timr_id':17467,'Name':'Kisimiri chini Dispensary','vims_id':16535},{'timr_id':13770,'Name':'Kisongo Charitable Health Centre','vims_id':19616},{'timr_id':15044,'Name':'Kisongo Luth Dispensary','vims_id':19620},{'timr_id':18748,'Name':'Kisongo PAG Dispensary','vims_id':19619},{'timr_id':15042,'Name':'Kisongo SDA Dispensary','vims_id':19622},{'timr_id':17472,'Name':'Kitefu Dispensary','vims_id':19599},{'timr_id':19822,'Name':'Kwaugoro Dispensary','vims_id':19598},{'timr_id':19555,'Name':'Laghangarer Dispensary','vims_id':19198},{'timr_id':15079,'Name':'Laja Dispensary','vims_id':16549},{'timr_id':19484,'Name':'Leguruki GVT Dispensary','vims_id':16554},{'timr_id':17516,'Name':'Leguruki Lutheran Dispensary','vims_id':19597},{'timr_id':15102,'Name':'Lemguru Dispensary','vims_id':16555},{'timr_id':17500,'Name':'Lendikinya Dispensary','vims_id':16557},{'timr_id':15031,'Name':'Lengijave Dispensary','vims_id':16559},{'timr_id':17502,'Name':'Lepurko Dispensary','vims_id':16560},{'timr_id':13907,'Name':'Lerangwa Dispensary','vims_id':19137},{'timr_id':18782,'Name':'Lesiraa Dispensary','vims_id':16561},{'timr_id':15101,'Name':'Levolosi Health Centre','vims_id':16345},{'timr_id':15059,'Name':'Likamba Dispensary','vims_id':16562},{'timr_id':12995,'Name':'Loliondo Health Centre','vims_id':16346},{'timr_id':16858,'Name':'Lolkisale Dispensary','vims_id':19202},{'timr_id':12613,'Name':'Longido District Council','vims_id':19079},{'timr_id':18377,'Name':'Longido Health Centre','vims_id':16347},{'timr_id':13665,'Name':'Losimingori Dispensary','vims_id':19203},{'timr_id':15039,'Name':'Losinoni Dispensary','vims_id':19610},{'timr_id':18766,'Name':'Lositete Dispensary','vims_id':16570},{'timr_id':17479,'Name':'Lulu Dispensary','vims_id':19260},{'timr_id':12750,'Name':'Lumeni Cristi Dispensary','vims_id':19222},{'timr_id':15043,'Name':'M & M Kiwera Dispensary','vims_id':19612},{'timr_id':13057,'Name':'Magaiduru Dispensary','vims_id':16579},{'timr_id':13411,'Name':'Magereza Arusha Dispensary','vims_id':16788},{'timr_id':13264,'Name':'Maisha Dispensary','vims_id':19223},{'timr_id':17523,'Name':'Majengo Govt Dispensary','vims_id':19224},{'timr_id':17476,'Name':'Maji ya Chai Dispensary','vims_id':16585},{'timr_id':17845,'Name':'Makhoromba Dispensary','vims_id':19199},{'timr_id':17522,'Name':'Makiba Rural Health Centre','vims_id':19225},{'timr_id':17530,'Name':'Makuyuni   Dispensary','vims_id':16588},{'timr_id':17484,'Name':'Malambo Health Centre','vims_id':16352},{'timr_id':17876,'Name':'Malekchand Dispensary','vims_id':16590},{'timr_id':17466,'Name':'Malula Dispensary','vims_id':16591},{'timr_id':18821,'Name':'Mang`ola Barazani Dispensary','vims_id':19226},{'timr_id':15082,'Name':'Mang`ola Juu Dispensary','vims_id':16354},{'timr_id':12655,'Name':'Mangashinyi Dispensary','vims_id':16598},{'timr_id':17507,'Name':'Manyara Dispensary','vims_id':16601},{'timr_id':15048,'Name':'Manyire Dispensary','vims_id':16602},{'timr_id':13661,'Name':'Mareu Rural Health Centre','vims_id':16355},{'timr_id':17857,'Name':'Marie Stopes  Dispensary','vims_id':19261},{'timr_id':17513,'Name':'Maroroni Dispensary','vims_id':19102},{'timr_id':15083,'Name':'Matala Dispensary','vims_id':16613},{'timr_id':15072,'Name':'Matale Dispensary','vims_id':16614},{'timr_id':18765,'Name':'Matevesi Dispensary','vims_id':19263},{'timr_id':17875,'Name':'Mbuganyekundu Health Centre','vims_id':19779},{'timr_id':17531,'Name':'Mbuguni Rural  Health Centre','vims_id':16359},{'timr_id':18074,'Name':'Mbuyuni   Health Centre','vims_id':16360},{'timr_id':18076,'Name':'Mbuyuni  Dispensary','vims_id':16624},{'timr_id':12609,'Name':'Meru District Council','vims_id':14152},{'timr_id':13867,'Name':'Meru District Hospital','vims_id':19227},{'timr_id':15018,'Name':'Meserani juu Dispensary','vims_id':19629},{'timr_id':17505,'Name':'Mfereji Dispensary','vims_id':16631},{'timr_id':15069,'Name':'Mgongo Dispensary','vims_id':19215},{'timr_id':15038,'Name':'Mirongoine Dispensary','vims_id':16636},{'timr_id':17493,'Name':'Mkonoo Health Centre','vims_id':16362},{'timr_id':15047,'Name':'Mlangarini Dispensary','vims_id':19617},{'timr_id':16845,'Name':'Moita Bwawani Dispensary','vims_id':19204},{'timr_id':18309,'Name':'Moita Kiloriti Dispensary','vims_id':16639},{'timr_id':18768,'Name':'Moivaro Health Centre','vims_id':16363},{'timr_id':15032,'Name':'Moivo Dispensary','vims_id':16640},{'timr_id':17503,'Name':'Momela Olkung`wado Dispensary','vims_id':16790},{'timr_id':12615,'Name':'Monduli District Council','vims_id':14160},{'timr_id':19520,'Name':'Monduli Hospital','vims_id':16308},{'timr_id':17504,'Name':'Monduli juu Dispensary','vims_id':16642},{'timr_id':13434,'Name':'Moshono Dispensary','vims_id':16643},{'timr_id':13421,'Name':'Mother Medical Care Dispensary','vims_id':19264},{'timr_id':13914,'Name':'Mount Meru Hospital','vims_id':16300},{'timr_id':19479,'Name':'Mswakini Dispensary','vims_id':16649},{'timr_id':13005,'Name':'Mswakini juu Dispensary','vims_id':19632},{'timr_id':15087,'Name':'Mti Mmoja Dispensary','vims_id':16799},{'timr_id':18798,'Name':'Mto wa Mbu Health Centre','vims_id':16365},{'timr_id':13900,'Name':'Mundarara Dispensary','vims_id':16653},{'timr_id':15303,'Name':'Mungere Dispensary','vims_id':16797},{'timr_id':13194,'Name':'Mungush Dispensary','vims_id':16654},{'timr_id':15091,'Name':'Musa Dispensary','vims_id':16658},{'timr_id':12631,'Name':'Muungano RC Dispensary','vims_id':16659},{'timr_id':15089,'Name':'Mwandeti Dispensarry','vims_id':16662},{'timr_id':13497,'Name':'Nadosoito Dispensary','vims_id':16667},{'timr_id':19758,'Name':'NAFCO Dispensary','vims_id':19205},{'timr_id':17485,'Name':'Nainokanona Health Centre','vims_id':16368},{'timr_id':12855,'Name':'Naiti Dispensary','vims_id':19206},{'timr_id':19833,'Name':'Naiyob Dispensary ','vims_id':19633},{'timr_id':13901,'Name':'Namanga Dispensary','vims_id':16670},{'timr_id':18312,'Name':'Nambala Dispensary','vims_id':19228},{'timr_id':15065,'Name':'Nduruma Health Centre','vims_id':16370},{'timr_id':13916,'Name':'Neema   Dispensary','vims_id':19103},{'timr_id':19821,'Name':'New Life Disp','vims_id':19601},{'timr_id':13309,'Name':'Ngaibara Dispensay','vims_id':19200},{'timr_id':17903,'Name':'Ngarasero Dispensary','vims_id':16679},{'timr_id':19819,'Name':'Ngarenanyuki Gov','vims_id':16371},{'timr_id':19485,'Name':'Ngarenanyuki Lutheran Dispensary','vims_id':19229},{'timr_id':19473,'Name':'Ngarenaro Health Centre','vims_id':16372},{'timr_id':17490,'Name':'Ngereyani Dispensary','vims_id':19138},{'timr_id':17468,'Name':'Ngongongare Dispensary','vims_id':19230},{'timr_id':15092,'Name':'Ngorbob Dispensary','vims_id':16683},{'timr_id':12612,'Name':'Ngorongoro District Council','vims_id':14182},{'timr_id':17521,'Name':'Ngurudoto RC Dispensary','vims_id':19231},{'timr_id':17463,'Name':'Ngyani Dispensary','vims_id':16686},{'timr_id':15099,'Name':'Ngyeku Dispensary','vims_id':16688},{'timr_id':13913,'Name':'Njiro SDA Dispensary','vims_id':19265},{'timr_id':19755,'Name':'Nkoanekoli Dispensary','vims_id':16691},{'timr_id':19481,'Name':'Nkoanrua Dispensary','vims_id':16692},{'timr_id':13717,'Name':'Nkoaranga Lutheran Hospital','vims_id':16803},{'timr_id':13165,'Name':'Nkoarisambu Dispensary','vims_id':19232},{'timr_id':13902,'Name':'Noondoto Dispensary','vims_id':16695},{'timr_id':17121,'Name':'Nsengony Dispensary','vims_id':16697},{'timr_id':16853,'Name':'Olarash Dispensary','vims_id':16703},{'timr_id':13176,'Name':'Olasiti SDA Dispensary','vims_id':19266},{'timr_id':13074,'Name':'Olbalbal Dispensary','vims_id':16704},{'timr_id':17508,'Name':'Old Arusha Healh Centre','vims_id':16812},{'timr_id':15088,'Name':'Oldadai Dispensary','vims_id':16706},{'timr_id':13751,'Name':'Oldean Health Centre','vims_id':16375},{'timr_id':15281,'Name':'Oldonyosambu Dispensary','vims_id':16376},{'timr_id':13210,'Name':'Oldonyosambu Health Centre','vims_id':16708},{'timr_id':18747,'Name':'Oldonyosapuku Dispensary','vims_id':16707},{'timr_id':15078,'Name':'Oldonyowasi Dispensary','vims_id':19625},{'timr_id':13455,'Name':'Olgilai Dispensary','vims_id':16377},{'timr_id':15103,'Name':'Oljoro JKT Dispensary','vims_id':19623},{'timr_id':17457,'Name':'Oljoro Road Dispensary','vims_id':19267},{'timr_id':15097,'Name':'Olkereyani Dispensary','vims_id':19268},{'timr_id':18057,'Name':'Olkokola Health Centre','vims_id':16378},{'timr_id':15062,'Name':'Olkokola RC  Dispensary','vims_id':19613},{'timr_id':18378,'Name':'Olmolog Dispensary','vims_id':19139},{'timr_id':13904,'Name':'Olmot Dispensary','vims_id':19773},{'timr_id':18078,'Name':'Olmotonyi Dispensary','vims_id':19607},{'timr_id':15100,'Name':'Oloipiri Dispensary','vims_id':19218},{'timr_id':19824,'Name':'Oloirien  Dispensary','vims_id':19270},{'timr_id':13438,'Name':'Oloirien Community Clinic','vims_id':19269},{'timr_id':17910,'Name':'Oloirobi Dispensary','vims_id':19217},{'timr_id':18757,'Name':'Oltrument Hospital','vims_id':16379},{'timr_id':15299,'Name':'Oltukai Dispensary','vims_id':16796},{'timr_id':18304,'Name':'Piyaya Dispensary','vims_id':19219},{'timr_id':17342,'Name':'Poli Rural Health Centre','vims_id':16382},{'timr_id':13501,'Name':'Police Arusha MC Dispensary','vims_id':16715},{'timr_id':17494,'Name':'Qangded Dispensary','vims_id':16719},{'timr_id':15081,'Name':'Qaru Dispensary','vims_id':16721},{'timr_id':19476,'Name':'Qurus Dispensary','vims_id':16723},{'timr_id':15076,'Name':'Rhotia Health Centre','vims_id':19789},{'timr_id':17483,'Name':'Sakala Health Centre','vims_id':16735},{'timr_id':17514,'Name':'Sakila Dispensary','vims_id':16736},{'timr_id':15036,'Name':'Sale Dispensary','vims_id':16737},{'timr_id':19823,'Name':'Samaritan Dispensary','vims_id':19271},{'timr_id':15093,'Name':'Sambasha Dispensary','vims_id':16738},{'timr_id':17487,'Name':'Samunge Dispensary','vims_id':16739},{'timr_id':13910,'Name':'Sanawari Dispensary','vims_id':19272},{'timr_id':19487,'Name':'Seela Singisi Dispensary','vims_id':16743},{'timr_id':19753,'Name':'Selela Dispensary','vims_id':16744},{'timr_id':15094,'Name':'Selian CDH Hospital','vims_id':16809},{'timr_id':17512,'Name':'Sero Dispensary','vims_id':16745},{'timr_id':13405,'Name':'Shree Hindu Health Centre','vims_id':19579},{'timr_id':13315,'Name':'Siha Dispensary','vims_id':19614},{'timr_id':13864,'Name':'Sikh Temple Dispensary','vims_id':19580},{'timr_id':12837,'Name':'Sinon Parish Dispensary','vims_id':19581},{'timr_id':13903,'Name':'Sinya Dispensary','vims_id':19140},{'timr_id':17495,'Name':'Slahhamo Dispensary','vims_id':16751},{'timr_id':13052,'Name':'Soit-Sambu Dispensary','vims_id':16752},{'timr_id':13502,'Name':'Sombetini Dispensary','vims_id':16753},{'timr_id':13375,'Name':'SOS Dipensary','vims_id':19627},{'timr_id':18361,'Name':'Soweto Health Centre','vims_id':19582},{'timr_id':13909,'Name':'St. Elizabeth Hospital','vims_id':16810},{'timr_id':15104,'Name':'St. Michael   Dispensary','vims_id':19583},{'timr_id':13895,'Name':'St. Thomas Health Centre','vims_id':19584},{'timr_id':13906,'Name':'St. Veronica Dispensary','vims_id':19585},{'timr_id':17474,'Name':'Sura Dispensary','vims_id':16758},{'timr_id':19480,'Name':'TAC Dispensary','vims_id':19233},{'timr_id':13771,'Name':'Tanganyika Packers Dispensary','vims_id':19586},{'timr_id':13422,'Name':'Tanzania Christiani Clinic Dispensary','vims_id':19209},{'timr_id':18758,'Name':'Terat Dispensary','vims_id':16762},{'timr_id':13911,'Name':'Terrat Road Dispensary','vims_id':19596},{'timr_id':17464,'Name':'Themi Health Centre','vims_id':16389},{'timr_id':15074,'Name':'Themi ya Simba Dispensary','vims_id':16763},{'timr_id':19827,'Name':'Tingalesingita Dispensary','vims_id':19770},{'timr_id':17437,'Name':'Tingatinga Dispensary','vims_id':16765},{'timr_id':15080,'Name':'Tloma Dispensary','vims_id':16767},{'timr_id':17529,'Name':'TMA Dispensary','vims_id':16800},{'timr_id':16818,'Name':'Total Care Dispensary','vims_id':19587},{'timr_id':18349,'Name':'Tumaini (Karatu) Dispensary','vims_id':19210},{'timr_id':18769,'Name':'Tumaini CRCT Dispensary','vims_id':19588},{'timr_id':13489,'Name':'Tumaini Dispensary','vims_id':19234},{'timr_id':17475,'Name':'Tumaini RC Health Centre','vims_id':19235},{'timr_id':19817,'Name':'UNCTR','vims_id':19589},{'timr_id':17443,'Name':'Upendo   Dispensary','vims_id':19590},{'timr_id':17456,'Name':'Upone Dispensary','vims_id':16394},{'timr_id':13466,'Name':'Uppe Kitete Dispensary','vims_id':16773},{'timr_id':19262,'Name':'Usa RC Health Centre','vims_id':19600},{'timr_id':17515,'Name':'Usa River Gvt Health Centre','vims_id':16394},{'timr_id':17509,'Name':'Valeska Dispensary','vims_id':19236},{'timr_id':19486,'Name':'Wasso Hospital','vims_id':16321},{'timr_id':19849,'Name':'Hope Dispensary','vims_id':19937},{'timr_id':12678,'Name':'Kam Medical Health Centre','vims_id':19936}]";
		private static JArray facilityMapping = JArray.Parse(facility_mapping);


		public static int GetVimsHealthFacilityFacilityId(int TimrId)
		{
			try
			{
				int count = facilityMapping.Count;
				for (int i = 0; i < count; i++)
				{
					if (TimrId == Helper.ConvertToInt(facilityMapping[i]["timr_id"]))
					{
						return ((int)facilityMapping[i]["vims_id"]);
					}
				}
				Exception e = new Exception();
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
			int id=0;
			try
			{
				int counter = facilityMapping.Count;
				for (int p = 0; p < counter; p++)
				{

					if (VimsId == Helper.ConvertToInt(facilityMapping[p]["vims_id"]))
					{
						id = ((int)facilityMapping[p]["timr_id"]);
						return id;
					}
				}
				throw new Exception();
			}
			catch
			{
				Exception e = new Exception("Mapping for  vims_id = " + id + " does not exist");
				throw e;
			}
		}

	}
}