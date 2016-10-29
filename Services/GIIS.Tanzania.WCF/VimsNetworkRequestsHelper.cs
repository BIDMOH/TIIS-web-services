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
	class Program
	{
		
		public static string GetSourceForMyShowsPage(string downloadUrl)
		{
			using (var client = new WebClientEx())
			{
				var values = new NameValueCollection
			{
				{ "j_username", "vims-divo" },
				{ "j_password", "Admin123" },
			};
				// Authenticating into vims
				client.UploadValues("http://uat.tz.elmis-dev.org/j_spring_security_check", values);
				// Downloading desired page
				return client.DownloadString(downloadUrl);
			}
		}
	}

	/// <summary>
	/// A custom WebClient featuring a cookie container
	/// </summary>

	public class WebClientEx : WebClient
	{
		public CookieContainer CookieContainer { get; private set; }

		public WebClientEx()
		{
			CookieContainer = new CookieContainer();
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			var request = base.GetWebRequest(address);
			if (request is HttpWebRequest)
			{
				(request as HttpWebRequest).CookieContainer = CookieContainer;
			}
			return request;
		}
	}
}