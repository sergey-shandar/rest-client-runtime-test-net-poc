using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Rest.ClientRuntime.Test.JsonRpc;
using System;
using System.IO;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public static class HttpSendMock
    {
        private static Lazy<RemoteServer> _RemoteServer = new Lazy<RemoteServer>(() =>
        {
            var serverPath = Environment.GetEnvironmentVariable("SDK_REMOTE_SERVER");
            return null;
        });

        /// <summary>
        /// TODO:
        ///     - ask Joel to have encrypted credentials
        ///     - create log.
        ///     - read credentials from the same ENV varaible.
        ///     - instantiate service if needed.
        ///     - publish logs on appveyor.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var paramsStr = request.Content.AsString();
            var @params = JsonConvert.DeserializeObject<Dictionary<string, object>>(paramsStr);
            using (var writer = File.AppendText("mock.log"))
            {
                writer.WriteLine(paramsStr);
            }
            var response = await _RemoteServer.Value.Call<JsonToken>(request.Method.Method, @params);
            return null;
        }
    }
}
