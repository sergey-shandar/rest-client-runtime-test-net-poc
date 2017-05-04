using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Rest.ClientRuntime.Test.JsonRpc;
using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using Microsoft.Rest.ClientRuntime.Test.Rpc;
using System.Linq;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public static class HttpSendMock
    {
        private static Lazy<Io> _Io = new Lazy<Io>(() =>
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = Environment.GetEnvironmentVariable("SDK_REMOTE_SERVER"),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
            };

            var process = new Process { StartInfo = processInfo };
            process.Start();
            /*
            
            */
            // processInfo.Environment.Add("AZURE_TENANT_ID", Environment.GetEnvironmentVariable("AZURE_TENANT_ID"));
            // processInfo.Environment.Add("AZURE_CLIENT_ID", Environment.GetEnvironmentVariable("AZURE_CLIENT_ID"));
            // processInfo.Environment.Add("AZURE_CLIENT_SECRET", Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET"));

            return process.CreateIo();
        });

        /// <summary>
        /// TODO:
        ///     - ask Joel to have encrypted credentials
        ///     - create log.
        ///     - read credentials from the same ENV varaible.
        ///     - instantiate a service if needed.
        ///     - publish logs on appveyor.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            // Parse Connection String
            // for example:
            // "SubscriptionId=...;ServicePrincipal=...;ServicePrincipalSecret=...;AADTenant=...;"
            var connectionString = Environment.GetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION");
            var split = connectionString
                .Split(';')
                .Select(s => {
                    var p = s.IndexOf('=');
                    return p <= 0 
                        ? Tuple.Create(string.Empty, string.Empty)
                        : Tuple.Create(s.Substring(0, p), s.Substring(p + 1));
                });

            //
            var method = request.Method.Method;
            var paramsStr = request.Content.AsString();
            using (var writer = File.AppendText(@"C:\projects\azure-rest-api-specs-tests\mock.log"))
            {
                writer.WriteLine(method);
                writer.WriteLine(paramsStr);
            }
            var @params = JsonConvert.DeserializeObject<Dictionary<string, object>>(paramsStr);
            @params["__reserved"] = new Reserved
            {
                credentials = new Credentials
                {
                    clientId = split.First(s => s.Item1 == "ServicePrincipal").Item2,
                    tenantId = split.First(s => s.Item1 == "AADTenant").Item2,
                    secret = split.First(s => s.Item1 == "ServicePrincipalSecret").Item2,
                }
            };
            var remoteServer = new RemoteServer(_Io.Value, new Marshalling(null, null));
            var response = await remoteServer.Call<Result<object>>(request.Method.Method, @params);
            
            return new HttpResponseMessage(response.statusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(response.response))
            };
        }
    }
}
