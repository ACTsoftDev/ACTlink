using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using actchargers.Code.Utility;
using ModernHttpClient;
using MvvmCross.Platform;
using Newtonsoft.Json;

namespace actchargers
{
    public enum ActviewResponseType
    {
        internalError,
        notValidSession,
        notAuthenticated,
        validResponse,
        invalidResponse,
        notInternet,
        notValidAPI,
        softwareError,
        expiredAPI,
        corruptedRecord,
    }

    public class ACTViewResponse
    {
        public ActviewResponseType responseType;
        public Newtonsoft.Json.Linq.JObject returnValue;
        public string responseString;
    }

    public static class ACTViewConnect
    {
        static IUserPreferences userPreferences;

        static CookieContainer cookie = new CookieContainer();
        static string userName;
        static string Password;

        static NativeCookieHandler cookieHandler = new NativeCookieHandler();

        static async Task<ACTViewResponse>
        ActViewSendRequest
        (ACConstants.APIs apiName, Dictionary<string, string> postParameters,
         Int32 readTimeoutinMilliSeconds, bool redo = true)
        {
            string ServerUrl =
                BaseUrlChooser.ChooseCorrectBaseUrl() + ACConstants.APIsURI[apiName];
            ACTViewResponse actResponse = new ACTViewResponse();
            CancellationTokenSource currentToken;


            var handler = new NativeMessageHandler(false, false, cookieHandler);
            handler.CookieContainer = cookie;
            handler.DisableCaching = true;



            //Allow Untrusted Certificates while making a Service call.
            if (ACConstants.ENABLE_UNTRUSTED_CERTICATE)
            {
                handler.EnableUntrustedCertificates();
            }

            var client = new HttpClient(handler);
            client.Timeout = new TimeSpan(0, 0, 30);
            //client.DefaultRequestHeaders.Authorization = 
            currentToken = new CancellationTokenSource();

            try
            {
                HttpResponseMessage resp = null;
                int RetryCount = 0;

                while (true)
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, ServerUrl);

                    string userAgent = Mvx.Resolve<IPlatformVersion>().GetUserAgentValue();

                    requestMessage.Headers.TryAddWithoutValidation
                                  (ACConstants.ACCEPT,
                                   ACConstants.ACCEPT_PARAMETER);
                    requestMessage.Headers.TryAddWithoutValidation
                                  (ACConstants.USERAGENT,
                                   userAgent);

                    if (postParameters.Count > 0)
                    {
                        requestMessage.Content =
                                          GetEncodedParameters(postParameters);
                    }

                    bool isReachable = await InternetConnectivityManager.IsReachableAsync();

                    if (isReachable)
                    {
                        resp = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, currentToken.Token);

                        if (resp.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            if (RetryCount == 2)
                            {
                                break;
                            }

                            Debug.WriteLine("Some unautorization response");

                            //TODO Need to Uncomment when background tasks need to run
                            //IOTJobManagerService mJobManager = Mvx.Resolve<IOTJobManagerService>();
                            //await mJobManager.ExecuteJob(typeof(OTSessionTokenJob));

                            RetryCount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        actResponse.responseType = ActviewResponseType.notInternet;
                        return actResponse;
                    }
                }

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string strChk = await resp.Content.ReadAsStringAsync();
                    actResponse.responseString = strChk;
                }
                else
                {
                    Logger.AddLog(true, "X3" + "Invalid Response-CALL="
                                  + ACConstants.APIsURI[apiName]);
                    actResponse.responseType = ActviewResponseType.invalidResponse;
                    return actResponse;
                }


                //IEnumerable<Cookie> responseCookies = cookieHandler.Cookies;

                //Retrieving cookies in normal method commented since using the modernhttpclient cookie handler
                var cookieCollection = handler.CookieContainer.GetCookies(new Uri(ServerUrl));
                foreach (Cookie cookieValue in cookieCollection)
                {
                    cookie.Add(new Uri(BaseUrlChooser.ChooseCorrectBaseUrl()), cookieValue);
                }


                Newtonsoft.Json.Linq.JObject res = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(actResponse.responseString.Replace("\r\n", ""));
                if (res["failed"] == null || res["validSession"] == null
                    || res["operationAuthenticated"] == null || res["returnObject"] == null ||
                    res["failed"].Type != Newtonsoft.Json.Linq.JTokenType.Boolean ||
                    res["validSession"].Type != Newtonsoft.Json.Linq.JTokenType.Boolean ||
                    res["operationAuthenticated"].Type != Newtonsoft.Json.Linq.JTokenType.Boolean ||
                    res["returnObject"].Type != Newtonsoft.Json.Linq.JTokenType.Object)
                {
                    if (redo)
                        return await ActViewSendRequest(apiName, postParameters, readTimeoutinMilliSeconds, false);
                    //something strange,
                    Logger.AddLog(true, "X4" + "Invalid Response-CALL2="
                                  + ACConstants.APIsURI[apiName]);
                    actResponse.responseType = ActviewResponseType.invalidResponse;
                    return actResponse;
                }

