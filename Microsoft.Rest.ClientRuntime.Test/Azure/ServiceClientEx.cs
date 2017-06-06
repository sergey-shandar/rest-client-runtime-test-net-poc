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
        private static async Task<AzureOperationResponse<R, H>> JsonRpcCall<T, R, H>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse<R>> _)
            where T : ServiceClient<T>, IAzureClient
        {
            var @params = new Dictionary<string, object>();
            foreach (var p in request.ParamList)
            {
                @params[p.Info.Name] = p.Value;
            }
            var method = request.Info.Title + "." + request.Info.Id;
            var response = await HttpSendMock.RemoteServerCall<Response<R>>(method, @params);
            return new AzureOperationResponse<R, H>
            {
                Body = response.result
            };
        }

        private static string GetUriValue(this AzureParam param)
            => Uri.EscapeDataString(param.Value.ToString());

        private static string GetUrlParam(this AzureRequest request, string name)
            => request.ConstAndParamList.First(v => v.Info.Name == name).GetUriValue();

        public static string GetPath(this AzureRequest request, IEnumerable<AzurePathPart> path)
           => path
               .Select(p => p.IsParam ? request.GetUrlParam(p.Value) : p.Value)
               .Aggregate((a, b) => a + b);

        private static string GetPath(this AzureRequest request)
            => request.GetPath(request.Info.Path);

        private static async Task<AzureOperationResponse<R, H>> HttpBeginCall<T, R, H>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse<R, H>> _)
            where T : ServiceClient<T>, IAzureClient
        {
            var cpList = request.ConstAndParamList;

            var query = string.Join(
                "&",
                cpList
                    .Where(p => p.Info.Location == AzureParamLocation.Query)
                    .Select(p => p.Info.Name + "=" + p.GetUriValue()));

            if (query != string.Empty)
            {
                query = "?" + query;
            }

            var body = cpList
                .Where(p => p.Info.Location == AzureParamLocation.Body)
                .Select(p => SafeJsonConvert.SerializeObject(p.Value, client.SerializationSettings))
                .FirstOrDefault();

            var httpRequest = new HttpRequestMessage
            {
                Method = new HttpMethod(request.Info.Method),
                RequestUri = new Uri(request.GetBaseUri(), request.GetPath() + query),
                Content = new StringContent(body, Encoding.UTF8),
            };

            foreach (var p in cpList.Where(p => p.Info.Location == AzureParamLocation.Header))
            {
                httpRequest.Headers.Add(p.Info.Name, p.Value.ToString());
            }

            var response = await client.HttpClient.SendAsync(httpRequest);

            var responseContent = await response.Content.ReadAsStringAsync();

            return new AzureOperationResponse<R, H>
            {
                Body = SafeJsonConvert.DeserializeObject<R>(responseContent, client.DeserializationSettings),
                Response = response,
            };
        }

        private static async Task<AzureOperationResponse<R, H>> HttpCall<T, R, H>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse<R, H>> _)            
            where T : ServiceClient<T>, IAzureClient
            where R : class
            where H : class
        {
            var response = await client.HttpBeginCall(request, _);
            if (request.Info.IsLongRunningOperation)
            {                
                return await client
                    .GetPutOrPatchOperationResultAsync(response, request.CustomHeaders, request.CancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                return response;
            }
        }

        public static Task<AzureOperationResponse<R, H>> Call<T, R, H>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse<R, H>> tag)
            where T : ServiceClient<T>, IAzureClient
            where R : class
            where H : class
            // => client.JsonRpcCall(request, tag);
            => client.HttpCall(request, tag);

        public static async Task<AzureOperationResponse<R>> Call<T, R>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse<R>> tag)
            where T : ServiceClient<T>, IAzureClient
            where R : class
        {
            var result = await client.Call(request, new Tag<AzureOperationResponse<R, object>>());
            return new AzureOperationResponse<R>
            {
                Body = result.Body,
                Response = result.Response                
            };
        }

        public static async Task<AzureOperationHeaderResponse<H>> Call<T, H>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationHeaderResponse<H>> tag)
            where T : ServiceClient<T>, IAzureClient
            where H : class
        {
            var result = await client.Call(request, new Tag<AzureOperationResponse<object, H>>());
            return new AzureOperationHeaderResponse<H>
            {
                Headers = result.Headers,
                Response = result.Response,
            };
        }

        public static async Task<AzureOperationResponse> Call<T>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse> _)
            where T : ServiceClient<T>, IAzureClient
        {
            await client.Call(request, new Tag<AzureOperationResponse<object, object>>());
            return new AzureOperationResponse();
        }
    }
}
