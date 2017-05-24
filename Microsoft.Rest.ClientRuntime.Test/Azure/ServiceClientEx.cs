using Microsoft.Rest.Azure;
using Microsoft.Rest.ClientRuntime.Test.JsonRpc;
using System.Collections.Generic;
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
                @params[p.Name] = p.Value;
            }
            var method = request.Info.Title + "." + request.Info.Id;
            var response = await HttpSendMock.RemoteServerCall<Response<R>>(method, @params);
            return new AzureOperationResponse<R>
            {
                Body = response.result
            };
        }

        private static async Task<AzureOperationResponse<R>> HttpCall<T, R>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse<R>> _)
            where T : ServiceClient<T>, IAzureClient
        {
            var httpRequest = new HttpRequestMessage
            {
                Method = new HttpMethod(request.Info.Method),
                RequestUri = request.BaseUri,
            };
            return new AzureOperationResponse<R>();
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