                if (res["expired"] != null && res["expired"].Type == Newtonsoft.Json.Linq.JTokenType.Boolean && (bool)res["expired"] == true)
                {
                    actResponse.responseType = ActviewResponseType.expiredAPI;
                    return actResponse;
                }
                if ((bool)res["failed"] == true)
                {
                    if (redo)
                        return await ActViewSendRequest(apiName, postParameters, readTimeoutinMilliSeconds, false);
                    actResponse.responseType = ActviewResponseType.internalError;
                    return actResponse;
                }
                if ((bool)res["operationAuthenticated"] == false)
                {
                    actResponse.responseType = ActviewResponseType.notAuthenticated;
                    return actResponse;
                }
                if ((bool)res["validSession"] == false)
                {
                    //Relogin
                    cookie = new CookieContainer();
                    if (redo)
                    {
                        ACTViewResponse res2 = await AuthenticateUser(userName, Password);
                        if (res2.responseType != ActviewResponseType.validResponse)
                            redo = false;

                    }
                    if (redo)
                    {
                        return await ActViewSendRequest(apiName, postParameters, readTimeoutinMilliSeconds, false);
                    }

                    actResponse.responseType = ActviewResponseType.notValidSession;
                    return actResponse;


                }
                actResponse.returnValue = (Newtonsoft.Json.Linq.JObject)res["returnObject"];
                actResponse.responseType = ActviewResponseType.validResponse;
                return actResponse;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                return actResponse;
            }
        }

        static HttpContent GetEncodedParameters
        (Dictionary<string, string> postParameters)
        {
            var stringPayload = JsonParser.SerializeObject(postParameters);
            var content = new StringContent
                (stringPayload, Encoding.UTF8, ACConstants.APPLICATION_JSON);

            return content;
        }

        public static async Task<ACTViewResponse> AuthenticateUser
        (string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                return await AuthenticateCachedUser();
            }
            else
            {
                userName = username;
                Password = password;

                return await AuthenticateByCredentials(username, password);
            }
        }

        public static async Task<ACTViewResponse> AuthenticateCachedUser()
        {
            userPreferences = Mvx.Resolve<IUserPreferences>();

            userName = userPreferences.GetString(ACConstants.PREF_USERNAME);
            Password = userPreferences.GetString(ACConstants.PREF_PASSWORD);

            return await AuthenticateByCredentials(userName, Password);
        }

        public static async Task<ACTViewResponse>
        AuthenticateByCredentials(string username, string password)
        {
            Dictionary<string, string> postParams
            = new Dictionary<string, string>();

            postParams.Add(ACConstants.PREF_USERNAME, username);
            postParams.Add(ACConstants.PREF_PASSWORD, password);
            ACTViewResponse response = await ActViewSendRequest
                (ACConstants.APIs.Login, postParams, 10000);

            if (response.responseType == ActviewResponseType.validResponse)
                DbSingleton.DBManagerServiceInstance.GetGenericObjectsLoader()
                           .InsertOrUpdateUsingFields
                           (ACConstants.DB_LOGIN_DATE,
                            DateTime.UtcNow.ToString());

            return response;
        }

        public static async Task<ACTViewResponse> GetMCBConfig(UInt32 id)
        {
            Dictionary<string, string> postParams = new Dictionary<string, string>
            {
                { "chargerid", id.ToString() }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.MCB_commission, postParams, 10000);

            return response;
        }

        public static async Task<ACTViewResponse> UploadBattViewEvents
        (UInt32 id, UInt32 studyId, string[] battevents)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "id", id.ToString() },
                { "studyId", studyId.ToString() }
            };

            object[] jsonObjects = new object[battevents.Length];

            for (int i = 0; i < battevents.Length; i++)
            {
                jsonObjects[i] = JsonConvert.DeserializeObject(battevents[i]);
            }

            postParams.Add("events", JsonConvert.SerializeObject(jsonObjects));

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.uploadBattViewEvents, postParams, 60000);

            return response;
        }

        public static async Task<ACTViewResponse> UploadMCBCycles
        (UInt32 id, string[] cycles)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "id", id.ToString() }
            };

            object[] jsonObjects = new object[cycles.Length];

            for (int i = 0; i < cycles.Length; i++)
            {
                jsonObjects[i] = JsonConvert.DeserializeObject(cycles[i]);
            }

            postParams.Add("cycles", JsonConvert.SerializeObject(jsonObjects));

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.uploadMCBCycles, postParams, 60000);

            return response;
        }

        public static async Task<ACTViewResponse> UploadMCBFaults
        (UInt32 id, string[] faults)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "id", id.ToString() }
            };

            object[] jsonObjects = new object[faults.Length];

            for (int i = 0; i < faults.Length; i++)
            {
                jsonObjects[i] = JsonConvert.DeserializeObject(faults[i]);
            }

            postParams.Add("faults", JsonConvert.SerializeObject(jsonObjects));

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.uploadMCBfaults, postParams, 60000);

            return response;
        }

        public static async Task<ACTViewResponse> UploadBattViewDeviceObject
        (Dictionary<string, string> postParams)
        {
            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.uploadBattViewDeviceObject, postParams, 12000);

            return response;
        }

        public static async Task<ACTViewResponse> UploadBattViewDeviceObject
        (UInt32 id, UInt32 studyId, string configJson,
         string globalRecordObject, float firmwareVersion, byte zone)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "id", id.ToString() },
                { "config", configJson },
                { "global", globalRecordObject },
                { "firmwareVersion", firmwareVersion.ToString() },
                { "zone", zone.ToString() },
                { "studyId", studyId.ToString() }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.uploadBattViewDeviceObject, postParams, 12000);

            return response;
        }

        public static async Task<ACTViewResponse> UploadBattViewDeviceObjectDirect
        (UInt32 id, UInt32 studyId, string configJson,
         string globalRecordObject, float firmwareVersion, byte zone)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "id", id.ToString() },
                { "config", configJson },
                { "global", globalRecordObject },
                { "firmwareVersion", firmwareVersion.ToString() },
                { "zone", zone.ToString() },
                { "studyId", studyId.ToString() }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.uploadBattObjectDirect, postParams, 12000);

            return response;
        }

        public static async Task<ACTViewResponse> UploadMCBDeviceObject
        (UInt32 id, string configJson, string globalRecordObject,
         float firmwareVersion, byte zone)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "id", id.ToString() },
                { "config", configJson },
                { "global", globalRecordObject },
                { "firmwareVersion", firmwareVersion.ToString() },
                { "zone", zone.ToString() }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.uploadMCBDeviceObject, postParams, 12000);

            return response;
        }

        public static async Task<ACTViewResponse> UploadMCBDeviceObject
        (Dictionary<string, string> postParams)
        {
            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.uploadMCBDeviceObject, postParams, 12000);

            return response;
        }

        public static async Task<ACTViewResponse> UploadMCBDeviceObjectDirect
        (UInt32 id, string configJson, string globalRecordObject,
         float firmwareVersion, byte zone)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "id", id.ToString() },
                { "config", configJson },
                { "global", globalRecordObject },
                { "firmwareVersion", firmwareVersion.ToString() },
                { "zone", zone.ToString() }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.uploadMCBDirectObject, postParams, 12000);

            return response;
        }

        public static async Task<ACTViewResponse> CommissionMCB
        (string configJson, bool updateIfExists)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "config", configJson },
                { "forceUpdate", (updateIfExists ? "true" : "false") }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.MCB_commission, postParams, 30000);

            return response;
        }

        public static async Task<ACTViewResponse> CommissionBattView
        (string configJson, bool updateIfExists)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "config", configJson },
                { "forceUpdate", (updateIfExists ? "true" : "false") }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.Batt_commission, postParams, 30000);

            return response;
        }

        public static async Task<ACTViewResponse> UserExist(UInt32 userID)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "id", userID.ToString() }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.userExit, postParams, 10000);

            return response;
        }

        public static async Task<ACTViewResponse> GetQuantumVersions()
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>();

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.getQuantumVersions, postParams, 10000);

            return response;
        }

        public static async Task<ACTViewResponse> GetUserList
        (int type, UInt32 optionalCustomerID)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>();

            if (type == 2)
                postParams.Add("customerId", optionalCustomerID.ToString());

            postParams.Add("reqType", type.ToString());

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.getusersList, postParams, 60000);

            return response;
        }

        public static async Task<ACTViewResponse> GetUserSites(UInt32 startFrom)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "startFrom", startFrom.ToString() }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.getUserSites, postParams, 60000);

            return response;
        }

        public static async Task<ACTViewResponse> DownloadMCBConfig
        (UInt32 customerID, UInt32 siteId)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "customerId", customerID.ToString() },
                { "siteId", siteId.ToString() }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.downloadMCBConfig, postParams, 60000);

            return response;
        }

        public static async Task<ACTViewResponse> DownloadBattViewConfig
        (UInt32 customerID, UInt32 siteId)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "customerId", customerID.ToString() },
                { "siteId", siteId.ToString() }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.downloadBattViewConfig, postParams, 60000);

            return response;
        }

        public static async Task<ACTViewResponse> ReplaceMCBDevice
        (UInt32 originalId, UInt32 newID)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "originalId", originalId.ToString() },
                { "newId", newID.ToString() }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.replaceMCBDevice, postParams, 120000);

            return response;
        }

        public static async Task<ACTViewResponse> ReplaceBattViewDevice
        (UInt32 originalId, UInt32 newID)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "originalId", originalId.ToString() },
                { "newId", newID.ToString() }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.replaceBattViewDevice, postParams, 120000);

            return response;
        }

        public static async Task<ACTViewResponse> UploadFailedDB
        (string db)
        {
            Dictionary<string, string> postParams =
                new Dictionary<string, string>
            {
                { "db", db }
            };

            ACTViewResponse response =
                await ActViewSendRequest
                (ACConstants.APIs.uploadFailedDB, postParams, 120000);

            return response;
        }
    }
}
