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
            AzureRequest request)
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
                httpRequest.Headers.Add(p.Info.Name, p.Value.ToString());
            }

            var httpResponse = await client.HttpClient.SendAsync(httpRequest);

            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new CloudException(string.Format("Operation returned an invalid status code '{0}'", httpResponse.StatusCode))
                {
                    Request = new HttpRequestMessageWrapper(httpRequest, requestContent),
                    Response = new HttpResponseMessageWrapper(httpResponse, responseContent)
                };
            }

            return new AzureOperationResponse<R, H>
            {
                Body = SafeJsonConvert.DeserializeObject<R>(responseContent, client.DeserializationSettings),
                Request = httpRequest,
                Response = httpResponse,
            };
        }

        private static async Task<AzureOperationResponse<R, H>> HttpCall<T, R, H>(
            this T client,
            AzureRequest request)            
            where T : ServiceClient<T>, IAzureClient
            where R : class
            where H : class
        {
            var response = await client.HttpBeginCall<T, R, H>(request);
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

        public static Task<AzureOperationResponse<R, H>> DispatchCall<T, R, H>(
            this T client,
            AzureRequest request)
            where T : ServiceClient<T>, IAzureClient
            where R : class
            where H : class
            // => client.JsonRpcCall(request, tag);
            => client.HttpCall<T, R, H>(request);

        public static async Task<AzureOperationResponse<R, H>> Call<T, R, H, F>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse<R, H>> tag,
            Tag<F> tagF)
            where T : ServiceClient<T>, IAzureClient
            where R : class
            where H : class
            where F : class, R
        {
            var result = await client.DispatchCall<T, F, H>(request);
            return new AzureOperationResponse<R, H>
            {
                Headers = result.Headers,
                Body = result.Body,
                Request = result.Request,
                Response = result.Response
            };
        }

        public static async Task<AzureOperationResponse<R>> Call<T, R, F>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse<R>> tag,
            Tag<F> tagF)
            where T : ServiceClient<T>, IAzureClient
            where R : class
            where F : class, R
        {
            var result = await client.DispatchCall<T, F, object>(request);
            return new AzureOperationResponse<R>
            {
                Body = result.Body,
                Request = result.Request,
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
            var result = await client.DispatchCall<T, object, H>(request);
            return new AzureOperationHeaderResponse<H>
            {
                Headers = result.Headers,
                Request = result.Request,
                Response = result.Response,
            };
        }

        public static async Task<AzureOperationResponse> Call<T>(
            this T client,
            AzureRequest request,
            Tag<AzureOperationResponse> _)
            where T : ServiceClient<T>, IAzureClient
        {
            var result = await client.DispatchCall<T, object, object>(request);
            return new AzureOperationResponse()
            {
                Request = result.Request,
                Response = result.Response
            };
        }
    }
}
