using Microsoft.Rest.Azure;
using Microsoft.Rest.ClientRuntime.Test.Rpc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Azure
{
    public abstract class ClientBase
    {
        private readonly IServer _Server;

        private readonly Credentials _Credentials;

        public ClientBase(IServer server, Credentials credentials)
        {
            _Server = server;
            _Credentials = credentials;
        }

        public async Task<AzureOperationResponse<I>> Call<I, C>(
            string method, Dictionary<string, object> @params)
            where C : I
        {
            @params["subscriptionId"] = SubscriptionId;
            @params["__reserved"] = new Reserved
            {
                credentials = _Credentials
            };
            var result = await _Server.Call<Result<C>>("Server." + method, @params);
            return new AzureOperationResponse<I> { Body = result.respose };
        }

        public async Task<AzureOperationResponse<I>> Call<I>(
            string method, Dictionary<string, object> @params)
            => await Call<I, I>(method, @params);

        public string SubscriptionId { get; set; }
    }
}
