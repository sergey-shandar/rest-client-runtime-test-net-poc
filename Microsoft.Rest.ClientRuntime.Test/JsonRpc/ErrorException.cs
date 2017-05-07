using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public class ErrorException : Exception
    {
        public string Method { get; }

        public string Json { get; }

        public Error Error { get; }

        public ErrorException(string method, string json, Error error):
            base($"JSONRPC error, method: {method}, code: {error.code}, message: '{error.message}', json: '{json}'")
        {
            Method = method;
            Json = json;
            Error = error;
        }
    }
}
