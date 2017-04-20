using System.Collections.Generic;

namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public sealed class Request : Message
    {
        public string method { get; set; }

        public Dictionary<string, object> @params { get; set; }
        
        public Request(string id, string method, Dictionary<string, object> @params): base(id)
        {
            this.method = method;
            this.@params = @params;
        }
    }
}
