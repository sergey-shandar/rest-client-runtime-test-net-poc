using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Rest.ClientRuntime.Test.JsonRpc;
using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Rest.ClientRuntime.Test.Utf8;
using System.Linq;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public static class HttpSendMock
    {
        public const string SdkRemoteServer = "SDK_REMOTE_SERVER";

        public static Process StartProcess()
        {
            var processName = Environment.GetEnvironmentVariable(SdkRemoteServer);
            if (string.IsNullOrWhiteSpace(processName))
            {
                throw new Exception($"{SdkRemoteServer} is not specified.");
            }

            var processInfo = new ProcessStartInfo
            {
                FileName = processName,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };

            var process = new Process { StartInfo = processInfo };
            try
            {
                process.Start();
            }
            catch (Exception e)
            {
                throw new Exception($"Can't start the server {processName}: {e.Message}", e);
            }
            return process;
        }

        public static string GetValue(IEnumerable<Tuple<string, string>> array, string key)
            => array.FirstOrDefault(s => s.Item1 == key)?.Item2;

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
            //
            var method = request.Method.Method;
            var paramsStr = request.Content.AsString();
            using (var writer = File.AppendText(@"mock.log"))
            {
                writer.WriteLine(method);
                writer.WriteLine(paramsStr);
            }

            // Parse Connection String
            // for example:
            // "SubscriptionId=...;ServicePrincipal=...;ServicePrincipalSecret=...;AADTenant=...;"
            var connectionString = Environment.GetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION");
            var split = (connectionString ?? string.Empty)
                .Split(';')
                .SelectMany(s => {
                    var p = s.IndexOf('=');
                    return p <= 0 
                        ? new Tuple<string, string>[] { }
                        : new[] { Tuple.Create(s.Substring(0, p), s.Substring(p + 1)) };
                })
                .ToArray();

            var @params = JsonConvert.DeserializeObject<Dictionary<string, object>>(paramsStr);
            @params["__reserved"] = new Reserved
            {
                credentials = new Credentials
                {
                    clientId = GetValue(split, "ServicePrincipal"),
                    tenantId = GetValue(split, "AADTenant"),
                    secret = GetValue(split, "ServicePrincipalSecret"),
                }
            };

            Result<object> response;
            using(var process = StartProcess())
            {
                try
                {
                    var remoteServer = new RemoteServer(process.CreateIo(), new Marshalling(null, null));
                    response = await remoteServer.Call<Result<object>>(request.Method.Method, @params);
                }
                finally
                {
                    process.Kill();
                }
            }            
            
            return new HttpResponseMessage(response.statusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(response.response))
            };
        }
    }
}
