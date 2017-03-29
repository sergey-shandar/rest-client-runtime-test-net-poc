using System.Collections.Generic;

namespace Rest.ClientRuntime.Test.JsonRpc
{
    public sealed class Request : Message
    {
        public string method { get; set; }
        public Dictionary<string, object> @params { get; set; }
    }
}
