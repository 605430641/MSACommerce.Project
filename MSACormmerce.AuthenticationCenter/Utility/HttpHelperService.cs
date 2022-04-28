using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AgileFramework.Common.Models;
using MSACormmerce.AuthenticationCenter.Model;

namespace MSACormmerce.AuthenticationCenter.Utility
{
    /// <summary>
    /// 就是去调用服务的---暂时没有Consul---ToDo
    /// </summary>
    public class HttpHelperService
    {

        #region Option注入
        private readonly ConfigInformation _ConfigInformation;
        public HttpHelperService(IOptionsMonitor<ConfigInformation> configInformation)
        {
            this._ConfigInformation = configInformation.CurrentValue;
        }
        #endregion

        public AjaxResult<User> VerifyUser(string name, string password)
        {
            string requestUrl = $"{_ConfigInformation.RootUrl}{_ConfigInformation.UserUrl}?username={name}&password={password}";
            Console.WriteLine(requestUrl);

            AjaxResult<User> ajaxResult = null;

            HttpResponseMessage sResult = this.HttpRequest(requestUrl, HttpMethod.Get, null);
            if (sResult.IsSuccessStatusCode)
            {
                string content = sResult.Content.ReadAsStringAsync().Result;
                AjaxResult<User> remoteResult = Newtonsoft.Json.JsonConvert.DeserializeObject<AjaxResult<User>>(content);
                ajaxResult = remoteResult;
                //    new AjaxResult<User>()
                //{
                //    StatusCode = (int)sResult.StatusCode,
                //    Result = true,
                //    Value=
                //    TValue = (User)remoteResult.Value
                //};
            }
            else
            {
                ajaxResult = new AjaxResult<User>()
                {
                    StatusCode = (int)sResult.StatusCode,
                    Result = false,
                };
            }
            return ajaxResult;
        }

        public HttpResponseMessage HttpRequest(string url, HttpMethod httpMethod, Dictionary<string, string> parameter)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage message = new HttpRequestMessage()
                {
                    Method = httpMethod,
                    RequestUri = new Uri(url)
                };
                if (parameter != null)
                {
                    var encodedContent = new FormUrlEncodedContent(parameter);
                    message.Content = encodedContent;
                }
                return httpClient.SendAsync(message).Result;
            }
        }


    }
}
