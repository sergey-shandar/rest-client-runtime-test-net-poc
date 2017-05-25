using Microsoft.Rest.Azure;
using Microsoft.Rest.ClientRuntime.Test.JsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        private static string GetUrlParam(this AzureRequest request, string name)
            => Uri.EscapeDataString(request.ParamList.First(v => v.Info.Name == name).Value.ToString());

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
            var httpRequest = new HttpRequestMessage
            {
                Method = new HttpMethod(request.Info.Method),
                RequestUri = new Uri(request.BaseUri, request.GetPath()),
            };
            var response = await client.HttpClient.SendAsync(httpRequest);
            return new AzureOperationResponse<R>
            {
                // Body = response.Content
            };
        }

        public static Task<AzureOperationResponse<R>> Call<T, R>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse<R>> tag)
            where T : ServiceClient<T>, IAzureClient
            => client.JsonRpcCall(request, tag);

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
