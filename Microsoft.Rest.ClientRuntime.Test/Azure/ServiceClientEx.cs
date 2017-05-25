using Microsoft.Rest.Azure;
using Microsoft.Rest.ClientRuntime.Test.JsonRpc;
using Microsoft.Rest.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public static class ServiceClientEx
    {
        private static async Task<AzureOperationResponse<R>> JsonRpcCall<T, R>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse<R>> _)
            where T : ServiceClient<T>, IAzureClient
        {
            var @params = new Dictionary<string, object>();
            @params["subscriptionId"] = request.SubscriptionId;
            foreach (var p in request.ParamList)
            {
                @params[p.Info.Name] = p.Value;
            }
            var method = request.Info.Title + "." + request.Info.Id;
            var response = await HttpSendMock.RemoteServerCall<Response<R>>(method, @params);
            return new AzureOperationResponse<R>
            {
                Body = response.result
            };
        }

        private static string GetUriValue(this AzureParam param)
            => Uri.EscapeDataString(param.Value.ToString());

        private static string GetUrlParam(this AzureRequest request, string name)
            => request.ParamList.First(v => v.Info.Name == name).GetUriValue();

        private static string GetPath(this AzureRequest request)
            => request.Info.Path
                .Select(p => p.IsParam ? request.GetUrlParam(p.Value) : p.Value)
                .Aggregate((a, b) => a + b);

        private static async Task<AzureOperationResponse<R>> HttpCall<T, R>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse<R>> _)
            where T : ServiceClient<T>, IAzureClient
        {
            var query = string.Join(
                "&",
                request.ParamList
                    .Where(p => p.Info.Location == AzureParamLocation.Query)
                    .Select(p => p.Info.Name + "=" + p.GetUriValue()));
            if (query != string.Empty)
            {
                query = "?" + query;
            }
            var body = request.ParamList
                .Where(p => p.Info.Location == AzureParamLocation.Body)
                .Select(p => SafeJsonConvert.SerializeObject(p.Value, client.SerializationSettings))
                .FirstOrDefault();
            var httpRequest = new HttpRequestMessage
            {
                Method = new HttpMethod(request.Info.Method),
                RequestUri = new Uri(request.BaseUri, request.GetPath() + query),
                Content = new StringContent(body, Encoding.UTF8),
            };
            var response = await client.HttpClient.SendAsync(httpRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            return new AzureOperationResponse<R>
            {
                Body = SafeJsonConvert.DeserializeObject<R>(responseContent, client.DeserializationSettings)
            };
        }

        public static Task<AzureOperationResponse<R>> Call<T, R>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse<R>> tag)
            where T : ServiceClient<T>, IAzureClient
            // => client.JsonRpcCall(request, tag);
            => client.HttpCall(request, tag);

        public static async Task<AzureOperationResponse> Call<T>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse> _)
            where T : ServiceClient<T>, IAzureClient
        {
            await client.Call(request, new Tag<AzureOperationResponse<object>>());
            return new AzureOperationResponse();
        }
    }
}
