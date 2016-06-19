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
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace GIIS.Tanzania.WCF
{
    /// <summary>
    /// Service Credentials validator
    /// </summary>
    public class ServiceCredentialValidator : ServiceAuthorizationManager
    {

        public override bool CheckAccess(OperationContext operationContext)
        {

            var basicAuth = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
            if (basicAuth == null)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                return false;
            }
            else
            {
                string encodedBasicAuth = basicAuth.Replace("Basic ", "");
                string[] decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encodedBasicAuth)).Split(':');
				string userName = decoded[0], password = decoded[1];

                if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
                    throw new ArgumentNullException();

                var user = User.GetDataByUsernameAndPassword(userName, password);
                if (user == null || !user.IsActive)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    return false;
                }
                else
                {
                    user.Isloggedin = true;
                    user.Lastlogin = DateTime.Now;
                    User.Update(user);
                    return true;
                }

            }
            //if(operationContext.IncomingMessageHeaders.f)
            //if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
            //    throw new ArgumentNullException();

            //var user = User.GetDataByUsernameAndPassword(userName, password);
            //if (user == null || !user.IsActive)
            //    throw new SecurityTokenException("Invalid username or password");
            //else
            //{
            //    user.Isloggedin = true;
            //    user.Lastlogin = DateTime.Now;
            //    User.Update(user);
            //}

        }
    }
}
