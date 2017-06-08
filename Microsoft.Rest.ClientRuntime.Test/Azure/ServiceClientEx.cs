using Microsoft.Rest.Azure;
using Microsoft.Rest.Azure.OData;
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
        private static async Task<AzureOperationResponse<R, H>> JsonRpcCall<T, R, H, E>(
            this T client,
            AzureRequest<E> request)
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

        private static string GetHttpString(this AzureParam param)
            => param.Value is bool b ? (b ? "true" : "false") : param.Value.ToString();

        private static string GetUriValue(this AzureParam param)
            => Uri.EscapeDataString(param.GetHttpString());

        private static string GetQueryParam(this AzureParam param)
        {
            var paramType = param.Value.GetType();
            return paramType.IsConstructedGenericType && paramType.GetGenericTypeDefinition() == typeof(ODataQuery<>)
                ? param.Value.ToString()
                : param.Info.Name + "=" + param.GetUriValue();
        }

        private static string GetUrlParam(this IAzureRequest request, string name)
            => request.GetConstAndParamList().First(v => v.Info.Name == name).GetUriValue();

        public static string GetPath(this IAzureRequest request, IEnumerable<AzurePathPart> path)
           => path
               .Select(p => p.IsParam ? request.GetUrlParam(p.Value) : p.Value)
               .Aggregate((a, b) => a + b);

        private static string GetPath(this IAzureRequest request)
            => request.GetPath(request.Info.Path);

        private static Task<HttpResponseMessage> LogAndSendAsync(this HttpClient client, HttpRequestMessage request)
            => client.SendAsync(request);

        private static async Task<AzureOperationResponse<R, H>> HttpBeginCall<T, R, H, E>(
            this T client,
            AzureRequest<E> request)
            where T : ServiceClient<T>, IAzureClient
        {
            // null validation
            foreach (var p in request.ParamList)
            {
                if (p.Value == null)
                {
                    if (p.Info.IsRequired)
                    {
                        throw new ValidationException(ValidationRules.CannotBeNull, p.Info.Name);
                    }
                }
                else
                {
                    foreach (var c in p.Info.Constraints)
                    {
                        c.Validate(p);
                    }
                }
            }

            var cpList = request.GetConstAndParamList();

            var query = string.Join(
                "&",
                cpList
                    .Where(p => p.Info.Location == AzureParamLocation.Query && p.Value != null)
                    .Select(GetQueryParam));

            if (query != string.Empty)
            {
                query = "?" + query;
            }

            var requestContent = cpList
                .Where(p => p.Info.Location == AzureParamLocation.Body)
                .Select(p => SafeJsonConvert.SerializeObject(p.Value, client.SerializationSettings))
                .FirstOrDefault();

            var httpRequest = new HttpRequestMessage
            {
                Method = new HttpMethod(request.Info.Method),
                RequestUri = new Uri(request.GetBaseUri(), request.GetPath() + query),
                Content = requestContent == null ? null : new StringContent(requestContent, Encoding.UTF8),
            };

            foreach (var p in cpList.Where(p => p.Info.Location == AzureParamLocation.Header))
            {
                httpRequest.Headers.Add(p.Info.Name, p.GetHttpString());
            }

            var httpResponse = await client.HttpClient.LogAndSendAsync(httpRequest);

            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw request.Info.CreateException(new AzureError<E>(
                    string.Format("Operation returned an invalid status code '{0}'", httpResponse.StatusCode),
                    new HttpRequestMessageWrapper(httpRequest, requestContent),
                    new HttpResponseMessageWrapper(httpResponse, responseContent),
                    SafeJsonConvert.DeserializeObject<E>(responseContent, client.DeserializationSettings)));
            }

            return new AzureOperationResponse<R, H>
            {
                Body = SafeJsonConvert.DeserializeObject<R>(responseContent, client.DeserializationSettings),
                Request = httpRequest,
                Response = httpResponse,
            };
        }

        private static async Task<AzureOperationResponse<R, H>> HttpCall<T, R, H, E>(
            this T client,
            AzureRequest<E> request)            
            where T : ServiceClient<T>, IAzureClient
            where R : class
            where H : class
        {
            var response = await client.HttpBeginCall<T, R, H, E>(request);
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

        public static Task<AzureOperationResponse<R, H>> DispatchCall<T, R, H, E>(
            this T client,
            AzureRequest<E> request)
            where T : ServiceClient<T>, IAzureClient
            where R : class
            where H : class
            => string.IsNullOrWhiteSpace(HttpSendMock.GetProcessName()) 
            ? client.HttpCall<T, R, H, E>(request) 
            : client.JsonRpcCall<T, R, H, E>(request);

        public static async Task<AzureOperationResponse<R, H>> Call<T, R, H, F, E>(
            this T client,
            AzureRequest<E> request,
            Tag<AzureOperationResponse<R, H>> tag,
            Tag<F> tagF)
            where T : ServiceClient<T>, IAzureClient
            where R : class
            where H : class
            where F : class, R
        {
            var result = await client.DispatchCall<T, F, H, E>(request);
            return new AzureOperationResponse<R, H>
            {
                Headers = result.Headers,
                Body = result.Body,
                Request = result.Request,
                Response = result.Response
            };
        }

        public static async Task<AzureOperationResponse<R>> Call<T, R, F, E>(
            this T client,
            AzureRequest<E> request,
            Tag<AzureOperationResponse<R>> tag,
            Tag<F> tagF)
            where T : ServiceClient<T>, IAzureClient
            where R : class
            where F : class, R
        {
            var result = await client.DispatchCall<T, F, object, E>(request);
            return new AzureOperationResponse<R>
            {
                Body = result.Body,
                Request = result.Request,
                Response = result.Response                
            };
        }

        public static async Task<AzureOperationHeaderResponse<H>> Call<T, H, E>(
            this T client,
            AzureRequest<E> request,
            Tag<AzureOperationHeaderResponse<H>> tag)
            where T : ServiceClient<T>, IAzureClient
            where H : class
        {
            var result = await client.DispatchCall<T, object, H, E>(request);
            return new AzureOperationHeaderResponse<H>
            {
                Headers = result.Headers,
                Request = result.Request,
                Response = result.Response,
            };
        }

        public static async Task<AzureOperationResponse> Call<T, E>(
            this T client,
            AzureRequest<E> request,
            Tag<AzureOperationResponse> _)
            where T : ServiceClient<T>, IAzureClient
        {
            var result = await client.DispatchCall<T, object, object, E>(request);
            return new AzureOperationResponse()
            {
                Request = result.Request,
                Response = result.Response
            };
        }
    }
}
