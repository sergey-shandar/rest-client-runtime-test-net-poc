using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Rest.ClientRuntime.Test.JsonRpc
{
    public class ErrorException : Exception
    {
        public string Method { get; }

        public Error Error { get; }

        public ErrorException(string method, Error error):
            base($"JSONRPC error, method: {method}, code: {error.code}, message: {error.message}")
        {
            Method = method;
            Error = error;
        }
    }
}
