using System.Collections.Generic;

namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public sealed class Request : Message
    {
        private static int s_RequestId;

        public string method { get; set; }

        public Dictionary<string, object> @params { get; set; }

        public static Request Create(string method, Dictionary<string, object> @params)
        {
            var request = new Request
            {
                jsonrpc = "2.0",
                id = s_RequestId,
                method = method,
                @params = @params,
            };

            ++s_RequestId;

            return request;
        }
    }
}
