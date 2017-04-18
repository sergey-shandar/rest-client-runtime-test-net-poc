using Microsoft.Rest.Azure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Rpc
{
    public abstract class AzureClientBase
    {
        private readonly IServer _Server;

        private readonly Credentials _Credentials;

        public AzureClientBase(IServer server, Credentials credentials)
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
            var result = await _Server.Call<C>("Server." + method, @params);
            return new AzureOperationResponse<I> { Body = result };
        }

        public async Task<AzureOperationResponse<I>> Call<I>(
            string method, Dictionary<string, object> @params)
            => await Call<I, I>(method, @params);

        public string SubscriptionId { get; set; }
    }
}
