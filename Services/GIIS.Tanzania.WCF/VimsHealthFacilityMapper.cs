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
		private static string facility_mapping = "[{'name':'UsariverGVT', 'vims_id':'16394', 'timr_id':'17515'},{'name':'KaloleniHC','vims_id':'16333','timr_id':'19472'},{'name':'Kikatiti','vims_id':'19221','timr_id':'19465'},{'name':'Sombetini','vims_id':'16753','timr_id':'13502'},{'name':'St.Elizabeth','vims_id':'16810','timr_id':'13909'},{'name':'Aicc','vims_id':'16815','timr_id':'15037'},{'name':'ArushaLutheranMedicalHospital','vims_id':'16802','timr_id':'17480'},{'name':'IthnaAsheri','vims_id':'16805','timr_id':'17460'},{'name':'KaloleniHealthCentre','vims_id':'16333','timr_id':'19472'},{'name':'Levolosi','vims_id':'16345','timr_id':'15101'},{'name':'Mkonoo','vims_id':'16362','timr_id':'17493'},{'name':'Moivaro','vims_id':'16363','timr_id':'18768'},{'name':'Mt.MeruHospital','vims_id':'16300','timr_id':'13914'},{'name':'NgarenaroUHC','vims_id':'16372','timr_id':'19473'},{'name':'OldArushaHealhCentre','vims_id':'16812','timr_id':'17508'},{'name':'Olkereyan','vims_id':'16711','timr_id':'15097'},{'name':'PoliceArushaMCDispensary','vims_id':'16715','timr_id':'13501'},{'name':'Terrat','vims_id':'16762','timr_id':'13911'},{'name':'ThemiHealthCentre','vims_id':'16389','timr_id':'17464'},{'name':'Elang'atadapashi','vims_id':'16439','timr_id':'18764'},{'name':'Engarenaibor','vims_id':'16328','timr_id':'15070'},{'name':'Engikaret','vims_id':'16451','timr_id':'15071'},{'name':'GelaiBomba','vims_id':'16456','timr_id':'17489'},{'name':'GelaiLumbwa','vims_id':'16457','timr_id':'17488'},{'name':'Kamwanga','vims_id':'16489','timr_id':'13898'},{'name':'Kiseriani','vims_id':'16533','timr_id':'15046'},{'name':'LongidoHealthCentre','vims_id':'16347','timr_id':'18377'},{'name':'LongidoDistrictHospital','vims_id':'14133','timr_id':'12613'},{'name':'Matale','vims_id':'16614','timr_id':'15072'},{'name':'Mundarara','vims_id':'16653','timr_id':'13900'},{'name':'Namanga','vims_id':'16670','timr_id':'13901'},{'name':'Noondoto','vims_id':'16695','timr_id':'13902'},{'name':'Tingatinga','vims_id':'16765','timr_id':'17437'},{'name':'Arash','vims_id':'16399','timr_id':'19471'},{'name':'Digodigo','vims_id':'16431','timr_id':'17482'},{'name':'Esere','vims_id':'16452','timr_id':'15298'},{'name':'Irkeekpusi','vims_id':'16479','timr_id':'15296'},{'name':'Kakesio','vims_id':'16486','timr_id':'18306'},{'name':'LoliondoHealthCentre','vims_id':'16346','timr_id':'12995'},{'name':'Magaiduru','vims_id':'16579','timr_id':'13057'},{'name':'MalamboHC','vims_id':'16352','timr_id':'17484'},{'name':'Nainokanoka','vims_id':'16368','timr_id':'17485'},{'name':'Ngarasero','vims_id':'16679','timr_id':'17903'},{'name':'Olbalbal','vims_id':'16704','timr_id':'13074'},{'name':'Oldonyosambu','vims_id':'16707','timr_id':'13210'},{'name':'Oloipiri','vims_id':'16712','timr_id':'15100'},{'name':'Sakala','vims_id':'16735','timr_id':'17483'},{'name':'Sale','vims_id':'16737','timr_id':'15036'},{'name':'Samunge','vims_id':'16739','timr_id':'17487'},{'name':'Sero','vims_id':'16745','timr_id':'17512'},{'name':'Soitsambu','vims_id':'16752','timr_id':'13052'},{'name':'Wasso','vims_id':'16321','timr_id':'19486'},{'name':'Kilinga','vims_id':'16514','timr_id':'18313'},{'name':'Kimundo','vims_id':'16518','timr_id':'17471'},{'name':'Kin'gori','vims_id':'16521','timr_id':'19482'},{'name':'Kipandenkoavele','vims_id':'16524','timr_id':'17465'},{'name':'Leguruki','vims_id':'16554','timr_id':'19484'},{'name':'MajiYaChai','vims_id':'16585','timr_id':'17476'},{'name':'Malula','vims_id':'16591','timr_id':'17466'},{'name':'Mareu','vims_id':'16355','timr_id':'13661'},{'name':'Mbuguni','vims_id':'16359','timr_id':'17531'},{'name':'NambalaDispensary','vims_id':'17130','timr_id':'18312'},{'name':'NgerenanyukiHealthCentre','vims_id':'16371','timr_id':'19754'},{'name':'Ngyani  Dispensary','vims_id':'16686','timr_id':'17463'},{'name':'NgyekuDispensary','vims_id':'16688','timr_id':'15099'},{'name':'NkoanekoliDispensary','vims_id':'16691','timr_id':'19755'},{'name':'Nkoanrua','vims_id':'16692','timr_id':'19481'},{'name':'Nkoaranga','vims_id':'16803','timr_id':'13717'},{'name':'Nsengony','vims_id':'16697','timr_id':'17121'},{'name':'Poli','vims_id':'16382','timr_id':'17342'},{'name':'Sakila','vims_id':'16736','timr_id':'17514'},{'name':'Sura','vims_id':'16758','timr_id':'17474'}]";

		public static int GetVimsHealthFacilityFacilityId(int TimrId)
		{
			JArray facilityMapping = JArray.Parse(facility_mapping);
			int count = facilityMapping.Count;
			for (int i = 0; i < count; i++) 
			{
				if (TimrId==((int)facilityMapping[i]["timr_id"]))
				{
					return ((int)facilityMapping[i]["vims_id"]);
				}
			}
			Exception e = new Exception("Mapping for  timr_id = "+TimrId+" does not exist");
			throw e;
		}


		public static int GetTimrHealthFacilityFacilityId(int VimsId)
		{
			JArray facilityMapping = JArray.Parse(facility_mapping);
			int count = facilityMapping.Count;
			for (int i = 0; i < count; i++)
			{
				if (VimsId==((int)facilityMapping[i]["vims_id"]))
				{
					return ((int)facilityMapping[i]["timr_id"]);
				}
			}
			Exception e = new Exception("Mapping for  vims_id = " + VimsId + " does not exist");
			throw e;
		}

	}
}