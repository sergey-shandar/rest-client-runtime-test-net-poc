using Microsoft.Rest.Azure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Rest.ClientRuntime.Test.Rpc
{
    public abstract class AzureClientBase
    {
        private readonly IServer _Server;

        public AzureClientBase(IServer server)
        {
            _Server = server;
        }

        protected async Task<AzureOperationResponse<I>> Call<I, C>(
            string method, Dictionary<string, object> @params)
            where C : I
        {
            @params["subscriptionId"] = SubscriptionId;
            var result = await _Server.Call<C>("Server." + method, @params);
            return new AzureOperationResponse<I> { Body = result };
        }

        protected async Task<AzureOperationResponse<I>> Call<I>(
            string method, Dictionary<string, object> @params)
            => await Call<I, I>(method, @params);

        public string SubscriptionId { get; set; }
    }
}
