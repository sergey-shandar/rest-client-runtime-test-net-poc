﻿using Microsoft.Rest.Azure;
using Microsoft.Rest.ClientRuntime.Test.JsonRpc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public static class ServiceClientEx
    {
        public static async Task<AzureOperationResponse<R>> Call<T, R>(this T client, AzureOperation<R> operation)
            where T : ServiceClient<T>, IAzureClient
        {
            var @params = new Dictionary<string, object>();
            @params["subscriptionId"] = operation.SubscriptionId;
            foreach (var p in operation.ParamList)
            {
                @params[p.Name] = p.Value;
            }
            var response = await HttpSendMock.RemoteServerCall<Response<R>>(operation.Title + "." + operation.Id, @params);
            return new AzureOperationResponse<R>
            {
                Body = response.result
            };
        }
    }
}
