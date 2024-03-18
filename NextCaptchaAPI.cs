using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NextCaptchaSDK
{
    public class NextCaptchaAPI
    {
        private const string HOST = "https://api.nextcaptcha.com";
        private const string RECAPTCHAV2_TYPE = "RecaptchaV2TaskProxyless";
        private const string RECAPTCHAV2_ENTERPRISE_TYPE = "RecaptchaV2EnterpriseTaskProxyless";
        private const string RECAPTCHAV3_PROXYLESS_TYPE = "RecaptchaV3TaskProxyless";
        private const string RECAPTCHAV3_TYPE = "RecaptchaV3Task";
        private const string RECAPTCHA_MOBILE_TYPE = "RecaptchaMobileProxyless";
        private const string HCAPTCHA_TYPE = "HCaptchaTask";
        private const string HCAPTCHA_PROXYLESS_TYPE = "HCaptchaTaskProxyless";
        private const string HCAPTCHA_ENTERPRISE_TYPE = "HCaptchaEnterpriseTask";
        private const string FUNCAPTCHA_TYPE = "FunCaptchaTask";
        private const string FUNCAPTCHA_PROXYLESS_TYPE = "FunCaptchaTaskProxyless";

        private const int TIMEOUT = 45;

        private const string PENDING_STATUS = "pending";
        private const string PROCESSING_STATUS = "processing";
        private const string READY_STATUS = "ready";
        private const string FAILED_STATUS = "failed";

        private readonly string _clientKey;
        private readonly string _solftId;
        private readonly string _callbackUrl;
        private readonly bool _openLog;
        private readonly HttpClient _httpClient;

        public NextCaptchaAPI(string clientKey, string solftId = "", string callbackUrl = "", bool openLog = true)
        {
            _clientKey = clientKey;
            _solftId = solftId;
            _callbackUrl = callbackUrl;
            _openLog = openLog;
            _httpClient = new HttpClient();
        }

        private async Task<string> GetBalanceAsync()
        {
            var response = await _httpClient.GetAsync($"{HOST}/getBalance?clientKey={_clientKey}");
            if (!response.IsSuccessStatusCode)
            {
                if (_openLog)
                {
                    Console.WriteLine($"Error: {response.StatusCode} {await response.Content.ReadAsStringAsync()}");
                }
                return null;
            }

            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
            if (_openLog)
            {
                Console.WriteLine($"Balance: {result["balance"]}");
            }
            return result["balance"];
        }
        private async Task<Dictionary<string, string>> SendTaskAsync(Dictionary<string, object> task)
        {
            var data = new Dictionary<string, object>
            {
                {"clientKey", _clientKey},
                {"solftId", _solftId},
                {"callbackUrl", _callbackUrl},
                {"task", task}
            };

            var response = await _httpClient.PostAsync($"{HOST}/createTask", new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                if (_openLog)
                {
                    Console.WriteLine($"Error: {response.StatusCode} {await response.Content.ReadAsStringAsync()}");
                    Console.WriteLine($"Data: {JsonConvert.SerializeObject(data)}");
                }
                return null;
            }

            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
            var taskId = result["taskId"];
            if (_openLog)
            {
                Console.WriteLine($"Task {taskId} created {JsonConvert.SerializeObject(result)}");
            }

            var startTime = DateTime.Now;
            while (true)
            {
                if ((DateTime.Now - startTime).TotalSeconds > TIMEOUT)
                {
                    return new Dictionary<string, string>
                    {
                        {"errorId", "12"},
                        {"errorDescription", "Timeout"},
                        {"status", "failed"}
                    };
                }

                response = await _httpClient.PostAsync($"{HOST}/getTaskResult", new StringContent(JsonConvert.SerializeObject(new Dictionary<string, string> { { "clientKey", _clientKey }, { "taskId", taskId } }), System.Text.Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    if (_openLog)
                    {
                        Console.WriteLine($"Error: {response.StatusCode} {await response.Content.ReadAsStringAsync()}");
                    }
                    return null;
                }

                result = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
                var status = result["status"];
                if (_openLog)
                {
                    Console.WriteLine($"Task status: {status}");
                }
                if (status == READY_STATUS)
                {
                    Console.WriteLine($"Task {taskId} ready {JsonConvert.SerializeObject(result)}");
                    return result;
                }
                if (status == FAILED_STATUS)
                {
                    Console.WriteLine($"Task {taskId} failed {JsonConvert.SerializeObject(result)}");
                    return result;
                }
                await Task.Delay(1000);
            }
        }
        public async Task<Dictionary<string, string>> SolveRecaptchaV2Async(string websiteUrl, string websiteKey, string recaptchaDataSValue = "", bool isInvisible = false, string apiDomain = "")
        {
            var task = new Dictionary<string, object>
            {
                {"type", RECAPTCHAV2_TYPE},
                {"websiteURL", websiteUrl},
                {"websiteKey", websiteKey},
                {"recaptchaDataSValue", recaptchaDataSValue},
                {"isInvisible", isInvisible},
                {"apiDomain", apiDomain}
            };
            return await SendTaskAsync(task);
        }
        public async Task<Dictionary<string, string>> SolveRecaptchaV2EnterpriseAsync(string websiteUrl, string websiteKey, Dictionary<string, object> enterprisePayload = null, bool isInvisible = false, string apiDomain = "")
        {
            var task = new Dictionary<string, object>
            {
                {"type", RECAPTCHAV2_ENTERPRISE_TYPE},
                {"websiteURL", websiteUrl},
                {"websiteKey", websiteKey},
                {"enterprisePayload", enterprisePayload ?? new Dictionary<string, object>()},
                {"isInvisible", isInvisible},
                {"apiDomain", apiDomain}
            };
            return await SendTaskAsync(task);
        }

        public async Task<Dictionary<string, string>> SolveRecaptchaV3Async(string websiteUrl, string websiteKey, string pageAction = "", string apiDomain = "", string proxyType = "", string proxyAddress = "", int proxyPort = 0, string proxyLogin = "", string proxyPassword = "")
        {
            var task = new Dictionary<string, object>
            {
                {"type", RECAPTCHAV3_PROXYLESS_TYPE},
                {"websiteURL", websiteUrl},
                {"websiteKey", websiteKey},
                {"pageAction", pageAction},
                {"apiDomain", apiDomain}
            };
            if (!string.IsNullOrEmpty(proxyAddress))
            {
                task["type"] = RECAPTCHAV3_TYPE;
                task["proxyType"] = proxyType;
                task["proxyAddress"] = proxyAddress;
                task["proxyPort"] = proxyPort;
                task["proxyLogin"] = proxyLogin;
                task["proxyPassword"] = proxyPassword;
            }
            return await SendTaskAsync(task);
        }
        public async Task<Dictionary<string, string>> SolveRecaptchaMobileAsync(string appKey, string appPackageName = "", string appAction = "")
        {
            var task = new Dictionary<string, object>
            {
                {"type", RECAPTCHA_MOBILE_TYPE},
                {"appKey", appKey},
                {"appPackageName", appPackageName},
                {"appAction", appAction}
            };
            return await SendTaskAsync(task);
        }

        public async Task<Dictionary<string, string>> SolveHCaptchaAsync(string websiteUrl, string websiteKey, bool isInvisible = false, Dictionary<string, object> enterprisePayload = null, string proxyType = "", string proxyAddress = "", int proxyPort = 0, string proxyLogin = "", string proxyPassword = "")
        {
            var task = new Dictionary<string, object>
            {
                {"type", HCAPTCHA_PROXYLESS_TYPE},
                {"websiteURL", websiteUrl},
                {"websiteKey", websiteKey},
                {"isInvisible", isInvisible},
                {"enterprisePayload", enterprisePayload ?? new Dictionary<string, object>()}
            };
            if (!string.IsNullOrEmpty(proxyAddress))
            {
                task["type"] = HCAPTCHA_TYPE;
                task["proxyType"] = proxyType;
                task["proxyAddress"] = proxyAddress;
                task["proxyPort"] = proxyPort;
                task["proxyLogin"] = proxyLogin;
                task["proxyPassword"] = proxyPassword;
            }
            return await SendTaskAsync(task);
        }

        public async Task<Dictionary<string, string>> SolveHCaptchaEnterpriseAsync(string websiteUrl, string websiteKey, Dictionary<string, object> enterprisePayload = null, bool isInvisible = false, string proxyType = "", string proxyAddress = "", int proxyPort = 0, string proxyLogin = "", string proxyPassword = "")
        {
            var task = new Dictionary<string, object>
            {
                {"type", HCAPTCHA_ENTERPRISE_TYPE},
                {"websiteURL", websiteUrl},
                {"websiteKey", websiteKey},
                {"enterprisePayload", enterprisePayload ?? new Dictionary<string, object>()},
                {"isInvisible", isInvisible},
                {"proxyType", proxyType},
                {"proxyAddress", proxyAddress},
                {"proxyPort", proxyPort},
                {"proxyLogin", proxyLogin},
                {"proxyPassword", proxyPassword}
            };
            return await SendTaskAsync(task);
        }

        public async Task<Dictionary<string, string>> SolveFunCaptchaAsync(string websitePublicKey, string websiteUrl = "", string data = "", string proxyType = "", string proxyAddress = "", int proxyPort = 0, string proxyLogin = "", string proxyPassword = "")
        {
            var task = new Dictionary<string, object>
            {
                {"type", FUNCAPTCHA_PROXYLESS_TYPE},
                {"websiteURL", websiteUrl},
                {"websitePublicKey", websitePublicKey},
                {"data", data}
            };
            if (!string.IsNullOrEmpty(proxyAddress))
            {
                task["type"] = FUNCAPTCHA_TYPE;
                task["proxyType"] = proxyType;
                task["proxyAddress"] = proxyAddress;
                task["proxyPort"] = proxyPort;
                task["proxyLogin"] = proxyLogin;
                task["proxyPassword"] = proxyPassword;
            }
            return await SendTaskAsync(task);
        }

        public async Task<string> GetBalanceAsync()
        {
            return await GetBalanceAsync();
        }
    }
}